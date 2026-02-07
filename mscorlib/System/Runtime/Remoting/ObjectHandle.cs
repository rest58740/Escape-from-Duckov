using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	// Token: 0x02000564 RID: 1380
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	public class ObjectHandle : MarshalByRefObject, IObjectHandle
	{
		// Token: 0x0600361F RID: 13855 RVA: 0x000C2AE9 File Offset: 0x000C0CE9
		public ObjectHandle(object o)
		{
			this._wrapped = o;
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000C2AF8 File Offset: 0x000C0CF8
		public override object InitializeLifetimeService()
		{
			return base.InitializeLifetimeService();
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x000C2B00 File Offset: 0x000C0D00
		public object Unwrap()
		{
			return this._wrapped;
		}

		// Token: 0x0400252C RID: 9516
		private object _wrapped;
	}
}
