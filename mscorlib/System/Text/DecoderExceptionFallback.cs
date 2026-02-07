using System;

namespace System.Text
{
	// Token: 0x02000393 RID: 915
	[Serializable]
	public sealed class DecoderExceptionFallback : DecoderFallback
	{
		// Token: 0x060025A8 RID: 9640 RVA: 0x00085F1B File Offset: 0x0008411B
		public override DecoderFallbackBuffer CreateFallbackBuffer()
		{
			return new DecoderExceptionFallbackBuffer();
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060025A9 RID: 9641 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override int MaxCharCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x00085F22 File Offset: 0x00084122
		public override bool Equals(object value)
		{
			return value is DecoderExceptionFallback;
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x00085F2F File Offset: 0x0008412F
		public override int GetHashCode()
		{
			return 879;
		}
	}
}
