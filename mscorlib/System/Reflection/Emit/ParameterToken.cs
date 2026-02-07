using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200093F RID: 2367
	[ComVisible(true)]
	[Serializable]
	public readonly struct ParameterToken : IEquatable<ParameterToken>
	{
		// Token: 0x0600520F RID: 21007 RVA: 0x001023F2 File Offset: 0x001005F2
		internal ParameterToken(int val)
		{
			this.tokValue = val;
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x001023FC File Offset: 0x001005FC
		public override bool Equals(object obj)
		{
			bool flag = obj is ParameterToken;
			if (flag)
			{
				ParameterToken parameterToken = (ParameterToken)obj;
				flag = (this.tokValue == parameterToken.tokValue);
			}
			return flag;
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x0010242D File Offset: 0x0010062D
		public bool Equals(ParameterToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		// Token: 0x06005212 RID: 21010 RVA: 0x0010243D File Offset: 0x0010063D
		public static bool operator ==(ParameterToken a, ParameterToken b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x00102450 File Offset: 0x00100650
		public static bool operator !=(ParameterToken a, ParameterToken b)
		{
			return !object.Equals(a, b);
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x00102466 File Offset: 0x00100666
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06005215 RID: 21013 RVA: 0x00102466 File Offset: 0x00100666
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x040032F6 RID: 13046
		internal readonly int tokValue;

		// Token: 0x040032F7 RID: 13047
		public static readonly ParameterToken Empty;
	}
}
