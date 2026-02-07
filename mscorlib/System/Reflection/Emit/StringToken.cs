using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000946 RID: 2374
	[ComVisible(true)]
	[Serializable]
	public readonly struct StringToken : IEquatable<StringToken>
	{
		// Token: 0x0600527D RID: 21117 RVA: 0x00102E42 File Offset: 0x00101042
		internal StringToken(int val)
		{
			this.tokValue = val;
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x00102E4C File Offset: 0x0010104C
		public override bool Equals(object obj)
		{
			bool flag = obj is StringToken;
			if (flag)
			{
				StringToken stringToken = (StringToken)obj;
				flag = (this.tokValue == stringToken.tokValue);
			}
			return flag;
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x00102E7D File Offset: 0x0010107D
		public bool Equals(StringToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x00102E8D File Offset: 0x0010108D
		public static bool operator ==(StringToken a, StringToken b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x00102EA0 File Offset: 0x001010A0
		public static bool operator !=(StringToken a, StringToken b)
		{
			return !object.Equals(a, b);
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x00102EB6 File Offset: 0x001010B6
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06005283 RID: 21123 RVA: 0x00102EB6 File Offset: 0x001010B6
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x0400331A RID: 13082
		internal readonly int tokValue;
	}
}
