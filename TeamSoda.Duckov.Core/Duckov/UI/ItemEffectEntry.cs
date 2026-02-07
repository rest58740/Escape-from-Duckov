using System;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003AF RID: 943
	public class ItemEffectEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x0600214C RID: 8524 RVA: 0x00074F09 File Offset: 0x00073109
		public void NotifyPooled()
		{
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x00074F0B File Offset: 0x0007310B
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x00074F1A File Offset: 0x0007311A
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00074F22 File Offset: 0x00073122
		public void Setup(Effect target)
		{
			this.target = target;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x00074F37 File Offset: 0x00073137
		private void Refresh()
		{
			this.text.text = this.target.GetDisplayString();
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x00074F4F File Offset: 0x0007314F
		private void RegisterEvents()
		{
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00074F51 File Offset: 0x00073151
		private void UnregisterEvents()
		{
		}

		// Token: 0x040016BC RID: 5820
		private Effect target;

		// Token: 0x040016BD RID: 5821
		[SerializeField]
		private TextMeshProUGUI text;
	}
}
