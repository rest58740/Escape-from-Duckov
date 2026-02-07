using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000038 RID: 56
	public static class UniTaskValueTaskExtensions
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x000044C9 File Offset: 0x000026C9
		public static ValueTask AsValueTask(this UniTask task)
		{
			return task;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000044D1 File Offset: 0x000026D1
		public static ValueTask<T> AsValueTask<T>(this UniTask<T> task)
		{
			return task;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000044DC File Offset: 0x000026DC
		public static UniTask<T> AsUniTask<T>(this ValueTask<T> task)
		{
			UniTaskValueTaskExtensions.<AsUniTask>d__2<T> <AsUniTask>d__;
			<AsUniTask>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<AsUniTask>d__.task = task;
			<AsUniTask>d__.<>1__state = -1;
			<AsUniTask>d__.<>t__builder.Start<UniTaskValueTaskExtensions.<AsUniTask>d__2<T>>(ref <AsUniTask>d__);
			return <AsUniTask>d__.<>t__builder.Task;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004520 File Offset: 0x00002720
		public static UniTask AsUniTask(this ValueTask task)
		{
			UniTaskValueTaskExtensions.<AsUniTask>d__3 <AsUniTask>d__;
			<AsUniTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AsUniTask>d__.task = task;
			<AsUniTask>d__.<>1__state = -1;
			<AsUniTask>d__.<>t__builder.Start<UniTaskValueTaskExtensions.<AsUniTask>d__3>(ref <AsUniTask>d__);
			return <AsUniTask>d__.<>t__builder.Task;
		}
	}
}
