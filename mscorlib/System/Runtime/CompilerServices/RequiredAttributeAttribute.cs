using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000847 RID: 2119
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class RequiredAttributeAttribute : Attribute
	{
		// Token: 0x060046C5 RID: 18117 RVA: 0x000E71AD File Offset: 0x000E53AD
		public RequiredAttributeAttribute(Type requiredContract)
		{
			this.requiredContract = requiredContract;
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x060046C6 RID: 18118 RVA: 0x000E71BC File Offset: 0x000E53BC
		public Type RequiredContract
		{
			get
			{
				return this.requiredContract;
			}
		}

		// Token: 0x04002D8F RID: 11663
		private Type requiredContract;
	}
}
