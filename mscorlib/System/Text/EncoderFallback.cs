using System;
using System.Threading;

namespace System.Text
{
	// Token: 0x020003A1 RID: 929
	[Serializable]
	public abstract class EncoderFallback
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600261A RID: 9754 RVA: 0x000871D3 File Offset: 0x000853D3
		public static EncoderFallback ReplacementFallback
		{
			get
			{
				if (EncoderFallback.s_replacementFallback == null)
				{
					Interlocked.CompareExchange<EncoderFallback>(ref EncoderFallback.s_replacementFallback, new EncoderReplacementFallback(), null);
				}
				return EncoderFallback.s_replacementFallback;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600261B RID: 9755 RVA: 0x000871F2 File Offset: 0x000853F2
		public static EncoderFallback ExceptionFallback
		{
			get
			{
				if (EncoderFallback.s_exceptionFallback == null)
				{
					Interlocked.CompareExchange<EncoderFallback>(ref EncoderFallback.s_exceptionFallback, new EncoderExceptionFallback(), null);
				}
				return EncoderFallback.s_exceptionFallback;
			}
		}

		// Token: 0x0600261C RID: 9756
		public abstract EncoderFallbackBuffer CreateFallbackBuffer();

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x0600261D RID: 9757
		public abstract int MaxCharCount { get; }

		// Token: 0x04001DB1 RID: 7601
		private static EncoderFallback s_replacementFallback;

		// Token: 0x04001DB2 RID: 7602
		private static EncoderFallback s_exceptionFallback;
	}
}
