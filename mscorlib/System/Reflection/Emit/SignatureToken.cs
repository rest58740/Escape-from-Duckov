using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000945 RID: 2373
	[ComVisible(true)]
	public readonly struct SignatureToken : IEquatable<SignatureToken>
	{
		// Token: 0x06005275 RID: 21109 RVA: 0x00102DC7 File Offset: 0x00100FC7
		internal SignatureToken(int val)
		{
			this.tokValue = val;
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x00102DD0 File Offset: 0x00100FD0
		public override bool Equals(object obj)
		{
			bool flag = obj is SignatureToken;
			if (flag)
			{
				SignatureToken signatureToken = (SignatureToken)obj;
				flag = (this.tokValue == signatureToken.tokValue);
			}
			return flag;
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x00102E01 File Offset: 0x00101001
		public bool Equals(SignatureToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x00102E11 File Offset: 0x00101011
		public static bool operator ==(SignatureToken a, SignatureToken b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x00102E24 File Offset: 0x00101024
		public static bool operator !=(SignatureToken a, SignatureToken b)
		{
			return !object.Equals(a, b);
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x00102E3A File Offset: 0x0010103A
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x0600527B RID: 21115 RVA: 0x00102E3A File Offset: 0x0010103A
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x04003318 RID: 13080
		internal readonly int tokValue;

		// Token: 0x04003319 RID: 13081
		public static readonly SignatureToken Empty;
	}
}
