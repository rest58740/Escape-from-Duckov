using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007EC RID: 2028
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class DateTimeConstantAttribute : CustomConstantAttribute
	{
		// Token: 0x060045F3 RID: 17907 RVA: 0x000E575F File Offset: 0x000E395F
		public DateTimeConstantAttribute(long ticks)
		{
			this._date = new DateTime(ticks);
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x000E5773 File Offset: 0x000E3973
		public override object Value
		{
			get
			{
				return this._date;
			}
		}

		// Token: 0x04002D36 RID: 11574
		private DateTime _date;
	}
}
