using System;
using System.Runtime.Serialization;

namespace System.Text
{
	// Token: 0x020003A0 RID: 928
	[Serializable]
	public sealed class EncoderFallbackException : ArgumentException
	{
		// Token: 0x0600260F RID: 9743 RVA: 0x00085FCF File Offset: 0x000841CF
		public EncoderFallbackException() : base("Value does not fall within the expected range.")
		{
			base.HResult = -2147024809;
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x00085FE7 File Offset: 0x000841E7
		public EncoderFallbackException(string message) : base(message)
		{
			base.HResult = -2147024809;
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x00085FFB File Offset: 0x000841FB
		public EncoderFallbackException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024809;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x00087105 File Offset: 0x00085305
		internal EncoderFallbackException(string message, char charUnknown, int index) : base(message)
		{
			this._charUnknown = charUnknown;
			this._index = index;
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x0008711C File Offset: 0x0008531C
		internal EncoderFallbackException(string message, char charUnknownHigh, char charUnknownLow, int index) : base(message)
		{
			if (!char.IsHighSurrogate(charUnknownHigh))
			{
				throw new ArgumentOutOfRangeException("charUnknownHigh", SR.Format("Valid values are between {0} and {1}, inclusive.", 55296, 56319));
			}
			if (!char.IsLowSurrogate(charUnknownLow))
			{
				throw new ArgumentOutOfRangeException("CharUnknownLow", SR.Format("Valid values are between {0} and {1}, inclusive.", 56320, 57343));
			}
			this._charUnknownHigh = charUnknownHigh;
			this._charUnknownLow = charUnknownLow;
			this._index = index;
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x00021113 File Offset: 0x0001F313
		private EncoderFallbackException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x000871A8 File Offset: 0x000853A8
		public char CharUnknown
		{
			get
			{
				return this._charUnknown;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06002616 RID: 9750 RVA: 0x000871B0 File Offset: 0x000853B0
		public char CharUnknownHigh
		{
			get
			{
				return this._charUnknownHigh;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06002617 RID: 9751 RVA: 0x000871B8 File Offset: 0x000853B8
		public char CharUnknownLow
		{
			get
			{
				return this._charUnknownLow;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06002618 RID: 9752 RVA: 0x000871C0 File Offset: 0x000853C0
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000871C8 File Offset: 0x000853C8
		public bool IsUnknownSurrogate()
		{
			return this._charUnknownHigh > '\0';
		}

		// Token: 0x04001DAD RID: 7597
		private char _charUnknown;

		// Token: 0x04001DAE RID: 7598
		private char _charUnknownHigh;

		// Token: 0x04001DAF RID: 7599
		private char _charUnknownLow;

		// Token: 0x04001DB0 RID: 7600
		private int _index;
	}
}
