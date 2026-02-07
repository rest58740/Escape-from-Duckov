using System;
using System.Collections.Generic;
using Duckov.MiniMaps;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CB RID: 459
public class MapMarkerSettingsPanel : MonoBehaviour
{
	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x00039CFF File Offset: 0x00037EFF
	private List<Sprite> Icons
	{
		get
		{
			return MapMarkerManager.Icons;
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x00039D08 File Offset: 0x00037F08
	private PrefabPool<MapMarkerPanelButton> IconBtnPool
	{
		get
		{
			if (this._iconBtnPool == null)
			{
				this._iconBtnPool = new PrefabPool<MapMarkerPanelButton>(this.iconBtnTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._iconBtnPool;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x00039D44 File Offset: 0x00037F44
	private PrefabPool<MapMarkerPanelButton> ColorBtnPool
	{
		get
		{
			if (this._colorBtnPool == null)
			{
				this._colorBtnPool = new PrefabPool<MapMarkerPanelButton>(this.colorBtnTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._colorBtnPool;
		}
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x00039D80 File Offset: 0x00037F80
	private void OnEnable()
	{
		this.Setup();
		MapMarkerManager.OnColorChanged = (Action<Color>)Delegate.Combine(MapMarkerManager.OnColorChanged, new Action<Color>(this.OnColorChanged));
		MapMarkerManager.OnIconChanged = (Action<int>)Delegate.Combine(MapMarkerManager.OnIconChanged, new Action<int>(this.OnIconChanged));
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x00039DD4 File Offset: 0x00037FD4
	private void OnDisable()
	{
		MapMarkerManager.OnColorChanged = (Action<Color>)Delegate.Remove(MapMarkerManager.OnColorChanged, new Action<Color>(this.OnColorChanged));
		MapMarkerManager.OnIconChanged = (Action<int>)Delegate.Remove(MapMarkerManager.OnIconChanged, new Action<int>(this.OnIconChanged));
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00039E21 File Offset: 0x00038021
	private void OnIconChanged(int obj)
	{
		this.Setup();
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x00039E29 File Offset: 0x00038029
	private void OnColorChanged(Color color)
	{
		this.Setup();
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x00039E34 File Offset: 0x00038034
	private void Setup()
	{
		if (MapMarkerManager.Instance == null)
		{
			return;
		}
		this.IconBtnPool.ReleaseAll();
		this.ColorBtnPool.ReleaseAll();
		Color[] array = this.colors;
		for (int i = 0; i < array.Length; i++)
		{
			Color cur = array[i];
			MapMarkerPanelButton mapMarkerPanelButton = this.ColorBtnPool.Get(null);
			mapMarkerPanelButton.Image.color = cur;
			mapMarkerPanelButton.Setup(delegate
			{
				MapMarkerManager.SelectColor(cur);
			}, cur == MapMarkerManager.SelectedColor);
		}
		for (int j = 0; j < this.Icons.Count; j++)
		{
			Sprite sprite = this.Icons[j];
			if (!(sprite == null))
			{
				MapMarkerPanelButton mapMarkerPanelButton2 = this.IconBtnPool.Get(null);
				Image image = mapMarkerPanelButton2.Image;
				image.sprite = sprite;
				image.color = MapMarkerManager.SelectedColor;
				int index = j;
				mapMarkerPanelButton2.Setup(delegate
				{
					MapMarkerManager.SelectIcon(index);
				}, index == MapMarkerManager.SelectedIconIndex);
			}
		}
	}

	// Token: 0x04000BDC RID: 3036
	[SerializeField]
	private Color[] colors;

	// Token: 0x04000BDD RID: 3037
	[SerializeField]
	private MapMarkerPanelButton iconBtnTemplate;

	// Token: 0x04000BDE RID: 3038
	[SerializeField]
	private MapMarkerPanelButton colorBtnTemplate;

	// Token: 0x04000BDF RID: 3039
	private PrefabPool<MapMarkerPanelButton> _iconBtnPool;

	// Token: 0x04000BE0 RID: 3040
	private PrefabPool<MapMarkerPanelButton> _colorBtnPool;
}
