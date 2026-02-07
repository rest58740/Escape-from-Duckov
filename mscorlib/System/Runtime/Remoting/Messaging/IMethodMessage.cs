using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200061F RID: 1567
	[ComVisible(true)]
	public interface IMethodMessage : IMessage
	{
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06003B01 RID: 15105
		int ArgCount { get; }

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06003B02 RID: 15106
		object[] Args { get; }

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06003B03 RID: 15107
		bool HasVarArgs { get; }

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06003B04 RID: 15108
		LogicalCallContext LogicalCallContext { get; }

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06003B05 RID: 15109
		MethodBase MethodBase { get; }

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06003B06 RID: 15110
		string MethodName { get; }

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06003B07 RID: 15111
		object MethodSignature { get; }

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06003B08 RID: 15112
		string TypeName { get; }

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06003B09 RID: 15113
		string Uri { get; }

		// Token: 0x06003B0A RID: 15114
		object GetArg(int argNum);

		// Token: 0x06003B0B RID: 15115
		string GetArgName(int index);
	}
}
