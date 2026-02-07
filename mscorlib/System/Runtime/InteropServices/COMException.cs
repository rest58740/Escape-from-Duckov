using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006D1 RID: 1745
	[Serializable]
	public class COMException : ExternalException
	{
		// Token: 0x06004021 RID: 16417 RVA: 0x000E0B41 File Offset: 0x000DED41
		internal COMException(int hr)
		{
			base.HResult = hr;
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x000E0B50 File Offset: 0x000DED50
		public COMException()
		{
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x000E0B58 File Offset: 0x000DED58
		public COMException(string message) : base(message)
		{
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x000E0B61 File Offset: 0x000DED61
		public COMException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x000E0B6B File Offset: 0x000DED6B
		public COMException(string message, int errorCode) : base(message)
		{
			base.HResult = errorCode;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x000E0B7B File Offset: 0x000DED7B
		protected COMException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x000E0B88 File Offset: 0x000DED88
		public override string ToString()
		{
			string message = this.Message;
			string text = base.GetType().ToString() + " (0x" + base.HResult.ToString("X8", CultureInfo.InvariantCulture) + ")";
			if (message != null && message.Length > 0)
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
