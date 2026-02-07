using System;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x020000A2 RID: 162
	public interface IAsyncOnJointBreakHandler
	{
		// Token: 0x06000496 RID: 1174
		UniTask<float> OnJointBreakAsync();
	}
}
