using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200003A RID: 58
	internal static class AwaiterActions
	{
		// Token: 0x06000175 RID: 373 RVA: 0x0000669D File Offset: 0x0000489D
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Continuation(object state)
		{
			((Action)state)();
		}

		// Token: 0x0400007F RID: 127
		internal static readonly Action<object> InvokeContinuationDelegate = new Action<object>(AwaiterActions.Continuation);
	}
}
