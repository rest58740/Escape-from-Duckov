using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x020003DE RID: 990
	[ComVisible(true)]
	[MonoTODO("Not supported in the runtime")]
	[Serializable]
	public class HostProtectionException : SystemException
	{
		// Token: 0x060028A2 RID: 10402 RVA: 0x00092A55 File Offset: 0x00090C55
		public HostProtectionException()
		{
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x0006E6B1 File Offset: 0x0006C8B1
		public HostProtectionException(string message) : base(message)
		{
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x0006E6BA File Offset: 0x0006C8BA
		public HostProtectionException(string message, Exception e) : base(message, e)
		{
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000932D0 File Offset: 0x000914D0
		public HostProtectionException(string message, HostProtectionResource protectedResources, HostProtectionResource demandedResources) : base(message)
		{
			this._protected = protectedResources;
			this._demanded = demandedResources;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000932E7 File Offset: 0x000914E7
		protected HostProtectionException(SerializationInfo info, StreamingContext context)
		{
			this.GetObjectData(info, context);
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060028A7 RID: 10407 RVA: 0x000932F7 File Offset: 0x000914F7
		public HostProtectionResource DemandedResources
		{
			get
			{
				return this._demanded;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x000932FF File Offset: 0x000914FF
		public HostProtectionResource ProtectedResources
		{
			get
			{
				return this._protected;
			}
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x0005D90C File Offset: 0x0005BB0C
		[MonoTODO]
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x00093307 File Offset: 0x00091507
		[MonoTODO]
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x04001EB5 RID: 7861
		private HostProtectionResource _protected;

		// Token: 0x04001EB6 RID: 7862
		private HostProtectionResource _demanded;
	}
}
