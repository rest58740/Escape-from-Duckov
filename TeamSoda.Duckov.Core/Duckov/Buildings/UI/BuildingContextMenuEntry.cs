using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000333 RID: 819
	public class BuildingContextMenuEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001B65 RID: 7013 RVA: 0x00063A85 File Offset: 0x00061C85
		private void OnEnable()
		{
			this.text.text = LocalizationManager.ToPlainText(this.textKey);
		}

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06001B66 RID: 7014 RVA: 0x00063AA0 File Offset: 0x00061CA0
		// (remove) Token: 0x06001B67 RID: 7015 RVA: 0x00063AD8 File Offset: 0x00061CD8
		public event Action<BuildingContextMenuEntry> onPointerClick;

		// Token: 0x06001B68 RID: 7016 RVA: 0x00063B0D File Offset: 0x00061D0D
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<BuildingContextMenuEntry> action = this.onPointerClick;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x040013A6 RID: 5030
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040013A7 RID: 5031
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey;
	}
}
