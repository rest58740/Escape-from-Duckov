using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020005D1 RID: 1489
	[ComVisible(true)]
	public interface IConstructionCallMessage : IMessage, IMethodCallMessage, IMethodMessage
	{
		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060038DB RID: 14555
		Type ActivationType { get; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060038DC RID: 14556
		string ActivationTypeName { get; }

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060038DD RID: 14557
		// (set) Token: 0x060038DE RID: 14558
		IActivator Activator { get; set; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060038DF RID: 14559
		object[] CallSiteActivationAttributes { get; }

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060038E0 RID: 14560
		IList ContextProperties { get; }
	}
}
