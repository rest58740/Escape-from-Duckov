using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000092 RID: 146
	public interface IAsyncOnCollisionStayHandler
	{
		// Token: 0x0600045E RID: 1118
		UniTask<Collision> OnCollisionStayAsync();
	}
}
