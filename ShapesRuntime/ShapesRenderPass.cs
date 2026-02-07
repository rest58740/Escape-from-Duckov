using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Shapes
{
	// Token: 0x0200003E RID: 62
	internal class ShapesRenderPass : ScriptableRenderPass
	{
		// Token: 0x06000C20 RID: 3104 RVA: 0x00018664 File Offset: 0x00016864
		public ShapesRenderPass Init(DrawCommand drawCommand)
		{
			this.drawCommand = drawCommand;
			base.renderPassEvent = drawCommand.camEvt;
			return this;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0001867A File Offset: 0x0001687A
		[Obsolete("This rendering path is for compatibility mode only (when Render Graph is disabled)", false)]
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.drawCommand.AppendToBuffer(this.cmdBuf);
			context.ExecuteCommandBuffer(this.cmdBuf);
			this.cmdBuf.Clear();
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000186A5 File Offset: 0x000168A5
		public override void FrameCleanup(CommandBuffer cmd)
		{
			DrawCommand.OnCommandRendered(this.drawCommand);
			this.drawCommand = null;
			ObjectPool<ShapesRenderPass>.Free(this);
		}

		// Token: 0x040001A7 RID: 423
		private DrawCommand drawCommand;

		// Token: 0x040001A8 RID: 424
		private readonly CommandBuffer cmdBuf = new CommandBuffer();
	}
}
