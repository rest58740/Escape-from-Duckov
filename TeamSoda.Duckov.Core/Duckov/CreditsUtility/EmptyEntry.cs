using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.CreditsUtility
{
	// Token: 0x0200030F RID: 783
	public class EmptyEntry : MonoBehaviour
	{
		// Token: 0x060019B8 RID: 6584 RVA: 0x0005E52C File Offset: 0x0005C72C
		public void Setup(params string[] args)
		{
			this.layoutElement.preferredWidth = this.defaultWidth;
			this.layoutElement.preferredHeight = this.defaultHeight;
			if (args == null)
			{
				return;
			}
			for (int i = 0; i < args.Length; i++)
			{
				if (i == 1)
				{
					this.TrySetWidth(args[i]);
				}
				if (i == 2)
				{
					this.TrySetHeight(args[i]);
				}
			}
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0005E588 File Offset: 0x0005C788
		private void TrySetWidth(string v)
		{
			float preferredWidth;
			if (!float.TryParse(v, out preferredWidth))
			{
				return;
			}
			this.layoutElement.preferredWidth = preferredWidth;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0005E5AC File Offset: 0x0005C7AC
		private void TrySetHeight(string v)
		{
			float preferredHeight;
			if (!float.TryParse(v, out preferredHeight))
			{
				return;
			}
			this.layoutElement.preferredHeight = preferredHeight;
		}

		// Token: 0x040012BB RID: 4795
		[SerializeField]
		private LayoutElement layoutElement;

		// Token: 0x040012BC RID: 4796
		[SerializeField]
		private float defaultWidth;

		// Token: 0x040012BD RID: 4797
		[SerializeField]
		private float defaultHeight;
	}
}
