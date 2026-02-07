using System;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BD RID: 957
	public class TagsDisplayEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ITooltipsProvider
	{
		// Token: 0x06002237 RID: 8759 RVA: 0x00077D7D File Offset: 0x00075F7D
		public string GetTooltipsText()
		{
			if (this.target == null)
			{
				return "";
			}
			return this.target.Description;
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x00077D9E File Offset: 0x00075F9E
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.target == null)
			{
				return;
			}
			if (!this.target.ShowDescription)
			{
				return;
			}
			Tooltips.NotifyEnterTooltipsProvider(this);
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x00077DC3 File Offset: 0x00075FC3
		public void OnPointerExit(PointerEventData eventData)
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x00077DCB File Offset: 0x00075FCB
		private void OnDisable()
		{
			Tooltips.NotifyExitTooltipsProvider(this);
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x00077DD3 File Offset: 0x00075FD3
		public void Setup(Tag tag)
		{
			this.target = tag;
			this.background.color = tag.Color;
			this.text.text = tag.DisplayName;
		}

		// Token: 0x04001739 RID: 5945
		[SerializeField]
		private Image background;

		// Token: 0x0400173A RID: 5946
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x0400173B RID: 5947
		private Tag target;
	}
}
