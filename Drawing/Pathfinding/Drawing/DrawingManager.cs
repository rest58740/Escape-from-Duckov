using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pathfinding.Drawing
{
	// Token: 0x02000049 RID: 73
	[ExecuteAlways]
	[AddComponentMenu("")]
	public class DrawingManager : MonoBehaviour
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000ADFF File Offset: 0x00008FFF
		public static DrawingManager instance
		{
			get
			{
				if (DrawingManager._instance == null)
				{
					DrawingManager.Init();
				}
				return DrawingManager._instance;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000AE18 File Offset: 0x00009018
		public static void Init()
		{
			if (DrawingManager._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("RetainedGizmos")
			{
				hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset)
			};
			DrawingManager._instance = gameObject.AddComponent<DrawingManager>();
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			if (Application.isBatchMode)
			{
				DrawingManager.ignoreAllDrawing = true;
				DrawingManager.gizmoDrawers.Clear();
				DrawingManager.gizmoDrawerIndices.Clear();
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000AE7F File Offset: 0x0000907F
		private void RefreshRenderPipelineMode()
		{
			if (((RenderPipelineManager.currentPipeline != null) ? RenderPipelineManager.currentPipeline.GetType() : null) == typeof(UniversalRenderPipeline))
			{
				this.detectedRenderPipeline = DetectedRenderPipeline.URP;
				return;
			}
			this.detectedRenderPipeline = DetectedRenderPipeline.BuiltInOrCustom;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000AEB8 File Offset: 0x000090B8
		private void OnEnable()
		{
			if (DrawingManager._instance == null)
			{
				DrawingManager._instance = this;
			}
			if (DrawingManager._instance != this)
			{
				return;
			}
			this.actuallyEnabled = true;
			if (this.gizmos == null)
			{
				this.gizmos = new DrawingData();
			}
			this.gizmos.frameRedrawScope = new RedrawScope(this.gizmos);
			Draw.builder = this.gizmos.GetBuiltInBuilder(false);
			Draw.ingame_builder = this.gizmos.GetBuiltInBuilder(true);
			this.commandBuffer = new CommandBuffer();
			this.commandBuffer.name = "ALINE Gizmos";
			this.detectedRenderPipeline = DetectedRenderPipeline.BuiltInOrCustom;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.PostRender));
			RenderPipelineManager.beginContextRendering += this.BeginContextRendering;
			RenderPipelineManager.beginCameraRendering += this.BeginCameraRendering;
			RenderPipelineManager.endCameraRendering += this.EndCameraRendering;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000AFAD File Offset: 0x000091AD
		private void BeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			this.RefreshRenderPipelineMode();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000AFAD File Offset: 0x000091AD
		private void BeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
		{
			this.RefreshRenderPipelineMode();
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000AFB8 File Offset: 0x000091B8
		private void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (this.detectedRenderPipeline == DetectedRenderPipeline.URP)
			{
				UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
				if (universalAdditionalCameraData != null)
				{
					ScriptableRenderer scriptableRenderer = universalAdditionalCameraData.scriptableRenderer;
					if (this.renderPassFeature == null)
					{
						this.renderPassFeature = ScriptableObject.CreateInstance<AlineURPRenderPassFeature>();
					}
					this.renderPassFeature.AddRenderPasses(scriptableRenderer);
				}
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000B00C File Offset: 0x0000920C
		private void OnDisable()
		{
			if (!this.actuallyEnabled)
			{
				return;
			}
			this.actuallyEnabled = false;
			this.commandBuffer.Dispose();
			this.commandBuffer = null;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.PostRender));
			RenderPipelineManager.beginContextRendering -= this.BeginContextRendering;
			RenderPipelineManager.beginCameraRendering -= this.BeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= this.EndCameraRendering;
			if (this.gizmos != null)
			{
				Draw.builder.DiscardAndDisposeInternal();
				Draw.ingame_builder.DiscardAndDisposeInternal();
				this.gizmos.ClearData();
			}
			if (this.renderPassFeature != null)
			{
				UnityEngine.Object.DestroyImmediate(this.renderPassFeature);
				this.renderPassFeature = null;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000B0D5 File Offset: 0x000092D5
		private void OnEditorUpdate()
		{
			this.framePassed = true;
			this.CleanupIfNoCameraRendered();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000B0E4 File Offset: 0x000092E4
		private void Update()
		{
			if (this.actuallyEnabled)
			{
				this.CleanupIfNoCameraRendered();
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000B0F4 File Offset: 0x000092F4
		private void CleanupIfNoCameraRendered()
		{
			if (Time.frameCount > this.lastFrameCount + 1)
			{
				this.CheckFrameTicking();
				this.gizmos.PostRenderCleanup();
			}
			if (Time.realtimeSinceStartup - this.lastFrameTime > 10f)
			{
				Draw.builder.DiscardAndDisposeInternal();
				Draw.ingame_builder.DiscardAndDisposeInternal();
				Draw.builder = this.gizmos.GetBuiltInBuilder(false);
				Draw.ingame_builder = this.gizmos.GetBuiltInBuilder(true);
				this.lastFrameTime = Time.realtimeSinceStartup;
				DrawingManager.RemoveDestroyedGizmoDrawers();
			}
			if (this.lastFilterFrame - Time.frameCount > 5)
			{
				this.lastFilterFrame = Time.frameCount;
				DrawingManager.RemoveDestroyedGizmoDrawers();
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B19C File Offset: 0x0000939C
		internal void ExecuteCustomRenderPass(ScriptableRenderContext context, Camera camera)
		{
			this.commandBuffer.Clear();
			this.SubmitFrame(camera, new DrawingData.CommandBufferWrapper
			{
				cmd = this.commandBuffer
			}, true);
			context.ExecuteCommandBuffer(this.commandBuffer);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000B1DF File Offset: 0x000093DF
		internal void ExecuteCustomRenderGraphPass(DrawingData.CommandBufferWrapper cmd, Camera camera)
		{
			this.SubmitFrame(camera, cmd, true);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000B1EA File Offset: 0x000093EA
		private void EndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (this.detectedRenderPipeline == DetectedRenderPipeline.BuiltInOrCustom)
			{
				this.ExecuteCustomRenderPass(context, camera);
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B1FC File Offset: 0x000093FC
		private void PostRender(Camera camera)
		{
			this.commandBuffer.Clear();
			this.SubmitFrame(camera, new DrawingData.CommandBufferWrapper
			{
				cmd = this.commandBuffer
			}, false);
			Graphics.ExecuteCommandBuffer(this.commandBuffer);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000B240 File Offset: 0x00009440
		private void CheckFrameTicking()
		{
			if (Time.frameCount != this.lastFrameCount)
			{
				this.framePassed = true;
				this.lastFrameCount = Time.frameCount;
				this.lastFrameTime = Time.realtimeSinceStartup;
				this.previousFrameRedrawScope = this.gizmos.frameRedrawScope;
				this.gizmos.frameRedrawScope = new RedrawScope(this.gizmos);
				Draw.builder.DisposeInternal();
				Draw.ingame_builder.DisposeInternal();
				Draw.builder = this.gizmos.GetBuiltInBuilder(false);
				Draw.ingame_builder = this.gizmos.GetBuiltInBuilder(true);
			}
			else if (this.framePassed && Application.isPlaying)
			{
				this.previousFrameRedrawScope.Draw();
			}
			if (this.framePassed)
			{
				this.gizmos.TickFramePreRender();
				this.framePassed = false;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000B30C File Offset: 0x0000950C
		internal void SubmitFrame(Camera camera, DrawingData.CommandBufferWrapper cmd, bool usingRenderPipeline)
		{
			bool flag = false;
			bool allowCameraDefault = DrawingManager.allowRenderToRenderTextures || DrawingManager.drawToAllCameras || camera.targetTexture == null || flag;
			this.CheckFrameTicking();
			this.Submit(camera, cmd, usingRenderPipeline, allowCameraDefault);
			this.gizmos.PostRenderCleanup();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000B356 File Offset: 0x00009556
		private bool ShouldDrawGizmos(UnityEngine.Object obj)
		{
			return true;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000B35C File Offset: 0x0000955C
		private static void RemoveDestroyedGizmoDrawers()
		{
			for (int i = 0; i < DrawingManager.gizmoDrawers.Count; i++)
			{
				DrawingManager.GizmoDrawerGroup gizmoDrawerGroup = DrawingManager.gizmoDrawers[i];
				int num = 0;
				for (int j = 0; j < gizmoDrawerGroup.drawers.Count; j++)
				{
					IDrawGizmos drawGizmos = gizmoDrawerGroup.drawers[j];
					if (drawGizmos as MonoBehaviour)
					{
						gizmoDrawerGroup.drawers[num] = drawGizmos;
						num++;
					}
				}
				gizmoDrawerGroup.drawers.RemoveRange(num, gizmoDrawerGroup.drawers.Count - num);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000B3EC File Offset: 0x000095EC
		private void Submit(Camera camera, DrawingData.CommandBufferWrapper cmd, bool usingRenderPipeline, bool allowCameraDefault)
		{
			bool allowGizmos = false;
			Draw.builder.DisposeInternal();
			Draw.ingame_builder.DisposeInternal();
			this.gizmos.Render(camera, allowGizmos, cmd, allowCameraDefault);
			Draw.builder = this.gizmos.GetBuiltInBuilder(false);
			Draw.ingame_builder = this.gizmos.GetBuiltInBuilder(true);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000B444 File Offset: 0x00009644
		public static void Register(IDrawGizmos item)
		{
			if (DrawingManager.ignoreAllDrawing)
			{
				return;
			}
			Type type = item.GetType();
			int num;
			if (!DrawingManager.gizmoDrawerIndices.TryGetValue(type, out num))
			{
				BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				MethodInfo methodInfo;
				if ((methodInfo = type.GetMethod("DrawGizmos", bindingAttr)) == null)
				{
					methodInfo = (type.GetMethod("Pathfinding.Drawing.IDrawGizmos.DrawGizmos", bindingAttr) ?? type.GetMethod("Drawing.IDrawGizmos.DrawGizmos", bindingAttr));
				}
				MethodInfo methodInfo2 = methodInfo;
				if (methodInfo2 == null)
				{
					throw new Exception("Could not find the DrawGizmos method in type " + type.Name);
				}
				if (methodInfo2.DeclaringType != typeof(MonoBehaviourGizmos))
				{
					num = (DrawingManager.gizmoDrawerIndices[type] = DrawingManager.gizmoDrawers.Count);
					DrawingManager.gizmoDrawers.Add(new DrawingManager.GizmoDrawerGroup
					{
						type = type,
						enabled = true,
						drawers = new List<IDrawGizmos>(),
						profilerMarker = new ProfilerMarker(ProfilerCategory.Render, "Gizmos for " + type.Name)
					});
				}
				else
				{
					num = (DrawingManager.gizmoDrawerIndices[type] = -1);
				}
			}
			if (num == -1)
			{
				return;
			}
			DrawingManager.gizmoDrawers[num].drawers.Add(item);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000B56D File Offset: 0x0000976D
		public static CommandBuilder GetBuilder(bool renderInGame = false)
		{
			return DrawingManager.instance.gizmos.GetBuilder(renderInGame);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000B57F File Offset: 0x0000977F
		public static CommandBuilder GetBuilder(RedrawScope redrawScope, bool renderInGame = false)
		{
			return DrawingManager.instance.gizmos.GetBuilder(redrawScope, renderInGame);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000B592 File Offset: 0x00009792
		public static CommandBuilder GetBuilder(DrawingData.Hasher hasher, RedrawScope redrawScope = default(RedrawScope), bool renderInGame = false)
		{
			return DrawingManager.instance.gizmos.GetBuilder(hasher, redrawScope, renderInGame);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000B5A6 File Offset: 0x000097A6
		public static bool TryDrawHasher(DrawingData.Hasher hasher, RedrawScope redrawScope = default(RedrawScope))
		{
			return DrawingManager.instance.gizmos.Draw(hasher, redrawScope);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000B5BC File Offset: 0x000097BC
		public static RedrawScope GetRedrawScope(GameObject associatedGameObject = null)
		{
			RedrawScope result = new RedrawScope(DrawingManager.instance.gizmos);
			result.DrawUntilDispose(associatedGameObject);
			return result;
		}

		// Token: 0x04000108 RID: 264
		public DrawingData gizmos;

		// Token: 0x04000109 RID: 265
		private static List<DrawingManager.GizmoDrawerGroup> gizmoDrawers = new List<DrawingManager.GizmoDrawerGroup>();

		// Token: 0x0400010A RID: 266
		private static Dictionary<Type, int> gizmoDrawerIndices = new Dictionary<Type, int>();

		// Token: 0x0400010B RID: 267
		private static bool ignoreAllDrawing;

		// Token: 0x0400010C RID: 268
		private static DrawingManager _instance;

		// Token: 0x0400010D RID: 269
		private bool framePassed;

		// Token: 0x0400010E RID: 270
		private int lastFrameCount = int.MinValue;

		// Token: 0x0400010F RID: 271
		private float lastFrameTime = float.PositiveInfinity;

		// Token: 0x04000110 RID: 272
		private int lastFilterFrame;

		// Token: 0x04000111 RID: 273
		[SerializeField]
		private bool actuallyEnabled;

		// Token: 0x04000112 RID: 274
		private RedrawScope previousFrameRedrawScope;

		// Token: 0x04000113 RID: 275
		public static bool allowRenderToRenderTextures = false;

		// Token: 0x04000114 RID: 276
		public static bool drawToAllCameras = false;

		// Token: 0x04000115 RID: 277
		public static float lineWidthMultiplier = 1f;

		// Token: 0x04000116 RID: 278
		private CommandBuffer commandBuffer;

		// Token: 0x04000117 RID: 279
		[NonSerialized]
		private DetectedRenderPipeline detectedRenderPipeline;

		// Token: 0x04000118 RID: 280
		private HashSet<ScriptableRenderer> scriptableRenderersWithPass = new HashSet<ScriptableRenderer>();

		// Token: 0x04000119 RID: 281
		private AlineURPRenderPassFeature renderPassFeature;

		// Token: 0x0400011A RID: 282
		private static readonly ProfilerMarker MarkerALINE = new ProfilerMarker("ALINE");

		// Token: 0x0400011B RID: 283
		private static readonly ProfilerMarker MarkerCommandBuffer = new ProfilerMarker("Executing command buffer");

		// Token: 0x0400011C RID: 284
		private static readonly ProfilerMarker MarkerFrameTick = new ProfilerMarker("Frame Tick");

		// Token: 0x0400011D RID: 285
		private static readonly ProfilerMarker MarkerFilterDestroyedObjects = new ProfilerMarker("Filter destroyed objects");

		// Token: 0x0400011E RID: 286
		internal static readonly ProfilerMarker MarkerRefreshSelectionCache = new ProfilerMarker("Refresh Selection Cache");

		// Token: 0x0400011F RID: 287
		private static readonly ProfilerMarker MarkerGizmosAllowed = new ProfilerMarker("GizmosAllowed");

		// Token: 0x04000120 RID: 288
		private static readonly ProfilerMarker MarkerDrawGizmos = new ProfilerMarker("DrawGizmos");

		// Token: 0x04000121 RID: 289
		private static readonly ProfilerMarker MarkerSubmitGizmos = new ProfilerMarker("Submit Gizmos");

		// Token: 0x04000122 RID: 290
		private const float NO_DRAWING_TIMEOUT_SECS = 10f;

		// Token: 0x0200004A RID: 74
		private struct GizmoDrawerGroup
		{
			// Token: 0x04000123 RID: 291
			public Type type;

			// Token: 0x04000124 RID: 292
			public ProfilerMarker profilerMarker;

			// Token: 0x04000125 RID: 293
			public List<IDrawGizmos> drawers;

			// Token: 0x04000126 RID: 294
			public bool enabled;
		}
	}
}
