using System;
using UnityEngine;

// Token: 0x0200010D RID: 269
[Serializable]
public struct EvacuationInfo
{
	// Token: 0x06000968 RID: 2408 RVA: 0x0002A51E File Offset: 0x0002871E
	public EvacuationInfo(string subsceneID, Vector3 position)
	{
		this.subsceneID = subsceneID;
		this.position = position;
	}

	// Token: 0x0400088D RID: 2189
	public string subsceneID;

	// Token: 0x0400088E RID: 2190
	public Vector3 position;
}
