using System;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x02000782 RID: 1922
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public sealed class DefaultInterfaceAttribute : Attribute
	{
		// Token: 0x0600447B RID: 17531 RVA: 0x000E3AC6 File Offset: 0x000E1CC6
		public DefaultInterfaceAttribute(Type defaultInterface)
		{
			this.m_defaultInterface = defaultInterface;
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x0600447C RID: 17532 RVA: 0x000E3AD5 File Offset: 0x000E1CD5
		public Type DefaultInterface
		{
			get
			{
				return this.m_defaultInterface;
			}
		}

		// Token: 0x04002C1D RID: 11293
		private Type m_defaultInterface;
	}
}
