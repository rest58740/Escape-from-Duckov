using System;

namespace System.Text
{
	// Token: 0x0200039E RID: 926
	[Serializable]
	public sealed class EncoderExceptionFallback : EncoderFallback
	{
		// Token: 0x06002605 RID: 9733 RVA: 0x0008702B File Offset: 0x0008522B
		public override EncoderFallbackBuffer CreateFallbackBuffer()
		{
			return new EncoderExceptionFallbackBuffer();
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override int MaxCharCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x00087032 File Offset: 0x00085232
		public override bool Equals(object value)
		{
			return value is EncoderExceptionFallback;
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0008703F File Offset: 0x0008523F
		public override int GetHashCode()
		{
			return 654;
		}
	}
}
