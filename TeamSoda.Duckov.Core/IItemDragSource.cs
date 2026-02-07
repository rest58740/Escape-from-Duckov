using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine.EventSystems;

// Token: 0x02000166 RID: 358
public interface IItemDragSource : IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
{
	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06000B1C RID: 2844 RVA: 0x00030834 File Offset: 0x0002EA34
	// (remove) Token: 0x06000B1D RID: 2845 RVA: 0x00030868 File Offset: 0x0002EA68
	public static event Action<Item> OnStartDragItem;

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06000B1E RID: 2846 RVA: 0x0003089C File Offset: 0x0002EA9C
	// (remove) Token: 0x06000B1F RID: 2847 RVA: 0x000308D0 File Offset: 0x0002EAD0
	public static event Action<Item> OnEndDragItem;

	// Token: 0x06000B20 RID: 2848
	bool IsEditable();

	// Token: 0x06000B21 RID: 2849
	Item GetItem();

	// Token: 0x06000B22 RID: 2850 RVA: 0x00030904 File Offset: 0x0002EB04
	void OnBeginDrag(PointerEventData eventData)
	{
		if (!this.IsEditable())
		{
			return;
		}
		if (eventData.button != null)
		{
			return;
		}
		Item item = this.GetItem();
		Action<Item> onStartDragItem = IItemDragSource.OnStartDragItem;
		if (onStartDragItem != null)
		{
			onStartDragItem(item);
		}
		if (item == null)
		{
			return;
		}
		ItemUIUtilities.NotifyPutItem(item, true);
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0003094C File Offset: 0x0002EB4C
	void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.button != null)
		{
			return;
		}
		Item item = this.GetItem();
		Action<Item> onEndDragItem = IItemDragSource.OnEndDragItem;
		if (onEndDragItem == null)
		{
			return;
		}
		onEndDragItem(item);
	}
}
