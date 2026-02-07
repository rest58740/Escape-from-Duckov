using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000313 RID: 787
	public class VerticalEntry : MonoBehaviour
	{
		// Token: 0x060019C2 RID: 6594 RVA: 0x0005E6F4 File Offset: 0x0005C8F4
		public void Setup(params string[] args)
		{
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0005E6F6 File Offset: 0x0005C8F6
		public void SetLayoutSpacing(float spacing)
		{
			this.layoutGroup.spacing = spacing;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0005E704 File Offset: 0x0005C904
		public void SetPreferredWidth(float width)
		{
			this.layoutElement.preferredWidth = width;
		}

		// Token: 0x040012C2 RID: 4802
		[SerializeField]
		private VerticalLayoutGroup layoutGroup;

		// Token: 0x040012C3 RID: 4803
		[SerializeField]
		private LayoutElement layoutElement;
	}
}
