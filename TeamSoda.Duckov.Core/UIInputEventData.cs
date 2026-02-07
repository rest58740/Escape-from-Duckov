using System;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class UIInputEventData
{
	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0003221F File Offset: 0x0003041F
	public bool Used
	{
		get
		{
			return this.used;
		}
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00032227 File Offset: 0x00030427
	public void Use()
	{
		this.used = true;
	}

	// Token: 0x04000A09 RID: 2569
	private bool used;

	// Token: 0x04000A0A RID: 2570
	public Vector2 vector;

	// Token: 0x04000A0B RID: 2571
	public bool confirm;

	// Token: 0x04000A0C RID: 2572
	public bool cancel;
}
