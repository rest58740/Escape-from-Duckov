using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006D4 RID: 1748
	public sealed class CurrencyWrapper
	{
		// Token: 0x0600402B RID: 16427 RVA: 0x000E0C4A File Offset: 0x000DEE4A
		public CurrencyWrapper(decimal obj)
		{
			this.m_WrappedObject = obj;
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x000E0C59 File Offset: 0x000DEE59
		public CurrencyWrapper(object obj)
		{
			if (!(obj is decimal))
			{
				throw new ArgumentException("Object must be of type Decimal.", "obj");
			}
			this.m_WrappedObject = (decimal)obj;
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x000E0C85 File Offset: 0x000DEE85
		public decimal WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x04002A1C RID: 10780
		private decimal m_WrappedObject;
	}
}
