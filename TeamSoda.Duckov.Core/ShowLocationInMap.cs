using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class ShowLocationInMap : MonoBehaviour
{
	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0002C40E File Offset: 0x0002A60E
	public string DisplayName
	{
		get
		{
			return this.displayName;
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0002C416 File Offset: 0x0002A616
	public string DisplayNameRaw
	{
		get
		{
			return this.displayName;
		}
	}

	// Token: 0x040008F9 RID: 2297
	[SerializeField]
	private string displayName;
}
