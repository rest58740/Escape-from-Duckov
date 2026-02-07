using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006F3 RID: 1779
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false)]
	[ComVisible(true)]
	[Obsolete("This attribute is deprecated and will be removed in a future version.", false)]
	public sealed class IDispatchImplAttribute : Attribute
	{
		// Token: 0x06004078 RID: 16504 RVA: 0x000E1037 File Offset: 0x000DF237
		public IDispatchImplAttribute(IDispatchImplType implType)
		{
			this._val = implType;
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x000E1037 File Offset: 0x000DF237
		public IDispatchImplAttribute(short implType)
		{
			this._val = (IDispatchImplType)implType;
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x0600407A RID: 16506 RVA: 0x000E1046 File Offset: 0x000DF246
		public IDispatchImplType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A43 RID: 10819
		internal IDispatchImplType _val;
	}
}
