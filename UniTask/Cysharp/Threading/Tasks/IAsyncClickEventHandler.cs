using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200005C RID: 92
	public interface IAsyncClickEventHandler : IDisposable
	{
		// Token: 0x0600029C RID: 668
		UniTask OnClickAsync();
	}
}
