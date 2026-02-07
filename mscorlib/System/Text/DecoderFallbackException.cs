using System;
using System.Runtime.Serialization;

namespace System.Text
{
	// Token: 0x02000395 RID: 917
	[Serializable]
	public sealed class DecoderFallbackException : ArgumentException
	{
		// Token: 0x060025B2 RID: 9650 RVA: 0x00085FCF File Offset: 0x000841CF
		public DecoderFallbackException() : base("Value does not fall within the expected range.")
		{
			base.HResult = -2147024809;
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x00085FE7 File Offset: 0x000841E7
		public DecoderFallbackException(string message) : base(message)
		{
			base.HResult = -2147024809;
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x00085FFB File Offset: 0x000841FB
		public DecoderFallbackException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024809;
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x00086010 File Offset: 0x00084210
		public DecoderFallbackException(string message, byte[] bytesUnknown, int index) : base(message)
		{
			this._bytesUnknown = bytesUnknown;
			this._index = index;
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x00021113 File Offset: 0x0001F313
		private DecoderFallbackException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x00086027 File Offset: 0x00084227
		public byte[] BytesUnknown
		{
			get
			{
				return this._bytesUnknown;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x0008602F File Offset: 0x0008422F
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x04001D96 RID: 7574
		private byte[] _bytesUnknown;

		// Token: 0x04001D97 RID: 7575
		private int _index;
	}
}
