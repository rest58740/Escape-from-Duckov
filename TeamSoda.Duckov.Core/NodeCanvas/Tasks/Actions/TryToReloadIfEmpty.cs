using System;
using Duckov;
using NodeCanvas.Framework;
using SodaCraft.Localizations;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000438 RID: 1080
	public class TryToReloadIfEmpty : ActionTask<AICharacterController>
	{
		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060026F2 RID: 9970 RVA: 0x00086ACA File Offset: 0x00084CCA
		public string SoundKey
		{
			get
			{
				return "normal";
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x00086AD1 File Offset: 0x00084CD1
		private string Key
		{
			get
			{
				return this.poptextWhileReloading;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x00086AD9 File Offset: 0x00084CD9
		private string DisplayText
		{
			get
			{
				return LocalizationManager.ToPlainText(this.poptextWhileReloading);
			}
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x00086AE6 File Offset: 0x00084CE6
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x00086AEC File Offset: 0x00084CEC
		protected override void OnExecute()
		{
			ItemAgent_Gun gun = base.agent.CharacterMainControl.GetGun();
			if (gun == null)
			{
				base.EndAction(true);
				return;
			}
			if (gun.BulletCount <= 0)
			{
				base.agent.CharacterMainControl.TryToReload(null);
				if (!this.isFirstTime)
				{
					if (!base.agent.CharacterMainControl.Health.Hidden && this.poptextWhileReloading != string.Empty && base.agent.canTalk)
					{
						base.agent.CharacterMainControl.PopText(LocalizationManager.ToPlainText(this.poptextWhileReloading), -1f);
					}
					if (this.postSound && this.SoundKey != string.Empty && base.agent && base.agent.CharacterMainControl)
					{
						AudioManager.PostQuak(this.SoundKey, base.agent.CharacterMainControl.AudioVoiceType, base.agent.CharacterMainControl.gameObject);
					}
				}
			}
			this.isFirstTime = false;
			base.EndAction(true);
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x00086C0D File Offset: 0x00084E0D
		protected override void OnUpdate()
		{
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x00086C0F File Offset: 0x00084E0F
		protected override void OnStop()
		{
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x00086C11 File Offset: 0x00084E11
		protected override void OnPause()
		{
		}

		// Token: 0x04001A85 RID: 6789
		public string poptextWhileReloading = "PopText_Reloading";

		// Token: 0x04001A86 RID: 6790
		public bool postSound;

		// Token: 0x04001A87 RID: 6791
		private bool isFirstTime = true;
	}
}
