using System;
using Duckov.Buffs;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200039E RID: 926
	public class ExtraBuffViewEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002031 RID: 8241 RVA: 0x000714F8 File Offset: 0x0006F6F8
		// (set) Token: 0x06002032 RID: 8242 RVA: 0x00071500 File Offset: 0x0006F700
		public ExtraBuffView Master { get; private set; }

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x00071509 File Offset: 0x0006F709
		// (set) Token: 0x06002034 RID: 8244 RVA: 0x00071511 File Offset: 0x0006F711
		public Buff target { get; private set; }

		// Token: 0x06002035 RID: 8245 RVA: 0x0007151C File Offset: 0x0006F71C
		internal void Setup(ExtraBuffView master, Buff buff)
		{
			this.Master = master;
			this.target = buff;
			if (buff == null)
			{
				this.SetupEmpty();
				Debug.LogError("[Extra Buff View] No buff feeded in the view entry.");
				return;
			}
			this.icon.sprite = buff.Icon;
			this.displayName.text = buff.DisplayName;
			this.layersCount.text = ((buff.CurrentLayers > 1) ? string.Format("{0}", buff.CurrentLayers) : "");
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000715A3 File Offset: 0x0006F7A3
		private void SetupEmpty()
		{
			this.icon.sprite = GameplayDataSettings.UIStyle.FallbackItemIcon;
			this.displayName.text = "?";
			this.layersCount.text = "";
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000715DA File Offset: 0x0006F7DA
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.Master.NotifyPointerEnter(this);
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000715E8 File Offset: 0x0006F7E8
		public void OnPointerExit(PointerEventData eventData)
		{
			this.Master.NotifyPointerExit(this);
		}

		// Token: 0x04001602 RID: 5634
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001603 RID: 5635
		[SerializeField]
		private Image icon;

		// Token: 0x04001604 RID: 5636
		[SerializeField]
		private TextMeshProUGUI layersCount;
	}
}
