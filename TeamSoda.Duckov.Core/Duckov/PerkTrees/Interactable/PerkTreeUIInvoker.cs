using System;
using Duckov.UI;

namespace Duckov.PerkTrees.Interactable
{
	// Token: 0x02000269 RID: 617
	public class PerkTreeUIInvoker : InteractableBase
	{
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x0600136A RID: 4970 RVA: 0x00049954 File Offset: 0x00047B54
		protected override bool ShowUnityEvents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00049957 File Offset: 0x00047B57
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			PerkTreeView.Show(PerkTreeManager.GetPerkTree(this.perkTreeID));
			base.StopInteract();
		}

		// Token: 0x04000EA9 RID: 3753
		public string perkTreeID;
	}
}
