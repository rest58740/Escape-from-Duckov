using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Security.Cryptography
{
	// Token: 0x020004C7 RID: 1223
	[ComVisible(true)]
	public sealed class CspKeyContainerInfo
	{
		// Token: 0x060030E5 RID: 12517 RVA: 0x000B3213 File Offset: 0x000B1413
		public CspKeyContainerInfo(CspParameters parameters)
		{
			this._params = parameters;
			this._random = true;
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool Accessible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060030E7 RID: 12519 RVA: 0x0000AF5E File Offset: 0x0000915E
		public CryptoKeySecurity CryptoKeySecurity
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060030E8 RID: 12520 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool Exportable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool HardwareDevice
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x000B3229 File Offset: 0x000B1429
		public string KeyContainerName
		{
			get
			{
				return this._params.KeyContainerName;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060030EB RID: 12523 RVA: 0x000B3236 File Offset: 0x000B1436
		public KeyNumber KeyNumber
		{
			get
			{
				return (KeyNumber)this._params.KeyNumber;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool MachineKeyStore
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060030ED RID: 12525 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool Protected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060030EE RID: 12526 RVA: 0x000B3243 File Offset: 0x000B1443
		public string ProviderName
		{
			get
			{
				return this._params.ProviderName;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060030EF RID: 12527 RVA: 0x000B3250 File Offset: 0x000B1450
		public int ProviderType
		{
			get
			{
				return this._params.ProviderType;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060030F0 RID: 12528 RVA: 0x000B325D File Offset: 0x000B145D
		public bool RandomlyGenerated
		{
			get
			{
				return this._random;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060030F1 RID: 12529 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool Removable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x000B3265 File Offset: 0x000B1465
		public string UniqueKeyContainerName
		{
			get
			{
				return this._params.ProviderName + "\\" + this._params.KeyContainerName;
			}
		}

		// Token: 0x04002246 RID: 8774
		private CspParameters _params;

		// Token: 0x04002247 RID: 8775
		internal bool _random;
	}
}
