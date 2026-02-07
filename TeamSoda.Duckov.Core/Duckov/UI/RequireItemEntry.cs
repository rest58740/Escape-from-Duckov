using System;
using Duckov.PerkTrees;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003D9 RID: 985
	public class RequireItemEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x060023DC RID: 9180 RVA: 0x0007DDA7 File Offset: 0x0007BFA7
		public void NotifyPooled()
		{
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x0007DDA9 File Offset: 0x0007BFA9
		public void NotifyReleased()
		{
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x0007DDAC File Offset: 0x0007BFAC
		public void Setup(PerkRequirement.RequireItemEntry target)
		{
			int id = target.id;
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(id);
			this.icon.sprite = metaData.icon;
			string displayName = metaData.DisplayName;
			int itemCount = ItemUtilities.GetItemCount(id);
			this.text.text = string.Format(this.textFormat, displayName, target.amount, itemCount);
		}

		// Token: 0x0400186E RID: 6254
		[SerializeField]
		private Image icon;

		// Token: 0x0400186F RID: 6255
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001870 RID: 6256
		[SerializeField]
		private string textFormat = "{0} x{1}";
	}
}
