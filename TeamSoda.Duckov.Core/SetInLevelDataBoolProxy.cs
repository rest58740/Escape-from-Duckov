using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class SetInLevelDataBoolProxy : MonoBehaviour
{
	// Token: 0x060005D1 RID: 1489 RVA: 0x0001A2F1 File Offset: 0x000184F1
	public void SetToTarget()
	{
		this.SetTo(this.targetValue);
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0001A300 File Offset: 0x00018500
	public void SetTo(bool target)
	{
		if (this.keyString == "")
		{
			return;
		}
		if (!this.keyInited)
		{
			this.InitKey();
		}
		if (MultiSceneCore.Instance)
		{
			MultiSceneCore.Instance.inLevelData[this.keyHash] = target;
		}
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0001A355 File Offset: 0x00018555
	private void InitKey()
	{
		this.keyHash = this.keyString.GetHashCode();
		this.keyInited = true;
	}

	// Token: 0x0400055A RID: 1370
	public bool targetValue = true;

	// Token: 0x0400055B RID: 1371
	public string keyString = "";

	// Token: 0x0400055C RID: 1372
	private int keyHash;

	// Token: 0x0400055D RID: 1373
	private bool keyInited;
}
