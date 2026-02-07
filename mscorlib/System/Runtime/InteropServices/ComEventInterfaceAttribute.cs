using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200070D RID: 1805
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComEventInterfaceAttribute : Attribute
	{
		// Token: 0x060040B6 RID: 16566 RVA: 0x000E159B File Offset: 0x000DF79B
		public ComEventInterfaceAttribute(Type SourceInterface, Type EventProvider)
		{
			this._SourceInterface = SourceInterface;
			this._EventProvider = EventProvider;
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x060040B7 RID: 16567 RVA: 0x000E15B1 File Offset: 0x000DF7B1
		public Type SourceInterface
		{
			get
			{
				return this._SourceInterface;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x060040B8 RID: 16568 RVA: 0x000E15B9 File Offset: 0x000DF7B9
		public Type EventProvider
		{
			get
			{
				return this._EventProvider;
			}
		}

		// Token: 0x04002AE5 RID: 10981
		internal Type _SourceInterface;

		// Token: 0x04002AE6 RID: 10982
		internal Type _EventProvider;
	}
}
