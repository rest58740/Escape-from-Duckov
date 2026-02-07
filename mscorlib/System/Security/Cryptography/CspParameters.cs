using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Security.Cryptography
{
	// Token: 0x02000486 RID: 1158
	[ComVisible(true)]
	public sealed class CspParameters
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06002EA1 RID: 11937 RVA: 0x000A698E File Offset: 0x000A4B8E
		// (set) Token: 0x06002EA2 RID: 11938 RVA: 0x000A6998 File Offset: 0x000A4B98
		public CspProviderFlags Flags
		{
			get
			{
				return (CspProviderFlags)this.m_flags;
			}
			set
			{
				int num = 255;
				if ((value & (CspProviderFlags)(~(CspProviderFlags)num)) != CspProviderFlags.NoFlags)
				{
					throw new ArgumentException(Environment.GetResourceString("Illegal enum value: {0}.", new object[]
					{
						(int)value
					}), "value");
				}
				this.m_flags = (int)value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x000A69DE File Offset: 0x000A4BDE
		// (set) Token: 0x06002EA4 RID: 11940 RVA: 0x000A69E6 File Offset: 0x000A4BE6
		public CryptoKeySecurity CryptoKeySecurity
		{
			get
			{
				return this.m_cryptoKeySecurity;
			}
			set
			{
				this.m_cryptoKeySecurity = value;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x000A69EF File Offset: 0x000A4BEF
		// (set) Token: 0x06002EA6 RID: 11942 RVA: 0x000A69F7 File Offset: 0x000A4BF7
		public SecureString KeyPassword
		{
			get
			{
				return this.m_keyPassword;
			}
			set
			{
				this.m_keyPassword = value;
				this.m_parentWindowHandle = IntPtr.Zero;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002EA7 RID: 11943 RVA: 0x000A6A0B File Offset: 0x000A4C0B
		// (set) Token: 0x06002EA8 RID: 11944 RVA: 0x000A6A13 File Offset: 0x000A4C13
		public IntPtr ParentWindowHandle
		{
			get
			{
				return this.m_parentWindowHandle;
			}
			set
			{
				this.m_parentWindowHandle = value;
				this.m_keyPassword = null;
			}
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x000A6A23 File Offset: 0x000A4C23
		public CspParameters() : this(1, null, null)
		{
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x000A6A2E File Offset: 0x000A4C2E
		public CspParameters(int dwTypeIn) : this(dwTypeIn, null, null)
		{
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x000A6A39 File Offset: 0x000A4C39
		public CspParameters(int dwTypeIn, string strProviderNameIn) : this(dwTypeIn, strProviderNameIn, null)
		{
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000A6A44 File Offset: 0x000A4C44
		public CspParameters(int dwTypeIn, string strProviderNameIn, string strContainerNameIn) : this(dwTypeIn, strProviderNameIn, strContainerNameIn, CspProviderFlags.NoFlags)
		{
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x000A6A50 File Offset: 0x000A4C50
		public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, SecureString keyPassword) : this(providerType, providerName, keyContainerName)
		{
			this.m_cryptoKeySecurity = cryptoKeySecurity;
			this.m_keyPassword = keyPassword;
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x000A6A6B File Offset: 0x000A4C6B
		public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, IntPtr parentWindowHandle) : this(providerType, providerName, keyContainerName)
		{
			this.m_cryptoKeySecurity = cryptoKeySecurity;
			this.m_parentWindowHandle = parentWindowHandle;
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000A6A86 File Offset: 0x000A4C86
		internal CspParameters(int providerType, string providerName, string keyContainerName, CspProviderFlags flags)
		{
			this.ProviderType = providerType;
			this.ProviderName = providerName;
			this.KeyContainerName = keyContainerName;
			this.KeyNumber = -1;
			this.Flags = flags;
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000A6AB4 File Offset: 0x000A4CB4
		internal CspParameters(CspParameters parameters)
		{
			this.ProviderType = parameters.ProviderType;
			this.ProviderName = parameters.ProviderName;
			this.KeyContainerName = parameters.KeyContainerName;
			this.KeyNumber = parameters.KeyNumber;
			this.Flags = parameters.Flags;
			this.m_cryptoKeySecurity = parameters.m_cryptoKeySecurity;
			this.m_keyPassword = parameters.m_keyPassword;
			this.m_parentWindowHandle = parameters.m_parentWindowHandle;
		}

		// Token: 0x0400214F RID: 8527
		public int ProviderType;

		// Token: 0x04002150 RID: 8528
		public string ProviderName;

		// Token: 0x04002151 RID: 8529
		public string KeyContainerName;

		// Token: 0x04002152 RID: 8530
		public int KeyNumber;

		// Token: 0x04002153 RID: 8531
		private int m_flags;

		// Token: 0x04002154 RID: 8532
		private CryptoKeySecurity m_cryptoKeySecurity;

		// Token: 0x04002155 RID: 8533
		private SecureString m_keyPassword;

		// Token: 0x04002156 RID: 8534
		private IntPtr m_parentWindowHandle;
	}
}
