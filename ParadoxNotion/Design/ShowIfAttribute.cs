using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D8 RID: 216
	[AttributeUsage(256)]
	public class ShowIfAttribute : DrawerAttribute
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x00017197 File Offset: 0x00015397
		public override bool isDecorator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0001719A File Offset: 0x0001539A
		public override int priority
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0001719D File Offset: 0x0001539D
		public ShowIfAttribute(string fieldName, int checkValue)
		{
			this.fieldName = fieldName;
			this.checkValue = checkValue;
		}

		// Token: 0x04000247 RID: 583
		public readonly string fieldName;

		// Token: 0x04000248 RID: 584
		public readonly int checkValue;
	}
}
