using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000089 RID: 137
	[Category("Camera")]
	public class FadeIn : ActionTask
	{
		// Token: 0x06000266 RID: 614 RVA: 0x00009AC6 File Offset: 0x00007CC6
		protected override void OnExecute()
		{
			CameraFader.current.FadeIn(this.fadeTime);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009AD8 File Offset: 0x00007CD8
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.fadeTime)
			{
				base.EndAction();
			}
		}

		// Token: 0x04000191 RID: 401
		public float fadeTime = 1f;
	}
}
