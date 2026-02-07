using System;

namespace FlexFramework.Excel
{
	// Token: 0x0200000F RID: 15
	public interface IConverter
	{
		// Token: 0x06000041 RID: 65
		object Convert(string input);

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000042 RID: 66
		Type Type { get; }
	}
}
