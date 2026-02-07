using System;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000080 RID: 128
	public interface IAsyncOnAudioFilterReadHandler
	{
		// Token: 0x0600041F RID: 1055
		[return: TupleElementNames(new string[]
		{
			"data",
			"channels"
		})]
		UniTask<ValueTuple<float[], int>> OnAudioFilterReadAsync();
	}
}
