using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000C6 RID: 198
	public interface IAsyncOnRenderImageHandler
	{
		// Token: 0x06000514 RID: 1300
		[return: TupleElementNames(new string[]
		{
			"source",
			"destination"
		})]
		UniTask<ValueTuple<RenderTexture, RenderTexture>> OnRenderImageAsync();
	}
}
