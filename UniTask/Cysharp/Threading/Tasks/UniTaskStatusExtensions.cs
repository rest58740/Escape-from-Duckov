using System;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000025 RID: 37
	public static class UniTaskStatusExtensions
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00002F6A File Offset: 0x0000116A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCompleted(this UniTaskStatus status)
		{
			return status > UniTaskStatus.Pending;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002F70 File Offset: 0x00001170
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCompletedSuccessfully(this UniTaskStatus status)
		{
			return status == UniTaskStatus.Succeeded;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00002F76 File Offset: 0x00001176
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCanceled(this UniTaskStatus status)
		{
			return status == UniTaskStatus.Canceled;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00002F7C File Offset: 0x0000117C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsFaulted(this UniTaskStatus status)
		{
			return status == UniTaskStatus.Faulted;
		}
	}
}
