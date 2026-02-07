using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000949 RID: 2377
	[ComVisible(true)]
	[Serializable]
	public readonly struct TypeToken : IEquatable<TypeToken>
	{
		// Token: 0x06005353 RID: 21331 RVA: 0x00105C14 File Offset: 0x00103E14
		internal TypeToken(int val)
		{
			this.tokValue = val;
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x00105C20 File Offset: 0x00103E20
		public override bool Equals(object obj)
		{
			bool flag = obj is TypeToken;
			if (flag)
			{
				TypeToken typeToken = (TypeToken)obj;
				flag = (this.tokValue == typeToken.tokValue);
			}
			return flag;
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x00105C51 File Offset: 0x00103E51
		public bool Equals(TypeToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x00105C61 File Offset: 0x00103E61
		public static bool operator ==(TypeToken a, TypeToken b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x00105C74 File Offset: 0x00103E74
		public static bool operator !=(TypeToken a, TypeToken b)
		{
			return !object.Equals(a, b);
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x00105C8A File Offset: 0x00103E8A
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06005359 RID: 21337 RVA: 0x00105C8A File Offset: 0x00103E8A
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x0400333D RID: 13117
		internal readonly int tokValue;

		// Token: 0x0400333E RID: 13118
		public static readonly TypeToken Empty;
	}
}
