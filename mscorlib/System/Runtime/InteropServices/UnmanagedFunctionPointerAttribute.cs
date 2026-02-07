using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006E2 RID: 1762
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public sealed class UnmanagedFunctionPointerAttribute : Attribute
	{
		// Token: 0x0600405B RID: 16475 RVA: 0x000E0F26 File Offset: 0x000DF126
		public UnmanagedFunctionPointerAttribute(CallingConvention callingConvention)
		{
			this.m_callingConvention = callingConvention;
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600405C RID: 16476 RVA: 0x000E0F35 File Offset: 0x000DF135
		public CallingConvention CallingConvention
		{
			get
			{
				return this.m_callingConvention;
			}
		}

		// Token: 0x04002A26 RID: 10790
		private CallingConvention m_callingConvention;

		// Token: 0x04002A27 RID: 10791
		public CharSet CharSet;

		// Token: 0x04002A28 RID: 10792
		public bool BestFitMapping;

		// Token: 0x04002A29 RID: 10793
		public bool ThrowOnUnmappableChar;

		// Token: 0x04002A2A RID: 10794
		public bool SetLastError;
	}
}
