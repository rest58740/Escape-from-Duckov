using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x0200058A RID: 1418
	[ComVisible(true)]
	public sealed class LifetimeServices
	{
		// Token: 0x0600377E RID: 14206 RVA: 0x000C8050 File Offset: 0x000C6250
		static LifetimeServices()
		{
			LifetimeServices._leaseManagerPollTime = TimeSpan.FromSeconds(10.0);
			LifetimeServices._leaseTime = TimeSpan.FromMinutes(5.0);
			LifetimeServices._renewOnCallTime = TimeSpan.FromMinutes(2.0);
			LifetimeServices._sponsorshipTimeout = TimeSpan.FromMinutes(2.0);
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x0000259F File Offset: 0x0000079F
		[Obsolete("Call the static methods directly on this type instead", true)]
		public LifetimeServices()
		{
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06003780 RID: 14208 RVA: 0x000C80B3 File Offset: 0x000C62B3
		// (set) Token: 0x06003781 RID: 14209 RVA: 0x000C80BA File Offset: 0x000C62BA
		public static TimeSpan LeaseManagerPollTime
		{
			get
			{
				return LifetimeServices._leaseManagerPollTime;
			}
			set
			{
				LifetimeServices._leaseManagerPollTime = value;
				LifetimeServices._leaseManager.SetPollTime(value);
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x000C80CD File Offset: 0x000C62CD
		// (set) Token: 0x06003783 RID: 14211 RVA: 0x000C80D4 File Offset: 0x000C62D4
		public static TimeSpan LeaseTime
		{
			get
			{
				return LifetimeServices._leaseTime;
			}
			set
			{
				LifetimeServices._leaseTime = value;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06003784 RID: 14212 RVA: 0x000C80DC File Offset: 0x000C62DC
		// (set) Token: 0x06003785 RID: 14213 RVA: 0x000C80E3 File Offset: 0x000C62E3
		public static TimeSpan RenewOnCallTime
		{
			get
			{
				return LifetimeServices._renewOnCallTime;
			}
			set
			{
				LifetimeServices._renewOnCallTime = value;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06003786 RID: 14214 RVA: 0x000C80EB File Offset: 0x000C62EB
		// (set) Token: 0x06003787 RID: 14215 RVA: 0x000C80F2 File Offset: 0x000C62F2
		public static TimeSpan SponsorshipTimeout
		{
			get
			{
				return LifetimeServices._sponsorshipTimeout;
			}
			set
			{
				LifetimeServices._sponsorshipTimeout = value;
			}
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x000C80FA File Offset: 0x000C62FA
		internal static void TrackLifetime(ServerIdentity identity)
		{
			LifetimeServices._leaseManager.TrackLifetime(identity);
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x000C8107 File Offset: 0x000C6307
		internal static void StopTrackingLifetime(ServerIdentity identity)
		{
			LifetimeServices._leaseManager.StopTrackingLifetime(identity);
		}

		// Token: 0x0400259A RID: 9626
		private static TimeSpan _leaseManagerPollTime;

		// Token: 0x0400259B RID: 9627
		private static TimeSpan _leaseTime;

		// Token: 0x0400259C RID: 9628
		private static TimeSpan _renewOnCallTime;

		// Token: 0x0400259D RID: 9629
		private static TimeSpan _sponsorshipTimeout;

		// Token: 0x0400259E RID: 9630
		private static LeaseManager _leaseManager = new LeaseManager();
	}
}
