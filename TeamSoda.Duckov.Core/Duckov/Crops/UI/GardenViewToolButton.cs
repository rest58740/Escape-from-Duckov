using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Crops.UI
{
	// Token: 0x02000304 RID: 772
	public class GardenViewToolButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x0600194D RID: 6477 RVA: 0x0005D127 File Offset: 0x0005B327
		public void OnPointerClick(PointerEventData eventData)
		{
			this.master.SetTool(this.tool);
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0005D13A File Offset: 0x0005B33A
		private void Awake()
		{
			this.master.onToolChanged += this.OnToolChanged;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0005D153 File Offset: 0x0005B353
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0005D15B File Offset: 0x0005B35B
		private void Refresh()
		{
			this.indicator.SetActive(this.tool == this.master.Tool);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0005D17B File Offset: 0x0005B37B
		private void OnToolChanged()
		{
			this.Refresh();
		}

		// Token: 0x04001279 RID: 4729
		[SerializeField]
		private GardenView master;

		// Token: 0x0400127A RID: 4730
		[SerializeField]
		private GardenView.ToolType tool;

		// Token: 0x0400127B RID: 4731
		[SerializeField]
		private GameObject indicator;
	}
}
