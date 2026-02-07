using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001CC RID: 460
public class PackedMapData : ScriptableObject, IMiniMapDataProvider
{
	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00039F54 File Offset: 0x00038154
	public Sprite CombinedSprite
	{
		get
		{
			return this.combinedSprite;
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00039F5C File Offset: 0x0003815C
	public float PixelSize
	{
		get
		{
			return this.pixelSize;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000DEB RID: 3563 RVA: 0x00039F64 File Offset: 0x00038164
	public Vector3 CombinedCenter
	{
		get
		{
			return this.combinedCenter;
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000DEC RID: 3564 RVA: 0x00039F6C File Offset: 0x0003816C
	public List<IMiniMapEntry> Maps
	{
		get
		{
			return this.maps.ToList<IMiniMapEntry>();
		}
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x00039F7C File Offset: 0x0003817C
	internal void Setup(IMiniMapDataProvider origin)
	{
		this.combinedSprite = origin.CombinedSprite;
		this.pixelSize = origin.PixelSize;
		this.combinedCenter = origin.CombinedCenter;
		this.maps.Clear();
		foreach (IMiniMapEntry miniMapEntry in origin.Maps)
		{
			PackedMapData.Entry item = new PackedMapData.Entry(miniMapEntry.Sprite, miniMapEntry.PixelSize, miniMapEntry.Offset, miniMapEntry.SceneID, miniMapEntry.Hide, miniMapEntry.NoSignal);
			this.maps.Add(item);
		}
	}

	// Token: 0x04000BE1 RID: 3041
	[SerializeField]
	private Sprite combinedSprite;

	// Token: 0x04000BE2 RID: 3042
	[SerializeField]
	private float pixelSize;

	// Token: 0x04000BE3 RID: 3043
	[SerializeField]
	private Vector3 combinedCenter;

	// Token: 0x04000BE4 RID: 3044
	[SerializeField]
	private List<PackedMapData.Entry> maps = new List<PackedMapData.Entry>();

	// Token: 0x020004F5 RID: 1269
	[Serializable]
	public class Entry : IMiniMapEntry
	{
		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x0600284D RID: 10317 RVA: 0x00092D15 File Offset: 0x00090F15
		public Sprite Sprite
		{
			get
			{
				return this.sprite;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x00092D1D File Offset: 0x00090F1D
		public float PixelSize
		{
			get
			{
				return this.pixelSize;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x0600284F RID: 10319 RVA: 0x00092D25 File Offset: 0x00090F25
		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002850 RID: 10320 RVA: 0x00092D2D File Offset: 0x00090F2D
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002851 RID: 10321 RVA: 0x00092D35 File Offset: 0x00090F35
		public bool Hide
		{
			get
			{
				return this.hide;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002852 RID: 10322 RVA: 0x00092D3D File Offset: 0x00090F3D
		public bool NoSignal
		{
			get
			{
				return this.noSignal;
			}
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x00092D45 File Offset: 0x00090F45
		public Entry()
		{
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x00092D4D File Offset: 0x00090F4D
		public Entry(Sprite sprite, float pixelSize, Vector2 offset, string sceneID, bool hide, bool noSignal)
		{
			this.sprite = sprite;
			this.pixelSize = pixelSize;
			this.offset = offset;
			this.sceneID = sceneID;
			this.hide = hide;
			this.noSignal = noSignal;
		}

		// Token: 0x04001DD4 RID: 7636
		[SerializeField]
		private Sprite sprite;

		// Token: 0x04001DD5 RID: 7637
		[SerializeField]
		private float pixelSize;

		// Token: 0x04001DD6 RID: 7638
		[SerializeField]
		private Vector2 offset;

		// Token: 0x04001DD7 RID: 7639
		[SerializeField]
		private string sceneID;

		// Token: 0x04001DD8 RID: 7640
		[SerializeField]
		private bool hide;

		// Token: 0x04001DD9 RID: 7641
		[SerializeField]
		private bool noSignal;
	}
}
