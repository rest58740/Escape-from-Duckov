using System;

namespace Cysharp.Threading.Tasks.CompilerServices
{
	// Token: 0x02000126 RID: 294
	internal interface IStateMachineRunner
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060006BC RID: 1724
		Action MoveNext { get; }

		// Token: 0x060006BD RID: 1725
		void Return();
	}
}
