using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000064 RID: 100
	[Category("Audio")]
	public class PlayAudioAtPosition : ActionTask<Transform>
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00008994 File Offset: 0x00006B94
		protected override string info
		{
			get
			{
				return "PlayAudio " + this.audioClip.ToString();
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000089AB File Offset: 0x00006BAB
		protected override void OnExecute()
		{
			AudioSource.PlayClipAtPoint(this.audioClip.value, base.agent.position, this.volume);
			if (!this.waitActionFinish)
			{
				base.EndAction();
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000089DC File Offset: 0x00006BDC
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.audioClip.value.length)
			{
				base.EndAction();
			}
		}

		// Token: 0x04000136 RID: 310
		[RequiredField]
		public BBParameter<AudioClip> audioClip;

		// Token: 0x04000137 RID: 311
		[SliderField(0, 1)]
		public float volume = 1f;

		// Token: 0x04000138 RID: 312
		public bool waitActionFinish;
	}
}
