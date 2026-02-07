using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x02000583 RID: 1411
	[ComVisible(true)]
	public interface ILease
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06003750 RID: 14160
		TimeSpan CurrentLeaseTime { get; }

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06003751 RID: 14161
		LeaseState CurrentState { get; }

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06003752 RID: 14162
		// (set) Token: 0x06003753 RID: 14163
		TimeSpan InitialLeaseTime { get; set; }

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06003754 RID: 14164
		// (set) Token: 0x06003755 RID: 14165
		TimeSpan RenewOnCallTime { get; set; }

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06003756 RID: 14166
		// (set) Token: 0x06003757 RID: 14167
		TimeSpan SponsorshipTimeout { get; set; }

		// Token: 0x06003758 RID: 14168
		void Register(ISponsor obj);

		// Token: 0x06003759 RID: 14169
		void Register(ISponsor obj, TimeSpan renewalTime);

		// Token: 0x0600375A RID: 14170
		TimeSpan Renew(TimeSpan renewalTime);

		// Token: 0x0600375B RID: 14171
		void Unregister(ISponsor obj);
	}
}
