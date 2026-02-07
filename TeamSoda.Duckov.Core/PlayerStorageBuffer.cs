using System;
using System.Collections.Generic;
using ItemStatsSystem.Data;
using Saves;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class PlayerStorageBuffer : MonoBehaviour
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x0600089A RID: 2202 RVA: 0x00026D53 File Offset: 0x00024F53
	// (set) Token: 0x0600089B RID: 2203 RVA: 0x00026D5A File Offset: 0x00024F5A
	public static PlayerStorageBuffer Instance { get; private set; }

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x0600089C RID: 2204 RVA: 0x00026D62 File Offset: 0x00024F62
	public static List<ItemTreeData> Buffer
	{
		get
		{
			return PlayerStorageBuffer.incomingItemBuffer;
		}
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00026D69 File Offset: 0x00024F69
	private void Awake()
	{
		PlayerStorageBuffer.Instance = this;
		PlayerStorageBuffer.LoadBuffer();
		SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x00026D87 File Offset: 0x00024F87
	private void OnCollectSaveData()
	{
		PlayerStorageBuffer.SaveBuffer();
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00026D90 File Offset: 0x00024F90
	public static void SaveBuffer()
	{
		List<ItemTreeData> list = new List<ItemTreeData>();
		foreach (ItemTreeData itemTreeData in PlayerStorageBuffer.incomingItemBuffer)
		{
			if (itemTreeData != null)
			{
				list.Add(itemTreeData);
			}
		}
		SavesSystem.Save<List<ItemTreeData>>("PlayerStorage_Buffer", list);
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00026DF8 File Offset: 0x00024FF8
	public static void LoadBuffer()
	{
		PlayerStorageBuffer.incomingItemBuffer.Clear();
		List<ItemTreeData> list = SavesSystem.Load<List<ItemTreeData>>("PlayerStorage_Buffer");
		if (list != null)
		{
			if (list.Count <= 0)
			{
				Debug.Log("tree data is empty");
			}
			using (List<ItemTreeData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemTreeData item = enumerator.Current;
					PlayerStorageBuffer.incomingItemBuffer.Add(item);
				}
				return;
			}
		}
		Debug.Log("Tree Data is null");
	}

	// Token: 0x040007DD RID: 2013
	private const string bufferSaveKey = "PlayerStorage_Buffer";

	// Token: 0x040007DE RID: 2014
	private static List<ItemTreeData> incomingItemBuffer = new List<ItemTreeData>();
}
