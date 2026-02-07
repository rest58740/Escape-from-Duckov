using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Drawing
{
	// Token: 0x02000048 RID: 72
	[ExecuteAlways]
	[AddComponentMenu("")]
	public class DrawingManager : MonoBehaviour
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000CAEB File Offset: 0x0000ACEB
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

		// Token: 0x06000367 RID: 871 RVA: 0x0000CB04 File Offset: 0x0000AD04
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
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000CB4A File Offset: 0x0000AD4A
		private void RefreshRenderPipelineMode()
		{
			if (((RenderPipelineManager.currentPipeline != null) ? RenderPipelineManager.currentPipeline.GetType() : null) == typeof(UniversalRenderPipeline))
			{
				this.detectedRenderPipeline = DetectedRenderPipeline.URP;
				return;
			}
			this.detectedRenderPipeline = DetectedRenderPipeline.BuiltInOrCustom;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000CB80 File Offset: 0x0000AD80
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
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.PostRender));
			RenderPipelineManager.beginFrameRendering += this.BeginFrameRendering;
			RenderPipelineManager.beginCameraRendering += this.BeginCameraRendering;
			RenderPipelineManager.endCameraRendering += this.EndCameraRendering;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000CC6E File Offset: 0x0000AE6E
		private void BeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			this.RefreshRenderPipelineMode();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000CC6E File Offset: 0x0000AE6E
		private void BeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
		{
			this.RefreshRenderPipelineMode();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000CC78 File Offset: 0x0000AE78
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

		// Token: 0x0600036D RID: 877 RVA: 0x0000CCCC File Offset: 0x0000AECC
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
			RenderPipelineManager.beginFrameRendering -= this.BeginFrameRendering;
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

		// Token: 0x0600036E RID: 878 RVA: 0x0000CD95 File Offset: 0x0000AF95
		private void OnEditorUpdate()
		{
			this.framePassed = true;
			this.CleanupIfNoCameraRendered();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000CDA4 File Offset: 0x0000AFA4
		private void Update()
		{
			if (this.actuallyEnabled)
			{
				this.CleanupIfNoCameraRendered();
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000CDB4 File Offset: 0x0000AFB4
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

		// Token: 0x06000371 RID: 881 RVA: 0x0000CE5C File Offset: 0x0000B05C
		internal void ExecuteCustomRenderPass(ScriptableRenderContext context, Camera camera)
		{
			this.commandBuffer.Clear();
			this.SubmitFrame(camera, new DrawingData.CommandBufferWrapper
			{
				cmd = this.commandBuffer
			}, true);
			context.ExecuteCommandBuffer(this.commandBuffer);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000CE9F File Offset: 0x0000B09F
		internal void ExecuteCustomRenderGraphPass(DrawingData.CommandBufferWrapper cmd, Camera camera)
		{
			this.SubmitFrame(camera, cmd, true);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000CEAA File Offset: 0x0000B0AA
		private void EndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (this.detectedRenderPipeline == DetectedRenderPipeline.BuiltInOrCustom)
			{
				this.ExecuteCustomRenderPass(context, camera);
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000CEBC File Offset: 0x0000B0BC
		private void PostRender(Camera camera)
		{
			this.commandBuffer.Clear();
			this.SubmitFrame(camera, new DrawingData.CommandBufferWrapper
			{
				cmd = this.commandBuffer
			}, false);
			Graphics.ExecuteCommandBuffer(this.commandBuffer);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000CF00 File Offset: 0x0000B100
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

		// Token: 0x06000376 RID: 886 RVA: 0x0000CFCC File Offset: 0x0000B1CC
		internal void SubmitFrame(Camera camera, DrawingData.CommandBufferWrapper cmd, bool usingRenderPipeline)
		{
			bool flag = false;
			bool allowCameraDefault = DrawingManager.allowRenderToRenderTextures || DrawingManager.drawToAllCameras || camera.targetTexture == null || flag;
			this.CheckFrameTicking();
			this.Submit(camera, cmd, usingRenderPipeline, allowCameraDefault);
			this.gizmos.PostRenderCleanup();
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000D016 File Offset: 0x0000B216
		private bool ShouldDrawGizmos(UnityEngine.Object obj)
		{
			return true;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000D01C File Offset: 0x0000B21C
		private static void RemoveDestroyedGizmoDrawers()
		{
			int num = 0;
			for (int i = 0; i < DrawingManager.gizmoDrawers.Count; i++)
			{
				IDrawGizmos drawGizmos = DrawingManager.gizmoDrawers[i];
				if (drawGizmos as MonoBehaviour)
				{
					DrawingManager.gizmoDrawers[num] = drawGizmos;
					num++;
				}
			}
			DrawingManager.gizmoDrawers.RemoveRange(num, DrawingManager.gizmoDrawers.Count - num);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000D080 File Offset: 0x0000B280
		private void Submit(Camera camera, DrawingData.CommandBufferWrapper cmd, bool usingRenderPipeline, bool allowCameraDefault)
		{
			bool allowGizmos = false;
			Draw.builder.DisposeInternal();
			Draw.ingame_builder.DisposeInternal();
			this.gizmos.Render(camera, allowGizmos, cmd, allowCameraDefault);
			Draw.builder = this.gizmos.GetBuiltInBuilder(false);
			Draw.ingame_builder = this.gizmos.GetBuiltInBuilder(true);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000D0D8 File Offset: 0x0000B2D8
		public static void Register(IDrawGizmos item)
		{
			Type type = item.GetType();
			bool flag;
			if (!DrawingManager.gizmoDrawerTypes.TryGetValue(type, out flag))
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
				flag = (methodInfo2.DeclaringType != typeof(MonoBehaviourGizmos));
				DrawingManager.gizmoDrawerTypes[type] = flag;
			}
			if (!flag)
			{
				return;
			}
			DrawingManager.gizmoDrawers.Add(item);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000D179 File Offset: 0x0000B379
		public static CommandBuilder GetBuilder(bool renderInGame = false)
		{
			return DrawingManager.instance.gizmos.GetBuilder(renderInGame);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000D18B File Offset: 0x0000B38B
		public static CommandBuilder GetBuilder(RedrawScope redrawScope, bool renderInGame = false)
		{
			return DrawingManager.instance.gizmos.GetBuilder(redrawScope, renderInGame);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000D19E File Offset: 0x0000B39E
		public static CommandBuilder GetBuilder(DrawingData.Hasher hasher, RedrawScope redrawScope = default(RedrawScope), bool renderInGame = false)
		{
			return DrawingManager.instance.gizmos.GetBuilder(hasher, redrawScope, renderInGame);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000D1B4 File Offset: 0x0000B3B4
		public static RedrawScope GetRedrawScope()
		{
			RedrawScope result = new RedrawScope(DrawingManager.instance.gizmos);
			result.DrawUntilDispose();
			return result;
		}

		// Token: 0x04000101 RID: 257
		public DrawingData gizmos;

		// Token: 0x04000102 RID: 258
		private static List<IDrawGizmos> gizmoDrawers = new List<IDrawGizmos>();

		// Token: 0x04000103 RID: 259
		private static Dictionary<Type, bool> gizmoDrawerTypes = new Dictionary<Type, bool>();

		// Token: 0x04000104 RID: 260
		private static DrawingManager _instance;

		// Token: 0x04000105 RID: 261
		private bool framePassed;

		// Token: 0x04000106 RID: 262
		private int lastFrameCount = int.MinValue;

		// Token: 0x04000107 RID: 263
		private float lastFrameTime = float.PositiveInfinity;

		// Token: 0x04000108 RID: 264
		private int lastFilterFrame;

		// Token: 0x04000109 RID: 265
		[SerializeField]
		private bool actuallyEnabled;

		// Token: 0x0400010A RID: 266
		private RedrawScope previousFrameRedrawScope;

		// Token: 0x0400010B RID: 267
		public static bool allowRenderToRenderTextures = false;

		// Token: 0x0400010C RID: 268
		public static bool drawToAllCameras = false;

		// Token: 0x0400010D RID: 269
		public static float lineWidthMultiplier = 1f;

		// Token: 0x0400010E RID: 270
		private CommandBuffer commandBuffer;

		// Token: 0x0400010F RID: 271
		[NonSerialized]
		private DetectedRenderPipeline detectedRenderPipeline;

		// Token: 0x04000110 RID: 272
		private HashSet<ScriptableRenderer> scriptableRenderersWithPass = new HashSet<ScriptableRenderer>();

		// Token: 0x04000111 RID: 273
		private AlineURPRenderPassFeature renderPassFeature;

		// Token: 0x04000112 RID: 274
		private static readonly ProfilerMarker MarkerALINE = new ProfilerMarker("ALINE");

		// Token: 0x04000113 RID: 275
		private static readonly ProfilerMarker MarkerCommandBuffer = new ProfilerMarker("Executing command buffer");

		// Token: 0x04000114 RID: 276
		private static readonly ProfilerMarker MarkerFrameTick = new ProfilerMarker("Frame Tick");

		// Token: 0x04000115 RID: 277
		private static readonly ProfilerMarker MarkerFilterDestroyedObjects = new ProfilerMarker("Filter destroyed objects");

		// Token: 0x04000116 RID: 278
		internal static readonly ProfilerMarker MarkerRefreshSelectionCache = new ProfilerMarker("Refresh Selection Cache");

		// Token: 0x04000117 RID: 279
		private static readonly ProfilerMarker MarkerGizmosAllowed = new ProfilerMarker("GizmosAllowed");

		// Token: 0x04000118 RID: 280
		private static readonly ProfilerMarker MarkerDrawGizmos = new ProfilerMarker("DrawGizmos");

		// Token: 0x04000119 RID: 281
		private static readonly ProfilerMarker MarkerSubmitGizmos = new ProfilerMarker("Submit Gizmos");

		// Token: 0x0400011A RID: 282
		private const float NO_DRAWING_TIMEOUT_SECS = 10f;

		// Token: 0x0400011B RID: 283
		private readonly Dictionary<Type, bool> typeToGizmosEnabled = new Dictionary<Type, bool>();
	}
}
