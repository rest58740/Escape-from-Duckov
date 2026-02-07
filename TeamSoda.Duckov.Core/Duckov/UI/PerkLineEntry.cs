using System;
using Duckov.PerkTrees;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003D8 RID: 984
	public class PerkLineEntry : MonoBehaviour
	{
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x0007DD43 File Offset: 0x0007BF43
		public RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x0007DD65 File Offset: 0x0007BF65
		internal void Setup(PerkTreeView perkTreeView, PerkLevelLineNode cur)
		{
			this.target = cur;
			this.label.text = this.target.DisplayName;
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x0007DD84 File Offset: 0x0007BF84
		internal Vector2 GetLayoutPosition()
		{
			if (this.target == null)
			{
				return Vector2.zero;
			}
			return this.target.cachedPosition;
		}

		// Token: 0x0400186B RID: 6251
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x0400186C RID: 6252
		private RectTransform _rectTransform;

		// Token: 0x0400186D RID: 6253
		private PerkLevelLineNode target;
	}
}
