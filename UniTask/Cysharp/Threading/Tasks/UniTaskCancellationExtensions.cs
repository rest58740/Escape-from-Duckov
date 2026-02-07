using System;
using System.Threading;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000037 RID: 55
	public static class UniTaskCancellationExtensions
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x00004488 File Offset: 0x00002688
		public static CancellationToken GetCancellationTokenOnDestroy(this MonoBehaviour monoBehaviour)
		{
			return monoBehaviour.destroyCancellationToken;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004490 File Offset: 0x00002690
		public static CancellationToken GetCancellationTokenOnDestroy(this GameObject gameObject)
		{
			return gameObject.GetAsyncDestroyTrigger().CancellationToken;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000044A0 File Offset: 0x000026A0
		public static CancellationToken GetCancellationTokenOnDestroy(this Component component)
		{
			MonoBehaviour monoBehaviour = component as MonoBehaviour;
			if (monoBehaviour != null)
			{
				return monoBehaviour.destroyCancellationToken;
			}
			return component.GetAsyncDestroyTrigger().CancellationToken;
		}
	}
}
