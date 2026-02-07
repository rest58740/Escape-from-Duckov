using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000072 RID: 114
	[AttributeUsage(1, AllowMultiple = true)]
	public class RegisterFormatterAttribute : Attribute
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0001A488 File Offset: 0x00018688
		// (set) Token: 0x060003AD RID: 941 RVA: 0x0001A490 File Offset: 0x00018690
		public Type FormatterType { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0001A499 File Offset: 0x00018699
		// (set) Token: 0x060003AF RID: 943 RVA: 0x0001A4A1 File Offset: 0x000186A1
		public Type WeakFallback { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0001A4AA File Offset: 0x000186AA
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x0001A4B2 File Offset: 0x000186B2
		public int Priority { get; private set; }

		// Token: 0x060003B2 RID: 946 RVA: 0x0001A4BB File Offset: 0x000186BB
		public RegisterFormatterAttribute(Type formatterType, int priority = 0)
		{
			this.FormatterType = formatterType;
			this.Priority = priority;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001A4D1 File Offset: 0x000186D1
		public RegisterFormatterAttribute(Type formatterType, Type weakFallback, int priority = 0)
		{
			this.FormatterType = formatterType;
			this.WeakFallback = weakFallback;
			this.Priority = priority;
		}
	}
}
