using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000926 RID: 2342
	[ComVisible(true)]
	[Serializable]
	public readonly struct FieldToken : IEquatable<FieldToken>
	{
		// Token: 0x0600503D RID: 20541 RVA: 0x000FAEAE File Offset: 0x000F90AE
		internal FieldToken(int val)
		{
			this.tokValue = val;
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x000FAEB8 File Offset: 0x000F90B8
		public override bool Equals(object obj)
		{
			bool flag = obj is FieldToken;
			if (flag)
			{
				FieldToken fieldToken = (FieldToken)obj;
				flag = (this.tokValue == fieldToken.tokValue);
			}
			return flag;
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x000FAEE9 File Offset: 0x000F90E9
		public bool Equals(FieldToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x000FAEF9 File Offset: 0x000F90F9
		public static bool operator ==(FieldToken a, FieldToken b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x000FAF0C File Offset: 0x000F910C
		public static bool operator !=(FieldToken a, FieldToken b)
		{
			return !object.Equals(a, b);
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x000FAF22 File Offset: 0x000F9122
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06005043 RID: 20547 RVA: 0x000FAF22 File Offset: 0x000F9122
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x0400316B RID: 12651
		internal readonly int tokValue;

		// Token: 0x0400316C RID: 12652
		public static readonly FieldToken Empty;
	}
}
