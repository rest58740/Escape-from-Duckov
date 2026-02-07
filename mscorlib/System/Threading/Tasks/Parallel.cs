using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace System.Threading.Tasks
{
	// Token: 0x0200031D RID: 797
	public static class Parallel
	{
		// Token: 0x060021E6 RID: 8678 RVA: 0x00079438 File Offset: 0x00077638
		public static void Invoke(params Action[] actions)
		{
			Parallel.Invoke(Parallel.s_defaultParallelOptions, actions);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x00079448 File Offset: 0x00077648
		public static void Invoke(ParallelOptions parallelOptions, params Action[] actions)
		{
			if (actions == null)
			{
				throw new ArgumentNullException("actions");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			parallelOptions.CancellationToken.ThrowIfCancellationRequested();
			Action[] actionsCopy = new Action[actions.Length];
			for (int i = 0; i < actionsCopy.Length; i++)
			{
				actionsCopy[i] = actions[i];
				if (actionsCopy[i] == null)
				{
					throw new ArgumentException("One of the actions was null.");
				}
			}
			int forkJoinContextID = 0;
			if (ParallelEtwProvider.Log.IsEnabled())
			{
				forkJoinContextID = Interlocked.Increment(ref Parallel.s_forkJoinContextID);
				ParallelEtwProvider.Log.ParallelInvokeBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelInvoke, actionsCopy.Length);
			}
			if (actionsCopy.Length < 1)
			{
				return;
			}
			try
			{
				if (actionsCopy.Length > 10 || (parallelOptions.MaxDegreeOfParallelism != -1 && parallelOptions.MaxDegreeOfParallelism < actionsCopy.Length))
				{
					ConcurrentQueue<Exception> exceptionQ = null;
					int actionIndex = 0;
					try
					{
						TaskReplicator.Run<object>(delegate(ref object state, int timeout, out bool replicationDelegateYieldedBeforeCompletion)
						{
							replicationDelegateYieldedBeforeCompletion = false;
							for (int k = Interlocked.Increment(ref actionIndex); k <= actionsCopy.Length; k = Interlocked.Increment(ref actionIndex))
							{
								try
								{
									actionsCopy[k - 1]();
								}
								catch (Exception item2)
								{
									LazyInitializer.EnsureInitialized<ConcurrentQueue<Exception>>(ref exceptionQ, () => new ConcurrentQueue<Exception>());
									exceptionQ.Enqueue(item2);
								}
								parallelOptions.CancellationToken.ThrowIfCancellationRequested();
							}
						}, parallelOptions, false);
					}
					catch (Exception ex)
					{
						LazyInitializer.EnsureInitialized<ConcurrentQueue<Exception>>(ref exceptionQ, () => new ConcurrentQueue<Exception>());
						if (ex is ObjectDisposedException)
						{
							throw;
						}
						AggregateException ex2 = ex as AggregateException;
						if (ex2 != null)
						{
							using (IEnumerator<Exception> enumerator = ex2.InnerExceptions.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Exception item = enumerator.Current;
									exceptionQ.Enqueue(item);
								}
								goto IL_1C3;
							}
						}
						exceptionQ.Enqueue(ex);
						IL_1C3:;
					}
					if (exceptionQ != null && exceptionQ.Count > 0)
					{
						Parallel.ThrowSingleCancellationExceptionOrOtherException(exceptionQ, parallelOptions.CancellationToken, new AggregateException(exceptionQ));
					}
				}
				else
				{
					Task[] array = new Task[actionsCopy.Length];
					parallelOptions.CancellationToken.ThrowIfCancellationRequested();
					for (int j = 1; j < array.Length; j++)
					{
						array[j] = Task.Factory.StartNew(actionsCopy[j], parallelOptions.CancellationToken, TaskCreationOptions.None, parallelOptions.EffectiveTaskScheduler);
					}
					array[0] = new Task(actionsCopy[0], parallelOptions.CancellationToken, TaskCreationOptions.None);
					array[0].RunSynchronously(parallelOptions.EffectiveTaskScheduler);
					try
					{
						Task.WaitAll(array);
					}
					catch (AggregateException ex3)
					{
						Parallel.ThrowSingleCancellationExceptionOrOtherException(ex3.InnerExceptions, parallelOptions.CancellationToken, ex3);
					}
				}
			}
			finally
			{
				if (ParallelEtwProvider.Log.IsEnabled())
				{
					ParallelEtwProvider.Log.ParallelInvokeEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
				}
			}
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x000797B0 File Offset: 0x000779B0
		public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return Parallel.ForWorker<object>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, body, null, null, null, null);
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x000797D1 File Offset: 0x000779D1
		public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return Parallel.ForWorker64<object>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, body, null, null, null, null);
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000797F2 File Offset: 0x000779F2
		public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForWorker<object>(fromInclusive, toExclusive, parallelOptions, body, null, null, null, null);
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0007981D File Offset: 0x00077A1D
		public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForWorker64<object>(fromInclusive, toExclusive, parallelOptions, body, null, null, null, null);
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x00079848 File Offset: 0x00077A48
		public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int, ParallelLoopState> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return Parallel.ForWorker<object>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, null, body, null, null, null);
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x00079869 File Offset: 0x00077A69
		public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long, ParallelLoopState> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return Parallel.ForWorker64<object>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, null, body, null, null, null);
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0007988A File Offset: 0x00077A8A
		public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int, ParallelLoopState> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForWorker<object>(fromInclusive, toExclusive, parallelOptions, null, body, null, null, null);
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000798B5 File Offset: 0x00077AB5
		public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long, ParallelLoopState> body)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForWorker64<object>(fromInclusive, toExclusive, parallelOptions, null, body, null, null, null);
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000798E0 File Offset: 0x00077AE0
		public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return Parallel.ForWorker<TLocal>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, null, null, body, localInit, localFinally);
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x0007991F File Offset: 0x00077B1F
		public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return Parallel.ForWorker64<TLocal>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, null, null, body, localInit, localFinally);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00079960 File Offset: 0x00077B60
		public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForWorker<TLocal>(fromInclusive, toExclusive, parallelOptions, null, null, body, localInit, localFinally);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x000799B8 File Offset: 0x00077BB8
		public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForWorker64<TLocal>(fromInclusive, toExclusive, parallelOptions, null, null, body, localInit, localFinally);
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00079A10 File Offset: 0x00077C10
		private static bool CheckTimeoutReached(int timeoutOccursAt)
		{
			int tickCount = Environment.TickCount;
			return tickCount >= timeoutOccursAt && (0 <= timeoutOccursAt || 0 >= tickCount);
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x00079A34 File Offset: 0x00077C34
		private static int ComputeTimeoutPoint(int timeoutLength)
		{
			return Environment.TickCount + timeoutLength;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x00079A40 File Offset: 0x00077C40
		private static ParallelLoopResult ForWorker<TLocal>(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int> body, Action<int, ParallelLoopState> bodyWithState, Func<int, ParallelLoopState, TLocal, TLocal> bodyWithLocal, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			ParallelLoopResult result = default(ParallelLoopResult);
			if (toExclusive <= fromInclusive)
			{
				result._completed = true;
				return result;
			}
			ParallelLoopStateFlags32 sharedPStateFlags = new ParallelLoopStateFlags32();
			parallelOptions.CancellationToken.ThrowIfCancellationRequested();
			int nNumExpectedWorkers = (parallelOptions.EffectiveMaxConcurrencyLevel == -1) ? PlatformHelper.ProcessorCount : parallelOptions.EffectiveMaxConcurrencyLevel;
			RangeManager rangeManager = new RangeManager((long)fromInclusive, (long)toExclusive, 1L, nNumExpectedWorkers);
			OperationCanceledException oce = null;
			CancellationTokenRegistration cancellationTokenRegistration = (!parallelOptions.CancellationToken.CanBeCanceled) ? default(CancellationTokenRegistration) : parallelOptions.CancellationToken.Register(delegate(object o)
			{
				oce = new OperationCanceledException(parallelOptions.CancellationToken);
				sharedPStateFlags.Cancel();
			}, null, false);
			int forkJoinContextID = 0;
			if (ParallelEtwProvider.Log.IsEnabled())
			{
				forkJoinContextID = Interlocked.Increment(ref Parallel.s_forkJoinContextID);
				ParallelEtwProvider.Log.ParallelLoopBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelFor, (long)fromInclusive, (long)toExclusive);
			}
			try
			{
				try
				{
					TaskReplicator.Run<RangeWorker>(delegate(ref RangeWorker currentWorker, int timeout, out bool replicationDelegateYieldedBeforeCompletion)
					{
						if (!currentWorker.IsInitialized)
						{
							currentWorker = rangeManager.RegisterNewWorker();
						}
						replicationDelegateYieldedBeforeCompletion = false;
						int num2;
						int num3;
						if (!currentWorker.FindNewWork32(out num2, out num3) || sharedPStateFlags.ShouldExitLoop(num2))
						{
							return;
						}
						if (ParallelEtwProvider.Log.IsEnabled())
						{
							ParallelEtwProvider.Log.ParallelFork(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
						}
						TLocal tlocal = default(TLocal);
						bool flag = false;
						try
						{
							ParallelLoopState32 parallelLoopState = null;
							if (bodyWithState != null)
							{
								parallelLoopState = new ParallelLoopState32(sharedPStateFlags);
							}
							else if (bodyWithLocal != null)
							{
								parallelLoopState = new ParallelLoopState32(sharedPStateFlags);
								if (localInit != null)
								{
									tlocal = localInit();
									flag = true;
								}
							}
							int timeoutOccursAt = Parallel.ComputeTimeoutPoint(timeout);
							for (;;)
							{
								if (body != null)
								{
									for (int i = num2; i < num3; i++)
									{
										if (sharedPStateFlags.LoopStateFlags != 0 && sharedPStateFlags.ShouldExitLoop())
										{
											break;
										}
										body(i);
									}
								}
								else if (bodyWithState != null)
								{
									for (int j = num2; j < num3; j++)
									{
										if (sharedPStateFlags.LoopStateFlags != 0 && sharedPStateFlags.ShouldExitLoop(j))
										{
											break;
										}
										parallelLoopState.CurrentIteration = j;
										bodyWithState(j, parallelLoopState);
									}
								}
								else
								{
									int num4 = num2;
									while (num4 < num3 && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(num4)))
									{
										parallelLoopState.CurrentIteration = num4;
										tlocal = bodyWithLocal(num4, parallelLoopState, tlocal);
										num4++;
									}
								}
								if (Parallel.CheckTimeoutReached(timeoutOccursAt))
								{
									break;
								}
								if (!currentWorker.FindNewWork32(out num2, out num3) || (sharedPStateFlags.LoopStateFlags != 0 && sharedPStateFlags.ShouldExitLoop(num2)))
								{
									goto IL_1D8;
								}
							}
							replicationDelegateYieldedBeforeCompletion = true;
							IL_1D8:;
						}
						catch (Exception source)
						{
							sharedPStateFlags.SetExceptional();
							ExceptionDispatchInfo.Throw(source);
						}
						finally
						{
							if (localFinally != null && flag)
							{
								localFinally(tlocal);
							}
							if (ParallelEtwProvider.Log.IsEnabled())
							{
								ParallelEtwProvider.Log.ParallelJoin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
							}
						}
					}, parallelOptions, true);
				}
				finally
				{
					if (parallelOptions.CancellationToken.CanBeCanceled)
					{
						cancellationTokenRegistration.Dispose();
					}
				}
				if (oce != null)
				{
					throw oce;
				}
			}
			catch (AggregateException ex)
			{
				Parallel.ThrowSingleCancellationExceptionOrOtherException(ex.InnerExceptions, parallelOptions.CancellationToken, ex);
			}
			finally
			{
				int loopStateFlags = sharedPStateFlags.LoopStateFlags;
				result._completed = (loopStateFlags == 0);
				if ((loopStateFlags & 2) != 0)
				{
					result._lowestBreakIteration = new long?((long)sharedPStateFlags.LowestBreakIteration);
				}
				if (ParallelEtwProvider.Log.IsEnabled())
				{
					int num;
					if (loopStateFlags == 0)
					{
						num = toExclusive - fromInclusive;
					}
					else if ((loopStateFlags & 2) != 0)
					{
						num = sharedPStateFlags.LowestBreakIteration - fromInclusive;
					}
					else
					{
						num = -1;
					}
					ParallelEtwProvider.Log.ParallelLoopEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, (long)num);
				}
			}
			return result;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00079CB4 File Offset: 0x00077EB4
		private static ParallelLoopResult ForWorker64<TLocal>(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long> body, Action<long, ParallelLoopState> bodyWithState, Func<long, ParallelLoopState, TLocal, TLocal> bodyWithLocal, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			ParallelLoopResult result = default(ParallelLoopResult);
			if (toExclusive <= fromInclusive)
			{
				result._completed = true;
				return result;
			}
			ParallelLoopStateFlags64 sharedPStateFlags = new ParallelLoopStateFlags64();
			parallelOptions.CancellationToken.ThrowIfCancellationRequested();
			int nNumExpectedWorkers = (parallelOptions.EffectiveMaxConcurrencyLevel == -1) ? PlatformHelper.ProcessorCount : parallelOptions.EffectiveMaxConcurrencyLevel;
			RangeManager rangeManager = new RangeManager(fromInclusive, toExclusive, 1L, nNumExpectedWorkers);
			OperationCanceledException oce = null;
			CancellationTokenRegistration cancellationTokenRegistration = (!parallelOptions.CancellationToken.CanBeCanceled) ? default(CancellationTokenRegistration) : parallelOptions.CancellationToken.Register(delegate(object o)
			{
				oce = new OperationCanceledException(parallelOptions.CancellationToken);
				sharedPStateFlags.Cancel();
			}, null, false);
			int forkJoinContextID = 0;
			if (ParallelEtwProvider.Log.IsEnabled())
			{
				forkJoinContextID = Interlocked.Increment(ref Parallel.s_forkJoinContextID);
				ParallelEtwProvider.Log.ParallelLoopBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelFor, fromInclusive, toExclusive);
			}
			try
			{
				try
				{
					TaskReplicator.Run<RangeWorker>(delegate(ref RangeWorker currentWorker, int timeout, out bool replicationDelegateYieldedBeforeCompletion)
					{
						if (!currentWorker.IsInitialized)
						{
							currentWorker = rangeManager.RegisterNewWorker();
						}
						replicationDelegateYieldedBeforeCompletion = false;
						long num;
						long num2;
						if (!currentWorker.FindNewWork(out num, out num2) || sharedPStateFlags.ShouldExitLoop(num))
						{
							return;
						}
						if (ParallelEtwProvider.Log.IsEnabled())
						{
							ParallelEtwProvider.Log.ParallelFork(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
						}
						TLocal tlocal = default(TLocal);
						bool flag = false;
						try
						{
							ParallelLoopState64 parallelLoopState = null;
							if (bodyWithState != null)
							{
								parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
							}
							else if (bodyWithLocal != null)
							{
								parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
								if (localInit != null)
								{
									tlocal = localInit();
									flag = true;
								}
							}
							int timeoutOccursAt = Parallel.ComputeTimeoutPoint(timeout);
							for (;;)
							{
								if (body != null)
								{
									for (long num3 = num; num3 < num2; num3 += 1L)
									{
										if (sharedPStateFlags.LoopStateFlags != 0 && sharedPStateFlags.ShouldExitLoop())
										{
											break;
										}
										body(num3);
									}
								}
								else if (bodyWithState != null)
								{
									for (long num4 = num; num4 < num2; num4 += 1L)
									{
										if (sharedPStateFlags.LoopStateFlags != 0 && sharedPStateFlags.ShouldExitLoop(num4))
										{
											break;
										}
										parallelLoopState.CurrentIteration = num4;
										bodyWithState(num4, parallelLoopState);
									}
								}
								else
								{
									long num5 = num;
									while (num5 < num2 && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(num5)))
									{
										parallelLoopState.CurrentIteration = num5;
										tlocal = bodyWithLocal(num5, parallelLoopState, tlocal);
										num5 += 1L;
									}
								}
								if (Parallel.CheckTimeoutReached(timeoutOccursAt))
								{
									break;
								}
								if (!currentWorker.FindNewWork(out num, out num2) || (sharedPStateFlags.LoopStateFlags != 0 && sharedPStateFlags.ShouldExitLoop(num)))
								{
									goto IL_1DB;
								}
							}
							replicationDelegateYieldedBeforeCompletion = true;
							IL_1DB:;
						}
						catch (Exception source)
						{
							sharedPStateFlags.SetExceptional();
							ExceptionDispatchInfo.Throw(source);
						}
						finally
						{
							if (localFinally != null && flag)
							{
								localFinally(tlocal);
							}
							if (ParallelEtwProvider.Log.IsEnabled())
							{
								ParallelEtwProvider.Log.ParallelJoin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
							}
						}
					}, parallelOptions, true);
				}
				finally
				{
					if (parallelOptions.CancellationToken.CanBeCanceled)
					{
						cancellationTokenRegistration.Dispose();
					}
				}
				if (oce != null)
				{
					throw oce;
				}
			}
			catch (AggregateException ex)
			{
				Parallel.ThrowSingleCancellationExceptionOrOtherException(ex.InnerExceptions, parallelOptions.CancellationToken, ex);
			}
			finally
			{
				int loopStateFlags = sharedPStateFlags.LoopStateFlags;
				result._completed = (loopStateFlags == 0);
				if ((loopStateFlags & 2) != 0)
				{
					result._lowestBreakIteration = new long?(sharedPStateFlags.LowestBreakIteration);
				}
				if (ParallelEtwProvider.Log.IsEnabled())
				{
					long totalIterations;
					if (loopStateFlags == 0)
					{
						totalIterations = toExclusive - fromInclusive;
					}
					else if ((loopStateFlags & 2) != 0)
					{
						totalIterations = sharedPStateFlags.LowestBreakIteration - fromInclusive;
					}
					else
					{
						totalIterations = -1L;
					}
					ParallelEtwProvider.Log.ParallelLoopEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, totalIterations);
				}
			}
			return result;
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x00079F24 File Offset: 0x00078124
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return Parallel.ForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, body, null, null, null, null, null, null);
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x00079F60 File Offset: 0x00078160
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForEachWorker<TSource, object>(source, parallelOptions, body, null, null, null, null, null, null);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x00079FA8 File Offset: 0x000781A8
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource, ParallelLoopState> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return Parallel.ForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, null, body, null, null, null, null, null);
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x00079FE4 File Offset: 0x000781E4
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForEachWorker<TSource, object>(source, parallelOptions, null, body, null, null, null, null, null);
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x0007A02C File Offset: 0x0007822C
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource, ParallelLoopState, long> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return Parallel.ForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, null, null, body, null, null, null, null);
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x0007A068 File Offset: 0x00078268
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForEachWorker<TSource, object>(source, parallelOptions, null, null, body, null, null, null, null);
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x0007A0B0 File Offset: 0x000782B0
		public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return Parallel.ForEachWorker<TSource, TLocal>(source, Parallel.s_defaultParallelOptions, null, null, null, body, null, localInit, localFinally);
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x0007A108 File Offset: 0x00078308
		public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForEachWorker<TSource, TLocal>(source, parallelOptions, null, null, null, body, null, localInit, localFinally);
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x0007A16C File Offset: 0x0007836C
		public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return Parallel.ForEachWorker<TSource, TLocal>(source, Parallel.s_defaultParallelOptions, null, null, null, null, body, localInit, localFinally);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x0007A1C4 File Offset: 0x000783C4
		public static ParallelLoopResult ForEach<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.ForEachWorker<TSource, TLocal>(source, parallelOptions, null, null, null, null, body, localInit, localFinally);
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x0007A228 File Offset: 0x00078428
		private static ParallelLoopResult ForEachWorker<TSource, TLocal>(IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource> body, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			parallelOptions.CancellationToken.ThrowIfCancellationRequested();
			TSource[] array = source as TSource[];
			if (array != null)
			{
				return Parallel.ForEachWorker<TSource, TLocal>(array, parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				return Parallel.ForEachWorker<TSource, TLocal>(list, parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
			}
			return Parallel.PartitionerForEachWorker<TSource, TLocal>(Partitioner.Create<TSource>(source), parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x0007A298 File Offset: 0x00078498
		private static ParallelLoopResult ForEachWorker<TSource, TLocal>(TSource[] array, ParallelOptions parallelOptions, Action<TSource> body, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			int lowerBound = array.GetLowerBound(0);
			int toExclusive = array.GetUpperBound(0) + 1;
			if (body != null)
			{
				return Parallel.ForWorker<object>(lowerBound, toExclusive, parallelOptions, delegate(int i)
				{
					body(array[i]);
				}, null, null, null, null);
			}
			if (bodyWithState != null)
			{
				return Parallel.ForWorker<object>(lowerBound, toExclusive, parallelOptions, null, delegate(int i, ParallelLoopState state)
				{
					bodyWithState(array[i], state);
				}, null, null, null);
			}
			if (bodyWithStateAndIndex != null)
			{
				return Parallel.ForWorker<object>(lowerBound, toExclusive, parallelOptions, null, delegate(int i, ParallelLoopState state)
				{
					bodyWithStateAndIndex(array[i], state, (long)i);
				}, null, null, null);
			}
			if (bodyWithStateAndLocal != null)
			{
				return Parallel.ForWorker<TLocal>(lowerBound, toExclusive, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithStateAndLocal(array[i], state, local), localInit, localFinally);
			}
			return Parallel.ForWorker<TLocal>(lowerBound, toExclusive, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithEverything(array[i], state, (long)i, local), localInit, localFinally);
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x0007A394 File Offset: 0x00078594
		private static ParallelLoopResult ForEachWorker<TSource, TLocal>(IList<TSource> list, ParallelOptions parallelOptions, Action<TSource> body, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			if (body != null)
			{
				return Parallel.ForWorker<object>(0, list.Count, parallelOptions, delegate(int i)
				{
					body(list[i]);
				}, null, null, null, null);
			}
			if (bodyWithState != null)
			{
				return Parallel.ForWorker<object>(0, list.Count, parallelOptions, null, delegate(int i, ParallelLoopState state)
				{
					bodyWithState(list[i], state);
				}, null, null, null);
			}
			if (bodyWithStateAndIndex != null)
			{
				return Parallel.ForWorker<object>(0, list.Count, parallelOptions, null, delegate(int i, ParallelLoopState state)
				{
					bodyWithStateAndIndex(list[i], state, (long)i);
				}, null, null, null);
			}
			if (bodyWithStateAndLocal != null)
			{
				return Parallel.ForWorker<TLocal>(0, list.Count, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithStateAndLocal(list[i], state, local), localInit, localFinally);
			}
			return Parallel.ForWorker<TLocal>(0, list.Count, parallelOptions, null, null, (int i, ParallelLoopState state, TLocal local) => bodyWithEverything(list[i], state, (long)i, local), localInit, localFinally);
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x0007A4A8 File Offset: 0x000786A8
		public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, Action<TSource> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return Parallel.PartitionerForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, body, null, null, null, null, null, null);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x0007A4E4 File Offset: 0x000786E4
		public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, Action<TSource, ParallelLoopState> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			return Parallel.PartitionerForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, null, body, null, null, null, null, null);
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x0007A520 File Offset: 0x00078720
		public static ParallelLoopResult ForEach<TSource>(OrderablePartitioner<TSource> source, Action<TSource, ParallelLoopState, long> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (!source.KeysNormalized)
			{
				throw new InvalidOperationException("This method requires the use of an OrderedPartitioner with the KeysNormalized property set to true.");
			}
			return Parallel.PartitionerForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, null, null, body, null, null, null, null);
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x0007A570 File Offset: 0x00078770
		public static ParallelLoopResult ForEach<TSource, TLocal>(Partitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			return Parallel.PartitionerForEachWorker<TSource, TLocal>(source, Parallel.s_defaultParallelOptions, null, null, null, body, null, localInit, localFinally);
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x0007A5C8 File Offset: 0x000787C8
		public static ParallelLoopResult ForEach<TSource, TLocal>(OrderablePartitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (!source.KeysNormalized)
			{
				throw new InvalidOperationException("This method requires the use of an OrderedPartitioner with the KeysNormalized property set to true.");
			}
			return Parallel.PartitionerForEachWorker<TSource, TLocal>(source, Parallel.s_defaultParallelOptions, null, null, null, null, body, localInit, localFinally);
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x0007A634 File Offset: 0x00078834
		public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.PartitionerForEachWorker<TSource, object>(source, parallelOptions, body, null, null, null, null, null, null);
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x0007A67C File Offset: 0x0007887C
		public static ParallelLoopResult ForEach<TSource>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.PartitionerForEachWorker<TSource, object>(source, parallelOptions, null, body, null, null, null, null, null);
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x0007A6C4 File Offset: 0x000788C4
		public static ParallelLoopResult ForEach<TSource>(OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			if (!source.KeysNormalized)
			{
				throw new InvalidOperationException("This method requires the use of an OrderedPartitioner with the KeysNormalized property set to true.");
			}
			return Parallel.PartitionerForEachWorker<TSource, object>(source, parallelOptions, null, null, body, null, null, null, null);
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x0007A71C File Offset: 0x0007891C
		public static ParallelLoopResult ForEach<TSource, TLocal>(Partitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			return Parallel.PartitionerForEachWorker<TSource, TLocal>(source, parallelOptions, null, null, null, body, null, localInit, localFinally);
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x0007A780 File Offset: 0x00078980
		public static ParallelLoopResult ForEach<TSource, TLocal>(OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (localInit == null)
			{
				throw new ArgumentNullException("localInit");
			}
			if (localFinally == null)
			{
				throw new ArgumentNullException("localFinally");
			}
			if (parallelOptions == null)
			{
				throw new ArgumentNullException("parallelOptions");
			}
			if (!source.KeysNormalized)
			{
				throw new InvalidOperationException("This method requires the use of an OrderedPartitioner with the KeysNormalized property set to true.");
			}
			return Parallel.PartitionerForEachWorker<TSource, TLocal>(source, parallelOptions, null, null, null, null, body, localInit, localFinally);
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x0007A7F8 File Offset: 0x000789F8
		private static ParallelLoopResult PartitionerForEachWorker<TSource, TLocal>(Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource> simpleBody, Action<TSource, ParallelLoopState> bodyWithState, Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything, Func<TLocal> localInit, Action<TLocal> localFinally)
		{
			OrderablePartitioner<TSource> orderedSource = source as OrderablePartitioner<TSource>;
			if (!source.SupportsDynamicPartitions)
			{
				throw new InvalidOperationException("The Partitioner used here must support dynamic partitioning.");
			}
			parallelOptions.CancellationToken.ThrowIfCancellationRequested();
			int forkJoinContextID = 0;
			if (ParallelEtwProvider.Log.IsEnabled())
			{
				forkJoinContextID = Interlocked.Increment(ref Parallel.s_forkJoinContextID);
				ParallelEtwProvider.Log.ParallelLoopBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelForEach, 0L, 0L);
			}
			ParallelLoopStateFlags64 sharedPStateFlags = new ParallelLoopStateFlags64();
			ParallelLoopResult result = default(ParallelLoopResult);
			OperationCanceledException oce = null;
			CancellationTokenRegistration cancellationTokenRegistration = (!parallelOptions.CancellationToken.CanBeCanceled) ? default(CancellationTokenRegistration) : parallelOptions.CancellationToken.Register(delegate(object o)
			{
				oce = new OperationCanceledException(parallelOptions.CancellationToken);
				sharedPStateFlags.Cancel();
			}, null, false);
			IEnumerable<TSource> partitionerSource = null;
			IEnumerable<KeyValuePair<long, TSource>> orderablePartitionerSource = null;
			if (orderedSource != null)
			{
				orderablePartitionerSource = orderedSource.GetOrderableDynamicPartitions();
				if (orderablePartitionerSource == null)
				{
					throw new InvalidOperationException("The Partitioner used here returned a null partitioner source.");
				}
			}
			else
			{
				partitionerSource = source.GetDynamicPartitions();
				if (partitionerSource == null)
				{
					throw new InvalidOperationException("The Partitioner used here returned a null partitioner source.");
				}
			}
			try
			{
				try
				{
					TaskReplicator.Run<IEnumerator>(delegate(ref IEnumerator partitionState, int timeout, out bool replicationDelegateYieldedBeforeCompletion)
					{
						replicationDelegateYieldedBeforeCompletion = false;
						if (ParallelEtwProvider.Log.IsEnabled())
						{
							ParallelEtwProvider.Log.ParallelFork(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
						}
						TLocal tlocal = default(TLocal);
						bool flag = false;
						try
						{
							ParallelLoopState64 parallelLoopState = null;
							if (bodyWithState != null || bodyWithStateAndIndex != null)
							{
								parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
							}
							else if (bodyWithStateAndLocal != null || bodyWithEverything != null)
							{
								parallelLoopState = new ParallelLoopState64(sharedPStateFlags);
								if (localInit != null)
								{
									tlocal = localInit();
									flag = true;
								}
							}
							int timeoutOccursAt = Parallel.ComputeTimeoutPoint(timeout);
							if (orderedSource != null)
							{
								IEnumerator<KeyValuePair<long, TSource>> enumerator = partitionState as IEnumerator<KeyValuePair<long, TSource>>;
								if (enumerator == null)
								{
									enumerator = orderablePartitionerSource.GetEnumerator();
									partitionState = enumerator;
								}
								if (enumerator == null)
								{
									throw new InvalidOperationException("The Partitioner source returned a null enumerator.");
								}
								while (enumerator.MoveNext())
								{
									KeyValuePair<long, TSource> keyValuePair = enumerator.Current;
									long key = keyValuePair.Key;
									TSource value = keyValuePair.Value;
									if (parallelLoopState != null)
									{
										parallelLoopState.CurrentIteration = key;
									}
									if (simpleBody != null)
									{
										simpleBody(value);
									}
									else if (bodyWithState != null)
									{
										bodyWithState(value, parallelLoopState);
									}
									else if (bodyWithStateAndIndex != null)
									{
										bodyWithStateAndIndex(value, parallelLoopState, key);
									}
									else if (bodyWithStateAndLocal != null)
									{
										tlocal = bodyWithStateAndLocal(value, parallelLoopState, tlocal);
									}
									else
									{
										tlocal = bodyWithEverything(value, parallelLoopState, key, tlocal);
									}
									if (sharedPStateFlags.ShouldExitLoop(key))
									{
										break;
									}
									if (Parallel.CheckTimeoutReached(timeoutOccursAt))
									{
										replicationDelegateYieldedBeforeCompletion = true;
										break;
									}
								}
							}
							else
							{
								IEnumerator<TSource> enumerator2 = partitionState as IEnumerator<!0>;
								if (enumerator2 == null)
								{
									enumerator2 = partitionerSource.GetEnumerator();
									partitionState = enumerator2;
								}
								if (enumerator2 == null)
								{
									throw new InvalidOperationException("The Partitioner source returned a null enumerator.");
								}
								if (parallelLoopState != null)
								{
									parallelLoopState.CurrentIteration = 0L;
								}
								while (enumerator2.MoveNext())
								{
									TSource tsource = enumerator2.Current;
									if (simpleBody != null)
									{
										simpleBody(tsource);
									}
									else if (bodyWithState != null)
									{
										bodyWithState(tsource, parallelLoopState);
									}
									else if (bodyWithStateAndLocal != null)
									{
										tlocal = bodyWithStateAndLocal(tsource, parallelLoopState, tlocal);
									}
									if (sharedPStateFlags.LoopStateFlags != 0)
									{
										break;
									}
									if (Parallel.CheckTimeoutReached(timeoutOccursAt))
									{
										replicationDelegateYieldedBeforeCompletion = true;
										break;
									}
								}
							}
						}
						catch (Exception source2)
						{
							sharedPStateFlags.SetExceptional();
							ExceptionDispatchInfo.Throw(source2);
						}
						finally
						{
							if (localFinally != null && flag)
							{
								localFinally(tlocal);
							}
							if (!replicationDelegateYieldedBeforeCompletion)
							{
								IDisposable disposable2 = partitionState as IDisposable;
								if (disposable2 != null)
								{
									disposable2.Dispose();
								}
							}
							if (ParallelEtwProvider.Log.IsEnabled())
							{
								ParallelEtwProvider.Log.ParallelJoin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
							}
						}
					}, parallelOptions, true);
				}
				finally
				{
					if (parallelOptions.CancellationToken.CanBeCanceled)
					{
						cancellationTokenRegistration.Dispose();
					}
				}
				if (oce != null)
				{
					throw oce;
				}
			}
			catch (AggregateException ex)
			{
				Parallel.ThrowSingleCancellationExceptionOrOtherException(ex.InnerExceptions, parallelOptions.CancellationToken, ex);
			}
			finally
			{
				int loopStateFlags = sharedPStateFlags.LoopStateFlags;
				result._completed = (loopStateFlags == 0);
				if ((loopStateFlags & 2) != 0)
				{
					result._lowestBreakIteration = new long?(sharedPStateFlags.LowestBreakIteration);
				}
				IDisposable disposable;
				if (orderablePartitionerSource != null)
				{
					disposable = (orderablePartitionerSource as IDisposable);
				}
				else
				{
					disposable = (partitionerSource as IDisposable);
				}
				if (disposable != null)
				{
					disposable.Dispose();
				}
				if (ParallelEtwProvider.Log.IsEnabled())
				{
					ParallelEtwProvider.Log.ParallelLoopEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, 0L);
				}
			}
			return result;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0007AAB4 File Offset: 0x00078CB4
		private static OperationCanceledException ReduceToSingleCancellationException(ICollection exceptions, CancellationToken cancelToken)
		{
			if (exceptions == null || exceptions.Count == 0)
			{
				return null;
			}
			if (!cancelToken.IsCancellationRequested)
			{
				return null;
			}
			Exception ex = null;
			foreach (object obj in exceptions)
			{
				Exception ex2 = (Exception)obj;
				if (ex == null)
				{
					ex = ex2;
				}
				OperationCanceledException ex3 = ex2 as OperationCanceledException;
				if (ex3 == null || !cancelToken.Equals(ex3.CancellationToken))
				{
					return null;
				}
			}
			return (OperationCanceledException)ex;
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x0007AB4C File Offset: 0x00078D4C
		private static void ThrowSingleCancellationExceptionOrOtherException(ICollection exceptions, CancellationToken cancelToken, Exception otherException)
		{
			ExceptionDispatchInfo.Throw(Parallel.ReduceToSingleCancellationException(exceptions, cancelToken) ?? otherException);
		}

		// Token: 0x04001BEC RID: 7148
		internal static int s_forkJoinContextID;

		// Token: 0x04001BED RID: 7149
		internal const int DEFAULT_LOOP_STRIDE = 16;

		// Token: 0x04001BEE RID: 7150
		internal static readonly ParallelOptions s_defaultParallelOptions = new ParallelOptions();
	}
}
