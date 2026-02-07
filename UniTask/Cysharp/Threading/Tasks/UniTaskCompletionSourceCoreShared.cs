using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000050 RID: 80
	internal static class UniTaskCompletionSourceCoreShared
	{
		// Token: 0x060001AD RID: 429 RVA: 0x00006E58 File Offset: 0x00005058
		private static void CompletionSentinel(object _)
		{
			throw new InvalidOperationException("The sentinel delegate should never be invoked.");
		}

		// Token: 0x040000A7 RID: 167
		internal static readonly Action<object> s_sentinel = new Action<object>(UniTaskCompletionSourceCoreShared.CompletionSentinel);
	}
}
