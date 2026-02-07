using System;
using System.Collections;
using System.Threading;

namespace ParadoxNotion.Services
{
	// Token: 0x02000084 RID: 132
	public static class Threader
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0000FF59 File Offset: 0x0000E159
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x0000FF60 File Offset: 0x0000E160
		public static bool applicationIsPlaying { get; private set; } = true;

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x0000FF68 File Offset: 0x0000E168
		public static bool isMainThread
		{
			get
			{
				return Thread.CurrentThread.ManagedThreadId == 1;
			}
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0000FF77 File Offset: 0x0000E177
		public static Thread StartAction(Thread thread, Action function, Action callback = null)
		{
			if (thread != null && thread.IsAlive)
			{
				thread.Abort();
			}
			thread = new Thread(delegate()
			{
				function.Invoke();
			});
			Threader.Begin(thread, callback);
			return thread;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0000FFB0 File Offset: 0x0000E1B0
		public static Thread StartFunction<TResult>(Thread thread, Func<TResult> function, Action<TResult> callback = null)
		{
			if (thread != null && thread.IsAlive)
			{
				thread.Abort();
			}
			TResult result = default(TResult);
			thread = new Thread(delegate()
			{
				result = function.Invoke();
			});
			Threader.Begin(thread, delegate
			{
				callback.Invoke(result);
			});
			return thread;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00010014 File Offset: 0x0000E214
		private static void Begin(Thread thread, Action callback)
		{
			thread.Start();
			MonoManager.current.StartCoroutine(Threader.ThreadMonitor(thread, callback));
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001002E File Offset: 0x0000E22E
		private static IEnumerator ThreadMonitor(Thread thread, Action callback)
		{
			while (thread.IsAlive)
			{
				yield return null;
			}
			yield return null;
			if ((thread.ThreadState & 128) != 128)
			{
				thread.Join();
				if (callback != null)
				{
					callback.Invoke();
				}
			}
			yield break;
		}
	}
}
