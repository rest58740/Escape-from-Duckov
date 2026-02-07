using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000C7 RID: 199
	[TupleElementNames(new string[]
	{
		"source",
		"destination"
	})]
	[DisallowMultipleComponent]
	public sealed class AsyncRenderImageTrigger : AsyncTriggerBase<ValueTuple<RenderTexture, RenderTexture>>
	{
		// Token: 0x06000515 RID: 1301 RVA: 0x0000C77A File Offset: 0x0000A97A
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.RaiseEvent(new ValueTuple<RenderTexture, RenderTexture>(source, destination));
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0000C789 File Offset: 0x0000A989
		public IAsyncOnRenderImageHandler GetOnRenderImageAsyncHandler()
		{
			return new AsyncTriggerHandler<ValueTuple<RenderTexture, RenderTexture>>(this, false);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000C792 File Offset: 0x0000A992
		public IAsyncOnRenderImageHandler GetOnRenderImageAsyncHandler(CancellationToken cancellationToken)
		{
			return new AsyncTriggerHandler<ValueTuple<RenderTexture, RenderTexture>>(this, cancellationToken, false);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000C79C File Offset: 0x0000A99C
		[return: TupleElementNames(new string[]
		{
			"source",
			"destination"
		})]
		public UniTask<ValueTuple<RenderTexture, RenderTexture>> OnRenderImageAsync()
		{
			return ((IAsyncOnRenderImageHandler)new AsyncTriggerHandler<ValueTuple<RenderTexture, RenderTexture>>(this, true)).OnRenderImageAsync();
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000C7AA File Offset: 0x0000A9AA
		[return: TupleElementNames(new string[]
		{
			"source",
			"destination"
		})]
		public UniTask<ValueTuple<RenderTexture, RenderTexture>> OnRenderImageAsync(CancellationToken cancellationToken)
		{
			return ((IAsyncOnRenderImageHandler)new AsyncTriggerHandler<ValueTuple<RenderTexture, RenderTexture>>(this, cancellationToken, true)).OnRenderImageAsync();
		}
	}
}
