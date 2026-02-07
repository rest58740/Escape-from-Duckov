using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000713 RID: 1811
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[ComVisible(false)]
	public sealed class ManagedToNativeComInteropStubAttribute : Attribute
	{
		// Token: 0x060040C6 RID: 16582 RVA: 0x000E165A File Offset: 0x000DF85A
		public ManagedToNativeComInteropStubAttribute(Type classType, string methodName)
		{
			this._classType = classType;
			this._methodName = methodName;
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x060040C7 RID: 16583 RVA: 0x000E1670 File Offset: 0x000DF870
		public Type ClassType
		{
			get
			{
				return this._classType;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x060040C8 RID: 16584 RVA: 0x000E1678 File Offset: 0x000DF878
		public string MethodName
		{
			get
			{
				return this._methodName;
			}
		}

		// Token: 0x04002AF0 RID: 10992
		internal Type _classType;

		// Token: 0x04002AF1 RID: 10993
		internal string _methodName;
	}
}
