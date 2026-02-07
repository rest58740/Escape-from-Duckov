using System;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks.CompilerServices
{
	// Token: 0x02000127 RID: 295
	internal interface IStateMachineRunnerPromise : IUniTaskSource, IValueTaskSource
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060006BE RID: 1726
		Action MoveNext { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060006BF RID: 1727
		UniTask Task { get; }

		// Token: 0x060006C0 RID: 1728
		void SetResult();

		// Token: 0x060006C1 RID: 1729
		void SetException(Exception exception);
	}
}
