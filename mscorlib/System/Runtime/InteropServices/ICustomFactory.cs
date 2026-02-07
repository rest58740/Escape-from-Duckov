using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006D8 RID: 1752
	public interface ICustomFactory
	{
		// Token: 0x06004033 RID: 16435
		MarshalByRefObject CreateInstance(Type serverType);
	}
}
