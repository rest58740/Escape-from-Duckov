using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.UI.MainMenu
{
	// Token: 0x0200040A RID: 1034
	public class SaveSlotSelectionMenu : MonoBehaviour
	{
		// Token: 0x06002575 RID: 9589 RVA: 0x00082CE9 File Offset: 0x00080EE9
		private void OnEnable()
		{
			UIInputManager.OnCancel += this.OnCancel;
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x00082CFC File Offset: 0x00080EFC
		private void OnDisable()
		{
			UIInputManager.OnCancel -= this.OnCancel;
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00082D0F File Offset: 0x00080F0F
		private void OnCancel(UIInputEventData data)
		{
			data.Use();
			this.Finish();
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x00082D20 File Offset: 0x00080F20
		internal UniTask Execute()
		{
			SaveSlotSelectionMenu.<Execute>d__6 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<SaveSlotSelectionMenu.<Execute>d__6>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x00082D63 File Offset: 0x00080F63
		public void Finish()
		{
			this.finished = true;
		}

		// Token: 0x0400197F RID: 6527
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001980 RID: 6528
		[SerializeField]
		private GameObject oldSaveIndicator;

		// Token: 0x04001981 RID: 6529
		internal bool finished;
	}
}
