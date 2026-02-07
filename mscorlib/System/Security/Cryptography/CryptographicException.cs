using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Win32;

namespace System.Security.Cryptography
{
	// Token: 0x02000483 RID: 1155
	[ComVisible(true)]
	[Serializable]
	public class CryptographicException : SystemException
	{
		// Token: 0x06002E95 RID: 11925 RVA: 0x000A6887 File Offset: 0x000A4A87
		public CryptographicException() : base(Environment.GetResourceString("Error occurred during a cryptographic operation."))
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000A68A4 File Offset: 0x000A4AA4
		public CryptographicException(string message) : base(message)
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000A68B8 File Offset: 0x000A4AB8
		public CryptographicException(string format, string insert) : base(string.Format(CultureInfo.CurrentCulture, format, insert))
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000A68D7 File Offset: 0x000A4AD7
		public CryptographicException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000A68EC File Offset: 0x000A4AEC
		[SecuritySafeCritical]
		public CryptographicException(int hr) : this(Win32Native.GetMessage(hr))
		{
			if (((long)hr & (long)((ulong)-2147483648)) != (long)((ulong)-2147483648))
			{
				hr = ((hr & 65535) | -2147024896);
			}
			base.SetErrorCode(hr);
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected CryptographicException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000A6921 File Offset: 0x000A4B21
		private static void ThrowCryptographicException(int hr)
		{
			throw new CryptographicException(hr);
		}

		// Token: 0x04002142 RID: 8514
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04002143 RID: 8515
		private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04002144 RID: 8516
		private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
	}
}
