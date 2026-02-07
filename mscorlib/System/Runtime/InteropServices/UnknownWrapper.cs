using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006DF RID: 1759
	public sealed class UnknownWrapper
	{
		// Token: 0x0600404E RID: 16462 RVA: 0x000E0E24 File Offset: 0x000DF024
		public UnknownWrapper(object obj)
		{
			this.m_WrappedObject = obj;
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x000E0E33 File Offset: 0x000DF033
		public object WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x04002A21 RID: 10785
		private object m_WrappedObject;
	}
}
