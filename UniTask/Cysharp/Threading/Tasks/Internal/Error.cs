using System;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x0200010B RID: 267
	internal static class Error
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x0000DF98 File Offset: 0x0000C198
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThrowArgumentNullException<T>(T value, string paramName) where T : class
		{
			if (value == null)
			{
				Error.ThrowArgumentNullExceptionCore(paramName);
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ThrowArgumentNullExceptionCore(string paramName)
		{
			throw new ArgumentNullException(paramName);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0000DFB0 File Offset: 0x0000C1B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Exception ArgumentOutOfRange(string paramName)
		{
			return new ArgumentOutOfRangeException(paramName);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Exception NoElements()
		{
			return new InvalidOperationException("Source sequence doesn't contain any elements.");
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Exception MoreThanOneElement()
		{
			return new InvalidOperationException("Source sequence contains more than one element.");
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0000DFD0 File Offset: 0x0000C1D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void ThrowArgumentException(string message)
		{
			throw new ArgumentException(message);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0000DFD8 File Offset: 0x0000C1D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void ThrowNotYetCompleted()
		{
			throw new InvalidOperationException("Not yet completed.");
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0000DFE4 File Offset: 0x0000C1E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static T ThrowNotYetCompleted<T>()
		{
			throw new InvalidOperationException("Not yet completed.");
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0000DFF0 File Offset: 0x0000C1F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThrowWhenContinuationIsAlreadyRegistered<T>(T continuationField) where T : class
		{
			if (continuationField != null)
			{
				Error.ThrowInvalidOperationExceptionCore("continuation is already registered.");
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0000E004 File Offset: 0x0000C204
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ThrowInvalidOperationExceptionCore(string message)
		{
			throw new InvalidOperationException(message);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0000E00C File Offset: 0x0000C20C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void ThrowOperationCanceledException()
		{
			throw new OperationCanceledException();
		}
	}
}
