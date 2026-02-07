using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Modding.UI
{
	// Token: 0x0200027B RID: 635
	public class ModChangedWarning : MonoBehaviour
	{
		// Token: 0x06001421 RID: 5153 RVA: 0x0004B962 File Offset: 0x00049B62
		private void Awake()
		{
			this.continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0004B99C File Offset: 0x00049B9C
		private void OnContinueButtonClicked()
		{
			this.continueBtnClicked = true;
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0004B9A5 File Offset: 0x00049BA5
		private void OnCancelButtonClicked()
		{
			this.cancelBtnClicked = true;
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0004B9B0 File Offset: 0x00049BB0
		public UniTask<bool> Check()
		{
			ModChangedWarning.<Check>d__10 <Check>d__;
			<Check>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Check>d__.<>4__this = this;
			<Check>d__.<>1__state = -1;
			<Check>d__.<>t__builder.Start<ModChangedWarning.<Check>d__10>(ref <Check>d__);
			return <Check>d__.<>t__builder.Task;
		}

		// Token: 0x04000EF3 RID: 3827
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000EF4 RID: 3828
		[SerializeField]
		private Button continueButton;

		// Token: 0x04000EF5 RID: 3829
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04000EF6 RID: 3830
		[SerializeField]
		private TextMeshProUGUI contentText;

		// Token: 0x04000EF7 RID: 3831
		private bool continueBtnClicked;

		// Token: 0x04000EF8 RID: 3832
		private bool cancelBtnClicked;

		// Token: 0x04000EF9 RID: 3833
		private bool checking;
	}
}
