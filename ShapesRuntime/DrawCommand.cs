using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Shapes
{
	// Token: 0x0200001B RID: 27
	public class DrawCommand : IDisposable
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00015139 File Offset: 0x00013339
		internal static bool IsAddingDrawCommandsToBuffer
		{
			get
			{
				return DrawCommand.drawCommandWriteNestLevel > 0;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00015143 File Offset: 0x00013343
		internal static DrawCommand CurrentWritingCommandBuffer
		{
			get
			{
				return DrawCommand.cBuffersWriting.Peek();
			}
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0001514F File Offset: 0x0001334F
		static DrawCommand()
		{
			SceneManager.sceneUnloaded += delegate(Scene scene)
			{
				DrawCommand.FlushNullCameras();
			};
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0001517C File Offset: 0x0001337C
		public static void ClearAllCommands()
		{
			DrawCommand.FlushNullCameras();
			foreach (List<DrawCommand> list in DrawCommand.cBuffersRendering.Values)
			{
				list.ForEach(delegate(DrawCommand cmd)
				{
					cmd.Clear();
				});
				list.Clear();
			}
			DrawCommand.cBuffersRendering.Clear();
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00015204 File Offset: 0x00013404
		public static void FlushNullCameras()
		{
			foreach (KeyValuePair<Camera, List<DrawCommand>> keyValuePair in (from kvp in DrawCommand.cBuffersRendering
			where kvp.Key == null
			select kvp).ToList<KeyValuePair<Camera, List<DrawCommand>>>())
			{
				keyValuePair.Value.ForEach(delegate(DrawCommand cmd)
				{
					cmd.Clear();
				});
				DrawCommand.cBuffersRendering.Remove(keyValuePair.Key);
			}
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x000152B8 File Offset: 0x000134B8
		private static void RegisterCommand(DrawCommand cmd)
		{
			List<DrawCommand> list;
			if (!DrawCommand.cBuffersRendering.TryGetValue(cmd.cam, ref list))
			{
				DrawCommand.cBuffersRendering.Add(cmd.cam, list = new List<DrawCommand>());
			}
			list.Add(cmd);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x000152F8 File Offset: 0x000134F8
		internal static void OnCommandRendered(DrawCommand cmd)
		{
			cmd.hasRendered = true;
			List<DrawCommand> list;
			if (DrawCommand.cBuffersRendering.TryGetValue(cmd.cam, ref list))
			{
				cmd.Clear();
				list.Remove(cmd);
				return;
			}
			Debug.LogError(string.Format("Tried to remove unlisted draw command {0}", cmd.id));
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0001534C File Offset: 0x0001354C
		internal DrawCommand Initialize(Camera cam, RenderPassEvent cameraEvent = RenderPassEvent.BeforeRenderingPostProcessing)
		{
			this.cam = cam;
			this.id = DrawCommand.bufferID++;
			this.hasValidCamera = (cam != null);
			if (!this.hasValidCamera)
			{
				Debug.LogWarning("null camera passed into DrawCommand, nothing will be drawn");
			}
			this.camEvt = cameraEvent;
			DrawCommand.cBuffersWriting.Push(this);
			DrawCommand.drawCommandWriteNestLevel++;
			this.pushPopState = ShapesConfig.Instance.pushPopStateInDrawCommands;
			if (this.pushPopState)
			{
				Draw.Push();
			}
			return this;
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x000153D0 File Offset: 0x000135D0
		internal void AppendToBuffer(CommandBuffer cmd)
		{
			foreach (ShapeDrawCall shapeDrawCall in this.drawCalls)
			{
				shapeDrawCall.AddToCommandBuffer(cmd);
			}
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00015424 File Offset: 0x00013624
		private void Clear()
		{
			this.CleanupCachedAssetsAndMeshes();
			this.hasRendered = false;
			for (int i = 0; i < this.drawCalls.Count; i++)
			{
				this.drawCalls[i].Cleanup();
			}
			this.drawCalls.Clear();
			ObjectPool<DrawCommand>.Free(this);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0001547C File Offset: 0x0001367C
		private void CleanupCachedAssetsAndMeshes()
		{
			foreach (int num in this.cachedTextIds)
			{
				ShapesObjPool<TextMeshProShapes, ShapesTextPool>.Instance.ReleaseElement(num);
			}
			this.cachedTextIds.Clear();
			foreach (Object obj in this.cachedAssets)
			{
				obj.DestroyBranched();
			}
			this.cachedAssets.Clear();
			foreach (DisposableMesh disposableMesh in this.cachedMeshes)
			{
				disposableMesh.ReleaseFromCommand(this);
			}
			this.cachedMeshes.Clear();
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00015574 File Offset: 0x00013774
		public void Dispose()
		{
			if (IMDrawer.metaMpbPrevious != null && IMDrawer.metaMpbPrevious.HasContent)
			{
				this.drawCalls.Add(IMDrawer.metaMpbPrevious.ExtractDrawCall());
			}
			if (this.hasValidCamera)
			{
				DrawCommand.RegisterCommand(this);
			}
			DrawCommand.drawCommandWriteNestLevel--;
			DrawCommand.cBuffersWriting.Pop();
			if (this.pushPopState)
			{
				Draw.Pop();
			}
		}

		// Token: 0x040000CB RID: 203
		private static int bufferID;

		// Token: 0x040000CC RID: 204
		private static int drawCommandWriteNestLevel;

		// Token: 0x040000CD RID: 205
		private static Stack<DrawCommand> cBuffersWriting = new Stack<DrawCommand>();

		// Token: 0x040000CE RID: 206
		internal static Dictionary<Camera, List<DrawCommand>> cBuffersRendering = new Dictionary<Camera, List<DrawCommand>>();

		// Token: 0x040000CF RID: 207
		private bool hasValidCamera;

		// Token: 0x040000D0 RID: 208
		internal bool hasRendered;

		// Token: 0x040000D1 RID: 209
		internal int id;

		// Token: 0x040000D2 RID: 210
		private bool pushPopState;

		// Token: 0x040000D3 RID: 211
		private Camera cam;

		// Token: 0x040000D4 RID: 212
		internal readonly List<int> cachedTextIds = new List<int>();

		// Token: 0x040000D5 RID: 213
		internal readonly List<Object> cachedAssets = new List<Object>();

		// Token: 0x040000D6 RID: 214
		internal readonly List<DisposableMesh> cachedMeshes = new List<DisposableMesh>();

		// Token: 0x040000D7 RID: 215
		internal readonly List<ShapeDrawCall> drawCalls = new List<ShapeDrawCall>();

		// Token: 0x040000D8 RID: 216
		public RenderPassEvent camEvt;
	}
}
