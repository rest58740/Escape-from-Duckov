using System;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks.CompilerServices
{
	// Token: 0x02000128 RID: 296
	internal interface IStateMachineRunnerPromise<T> : IUniTaskSource<!0>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060006C2 RID: 1730
		Action MoveNext { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060006C3 RID: 1731
		UniTask<T> Task { get; }

		// Token: 0x060006C4 RID: 1732
		void SetResult(T result);

		// Token: 0x060006C5 RID: 1733
		void SetException(Exception exception);
	}
}
