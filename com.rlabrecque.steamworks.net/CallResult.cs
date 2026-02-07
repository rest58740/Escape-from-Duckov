using System;

namespace Steamworks
{
	// Token: 0x02000188 RID: 392
	public abstract class CallResult
	{
		// Token: 0x060008EF RID: 2287
		internal abstract Type GetCallbackType();

		// Token: 0x060008F0 RID: 2288
		internal abstract void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall);

		// Token: 0x060008F1 RID: 2289
		internal abstract void SetUnregistered();
	}
}
