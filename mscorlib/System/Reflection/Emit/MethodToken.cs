using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000937 RID: 2359
	[ComVisible(true)]
	[Serializable]
	public readonly struct MethodToken : IEquatable<MethodToken>
	{
		// Token: 0x06005170 RID: 20848 RVA: 0x000FE97D File Offset: 0x000FCB7D
		internal MethodToken(int val)
		{
			this.tokValue = val;
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x000FE988 File Offset: 0x000FCB88
		public override bool Equals(object obj)
		{
			bool flag = obj is MethodToken;
			if (flag)
			{
				MethodToken methodToken = (MethodToken)obj;
				flag = (this.tokValue == methodToken.tokValue);
			}
			return flag;
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x000FE9B9 File Offset: 0x000FCBB9
		public bool Equals(MethodToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x000FE9C9 File Offset: 0x000FCBC9
		public static bool operator ==(MethodToken a, MethodToken b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x000FE9DC File Offset: 0x000FCBDC
		public static bool operator !=(MethodToken a, MethodToken b)
		{
			return !object.Equals(a, b);
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x000FE9F2 File Offset: 0x000FCBF2
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06005176 RID: 20854 RVA: 0x000FE9F2 File Offset: 0x000FCBF2
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x040031D8 RID: 12760
		internal readonly int tokValue;

		// Token: 0x040031D9 RID: 12761
		public static readonly MethodToken Empty;
	}
}
