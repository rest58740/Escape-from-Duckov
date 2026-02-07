using System;
using System.Security;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000B78 RID: 2936
	public class IsolatedStorageSecurityState : SecurityState
	{
		// Token: 0x06006AF4 RID: 27380 RVA: 0x0016E11F File Offset: 0x0016C31F
		internal IsolatedStorageSecurityState()
		{
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06006AF5 RID: 27381 RVA: 0x0002280B File Offset: 0x00020A0B
		public IsolatedStorageSecurityOptions Options
		{
			get
			{
				return IsolatedStorageSecurityOptions.IncreaseQuotaForApplication;
			}
		}

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06006AF6 RID: 27382 RVA: 0x000479FC File Offset: 0x00045BFC
		// (set) Token: 0x06006AF7 RID: 27383 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public long Quota
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
			}
		}

		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06006AF8 RID: 27384 RVA: 0x000479FC File Offset: 0x00045BFC
		public long UsedSize
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06006AF9 RID: 27385 RVA: 0x000479FC File Offset: 0x00045BFC
		public override void EnsureState()
		{
			throw new NotImplementedException();
		}
	}
}
