using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class StorageDockCountTMP : MonoBehaviour
{
	// Token: 0x06000F75 RID: 3957 RVA: 0x0003DF6E File Offset: 0x0003C16E
	private void Awake()
	{
		PlayerStorage.OnItemAddedToBuffer += this.OnItemAddedToBuffer;
		PlayerStorage.OnTakeBufferItem += this.OnTakeBufferItem;
		PlayerStorage.OnLoadingFinished += this.OnLoadingFinished;
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x0003DFA3 File Offset: 0x0003C1A3
	private void OnDestroy()
	{
		PlayerStorage.OnItemAddedToBuffer -= this.OnItemAddedToBuffer;
		PlayerStorage.OnTakeBufferItem -= this.OnTakeBufferItem;
		PlayerStorage.OnLoadingFinished -= this.OnLoadingFinished;
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x0003DFD8 File Offset: 0x0003C1D8
	private void OnLoadingFinished()
	{
		this.Refresh();
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x0003DFE0 File Offset: 0x0003C1E0
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x0003DFE8 File Offset: 0x0003C1E8
	private void OnTakeBufferItem()
	{
		this.Refresh();
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x0003DFF0 File Offset: 0x0003C1F0
	private void OnItemAddedToBuffer(Item item)
	{
		this.Refresh();
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x0003DFF8 File Offset: 0x0003C1F8
	private void Refresh()
	{
		int count = PlayerStorage.IncomingItemBuffer.Count;
		this.tmp.text = string.Format("{0}", count);
		if (this.setActiveFalseWhenCountIsZero)
		{
			base.gameObject.SetActive(count > 0);
		}
	}

	// Token: 0x04000CC7 RID: 3271
	[SerializeField]
	private TextMeshPro tmp;

	// Token: 0x04000CC8 RID: 3272
	[SerializeField]
	private bool setActiveFalseWhenCountIsZero;
}
