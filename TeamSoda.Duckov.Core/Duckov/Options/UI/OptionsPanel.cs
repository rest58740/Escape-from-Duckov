using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Options.UI
{
	// Token: 0x0200026B RID: 619
	public class OptionsPanel : UIPanel, ISingleSelectionMenu<OptionsPanel_TabButton>
	{
		// Token: 0x06001377 RID: 4983 RVA: 0x00049B74 File Offset: 0x00047D74
		private void Start()
		{
			this.Setup();
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00049B7C File Offset: 0x00047D7C
		private void Setup()
		{
			foreach (OptionsPanel_TabButton optionsPanel_TabButton in this.tabButtons)
			{
				optionsPanel_TabButton.onClicked = (Action<OptionsPanel_TabButton, PointerEventData>)Delegate.Combine(optionsPanel_TabButton.onClicked, new Action<OptionsPanel_TabButton, PointerEventData>(this.OnTabButtonClicked));
			}
			if (this.selection == null)
			{
				this.selection = this.tabButtons[0];
			}
			this.SetSelection(this.selection);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00049C18 File Offset: 0x00047E18
		private void OnTabButtonClicked(OptionsPanel_TabButton button, PointerEventData data)
		{
			data.Use();
			this.SetSelection(button);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00049C28 File Offset: 0x00047E28
		protected override void OnOpen()
		{
			base.OnOpen();
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00049C30 File Offset: 0x00047E30
		public OptionsPanel_TabButton GetSelection()
		{
			return this.selection;
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00049C38 File Offset: 0x00047E38
		public bool SetSelection(OptionsPanel_TabButton selection)
		{
			this.selection = selection;
			foreach (OptionsPanel_TabButton optionsPanel_TabButton in this.tabButtons)
			{
				optionsPanel_TabButton.NotifySelectionChanged(this, selection);
			}
			return true;
		}

		// Token: 0x04000EAD RID: 3757
		[SerializeField]
		private List<OptionsPanel_TabButton> tabButtons;

		// Token: 0x04000EAE RID: 3758
		private OptionsPanel_TabButton selection;
	}
}
