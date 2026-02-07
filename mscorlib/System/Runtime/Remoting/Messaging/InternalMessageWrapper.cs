using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000623 RID: 1571
	[ComVisible(true)]
	public class InternalMessageWrapper
	{
		// Token: 0x06003B15 RID: 15125 RVA: 0x000CE36D File Offset: 0x000CC56D
		public InternalMessageWrapper(IMessage msg)
		{
			this.WrappedMessage = msg;
		}

		// Token: 0x04002692 RID: 9874
		protected IMessage WrappedMessage;
	}
}
