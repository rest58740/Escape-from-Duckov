using System;

namespace FlexFramework.Excel
{
	// Token: 0x02000014 RID: 20
	public interface ICloneable<T> where T : class
	{
		// Token: 0x0600008B RID: 139
		T DeepClone();

		// Token: 0x0600008C RID: 140
		T ShallowClone();
	}
}
