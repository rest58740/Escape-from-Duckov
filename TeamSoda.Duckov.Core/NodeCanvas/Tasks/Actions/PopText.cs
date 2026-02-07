using System;
using NodeCanvas.Framework;
using SodaCraft.Localizations;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200042B RID: 1067
	public class PopText : ActionTask<AICharacterController>
	{
		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060026A0 RID: 9888 RVA: 0x00085B02 File Offset: 0x00083D02
		private string Key
		{
			get
			{
				return this.content.value;
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060026A1 RID: 9889 RVA: 0x00085B0F File Offset: 0x00083D0F
		private string DisplayText
		{
			get
			{
				return LocalizationManager.ToPlainText(this.Key);
			}
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x00085B1C File Offset: 0x00083D1C
		protected override string OnInit()
		{
			return null;
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060026A3 RID: 9891 RVA: 0x00085B1F File Offset: 0x00083D1F
		protected override string info
		{
			get
			{
				return string.Format("Pop:'{0}'", this.DisplayText);
			}
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x00085B34 File Offset: 0x00083D34
		protected override void OnExecute()
		{
			if (this.checkHide && base.agent.CharacterMainControl.Hidden)
			{
				base.EndAction(true);
				return;
			}
			if (!base.agent.canTalk)
			{
				base.EndAction(true);
				return;
			}
			base.agent.CharacterMainControl.PopText(this.DisplayText, -1f);
			base.EndAction(true);
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x00085B9A File Offset: 0x00083D9A
		protected override void OnStop()
		{
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x00085B9C File Offset: 0x00083D9C
		protected override void OnPause()
		{
		}

		// Token: 0x04001A51 RID: 6737
		public BBParameter<string> content;

		// Token: 0x04001A52 RID: 6738
		public bool checkHide;
	}
}
