using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000914 RID: 2324
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum AssemblyBuilderAccess
	{
		// Token: 0x04003114 RID: 12564
		Run = 1,
		// Token: 0x04003115 RID: 12565
		Save = 2,
		// Token: 0x04003116 RID: 12566
		RunAndSave = 3,
		// Token: 0x04003117 RID: 12567
		ReflectionOnly = 6,
		// Token: 0x04003118 RID: 12568
		RunAndCollect = 9
	}
}
