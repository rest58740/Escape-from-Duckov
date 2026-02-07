using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniExcelLibs
{
	// Token: 0x02000017 RID: 23
	internal class MiniExcelTask
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00003B82 File Offset: 0x00001D82
		public static Task FromException(Exception exception)
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			taskCompletionSource.SetException(exception);
			return taskCompletionSource.Task;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003B95 File Offset: 0x00001D95
		public static Task<T> FromException<T>(Exception exception)
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetException(exception);
			return taskCompletionSource.Task;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003BA8 File Offset: 0x00001DA8
		public static Task FromCanceled(CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			cancellationToken.Register(delegate()
			{
				tcs.SetCanceled();
			});
			return tcs.Task;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003BE8 File Offset: 0x00001DE8
		public static Task<T> FromCanceled<T>(CancellationToken cancellationToken)
		{
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			cancellationToken.Register(delegate()
			{
				tcs.SetCanceled();
			});
			return tcs.Task;
		}

		// Token: 0x04000020 RID: 32
		public static Task CompletedTask = Task.FromResult<int>(0);
	}
}
