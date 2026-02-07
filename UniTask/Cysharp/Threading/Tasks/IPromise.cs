using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200004C RID: 76
	public interface IPromise<T> : IResolvePromise<T>, IRejectPromise, ICancelPromise
	{
	}
}
