using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks.Internal;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000018 RID: 24
	public static class EnumeratorAsyncExtensions
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00002BFC File Offset: 0x00000DFC
		public static UniTask.Awaiter GetAwaiter<T>(this T enumerator) where T : IEnumerator
		{
			T t = enumerator;
			Error.ThrowArgumentNullException<IEnumerator>(t, "enumerator");
			short token;
			return new UniTask(EnumeratorAsyncExtensions.EnumeratorPromise.Create(t, PlayerLoopTiming.Update, CancellationToken.None, out token), token).GetAwaiter();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002C38 File Offset: 0x00000E38
		public static UniTask WithCancellation(this IEnumerator enumerator, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IEnumerator>(enumerator, "enumerator");
			short token;
			return new UniTask(EnumeratorAsyncExtensions.EnumeratorPromise.Create(enumerator, PlayerLoopTiming.Update, cancellationToken, out token), token);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002C60 File Offset: 0x00000E60
		public static UniTask ToUniTask(this IEnumerator enumerator, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IEnumerator>(enumerator, "enumerator");
			short token;
			return new UniTask(EnumeratorAsyncExtensions.EnumeratorPromise.Create(enumerator, timing, cancellationToken, out token), token);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002C88 File Offset: 0x00000E88
		public static UniTask ToUniTask(this IEnumerator enumerator, MonoBehaviour coroutineRunner)
		{
			AutoResetUniTaskCompletionSource autoResetUniTaskCompletionSource = AutoResetUniTaskCompletionSource.Create();
			coroutineRunner.StartCoroutine(EnumeratorAsyncExtensions.Core(enumerator, coroutineRunner, autoResetUniTaskCompletionSource));
			return autoResetUniTaskCompletionSource.Task;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002CB0 File Offset: 0x00000EB0
		private static IEnumerator Core(IEnumerator inner, MonoBehaviour coroutineRunner, AutoResetUniTaskCompletionSource source)
		{
			yield return coroutineRunner.StartCoroutine(inner);
			source.TrySetResult();
			yield break;
		}

		// Token: 0x0200013B RID: 315
		private sealed class EnumeratorPromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<EnumeratorAsyncExtensions.EnumeratorPromise>
		{
			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000753 RID: 1875 RVA: 0x000110DA File Offset: 0x0000F2DA
			public ref EnumeratorAsyncExtensions.EnumeratorPromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000754 RID: 1876 RVA: 0x000110E2 File Offset: 0x0000F2E2
			static EnumeratorPromise()
			{
				TaskPool.RegisterSizeGetter(typeof(EnumeratorAsyncExtensions.EnumeratorPromise), () => EnumeratorAsyncExtensions.EnumeratorPromise.pool.Size);
			}

			// Token: 0x06000755 RID: 1877 RVA: 0x00011121 File Offset: 0x0000F321
			private EnumeratorPromise()
			{
			}

			// Token: 0x06000756 RID: 1878 RVA: 0x0001112C File Offset: 0x0000F32C
			public static IUniTaskSource Create(IEnumerator innerEnumerator, PlayerLoopTiming timing, CancellationToken cancellationToken, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				EnumeratorAsyncExtensions.EnumeratorPromise enumeratorPromise;
				if (!EnumeratorAsyncExtensions.EnumeratorPromise.pool.TryPop(out enumeratorPromise))
				{
					enumeratorPromise = new EnumeratorAsyncExtensions.EnumeratorPromise();
				}
				enumeratorPromise.innerEnumerator = EnumeratorAsyncExtensions.EnumeratorPromise.ConsumeEnumerator(innerEnumerator);
				enumeratorPromise.cancellationToken = cancellationToken;
				enumeratorPromise.loopRunning = true;
				enumeratorPromise.calledGetResult = false;
				enumeratorPromise.initialFrame = -1;
				token = enumeratorPromise.core.Version;
				if (enumeratorPromise.MoveNext())
				{
					PlayerLoopHelper.AddAction(timing, enumeratorPromise);
				}
				return enumeratorPromise;
			}

			// Token: 0x06000757 RID: 1879 RVA: 0x000111A4 File Offset: 0x0000F3A4
			public void GetResult(short token)
			{
				try
				{
					this.calledGetResult = true;
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.loopRunning)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x06000758 RID: 1880 RVA: 0x000111E8 File Offset: 0x0000F3E8
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000759 RID: 1881 RVA: 0x000111F6 File Offset: 0x0000F3F6
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600075A RID: 1882 RVA: 0x00011203 File Offset: 0x0000F403
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600075B RID: 1883 RVA: 0x00011214 File Offset: 0x0000F414
			public bool MoveNext()
			{
				if (this.calledGetResult)
				{
					this.loopRunning = false;
					this.TryReturn();
					return false;
				}
				if (this.innerEnumerator == null)
				{
					return false;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.loopRunning = false;
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.initialFrame == -1)
				{
					if (PlayerLoopHelper.IsMainThread)
					{
						this.initialFrame = Time.frameCount;
					}
				}
				else if (this.initialFrame == Time.frameCount)
				{
					return true;
				}
				try
				{
					if (this.innerEnumerator.MoveNext())
					{
						return true;
					}
				}
				catch (Exception error)
				{
					this.loopRunning = false;
					this.core.TrySetException(error);
					return false;
				}
				this.loopRunning = false;
				this.core.TrySetResult(null);
				return false;
			}

			// Token: 0x0600075C RID: 1884 RVA: 0x000112EC File Offset: 0x0000F4EC
			private bool TryReturn()
			{
				this.core.Reset();
				this.innerEnumerator = null;
				this.cancellationToken = default(CancellationToken);
				return EnumeratorAsyncExtensions.EnumeratorPromise.pool.TryPush(this);
			}

			// Token: 0x0600075D RID: 1885 RVA: 0x00011317 File Offset: 0x0000F517
			private static IEnumerator ConsumeEnumerator(IEnumerator enumerator)
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (obj == null)
					{
						yield return null;
					}
					else
					{
						CustomYieldInstruction cyi = obj as CustomYieldInstruction;
						if (cyi == null)
						{
							if (obj is YieldInstruction)
							{
								IEnumerator innerCoroutine = null;
								AsyncOperation asyncOperation = obj as AsyncOperation;
								if (asyncOperation == null)
								{
									WaitForSeconds waitForSeconds = obj as WaitForSeconds;
									if (waitForSeconds != null)
									{
										innerCoroutine = EnumeratorAsyncExtensions.EnumeratorPromise.UnwrapWaitForSeconds(waitForSeconds);
									}
								}
								else
								{
									innerCoroutine = EnumeratorAsyncExtensions.EnumeratorPromise.UnwrapWaitAsyncOperation(asyncOperation);
								}
								if (innerCoroutine != null)
								{
									while (innerCoroutine.MoveNext())
									{
										yield return null;
									}
									innerCoroutine = null;
									goto IL_159;
								}
							}
							else
							{
								IEnumerator enumerator2 = obj as IEnumerator;
								if (enumerator2 != null)
								{
									IEnumerator innerCoroutine = EnumeratorAsyncExtensions.EnumeratorPromise.ConsumeEnumerator(enumerator2);
									while (innerCoroutine.MoveNext())
									{
										yield return null;
									}
									innerCoroutine = null;
									goto IL_159;
								}
							}
							Debug.LogWarning("yield " + obj.GetType().Name + " is not supported on await IEnumerator or IEnumerator.ToUniTask(), please use ToUniTask(MonoBehaviour coroutineRunner) instead.");
							yield return null;
							continue;
						}
						while (cyi.keepWaiting)
						{
							yield return null;
						}
						IL_159:
						cyi = null;
					}
				}
				yield break;
			}

			// Token: 0x0600075E RID: 1886 RVA: 0x00011326 File Offset: 0x0000F526
			private static IEnumerator UnwrapWaitForSeconds(WaitForSeconds waitForSeconds)
			{
				float second = (float)EnumeratorAsyncExtensions.EnumeratorPromise.waitForSeconds_Seconds.GetValue(waitForSeconds);
				float elapsed = 0f;
				do
				{
					yield return null;
					elapsed += Time.deltaTime;
				}
				while (elapsed < second);
				yield break;
			}

			// Token: 0x0600075F RID: 1887 RVA: 0x00011335 File Offset: 0x0000F535
			private static IEnumerator UnwrapWaitAsyncOperation(AsyncOperation asyncOperation)
			{
				while (!asyncOperation.isDone)
				{
					yield return null;
				}
				yield break;
			}

			// Token: 0x040001B7 RID: 439
			private static TaskPool<EnumeratorAsyncExtensions.EnumeratorPromise> pool;

			// Token: 0x040001B8 RID: 440
			private EnumeratorAsyncExtensions.EnumeratorPromise nextNode;

			// Token: 0x040001B9 RID: 441
			private IEnumerator innerEnumerator;

			// Token: 0x040001BA RID: 442
			private CancellationToken cancellationToken;

			// Token: 0x040001BB RID: 443
			private int initialFrame;

			// Token: 0x040001BC RID: 444
			private bool loopRunning;

			// Token: 0x040001BD RID: 445
			private bool calledGetResult;

			// Token: 0x040001BE RID: 446
			private UniTaskCompletionSourceCore<object> core;

			// Token: 0x040001BF RID: 447
			private static readonly FieldInfo waitForSeconds_Seconds = typeof(WaitForSeconds).GetField("m_Seconds", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
		}
	}
}
