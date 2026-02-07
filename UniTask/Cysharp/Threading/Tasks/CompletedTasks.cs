using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200003E RID: 62
	internal static class CompletedTasks
	{
		// Token: 0x04000088 RID: 136
		public static readonly UniTask<AsyncUnit> AsyncUnit = UniTask.FromResult<AsyncUnit>(Cysharp.Threading.Tasks.AsyncUnit.Default);

		// Token: 0x04000089 RID: 137
		public static readonly UniTask<bool> True = UniTask.FromResult<bool>(true);

		// Token: 0x0400008A RID: 138
		public static readonly UniTask<bool> False = UniTask.FromResult<bool>(false);

		// Token: 0x0400008B RID: 139
		public static readonly UniTask<int> Zero = UniTask.FromResult<int>(0);

		// Token: 0x0400008C RID: 140
		public static readonly UniTask<int> MinusOne = UniTask.FromResult<int>(-1);

		// Token: 0x0400008D RID: 141
		public static readonly UniTask<int> One = UniTask.FromResult<int>(1);
	}
}
