using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FOW
{
	// Token: 0x02000014 RID: 20
	public class FogOfWarRenderFeature : ScriptableRendererFeature
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00007C44 File Offset: 0x00005E44
		public override void Create()
		{
			this.fowPass = new FogOfWarPass(base.name);
			this.fowPass.renderPassEvent = this.renderPassEvent;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00007C68 File Offset: 0x00005E68
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			this.fowPass.renderPassEvent = this.renderPassEvent;
			if (this.EnableNormals)
			{
				this.fowPass.ConfigureInput(ScriptableRenderPassInput.Normal);
			}
			else
			{
				this.fowPass.ConfigureInput(ScriptableRenderPassInput.Depth);
			}
			renderer.EnqueuePass(this.fowPass);
		}

		// Token: 0x040000F7 RID: 247
		public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

		// Token: 0x040000F8 RID: 248
		[Tooltip("This is required for 'Texture Color' fog, but can increase gpu usage on mobile.")]
		public bool EnableNormals;

		// Token: 0x040000F9 RID: 249
		private FogOfWarPass fowPass;
	}
}
