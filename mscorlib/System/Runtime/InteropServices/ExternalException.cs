using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006C4 RID: 1732
	[Serializable]
	public class ExternalException : SystemException
	{
		// Token: 0x06003FD0 RID: 16336 RVA: 0x000DFD78 File Offset: 0x000DDF78
		public ExternalException() : base("External component has thrown an exception.")
		{
			base.HResult = -2147467259;
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x000DFD90 File Offset: 0x000DDF90
		public ExternalException(string message) : base(message)
		{
			base.HResult = -2147467259;
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x000DFDA4 File Offset: 0x000DDFA4
		public ExternalException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2147467259;
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00032814 File Offset: 0x00030A14
		public ExternalException(string message, int errorCode) : base(message)
		{
			base.HResult = errorCode;
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected ExternalException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06003FD5 RID: 16341 RVA: 0x000DFDB9 File Offset: 0x000DDFB9
		public virtual int ErrorCode
		{
			get
			{
				return base.HResult;
			}
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x000DFDC4 File Offset: 0x000DDFC4
		public override string ToString()
		{
			string message = this.Message;
			string text = base.GetType().ToString() + " (0x" + base.HResult.ToString("X8", CultureInfo.InvariantCulture) + ")";
			if (!string.IsNullOrEmpty(message))
			{
				text = text + ": " + message;
			}
			Exception innerException = base.InnerException;
			if (innerException != null)
			{
				text = text + " ---> " + innerException.ToString();
			}
			if (this.StackTrace != null)
			{
				text = text + Environment.NewLine + this.StackTrace;
			}
			return text;
		}
	}
}
