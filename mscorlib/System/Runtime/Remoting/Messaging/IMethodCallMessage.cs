using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200061E RID: 1566
	[ComVisible(true)]
	public interface IMethodCallMessage : IMethodMessage, IMessage
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06003AFD RID: 15101
		int InArgCount { get; }

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06003AFE RID: 15102
		object[] InArgs { get; }

		// Token: 0x06003AFF RID: 15103
		object GetInArg(int argNum);

		// Token: 0x06003B00 RID: 15104
		string GetInArgName(int index);
	}
}
