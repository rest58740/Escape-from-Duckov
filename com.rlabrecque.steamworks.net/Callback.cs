using System;

namespace Steamworks
{
	// Token: 0x02000186 RID: 390
	public abstract class Callback
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060008DD RID: 2269
		public abstract bool IsGameServer { get; }

		// Token: 0x060008DE RID: 2270
		internal abstract Type GetCallbackType();

		// Token: 0x060008DF RID: 2271
		internal abstract void OnRunCallback(IntPtr pvParam);

		// Token: 0x060008E0 RID: 2272
		internal abstract void SetUnregistered();
	}
}
