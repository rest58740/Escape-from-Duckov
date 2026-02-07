using System;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public abstract class OptionsProviderBase : MonoBehaviour
{
	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000E08 RID: 3592
	public abstract string Key { get; }

	// Token: 0x06000E09 RID: 3593
	public abstract string[] GetOptions();

	// Token: 0x06000E0A RID: 3594
	public abstract string GetCurrentOption();

	// Token: 0x06000E0B RID: 3595
	public abstract void Set(int index);
}
