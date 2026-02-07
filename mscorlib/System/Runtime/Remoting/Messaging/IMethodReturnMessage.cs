using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000620 RID: 1568
	[ComVisible(true)]
	public interface IMethodReturnMessage : IMethodMessage, IMessage
	{
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06003B0C RID: 15116
		Exception Exception { get; }

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06003B0D RID: 15117
		int OutArgCount { get; }

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003B0E RID: 15118
		object[] OutArgs { get; }

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003B0F RID: 15119
		object ReturnValue { get; }

		// Token: 0x06003B10 RID: 15120
		object GetOutArg(int argNum);

		// Token: 0x06003B11 RID: 15121
		string GetOutArgName(int index);
	}
}
