using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200008A RID: 138
	[Category("Camera")]
	public class FadeOut : ActionTask
	{
		// Token: 0x06000269 RID: 617 RVA: 0x00009B01 File Offset: 0x00007D01
		protected override void OnExecute()
		{
			CameraFader.current.FadeOut(this.fadeTime);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00009B13 File Offset: 0x00007D13
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.fadeTime)
			{
				base.EndAction();
			}
		}

		// Token: 0x04000192 RID: 402
		public float fadeTime = 1f;
	}
}
