using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class ftLocalStorage : ScriptableObject
{
	// Token: 0x040000BF RID: 191
	[SerializeField]
	public List<string> modifiedAssetPathList = new List<string>();

	// Token: 0x040000C0 RID: 192
	[SerializeField]
	public List<int> modifiedAssetPaddingHash = new List<int>();
}
