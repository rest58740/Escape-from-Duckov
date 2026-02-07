using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000167 RID: 359
public class ItemDraggingPointerDisplay : MonoBehaviour
{
	// Token: 0x06000B24 RID: 2852 RVA: 0x0003097C File Offset: 0x0002EB7C
	private void Awake()
	{
		this.rectTransform = (base.transform as RectTransform);
		this.parentRectTransform = (base.transform.parent as RectTransform);
		IItemDragSource.OnStartDragItem += this.OnStartDragItem;
		IItemDragSource.OnEndDragItem += this.OnEndDragItem;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x000309DE File Offset: 0x0002EBDE
	private void OnDestroy()
	{
		IItemDragSource.OnStartDragItem -= this.OnStartDragItem;
		IItemDragSource.OnEndDragItem -= this.OnEndDragItem;
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x00030A02 File Offset: 0x0002EC02
	private void Update()
	{
		this.RefreshPosition();
		if (Mouse.current.leftButton.wasReleasedThisFrame)
		{
			this.OnEndDragItem(null);
		}
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x00030A24 File Offset: 0x0002EC24
	private unsafe void RefreshPosition()
	{
		Vector2 vector;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform.parent as RectTransform, *Pointer.current.position.value, null, ref vector);
		this.rectTransform.localPosition = vector;
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x00030A6F File Offset: 0x0002EC6F
	private void OnStartDragItem(Item item)
	{
		this.target = item;
		if (this.target == null)
		{
			return;
		}
		this.display.Setup(this.target);
		this.RefreshPosition();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x00030AAA File Offset: 0x0002ECAA
	private void OnEndDragItem(Item item)
	{
		this.target = null;
		base.gameObject.SetActive(false);
	}

	// Token: 0x040009BD RID: 2493
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x040009BE RID: 2494
	[SerializeField]
	private RectTransform parentRectTransform;

	// Token: 0x040009BF RID: 2495
	[SerializeField]
	private ItemDisplay display;

	// Token: 0x040009C0 RID: 2496
	private Item target;
}
