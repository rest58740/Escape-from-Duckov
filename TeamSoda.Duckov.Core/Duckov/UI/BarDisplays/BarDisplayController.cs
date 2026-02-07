using System;
using UnityEngine;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003E9 RID: 1001
	public class BarDisplayController : MonoBehaviour
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x0600247D RID: 9341 RVA: 0x000802E3 File Offset: 0x0007E4E3
		protected virtual float Current
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x000802EA File Offset: 0x0007E4EA
		protected virtual float Max
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000802F4 File Offset: 0x0007E4F4
		protected void Refresh()
		{
			float current = this.Current;
			float max = this.Max;
			this.bar.SetValue(current, max, "0.#", 0f);
		}

		// Token: 0x040018D4 RID: 6356
		[SerializeField]
		private BarDisplay bar;
	}
}
