using System;
using Duckov.Utilities;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B2 RID: 946
	public class ItemVariableEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06002164 RID: 8548 RVA: 0x00075169 File Offset: 0x00073369
		public void NotifyPooled()
		{
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0007516B File Offset: 0x0007336B
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0007517A File Offset: 0x0007337A
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x00075182 File Offset: 0x00073382
		internal void Setup(CustomData target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0007519D File Offset: 0x0007339D
		private void Refresh()
		{
			this.displayName.text = this.target.DisplayName;
			this.value.text = this.target.GetValueDisplayString("");
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000751D0 File Offset: 0x000733D0
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetData += this.OnTargetSetData;
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x000751F2 File Offset: 0x000733F2
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetData -= this.OnTargetSetData;
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x00075214 File Offset: 0x00073414
		private void OnTargetSetData(CustomData data)
		{
			this.Refresh();
		}

		// Token: 0x040016C7 RID: 5831
		private CustomData target;

		// Token: 0x040016C8 RID: 5832
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x040016C9 RID: 5833
		[SerializeField]
		private TextMeshProUGUI value;
	}
}
