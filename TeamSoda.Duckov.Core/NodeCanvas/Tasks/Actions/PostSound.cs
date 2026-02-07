using System;
using Duckov;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200042C RID: 1068
	public class PostSound : ActionTask<AICharacterController>
	{
		// Token: 0x060026A8 RID: 9896 RVA: 0x00085BA6 File Offset: 0x00083DA6
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060026A9 RID: 9897 RVA: 0x00085BA9 File Offset: 0x00083DA9
		protected override string info
		{
			get
			{
				return string.Format("Post Sound: {0} ", this.voiceSound.ToString());
			}
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x00085BC8 File Offset: 0x00083DC8
		protected override void OnExecute()
		{
			if (base.agent && base.agent.CharacterMainControl)
			{
				if (!base.agent.canTalk)
				{
					base.EndAction(true);
					return;
				}
				GameObject gameObject = base.agent.CharacterMainControl.gameObject;
				switch (this.voiceSound)
				{
				case PostSound.VoiceSounds.normal:
					AudioManager.PostQuak("normal", base.agent.CharacterMainControl.AudioVoiceType, gameObject);
					break;
				case PostSound.VoiceSounds.surprise:
					AudioManager.PostQuak("surprise", base.agent.CharacterMainControl.AudioVoiceType, gameObject);
					break;
				case PostSound.VoiceSounds.death:
					AudioManager.PostQuak("death", base.agent.CharacterMainControl.AudioVoiceType, gameObject);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			base.EndAction(true);
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x00085CA2 File Offset: 0x00083EA2
		protected override void OnStop()
		{
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x00085CA4 File Offset: 0x00083EA4
		protected override void OnPause()
		{
		}

		// Token: 0x04001A53 RID: 6739
		public PostSound.VoiceSounds voiceSound;

		// Token: 0x0200069B RID: 1691
		public enum VoiceSounds
		{
			// Token: 0x0400243C RID: 9276
			normal,
			// Token: 0x0400243D RID: 9277
			surprise,
			// Token: 0x0400243E RID: 9278
			death
		}
	}
}
