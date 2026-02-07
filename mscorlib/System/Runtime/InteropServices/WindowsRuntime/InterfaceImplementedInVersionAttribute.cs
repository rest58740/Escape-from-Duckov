using System;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x02000784 RID: 1924
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
	public sealed class InterfaceImplementedInVersionAttribute : Attribute
	{
		// Token: 0x0600447E RID: 17534 RVA: 0x000E3ADD File Offset: 0x000E1CDD
		public InterfaceImplementedInVersionAttribute(Type interfaceType, byte majorVersion, byte minorVersion, byte buildVersion, byte revisionVersion)
		{
			this.m_interfaceType = interfaceType;
			this.m_majorVersion = majorVersion;
			this.m_minorVersion = minorVersion;
			this.m_buildVersion = buildVersion;
			this.m_revisionVersion = revisionVersion;
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x0600447F RID: 17535 RVA: 0x000E3B0A File Offset: 0x000E1D0A
		public Type InterfaceType
		{
			get
			{
				return this.m_interfaceType;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06004480 RID: 17536 RVA: 0x000E3B12 File Offset: 0x000E1D12
		public byte MajorVersion
		{
			get
			{
				return this.m_majorVersion;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06004481 RID: 17537 RVA: 0x000E3B1A File Offset: 0x000E1D1A
		public byte MinorVersion
		{
			get
			{
				return this.m_minorVersion;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06004482 RID: 17538 RVA: 0x000E3B22 File Offset: 0x000E1D22
		public byte BuildVersion
		{
			get
			{
				return this.m_buildVersion;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06004483 RID: 17539 RVA: 0x000E3B2A File Offset: 0x000E1D2A
		public byte RevisionVersion
		{
			get
			{
				return this.m_revisionVersion;
			}
		}

		// Token: 0x04002C1E RID: 11294
		private Type m_interfaceType;

		// Token: 0x04002C1F RID: 11295
		private byte m_majorVersion;

		// Token: 0x04002C20 RID: 11296
		private byte m_minorVersion;

		// Token: 0x04002C21 RID: 11297
		private byte m_buildVersion;

		// Token: 0x04002C22 RID: 11298
		private byte m_revisionVersion;
	}
}
