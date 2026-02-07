using System;
using Unity;

namespace System.Security.Cryptography
{
	// Token: 0x02000475 RID: 1141
	public sealed class RSASignaturePadding : IEquatable<RSASignaturePadding>
	{
		// Token: 0x06002E39 RID: 11833 RVA: 0x000A6009 File Offset: 0x000A4209
		private RSASignaturePadding(RSASignaturePaddingMode mode)
		{
			this._mode = mode;
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06002E3A RID: 11834 RVA: 0x000A6018 File Offset: 0x000A4218
		public static RSASignaturePadding Pkcs1
		{
			get
			{
				return RSASignaturePadding.s_pkcs1;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002E3B RID: 11835 RVA: 0x000A601F File Offset: 0x000A421F
		public static RSASignaturePadding Pss
		{
			get
			{
				return RSASignaturePadding.s_pss;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002E3C RID: 11836 RVA: 0x000A6026 File Offset: 0x000A4226
		public RSASignaturePaddingMode Mode
		{
			get
			{
				return this._mode;
			}
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000A602E File Offset: 0x000A422E
		public override int GetHashCode()
		{
			return this._mode.GetHashCode();
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000A6041 File Offset: 0x000A4241
		public override bool Equals(object obj)
		{
			return this.Equals(obj as RSASignaturePadding);
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000A604F File Offset: 0x000A424F
		public bool Equals(RSASignaturePadding other)
		{
			return other != null && this._mode == other._mode;
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000A606A File Offset: 0x000A426A
		public static bool operator ==(RSASignaturePadding left, RSASignaturePadding right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000A607B File Offset: 0x000A427B
		public static bool operator !=(RSASignaturePadding left, RSASignaturePadding right)
		{
			return !(left == right);
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000A6087 File Offset: 0x000A4287
		public override string ToString()
		{
			return this._mode.ToString();
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000173AD File Offset: 0x000155AD
		internal RSASignaturePadding()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002123 RID: 8483
		private static readonly RSASignaturePadding s_pkcs1 = new RSASignaturePadding(RSASignaturePaddingMode.Pkcs1);

		// Token: 0x04002124 RID: 8484
		private static readonly RSASignaturePadding s_pss = new RSASignaturePadding(RSASignaturePaddingMode.Pss);

		// Token: 0x04002125 RID: 8485
		private readonly RSASignaturePaddingMode _mode;
	}
}
