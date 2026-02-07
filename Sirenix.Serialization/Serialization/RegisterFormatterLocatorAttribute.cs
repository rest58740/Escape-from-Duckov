using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000073 RID: 115
	[AttributeUsage(1, AllowMultiple = true)]
	public class RegisterFormatterLocatorAttribute : Attribute
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0001A4EE File Offset: 0x000186EE
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0001A4F6 File Offset: 0x000186F6
		public Type FormatterLocatorType { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0001A4FF File Offset: 0x000186FF
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x0001A507 File Offset: 0x00018707
		public int Priority { get; private set; }

		// Token: 0x060003B8 RID: 952 RVA: 0x0001A510 File Offset: 0x00018710
		public RegisterFormatterLocatorAttribute(Type formatterLocatorType, int priority = 0)
		{
			this.FormatterLocatorType = formatterLocatorType;
			this.Priority = priority;
		}
	}
}
