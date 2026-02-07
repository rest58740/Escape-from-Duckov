using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Cryptography
{
	// Token: 0x02000484 RID: 1156
	[ComVisible(true)]
	[Serializable]
	public class CryptographicUnexpectedOperationException : CryptographicException
	{
		// Token: 0x06002E9C RID: 11932 RVA: 0x000A6929 File Offset: 0x000A4B29
		public CryptographicUnexpectedOperationException()
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000A693C File Offset: 0x000A4B3C
		public CryptographicUnexpectedOperationException(string message) : base(message)
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000A6950 File Offset: 0x000A4B50
		public CryptographicUnexpectedOperationException(string format, string insert) : base(string.Format(CultureInfo.CurrentCulture, format, insert))
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000A696F File Offset: 0x000A4B6F
		public CryptographicUnexpectedOperationException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000A6984 File Offset: 0x000A4B84
		protected CryptographicUnexpectedOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
