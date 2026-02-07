using System;
using System.Threading;

namespace System.Text
{
	// Token: 0x02000396 RID: 918
	[Serializable]
	public abstract class DecoderFallback
	{
		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x00086037 File Offset: 0x00084237
		public static DecoderFallback ReplacementFallback
		{
			get
			{
				DecoderFallback result;
				if ((result = DecoderFallback.s_replacementFallback) == null)
				{
					result = (Interlocked.CompareExchange<DecoderFallback>(ref DecoderFallback.s_replacementFallback, new DecoderReplacementFallback(), null) ?? DecoderFallback.s_replacementFallback);
				}
				return result;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x0008605B File Offset: 0x0008425B
		public static DecoderFallback ExceptionFallback
		{
			get
			{
				DecoderFallback result;
				if ((result = DecoderFallback.s_exceptionFallback) == null)
				{
					result = (Interlocked.CompareExchange<DecoderFallback>(ref DecoderFallback.s_exceptionFallback, new DecoderExceptionFallback(), null) ?? DecoderFallback.s_exceptionFallback);
				}
				return result;
			}
		}

		// Token: 0x060025BB RID: 9659
		public abstract DecoderFallbackBuffer CreateFallbackBuffer();

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060025BC RID: 9660
		public abstract int MaxCharCount { get; }

		// Token: 0x04001D98 RID: 7576
		private static DecoderFallback s_replacementFallback;

		// Token: 0x04001D99 RID: 7577
		private static DecoderFallback s_exceptionFallback;
	}
}
