using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000019 RID: 25
	public static class ExceptionExtensions
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00002CCD File Offset: 0x00000ECD
		public static bool IsOperationCanceledException(this Exception exception)
		{
			return exception is OperationCanceledException;
		}
	}
}
