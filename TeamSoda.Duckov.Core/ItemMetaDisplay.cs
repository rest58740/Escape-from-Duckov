using System;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001FD RID: 509
public class ItemMetaDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IItemMetaDataProvider
{
	// Token: 0x06000F16 RID: 3862 RVA: 0x0003D05D File Offset: 0x0003B25D
	public ItemMetaData GetMetaData()
	{
		return this.data;
	}

	// Token: 0x14000071 RID: 113
	// (add) Token: 0x06000F17 RID: 3863 RVA: 0x0003D068 File Offset: 0x0003B268
	// (remove) Token: 0x06000F18 RID: 3864 RVA: 0x0003D09C File Offset: 0x0003B29C
	public static event Action<ItemMetaDisplay> OnMouseEnter;

	// Token: 0x14000072 RID: 114
	// (add) Token: 0x06000F19 RID: 3865 RVA: 0x0003D0D0 File Offset: 0x0003B2D0
	// (remove) Token: 0x06000F1A RID: 3866 RVA: 0x0003D104 File Offset: 0x0003B304
	public static event Action<ItemMetaDisplay> OnMouseExit;

	// Token: 0x06000F1B RID: 3867 RVA: 0x0003D137 File Offset: 0x0003B337
	public void OnPointerEnter(PointerEventData eventData)
	{
		Action<ItemMetaDisplay> onMouseEnter = ItemMetaDisplay.OnMouseEnter;
		if (onMouseEnter == null)
		{
			return;
		}
		onMouseEnter(this);
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x0003D149 File Offset: 0x0003B349
	public void OnPointerExit(PointerEventData eventData)
	{
		Action<ItemMetaDisplay> onMouseExit = ItemMetaDisplay.OnMouseExit;
		if (onMouseExit == null)
		{
			return;
		}
		onMouseExit(this);
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x0003D15C File Offset: 0x0003B35C
	public void Setup(int typeID)
	{
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(typeID);
		this.Setup(metaData);
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x0003D177 File Offset: 0x0003B377
	public void Setup(ItemMetaData data)
	{
		this.data = data;
		this.icon.sprite = data.icon;
		GameplayDataSettings.UIStyle.ApplyDisplayQualityShadow(data.displayQuality, this.displayQualityShadow);
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0003D1A7 File Offset: 0x0003B3A7
	internal void Setup(object rootTypeID)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04000C8F RID: 3215
	[SerializeField]
	private Image icon;

	// Token: 0x04000C90 RID: 3216
	[SerializeField]
	private TrueShadow displayQualityShadow;

	// Token: 0x04000C91 RID: 3217
	private ItemMetaData data;
}
