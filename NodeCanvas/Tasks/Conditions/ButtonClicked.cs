using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200004F RID: 79
	[Category("UGUI")]
	public class ButtonClicked : ConditionTask
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000756F File Offset: 0x0000576F
		protected override string info
		{
			get
			{
				return string.Format("Button {0} Clicked", this.button.ToString());
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00007586 File Offset: 0x00005786
		protected override string OnInit()
		{
			this.button.value.onClick.AddListener(new UnityAction(this.OnClick));
			return null;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000075AA File Offset: 0x000057AA
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000075AD File Offset: 0x000057AD
		private void OnClick()
		{
			base.YieldReturn(true);
		}

		// Token: 0x040000F7 RID: 247
		[RequiredField]
		public BBParameter<Button> button;
	}
}
