using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006D6 RID: 1750
	public sealed class ErrorWrapper
	{
		// Token: 0x0600402E RID: 16430 RVA: 0x000E0C8D File Offset: 0x000DEE8D
		public ErrorWrapper(int errorCode)
		{
			this.m_ErrorCode = errorCode;
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x000E0C9C File Offset: 0x000DEE9C
		public ErrorWrapper(object errorCode)
		{
			if (!(errorCode is int))
			{
				throw new ArgumentException("Object must be of type Int32.", "errorCode");
			}
			this.m_ErrorCode = (int)errorCode;
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x000E0CC8 File Offset: 0x000DEEC8
		public ErrorWrapper(Exception e)
		{
			this.m_ErrorCode = Marshal.GetHRForException(e);
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06004031 RID: 16433 RVA: 0x000E0CDC File Offset: 0x000DEEDC
		public int ErrorCode
		{
			get
			{
				return this.m_ErrorCode;
			}
		}

		// Token: 0x04002A20 RID: 10784
		private int m_ErrorCode;
	}
}
