using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007ED RID: 2029
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class DecimalConstantAttribute : Attribute
	{
		// Token: 0x060045F5 RID: 17909 RVA: 0x000E5780 File Offset: 0x000E3980
		[CLSCompliant(false)]
		public DecimalConstantAttribute(byte scale, byte sign, uint hi, uint mid, uint low)
		{
			this._dec = new decimal((int)low, (int)mid, (int)hi, sign > 0, scale);
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x000E5780 File Offset: 0x000E3980
		public DecimalConstantAttribute(byte scale, byte sign, int hi, int mid, int low)
		{
			this._dec = new decimal(low, mid, hi, sign > 0, scale);
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x060045F7 RID: 17911 RVA: 0x000E579D File Offset: 0x000E399D
		public decimal Value
		{
			get
			{
				return this._dec;
			}
		}

		// Token: 0x04002D37 RID: 11575
		private decimal _dec;
	}
}
