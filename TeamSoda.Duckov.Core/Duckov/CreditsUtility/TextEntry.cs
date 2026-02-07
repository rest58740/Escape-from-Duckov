using System;
using TMPro;
using UnityEngine;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000312 RID: 786
	public class TextEntry : MonoBehaviour
	{
		// Token: 0x060019C0 RID: 6592 RVA: 0x0005E684 File Offset: 0x0005C884
		internal void Setup(string text, Color color, int size = -1, bool bold = false)
		{
			this.text.text = text;
			if (size < 0)
			{
				size = this.defaultSize;
			}
			this.text.color = color;
			this.text.fontSize = (float)size;
			this.text.fontStyle = ((this.text.fontStyle & -2) | (bold ? 1 : 0));
		}

		// Token: 0x040012C0 RID: 4800
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040012C1 RID: 4801
		public int defaultSize = 26;
	}
}
