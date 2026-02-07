using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000332 RID: 818
	public class BuildingContextMenu : MonoBehaviour
	{
		// Token: 0x06001B5C RID: 7004 RVA: 0x00063961 File Offset: 0x00061B61
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.recycleButton.onPointerClick += this.OnRecycleButtonClicked;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0006398B File Offset: 0x00061B8B
		private void OnRecycleButtonClicked(BuildingContextMenuEntry entry)
		{
			if (this.Target == null)
			{
				return;
			}
			BuildingManager.ReturnBuilding(this.Target.GUID, null).Forget<bool>();
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001B5E RID: 7006 RVA: 0x000639B2 File Offset: 0x00061BB2
		// (set) Token: 0x06001B5F RID: 7007 RVA: 0x000639BA File Offset: 0x00061BBA
		public Building Target { get; private set; }

		// Token: 0x06001B60 RID: 7008 RVA: 0x000639C3 File Offset: 0x00061BC3
		public void Setup(Building target)
		{
			this.Target = target;
			if (target == null)
			{
				this.Hide();
				return;
			}
			this.nameText.text = target.DisplayName;
			this.Show();
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x000639F4 File Offset: 0x00061BF4
		private void LateUpdate()
		{
			if (this.Target == null)
			{
				this.Hide();
				return;
			}
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(GameCamera.Instance.renderCamera, this.Target.transform.position);
			Vector2 vector2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, vector, null, ref vector2);
			this.rectTransform.localPosition = vector2;
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x00063A61 File Offset: 0x00061C61
		private void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x00063A6F File Offset: 0x00061C6F
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x040013A2 RID: 5026
		private RectTransform rectTransform;

		// Token: 0x040013A3 RID: 5027
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x040013A4 RID: 5028
		[SerializeField]
		private BuildingContextMenuEntry recycleButton;
	}
}
