using System;

namespace VLB
{
	// Token: 0x02000046 RID: 70
	public static class Version
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000AC81 File Offset: 0x00008E81
		public static string CurrentAsString
		{
			get
			{
				return Version.GetVersionAsString(20200);
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000AC90 File Offset: 0x00008E90
		private static string GetVersionAsString(int version)
		{
			int num = version / 10000;
			int num2 = (version - num * 10000) / 100;
			int num3 = (version - num * 10000 - num2 * 100) / 1;
			return string.Format("{0}.{1}.{2}", num, num2, num3);
		}

		// Token: 0x0400019E RID: 414
		public const int Current = 20200;
	}
}
