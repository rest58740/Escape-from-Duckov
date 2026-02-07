using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000942 RID: 2370
	[ComVisible(true)]
	[Serializable]
	public readonly struct PropertyToken : IEquatable<PropertyToken>
	{
		// Token: 0x0600524A RID: 21066 RVA: 0x001027C2 File Offset: 0x001009C2
		internal PropertyToken(int val)
		{
			this.tokValue = val;
		}

		// Token: 0x0600524B RID: 21067 RVA: 0x001027CC File Offset: 0x001009CC
		public override bool Equals(object obj)
		{
			bool flag = obj is PropertyToken;
			if (flag)
			{
				PropertyToken propertyToken = (PropertyToken)obj;
				flag = (this.tokValue == propertyToken.tokValue);
			}
			return flag;
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x001027FD File Offset: 0x001009FD
		public bool Equals(PropertyToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x0010280D File Offset: 0x00100A0D
		public static bool operator ==(PropertyToken a, PropertyToken b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x00102820 File Offset: 0x00100A20
		public static bool operator !=(PropertyToken a, PropertyToken b)
		{
			return !object.Equals(a, b);
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x00102836 File Offset: 0x00100A36
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06005250 RID: 21072 RVA: 0x00102836 File Offset: 0x00100A36
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x04003309 RID: 13065
		internal readonly int tokValue;

		// Token: 0x0400330A RID: 13066
		public static readonly PropertyToken Empty;
	}
}
