using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006E0 RID: 1760
	public sealed class VariantWrapper
	{
		// Token: 0x06004050 RID: 16464 RVA: 0x000E0E3B File Offset: 0x000DF03B
		public VariantWrapper(object obj)
		{
			this.m_WrappedObject = obj;
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06004051 RID: 16465 RVA: 0x000E0E4A File Offset: 0x000DF04A
		public object WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x04002A22 RID: 10786
		private object m_WrappedObject;
	}
}
