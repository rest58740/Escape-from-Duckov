using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks.Internal;
using UnityEngine;
using UnityEngine.Events;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000039 RID: 57
	[AsyncMethodBuilder(typeof(AsyncUniTaskMethodBuilder))]
	[StructLayout(LayoutKind.Auto)]
	public readonly struct UniTask
	{
		// Token: 0x060000ED RID: 237 RVA: 0x00004563 File Offset: 0x00002763
		public static IEnumerator ToCoroutine(Func<UniTask> taskFactory)
		{
			return taskFactory().ToCoroutine(null);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004571 File Offset: 0x00002771
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UniTask(IUniTaskSource source, short token)
		{
			this.source = source;
			this.token = token;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004581 File Offset: 0x00002781
		public UniTaskStatus Status
		{
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (this.source == null)
				{
					return UniTaskStatus.Succeeded;
				}
				return this.source.GetStatus(this.token);
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000459E File Offset: 0x0000279E
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UniTask.Awaiter GetAwaiter()
		{
			return new UniTask.Awaiter(ref this);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000045A8 File Offset: 0x000027A8
		public UniTask<bool> SuppressCancellationThrow()
		{
			UniTaskStatus status = this.Status;
			if (status == UniTaskStatus.Succeeded)
			{
				return CompletedTasks.False;
			}
			if (status == UniTaskStatus.Canceled)
			{
				return CompletedTasks.True;
			}
			return new UniTask<bool>(new UniTask.IsCanceledSource(this.source), this.token);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000045E8 File Offset: 0x000027E8
		public static implicit operator ValueTask(in UniTask self)
		{
			if (self.source == null)
			{
				return default(ValueTask);
			}
			return new ValueTask(self.source, self.token);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004618 File Offset: 0x00002818
		public override string ToString()
		{
			if (this.source == null)
			{
				return "()";
			}
			return "(" + this.source.UnsafeGetStatus().ToString() + ")";
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000465B File Offset: 0x0000285B
		public UniTask Preserve()
		{
			if (this.source == null)
			{
				return this;
			}
			return new UniTask(new UniTask.MemoizeSource(this.source), this.token);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004684 File Offset: 0x00002884
		public UniTask<AsyncUnit> AsAsyncUnitUniTask()
		{
			if (this.source == null)
			{
				return CompletedTasks.AsyncUnit;
			}
			if (this.source.GetStatus(this.token).IsCompletedSuccessfully())
			{
				this.source.GetResult(this.token);
				return CompletedTasks.AsyncUnit;
			}
			IUniTaskSource<AsyncUnit> uniTaskSource = this.source as IUniTaskSource<AsyncUnit>;
			if (uniTaskSource != null)
			{
				return new UniTask<AsyncUnit>(uniTaskSource, this.token);
			}
			return new UniTask<AsyncUnit>(new UniTask.AsyncUnitSource(this.source), this.token);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004700 File Offset: 0x00002900
		public static YieldAwaitable Yield()
		{
			return new YieldAwaitable(PlayerLoopTiming.Update);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004708 File Offset: 0x00002908
		public static YieldAwaitable Yield(PlayerLoopTiming timing)
		{
			return new YieldAwaitable(timing);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004710 File Offset: 0x00002910
		public static UniTask Yield(CancellationToken cancellationToken, bool cancelImmediately = false)
		{
			short num;
			return new UniTask(UniTask.YieldPromise.Create(PlayerLoopTiming.Update, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004730 File Offset: 0x00002930
		public static UniTask Yield(PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately = false)
		{
			short num;
			return new UniTask(UniTask.YieldPromise.Create(timing, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004750 File Offset: 0x00002950
		public static UniTask NextFrame()
		{
			short num;
			return new UniTask(UniTask.NextFramePromise.Create(PlayerLoopTiming.Update, CancellationToken.None, false, out num), num);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004774 File Offset: 0x00002974
		public static UniTask NextFrame(PlayerLoopTiming timing)
		{
			short num;
			return new UniTask(UniTask.NextFramePromise.Create(timing, CancellationToken.None, false, out num), num);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004798 File Offset: 0x00002998
		public static UniTask NextFrame(CancellationToken cancellationToken, bool cancelImmediately = false)
		{
			short num;
			return new UniTask(UniTask.NextFramePromise.Create(PlayerLoopTiming.Update, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000047B8 File Offset: 0x000029B8
		public static UniTask NextFrame(PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately = false)
		{
			short num;
			return new UniTask(UniTask.NextFramePromise.Create(timing, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000047D5 File Offset: 0x000029D5
		[Obsolete("Use WaitForEndOfFrame(MonoBehaviour) instead or UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate). Equivalent for coroutine's WaitForEndOfFrame requires MonoBehaviour(runner of Coroutine).")]
		public static YieldAwaitable WaitForEndOfFrame()
		{
			return UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000047DE File Offset: 0x000029DE
		[Obsolete("Use WaitForEndOfFrame(MonoBehaviour) instead or UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate). Equivalent for coroutine's WaitForEndOfFrame requires MonoBehaviour(runner of Coroutine).")]
		public static UniTask WaitForEndOfFrame(CancellationToken cancellationToken, bool cancelImmediately = false)
		{
			return UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000047EC File Offset: 0x000029EC
		public static UniTask WaitForEndOfFrame(MonoBehaviour coroutineRunner)
		{
			short num;
			return new UniTask(UniTask.WaitForEndOfFramePromise.Create(coroutineRunner, CancellationToken.None, false, out num), num);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004810 File Offset: 0x00002A10
		public static UniTask WaitForEndOfFrame(MonoBehaviour coroutineRunner, CancellationToken cancellationToken, bool cancelImmediately = false)
		{
			short num;
			return new UniTask(UniTask.WaitForEndOfFramePromise.Create(coroutineRunner, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000482D File Offset: 0x00002A2D
		public static YieldAwaitable WaitForFixedUpdate()
		{
			return UniTask.Yield(PlayerLoopTiming.LastFixedUpdate);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004835 File Offset: 0x00002A35
		public static UniTask WaitForFixedUpdate(CancellationToken cancellationToken, bool cancelImmediately = false)
		{
			return UniTask.Yield(PlayerLoopTiming.LastFixedUpdate, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000483F File Offset: 0x00002A3F
		public static UniTask WaitForSeconds(float duration, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			return UniTask.Delay(Mathf.RoundToInt(1000f * duration), ignoreTimeScale, delayTiming, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004857 File Offset: 0x00002A57
		public static UniTask WaitForSeconds(int duration, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			return UniTask.Delay(1000 * duration, ignoreTimeScale, delayTiming, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000486C File Offset: 0x00002A6C
		public static UniTask DelayFrame(int delayFrameCount, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			if (delayFrameCount < 0)
			{
				throw new ArgumentOutOfRangeException("Delay does not allow minus delayFrameCount. delayFrameCount:" + delayFrameCount.ToString());
			}
			short num;
			return new UniTask(UniTask.DelayFramePromise.Create(delayFrameCount, delayTiming, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000048A5 File Offset: 0x00002AA5
		public static UniTask Delay(int millisecondsDelay, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			return UniTask.Delay(TimeSpan.FromMilliseconds((double)millisecondsDelay), ignoreTimeScale, delayTiming, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000048B8 File Offset: 0x00002AB8
		public static UniTask Delay(TimeSpan delayTimeSpan, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			DelayType delayType = ignoreTimeScale ? DelayType.UnscaledDeltaTime : DelayType.DeltaTime;
			return UniTask.Delay(delayTimeSpan, delayType, delayTiming, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000048D8 File Offset: 0x00002AD8
		public static UniTask Delay(int millisecondsDelay, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			return UniTask.Delay(TimeSpan.FromMilliseconds((double)millisecondsDelay), delayType, delayTiming, cancellationToken, cancelImmediately);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000048EC File Offset: 0x00002AEC
		public static UniTask Delay(TimeSpan delayTimeSpan, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			if (delayTimeSpan < TimeSpan.Zero)
			{
				string str = "Delay does not allow minus delayTimeSpan. delayTimeSpan:";
				TimeSpan timeSpan = delayTimeSpan;
				throw new ArgumentOutOfRangeException(str + timeSpan.ToString());
			}
			switch (delayType)
			{
			case DelayType.UnscaledDeltaTime:
			{
				short num;
				return new UniTask(UniTask.DelayIgnoreTimeScalePromise.Create(delayTimeSpan, delayTiming, cancellationToken, cancelImmediately, out num), num);
			}
			case DelayType.Realtime:
			{
				short num2;
				return new UniTask(UniTask.DelayRealtimePromise.Create(delayTimeSpan, delayTiming, cancellationToken, cancelImmediately, out num2), num2);
			}
			}
			short num3;
			return new UniTask(UniTask.DelayPromise.Create(delayTimeSpan, delayTiming, cancellationToken, cancelImmediately, out num3), num3);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004974 File Offset: 0x00002B74
		public static UniTask FromException(Exception ex)
		{
			OperationCanceledException ex2 = ex as OperationCanceledException;
			if (ex2 != null)
			{
				return UniTask.FromCanceled(ex2.CancellationToken);
			}
			return new UniTask(new UniTask.ExceptionResultSource(ex), 0);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000049A4 File Offset: 0x00002BA4
		public static UniTask<T> FromException<T>(Exception ex)
		{
			OperationCanceledException ex2 = ex as OperationCanceledException;
			if (ex2 != null)
			{
				return UniTask.FromCanceled<T>(ex2.CancellationToken);
			}
			return new UniTask<T>(new UniTask.ExceptionResultSource<T>(ex), 0);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000049D3 File Offset: 0x00002BD3
		public static UniTask<T> FromResult<T>(T value)
		{
			return new UniTask<T>(value);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000049DB File Offset: 0x00002BDB
		public static UniTask FromCanceled(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken == CancellationToken.None)
			{
				return UniTask.CanceledUniTask;
			}
			return new UniTask(new UniTask.CanceledResultSource(cancellationToken), 0);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000049FC File Offset: 0x00002BFC
		public static UniTask<T> FromCanceled<T>(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken == CancellationToken.None)
			{
				return UniTask.CanceledUniTaskCache<T>.Task;
			}
			return new UniTask<T>(new UniTask.CanceledResultSource<T>(cancellationToken), 0);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004A1D File Offset: 0x00002C1D
		public static UniTask Create(Func<UniTask> factory)
		{
			return factory();
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004A25 File Offset: 0x00002C25
		public static UniTask Create(Func<CancellationToken, UniTask> factory, CancellationToken cancellationToken)
		{
			return factory(cancellationToken);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004A2E File Offset: 0x00002C2E
		public static UniTask Create<T>(T state, Func<T, UniTask> factory)
		{
			return factory(state);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004A37 File Offset: 0x00002C37
		public static UniTask<T> Create<T>(Func<UniTask<T>> factory)
		{
			return factory();
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004A3F File Offset: 0x00002C3F
		public static AsyncLazy Lazy(Func<UniTask> factory)
		{
			return new AsyncLazy(factory);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004A47 File Offset: 0x00002C47
		public static AsyncLazy<T> Lazy<T>(Func<UniTask<T>> factory)
		{
			return new AsyncLazy<T>(factory);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004A50 File Offset: 0x00002C50
		public static void Void(Func<UniTaskVoid> asyncAction)
		{
			asyncAction().Forget();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004A6C File Offset: 0x00002C6C
		public static void Void(Func<CancellationToken, UniTaskVoid> asyncAction, CancellationToken cancellationToken)
		{
			asyncAction(cancellationToken).Forget();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004A88 File Offset: 0x00002C88
		public static void Void<T>(Func<T, UniTaskVoid> asyncAction, T state)
		{
			asyncAction(state).Forget();
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004AA4 File Offset: 0x00002CA4
		public static Action Action(Func<UniTaskVoid> asyncAction)
		{
			return delegate()
			{
				asyncAction().Forget();
			};
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00004ABD File Offset: 0x00002CBD
		public static Action Action(Func<CancellationToken, UniTaskVoid> asyncAction, CancellationToken cancellationToken)
		{
			return delegate()
			{
				asyncAction(cancellationToken).Forget();
			};
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004ADD File Offset: 0x00002CDD
		public static Action Action<T>(T state, Func<T, UniTaskVoid> asyncAction)
		{
			return delegate()
			{
				asyncAction(state).Forget();
			};
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00004AFD File Offset: 0x00002CFD
		public static UnityAction UnityAction(Func<UniTaskVoid> asyncAction)
		{
			return delegate()
			{
				asyncAction().Forget();
			};
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00004B16 File Offset: 0x00002D16
		public static UnityAction UnityAction(Func<CancellationToken, UniTaskVoid> asyncAction, CancellationToken cancellationToken)
		{
			return delegate()
			{
				asyncAction(cancellationToken).Forget();
			};
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004B36 File Offset: 0x00002D36
		public static UnityAction UnityAction<T>(T state, Func<T, UniTaskVoid> asyncAction)
		{
			return delegate()
			{
				asyncAction(state).Forget();
			};
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00004B56 File Offset: 0x00002D56
		public static UnityAction<T> UnityAction<T>(Func<T, UniTaskVoid> asyncAction)
		{
			return delegate(T arg)
			{
				asyncAction(arg).Forget();
			};
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00004B6F File Offset: 0x00002D6F
		public static UnityAction<T0, T1> UnityAction<T0, T1>(Func<T0, T1, UniTaskVoid> asyncAction)
		{
			return delegate(T0 arg0, T1 arg1)
			{
				asyncAction(arg0, arg1).Forget();
			};
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00004B88 File Offset: 0x00002D88
		public static UnityAction<T0, T1, T2> UnityAction<T0, T1, T2>(Func<T0, T1, T2, UniTaskVoid> asyncAction)
		{
			return delegate(T0 arg0, T1 arg1, T2 arg2)
			{
				asyncAction(arg0, arg1, arg2).Forget();
			};
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004BA1 File Offset: 0x00002DA1
		public static UnityAction<T0, T1, T2, T3> UnityAction<T0, T1, T2, T3>(Func<T0, T1, T2, T3, UniTaskVoid> asyncAction)
		{
			return delegate(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
			{
				asyncAction(arg0, arg1, arg2, arg3).Forget();
			};
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00004BBA File Offset: 0x00002DBA
		public static UnityAction<T> UnityAction<T>(Func<T, CancellationToken, UniTaskVoid> asyncAction, CancellationToken cancellationToken)
		{
			return delegate(T arg)
			{
				asyncAction(arg, cancellationToken).Forget();
			};
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004BDA File Offset: 0x00002DDA
		public static UnityAction<T0, T1> UnityAction<T0, T1>(Func<T0, T1, CancellationToken, UniTaskVoid> asyncAction, CancellationToken cancellationToken)
		{
			return delegate(T0 arg0, T1 arg1)
			{
				asyncAction(arg0, arg1, cancellationToken).Forget();
			};
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00004BFA File Offset: 0x00002DFA
		public static UnityAction<T0, T1, T2> UnityAction<T0, T1, T2>(Func<T0, T1, T2, CancellationToken, UniTaskVoid> asyncAction, CancellationToken cancellationToken)
		{
			return delegate(T0 arg0, T1 arg1, T2 arg2)
			{
				asyncAction(arg0, arg1, arg2, cancellationToken).Forget();
			};
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00004C1A File Offset: 0x00002E1A
		public static UnityAction<T0, T1, T2, T3> UnityAction<T0, T1, T2, T3>(Func<T0, T1, T2, T3, CancellationToken, UniTaskVoid> asyncAction, CancellationToken cancellationToken)
		{
			return delegate(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
			{
				asyncAction(arg0, arg1, arg2, arg3, cancellationToken).Forget();
			};
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00004C3A File Offset: 0x00002E3A
		public static UniTask Defer(Func<UniTask> factory)
		{
			return new UniTask(new UniTask.DeferPromise(factory), 0);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004C48 File Offset: 0x00002E48
		public static UniTask<T> Defer<T>(Func<UniTask<T>> factory)
		{
			return new UniTask<T>(new UniTask.DeferPromise<T>(factory), 0);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004C56 File Offset: 0x00002E56
		public static UniTask Defer<TState>(TState state, Func<TState, UniTask> factory)
		{
			return new UniTask(new UniTask.DeferPromiseWithState<TState>(state, factory), 0);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004C65 File Offset: 0x00002E65
		public static UniTask<TResult> Defer<TState, TResult>(TState state, Func<TState, UniTask<TResult>> factory)
		{
			return new UniTask<TResult>(new UniTask.DeferPromiseWithState<TState, TResult>(state, factory), 0);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004C74 File Offset: 0x00002E74
		public static UniTask Never(CancellationToken cancellationToken)
		{
			return new UniTask<AsyncUnit>(new UniTask.NeverPromise<AsyncUnit>(cancellationToken), 0);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004C87 File Offset: 0x00002E87
		public static UniTask<T> Never<T>(CancellationToken cancellationToken)
		{
			return new UniTask<T>(new UniTask.NeverPromise<T>(cancellationToken), 0);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004C95 File Offset: 0x00002E95
		[Obsolete("UniTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use UniTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use UniTask.Void(async void) or UniTask.Create(async UniTask) too.")]
		public static UniTask Run(Action action, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return UniTask.RunOnThreadPool(action, configureAwait, cancellationToken);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004C9F File Offset: 0x00002E9F
		[Obsolete("UniTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use UniTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use UniTask.Void(async void) or UniTask.Create(async UniTask) too.")]
		public static UniTask Run(Action<object> action, object state, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return UniTask.RunOnThreadPool(action, state, configureAwait, cancellationToken);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00004CAA File Offset: 0x00002EAA
		[Obsolete("UniTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use UniTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use UniTask.Void(async void) or UniTask.Create(async UniTask) too.")]
		public static UniTask Run(Func<UniTask> action, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return UniTask.RunOnThreadPool(action, configureAwait, cancellationToken);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004CB4 File Offset: 0x00002EB4
		[Obsolete("UniTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use UniTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use UniTask.Void(async void) or UniTask.Create(async UniTask) too.")]
		public static UniTask Run(Func<object, UniTask> action, object state, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return UniTask.RunOnThreadPool(action, state, configureAwait, cancellationToken);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00004CBF File Offset: 0x00002EBF
		[Obsolete("UniTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use UniTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use UniTask.Void(async void) or UniTask.Create(async UniTask) too.")]
		public static UniTask<T> Run<T>(Func<T> func, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return UniTask.RunOnThreadPool<T>(func, configureAwait, cancellationToken);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00004CC9 File Offset: 0x00002EC9
		[Obsolete("UniTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use UniTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use UniTask.Void(async void) or UniTask.Create(async UniTask) too.")]
		public static UniTask<T> Run<T>(Func<UniTask<T>> func, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return UniTask.RunOnThreadPool<T>(func, configureAwait, cancellationToken);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004CD3 File Offset: 0x00002ED3
		[Obsolete("UniTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use UniTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use UniTask.Void(async void) or UniTask.Create(async UniTask) too.")]
		public static UniTask<T> Run<T>(Func<object, T> func, object state, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return UniTask.RunOnThreadPool<T>(func, state, configureAwait, cancellationToken);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00004CDE File Offset: 0x00002EDE
		[Obsolete("UniTask.Run is similar as Task.Run, it uses ThreadPool. For equivalent behaviour, use UniTask.RunOnThreadPool instead. If you don't want to use ThreadPool, you can use UniTask.Void(async void) or UniTask.Create(async UniTask) too.")]
		public static UniTask<T> Run<T>(Func<object, UniTask<T>> func, object state, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return UniTask.RunOnThreadPool<T>(func, state, configureAwait, cancellationToken);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00004CEC File Offset: 0x00002EEC
		public static UniTask RunOnThreadPool(Action action, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			UniTask.<RunOnThreadPool>d__98 <RunOnThreadPool>d__;
			<RunOnThreadPool>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RunOnThreadPool>d__.action = action;
			<RunOnThreadPool>d__.configureAwait = configureAwait;
			<RunOnThreadPool>d__.cancellationToken = cancellationToken;
			<RunOnThreadPool>d__.<>1__state = -1;
			<RunOnThreadPool>d__.<>t__builder.Start<UniTask.<RunOnThreadPool>d__98>(ref <RunOnThreadPool>d__);
			return <RunOnThreadPool>d__.<>t__builder.Task;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00004D40 File Offset: 0x00002F40
		public static UniTask RunOnThreadPool(Action<object> action, object state, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			UniTask.<RunOnThreadPool>d__99 <RunOnThreadPool>d__;
			<RunOnThreadPool>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RunOnThreadPool>d__.action = action;
			<RunOnThreadPool>d__.state = state;
			<RunOnThreadPool>d__.configureAwait = configureAwait;
			<RunOnThreadPool>d__.cancellationToken = cancellationToken;
			<RunOnThreadPool>d__.<>1__state = -1;
			<RunOnThreadPool>d__.<>t__builder.Start<UniTask.<RunOnThreadPool>d__99>(ref <RunOnThreadPool>d__);
			return <RunOnThreadPool>d__.<>t__builder.Task;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00004D9C File Offset: 0x00002F9C
		public static UniTask RunOnThreadPool(Func<UniTask> action, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			UniTask.<RunOnThreadPool>d__100 <RunOnThreadPool>d__;
			<RunOnThreadPool>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RunOnThreadPool>d__.action = action;
			<RunOnThreadPool>d__.configureAwait = configureAwait;
			<RunOnThreadPool>d__.cancellationToken = cancellationToken;
			<RunOnThreadPool>d__.<>1__state = -1;
			<RunOnThreadPool>d__.<>t__builder.Start<UniTask.<RunOnThreadPool>d__100>(ref <RunOnThreadPool>d__);
			return <RunOnThreadPool>d__.<>t__builder.Task;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00004DF0 File Offset: 0x00002FF0
		public static UniTask RunOnThreadPool(Func<object, UniTask> action, object state, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			UniTask.<RunOnThreadPool>d__101 <RunOnThreadPool>d__;
			<RunOnThreadPool>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<RunOnThreadPool>d__.action = action;
			<RunOnThreadPool>d__.state = state;
			<RunOnThreadPool>d__.configureAwait = configureAwait;
			<RunOnThreadPool>d__.cancellationToken = cancellationToken;
			<RunOnThreadPool>d__.<>1__state = -1;
			<RunOnThreadPool>d__.<>t__builder.Start<UniTask.<RunOnThreadPool>d__101>(ref <RunOnThreadPool>d__);
			return <RunOnThreadPool>d__.<>t__builder.Task;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00004E4C File Offset: 0x0000304C
		public static UniTask<T> RunOnThreadPool<T>(Func<T> func, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			UniTask.<RunOnThreadPool>d__102<T> <RunOnThreadPool>d__;
			<RunOnThreadPool>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<RunOnThreadPool>d__.func = func;
			<RunOnThreadPool>d__.configureAwait = configureAwait;
			<RunOnThreadPool>d__.cancellationToken = cancellationToken;
			<RunOnThreadPool>d__.<>1__state = -1;
			<RunOnThreadPool>d__.<>t__builder.Start<UniTask.<RunOnThreadPool>d__102<T>>(ref <RunOnThreadPool>d__);
			return <RunOnThreadPool>d__.<>t__builder.Task;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00004EA0 File Offset: 0x000030A0
		public static UniTask<T> RunOnThreadPool<T>(Func<UniTask<T>> func, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			UniTask.<RunOnThreadPool>d__103<T> <RunOnThreadPool>d__;
			<RunOnThreadPool>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<RunOnThreadPool>d__.func = func;
			<RunOnThreadPool>d__.configureAwait = configureAwait;
			<RunOnThreadPool>d__.cancellationToken = cancellationToken;
			<RunOnThreadPool>d__.<>1__state = -1;
			<RunOnThreadPool>d__.<>t__builder.Start<UniTask.<RunOnThreadPool>d__103<T>>(ref <RunOnThreadPool>d__);
			return <RunOnThreadPool>d__.<>t__builder.Task;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004EF4 File Offset: 0x000030F4
		public static UniTask<T> RunOnThreadPool<T>(Func<object, T> func, object state, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			UniTask.<RunOnThreadPool>d__104<T> <RunOnThreadPool>d__;
			<RunOnThreadPool>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<RunOnThreadPool>d__.func = func;
			<RunOnThreadPool>d__.state = state;
			<RunOnThreadPool>d__.configureAwait = configureAwait;
			<RunOnThreadPool>d__.cancellationToken = cancellationToken;
			<RunOnThreadPool>d__.<>1__state = -1;
			<RunOnThreadPool>d__.<>t__builder.Start<UniTask.<RunOnThreadPool>d__104<T>>(ref <RunOnThreadPool>d__);
			return <RunOnThreadPool>d__.<>t__builder.Task;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004F50 File Offset: 0x00003150
		public static UniTask<T> RunOnThreadPool<T>(Func<object, UniTask<T>> func, object state, bool configureAwait = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			UniTask.<RunOnThreadPool>d__105<T> <RunOnThreadPool>d__;
			<RunOnThreadPool>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<RunOnThreadPool>d__.func = func;
			<RunOnThreadPool>d__.state = state;
			<RunOnThreadPool>d__.configureAwait = configureAwait;
			<RunOnThreadPool>d__.cancellationToken = cancellationToken;
			<RunOnThreadPool>d__.<>1__state = -1;
			<RunOnThreadPool>d__.<>t__builder.Start<UniTask.<RunOnThreadPool>d__105<T>>(ref <RunOnThreadPool>d__);
			return <RunOnThreadPool>d__.<>t__builder.Task;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004FAB File Offset: 0x000031AB
		public static SwitchToMainThreadAwaitable SwitchToMainThread(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SwitchToMainThreadAwaitable(PlayerLoopTiming.Update, cancellationToken);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00004FB4 File Offset: 0x000031B4
		public static SwitchToMainThreadAwaitable SwitchToMainThread(PlayerLoopTiming timing, CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SwitchToMainThreadAwaitable(timing, cancellationToken);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00004FBD File Offset: 0x000031BD
		public static ReturnToMainThread ReturnToMainThread(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ReturnToMainThread(PlayerLoopTiming.Update, cancellationToken);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00004FC6 File Offset: 0x000031C6
		public static ReturnToMainThread ReturnToMainThread(PlayerLoopTiming timing, CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ReturnToMainThread(timing, cancellationToken);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00004FCF File Offset: 0x000031CF
		public static void Post(Action action, PlayerLoopTiming timing = PlayerLoopTiming.Update)
		{
			PlayerLoopHelper.AddContinuation(timing, action);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00004FD8 File Offset: 0x000031D8
		public static SwitchToThreadPoolAwaitable SwitchToThreadPool()
		{
			return default(SwitchToThreadPoolAwaitable);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00004FF0 File Offset: 0x000031F0
		public static SwitchToTaskPoolAwaitable SwitchToTaskPool()
		{
			return default(SwitchToTaskPoolAwaitable);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005006 File Offset: 0x00003206
		public static SwitchToSynchronizationContextAwaitable SwitchToSynchronizationContext(SynchronizationContext synchronizationContext, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<SynchronizationContext>(synchronizationContext, "synchronizationContext");
			return new SwitchToSynchronizationContextAwaitable(synchronizationContext, cancellationToken);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000501A File Offset: 0x0000321A
		public static ReturnToSynchronizationContext ReturnToSynchronizationContext(SynchronizationContext synchronizationContext, CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ReturnToSynchronizationContext(synchronizationContext, false, cancellationToken);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005024 File Offset: 0x00003224
		public static ReturnToSynchronizationContext ReturnToCurrentSynchronizationContext(bool dontPostWhenSameContext = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			return new ReturnToSynchronizationContext(SynchronizationContext.Current, dontPostWhenSameContext, cancellationToken);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005034 File Offset: 0x00003234
		public static UniTask WaitUntil(Func<bool> predicate, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			short num;
			return new UniTask(UniTask.WaitUntilPromise.Create(predicate, timing, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005054 File Offset: 0x00003254
		public static UniTask WaitUntil<T>(T state, Func<T, bool> predicate, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			short num;
			return new UniTask(UniTask.WaitUntilPromise<T>.Create(state, predicate, timing, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005074 File Offset: 0x00003274
		public static UniTask WaitWhile(Func<bool> predicate, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			short num;
			return new UniTask(UniTask.WaitWhilePromise.Create(predicate, timing, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005094 File Offset: 0x00003294
		public static UniTask WaitWhile<T>(T state, Func<T, bool> predicate, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			short num;
			return new UniTask(UniTask.WaitWhilePromise<T>.Create(state, predicate, timing, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000050B4 File Offset: 0x000032B4
		public static UniTask WaitUntilCanceled(CancellationToken cancellationToken, PlayerLoopTiming timing = PlayerLoopTiming.Update, bool completeImmediately = false)
		{
			short num;
			return new UniTask(UniTask.WaitUntilCanceledPromise.Create(cancellationToken, timing, completeImmediately, out num), num);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000050D4 File Offset: 0x000032D4
		public static UniTask<U> WaitUntilValueChanged<T, U>(T target, Func<T, U> monitorFunction, PlayerLoopTiming monitorTiming = PlayerLoopTiming.Update, IEqualityComparer<U> equalityComparer = null, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false) where T : class
		{
			short num;
			return new UniTask<U>((target is Object) ? UniTask.WaitUntilValueChangedUnityObjectPromise<T, U>.Create(target, monitorFunction, equalityComparer, monitorTiming, cancellationToken, cancelImmediately, out num) : UniTask.WaitUntilValueChangedStandardObjectPromise<T, U>.Create(target, monitorFunction, equalityComparer, monitorTiming, cancellationToken, cancelImmediately, out num), num);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005117 File Offset: 0x00003317
		public static UniTask<T[]> WhenAll<T>(params UniTask<T>[] tasks)
		{
			if (tasks.Length == 0)
			{
				return UniTask.FromResult<T[]>(Array.Empty<T>());
			}
			return new UniTask<T[]>(new UniTask.WhenAllPromise<T>(tasks, tasks.Length), 0);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005138 File Offset: 0x00003338
		public static UniTask<T[]> WhenAll<T>(IEnumerable<UniTask<T>> tasks)
		{
			UniTask<T[]> result;
			using (ArrayPoolUtil.RentArray<UniTask<T>> rentArray = ArrayPoolUtil.Materialize<UniTask<T>>(tasks))
			{
				result = new UniTask<T[]>(new UniTask.WhenAllPromise<T>(rentArray.Array, rentArray.Length), 0);
			}
			return result;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005188 File Offset: 0x00003388
		public static UniTask WhenAll(params UniTask[] tasks)
		{
			if (tasks.Length == 0)
			{
				return UniTask.CompletedTask;
			}
			return new UniTask(new UniTask.WhenAllPromise(tasks, tasks.Length), 0);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000051A4 File Offset: 0x000033A4
		public static UniTask WhenAll(IEnumerable<UniTask> tasks)
		{
			UniTask result;
			using (ArrayPoolUtil.RentArray<UniTask> rentArray = ArrayPoolUtil.Materialize<UniTask>(tasks))
			{
				result = new UniTask(new UniTask.WhenAllPromise(rentArray.Array, rentArray.Length), 0);
			}
			return result;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000051F4 File Offset: 0x000033F4
		public static UniTask<ValueTuple<T1, T2>> WhenAll<T1, T2>(UniTask<T1> task1, UniTask<T2> task2)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2>>(new ValueTuple<T1, T2>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult()));
			}
			return new UniTask<ValueTuple<T1, T2>>(new UniTask.WhenAllPromise<T1, T2>(task1, task2), 0);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005254 File Offset: 0x00003454
		public static UniTask<ValueTuple<T1, T2, T3>> WhenAll<T1, T2, T3>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3>>(new ValueTuple<T1, T2, T3>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult()));
			}
			return new UniTask<ValueTuple<T1, T2, T3>>(new UniTask.WhenAllPromise<T1, T2, T3>(task1, task2, task3), 0);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000052D4 File Offset: 0x000034D4
		public static UniTask<ValueTuple<T1, T2, T3, T4>> WhenAll<T1, T2, T3, T4>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4>>(new ValueTuple<T1, T2, T3, T4>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult()));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4>>(new UniTask.WhenAllPromise<T1, T2, T3, T4>(task1, task2, task3, task4), 0);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005370 File Offset: 0x00003570
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5>> WhenAll<T1, T2, T3, T4, T5>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5>>(new ValueTuple<T1, T2, T3, T4, T5>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult()));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5>(task1, task2, task3, task4, task5), 0);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005434 File Offset: 0x00003634
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6>> WhenAll<T1, T2, T3, T4, T5, T6>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6>>(new ValueTuple<T1, T2, T3, T4, T5, T6>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult()));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>(task1, task2, task3, task4, task5, task6), 0);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000551C File Offset: 0x0000371C
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> WhenAll<T1, T2, T3, T4, T5, T6, T7>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>(new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult()));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>(task1, task2, task3, task4, task5, task6, task7), 0);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000562C File Offset: 0x0000382C
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>>(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), new ValueTuple<T8>(task8.GetAwaiter().GetResult())));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>(task1, task2, task3, task4, task5, task6, task7, task8), 0);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005764 File Offset: 0x00003964
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>>(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), new ValueTuple<T8, T9>(task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult())));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>(task1, task2, task3, task4, task5, task6, task7, task8, task9), 0);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000058C0 File Offset: 0x00003AC0
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>>(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), new ValueTuple<T8, T9, T10>(task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult())));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10), 0);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005A40 File Offset: 0x00003C40
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>>(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), new ValueTuple<T8, T9, T10, T11>(task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult())));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11), 0);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005BE4 File Offset: 0x00003DE4
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully() && task12.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>>(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), new ValueTuple<T8, T9, T10, T11, T12>(task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult(), task12.GetAwaiter().GetResult())));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12), 0);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005DA8 File Offset: 0x00003FA8
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully() && task12.Status.IsCompletedSuccessfully() && task13.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>>(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), new ValueTuple<T8, T9, T10, T11, T12, T13>(task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult(), task12.GetAwaiter().GetResult(), task13.GetAwaiter().GetResult())));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13), 0);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005F90 File Offset: 0x00004190
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13, UniTask<T14> task14)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully() && task12.Status.IsCompletedSuccessfully() && task13.Status.IsCompletedSuccessfully() && task14.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>>(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult(), task12.GetAwaiter().GetResult(), task13.GetAwaiter().GetResult(), task14.GetAwaiter().GetResult())));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14), 0);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000619C File Offset: 0x0000439C
		public static UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13, UniTask<T14> task14, UniTask<T15> task15)
		{
			if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully() && task4.Status.IsCompletedSuccessfully() && task5.Status.IsCompletedSuccessfully() && task6.Status.IsCompletedSuccessfully() && task7.Status.IsCompletedSuccessfully() && task8.Status.IsCompletedSuccessfully() && task9.Status.IsCompletedSuccessfully() && task10.Status.IsCompletedSuccessfully() && task11.Status.IsCompletedSuccessfully() && task12.Status.IsCompletedSuccessfully() && task13.Status.IsCompletedSuccessfully() && task14.Status.IsCompletedSuccessfully() && task15.Status.IsCompletedSuccessfully())
			{
				return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>>(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult(), task4.GetAwaiter().GetResult(), task5.GetAwaiter().GetResult(), task6.GetAwaiter().GetResult(), task7.GetAwaiter().GetResult(), new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(task8.GetAwaiter().GetResult(), task9.GetAwaiter().GetResult(), task10.GetAwaiter().GetResult(), task11.GetAwaiter().GetResult(), task12.GetAwaiter().GetResult(), task13.GetAwaiter().GetResult(), task14.GetAwaiter().GetResult(), new ValueTuple<T15>(task15.GetAwaiter().GetResult()))));
			}
			return new UniTask<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>>(new UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14, task15), 0);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000063CE File Offset: 0x000045CE
		[return: TupleElementNames(new string[]
		{
			"hasResultLeft",
			"result"
		})]
		public static UniTask<ValueTuple<bool, T>> WhenAny<T>(UniTask<T> leftTask, UniTask rightTask)
		{
			return new UniTask<ValueTuple<bool, T>>(new UniTask.WhenAnyLRPromise<T>(leftTask, rightTask), 0);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000063DD File Offset: 0x000045DD
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result"
		})]
		public static UniTask<ValueTuple<int, T>> WhenAny<T>(params UniTask<T>[] tasks)
		{
			return new UniTask<ValueTuple<int, T>>(new UniTask.WhenAnyPromise<T>(tasks, tasks.Length), 0);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000063F0 File Offset: 0x000045F0
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result"
		})]
		public static UniTask<ValueTuple<int, T>> WhenAny<T>(IEnumerable<UniTask<T>> tasks)
		{
			UniTask<ValueTuple<int, T>> result;
			using (ArrayPoolUtil.RentArray<UniTask<T>> rentArray = ArrayPoolUtil.Materialize<UniTask<T>>(tasks))
			{
				result = new UniTask<ValueTuple<int, T>>(new UniTask.WhenAnyPromise<T>(rentArray.Array, rentArray.Length), 0);
			}
			return result;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006440 File Offset: 0x00004640
		public static UniTask<int> WhenAny(params UniTask[] tasks)
		{
			return new UniTask<int>(new UniTask.WhenAnyPromise(tasks, tasks.Length), 0);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006454 File Offset: 0x00004654
		public static UniTask<int> WhenAny(IEnumerable<UniTask> tasks)
		{
			UniTask<int> result;
			using (ArrayPoolUtil.RentArray<UniTask> rentArray = ArrayPoolUtil.Materialize<UniTask>(tasks))
			{
				result = new UniTask<int>(new UniTask.WhenAnyPromise(rentArray.Array, rentArray.Length), 0);
			}
			return result;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000064A4 File Offset: 0x000046A4
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2"
		})]
		public static UniTask<ValueTuple<int, T1, T2>> WhenAny<T1, T2>(UniTask<T1> task1, UniTask<T2> task2)
		{
			return new UniTask<ValueTuple<int, T1, T2>>(new UniTask.WhenAnyPromise<T1, T2>(task1, task2), 0);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000064B3 File Offset: 0x000046B3
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3"
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3>> WhenAny<T1, T2, T3>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3>>(new UniTask.WhenAnyPromise<T1, T2, T3>(task1, task2, task3), 0);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000064C3 File Offset: 0x000046C3
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4"
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4>> WhenAny<T1, T2, T3, T4>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4>(task1, task2, task3, task4), 0);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000064D4 File Offset: 0x000046D4
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5"
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5>> WhenAny<T1, T2, T3, T4, T5>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>(task1, task2, task3, task4, task5), 0);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000064E7 File Offset: 0x000046E7
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6"
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6>> WhenAny<T1, T2, T3, T4, T5, T6>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>(task1, task2, task3, task4, task5, task6), 0);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000064FC File Offset: 0x000046FC
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6",
			"result7",
			null
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>> WhenAny<T1, T2, T3, T4, T5, T6, T7>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>(task1, task2, task3, task4, task5, task6, task7), 0);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006513 File Offset: 0x00004713
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6",
			"result7",
			"result8",
			null,
			null
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>> WhenAny<T1, T2, T3, T4, T5, T6, T7, T8>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>(task1, task2, task3, task4, task5, task6, task7, task8), 0);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000652C File Offset: 0x0000472C
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6",
			"result7",
			"result8",
			"result9",
			null,
			null,
			null
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>> WhenAny<T1, T2, T3, T4, T5, T6, T7, T8, T9>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>(task1, task2, task3, task4, task5, task6, task7, task8, task9), 0);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006554 File Offset: 0x00004754
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6",
			"result7",
			"result8",
			"result9",
			"result10",
			null,
			null,
			null,
			null
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>> WhenAny<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10), 0);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000657C File Offset: 0x0000477C
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6",
			"result7",
			"result8",
			"result9",
			"result10",
			"result11",
			null,
			null,
			null,
			null,
			null
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>> WhenAny<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11), 0);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000065A8 File Offset: 0x000047A8
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6",
			"result7",
			"result8",
			"result9",
			"result10",
			"result11",
			"result12",
			null,
			null,
			null,
			null,
			null,
			null
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>> WhenAny<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12), 0);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000065D4 File Offset: 0x000047D4
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6",
			"result7",
			"result8",
			"result9",
			"result10",
			"result11",
			"result12",
			"result13",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>> WhenAny<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13), 0);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006604 File Offset: 0x00004804
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6",
			"result7",
			"result8",
			"result9",
			"result10",
			"result11",
			"result12",
			"result13",
			"result14",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>> WhenAny<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13, UniTask<T14> task14)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14), 0);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006634 File Offset: 0x00004834
		[return: TupleElementNames(new string[]
		{
			"winArgumentIndex",
			"result1",
			"result2",
			"result3",
			"result4",
			"result5",
			"result6",
			"result7",
			"result8",
			"result9",
			"result10",
			"result11",
			"result12",
			"result13",
			"result14",
			"result15",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		})]
		public static UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>> WhenAny<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13, UniTask<T14> task14, UniTask<T15> task15)
		{
			return new UniTask<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>>(new UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14, task15), 0);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006666 File Offset: 0x00004866
		public static IUniTaskAsyncEnumerable<WhenEachResult<T>> WhenEach<T>(IEnumerable<UniTask<T>> tasks)
		{
			return new WhenEachEnumerable<T>(tasks);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000666E File Offset: 0x0000486E
		public static IUniTaskAsyncEnumerable<WhenEachResult<T>> WhenEach<T>(params UniTask<T>[] tasks)
		{
			return new WhenEachEnumerable<T>(tasks);
		}

		// Token: 0x0400007B RID: 123
		private readonly IUniTaskSource source;

		// Token: 0x0400007C RID: 124
		private readonly short token;

		// Token: 0x0400007D RID: 125
		private static readonly UniTask CanceledUniTask = (() => new UniTask(new UniTask.CanceledResultSource(CancellationToken.None), 0))();

		// Token: 0x0400007E RID: 126
		public static readonly UniTask CompletedTask = default(UniTask);

		// Token: 0x02000166 RID: 358
		private sealed class AsyncUnitSource : IUniTaskSource<AsyncUnit>, IUniTaskSource, IValueTaskSource, IValueTaskSource<AsyncUnit>
		{
			// Token: 0x06000786 RID: 1926 RVA: 0x00011886 File Offset: 0x0000FA86
			public AsyncUnitSource(IUniTaskSource source)
			{
				this.source = source;
			}

			// Token: 0x06000787 RID: 1927 RVA: 0x00011895 File Offset: 0x0000FA95
			public AsyncUnit GetResult(short token)
			{
				this.source.GetResult(token);
				return AsyncUnit.Default;
			}

			// Token: 0x06000788 RID: 1928 RVA: 0x000118A8 File Offset: 0x0000FAA8
			public UniTaskStatus GetStatus(short token)
			{
				return this.source.GetStatus(token);
			}

			// Token: 0x06000789 RID: 1929 RVA: 0x000118B6 File Offset: 0x0000FAB6
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.source.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600078A RID: 1930 RVA: 0x000118C6 File Offset: 0x0000FAC6
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.source.UnsafeGetStatus();
			}

			// Token: 0x0600078B RID: 1931 RVA: 0x000118D3 File Offset: 0x0000FAD3
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x040001E0 RID: 480
			private readonly IUniTaskSource source;
		}

		// Token: 0x02000167 RID: 359
		private sealed class IsCanceledSource : IUniTaskSource<bool>, IUniTaskSource, IValueTaskSource, IValueTaskSource<bool>
		{
			// Token: 0x0600078C RID: 1932 RVA: 0x000118DD File Offset: 0x0000FADD
			public IsCanceledSource(IUniTaskSource source)
			{
				this.source = source;
			}

			// Token: 0x0600078D RID: 1933 RVA: 0x000118EC File Offset: 0x0000FAEC
			public bool GetResult(short token)
			{
				if (this.source.GetStatus(token) == UniTaskStatus.Canceled)
				{
					return true;
				}
				this.source.GetResult(token);
				return false;
			}

			// Token: 0x0600078E RID: 1934 RVA: 0x0001190C File Offset: 0x0000FB0C
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0600078F RID: 1935 RVA: 0x00011916 File Offset: 0x0000FB16
			public UniTaskStatus GetStatus(short token)
			{
				return this.source.GetStatus(token);
			}

			// Token: 0x06000790 RID: 1936 RVA: 0x00011924 File Offset: 0x0000FB24
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.source.UnsafeGetStatus();
			}

			// Token: 0x06000791 RID: 1937 RVA: 0x00011931 File Offset: 0x0000FB31
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.source.OnCompleted(continuation, state, token);
			}

			// Token: 0x040001E1 RID: 481
			private readonly IUniTaskSource source;
		}

		// Token: 0x02000168 RID: 360
		private sealed class MemoizeSource : IUniTaskSource, IValueTaskSource
		{
			// Token: 0x06000792 RID: 1938 RVA: 0x00011941 File Offset: 0x0000FB41
			public MemoizeSource(IUniTaskSource source)
			{
				this.source = source;
			}

			// Token: 0x06000793 RID: 1939 RVA: 0x00011950 File Offset: 0x0000FB50
			public void GetResult(short token)
			{
				if (this.source == null)
				{
					if (this.exception != null)
					{
						this.exception.Throw();
						return;
					}
				}
				else
				{
					try
					{
						this.source.GetResult(token);
						this.status = UniTaskStatus.Succeeded;
					}
					catch (Exception ex)
					{
						this.exception = ExceptionDispatchInfo.Capture(ex);
						if (ex is OperationCanceledException)
						{
							this.status = UniTaskStatus.Canceled;
						}
						else
						{
							this.status = UniTaskStatus.Faulted;
						}
						throw;
					}
					finally
					{
						this.source = null;
					}
				}
			}

			// Token: 0x06000794 RID: 1940 RVA: 0x000119DC File Offset: 0x0000FBDC
			public UniTaskStatus GetStatus(short token)
			{
				if (this.source == null)
				{
					return this.status;
				}
				return this.source.GetStatus(token);
			}

			// Token: 0x06000795 RID: 1941 RVA: 0x000119F9 File Offset: 0x0000FBF9
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				if (this.source == null)
				{
					continuation(state);
					return;
				}
				this.source.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000796 RID: 1942 RVA: 0x00011A19 File Offset: 0x0000FC19
			public UniTaskStatus UnsafeGetStatus()
			{
				if (this.source == null)
				{
					return this.status;
				}
				return this.source.UnsafeGetStatus();
			}

			// Token: 0x040001E2 RID: 482
			private IUniTaskSource source;

			// Token: 0x040001E3 RID: 483
			private ExceptionDispatchInfo exception;

			// Token: 0x040001E4 RID: 484
			private UniTaskStatus status;
		}

		// Token: 0x02000169 RID: 361
		public readonly struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000797 RID: 1943 RVA: 0x00011A35 File Offset: 0x0000FC35
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Awaiter(in UniTask task)
			{
				this.task = task;
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000798 RID: 1944 RVA: 0x00011A43 File Offset: 0x0000FC43
			public bool IsCompleted
			{
				[DebuggerHidden]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.task.Status.IsCompleted();
				}
			}

			// Token: 0x06000799 RID: 1945 RVA: 0x00011A55 File Offset: 0x0000FC55
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void GetResult()
			{
				if (this.task.source == null)
				{
					return;
				}
				this.task.source.GetResult(this.task.token);
			}

			// Token: 0x0600079A RID: 1946 RVA: 0x00011A80 File Offset: 0x0000FC80
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void OnCompleted(Action continuation)
			{
				if (this.task.source == null)
				{
					continuation();
					return;
				}
				this.task.source.OnCompleted(AwaiterActions.InvokeContinuationDelegate, continuation, this.task.token);
			}

			// Token: 0x0600079B RID: 1947 RVA: 0x00011AB7 File Offset: 0x0000FCB7
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UnsafeOnCompleted(Action continuation)
			{
				if (this.task.source == null)
				{
					continuation();
					return;
				}
				this.task.source.OnCompleted(AwaiterActions.InvokeContinuationDelegate, continuation, this.task.token);
			}

			// Token: 0x0600079C RID: 1948 RVA: 0x00011AEE File Offset: 0x0000FCEE
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SourceOnCompleted(Action<object> continuation, object state)
			{
				if (this.task.source == null)
				{
					continuation(state);
					return;
				}
				this.task.source.OnCompleted(continuation, state, this.task.token);
			}

			// Token: 0x040001E5 RID: 485
			private readonly UniTask task;
		}

		// Token: 0x0200016A RID: 362
		private sealed class YieldPromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.YieldPromise>
		{
			// Token: 0x1700005D RID: 93
			// (get) Token: 0x0600079D RID: 1949 RVA: 0x00011B22 File Offset: 0x0000FD22
			public ref UniTask.YieldPromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x0600079E RID: 1950 RVA: 0x00011B2A File Offset: 0x0000FD2A
			static YieldPromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.YieldPromise), () => UniTask.YieldPromise.pool.Size);
			}

			// Token: 0x0600079F RID: 1951 RVA: 0x00011B4B File Offset: 0x0000FD4B
			private YieldPromise()
			{
			}

			// Token: 0x060007A0 RID: 1952 RVA: 0x00011B54 File Offset: 0x0000FD54
			public static IUniTaskSource Create(PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.YieldPromise yieldPromise;
				if (!UniTask.YieldPromise.pool.TryPop(out yieldPromise))
				{
					yieldPromise = new UniTask.YieldPromise();
				}
				yieldPromise.cancellationToken = cancellationToken;
				yieldPromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					yieldPromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.YieldPromise yieldPromise2 = (UniTask.YieldPromise)state;
						yieldPromise2.core.TrySetCanceled(yieldPromise2.cancellationToken);
					}, yieldPromise);
				}
				PlayerLoopHelper.AddAction(timing, yieldPromise);
				token = yieldPromise.core.Version;
				return yieldPromise;
			}

			// Token: 0x060007A1 RID: 1953 RVA: 0x00011BE4 File Offset: 0x0000FDE4
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x060007A2 RID: 1954 RVA: 0x00011C30 File Offset: 0x0000FE30
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060007A3 RID: 1955 RVA: 0x00011C3E File Offset: 0x0000FE3E
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060007A4 RID: 1956 RVA: 0x00011C4B File Offset: 0x0000FE4B
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060007A5 RID: 1957 RVA: 0x00011C5B File Offset: 0x0000FE5B
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				this.core.TrySetResult(null);
				return false;
			}

			// Token: 0x060007A6 RID: 1958 RVA: 0x00011C8C File Offset: 0x0000FE8C
			private bool TryReturn()
			{
				this.core.Reset();
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.YieldPromise.pool.TryPush(this);
			}

			// Token: 0x040001E6 RID: 486
			private static TaskPool<UniTask.YieldPromise> pool;

			// Token: 0x040001E7 RID: 487
			private UniTask.YieldPromise nextNode;

			// Token: 0x040001E8 RID: 488
			private CancellationToken cancellationToken;

			// Token: 0x040001E9 RID: 489
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040001EA RID: 490
			private bool cancelImmediately;

			// Token: 0x040001EB RID: 491
			private UniTaskCompletionSourceCore<object> core;
		}

		// Token: 0x0200016B RID: 363
		private sealed class NextFramePromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.NextFramePromise>
		{
			// Token: 0x1700005E RID: 94
			// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00011CC2 File Offset: 0x0000FEC2
			public ref UniTask.NextFramePromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x060007A8 RID: 1960 RVA: 0x00011CCA File Offset: 0x0000FECA
			static NextFramePromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.NextFramePromise), () => UniTask.NextFramePromise.pool.Size);
			}

			// Token: 0x060007A9 RID: 1961 RVA: 0x00011CEB File Offset: 0x0000FEEB
			private NextFramePromise()
			{
			}

			// Token: 0x060007AA RID: 1962 RVA: 0x00011CF4 File Offset: 0x0000FEF4
			public static IUniTaskSource Create(PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.NextFramePromise nextFramePromise;
				if (!UniTask.NextFramePromise.pool.TryPop(out nextFramePromise))
				{
					nextFramePromise = new UniTask.NextFramePromise();
				}
				nextFramePromise.frameCount = (PlayerLoopHelper.IsMainThread ? Time.frameCount : -1);
				nextFramePromise.cancellationToken = cancellationToken;
				nextFramePromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					nextFramePromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.NextFramePromise nextFramePromise2 = (UniTask.NextFramePromise)state;
						nextFramePromise2.core.TrySetCanceled(nextFramePromise2.cancellationToken);
					}, nextFramePromise);
				}
				PlayerLoopHelper.AddAction(timing, nextFramePromise);
				token = nextFramePromise.core.Version;
				return nextFramePromise;
			}

			// Token: 0x060007AB RID: 1963 RVA: 0x00011D98 File Offset: 0x0000FF98
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x060007AC RID: 1964 RVA: 0x00011DE4 File Offset: 0x0000FFE4
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060007AD RID: 1965 RVA: 0x00011DF2 File Offset: 0x0000FFF2
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060007AE RID: 1966 RVA: 0x00011DFF File Offset: 0x0000FFFF
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060007AF RID: 1967 RVA: 0x00011E10 File Offset: 0x00010010
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.frameCount == Time.frameCount)
				{
					return true;
				}
				this.core.TrySetResult(AsyncUnit.Default);
				return false;
			}

			// Token: 0x060007B0 RID: 1968 RVA: 0x00011E5F File Offset: 0x0001005F
			private bool TryReturn()
			{
				this.core.Reset();
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				return UniTask.NextFramePromise.pool.TryPush(this);
			}

			// Token: 0x040001EC RID: 492
			private static TaskPool<UniTask.NextFramePromise> pool;

			// Token: 0x040001ED RID: 493
			private UniTask.NextFramePromise nextNode;

			// Token: 0x040001EE RID: 494
			private int frameCount;

			// Token: 0x040001EF RID: 495
			private UniTaskCompletionSourceCore<AsyncUnit> core;

			// Token: 0x040001F0 RID: 496
			private CancellationToken cancellationToken;

			// Token: 0x040001F1 RID: 497
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040001F2 RID: 498
			private bool cancelImmediately;
		}

		// Token: 0x0200016C RID: 364
		private sealed class WaitForEndOfFramePromise : IUniTaskSource, IValueTaskSource, ITaskPoolNode<UniTask.WaitForEndOfFramePromise>, IEnumerator
		{
			// Token: 0x1700005F RID: 95
			// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00011E8E File Offset: 0x0001008E
			public ref UniTask.WaitForEndOfFramePromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x060007B2 RID: 1970 RVA: 0x00011E96 File Offset: 0x00010096
			static WaitForEndOfFramePromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.WaitForEndOfFramePromise), () => UniTask.WaitForEndOfFramePromise.pool.Size);
			}

			// Token: 0x060007B3 RID: 1971 RVA: 0x00011EC1 File Offset: 0x000100C1
			private WaitForEndOfFramePromise()
			{
			}

			// Token: 0x060007B4 RID: 1972 RVA: 0x00011ED0 File Offset: 0x000100D0
			public static IUniTaskSource Create(MonoBehaviour coroutineRunner, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.WaitForEndOfFramePromise waitForEndOfFramePromise;
				if (!UniTask.WaitForEndOfFramePromise.pool.TryPop(out waitForEndOfFramePromise))
				{
					waitForEndOfFramePromise = new UniTask.WaitForEndOfFramePromise();
				}
				waitForEndOfFramePromise.cancellationToken = cancellationToken;
				waitForEndOfFramePromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					waitForEndOfFramePromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.WaitForEndOfFramePromise waitForEndOfFramePromise2 = (UniTask.WaitForEndOfFramePromise)state;
						waitForEndOfFramePromise2.core.TrySetCanceled(waitForEndOfFramePromise2.cancellationToken);
					}, waitForEndOfFramePromise);
				}
				coroutineRunner.StartCoroutine(waitForEndOfFramePromise);
				token = waitForEndOfFramePromise.core.Version;
				return waitForEndOfFramePromise;
			}

			// Token: 0x060007B5 RID: 1973 RVA: 0x00011F60 File Offset: 0x00010160
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x060007B6 RID: 1974 RVA: 0x00011FAC File Offset: 0x000101AC
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060007B7 RID: 1975 RVA: 0x00011FBA File Offset: 0x000101BA
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060007B8 RID: 1976 RVA: 0x00011FC7 File Offset: 0x000101C7
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060007B9 RID: 1977 RVA: 0x00011FD7 File Offset: 0x000101D7
			private bool TryReturn()
			{
				this.core.Reset();
				this.Reset();
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				return UniTask.WaitForEndOfFramePromise.pool.TryPush(this);
			}

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001200C File Offset: 0x0001020C
			object IEnumerator.Current
			{
				get
				{
					return UniTask.WaitForEndOfFramePromise.waitForEndOfFrameYieldInstruction;
				}
			}

			// Token: 0x060007BB RID: 1979 RVA: 0x00012014 File Offset: 0x00010214
			bool IEnumerator.MoveNext()
			{
				if (this.isFirst)
				{
					this.isFirst = false;
					return true;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				this.core.TrySetResult(null);
				return false;
			}

			// Token: 0x060007BC RID: 1980 RVA: 0x00012061 File Offset: 0x00010261
			public void Reset()
			{
				this.isFirst = true;
			}

			// Token: 0x040001F3 RID: 499
			private static TaskPool<UniTask.WaitForEndOfFramePromise> pool;

			// Token: 0x040001F4 RID: 500
			private UniTask.WaitForEndOfFramePromise nextNode;

			// Token: 0x040001F5 RID: 501
			private UniTaskCompletionSourceCore<object> core;

			// Token: 0x040001F6 RID: 502
			private CancellationToken cancellationToken;

			// Token: 0x040001F7 RID: 503
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040001F8 RID: 504
			private bool cancelImmediately;

			// Token: 0x040001F9 RID: 505
			private static readonly WaitForEndOfFrame waitForEndOfFrameYieldInstruction = new WaitForEndOfFrame();

			// Token: 0x040001FA RID: 506
			private bool isFirst = true;
		}

		// Token: 0x0200016D RID: 365
		private sealed class DelayFramePromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.DelayFramePromise>
		{
			// Token: 0x17000061 RID: 97
			// (get) Token: 0x060007BD RID: 1981 RVA: 0x0001206A File Offset: 0x0001026A
			public ref UniTask.DelayFramePromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x060007BE RID: 1982 RVA: 0x00012072 File Offset: 0x00010272
			static DelayFramePromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.DelayFramePromise), () => UniTask.DelayFramePromise.pool.Size);
			}

			// Token: 0x060007BF RID: 1983 RVA: 0x00012093 File Offset: 0x00010293
			private DelayFramePromise()
			{
			}

			// Token: 0x060007C0 RID: 1984 RVA: 0x0001209C File Offset: 0x0001029C
			public static IUniTaskSource Create(int delayFrameCount, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.DelayFramePromise delayFramePromise;
				if (!UniTask.DelayFramePromise.pool.TryPop(out delayFramePromise))
				{
					delayFramePromise = new UniTask.DelayFramePromise();
				}
				delayFramePromise.delayFrameCount = delayFrameCount;
				delayFramePromise.cancellationToken = cancellationToken;
				delayFramePromise.initialFrame = (PlayerLoopHelper.IsMainThread ? Time.frameCount : -1);
				delayFramePromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					delayFramePromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.DelayFramePromise delayFramePromise2 = (UniTask.DelayFramePromise)state;
						delayFramePromise2.core.TrySetCanceled(delayFramePromise2.cancellationToken);
					}, delayFramePromise);
				}
				PlayerLoopHelper.AddAction(timing, delayFramePromise);
				token = delayFramePromise.core.Version;
				return delayFramePromise;
			}

			// Token: 0x060007C1 RID: 1985 RVA: 0x00012148 File Offset: 0x00010348
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x060007C2 RID: 1986 RVA: 0x00012194 File Offset: 0x00010394
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060007C3 RID: 1987 RVA: 0x000121A2 File Offset: 0x000103A2
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060007C4 RID: 1988 RVA: 0x000121AF File Offset: 0x000103AF
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060007C5 RID: 1989 RVA: 0x000121C0 File Offset: 0x000103C0
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.currentFrameCount == 0)
				{
					if (this.delayFrameCount == 0)
					{
						this.core.TrySetResult(AsyncUnit.Default);
						return false;
					}
					if (this.initialFrame == Time.frameCount)
					{
						return true;
					}
				}
				int num = this.currentFrameCount + 1;
				this.currentFrameCount = num;
				if (num >= this.delayFrameCount)
				{
					this.core.TrySetResult(AsyncUnit.Default);
					return false;
				}
				return true;
			}

			// Token: 0x060007C6 RID: 1990 RVA: 0x00012250 File Offset: 0x00010450
			private bool TryReturn()
			{
				this.core.Reset();
				this.currentFrameCount = 0;
				this.delayFrameCount = 0;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.DelayFramePromise.pool.TryPush(this);
			}

			// Token: 0x040001FB RID: 507
			private static TaskPool<UniTask.DelayFramePromise> pool;

			// Token: 0x040001FC RID: 508
			private UniTask.DelayFramePromise nextNode;

			// Token: 0x040001FD RID: 509
			private int initialFrame;

			// Token: 0x040001FE RID: 510
			private int delayFrameCount;

			// Token: 0x040001FF RID: 511
			private CancellationToken cancellationToken;

			// Token: 0x04000200 RID: 512
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04000201 RID: 513
			private bool cancelImmediately;

			// Token: 0x04000202 RID: 514
			private int currentFrameCount;

			// Token: 0x04000203 RID: 515
			private UniTaskCompletionSourceCore<AsyncUnit> core;
		}

		// Token: 0x0200016E RID: 366
		private sealed class DelayPromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.DelayPromise>
		{
			// Token: 0x17000062 RID: 98
			// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0001229F File Offset: 0x0001049F
			public ref UniTask.DelayPromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x060007C8 RID: 1992 RVA: 0x000122A7 File Offset: 0x000104A7
			static DelayPromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.DelayPromise), () => UniTask.DelayPromise.pool.Size);
			}

			// Token: 0x060007C9 RID: 1993 RVA: 0x000122C8 File Offset: 0x000104C8
			private DelayPromise()
			{
			}

			// Token: 0x060007CA RID: 1994 RVA: 0x000122D0 File Offset: 0x000104D0
			public static IUniTaskSource Create(TimeSpan delayTimeSpan, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.DelayPromise delayPromise;
				if (!UniTask.DelayPromise.pool.TryPop(out delayPromise))
				{
					delayPromise = new UniTask.DelayPromise();
				}
				delayPromise.elapsed = 0f;
				delayPromise.delayTimeSpan = (float)delayTimeSpan.TotalSeconds;
				delayPromise.cancellationToken = cancellationToken;
				delayPromise.initialFrame = (PlayerLoopHelper.IsMainThread ? Time.frameCount : -1);
				delayPromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					delayPromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.DelayPromise delayPromise2 = (UniTask.DelayPromise)state;
						delayPromise2.core.TrySetCanceled(delayPromise2.cancellationToken);
					}, delayPromise);
				}
				PlayerLoopHelper.AddAction(timing, delayPromise);
				token = delayPromise.core.Version;
				return delayPromise;
			}

			// Token: 0x060007CB RID: 1995 RVA: 0x00012390 File Offset: 0x00010590
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x060007CC RID: 1996 RVA: 0x000123DC File Offset: 0x000105DC
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060007CD RID: 1997 RVA: 0x000123EA File Offset: 0x000105EA
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060007CE RID: 1998 RVA: 0x000123F7 File Offset: 0x000105F7
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060007CF RID: 1999 RVA: 0x00012408 File Offset: 0x00010608
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.elapsed == 0f && this.initialFrame == Time.frameCount)
				{
					return true;
				}
				this.elapsed += Time.deltaTime;
				if (this.elapsed >= this.delayTimeSpan)
				{
					this.core.TrySetResult(null);
					return false;
				}
				return true;
			}

			// Token: 0x060007D0 RID: 2000 RVA: 0x00012484 File Offset: 0x00010684
			private bool TryReturn()
			{
				this.core.Reset();
				this.delayTimeSpan = 0f;
				this.elapsed = 0f;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.DelayPromise.pool.TryPush(this);
			}

			// Token: 0x04000204 RID: 516
			private static TaskPool<UniTask.DelayPromise> pool;

			// Token: 0x04000205 RID: 517
			private UniTask.DelayPromise nextNode;

			// Token: 0x04000206 RID: 518
			private int initialFrame;

			// Token: 0x04000207 RID: 519
			private float delayTimeSpan;

			// Token: 0x04000208 RID: 520
			private float elapsed;

			// Token: 0x04000209 RID: 521
			private CancellationToken cancellationToken;

			// Token: 0x0400020A RID: 522
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x0400020B RID: 523
			private bool cancelImmediately;

			// Token: 0x0400020C RID: 524
			private UniTaskCompletionSourceCore<object> core;
		}

		// Token: 0x0200016F RID: 367
		private sealed class DelayIgnoreTimeScalePromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.DelayIgnoreTimeScalePromise>
		{
			// Token: 0x17000063 RID: 99
			// (get) Token: 0x060007D1 RID: 2001 RVA: 0x000124DB File Offset: 0x000106DB
			public ref UniTask.DelayIgnoreTimeScalePromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x060007D2 RID: 2002 RVA: 0x000124E3 File Offset: 0x000106E3
			static DelayIgnoreTimeScalePromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.DelayIgnoreTimeScalePromise), () => UniTask.DelayIgnoreTimeScalePromise.pool.Size);
			}

			// Token: 0x060007D3 RID: 2003 RVA: 0x00012504 File Offset: 0x00010704
			private DelayIgnoreTimeScalePromise()
			{
			}

			// Token: 0x060007D4 RID: 2004 RVA: 0x0001250C File Offset: 0x0001070C
			public static IUniTaskSource Create(TimeSpan delayFrameTimeSpan, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.DelayIgnoreTimeScalePromise delayIgnoreTimeScalePromise;
				if (!UniTask.DelayIgnoreTimeScalePromise.pool.TryPop(out delayIgnoreTimeScalePromise))
				{
					delayIgnoreTimeScalePromise = new UniTask.DelayIgnoreTimeScalePromise();
				}
				delayIgnoreTimeScalePromise.elapsed = 0f;
				delayIgnoreTimeScalePromise.delayFrameTimeSpan = (float)delayFrameTimeSpan.TotalSeconds;
				delayIgnoreTimeScalePromise.initialFrame = (PlayerLoopHelper.IsMainThread ? Time.frameCount : -1);
				delayIgnoreTimeScalePromise.cancellationToken = cancellationToken;
				delayIgnoreTimeScalePromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					delayIgnoreTimeScalePromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.DelayIgnoreTimeScalePromise delayIgnoreTimeScalePromise2 = (UniTask.DelayIgnoreTimeScalePromise)state;
						delayIgnoreTimeScalePromise2.core.TrySetCanceled(delayIgnoreTimeScalePromise2.cancellationToken);
					}, delayIgnoreTimeScalePromise);
				}
				PlayerLoopHelper.AddAction(timing, delayIgnoreTimeScalePromise);
				token = delayIgnoreTimeScalePromise.core.Version;
				return delayIgnoreTimeScalePromise;
			}

			// Token: 0x060007D5 RID: 2005 RVA: 0x000125CC File Offset: 0x000107CC
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x060007D6 RID: 2006 RVA: 0x00012618 File Offset: 0x00010818
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060007D7 RID: 2007 RVA: 0x00012626 File Offset: 0x00010826
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060007D8 RID: 2008 RVA: 0x00012633 File Offset: 0x00010833
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060007D9 RID: 2009 RVA: 0x00012644 File Offset: 0x00010844
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.elapsed == 0f && this.initialFrame == Time.frameCount)
				{
					return true;
				}
				this.elapsed += Time.unscaledDeltaTime;
				if (this.elapsed >= this.delayFrameTimeSpan)
				{
					this.core.TrySetResult(null);
					return false;
				}
				return true;
			}

			// Token: 0x060007DA RID: 2010 RVA: 0x000126C0 File Offset: 0x000108C0
			private bool TryReturn()
			{
				this.core.Reset();
				this.delayFrameTimeSpan = 0f;
				this.elapsed = 0f;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.DelayIgnoreTimeScalePromise.pool.TryPush(this);
			}

			// Token: 0x0400020D RID: 525
			private static TaskPool<UniTask.DelayIgnoreTimeScalePromise> pool;

			// Token: 0x0400020E RID: 526
			private UniTask.DelayIgnoreTimeScalePromise nextNode;

			// Token: 0x0400020F RID: 527
			private float delayFrameTimeSpan;

			// Token: 0x04000210 RID: 528
			private float elapsed;

			// Token: 0x04000211 RID: 529
			private int initialFrame;

			// Token: 0x04000212 RID: 530
			private CancellationToken cancellationToken;

			// Token: 0x04000213 RID: 531
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04000214 RID: 532
			private bool cancelImmediately;

			// Token: 0x04000215 RID: 533
			private UniTaskCompletionSourceCore<object> core;
		}

		// Token: 0x02000170 RID: 368
		private sealed class DelayRealtimePromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.DelayRealtimePromise>
		{
			// Token: 0x17000064 RID: 100
			// (get) Token: 0x060007DB RID: 2011 RVA: 0x00012717 File Offset: 0x00010917
			public ref UniTask.DelayRealtimePromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x060007DC RID: 2012 RVA: 0x0001271F File Offset: 0x0001091F
			static DelayRealtimePromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.DelayRealtimePromise), () => UniTask.DelayRealtimePromise.pool.Size);
			}

			// Token: 0x060007DD RID: 2013 RVA: 0x00012740 File Offset: 0x00010940
			private DelayRealtimePromise()
			{
			}

			// Token: 0x060007DE RID: 2014 RVA: 0x00012748 File Offset: 0x00010948
			public static IUniTaskSource Create(TimeSpan delayTimeSpan, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.DelayRealtimePromise delayRealtimePromise;
				if (!UniTask.DelayRealtimePromise.pool.TryPop(out delayRealtimePromise))
				{
					delayRealtimePromise = new UniTask.DelayRealtimePromise();
				}
				delayRealtimePromise.stopwatch = ValueStopwatch.StartNew();
				delayRealtimePromise.delayTimeSpanTicks = delayTimeSpan.Ticks;
				delayRealtimePromise.cancellationToken = cancellationToken;
				delayRealtimePromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					delayRealtimePromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.DelayRealtimePromise delayRealtimePromise2 = (UniTask.DelayRealtimePromise)state;
						delayRealtimePromise2.core.TrySetCanceled(delayRealtimePromise2.cancellationToken);
					}, delayRealtimePromise);
				}
				PlayerLoopHelper.AddAction(timing, delayRealtimePromise);
				token = delayRealtimePromise.core.Version;
				return delayRealtimePromise;
			}

			// Token: 0x060007DF RID: 2015 RVA: 0x000127F0 File Offset: 0x000109F0
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x060007E0 RID: 2016 RVA: 0x0001283C File Offset: 0x00010A3C
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060007E1 RID: 2017 RVA: 0x0001284A File Offset: 0x00010A4A
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060007E2 RID: 2018 RVA: 0x00012857 File Offset: 0x00010A57
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060007E3 RID: 2019 RVA: 0x00012868 File Offset: 0x00010A68
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.stopwatch.IsInvalid)
				{
					this.core.TrySetResult(AsyncUnit.Default);
					return false;
				}
				if (this.stopwatch.ElapsedTicks >= this.delayTimeSpanTicks)
				{
					this.core.TrySetResult(AsyncUnit.Default);
					return false;
				}
				return true;
			}

			// Token: 0x060007E4 RID: 2020 RVA: 0x000128E0 File Offset: 0x00010AE0
			private bool TryReturn()
			{
				this.core.Reset();
				this.stopwatch = default(ValueStopwatch);
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.DelayRealtimePromise.pool.TryPush(this);
			}

			// Token: 0x04000216 RID: 534
			private static TaskPool<UniTask.DelayRealtimePromise> pool;

			// Token: 0x04000217 RID: 535
			private UniTask.DelayRealtimePromise nextNode;

			// Token: 0x04000218 RID: 536
			private long delayTimeSpanTicks;

			// Token: 0x04000219 RID: 537
			private ValueStopwatch stopwatch;

			// Token: 0x0400021A RID: 538
			private CancellationToken cancellationToken;

			// Token: 0x0400021B RID: 539
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x0400021C RID: 540
			private bool cancelImmediately;

			// Token: 0x0400021D RID: 541
			private UniTaskCompletionSourceCore<AsyncUnit> core;
		}

		// Token: 0x02000171 RID: 369
		private static class CanceledUniTaskCache<T>
		{
			// Token: 0x0400021E RID: 542
			public static readonly UniTask<T> Task = new UniTask<T>(new UniTask.CanceledResultSource<T>(CancellationToken.None), 0);
		}

		// Token: 0x02000172 RID: 370
		private sealed class ExceptionResultSource : IUniTaskSource, IValueTaskSource
		{
			// Token: 0x060007E6 RID: 2022 RVA: 0x00012944 File Offset: 0x00010B44
			public ExceptionResultSource(Exception exception)
			{
				this.exception = ExceptionDispatchInfo.Capture(exception);
			}

			// Token: 0x060007E7 RID: 2023 RVA: 0x00012958 File Offset: 0x00010B58
			public void GetResult(short token)
			{
				if (!this.calledGet)
				{
					this.calledGet = true;
					GC.SuppressFinalize(this);
				}
				this.exception.Throw();
			}

			// Token: 0x060007E8 RID: 2024 RVA: 0x0001297A File Offset: 0x00010B7A
			public UniTaskStatus GetStatus(short token)
			{
				return UniTaskStatus.Faulted;
			}

			// Token: 0x060007E9 RID: 2025 RVA: 0x0001297D File Offset: 0x00010B7D
			public UniTaskStatus UnsafeGetStatus()
			{
				return UniTaskStatus.Faulted;
			}

			// Token: 0x060007EA RID: 2026 RVA: 0x00012980 File Offset: 0x00010B80
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				continuation(state);
			}

			// Token: 0x060007EB RID: 2027 RVA: 0x0001298C File Offset: 0x00010B8C
			~ExceptionResultSource()
			{
				if (!this.calledGet)
				{
					UniTaskScheduler.PublishUnobservedTaskException(this.exception.SourceException);
				}
			}

			// Token: 0x0400021F RID: 543
			private readonly ExceptionDispatchInfo exception;

			// Token: 0x04000220 RID: 544
			private bool calledGet;
		}

		// Token: 0x02000173 RID: 371
		private sealed class ExceptionResultSource<T> : IUniTaskSource<!0>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>
		{
			// Token: 0x060007EC RID: 2028 RVA: 0x000129CC File Offset: 0x00010BCC
			public ExceptionResultSource(Exception exception)
			{
				this.exception = ExceptionDispatchInfo.Capture(exception);
			}

			// Token: 0x060007ED RID: 2029 RVA: 0x000129E0 File Offset: 0x00010BE0
			public T GetResult(short token)
			{
				if (!this.calledGet)
				{
					this.calledGet = true;
					GC.SuppressFinalize(this);
				}
				this.exception.Throw();
				return default(T);
			}

			// Token: 0x060007EE RID: 2030 RVA: 0x00012A16 File Offset: 0x00010C16
			void IUniTaskSource.GetResult(short token)
			{
				if (!this.calledGet)
				{
					this.calledGet = true;
					GC.SuppressFinalize(this);
				}
				this.exception.Throw();
			}

			// Token: 0x060007EF RID: 2031 RVA: 0x00012A38 File Offset: 0x00010C38
			public UniTaskStatus GetStatus(short token)
			{
				return UniTaskStatus.Faulted;
			}

			// Token: 0x060007F0 RID: 2032 RVA: 0x00012A3B File Offset: 0x00010C3B
			public UniTaskStatus UnsafeGetStatus()
			{
				return UniTaskStatus.Faulted;
			}

			// Token: 0x060007F1 RID: 2033 RVA: 0x00012A3E File Offset: 0x00010C3E
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				continuation(state);
			}

			// Token: 0x060007F2 RID: 2034 RVA: 0x00012A48 File Offset: 0x00010C48
			~ExceptionResultSource()
			{
				if (!this.calledGet)
				{
					UniTaskScheduler.PublishUnobservedTaskException(this.exception.SourceException);
				}
			}

			// Token: 0x04000221 RID: 545
			private readonly ExceptionDispatchInfo exception;

			// Token: 0x04000222 RID: 546
			private bool calledGet;
		}

		// Token: 0x02000174 RID: 372
		private sealed class CanceledResultSource : IUniTaskSource, IValueTaskSource
		{
			// Token: 0x060007F3 RID: 2035 RVA: 0x00012A88 File Offset: 0x00010C88
			public CanceledResultSource(CancellationToken cancellationToken)
			{
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x060007F4 RID: 2036 RVA: 0x00012A97 File Offset: 0x00010C97
			public void GetResult(short token)
			{
				throw new OperationCanceledException(this.cancellationToken);
			}

			// Token: 0x060007F5 RID: 2037 RVA: 0x00012AA4 File Offset: 0x00010CA4
			public UniTaskStatus GetStatus(short token)
			{
				return UniTaskStatus.Canceled;
			}

			// Token: 0x060007F6 RID: 2038 RVA: 0x00012AA7 File Offset: 0x00010CA7
			public UniTaskStatus UnsafeGetStatus()
			{
				return UniTaskStatus.Canceled;
			}

			// Token: 0x060007F7 RID: 2039 RVA: 0x00012AAA File Offset: 0x00010CAA
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				continuation(state);
			}

			// Token: 0x04000223 RID: 547
			private readonly CancellationToken cancellationToken;
		}

		// Token: 0x02000175 RID: 373
		private sealed class CanceledResultSource<T> : IUniTaskSource<!0>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>
		{
			// Token: 0x060007F8 RID: 2040 RVA: 0x00012AB3 File Offset: 0x00010CB3
			public CanceledResultSource(CancellationToken cancellationToken)
			{
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x060007F9 RID: 2041 RVA: 0x00012AC2 File Offset: 0x00010CC2
			public T GetResult(short token)
			{
				throw new OperationCanceledException(this.cancellationToken);
			}

			// Token: 0x060007FA RID: 2042 RVA: 0x00012ACF File Offset: 0x00010CCF
			void IUniTaskSource.GetResult(short token)
			{
				throw new OperationCanceledException(this.cancellationToken);
			}

			// Token: 0x060007FB RID: 2043 RVA: 0x00012ADC File Offset: 0x00010CDC
			public UniTaskStatus GetStatus(short token)
			{
				return UniTaskStatus.Canceled;
			}

			// Token: 0x060007FC RID: 2044 RVA: 0x00012ADF File Offset: 0x00010CDF
			public UniTaskStatus UnsafeGetStatus()
			{
				return UniTaskStatus.Canceled;
			}

			// Token: 0x060007FD RID: 2045 RVA: 0x00012AE2 File Offset: 0x00010CE2
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				continuation(state);
			}

			// Token: 0x04000224 RID: 548
			private readonly CancellationToken cancellationToken;
		}

		// Token: 0x02000176 RID: 374
		private sealed class DeferPromise : IUniTaskSource, IValueTaskSource
		{
			// Token: 0x060007FE RID: 2046 RVA: 0x00012AEB File Offset: 0x00010CEB
			public DeferPromise(Func<UniTask> factory)
			{
				this.factory = factory;
			}

			// Token: 0x060007FF RID: 2047 RVA: 0x00012AFA File Offset: 0x00010CFA
			public void GetResult(short token)
			{
				this.awaiter.GetResult();
			}

			// Token: 0x06000800 RID: 2048 RVA: 0x00012B08 File Offset: 0x00010D08
			public UniTaskStatus GetStatus(short token)
			{
				Func<UniTask> func = Interlocked.Exchange<Func<UniTask>>(ref this.factory, null);
				if (func != null)
				{
					this.task = func();
					this.awaiter = this.task.GetAwaiter();
				}
				return this.task.Status;
			}

			// Token: 0x06000801 RID: 2049 RVA: 0x00012B4D File Offset: 0x00010D4D
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.awaiter.SourceOnCompleted(continuation, state);
			}

			// Token: 0x06000802 RID: 2050 RVA: 0x00012B5C File Offset: 0x00010D5C
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.task.Status;
			}

			// Token: 0x04000225 RID: 549
			private Func<UniTask> factory;

			// Token: 0x04000226 RID: 550
			private UniTask task;

			// Token: 0x04000227 RID: 551
			private UniTask.Awaiter awaiter;
		}

		// Token: 0x02000177 RID: 375
		private sealed class DeferPromise<T> : IUniTaskSource<!0>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>
		{
			// Token: 0x06000803 RID: 2051 RVA: 0x00012B69 File Offset: 0x00010D69
			public DeferPromise(Func<UniTask<T>> factory)
			{
				this.factory = factory;
			}

			// Token: 0x06000804 RID: 2052 RVA: 0x00012B78 File Offset: 0x00010D78
			public T GetResult(short token)
			{
				return this.awaiter.GetResult();
			}

			// Token: 0x06000805 RID: 2053 RVA: 0x00012B85 File Offset: 0x00010D85
			void IUniTaskSource.GetResult(short token)
			{
				this.awaiter.GetResult();
			}

			// Token: 0x06000806 RID: 2054 RVA: 0x00012B94 File Offset: 0x00010D94
			public UniTaskStatus GetStatus(short token)
			{
				Func<UniTask<T>> func = Interlocked.Exchange<Func<UniTask<T>>>(ref this.factory, null);
				if (func != null)
				{
					this.task = func();
					this.awaiter = this.task.GetAwaiter();
				}
				return this.task.Status;
			}

			// Token: 0x06000807 RID: 2055 RVA: 0x00012BD9 File Offset: 0x00010DD9
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.awaiter.SourceOnCompleted(continuation, state);
			}

			// Token: 0x06000808 RID: 2056 RVA: 0x00012BE8 File Offset: 0x00010DE8
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.task.Status;
			}

			// Token: 0x04000228 RID: 552
			private Func<UniTask<T>> factory;

			// Token: 0x04000229 RID: 553
			private UniTask<T> task;

			// Token: 0x0400022A RID: 554
			private UniTask<T>.Awaiter awaiter;
		}

		// Token: 0x02000178 RID: 376
		private sealed class DeferPromiseWithState<TState> : IUniTaskSource, IValueTaskSource
		{
			// Token: 0x06000809 RID: 2057 RVA: 0x00012BF5 File Offset: 0x00010DF5
			public DeferPromiseWithState(TState argument, Func<TState, UniTask> factory)
			{
				this.argument = argument;
				this.factory = factory;
			}

			// Token: 0x0600080A RID: 2058 RVA: 0x00012C0B File Offset: 0x00010E0B
			public void GetResult(short token)
			{
				this.awaiter.GetResult();
			}

			// Token: 0x0600080B RID: 2059 RVA: 0x00012C18 File Offset: 0x00010E18
			public UniTaskStatus GetStatus(short token)
			{
				Func<TState, UniTask> func = Interlocked.Exchange<Func<TState, UniTask>>(ref this.factory, null);
				if (func != null)
				{
					this.task = func(this.argument);
					this.awaiter = this.task.GetAwaiter();
				}
				return this.task.Status;
			}

			// Token: 0x0600080C RID: 2060 RVA: 0x00012C63 File Offset: 0x00010E63
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.awaiter.SourceOnCompleted(continuation, state);
			}

			// Token: 0x0600080D RID: 2061 RVA: 0x00012C72 File Offset: 0x00010E72
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.task.Status;
			}

			// Token: 0x0400022B RID: 555
			private Func<TState, UniTask> factory;

			// Token: 0x0400022C RID: 556
			private TState argument;

			// Token: 0x0400022D RID: 557
			private UniTask task;

			// Token: 0x0400022E RID: 558
			private UniTask.Awaiter awaiter;
		}

		// Token: 0x02000179 RID: 377
		private sealed class DeferPromiseWithState<TState, TResult> : IUniTaskSource<TResult>, IUniTaskSource, IValueTaskSource, IValueTaskSource<TResult>
		{
			// Token: 0x0600080E RID: 2062 RVA: 0x00012C7F File Offset: 0x00010E7F
			public DeferPromiseWithState(TState argument, Func<TState, UniTask<TResult>> factory)
			{
				this.argument = argument;
				this.factory = factory;
			}

			// Token: 0x0600080F RID: 2063 RVA: 0x00012C95 File Offset: 0x00010E95
			public TResult GetResult(short token)
			{
				return this.awaiter.GetResult();
			}

			// Token: 0x06000810 RID: 2064 RVA: 0x00012CA2 File Offset: 0x00010EA2
			void IUniTaskSource.GetResult(short token)
			{
				this.awaiter.GetResult();
			}

			// Token: 0x06000811 RID: 2065 RVA: 0x00012CB0 File Offset: 0x00010EB0
			public UniTaskStatus GetStatus(short token)
			{
				Func<TState, UniTask<TResult>> func = Interlocked.Exchange<Func<TState, UniTask<TResult>>>(ref this.factory, null);
				if (func != null)
				{
					this.task = func(this.argument);
					this.awaiter = this.task.GetAwaiter();
				}
				return this.task.Status;
			}

			// Token: 0x06000812 RID: 2066 RVA: 0x00012CFB File Offset: 0x00010EFB
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.awaiter.SourceOnCompleted(continuation, state);
			}

			// Token: 0x06000813 RID: 2067 RVA: 0x00012D0A File Offset: 0x00010F0A
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.task.Status;
			}

			// Token: 0x0400022F RID: 559
			private Func<TState, UniTask<TResult>> factory;

			// Token: 0x04000230 RID: 560
			private TState argument;

			// Token: 0x04000231 RID: 561
			private UniTask<TResult> task;

			// Token: 0x04000232 RID: 562
			private UniTask<TResult>.Awaiter awaiter;
		}

		// Token: 0x0200017A RID: 378
		private sealed class NeverPromise<T> : IUniTaskSource<!0>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>
		{
			// Token: 0x06000814 RID: 2068 RVA: 0x00012D17 File Offset: 0x00010F17
			public NeverPromise(CancellationToken cancellationToken)
			{
				this.cancellationToken = cancellationToken;
				if (this.cancellationToken.CanBeCanceled)
				{
					this.cancellationToken.RegisterWithoutCaptureExecutionContext(UniTask.NeverPromise<T>.cancellationCallback, this);
				}
			}

			// Token: 0x06000815 RID: 2069 RVA: 0x00012D48 File Offset: 0x00010F48
			private static void CancellationCallback(object state)
			{
				UniTask.NeverPromise<T> neverPromise = (UniTask.NeverPromise<T>)state;
				neverPromise.core.TrySetCanceled(neverPromise.cancellationToken);
			}

			// Token: 0x06000816 RID: 2070 RVA: 0x00012D6E File Offset: 0x00010F6E
			public T GetResult(short token)
			{
				return this.core.GetResult(token);
			}

			// Token: 0x06000817 RID: 2071 RVA: 0x00012D7C File Offset: 0x00010F7C
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000818 RID: 2072 RVA: 0x00012D8A File Offset: 0x00010F8A
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000819 RID: 2073 RVA: 0x00012D97 File Offset: 0x00010F97
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600081A RID: 2074 RVA: 0x00012DA7 File Offset: 0x00010FA7
			void IUniTaskSource.GetResult(short token)
			{
				this.core.GetResult(token);
			}

			// Token: 0x04000233 RID: 563
			private static readonly Action<object> cancellationCallback = new Action<object>(UniTask.NeverPromise<T>.CancellationCallback);

			// Token: 0x04000234 RID: 564
			private CancellationToken cancellationToken;

			// Token: 0x04000235 RID: 565
			private UniTaskCompletionSourceCore<T> core;
		}

		// Token: 0x0200017B RID: 379
		private sealed class WaitUntilPromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.WaitUntilPromise>
		{
			// Token: 0x17000065 RID: 101
			// (get) Token: 0x0600081C RID: 2076 RVA: 0x00012DC9 File Offset: 0x00010FC9
			public ref UniTask.WaitUntilPromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x0600081D RID: 2077 RVA: 0x00012DD1 File Offset: 0x00010FD1
			static WaitUntilPromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.WaitUntilPromise), () => UniTask.WaitUntilPromise.pool.Size);
			}

			// Token: 0x0600081E RID: 2078 RVA: 0x00012DF2 File Offset: 0x00010FF2
			private WaitUntilPromise()
			{
			}

			// Token: 0x0600081F RID: 2079 RVA: 0x00012DFC File Offset: 0x00010FFC
			public static IUniTaskSource Create(Func<bool> predicate, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.WaitUntilPromise waitUntilPromise;
				if (!UniTask.WaitUntilPromise.pool.TryPop(out waitUntilPromise))
				{
					waitUntilPromise = new UniTask.WaitUntilPromise();
				}
				waitUntilPromise.predicate = predicate;
				waitUntilPromise.cancellationToken = cancellationToken;
				waitUntilPromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					waitUntilPromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.WaitUntilPromise waitUntilPromise2 = (UniTask.WaitUntilPromise)state;
						waitUntilPromise2.core.TrySetCanceled(waitUntilPromise2.cancellationToken);
					}, waitUntilPromise);
				}
				PlayerLoopHelper.AddAction(timing, waitUntilPromise);
				token = waitUntilPromise.core.Version;
				return waitUntilPromise;
			}

			// Token: 0x06000820 RID: 2080 RVA: 0x00012E94 File Offset: 0x00011094
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x06000821 RID: 2081 RVA: 0x00012EE0 File Offset: 0x000110E0
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000822 RID: 2082 RVA: 0x00012EEE File Offset: 0x000110EE
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000823 RID: 2083 RVA: 0x00012EFB File Offset: 0x000110FB
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000824 RID: 2084 RVA: 0x00012F0C File Offset: 0x0001110C
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				try
				{
					if (!this.predicate())
					{
						return true;
					}
				}
				catch (Exception error)
				{
					this.core.TrySetException(error);
					return false;
				}
				this.core.TrySetResult(null);
				return false;
			}

			// Token: 0x06000825 RID: 2085 RVA: 0x00012F80 File Offset: 0x00011180
			private bool TryReturn()
			{
				this.core.Reset();
				this.predicate = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.WaitUntilPromise.pool.TryPush(this);
			}

			// Token: 0x04000236 RID: 566
			private static TaskPool<UniTask.WaitUntilPromise> pool;

			// Token: 0x04000237 RID: 567
			private UniTask.WaitUntilPromise nextNode;

			// Token: 0x04000238 RID: 568
			private Func<bool> predicate;

			// Token: 0x04000239 RID: 569
			private CancellationToken cancellationToken;

			// Token: 0x0400023A RID: 570
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x0400023B RID: 571
			private bool cancelImmediately;

			// Token: 0x0400023C RID: 572
			private UniTaskCompletionSourceCore<object> core;
		}

		// Token: 0x0200017C RID: 380
		private sealed class WaitUntilPromise<T> : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.WaitUntilPromise<T>>
		{
			// Token: 0x17000066 RID: 102
			// (get) Token: 0x06000826 RID: 2086 RVA: 0x00012FBD File Offset: 0x000111BD
			public ref UniTask.WaitUntilPromise<T> NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000827 RID: 2087 RVA: 0x00012FC5 File Offset: 0x000111C5
			static WaitUntilPromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.WaitUntilPromise<T>), () => UniTask.WaitUntilPromise<T>.pool.Size);
			}

			// Token: 0x06000828 RID: 2088 RVA: 0x00012FE6 File Offset: 0x000111E6
			private WaitUntilPromise()
			{
			}

			// Token: 0x06000829 RID: 2089 RVA: 0x00012FF0 File Offset: 0x000111F0
			public static IUniTaskSource Create(T argument, Func<T, bool> predicate, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.WaitUntilPromise<T> waitUntilPromise;
				if (!UniTask.WaitUntilPromise<T>.pool.TryPop(out waitUntilPromise))
				{
					waitUntilPromise = new UniTask.WaitUntilPromise<T>();
				}
				waitUntilPromise.predicate = predicate;
				waitUntilPromise.argument = argument;
				waitUntilPromise.cancellationToken = cancellationToken;
				waitUntilPromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					waitUntilPromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.WaitUntilPromise<T> waitUntilPromise2 = (UniTask.WaitUntilPromise<T>)state;
						waitUntilPromise2.core.TrySetCanceled(waitUntilPromise2.cancellationToken);
					}, waitUntilPromise);
				}
				PlayerLoopHelper.AddAction(timing, waitUntilPromise);
				token = waitUntilPromise.core.Version;
				return waitUntilPromise;
			}

			// Token: 0x0600082A RID: 2090 RVA: 0x00013090 File Offset: 0x00011290
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x0600082B RID: 2091 RVA: 0x000130DC File Offset: 0x000112DC
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600082C RID: 2092 RVA: 0x000130EA File Offset: 0x000112EA
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600082D RID: 2093 RVA: 0x000130F7 File Offset: 0x000112F7
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600082E RID: 2094 RVA: 0x00013108 File Offset: 0x00011308
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				try
				{
					if (!this.predicate(this.argument))
					{
						return true;
					}
				}
				catch (Exception error)
				{
					this.core.TrySetException(error);
					return false;
				}
				this.core.TrySetResult(null);
				return false;
			}

			// Token: 0x0600082F RID: 2095 RVA: 0x00013184 File Offset: 0x00011384
			private bool TryReturn()
			{
				this.core.Reset();
				this.predicate = null;
				this.argument = default(T);
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.WaitUntilPromise<T>.pool.TryPush(this);
			}

			// Token: 0x0400023D RID: 573
			private static TaskPool<UniTask.WaitUntilPromise<T>> pool;

			// Token: 0x0400023E RID: 574
			private UniTask.WaitUntilPromise<T> nextNode;

			// Token: 0x0400023F RID: 575
			private Func<T, bool> predicate;

			// Token: 0x04000240 RID: 576
			private T argument;

			// Token: 0x04000241 RID: 577
			private CancellationToken cancellationToken;

			// Token: 0x04000242 RID: 578
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04000243 RID: 579
			private bool cancelImmediately;

			// Token: 0x04000244 RID: 580
			private UniTaskCompletionSourceCore<object> core;
		}

		// Token: 0x0200017D RID: 381
		private sealed class WaitWhilePromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.WaitWhilePromise>
		{
			// Token: 0x17000067 RID: 103
			// (get) Token: 0x06000830 RID: 2096 RVA: 0x000131D8 File Offset: 0x000113D8
			public ref UniTask.WaitWhilePromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000831 RID: 2097 RVA: 0x000131E0 File Offset: 0x000113E0
			static WaitWhilePromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.WaitWhilePromise), () => UniTask.WaitWhilePromise.pool.Size);
			}

			// Token: 0x06000832 RID: 2098 RVA: 0x00013201 File Offset: 0x00011401
			private WaitWhilePromise()
			{
			}

			// Token: 0x06000833 RID: 2099 RVA: 0x0001320C File Offset: 0x0001140C
			public static IUniTaskSource Create(Func<bool> predicate, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.WaitWhilePromise waitWhilePromise;
				if (!UniTask.WaitWhilePromise.pool.TryPop(out waitWhilePromise))
				{
					waitWhilePromise = new UniTask.WaitWhilePromise();
				}
				waitWhilePromise.predicate = predicate;
				waitWhilePromise.cancellationToken = cancellationToken;
				waitWhilePromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					waitWhilePromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.WaitWhilePromise waitWhilePromise2 = (UniTask.WaitWhilePromise)state;
						waitWhilePromise2.core.TrySetCanceled(waitWhilePromise2.cancellationToken);
					}, waitWhilePromise);
				}
				PlayerLoopHelper.AddAction(timing, waitWhilePromise);
				token = waitWhilePromise.core.Version;
				return waitWhilePromise;
			}

			// Token: 0x06000834 RID: 2100 RVA: 0x000132A4 File Offset: 0x000114A4
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x06000835 RID: 2101 RVA: 0x000132F0 File Offset: 0x000114F0
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000836 RID: 2102 RVA: 0x000132FE File Offset: 0x000114FE
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000837 RID: 2103 RVA: 0x0001330B File Offset: 0x0001150B
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000838 RID: 2104 RVA: 0x0001331C File Offset: 0x0001151C
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				try
				{
					if (this.predicate())
					{
						return true;
					}
				}
				catch (Exception error)
				{
					this.core.TrySetException(error);
					return false;
				}
				this.core.TrySetResult(null);
				return false;
			}

			// Token: 0x06000839 RID: 2105 RVA: 0x00013390 File Offset: 0x00011590
			private bool TryReturn()
			{
				this.core.Reset();
				this.predicate = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.WaitWhilePromise.pool.TryPush(this);
			}

			// Token: 0x04000245 RID: 581
			private static TaskPool<UniTask.WaitWhilePromise> pool;

			// Token: 0x04000246 RID: 582
			private UniTask.WaitWhilePromise nextNode;

			// Token: 0x04000247 RID: 583
			private Func<bool> predicate;

			// Token: 0x04000248 RID: 584
			private CancellationToken cancellationToken;

			// Token: 0x04000249 RID: 585
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x0400024A RID: 586
			private bool cancelImmediately;

			// Token: 0x0400024B RID: 587
			private UniTaskCompletionSourceCore<object> core;
		}

		// Token: 0x0200017E RID: 382
		private sealed class WaitWhilePromise<T> : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.WaitWhilePromise<T>>
		{
			// Token: 0x17000068 RID: 104
			// (get) Token: 0x0600083A RID: 2106 RVA: 0x000133CD File Offset: 0x000115CD
			public ref UniTask.WaitWhilePromise<T> NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x0600083B RID: 2107 RVA: 0x000133D5 File Offset: 0x000115D5
			static WaitWhilePromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.WaitWhilePromise<T>), () => UniTask.WaitWhilePromise<T>.pool.Size);
			}

			// Token: 0x0600083C RID: 2108 RVA: 0x000133F6 File Offset: 0x000115F6
			private WaitWhilePromise()
			{
			}

			// Token: 0x0600083D RID: 2109 RVA: 0x00013400 File Offset: 0x00011600
			public static IUniTaskSource Create(T argument, Func<T, bool> predicate, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.WaitWhilePromise<T> waitWhilePromise;
				if (!UniTask.WaitWhilePromise<T>.pool.TryPop(out waitWhilePromise))
				{
					waitWhilePromise = new UniTask.WaitWhilePromise<T>();
				}
				waitWhilePromise.predicate = predicate;
				waitWhilePromise.argument = argument;
				waitWhilePromise.cancellationToken = cancellationToken;
				waitWhilePromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					waitWhilePromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.WaitWhilePromise<T> waitWhilePromise2 = (UniTask.WaitWhilePromise<T>)state;
						waitWhilePromise2.core.TrySetCanceled(waitWhilePromise2.cancellationToken);
					}, waitWhilePromise);
				}
				PlayerLoopHelper.AddAction(timing, waitWhilePromise);
				token = waitWhilePromise.core.Version;
				return waitWhilePromise;
			}

			// Token: 0x0600083E RID: 2110 RVA: 0x000134A0 File Offset: 0x000116A0
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x0600083F RID: 2111 RVA: 0x000134EC File Offset: 0x000116EC
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000840 RID: 2112 RVA: 0x000134FA File Offset: 0x000116FA
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000841 RID: 2113 RVA: 0x00013507 File Offset: 0x00011707
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000842 RID: 2114 RVA: 0x00013518 File Offset: 0x00011718
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				try
				{
					if (this.predicate(this.argument))
					{
						return true;
					}
				}
				catch (Exception error)
				{
					this.core.TrySetException(error);
					return false;
				}
				this.core.TrySetResult(null);
				return false;
			}

			// Token: 0x06000843 RID: 2115 RVA: 0x00013594 File Offset: 0x00011794
			private bool TryReturn()
			{
				this.core.Reset();
				this.predicate = null;
				this.argument = default(T);
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.WaitWhilePromise<T>.pool.TryPush(this);
			}

			// Token: 0x0400024C RID: 588
			private static TaskPool<UniTask.WaitWhilePromise<T>> pool;

			// Token: 0x0400024D RID: 589
			private UniTask.WaitWhilePromise<T> nextNode;

			// Token: 0x0400024E RID: 590
			private Func<T, bool> predicate;

			// Token: 0x0400024F RID: 591
			private T argument;

			// Token: 0x04000250 RID: 592
			private CancellationToken cancellationToken;

			// Token: 0x04000251 RID: 593
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04000252 RID: 594
			private bool cancelImmediately;

			// Token: 0x04000253 RID: 595
			private UniTaskCompletionSourceCore<object> core;
		}

		// Token: 0x0200017F RID: 383
		private sealed class WaitUntilCanceledPromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UniTask.WaitUntilCanceledPromise>
		{
			// Token: 0x17000069 RID: 105
			// (get) Token: 0x06000844 RID: 2116 RVA: 0x000135E8 File Offset: 0x000117E8
			public ref UniTask.WaitUntilCanceledPromise NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000845 RID: 2117 RVA: 0x000135F0 File Offset: 0x000117F0
			static WaitUntilCanceledPromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.WaitUntilCanceledPromise), () => UniTask.WaitUntilCanceledPromise.pool.Size);
			}

			// Token: 0x06000846 RID: 2118 RVA: 0x00013611 File Offset: 0x00011811
			private WaitUntilCanceledPromise()
			{
			}

			// Token: 0x06000847 RID: 2119 RVA: 0x0001361C File Offset: 0x0001181C
			public static IUniTaskSource Create(CancellationToken cancellationToken, PlayerLoopTiming timing, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.WaitUntilCanceledPromise waitUntilCanceledPromise;
				if (!UniTask.WaitUntilCanceledPromise.pool.TryPop(out waitUntilCanceledPromise))
				{
					waitUntilCanceledPromise = new UniTask.WaitUntilCanceledPromise();
				}
				waitUntilCanceledPromise.cancellationToken = cancellationToken;
				waitUntilCanceledPromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					waitUntilCanceledPromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						((UniTask.WaitUntilCanceledPromise)state).core.TrySetResult(null);
					}, waitUntilCanceledPromise);
				}
				PlayerLoopHelper.AddAction(timing, waitUntilCanceledPromise);
				token = waitUntilCanceledPromise.core.Version;
				return waitUntilCanceledPromise;
			}

			// Token: 0x06000848 RID: 2120 RVA: 0x000136AC File Offset: 0x000118AC
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x06000849 RID: 2121 RVA: 0x000136F8 File Offset: 0x000118F8
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600084A RID: 2122 RVA: 0x00013706 File Offset: 0x00011906
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600084B RID: 2123 RVA: 0x00013713 File Offset: 0x00011913
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600084C RID: 2124 RVA: 0x00013723 File Offset: 0x00011923
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetResult(null);
					return false;
				}
				return true;
			}

			// Token: 0x0600084D RID: 2125 RVA: 0x00013742 File Offset: 0x00011942
			private bool TryReturn()
			{
				this.core.Reset();
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.WaitUntilCanceledPromise.pool.TryPush(this);
			}

			// Token: 0x04000254 RID: 596
			private static TaskPool<UniTask.WaitUntilCanceledPromise> pool;

			// Token: 0x04000255 RID: 597
			private UniTask.WaitUntilCanceledPromise nextNode;

			// Token: 0x04000256 RID: 598
			private CancellationToken cancellationToken;

			// Token: 0x04000257 RID: 599
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04000258 RID: 600
			private bool cancelImmediately;

			// Token: 0x04000259 RID: 601
			private UniTaskCompletionSourceCore<object> core;
		}

		// Token: 0x02000180 RID: 384
		private sealed class WaitUntilValueChangedUnityObjectPromise<T, U> : IUniTaskSource<U>, IUniTaskSource, IValueTaskSource, IValueTaskSource<U>, IPlayerLoopItem, ITaskPoolNode<UniTask.WaitUntilValueChangedUnityObjectPromise<T, U>>
		{
			// Token: 0x1700006A RID: 106
			// (get) Token: 0x0600084E RID: 2126 RVA: 0x00013778 File Offset: 0x00011978
			public ref UniTask.WaitUntilValueChangedUnityObjectPromise<T, U> NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x0600084F RID: 2127 RVA: 0x00013780 File Offset: 0x00011980
			static WaitUntilValueChangedUnityObjectPromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.WaitUntilValueChangedUnityObjectPromise<T, U>), () => UniTask.WaitUntilValueChangedUnityObjectPromise<T, U>.pool.Size);
			}

			// Token: 0x06000850 RID: 2128 RVA: 0x000137A1 File Offset: 0x000119A1
			private WaitUntilValueChangedUnityObjectPromise()
			{
			}

			// Token: 0x06000851 RID: 2129 RVA: 0x000137AC File Offset: 0x000119AC
			public static IUniTaskSource<U> Create(T target, Func<T, U> monitorFunction, IEqualityComparer<U> equalityComparer, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<U>.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.WaitUntilValueChangedUnityObjectPromise<T, U> waitUntilValueChangedUnityObjectPromise;
				if (!UniTask.WaitUntilValueChangedUnityObjectPromise<T, U>.pool.TryPop(out waitUntilValueChangedUnityObjectPromise))
				{
					waitUntilValueChangedUnityObjectPromise = new UniTask.WaitUntilValueChangedUnityObjectPromise<T, U>();
				}
				waitUntilValueChangedUnityObjectPromise.target = target;
				waitUntilValueChangedUnityObjectPromise.targetAsUnityObject = (target as Object);
				waitUntilValueChangedUnityObjectPromise.monitorFunction = monitorFunction;
				waitUntilValueChangedUnityObjectPromise.currentValue = monitorFunction(target);
				waitUntilValueChangedUnityObjectPromise.equalityComparer = (equalityComparer ?? UnityEqualityComparer.GetDefault<U>());
				waitUntilValueChangedUnityObjectPromise.cancellationToken = cancellationToken;
				waitUntilValueChangedUnityObjectPromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					waitUntilValueChangedUnityObjectPromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.WaitUntilValueChangedUnityObjectPromise<T, U> waitUntilValueChangedUnityObjectPromise2 = (UniTask.WaitUntilValueChangedUnityObjectPromise<T, U>)state;
						waitUntilValueChangedUnityObjectPromise2.core.TrySetCanceled(waitUntilValueChangedUnityObjectPromise2.cancellationToken);
					}, waitUntilValueChangedUnityObjectPromise);
				}
				PlayerLoopHelper.AddAction(timing, waitUntilValueChangedUnityObjectPromise);
				token = waitUntilValueChangedUnityObjectPromise.core.Version;
				return waitUntilValueChangedUnityObjectPromise;
			}

			// Token: 0x06000852 RID: 2130 RVA: 0x0001387C File Offset: 0x00011A7C
			public U GetResult(short token)
			{
				U result;
				try
				{
					result = this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
				return result;
			}

			// Token: 0x06000853 RID: 2131 RVA: 0x000138C8 File Offset: 0x00011AC8
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000854 RID: 2132 RVA: 0x000138D2 File Offset: 0x00011AD2
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000855 RID: 2133 RVA: 0x000138E0 File Offset: 0x00011AE0
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000856 RID: 2134 RVA: 0x000138ED File Offset: 0x00011AED
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000857 RID: 2135 RVA: 0x00013900 File Offset: 0x00011B00
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested || this.targetAsUnityObject == null)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				U u = default(U);
				try
				{
					u = this.monitorFunction(this.target);
					if (this.equalityComparer.Equals(this.currentValue, u))
					{
						return true;
					}
				}
				catch (Exception error)
				{
					this.core.TrySetException(error);
					return false;
				}
				this.core.TrySetResult(u);
				return false;
			}

			// Token: 0x06000858 RID: 2136 RVA: 0x000139A4 File Offset: 0x00011BA4
			private bool TryReturn()
			{
				this.core.Reset();
				this.target = default(T);
				this.currentValue = default(U);
				this.monitorFunction = null;
				this.equalityComparer = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.WaitUntilValueChangedUnityObjectPromise<T, U>.pool.TryPush(this);
			}

			// Token: 0x0400025A RID: 602
			private static TaskPool<UniTask.WaitUntilValueChangedUnityObjectPromise<T, U>> pool;

			// Token: 0x0400025B RID: 603
			private UniTask.WaitUntilValueChangedUnityObjectPromise<T, U> nextNode;

			// Token: 0x0400025C RID: 604
			private T target;

			// Token: 0x0400025D RID: 605
			private Object targetAsUnityObject;

			// Token: 0x0400025E RID: 606
			private U currentValue;

			// Token: 0x0400025F RID: 607
			private Func<T, U> monitorFunction;

			// Token: 0x04000260 RID: 608
			private IEqualityComparer<U> equalityComparer;

			// Token: 0x04000261 RID: 609
			private CancellationToken cancellationToken;

			// Token: 0x04000262 RID: 610
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04000263 RID: 611
			private bool cancelImmediately;

			// Token: 0x04000264 RID: 612
			private UniTaskCompletionSourceCore<U> core;
		}

		// Token: 0x02000181 RID: 385
		private sealed class WaitUntilValueChangedStandardObjectPromise<T, U> : IUniTaskSource<U>, IUniTaskSource, IValueTaskSource, IValueTaskSource<U>, IPlayerLoopItem, ITaskPoolNode<UniTask.WaitUntilValueChangedStandardObjectPromise<T, U>> where T : class
		{
			// Token: 0x1700006B RID: 107
			// (get) Token: 0x06000859 RID: 2137 RVA: 0x00013A0B File Offset: 0x00011C0B
			public ref UniTask.WaitUntilValueChangedStandardObjectPromise<T, U> NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x0600085A RID: 2138 RVA: 0x00013A13 File Offset: 0x00011C13
			static WaitUntilValueChangedStandardObjectPromise()
			{
				TaskPool.RegisterSizeGetter(typeof(UniTask.WaitUntilValueChangedStandardObjectPromise<T, U>), () => UniTask.WaitUntilValueChangedStandardObjectPromise<T, U>.pool.Size);
			}

			// Token: 0x0600085B RID: 2139 RVA: 0x00013A34 File Offset: 0x00011C34
			private WaitUntilValueChangedStandardObjectPromise()
			{
			}

			// Token: 0x0600085C RID: 2140 RVA: 0x00013A3C File Offset: 0x00011C3C
			public static IUniTaskSource<U> Create(T target, Func<T, U> monitorFunction, IEqualityComparer<U> equalityComparer, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<U>.CreateFromCanceled(cancellationToken, out token);
				}
				UniTask.WaitUntilValueChangedStandardObjectPromise<T, U> waitUntilValueChangedStandardObjectPromise;
				if (!UniTask.WaitUntilValueChangedStandardObjectPromise<T, U>.pool.TryPop(out waitUntilValueChangedStandardObjectPromise))
				{
					waitUntilValueChangedStandardObjectPromise = new UniTask.WaitUntilValueChangedStandardObjectPromise<T, U>();
				}
				waitUntilValueChangedStandardObjectPromise.target = new WeakReference<T>(target, false);
				waitUntilValueChangedStandardObjectPromise.monitorFunction = monitorFunction;
				waitUntilValueChangedStandardObjectPromise.currentValue = monitorFunction(target);
				waitUntilValueChangedStandardObjectPromise.equalityComparer = (equalityComparer ?? UnityEqualityComparer.GetDefault<U>());
				waitUntilValueChangedStandardObjectPromise.cancellationToken = cancellationToken;
				waitUntilValueChangedStandardObjectPromise.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					waitUntilValueChangedStandardObjectPromise.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UniTask.WaitUntilValueChangedStandardObjectPromise<T, U> waitUntilValueChangedStandardObjectPromise2 = (UniTask.WaitUntilValueChangedStandardObjectPromise<T, U>)state;
						waitUntilValueChangedStandardObjectPromise2.core.TrySetCanceled(waitUntilValueChangedStandardObjectPromise2.cancellationToken);
					}, waitUntilValueChangedStandardObjectPromise);
				}
				PlayerLoopHelper.AddAction(timing, waitUntilValueChangedStandardObjectPromise);
				token = waitUntilValueChangedStandardObjectPromise.core.Version;
				return waitUntilValueChangedStandardObjectPromise;
			}

			// Token: 0x0600085D RID: 2141 RVA: 0x00013B04 File Offset: 0x00011D04
			public U GetResult(short token)
			{
				U result;
				try
				{
					result = this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
				return result;
			}

			// Token: 0x0600085E RID: 2142 RVA: 0x00013B50 File Offset: 0x00011D50
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0600085F RID: 2143 RVA: 0x00013B5A File Offset: 0x00011D5A
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000860 RID: 2144 RVA: 0x00013B68 File Offset: 0x00011D68
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000861 RID: 2145 RVA: 0x00013B75 File Offset: 0x00011D75
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000862 RID: 2146 RVA: 0x00013B88 File Offset: 0x00011D88
			public bool MoveNext()
			{
				T arg;
				if (this.cancellationToken.IsCancellationRequested || !this.target.TryGetTarget(out arg))
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				U u = default(U);
				try
				{
					u = this.monitorFunction(arg);
					if (this.equalityComparer.Equals(this.currentValue, u))
					{
						return true;
					}
				}
				catch (Exception error)
				{
					this.core.TrySetException(error);
					return false;
				}
				this.core.TrySetResult(u);
				return false;
			}

			// Token: 0x06000863 RID: 2147 RVA: 0x00013C28 File Offset: 0x00011E28
			private bool TryReturn()
			{
				this.core.Reset();
				this.target = null;
				this.currentValue = default(U);
				this.monitorFunction = null;
				this.equalityComparer = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UniTask.WaitUntilValueChangedStandardObjectPromise<T, U>.pool.TryPush(this);
			}

			// Token: 0x04000265 RID: 613
			private static TaskPool<UniTask.WaitUntilValueChangedStandardObjectPromise<T, U>> pool;

			// Token: 0x04000266 RID: 614
			private UniTask.WaitUntilValueChangedStandardObjectPromise<T, U> nextNode;

			// Token: 0x04000267 RID: 615
			private WeakReference<T> target;

			// Token: 0x04000268 RID: 616
			private U currentValue;

			// Token: 0x04000269 RID: 617
			private Func<T, U> monitorFunction;

			// Token: 0x0400026A RID: 618
			private IEqualityComparer<U> equalityComparer;

			// Token: 0x0400026B RID: 619
			private CancellationToken cancellationToken;

			// Token: 0x0400026C RID: 620
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x0400026D RID: 621
			private bool cancelImmediately;

			// Token: 0x0400026E RID: 622
			private UniTaskCompletionSourceCore<U> core;
		}

		// Token: 0x02000182 RID: 386
		private sealed class WhenAllPromise<T> : IUniTaskSource<T[]>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T[]>
		{
			// Token: 0x06000864 RID: 2148 RVA: 0x00013C8C File Offset: 0x00011E8C
			public WhenAllPromise(UniTask<T>[] tasks, int tasksLength)
			{
				this.completeCount = 0;
				if (tasksLength == 0)
				{
					this.result = Array.Empty<T>();
					this.core.TrySetResult(this.result);
					return;
				}
				this.result = new T[tasksLength];
				int i = 0;
				while (i < tasksLength)
				{
					UniTask<T>.Awaiter awaiter;
					try
					{
						awaiter = tasks[i].GetAwaiter();
					}
					catch (Exception error)
					{
						this.core.TrySetException(error);
						goto IL_A0;
					}
					goto IL_5E;
					IL_A0:
					i++;
					continue;
					IL_5E:
					if (awaiter.IsCompleted)
					{
						UniTask.WhenAllPromise<T>.TryInvokeContinuation(this, awaiter, i);
						goto IL_A0;
					}
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T>, UniTask<T>.Awaiter, int> stateTuple = (StateTuple<UniTask.WhenAllPromise<T>, UniTask<T>.Awaiter, int>)state)
						{
							UniTask.WhenAllPromise<T>.TryInvokeContinuation(stateTuple.Item1, stateTuple.Item2, stateTuple.Item3);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T>, UniTask<T>.Awaiter, int>(this, awaiter, i));
					goto IL_A0;
				}
			}

			// Token: 0x06000865 RID: 2149 RVA: 0x00013D54 File Offset: 0x00011F54
			private static void TryInvokeContinuation(UniTask.WhenAllPromise<T> self, in UniTask<T>.Awaiter awaiter, int i)
			{
				try
				{
					self.result[i] = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completeCount) == self.result.Length)
				{
					self.core.TrySetResult(self.result);
				}
			}

			// Token: 0x06000866 RID: 2150 RVA: 0x00013DBC File Offset: 0x00011FBC
			public T[] GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000867 RID: 2151 RVA: 0x00013DD0 File Offset: 0x00011FD0
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000868 RID: 2152 RVA: 0x00013DDA File Offset: 0x00011FDA
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000869 RID: 2153 RVA: 0x00013DE8 File Offset: 0x00011FE8
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600086A RID: 2154 RVA: 0x00013DF5 File Offset: 0x00011FF5
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0400026F RID: 623
			private T[] result;

			// Token: 0x04000270 RID: 624
			private int completeCount;

			// Token: 0x04000271 RID: 625
			private UniTaskCompletionSourceCore<T[]> core;
		}

		// Token: 0x02000183 RID: 387
		private sealed class WhenAllPromise : IUniTaskSource, IValueTaskSource
		{
			// Token: 0x0600086B RID: 2155 RVA: 0x00013E08 File Offset: 0x00012008
			public WhenAllPromise(UniTask[] tasks, int tasksLength)
			{
				this.tasksLength = tasksLength;
				this.completeCount = 0;
				if (tasksLength == 0)
				{
					this.core.TrySetResult(AsyncUnit.Default);
					return;
				}
				int i = 0;
				while (i < tasksLength)
				{
					UniTask.Awaiter awaiter;
					try
					{
						awaiter = tasks[i].GetAwaiter();
					}
					catch (Exception error)
					{
						this.core.TrySetException(error);
						goto IL_8D;
					}
					goto IL_4D;
					IL_8D:
					i++;
					continue;
					IL_4D:
					if (awaiter.IsCompleted)
					{
						UniTask.WhenAllPromise.TryInvokeContinuation(this, awaiter);
						goto IL_8D;
					}
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise, UniTask.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise, UniTask.Awaiter>)state)
						{
							UniTask.WhenAllPromise.TryInvokeContinuation(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise, UniTask.Awaiter>(this, awaiter));
					goto IL_8D;
				}
			}

			// Token: 0x0600086C RID: 2156 RVA: 0x00013EBC File Offset: 0x000120BC
			private static void TryInvokeContinuation(UniTask.WhenAllPromise self, in UniTask.Awaiter awaiter)
			{
				try
				{
					awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completeCount) == self.tasksLength)
				{
					self.core.TrySetResult(AsyncUnit.Default);
				}
			}

			// Token: 0x0600086D RID: 2157 RVA: 0x00013F18 File Offset: 0x00012118
			public void GetResult(short token)
			{
				GC.SuppressFinalize(this);
				this.core.GetResult(token);
			}

			// Token: 0x0600086E RID: 2158 RVA: 0x00013F2D File Offset: 0x0001212D
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600086F RID: 2159 RVA: 0x00013F3B File Offset: 0x0001213B
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000870 RID: 2160 RVA: 0x00013F48 File Offset: 0x00012148
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x04000272 RID: 626
			private int completeCount;

			// Token: 0x04000273 RID: 627
			private int tasksLength;

			// Token: 0x04000274 RID: 628
			private UniTaskCompletionSourceCore<AsyncUnit> core;
		}

		// Token: 0x02000184 RID: 388
		private sealed class WhenAllPromise<T1, T2> : IUniTaskSource<ValueTuple<T1, T2>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2>>
		{
			// Token: 0x06000871 RID: 2161 RVA: 0x00013F58 File Offset: 0x00012158
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2>.TryInvokeContinuationT2(this, awaiter2);
					return;
				}
				awaiter2.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2>, UniTask<T2>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2>, UniTask<T2>.Awaiter>(this, awaiter2));
			}

			// Token: 0x06000872 RID: 2162 RVA: 0x00014004 File Offset: 0x00012204
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 2)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2>(self.t1, self.t2));
				}
			}

			// Token: 0x06000873 RID: 2163 RVA: 0x0001406C File Offset: 0x0001226C
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 2)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2>(self.t1, self.t2));
				}
			}

			// Token: 0x06000874 RID: 2164 RVA: 0x000140D4 File Offset: 0x000122D4
			public ValueTuple<T1, T2> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000875 RID: 2165 RVA: 0x000140E8 File Offset: 0x000122E8
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000876 RID: 2166 RVA: 0x000140F2 File Offset: 0x000122F2
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000877 RID: 2167 RVA: 0x00014100 File Offset: 0x00012300
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000878 RID: 2168 RVA: 0x0001410D File Offset: 0x0001230D
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x04000275 RID: 629
			private T1 t1;

			// Token: 0x04000276 RID: 630
			private T2 t2;

			// Token: 0x04000277 RID: 631
			private int completedCount;

			// Token: 0x04000278 RID: 632
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2>> core;
		}

		// Token: 0x02000185 RID: 389
		private sealed class WhenAllPromise<T1, T2, T3> : IUniTaskSource<ValueTuple<T1, T2, T3>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3>>
		{
			// Token: 0x06000879 RID: 2169 RVA: 0x00014120 File Offset: 0x00012320
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3>.TryInvokeContinuationT3(this, awaiter3);
					return;
				}
				awaiter3.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3>, UniTask<T3>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3>, UniTask<T3>.Awaiter>(this, awaiter3));
			}

			// Token: 0x0600087A RID: 2170 RVA: 0x00014214 File Offset: 0x00012414
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 3)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3>(self.t1, self.t2, self.t3));
				}
			}

			// Token: 0x0600087B RID: 2171 RVA: 0x00014280 File Offset: 0x00012480
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 3)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3>(self.t1, self.t2, self.t3));
				}
			}

			// Token: 0x0600087C RID: 2172 RVA: 0x000142EC File Offset: 0x000124EC
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 3)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3>(self.t1, self.t2, self.t3));
				}
			}

			// Token: 0x0600087D RID: 2173 RVA: 0x00014358 File Offset: 0x00012558
			public ValueTuple<T1, T2, T3> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x0600087E RID: 2174 RVA: 0x0001436C File Offset: 0x0001256C
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0600087F RID: 2175 RVA: 0x00014376 File Offset: 0x00012576
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000880 RID: 2176 RVA: 0x00014384 File Offset: 0x00012584
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000881 RID: 2177 RVA: 0x00014391 File Offset: 0x00012591
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x04000279 RID: 633
			private T1 t1;

			// Token: 0x0400027A RID: 634
			private T2 t2;

			// Token: 0x0400027B RID: 635
			private T3 t3;

			// Token: 0x0400027C RID: 636
			private int completedCount;

			// Token: 0x0400027D RID: 637
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3>> core;
		}

		// Token: 0x02000186 RID: 390
		private sealed class WhenAllPromise<T1, T2, T3, T4> : IUniTaskSource<ValueTuple<T1, T2, T3, T4>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4>>
		{
			// Token: 0x06000882 RID: 2178 RVA: 0x000143A4 File Offset: 0x000125A4
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4>.TryInvokeContinuationT4(this, awaiter4);
					return;
				}
				awaiter4.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T4>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4>, UniTask<T4>.Awaiter>(this, awaiter4));
			}

			// Token: 0x06000883 RID: 2179 RVA: 0x000144E0 File Offset: 0x000126E0
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 4)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4>(self.t1, self.t2, self.t3, self.t4));
				}
			}

			// Token: 0x06000884 RID: 2180 RVA: 0x00014554 File Offset: 0x00012754
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 4)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4>(self.t1, self.t2, self.t3, self.t4));
				}
			}

			// Token: 0x06000885 RID: 2181 RVA: 0x000145C8 File Offset: 0x000127C8
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 4)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4>(self.t1, self.t2, self.t3, self.t4));
				}
			}

			// Token: 0x06000886 RID: 2182 RVA: 0x0001463C File Offset: 0x0001283C
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 4)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4>(self.t1, self.t2, self.t3, self.t4));
				}
			}

			// Token: 0x06000887 RID: 2183 RVA: 0x000146B0 File Offset: 0x000128B0
			public ValueTuple<T1, T2, T3, T4> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000888 RID: 2184 RVA: 0x000146C4 File Offset: 0x000128C4
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000889 RID: 2185 RVA: 0x000146CE File Offset: 0x000128CE
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600088A RID: 2186 RVA: 0x000146DC File Offset: 0x000128DC
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600088B RID: 2187 RVA: 0x000146E9 File Offset: 0x000128E9
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0400027E RID: 638
			private T1 t1;

			// Token: 0x0400027F RID: 639
			private T2 t2;

			// Token: 0x04000280 RID: 640
			private T3 t3;

			// Token: 0x04000281 RID: 641
			private T4 t4;

			// Token: 0x04000282 RID: 642
			private int completedCount;

			// Token: 0x04000283 RID: 643
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4>> core;
		}

		// Token: 0x02000187 RID: 391
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5>>
		{
			// Token: 0x0600088C RID: 2188 RVA: 0x000146FC File Offset: 0x000128FC
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT5(this, awaiter5);
					return;
				}
				awaiter5.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T5>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5>, UniTask<T5>.Awaiter>(this, awaiter5));
			}

			// Token: 0x0600088D RID: 2189 RVA: 0x00014880 File Offset: 0x00012A80
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 5)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5>(self.t1, self.t2, self.t3, self.t4, self.t5));
				}
			}

			// Token: 0x0600088E RID: 2190 RVA: 0x000148F8 File Offset: 0x00012AF8
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 5)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5>(self.t1, self.t2, self.t3, self.t4, self.t5));
				}
			}

			// Token: 0x0600088F RID: 2191 RVA: 0x00014970 File Offset: 0x00012B70
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 5)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5>(self.t1, self.t2, self.t3, self.t4, self.t5));
				}
			}

			// Token: 0x06000890 RID: 2192 RVA: 0x000149E8 File Offset: 0x00012BE8
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 5)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5>(self.t1, self.t2, self.t3, self.t4, self.t5));
				}
			}

			// Token: 0x06000891 RID: 2193 RVA: 0x00014A60 File Offset: 0x00012C60
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 5)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5>(self.t1, self.t2, self.t3, self.t4, self.t5));
				}
			}

			// Token: 0x06000892 RID: 2194 RVA: 0x00014AD8 File Offset: 0x00012CD8
			public ValueTuple<T1, T2, T3, T4, T5> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000893 RID: 2195 RVA: 0x00014AEC File Offset: 0x00012CEC
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000894 RID: 2196 RVA: 0x00014AF6 File Offset: 0x00012CF6
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000895 RID: 2197 RVA: 0x00014B04 File Offset: 0x00012D04
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000896 RID: 2198 RVA: 0x00014B11 File Offset: 0x00012D11
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x04000284 RID: 644
			private T1 t1;

			// Token: 0x04000285 RID: 645
			private T2 t2;

			// Token: 0x04000286 RID: 646
			private T3 t3;

			// Token: 0x04000287 RID: 647
			private T4 t4;

			// Token: 0x04000288 RID: 648
			private T5 t5;

			// Token: 0x04000289 RID: 649
			private int completedCount;

			// Token: 0x0400028A RID: 650
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5>> core;
		}

		// Token: 0x02000188 RID: 392
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6>>
		{
			// Token: 0x06000897 RID: 2199 RVA: 0x00014B24 File Offset: 0x00012D24
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT6(this, awaiter6);
					return;
				}
				awaiter6.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T6>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6>, UniTask<T6>.Awaiter>(this, awaiter6));
			}

			// Token: 0x06000898 RID: 2200 RVA: 0x00014CF4 File Offset: 0x00012EF4
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 6)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
				}
			}

			// Token: 0x06000899 RID: 2201 RVA: 0x00014D74 File Offset: 0x00012F74
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 6)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
				}
			}

			// Token: 0x0600089A RID: 2202 RVA: 0x00014DF4 File Offset: 0x00012FF4
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 6)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
				}
			}

			// Token: 0x0600089B RID: 2203 RVA: 0x00014E74 File Offset: 0x00013074
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 6)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
				}
			}

			// Token: 0x0600089C RID: 2204 RVA: 0x00014EF4 File Offset: 0x000130F4
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 6)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
				}
			}

			// Token: 0x0600089D RID: 2205 RVA: 0x00014F74 File Offset: 0x00013174
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 6)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6));
				}
			}

			// Token: 0x0600089E RID: 2206 RVA: 0x00014FF4 File Offset: 0x000131F4
			public ValueTuple<T1, T2, T3, T4, T5, T6> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x0600089F RID: 2207 RVA: 0x00015008 File Offset: 0x00013208
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x060008A0 RID: 2208 RVA: 0x00015012 File Offset: 0x00013212
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060008A1 RID: 2209 RVA: 0x00015020 File Offset: 0x00013220
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060008A2 RID: 2210 RVA: 0x0001502D File Offset: 0x0001322D
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0400028B RID: 651
			private T1 t1;

			// Token: 0x0400028C RID: 652
			private T2 t2;

			// Token: 0x0400028D RID: 653
			private T3 t3;

			// Token: 0x0400028E RID: 654
			private T4 t4;

			// Token: 0x0400028F RID: 655
			private T5 t5;

			// Token: 0x04000290 RID: 656
			private T6 t6;

			// Token: 0x04000291 RID: 657
			private int completedCount;

			// Token: 0x04000292 RID: 658
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6>> core;
		}

		// Token: 0x02000189 RID: 393
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>
		{
			// Token: 0x060008A3 RID: 2211 RVA: 0x00015040 File Offset: 0x00013240
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT7(this, awaiter7);
					return;
				}
				awaiter7.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T7>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T7>.Awaiter>(this, awaiter7));
			}

			// Token: 0x060008A4 RID: 2212 RVA: 0x00015258 File Offset: 0x00013458
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 7)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
				}
			}

			// Token: 0x060008A5 RID: 2213 RVA: 0x000152DC File Offset: 0x000134DC
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 7)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
				}
			}

			// Token: 0x060008A6 RID: 2214 RVA: 0x00015360 File Offset: 0x00013560
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 7)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
				}
			}

			// Token: 0x060008A7 RID: 2215 RVA: 0x000153E4 File Offset: 0x000135E4
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 7)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
				}
			}

			// Token: 0x060008A8 RID: 2216 RVA: 0x00015468 File Offset: 0x00013668
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 7)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
				}
			}

			// Token: 0x060008A9 RID: 2217 RVA: 0x000154EC File Offset: 0x000136EC
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 7)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
				}
			}

			// Token: 0x060008AA RID: 2218 RVA: 0x00015570 File Offset: 0x00013770
			private static void TryInvokeContinuationT7(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T7>.Awaiter awaiter)
			{
				try
				{
					self.t7 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 7)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7));
				}
			}

			// Token: 0x060008AB RID: 2219 RVA: 0x000155F4 File Offset: 0x000137F4
			public ValueTuple<T1, T2, T3, T4, T5, T6, T7> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060008AC RID: 2220 RVA: 0x00015608 File Offset: 0x00013808
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x060008AD RID: 2221 RVA: 0x00015612 File Offset: 0x00013812
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060008AE RID: 2222 RVA: 0x00015620 File Offset: 0x00013820
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060008AF RID: 2223 RVA: 0x0001562D File Offset: 0x0001382D
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x04000293 RID: 659
			private T1 t1;

			// Token: 0x04000294 RID: 660
			private T2 t2;

			// Token: 0x04000295 RID: 661
			private T3 t3;

			// Token: 0x04000296 RID: 662
			private T4 t4;

			// Token: 0x04000297 RID: 663
			private T5 t5;

			// Token: 0x04000298 RID: 664
			private T6 t6;

			// Token: 0x04000299 RID: 665
			private T7 t7;

			// Token: 0x0400029A RID: 666
			private int completedCount;

			// Token: 0x0400029B RID: 667
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> core;
		}

		// Token: 0x0200018A RID: 394
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>>
		{
			// Token: 0x060008B0 RID: 2224 RVA: 0x00015640 File Offset: 0x00013840
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT8(this, awaiter8);
					return;
				}
				awaiter8.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T8>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T8>.Awaiter>(this, awaiter8));
			}

			// Token: 0x060008B1 RID: 2225 RVA: 0x000158A4 File Offset: 0x00013AA4
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 8)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8>(self.t8)));
				}
			}

			// Token: 0x060008B2 RID: 2226 RVA: 0x00015934 File Offset: 0x00013B34
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 8)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8>(self.t8)));
				}
			}

			// Token: 0x060008B3 RID: 2227 RVA: 0x000159C4 File Offset: 0x00013BC4
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 8)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8>(self.t8)));
				}
			}

			// Token: 0x060008B4 RID: 2228 RVA: 0x00015A54 File Offset: 0x00013C54
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 8)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8>(self.t8)));
				}
			}

			// Token: 0x060008B5 RID: 2229 RVA: 0x00015AE4 File Offset: 0x00013CE4
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 8)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8>(self.t8)));
				}
			}

			// Token: 0x060008B6 RID: 2230 RVA: 0x00015B74 File Offset: 0x00013D74
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 8)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8>(self.t8)));
				}
			}

			// Token: 0x060008B7 RID: 2231 RVA: 0x00015C04 File Offset: 0x00013E04
			private static void TryInvokeContinuationT7(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T7>.Awaiter awaiter)
			{
				try
				{
					self.t7 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 8)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8>(self.t8)));
				}
			}

			// Token: 0x060008B8 RID: 2232 RVA: 0x00015C94 File Offset: 0x00013E94
			private static void TryInvokeContinuationT8(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T8>.Awaiter awaiter)
			{
				try
				{
					self.t8 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 8)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8>(self.t8)));
				}
			}

			// Token: 0x060008B9 RID: 2233 RVA: 0x00015D24 File Offset: 0x00013F24
			public ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060008BA RID: 2234 RVA: 0x00015D38 File Offset: 0x00013F38
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x060008BB RID: 2235 RVA: 0x00015D42 File Offset: 0x00013F42
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060008BC RID: 2236 RVA: 0x00015D50 File Offset: 0x00013F50
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060008BD RID: 2237 RVA: 0x00015D5D File Offset: 0x00013F5D
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0400029C RID: 668
			private T1 t1;

			// Token: 0x0400029D RID: 669
			private T2 t2;

			// Token: 0x0400029E RID: 670
			private T3 t3;

			// Token: 0x0400029F RID: 671
			private T4 t4;

			// Token: 0x040002A0 RID: 672
			private T5 t5;

			// Token: 0x040002A1 RID: 673
			private T6 t6;

			// Token: 0x040002A2 RID: 674
			private T7 t7;

			// Token: 0x040002A3 RID: 675
			private T8 t8;

			// Token: 0x040002A4 RID: 676
			private int completedCount;

			// Token: 0x040002A5 RID: 677
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8>>> core;
		}

		// Token: 0x0200018B RID: 395
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>>
		{
			// Token: 0x060008BE RID: 2238 RVA: 0x00015D70 File Offset: 0x00013F70
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT9(this, awaiter9);
					return;
				}
				awaiter9.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T9>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T9>.Awaiter>(this, awaiter9));
			}

			// Token: 0x060008BF RID: 2239 RVA: 0x0001601C File Offset: 0x0001421C
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 9)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9>(self.t8, self.t9)));
				}
			}

			// Token: 0x060008C0 RID: 2240 RVA: 0x000160B4 File Offset: 0x000142B4
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 9)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9>(self.t8, self.t9)));
				}
			}

			// Token: 0x060008C1 RID: 2241 RVA: 0x0001614C File Offset: 0x0001434C
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 9)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9>(self.t8, self.t9)));
				}
			}

			// Token: 0x060008C2 RID: 2242 RVA: 0x000161E4 File Offset: 0x000143E4
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 9)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9>(self.t8, self.t9)));
				}
			}

			// Token: 0x060008C3 RID: 2243 RVA: 0x0001627C File Offset: 0x0001447C
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 9)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9>(self.t8, self.t9)));
				}
			}

			// Token: 0x060008C4 RID: 2244 RVA: 0x00016314 File Offset: 0x00014514
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 9)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9>(self.t8, self.t9)));
				}
			}

			// Token: 0x060008C5 RID: 2245 RVA: 0x000163AC File Offset: 0x000145AC
			private static void TryInvokeContinuationT7(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T7>.Awaiter awaiter)
			{
				try
				{
					self.t7 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 9)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9>(self.t8, self.t9)));
				}
			}

			// Token: 0x060008C6 RID: 2246 RVA: 0x00016444 File Offset: 0x00014644
			private static void TryInvokeContinuationT8(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T8>.Awaiter awaiter)
			{
				try
				{
					self.t8 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 9)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9>(self.t8, self.t9)));
				}
			}

			// Token: 0x060008C7 RID: 2247 RVA: 0x000164DC File Offset: 0x000146DC
			private static void TryInvokeContinuationT9(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T9>.Awaiter awaiter)
			{
				try
				{
					self.t9 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 9)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9>(self.t8, self.t9)));
				}
			}

			// Token: 0x060008C8 RID: 2248 RVA: 0x00016574 File Offset: 0x00014774
			public ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060008C9 RID: 2249 RVA: 0x00016588 File Offset: 0x00014788
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x060008CA RID: 2250 RVA: 0x00016592 File Offset: 0x00014792
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060008CB RID: 2251 RVA: 0x000165A0 File Offset: 0x000147A0
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060008CC RID: 2252 RVA: 0x000165AD File Offset: 0x000147AD
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x040002A6 RID: 678
			private T1 t1;

			// Token: 0x040002A7 RID: 679
			private T2 t2;

			// Token: 0x040002A8 RID: 680
			private T3 t3;

			// Token: 0x040002A9 RID: 681
			private T4 t4;

			// Token: 0x040002AA RID: 682
			private T5 t5;

			// Token: 0x040002AB RID: 683
			private T6 t6;

			// Token: 0x040002AC RID: 684
			private T7 t7;

			// Token: 0x040002AD RID: 685
			private T8 t8;

			// Token: 0x040002AE RID: 686
			private T9 t9;

			// Token: 0x040002AF RID: 687
			private int completedCount;

			// Token: 0x040002B0 RID: 688
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9>>> core;
		}

		// Token: 0x0200018C RID: 396
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>>
		{
			// Token: 0x060008CD RID: 2253 RVA: 0x000165C0 File Offset: 0x000147C0
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT10(this, awaiter10);
					return;
				}
				awaiter10.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T10>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T10>.Awaiter>(this, awaiter10));
			}

			// Token: 0x060008CE RID: 2254 RVA: 0x000168B8 File Offset: 0x00014AB8
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008CF RID: 2255 RVA: 0x00016954 File Offset: 0x00014B54
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008D0 RID: 2256 RVA: 0x000169F0 File Offset: 0x00014BF0
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008D1 RID: 2257 RVA: 0x00016A8C File Offset: 0x00014C8C
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008D2 RID: 2258 RVA: 0x00016B28 File Offset: 0x00014D28
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008D3 RID: 2259 RVA: 0x00016BC4 File Offset: 0x00014DC4
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008D4 RID: 2260 RVA: 0x00016C60 File Offset: 0x00014E60
			private static void TryInvokeContinuationT7(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T7>.Awaiter awaiter)
			{
				try
				{
					self.t7 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008D5 RID: 2261 RVA: 0x00016CFC File Offset: 0x00014EFC
			private static void TryInvokeContinuationT8(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T8>.Awaiter awaiter)
			{
				try
				{
					self.t8 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008D6 RID: 2262 RVA: 0x00016D98 File Offset: 0x00014F98
			private static void TryInvokeContinuationT9(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T9>.Awaiter awaiter)
			{
				try
				{
					self.t9 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008D7 RID: 2263 RVA: 0x00016E34 File Offset: 0x00015034
			private static void TryInvokeContinuationT10(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T10>.Awaiter awaiter)
			{
				try
				{
					self.t10 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 10)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10>(self.t8, self.t9, self.t10)));
				}
			}

			// Token: 0x060008D8 RID: 2264 RVA: 0x00016ED0 File Offset: 0x000150D0
			public ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060008D9 RID: 2265 RVA: 0x00016EE4 File Offset: 0x000150E4
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x060008DA RID: 2266 RVA: 0x00016EEE File Offset: 0x000150EE
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060008DB RID: 2267 RVA: 0x00016EFC File Offset: 0x000150FC
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060008DC RID: 2268 RVA: 0x00016F09 File Offset: 0x00015109
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x040002B1 RID: 689
			private T1 t1;

			// Token: 0x040002B2 RID: 690
			private T2 t2;

			// Token: 0x040002B3 RID: 691
			private T3 t3;

			// Token: 0x040002B4 RID: 692
			private T4 t4;

			// Token: 0x040002B5 RID: 693
			private T5 t5;

			// Token: 0x040002B6 RID: 694
			private T6 t6;

			// Token: 0x040002B7 RID: 695
			private T7 t7;

			// Token: 0x040002B8 RID: 696
			private T8 t8;

			// Token: 0x040002B9 RID: 697
			private T9 t9;

			// Token: 0x040002BA RID: 698
			private T10 t10;

			// Token: 0x040002BB RID: 699
			private int completedCount;

			// Token: 0x040002BC RID: 700
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10>>> core;
		}

		// Token: 0x0200018D RID: 397
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>>
		{
			// Token: 0x060008DD RID: 2269 RVA: 0x00016F1C File Offset: 0x0001511C
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT11(this, awaiter11);
					return;
				}
				awaiter11.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T11>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T11>.Awaiter>(this, awaiter11));
			}

			// Token: 0x060008DE RID: 2270 RVA: 0x0001725C File Offset: 0x0001545C
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008DF RID: 2271 RVA: 0x00017300 File Offset: 0x00015500
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E0 RID: 2272 RVA: 0x000173A4 File Offset: 0x000155A4
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E1 RID: 2273 RVA: 0x00017448 File Offset: 0x00015648
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E2 RID: 2274 RVA: 0x000174EC File Offset: 0x000156EC
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E3 RID: 2275 RVA: 0x00017590 File Offset: 0x00015790
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E4 RID: 2276 RVA: 0x00017634 File Offset: 0x00015834
			private static void TryInvokeContinuationT7(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T7>.Awaiter awaiter)
			{
				try
				{
					self.t7 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E5 RID: 2277 RVA: 0x000176D8 File Offset: 0x000158D8
			private static void TryInvokeContinuationT8(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T8>.Awaiter awaiter)
			{
				try
				{
					self.t8 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E6 RID: 2278 RVA: 0x0001777C File Offset: 0x0001597C
			private static void TryInvokeContinuationT9(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T9>.Awaiter awaiter)
			{
				try
				{
					self.t9 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E7 RID: 2279 RVA: 0x00017820 File Offset: 0x00015A20
			private static void TryInvokeContinuationT10(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T10>.Awaiter awaiter)
			{
				try
				{
					self.t10 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E8 RID: 2280 RVA: 0x000178C4 File Offset: 0x00015AC4
			private static void TryInvokeContinuationT11(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T11>.Awaiter awaiter)
			{
				try
				{
					self.t11 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 11)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11>(self.t8, self.t9, self.t10, self.t11)));
				}
			}

			// Token: 0x060008E9 RID: 2281 RVA: 0x00017968 File Offset: 0x00015B68
			public ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060008EA RID: 2282 RVA: 0x0001797C File Offset: 0x00015B7C
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x060008EB RID: 2283 RVA: 0x00017986 File Offset: 0x00015B86
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060008EC RID: 2284 RVA: 0x00017994 File Offset: 0x00015B94
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060008ED RID: 2285 RVA: 0x000179A1 File Offset: 0x00015BA1
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x040002BD RID: 701
			private T1 t1;

			// Token: 0x040002BE RID: 702
			private T2 t2;

			// Token: 0x040002BF RID: 703
			private T3 t3;

			// Token: 0x040002C0 RID: 704
			private T4 t4;

			// Token: 0x040002C1 RID: 705
			private T5 t5;

			// Token: 0x040002C2 RID: 706
			private T6 t6;

			// Token: 0x040002C3 RID: 707
			private T7 t7;

			// Token: 0x040002C4 RID: 708
			private T8 t8;

			// Token: 0x040002C5 RID: 709
			private T9 t9;

			// Token: 0x040002C6 RID: 710
			private T10 t10;

			// Token: 0x040002C7 RID: 711
			private T11 t11;

			// Token: 0x040002C8 RID: 712
			private int completedCount;

			// Token: 0x040002C9 RID: 713
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11>>> core;
		}

		// Token: 0x0200018E RID: 398
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>>
		{
			// Token: 0x060008EE RID: 2286 RVA: 0x000179B4 File Offset: 0x00015BB4
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT11(this, awaiter11);
				}
				else
				{
					awaiter11.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T11>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T11>.Awaiter>(this, awaiter11));
				}
				UniTask<T12>.Awaiter awaiter12 = task12.GetAwaiter();
				if (awaiter12.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT12(this, awaiter12);
					return;
				}
				awaiter12.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T12>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T12>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT12(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T12>.Awaiter>(this, awaiter12));
			}

			// Token: 0x060008EF RID: 2287 RVA: 0x00017D40 File Offset: 0x00015F40
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F0 RID: 2288 RVA: 0x00017DE8 File Offset: 0x00015FE8
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F1 RID: 2289 RVA: 0x00017E90 File Offset: 0x00016090
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F2 RID: 2290 RVA: 0x00017F38 File Offset: 0x00016138
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F3 RID: 2291 RVA: 0x00017FE0 File Offset: 0x000161E0
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F4 RID: 2292 RVA: 0x00018088 File Offset: 0x00016288
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F5 RID: 2293 RVA: 0x00018130 File Offset: 0x00016330
			private static void TryInvokeContinuationT7(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T7>.Awaiter awaiter)
			{
				try
				{
					self.t7 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F6 RID: 2294 RVA: 0x000181D8 File Offset: 0x000163D8
			private static void TryInvokeContinuationT8(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T8>.Awaiter awaiter)
			{
				try
				{
					self.t8 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F7 RID: 2295 RVA: 0x00018280 File Offset: 0x00016480
			private static void TryInvokeContinuationT9(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T9>.Awaiter awaiter)
			{
				try
				{
					self.t9 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F8 RID: 2296 RVA: 0x00018328 File Offset: 0x00016528
			private static void TryInvokeContinuationT10(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T10>.Awaiter awaiter)
			{
				try
				{
					self.t10 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008F9 RID: 2297 RVA: 0x000183D0 File Offset: 0x000165D0
			private static void TryInvokeContinuationT11(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T11>.Awaiter awaiter)
			{
				try
				{
					self.t11 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008FA RID: 2298 RVA: 0x00018478 File Offset: 0x00016678
			private static void TryInvokeContinuationT12(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T12>.Awaiter awaiter)
			{
				try
				{
					self.t12 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 12)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12>(self.t8, self.t9, self.t10, self.t11, self.t12)));
				}
			}

			// Token: 0x060008FB RID: 2299 RVA: 0x00018520 File Offset: 0x00016720
			public ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060008FC RID: 2300 RVA: 0x00018534 File Offset: 0x00016734
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x060008FD RID: 2301 RVA: 0x0001853E File Offset: 0x0001673E
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060008FE RID: 2302 RVA: 0x0001854C File Offset: 0x0001674C
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060008FF RID: 2303 RVA: 0x00018559 File Offset: 0x00016759
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x040002CA RID: 714
			private T1 t1;

			// Token: 0x040002CB RID: 715
			private T2 t2;

			// Token: 0x040002CC RID: 716
			private T3 t3;

			// Token: 0x040002CD RID: 717
			private T4 t4;

			// Token: 0x040002CE RID: 718
			private T5 t5;

			// Token: 0x040002CF RID: 719
			private T6 t6;

			// Token: 0x040002D0 RID: 720
			private T7 t7;

			// Token: 0x040002D1 RID: 721
			private T8 t8;

			// Token: 0x040002D2 RID: 722
			private T9 t9;

			// Token: 0x040002D3 RID: 723
			private T10 t10;

			// Token: 0x040002D4 RID: 724
			private T11 t11;

			// Token: 0x040002D5 RID: 725
			private T12 t12;

			// Token: 0x040002D6 RID: 726
			private int completedCount;

			// Token: 0x040002D7 RID: 727
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12>>> core;
		}

		// Token: 0x0200018F RID: 399
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>>
		{
			// Token: 0x06000900 RID: 2304 RVA: 0x0001856C File Offset: 0x0001676C
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT11(this, awaiter11);
				}
				else
				{
					awaiter11.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T11>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T11>.Awaiter>(this, awaiter11));
				}
				UniTask<T12>.Awaiter awaiter12 = task12.GetAwaiter();
				if (awaiter12.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT12(this, awaiter12);
				}
				else
				{
					awaiter12.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T12>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T12>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT12(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T12>.Awaiter>(this, awaiter12));
				}
				UniTask<T13>.Awaiter awaiter13 = task13.GetAwaiter();
				if (awaiter13.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT13(this, awaiter13);
					return;
				}
				awaiter13.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T13>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T13>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT13(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T13>.Awaiter>(this, awaiter13));
			}

			// Token: 0x06000901 RID: 2305 RVA: 0x00018940 File Offset: 0x00016B40
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x06000902 RID: 2306 RVA: 0x000189F0 File Offset: 0x00016BF0
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x06000903 RID: 2307 RVA: 0x00018AA0 File Offset: 0x00016CA0
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x06000904 RID: 2308 RVA: 0x00018B50 File Offset: 0x00016D50
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x06000905 RID: 2309 RVA: 0x00018C00 File Offset: 0x00016E00
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x06000906 RID: 2310 RVA: 0x00018CB0 File Offset: 0x00016EB0
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x06000907 RID: 2311 RVA: 0x00018D60 File Offset: 0x00016F60
			private static void TryInvokeContinuationT7(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T7>.Awaiter awaiter)
			{
				try
				{
					self.t7 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x06000908 RID: 2312 RVA: 0x00018E10 File Offset: 0x00017010
			private static void TryInvokeContinuationT8(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T8>.Awaiter awaiter)
			{
				try
				{
					self.t8 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x06000909 RID: 2313 RVA: 0x00018EC0 File Offset: 0x000170C0
			private static void TryInvokeContinuationT9(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T9>.Awaiter awaiter)
			{
				try
				{
					self.t9 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x0600090A RID: 2314 RVA: 0x00018F70 File Offset: 0x00017170
			private static void TryInvokeContinuationT10(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T10>.Awaiter awaiter)
			{
				try
				{
					self.t10 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x0600090B RID: 2315 RVA: 0x00019020 File Offset: 0x00017220
			private static void TryInvokeContinuationT11(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T11>.Awaiter awaiter)
			{
				try
				{
					self.t11 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x0600090C RID: 2316 RVA: 0x000190D0 File Offset: 0x000172D0
			private static void TryInvokeContinuationT12(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T12>.Awaiter awaiter)
			{
				try
				{
					self.t12 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x0600090D RID: 2317 RVA: 0x00019180 File Offset: 0x00017380
			private static void TryInvokeContinuationT13(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T13>.Awaiter awaiter)
			{
				try
				{
					self.t13 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 13)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13)));
				}
			}

			// Token: 0x0600090E RID: 2318 RVA: 0x00019230 File Offset: 0x00017430
			public ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x0600090F RID: 2319 RVA: 0x00019244 File Offset: 0x00017444
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000910 RID: 2320 RVA: 0x0001924E File Offset: 0x0001744E
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000911 RID: 2321 RVA: 0x0001925C File Offset: 0x0001745C
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000912 RID: 2322 RVA: 0x00019269 File Offset: 0x00017469
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x040002D8 RID: 728
			private T1 t1;

			// Token: 0x040002D9 RID: 729
			private T2 t2;

			// Token: 0x040002DA RID: 730
			private T3 t3;

			// Token: 0x040002DB RID: 731
			private T4 t4;

			// Token: 0x040002DC RID: 732
			private T5 t5;

			// Token: 0x040002DD RID: 733
			private T6 t6;

			// Token: 0x040002DE RID: 734
			private T7 t7;

			// Token: 0x040002DF RID: 735
			private T8 t8;

			// Token: 0x040002E0 RID: 736
			private T9 t9;

			// Token: 0x040002E1 RID: 737
			private T10 t10;

			// Token: 0x040002E2 RID: 738
			private T11 t11;

			// Token: 0x040002E3 RID: 739
			private T12 t12;

			// Token: 0x040002E4 RID: 740
			private T13 t13;

			// Token: 0x040002E5 RID: 741
			private int completedCount;

			// Token: 0x040002E6 RID: 742
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13>>> core;
		}

		// Token: 0x02000190 RID: 400
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>>
		{
			// Token: 0x06000913 RID: 2323 RVA: 0x0001927C File Offset: 0x0001747C
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13, UniTask<T14> task14)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT11(this, awaiter11);
				}
				else
				{
					awaiter11.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T11>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T11>.Awaiter>(this, awaiter11));
				}
				UniTask<T12>.Awaiter awaiter12 = task12.GetAwaiter();
				if (awaiter12.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT12(this, awaiter12);
				}
				else
				{
					awaiter12.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T12>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T12>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT12(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T12>.Awaiter>(this, awaiter12));
				}
				UniTask<T13>.Awaiter awaiter13 = task13.GetAwaiter();
				if (awaiter13.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT13(this, awaiter13);
				}
				else
				{
					awaiter13.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T13>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T13>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT13(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T13>.Awaiter>(this, awaiter13));
				}
				UniTask<T14>.Awaiter awaiter14 = task14.GetAwaiter();
				if (awaiter14.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT14(this, awaiter14);
					return;
				}
				awaiter14.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T14>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T14>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT14(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T14>.Awaiter>(this, awaiter14));
			}

			// Token: 0x06000914 RID: 2324 RVA: 0x0001969C File Offset: 0x0001789C
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x06000915 RID: 2325 RVA: 0x00019750 File Offset: 0x00017950
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x06000916 RID: 2326 RVA: 0x00019804 File Offset: 0x00017A04
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x06000917 RID: 2327 RVA: 0x000198B8 File Offset: 0x00017AB8
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x06000918 RID: 2328 RVA: 0x0001996C File Offset: 0x00017B6C
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x06000919 RID: 2329 RVA: 0x00019A20 File Offset: 0x00017C20
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x0600091A RID: 2330 RVA: 0x00019AD4 File Offset: 0x00017CD4
			private static void TryInvokeContinuationT7(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T7>.Awaiter awaiter)
			{
				try
				{
					self.t7 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x0600091B RID: 2331 RVA: 0x00019B88 File Offset: 0x00017D88
			private static void TryInvokeContinuationT8(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T8>.Awaiter awaiter)
			{
				try
				{
					self.t8 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x0600091C RID: 2332 RVA: 0x00019C3C File Offset: 0x00017E3C
			private static void TryInvokeContinuationT9(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T9>.Awaiter awaiter)
			{
				try
				{
					self.t9 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x0600091D RID: 2333 RVA: 0x00019CF0 File Offset: 0x00017EF0
			private static void TryInvokeContinuationT10(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T10>.Awaiter awaiter)
			{
				try
				{
					self.t10 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x0600091E RID: 2334 RVA: 0x00019DA4 File Offset: 0x00017FA4
			private static void TryInvokeContinuationT11(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T11>.Awaiter awaiter)
			{
				try
				{
					self.t11 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x0600091F RID: 2335 RVA: 0x00019E58 File Offset: 0x00018058
			private static void TryInvokeContinuationT12(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T12>.Awaiter awaiter)
			{
				try
				{
					self.t12 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x06000920 RID: 2336 RVA: 0x00019F0C File Offset: 0x0001810C
			private static void TryInvokeContinuationT13(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T13>.Awaiter awaiter)
			{
				try
				{
					self.t13 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x06000921 RID: 2337 RVA: 0x00019FC0 File Offset: 0x000181C0
			private static void TryInvokeContinuationT14(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T14>.Awaiter awaiter)
			{
				try
				{
					self.t14 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 14)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14)));
				}
			}

			// Token: 0x06000922 RID: 2338 RVA: 0x0001A074 File Offset: 0x00018274
			public ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000923 RID: 2339 RVA: 0x0001A088 File Offset: 0x00018288
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000924 RID: 2340 RVA: 0x0001A092 File Offset: 0x00018292
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000925 RID: 2341 RVA: 0x0001A0A0 File Offset: 0x000182A0
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000926 RID: 2342 RVA: 0x0001A0AD File Offset: 0x000182AD
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x040002E7 RID: 743
			private T1 t1;

			// Token: 0x040002E8 RID: 744
			private T2 t2;

			// Token: 0x040002E9 RID: 745
			private T3 t3;

			// Token: 0x040002EA RID: 746
			private T4 t4;

			// Token: 0x040002EB RID: 747
			private T5 t5;

			// Token: 0x040002EC RID: 748
			private T6 t6;

			// Token: 0x040002ED RID: 749
			private T7 t7;

			// Token: 0x040002EE RID: 750
			private T8 t8;

			// Token: 0x040002EF RID: 751
			private T9 t9;

			// Token: 0x040002F0 RID: 752
			private T10 t10;

			// Token: 0x040002F1 RID: 753
			private T11 t11;

			// Token: 0x040002F2 RID: 754
			private T12 t12;

			// Token: 0x040002F3 RID: 755
			private T13 t13;

			// Token: 0x040002F4 RID: 756
			private T14 t14;

			// Token: 0x040002F5 RID: 757
			private int completedCount;

			// Token: 0x040002F6 RID: 758
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14>>> core;
		}

		// Token: 0x02000191 RID: 401
		private sealed class WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IUniTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>>
		{
			// Token: 0x06000927 RID: 2343 RVA: 0x0001A0C0 File Offset: 0x000182C0
			public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13, UniTask<T14> task14, UniTask<T15> task15)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT11(this, awaiter11);
				}
				else
				{
					awaiter11.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T11>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T11>.Awaiter>(this, awaiter11));
				}
				UniTask<T12>.Awaiter awaiter12 = task12.GetAwaiter();
				if (awaiter12.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT12(this, awaiter12);
				}
				else
				{
					awaiter12.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T12>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T12>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT12(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T12>.Awaiter>(this, awaiter12));
				}
				UniTask<T13>.Awaiter awaiter13 = task13.GetAwaiter();
				if (awaiter13.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT13(this, awaiter13);
				}
				else
				{
					awaiter13.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T13>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T13>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT13(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T13>.Awaiter>(this, awaiter13));
				}
				UniTask<T14>.Awaiter awaiter14 = task14.GetAwaiter();
				if (awaiter14.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT14(this, awaiter14);
				}
				else
				{
					awaiter14.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T14>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T14>.Awaiter>)state)
						{
							UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT14(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T14>.Awaiter>(this, awaiter14));
				}
				UniTask<T15>.Awaiter awaiter15 = task15.GetAwaiter();
				if (awaiter15.IsCompleted)
				{
					UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT15(this, awaiter15);
					return;
				}
				awaiter15.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T15>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T15>.Awaiter>)state)
					{
						UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT15(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T15>.Awaiter>(this, awaiter15));
			}

			// Token: 0x06000928 RID: 2344 RVA: 0x0001A528 File Offset: 0x00018728
			private static void TryInvokeContinuationT1(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T1>.Awaiter awaiter)
			{
				try
				{
					self.t1 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x06000929 RID: 2345 RVA: 0x0001A5EC File Offset: 0x000187EC
			private static void TryInvokeContinuationT2(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T2>.Awaiter awaiter)
			{
				try
				{
					self.t2 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x0600092A RID: 2346 RVA: 0x0001A6B0 File Offset: 0x000188B0
			private static void TryInvokeContinuationT3(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T3>.Awaiter awaiter)
			{
				try
				{
					self.t3 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x0600092B RID: 2347 RVA: 0x0001A774 File Offset: 0x00018974
			private static void TryInvokeContinuationT4(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T4>.Awaiter awaiter)
			{
				try
				{
					self.t4 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x0600092C RID: 2348 RVA: 0x0001A838 File Offset: 0x00018A38
			private static void TryInvokeContinuationT5(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T5>.Awaiter awaiter)
			{
				try
				{
					self.t5 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x0600092D RID: 2349 RVA: 0x0001A8FC File Offset: 0x00018AFC
			private static void TryInvokeContinuationT6(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T6>.Awaiter awaiter)
			{
				try
				{
					self.t6 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x0600092E RID: 2350 RVA: 0x0001A9C0 File Offset: 0x00018BC0
			private static void TryInvokeContinuationT7(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T7>.Awaiter awaiter)
			{
				try
				{
					self.t7 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x0600092F RID: 2351 RVA: 0x0001AA84 File Offset: 0x00018C84
			private static void TryInvokeContinuationT8(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T8>.Awaiter awaiter)
			{
				try
				{
					self.t8 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x06000930 RID: 2352 RVA: 0x0001AB48 File Offset: 0x00018D48
			private static void TryInvokeContinuationT9(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T9>.Awaiter awaiter)
			{
				try
				{
					self.t9 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x06000931 RID: 2353 RVA: 0x0001AC0C File Offset: 0x00018E0C
			private static void TryInvokeContinuationT10(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T10>.Awaiter awaiter)
			{
				try
				{
					self.t10 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x06000932 RID: 2354 RVA: 0x0001ACD0 File Offset: 0x00018ED0
			private static void TryInvokeContinuationT11(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T11>.Awaiter awaiter)
			{
				try
				{
					self.t11 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x06000933 RID: 2355 RVA: 0x0001AD94 File Offset: 0x00018F94
			private static void TryInvokeContinuationT12(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T12>.Awaiter awaiter)
			{
				try
				{
					self.t12 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x06000934 RID: 2356 RVA: 0x0001AE58 File Offset: 0x00019058
			private static void TryInvokeContinuationT13(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T13>.Awaiter awaiter)
			{
				try
				{
					self.t13 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x06000935 RID: 2357 RVA: 0x0001AF1C File Offset: 0x0001911C
			private static void TryInvokeContinuationT14(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T14>.Awaiter awaiter)
			{
				try
				{
					self.t14 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x06000936 RID: 2358 RVA: 0x0001AFE0 File Offset: 0x000191E0
			private static void TryInvokeContinuationT15(UniTask.WhenAllPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T15>.Awaiter awaiter)
			{
				try
				{
					self.t15 = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 15)
				{
					self.core.TrySetResult(new ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>(self.t1, self.t2, self.t3, self.t4, self.t5, self.t6, self.t7, new ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>(self.t8, self.t9, self.t10, self.t11, self.t12, self.t13, self.t14, new ValueTuple<T15>(self.t15))));
				}
			}

			// Token: 0x06000937 RID: 2359 RVA: 0x0001B0A4 File Offset: 0x000192A4
			public ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000938 RID: 2360 RVA: 0x0001B0B8 File Offset: 0x000192B8
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000939 RID: 2361 RVA: 0x0001B0C2 File Offset: 0x000192C2
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600093A RID: 2362 RVA: 0x0001B0D0 File Offset: 0x000192D0
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600093B RID: 2363 RVA: 0x0001B0DD File Offset: 0x000192DD
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x040002F7 RID: 759
			private T1 t1;

			// Token: 0x040002F8 RID: 760
			private T2 t2;

			// Token: 0x040002F9 RID: 761
			private T3 t3;

			// Token: 0x040002FA RID: 762
			private T4 t4;

			// Token: 0x040002FB RID: 763
			private T5 t5;

			// Token: 0x040002FC RID: 764
			private T6 t6;

			// Token: 0x040002FD RID: 765
			private T7 t7;

			// Token: 0x040002FE RID: 766
			private T8 t8;

			// Token: 0x040002FF RID: 767
			private T9 t9;

			// Token: 0x04000300 RID: 768
			private T10 t10;

			// Token: 0x04000301 RID: 769
			private T11 t11;

			// Token: 0x04000302 RID: 770
			private T12 t12;

			// Token: 0x04000303 RID: 771
			private T13 t13;

			// Token: 0x04000304 RID: 772
			private T14 t14;

			// Token: 0x04000305 RID: 773
			private T15 t15;

			// Token: 0x04000306 RID: 774
			private int completedCount;

			// Token: 0x04000307 RID: 775
			private UniTaskCompletionSourceCore<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<T8, T9, T10, T11, T12, T13, T14, ValueTuple<T15>>>> core;
		}

		// Token: 0x02000192 RID: 402
		private sealed class WhenAnyLRPromise<T> : IUniTaskSource<ValueTuple<bool, T>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<bool, T>>
		{
			// Token: 0x0600093C RID: 2364 RVA: 0x0001B0F0 File Offset: 0x000192F0
			public WhenAnyLRPromise(UniTask<T> leftTask, UniTask rightTask)
			{
				UniTask<T>.Awaiter awaiter;
				try
				{
					awaiter = leftTask.GetAwaiter();
				}
				catch (Exception error)
				{
					this.core.TrySetException(error);
					goto IL_60;
				}
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyLRPromise<T>.TryLeftInvokeContinuation(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyLRPromise<T>, UniTask<T>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyLRPromise<T>, UniTask<T>.Awaiter>)state)
						{
							UniTask.WhenAnyLRPromise<T>.TryLeftInvokeContinuation(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyLRPromise<T>, UniTask<T>.Awaiter>(this, awaiter));
				}
				IL_60:
				UniTask.Awaiter awaiter2;
				try
				{
					awaiter2 = rightTask.GetAwaiter();
				}
				catch (Exception error2)
				{
					this.core.TrySetException(error2);
					return;
				}
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyLRPromise<T>.TryRightInvokeContinuation(this, awaiter2);
					return;
				}
				awaiter2.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyLRPromise<T>, UniTask.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyLRPromise<T>, UniTask.Awaiter>)state)
					{
						UniTask.WhenAnyLRPromise<T>.TryRightInvokeContinuation(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyLRPromise<T>, UniTask.Awaiter>(this, awaiter2));
			}

			// Token: 0x0600093D RID: 2365 RVA: 0x0001B1D4 File Offset: 0x000193D4
			private static void TryLeftInvokeContinuation(UniTask.WhenAnyLRPromise<T> self, in UniTask<T>.Awaiter awaiter)
			{
				T result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<bool, T>(true, result));
				}
			}

			// Token: 0x0600093E RID: 2366 RVA: 0x0001B22C File Offset: 0x0001942C
			private static void TryRightInvokeContinuation(UniTask.WhenAnyLRPromise<T> self, in UniTask.Awaiter awaiter)
			{
				try
				{
					awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<bool, T>(false, default(T)));
				}
			}

			// Token: 0x0600093F RID: 2367 RVA: 0x0001B28C File Offset: 0x0001948C
			public ValueTuple<bool, T> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000940 RID: 2368 RVA: 0x0001B2A0 File Offset: 0x000194A0
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000941 RID: 2369 RVA: 0x0001B2AE File Offset: 0x000194AE
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000942 RID: 2370 RVA: 0x0001B2BE File Offset: 0x000194BE
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000943 RID: 2371 RVA: 0x0001B2CB File Offset: 0x000194CB
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000308 RID: 776
			private int completedCount;

			// Token: 0x04000309 RID: 777
			private UniTaskCompletionSourceCore<ValueTuple<bool, T>> core;
		}

		// Token: 0x02000193 RID: 403
		private sealed class WhenAnyPromise<T> : IUniTaskSource<ValueTuple<int, T>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T>>
		{
			// Token: 0x06000944 RID: 2372 RVA: 0x0001B2D8 File Offset: 0x000194D8
			public WhenAnyPromise(UniTask<T>[] tasks, int tasksLength)
			{
				if (tasksLength == 0)
				{
					throw new ArgumentException("The tasks argument contains no tasks.");
				}
				int i = 0;
				while (i < tasksLength)
				{
					UniTask<T>.Awaiter awaiter;
					try
					{
						awaiter = tasks[i].GetAwaiter();
					}
					catch (Exception error)
					{
						this.core.TrySetException(error);
						goto IL_7A;
					}
					goto IL_38;
					IL_7A:
					i++;
					continue;
					IL_38:
					if (awaiter.IsCompleted)
					{
						UniTask.WhenAnyPromise<T>.TryInvokeContinuation(this, awaiter, i);
						goto IL_7A;
					}
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T>, UniTask<T>.Awaiter, int> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T>, UniTask<T>.Awaiter, int>)state)
						{
							UniTask.WhenAnyPromise<T>.TryInvokeContinuation(stateTuple.Item1, stateTuple.Item2, stateTuple.Item3);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T>, UniTask<T>.Awaiter, int>(this, awaiter, i));
					goto IL_7A;
				}
			}

			// Token: 0x06000945 RID: 2373 RVA: 0x0001B378 File Offset: 0x00019578
			private static void TryInvokeContinuation(UniTask.WhenAnyPromise<T> self, in UniTask<T>.Awaiter awaiter, int i)
			{
				T result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T>(i, result));
				}
			}

			// Token: 0x06000946 RID: 2374 RVA: 0x0001B3D0 File Offset: 0x000195D0
			public ValueTuple<int, T> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000947 RID: 2375 RVA: 0x0001B3E4 File Offset: 0x000195E4
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000948 RID: 2376 RVA: 0x0001B3F2 File Offset: 0x000195F2
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000949 RID: 2377 RVA: 0x0001B402 File Offset: 0x00019602
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600094A RID: 2378 RVA: 0x0001B40F File Offset: 0x0001960F
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0400030A RID: 778
			private int completedCount;

			// Token: 0x0400030B RID: 779
			private UniTaskCompletionSourceCore<ValueTuple<int, T>> core;
		}

		// Token: 0x02000194 RID: 404
		private sealed class WhenAnyPromise : IUniTaskSource<int>, IUniTaskSource, IValueTaskSource, IValueTaskSource<int>
		{
			// Token: 0x0600094B RID: 2379 RVA: 0x0001B41C File Offset: 0x0001961C
			public WhenAnyPromise(UniTask[] tasks, int tasksLength)
			{
				if (tasksLength == 0)
				{
					throw new ArgumentException("The tasks argument contains no tasks.");
				}
				int i = 0;
				while (i < tasksLength)
				{
					UniTask.Awaiter awaiter;
					try
					{
						awaiter = tasks[i].GetAwaiter();
					}
					catch (Exception error)
					{
						this.core.TrySetException(error);
						goto IL_7A;
					}
					goto IL_38;
					IL_7A:
					i++;
					continue;
					IL_38:
					if (awaiter.IsCompleted)
					{
						UniTask.WhenAnyPromise.TryInvokeContinuation(this, awaiter, i);
						goto IL_7A;
					}
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise, UniTask.Awaiter, int> stateTuple = (StateTuple<UniTask.WhenAnyPromise, UniTask.Awaiter, int>)state)
						{
							UniTask.WhenAnyPromise.TryInvokeContinuation(stateTuple.Item1, stateTuple.Item2, stateTuple.Item3);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise, UniTask.Awaiter, int>(this, awaiter, i));
					goto IL_7A;
				}
			}

			// Token: 0x0600094C RID: 2380 RVA: 0x0001B4BC File Offset: 0x000196BC
			private static void TryInvokeContinuation(UniTask.WhenAnyPromise self, in UniTask.Awaiter awaiter, int i)
			{
				try
				{
					awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(i);
				}
			}

			// Token: 0x0600094D RID: 2381 RVA: 0x0001B50C File Offset: 0x0001970C
			public int GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x0600094E RID: 2382 RVA: 0x0001B520 File Offset: 0x00019720
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600094F RID: 2383 RVA: 0x0001B52E File Offset: 0x0001972E
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000950 RID: 2384 RVA: 0x0001B53E File Offset: 0x0001973E
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000951 RID: 2385 RVA: 0x0001B54B File Offset: 0x0001974B
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0400030C RID: 780
			private int completedCount;

			// Token: 0x0400030D RID: 781
			private UniTaskCompletionSourceCore<int> core;
		}

		// Token: 0x02000195 RID: 405
		private sealed class WhenAnyPromise<T1, T2> : IUniTaskSource<ValueTuple<int, T1, T2>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2>>
		{
			// Token: 0x06000952 RID: 2386 RVA: 0x0001B558 File Offset: 0x00019758
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2>.TryInvokeContinuationT2(this, awaiter2);
					return;
				}
				awaiter2.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2>, UniTask<T2>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2>, UniTask<T2>.Awaiter>(this, awaiter2));
			}

			// Token: 0x06000953 RID: 2387 RVA: 0x0001B604 File Offset: 0x00019804
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2>(0, result, default(T2)));
				}
			}

			// Token: 0x06000954 RID: 2388 RVA: 0x0001B664 File Offset: 0x00019864
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2>(1, default(T1), result));
				}
			}

			// Token: 0x06000955 RID: 2389 RVA: 0x0001B6C4 File Offset: 0x000198C4
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2"
			})]
			public ValueTuple<int, T1, T2> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000956 RID: 2390 RVA: 0x0001B6D8 File Offset: 0x000198D8
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000957 RID: 2391 RVA: 0x0001B6E6 File Offset: 0x000198E6
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000958 RID: 2392 RVA: 0x0001B6F6 File Offset: 0x000198F6
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000959 RID: 2393 RVA: 0x0001B703 File Offset: 0x00019903
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0400030E RID: 782
			private int completedCount;

			// Token: 0x0400030F RID: 783
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2"
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2>> core;
		}

		// Token: 0x02000196 RID: 406
		private sealed class WhenAnyPromise<T1, T2, T3> : IUniTaskSource<ValueTuple<int, T1, T2, T3>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3>>
		{
			// Token: 0x0600095A RID: 2394 RVA: 0x0001B710 File Offset: 0x00019910
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3>.TryInvokeContinuationT3(this, awaiter3);
					return;
				}
				awaiter3.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3>, UniTask<T3>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3>, UniTask<T3>.Awaiter>(this, awaiter3));
			}

			// Token: 0x0600095B RID: 2395 RVA: 0x0001B804 File Offset: 0x00019A04
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3>(0, result, default(T2), default(T3)));
				}
			}

			// Token: 0x0600095C RID: 2396 RVA: 0x0001B870 File Offset: 0x00019A70
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3>(1, default(T1), result, default(T3)));
				}
			}

			// Token: 0x0600095D RID: 2397 RVA: 0x0001B8DC File Offset: 0x00019ADC
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3>(2, default(T1), default(T2), result));
				}
			}

			// Token: 0x0600095E RID: 2398 RVA: 0x0001B948 File Offset: 0x00019B48
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3"
			})]
			public ValueTuple<int, T1, T2, T3> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x0600095F RID: 2399 RVA: 0x0001B95C File Offset: 0x00019B5C
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000960 RID: 2400 RVA: 0x0001B96A File Offset: 0x00019B6A
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000961 RID: 2401 RVA: 0x0001B97A File Offset: 0x00019B7A
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000962 RID: 2402 RVA: 0x0001B987 File Offset: 0x00019B87
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000310 RID: 784
			private int completedCount;

			// Token: 0x04000311 RID: 785
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3"
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3>> core;
		}

		// Token: 0x02000197 RID: 407
		private sealed class WhenAnyPromise<T1, T2, T3, T4> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4>>
		{
			// Token: 0x06000963 RID: 2403 RVA: 0x0001B994 File Offset: 0x00019B94
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4>.TryInvokeContinuationT4(this, awaiter4);
					return;
				}
				awaiter4.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T4>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4>, UniTask<T4>.Awaiter>(this, awaiter4));
			}

			// Token: 0x06000964 RID: 2404 RVA: 0x0001BAD0 File Offset: 0x00019CD0
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4>(0, result, default(T2), default(T3), default(T4)));
				}
			}

			// Token: 0x06000965 RID: 2405 RVA: 0x0001BB44 File Offset: 0x00019D44
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4>(1, default(T1), result, default(T3), default(T4)));
				}
			}

			// Token: 0x06000966 RID: 2406 RVA: 0x0001BBB8 File Offset: 0x00019DB8
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4>(2, default(T1), default(T2), result, default(T4)));
				}
			}

			// Token: 0x06000967 RID: 2407 RVA: 0x0001BC2C File Offset: 0x00019E2C
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4>(3, default(T1), default(T2), default(T3), result));
				}
			}

			// Token: 0x06000968 RID: 2408 RVA: 0x0001BCA0 File Offset: 0x00019EA0
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4"
			})]
			public ValueTuple<int, T1, T2, T3, T4> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000969 RID: 2409 RVA: 0x0001BCB4 File Offset: 0x00019EB4
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600096A RID: 2410 RVA: 0x0001BCC2 File Offset: 0x00019EC2
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600096B RID: 2411 RVA: 0x0001BCD2 File Offset: 0x00019ED2
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600096C RID: 2412 RVA: 0x0001BCDF File Offset: 0x00019EDF
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000312 RID: 786
			private int completedCount;

			// Token: 0x04000313 RID: 787
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4"
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4>> core;
		}

		// Token: 0x02000198 RID: 408
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5>>
		{
			// Token: 0x0600096D RID: 2413 RVA: 0x0001BCEC File Offset: 0x00019EEC
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT5(this, awaiter5);
					return;
				}
				awaiter5.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T5>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5>, UniTask<T5>.Awaiter>(this, awaiter5));
			}

			// Token: 0x0600096E RID: 2414 RVA: 0x0001BE70 File Offset: 0x0001A070
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5>(0, result, default(T2), default(T3), default(T4), default(T5)));
				}
			}

			// Token: 0x0600096F RID: 2415 RVA: 0x0001BEF0 File Offset: 0x0001A0F0
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5>(1, default(T1), result, default(T3), default(T4), default(T5)));
				}
			}

			// Token: 0x06000970 RID: 2416 RVA: 0x0001BF70 File Offset: 0x0001A170
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5>(2, default(T1), default(T2), result, default(T4), default(T5)));
				}
			}

			// Token: 0x06000971 RID: 2417 RVA: 0x0001BFF0 File Offset: 0x0001A1F0
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5>(3, default(T1), default(T2), default(T3), result, default(T5)));
				}
			}

			// Token: 0x06000972 RID: 2418 RVA: 0x0001C070 File Offset: 0x0001A270
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5>(4, default(T1), default(T2), default(T3), default(T4), result));
				}
			}

			// Token: 0x06000973 RID: 2419 RVA: 0x0001C0F0 File Offset: 0x0001A2F0
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5"
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000974 RID: 2420 RVA: 0x0001C104 File Offset: 0x0001A304
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000975 RID: 2421 RVA: 0x0001C112 File Offset: 0x0001A312
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000976 RID: 2422 RVA: 0x0001C122 File Offset: 0x0001A322
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000977 RID: 2423 RVA: 0x0001C12F File Offset: 0x0001A32F
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000314 RID: 788
			private int completedCount;

			// Token: 0x04000315 RID: 789
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5"
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5>> core;
		}

		// Token: 0x02000199 RID: 409
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6>>
		{
			// Token: 0x06000978 RID: 2424 RVA: 0x0001C13C File Offset: 0x0001A33C
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT6(this, awaiter6);
					return;
				}
				awaiter6.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T6>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6>, UniTask<T6>.Awaiter>(this, awaiter6));
			}

			// Token: 0x06000979 RID: 2425 RVA: 0x0001C30C File Offset: 0x0001A50C
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6)));
				}
			}

			// Token: 0x0600097A RID: 2426 RVA: 0x0001C394 File Offset: 0x0001A594
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6)));
				}
			}

			// Token: 0x0600097B RID: 2427 RVA: 0x0001C41C File Offset: 0x0001A61C
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6)));
				}
			}

			// Token: 0x0600097C RID: 2428 RVA: 0x0001C4A4 File Offset: 0x0001A6A4
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6)));
				}
			}

			// Token: 0x0600097D RID: 2429 RVA: 0x0001C52C File Offset: 0x0001A72C
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6)));
				}
			}

			// Token: 0x0600097E RID: 2430 RVA: 0x0001C5B4 File Offset: 0x0001A7B4
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result));
				}
			}

			// Token: 0x0600097F RID: 2431 RVA: 0x0001C63C File Offset: 0x0001A83C
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6"
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000980 RID: 2432 RVA: 0x0001C650 File Offset: 0x0001A850
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000981 RID: 2433 RVA: 0x0001C65E File Offset: 0x0001A85E
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000982 RID: 2434 RVA: 0x0001C66E File Offset: 0x0001A86E
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000983 RID: 2435 RVA: 0x0001C67B File Offset: 0x0001A87B
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000316 RID: 790
			private int completedCount;

			// Token: 0x04000317 RID: 791
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6"
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6>> core;
		}

		// Token: 0x0200019A RID: 410
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>>
		{
			// Token: 0x06000984 RID: 2436 RVA: 0x0001C688 File Offset: 0x0001A888
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT7(this, awaiter7);
					return;
				}
				awaiter7.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T7>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7>, UniTask<T7>.Awaiter>(this, awaiter7));
			}

			// Token: 0x06000985 RID: 2437 RVA: 0x0001C8A0 File Offset: 0x0001AAA0
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7>(default(T7))));
				}
			}

			// Token: 0x06000986 RID: 2438 RVA: 0x0001C938 File Offset: 0x0001AB38
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7>(default(T7))));
				}
			}

			// Token: 0x06000987 RID: 2439 RVA: 0x0001C9D0 File Offset: 0x0001ABD0
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6), new ValueTuple<T7>(default(T7))));
				}
			}

			// Token: 0x06000988 RID: 2440 RVA: 0x0001CA68 File Offset: 0x0001AC68
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6), new ValueTuple<T7>(default(T7))));
				}
			}

			// Token: 0x06000989 RID: 2441 RVA: 0x0001CB00 File Offset: 0x0001AD00
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6), new ValueTuple<T7>(default(T7))));
				}
			}

			// Token: 0x0600098A RID: 2442 RVA: 0x0001CB98 File Offset: 0x0001AD98
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result, new ValueTuple<T7>(default(T7))));
				}
			}

			// Token: 0x0600098B RID: 2443 RVA: 0x0001CC30 File Offset: 0x0001AE30
			private static void TryInvokeContinuationT7(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7> self, in UniTask<T7>.Awaiter awaiter)
			{
				T7 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>(6, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7>(result)));
				}
			}

			// Token: 0x0600098C RID: 2444 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				null
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x0600098D RID: 2445 RVA: 0x0001CCDC File Offset: 0x0001AEDC
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600098E RID: 2446 RVA: 0x0001CCEA File Offset: 0x0001AEEA
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600098F RID: 2447 RVA: 0x0001CCFA File Offset: 0x0001AEFA
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000990 RID: 2448 RVA: 0x0001CD07 File Offset: 0x0001AF07
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000318 RID: 792
			private int completedCount;

			// Token: 0x04000319 RID: 793
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				null
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7>>> core;
		}

		// Token: 0x0200019B RID: 411
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>>
		{
			// Token: 0x06000991 RID: 2449 RVA: 0x0001CD14 File Offset: 0x0001AF14
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT8(this, awaiter8);
					return;
				}
				awaiter8.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T8>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8>, UniTask<T8>.Awaiter>(this, awaiter8));
			}

			// Token: 0x06000992 RID: 2450 RVA: 0x0001CF78 File Offset: 0x0001B178
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8>(default(T7), default(T8))));
				}
			}

			// Token: 0x06000993 RID: 2451 RVA: 0x0001D018 File Offset: 0x0001B218
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8>(default(T7), default(T8))));
				}
			}

			// Token: 0x06000994 RID: 2452 RVA: 0x0001D0B8 File Offset: 0x0001B2B8
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6), new ValueTuple<T7, T8>(default(T7), default(T8))));
				}
			}

			// Token: 0x06000995 RID: 2453 RVA: 0x0001D158 File Offset: 0x0001B358
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6), new ValueTuple<T7, T8>(default(T7), default(T8))));
				}
			}

			// Token: 0x06000996 RID: 2454 RVA: 0x0001D1F8 File Offset: 0x0001B3F8
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6), new ValueTuple<T7, T8>(default(T7), default(T8))));
				}
			}

			// Token: 0x06000997 RID: 2455 RVA: 0x0001D298 File Offset: 0x0001B498
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result, new ValueTuple<T7, T8>(default(T7), default(T8))));
				}
			}

			// Token: 0x06000998 RID: 2456 RVA: 0x0001D338 File Offset: 0x0001B538
			private static void TryInvokeContinuationT7(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T7>.Awaiter awaiter)
			{
				T7 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>(6, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8>(result, default(T8))));
				}
			}

			// Token: 0x06000999 RID: 2457 RVA: 0x0001D3D8 File Offset: 0x0001B5D8
			private static void TryInvokeContinuationT8(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8> self, in UniTask<T8>.Awaiter awaiter)
			{
				T8 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>(7, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8>(default(T7), result)));
				}
			}

			// Token: 0x0600099A RID: 2458 RVA: 0x0001D478 File Offset: 0x0001B678
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				null,
				null
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x0600099B RID: 2459 RVA: 0x0001D48C File Offset: 0x0001B68C
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600099C RID: 2460 RVA: 0x0001D49A File Offset: 0x0001B69A
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600099D RID: 2461 RVA: 0x0001D4AA File Offset: 0x0001B6AA
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600099E RID: 2462 RVA: 0x0001D4B7 File Offset: 0x0001B6B7
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0400031A RID: 794
			private int completedCount;

			// Token: 0x0400031B RID: 795
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				null,
				null
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8>>> core;
		}

		// Token: 0x0200019C RID: 412
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>>
		{
			// Token: 0x0600099F RID: 2463 RVA: 0x0001D4C4 File Offset: 0x0001B6C4
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT9(this, awaiter9);
					return;
				}
				awaiter9.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T9>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9>, UniTask<T9>.Awaiter>(this, awaiter9));
			}

			// Token: 0x060009A0 RID: 2464 RVA: 0x0001D770 File Offset: 0x0001B970
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9>(default(T7), default(T8), default(T9))));
				}
			}

			// Token: 0x060009A1 RID: 2465 RVA: 0x0001D81C File Offset: 0x0001BA1C
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9>(default(T7), default(T8), default(T9))));
				}
			}

			// Token: 0x060009A2 RID: 2466 RVA: 0x0001D8C8 File Offset: 0x0001BAC8
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9>(default(T7), default(T8), default(T9))));
				}
			}

			// Token: 0x060009A3 RID: 2467 RVA: 0x0001D974 File Offset: 0x0001BB74
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6), new ValueTuple<T7, T8, T9>(default(T7), default(T8), default(T9))));
				}
			}

			// Token: 0x060009A4 RID: 2468 RVA: 0x0001DA20 File Offset: 0x0001BC20
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6), new ValueTuple<T7, T8, T9>(default(T7), default(T8), default(T9))));
				}
			}

			// Token: 0x060009A5 RID: 2469 RVA: 0x0001DACC File Offset: 0x0001BCCC
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result, new ValueTuple<T7, T8, T9>(default(T7), default(T8), default(T9))));
				}
			}

			// Token: 0x060009A6 RID: 2470 RVA: 0x0001DB78 File Offset: 0x0001BD78
			private static void TryInvokeContinuationT7(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T7>.Awaiter awaiter)
			{
				T7 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>(6, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9>(result, default(T8), default(T9))));
				}
			}

			// Token: 0x060009A7 RID: 2471 RVA: 0x0001DC24 File Offset: 0x0001BE24
			private static void TryInvokeContinuationT8(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T8>.Awaiter awaiter)
			{
				T8 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>(7, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9>(default(T7), result, default(T9))));
				}
			}

			// Token: 0x060009A8 RID: 2472 RVA: 0x0001DCD0 File Offset: 0x0001BED0
			private static void TryInvokeContinuationT9(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, in UniTask<T9>.Awaiter awaiter)
			{
				T9 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>(8, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9>(default(T7), default(T8), result)));
				}
			}

			// Token: 0x060009A9 RID: 2473 RVA: 0x0001DD7C File Offset: 0x0001BF7C
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				null,
				null,
				null
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060009AA RID: 2474 RVA: 0x0001DD90 File Offset: 0x0001BF90
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060009AB RID: 2475 RVA: 0x0001DD9E File Offset: 0x0001BF9E
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060009AC RID: 2476 RVA: 0x0001DDAE File Offset: 0x0001BFAE
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060009AD RID: 2477 RVA: 0x0001DDBB File Offset: 0x0001BFBB
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0400031C RID: 796
			private int completedCount;

			// Token: 0x0400031D RID: 797
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				null,
				null,
				null
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9>>> core;
		}

		// Token: 0x0200019D RID: 413
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>>
		{
			// Token: 0x060009AE RID: 2478 RVA: 0x0001DDC8 File Offset: 0x0001BFC8
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT10(this, awaiter10);
					return;
				}
				awaiter10.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T10>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, UniTask<T10>.Awaiter>(this, awaiter10));
			}

			// Token: 0x060009AF RID: 2479 RVA: 0x0001E0C0 File Offset: 0x0001C2C0
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10>(default(T7), default(T8), default(T9), default(T10))));
				}
			}

			// Token: 0x060009B0 RID: 2480 RVA: 0x0001E174 File Offset: 0x0001C374
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10>(default(T7), default(T8), default(T9), default(T10))));
				}
			}

			// Token: 0x060009B1 RID: 2481 RVA: 0x0001E228 File Offset: 0x0001C428
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10>(default(T7), default(T8), default(T9), default(T10))));
				}
			}

			// Token: 0x060009B2 RID: 2482 RVA: 0x0001E2DC File Offset: 0x0001C4DC
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6), new ValueTuple<T7, T8, T9, T10>(default(T7), default(T8), default(T9), default(T10))));
				}
			}

			// Token: 0x060009B3 RID: 2483 RVA: 0x0001E390 File Offset: 0x0001C590
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6), new ValueTuple<T7, T8, T9, T10>(default(T7), default(T8), default(T9), default(T10))));
				}
			}

			// Token: 0x060009B4 RID: 2484 RVA: 0x0001E444 File Offset: 0x0001C644
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result, new ValueTuple<T7, T8, T9, T10>(default(T7), default(T8), default(T9), default(T10))));
				}
			}

			// Token: 0x060009B5 RID: 2485 RVA: 0x0001E4F8 File Offset: 0x0001C6F8
			private static void TryInvokeContinuationT7(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T7>.Awaiter awaiter)
			{
				T7 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(6, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10>(result, default(T8), default(T9), default(T10))));
				}
			}

			// Token: 0x060009B6 RID: 2486 RVA: 0x0001E5AC File Offset: 0x0001C7AC
			private static void TryInvokeContinuationT8(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T8>.Awaiter awaiter)
			{
				T8 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(7, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10>(default(T7), result, default(T9), default(T10))));
				}
			}

			// Token: 0x060009B7 RID: 2487 RVA: 0x0001E660 File Offset: 0x0001C860
			private static void TryInvokeContinuationT9(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T9>.Awaiter awaiter)
			{
				T9 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(8, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10>(default(T7), default(T8), result, default(T10))));
				}
			}

			// Token: 0x060009B8 RID: 2488 RVA: 0x0001E714 File Offset: 0x0001C914
			private static void TryInvokeContinuationT10(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, in UniTask<T10>.Awaiter awaiter)
			{
				T10 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>(9, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10>(default(T7), default(T8), default(T9), result)));
				}
			}

			// Token: 0x060009B9 RID: 2489 RVA: 0x0001E7CC File Offset: 0x0001C9CC
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				null,
				null,
				null,
				null
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060009BA RID: 2490 RVA: 0x0001E7E0 File Offset: 0x0001C9E0
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060009BB RID: 2491 RVA: 0x0001E7EE File Offset: 0x0001C9EE
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060009BC RID: 2492 RVA: 0x0001E7FE File Offset: 0x0001C9FE
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060009BD RID: 2493 RVA: 0x0001E80B File Offset: 0x0001CA0B
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0400031E RID: 798
			private int completedCount;

			// Token: 0x0400031F RID: 799
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				null,
				null,
				null,
				null
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10>>> core;
		}

		// Token: 0x0200019E RID: 414
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>>
		{
			// Token: 0x060009BE RID: 2494 RVA: 0x0001E818 File Offset: 0x0001CA18
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT11(this, awaiter11);
					return;
				}
				awaiter11.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T11>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, UniTask<T11>.Awaiter>(this, awaiter11));
			}

			// Token: 0x060009BF RID: 2495 RVA: 0x0001EB58 File Offset: 0x0001CD58
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11>(default(T7), default(T8), default(T9), default(T10), default(T11))));
				}
			}

			// Token: 0x060009C0 RID: 2496 RVA: 0x0001EC1C File Offset: 0x0001CE1C
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11>(default(T7), default(T8), default(T9), default(T10), default(T11))));
				}
			}

			// Token: 0x060009C1 RID: 2497 RVA: 0x0001ECE0 File Offset: 0x0001CEE0
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11>(default(T7), default(T8), default(T9), default(T10), default(T11))));
				}
			}

			// Token: 0x060009C2 RID: 2498 RVA: 0x0001EDA4 File Offset: 0x0001CFA4
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11>(default(T7), default(T8), default(T9), default(T10), default(T11))));
				}
			}

			// Token: 0x060009C3 RID: 2499 RVA: 0x0001EE68 File Offset: 0x0001D068
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6), new ValueTuple<T7, T8, T9, T10, T11>(default(T7), default(T8), default(T9), default(T10), default(T11))));
				}
			}

			// Token: 0x060009C4 RID: 2500 RVA: 0x0001EF2C File Offset: 0x0001D12C
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result, new ValueTuple<T7, T8, T9, T10, T11>(default(T7), default(T8), default(T9), default(T10), default(T11))));
				}
			}

			// Token: 0x060009C5 RID: 2501 RVA: 0x0001EFF0 File Offset: 0x0001D1F0
			private static void TryInvokeContinuationT7(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T7>.Awaiter awaiter)
			{
				T7 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(6, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11>(result, default(T8), default(T9), default(T10), default(T11))));
				}
			}

			// Token: 0x060009C6 RID: 2502 RVA: 0x0001F0B4 File Offset: 0x0001D2B4
			private static void TryInvokeContinuationT8(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T8>.Awaiter awaiter)
			{
				T8 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(7, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11>(default(T7), result, default(T9), default(T10), default(T11))));
				}
			}

			// Token: 0x060009C7 RID: 2503 RVA: 0x0001F178 File Offset: 0x0001D378
			private static void TryInvokeContinuationT9(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T9>.Awaiter awaiter)
			{
				T9 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(8, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11>(default(T7), default(T8), result, default(T10), default(T11))));
				}
			}

			// Token: 0x060009C8 RID: 2504 RVA: 0x0001F23C File Offset: 0x0001D43C
			private static void TryInvokeContinuationT10(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T10>.Awaiter awaiter)
			{
				T10 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(9, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11>(default(T7), default(T8), default(T9), result, default(T11))));
				}
			}

			// Token: 0x060009C9 RID: 2505 RVA: 0x0001F300 File Offset: 0x0001D500
			private static void TryInvokeContinuationT11(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, in UniTask<T11>.Awaiter awaiter)
			{
				T11 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>(10, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11>(default(T7), default(T8), default(T9), default(T10), result)));
				}
			}

			// Token: 0x060009CA RID: 2506 RVA: 0x0001F3C4 File Offset: 0x0001D5C4
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				null,
				null,
				null,
				null,
				null
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060009CB RID: 2507 RVA: 0x0001F3D8 File Offset: 0x0001D5D8
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060009CC RID: 2508 RVA: 0x0001F3E6 File Offset: 0x0001D5E6
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060009CD RID: 2509 RVA: 0x0001F3F6 File Offset: 0x0001D5F6
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060009CE RID: 2510 RVA: 0x0001F403 File Offset: 0x0001D603
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000320 RID: 800
			private int completedCount;

			// Token: 0x04000321 RID: 801
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				null,
				null,
				null,
				null,
				null
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11>>> core;
		}

		// Token: 0x0200019F RID: 415
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>>
		{
			// Token: 0x060009CF RID: 2511 RVA: 0x0001F410 File Offset: 0x0001D610
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT11(this, awaiter11);
				}
				else
				{
					awaiter11.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T11>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T11>.Awaiter>(this, awaiter11));
				}
				UniTask<T12>.Awaiter awaiter12 = task12.GetAwaiter();
				if (awaiter12.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT12(this, awaiter12);
					return;
				}
				awaiter12.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T12>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T12>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.TryInvokeContinuationT12(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, UniTask<T12>.Awaiter>(this, awaiter12));
			}

			// Token: 0x060009D0 RID: 2512 RVA: 0x0001F79C File Offset: 0x0001D99C
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12))));
				}
			}

			// Token: 0x060009D1 RID: 2513 RVA: 0x0001F86C File Offset: 0x0001DA6C
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12))));
				}
			}

			// Token: 0x060009D2 RID: 2514 RVA: 0x0001F93C File Offset: 0x0001DB3C
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12))));
				}
			}

			// Token: 0x060009D3 RID: 2515 RVA: 0x0001FA0C File Offset: 0x0001DC0C
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12))));
				}
			}

			// Token: 0x060009D4 RID: 2516 RVA: 0x0001FADC File Offset: 0x0001DCDC
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12))));
				}
			}

			// Token: 0x060009D5 RID: 2517 RVA: 0x0001FBAC File Offset: 0x0001DDAC
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result, new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12))));
				}
			}

			// Token: 0x060009D6 RID: 2518 RVA: 0x0001FC7C File Offset: 0x0001DE7C
			private static void TryInvokeContinuationT7(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T7>.Awaiter awaiter)
			{
				T7 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(6, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(result, default(T8), default(T9), default(T10), default(T11), default(T12))));
				}
			}

			// Token: 0x060009D7 RID: 2519 RVA: 0x0001FD4C File Offset: 0x0001DF4C
			private static void TryInvokeContinuationT8(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T8>.Awaiter awaiter)
			{
				T8 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(7, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), result, default(T9), default(T10), default(T11), default(T12))));
				}
			}

			// Token: 0x060009D8 RID: 2520 RVA: 0x0001FE1C File Offset: 0x0001E01C
			private static void TryInvokeContinuationT9(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T9>.Awaiter awaiter)
			{
				T9 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(8, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), result, default(T10), default(T11), default(T12))));
				}
			}

			// Token: 0x060009D9 RID: 2521 RVA: 0x0001FEEC File Offset: 0x0001E0EC
			private static void TryInvokeContinuationT10(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T10>.Awaiter awaiter)
			{
				T10 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(9, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), default(T9), result, default(T11), default(T12))));
				}
			}

			// Token: 0x060009DA RID: 2522 RVA: 0x0001FFBC File Offset: 0x0001E1BC
			private static void TryInvokeContinuationT11(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T11>.Awaiter awaiter)
			{
				T11 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(10, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), default(T9), default(T10), result, default(T12))));
				}
			}

			// Token: 0x060009DB RID: 2523 RVA: 0x0002008C File Offset: 0x0001E28C
			private static void TryInvokeContinuationT12(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, in UniTask<T12>.Awaiter awaiter)
			{
				T12 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>(11, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12>(default(T7), default(T8), default(T9), default(T10), default(T11), result)));
				}
			}

			// Token: 0x060009DC RID: 2524 RVA: 0x0002015C File Offset: 0x0001E35C
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				"result12",
				null,
				null,
				null,
				null,
				null,
				null
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060009DD RID: 2525 RVA: 0x00020170 File Offset: 0x0001E370
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060009DE RID: 2526 RVA: 0x0002017E File Offset: 0x0001E37E
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060009DF RID: 2527 RVA: 0x0002018E File Offset: 0x0001E38E
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060009E0 RID: 2528 RVA: 0x0002019B File Offset: 0x0001E39B
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000322 RID: 802
			private int completedCount;

			// Token: 0x04000323 RID: 803
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				"result12",
				null,
				null,
				null,
				null,
				null,
				null
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12>>> core;
		}

		// Token: 0x020001A0 RID: 416
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>>
		{
			// Token: 0x060009E1 RID: 2529 RVA: 0x000201A8 File Offset: 0x0001E3A8
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT11(this, awaiter11);
				}
				else
				{
					awaiter11.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T11>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T11>.Awaiter>(this, awaiter11));
				}
				UniTask<T12>.Awaiter awaiter12 = task12.GetAwaiter();
				if (awaiter12.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT12(this, awaiter12);
				}
				else
				{
					awaiter12.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T12>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T12>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT12(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T12>.Awaiter>(this, awaiter12));
				}
				UniTask<T13>.Awaiter awaiter13 = task13.GetAwaiter();
				if (awaiter13.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT13(this, awaiter13);
					return;
				}
				awaiter13.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T13>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T13>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.TryInvokeContinuationT13(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, UniTask<T13>.Awaiter>(this, awaiter13));
			}

			// Token: 0x060009E2 RID: 2530 RVA: 0x0002057C File Offset: 0x0001E77C
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009E3 RID: 2531 RVA: 0x00020654 File Offset: 0x0001E854
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009E4 RID: 2532 RVA: 0x0002072C File Offset: 0x0001E92C
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009E5 RID: 2533 RVA: 0x00020804 File Offset: 0x0001EA04
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009E6 RID: 2534 RVA: 0x000208DC File Offset: 0x0001EADC
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009E7 RID: 2535 RVA: 0x000209B4 File Offset: 0x0001EBB4
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result, new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009E8 RID: 2536 RVA: 0x00020A8C File Offset: 0x0001EC8C
			private static void TryInvokeContinuationT7(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T7>.Awaiter awaiter)
			{
				T7 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(6, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(result, default(T8), default(T9), default(T10), default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009E9 RID: 2537 RVA: 0x00020B64 File Offset: 0x0001ED64
			private static void TryInvokeContinuationT8(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T8>.Awaiter awaiter)
			{
				T8 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(7, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), result, default(T9), default(T10), default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009EA RID: 2538 RVA: 0x00020C3C File Offset: 0x0001EE3C
			private static void TryInvokeContinuationT9(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T9>.Awaiter awaiter)
			{
				T9 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(8, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), result, default(T10), default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009EB RID: 2539 RVA: 0x00020D14 File Offset: 0x0001EF14
			private static void TryInvokeContinuationT10(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T10>.Awaiter awaiter)
			{
				T10 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(9, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), result, default(T11), default(T12), default(T13))));
				}
			}

			// Token: 0x060009EC RID: 2540 RVA: 0x00020DF0 File Offset: 0x0001EFF0
			private static void TryInvokeContinuationT11(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T11>.Awaiter awaiter)
			{
				T11 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(10, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), default(T10), result, default(T12), default(T13))));
				}
			}

			// Token: 0x060009ED RID: 2541 RVA: 0x00020ECC File Offset: 0x0001F0CC
			private static void TryInvokeContinuationT12(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T12>.Awaiter awaiter)
			{
				T12 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(11, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), default(T10), default(T11), result, default(T13))));
				}
			}

			// Token: 0x060009EE RID: 2542 RVA: 0x00020FA8 File Offset: 0x0001F1A8
			private static void TryInvokeContinuationT13(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, in UniTask<T13>.Awaiter awaiter)
			{
				T13 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>(12, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), result)));
				}
			}

			// Token: 0x060009EF RID: 2543 RVA: 0x00021084 File Offset: 0x0001F284
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				"result12",
				"result13",
				null,
				null,
				null,
				null,
				null,
				null,
				null
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x060009F0 RID: 2544 RVA: 0x00021098 File Offset: 0x0001F298
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x060009F1 RID: 2545 RVA: 0x000210A6 File Offset: 0x0001F2A6
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x060009F2 RID: 2546 RVA: 0x000210B6 File Offset: 0x0001F2B6
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x060009F3 RID: 2547 RVA: 0x000210C3 File Offset: 0x0001F2C3
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000324 RID: 804
			private int completedCount;

			// Token: 0x04000325 RID: 805
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				"result12",
				"result13",
				null,
				null,
				null,
				null,
				null,
				null,
				null
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13>>> core;
		}

		// Token: 0x020001A1 RID: 417
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>>
		{
			// Token: 0x060009F4 RID: 2548 RVA: 0x000210D0 File Offset: 0x0001F2D0
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13, UniTask<T14> task14)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT11(this, awaiter11);
				}
				else
				{
					awaiter11.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T11>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T11>.Awaiter>(this, awaiter11));
				}
				UniTask<T12>.Awaiter awaiter12 = task12.GetAwaiter();
				if (awaiter12.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT12(this, awaiter12);
				}
				else
				{
					awaiter12.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T12>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T12>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT12(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T12>.Awaiter>(this, awaiter12));
				}
				UniTask<T13>.Awaiter awaiter13 = task13.GetAwaiter();
				if (awaiter13.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT13(this, awaiter13);
				}
				else
				{
					awaiter13.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T13>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T13>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT13(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T13>.Awaiter>(this, awaiter13));
				}
				UniTask<T14>.Awaiter awaiter14 = task14.GetAwaiter();
				if (awaiter14.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT14(this, awaiter14);
					return;
				}
				awaiter14.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T14>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T14>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>.TryInvokeContinuationT14(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, UniTask<T14>.Awaiter>(this, awaiter14));
			}

			// Token: 0x060009F5 RID: 2549 RVA: 0x000214F0 File Offset: 0x0001F6F0
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009F6 RID: 2550 RVA: 0x000215D8 File Offset: 0x0001F7D8
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009F7 RID: 2551 RVA: 0x000216C0 File Offset: 0x0001F8C0
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009F8 RID: 2552 RVA: 0x000217A8 File Offset: 0x0001F9A8
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009F9 RID: 2553 RVA: 0x00021890 File Offset: 0x0001FA90
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009FA RID: 2554 RVA: 0x00021978 File Offset: 0x0001FB78
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result, new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009FB RID: 2555 RVA: 0x00021A60 File Offset: 0x0001FC60
			private static void TryInvokeContinuationT7(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T7>.Awaiter awaiter)
			{
				T7 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(6, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(result, default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009FC RID: 2556 RVA: 0x00021B48 File Offset: 0x0001FD48
			private static void TryInvokeContinuationT8(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T8>.Awaiter awaiter)
			{
				T8 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(7, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), result, default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009FD RID: 2557 RVA: 0x00021C30 File Offset: 0x0001FE30
			private static void TryInvokeContinuationT9(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T9>.Awaiter awaiter)
			{
				T9 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(8, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), result, default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009FE RID: 2558 RVA: 0x00021D18 File Offset: 0x0001FF18
			private static void TryInvokeContinuationT10(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T10>.Awaiter awaiter)
			{
				T10 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(9, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), result, default(T11), default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x060009FF RID: 2559 RVA: 0x00021E00 File Offset: 0x00020000
			private static void TryInvokeContinuationT11(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T11>.Awaiter awaiter)
			{
				T11 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(10, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), result, default(T12), default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x06000A00 RID: 2560 RVA: 0x00021EE8 File Offset: 0x000200E8
			private static void TryInvokeContinuationT12(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T12>.Awaiter awaiter)
			{
				T12 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(11, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), default(T11), result, default(T13), new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x06000A01 RID: 2561 RVA: 0x00021FD0 File Offset: 0x000201D0
			private static void TryInvokeContinuationT13(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T13>.Awaiter awaiter)
			{
				T13 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(12, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), result, new ValueTuple<T14>(default(T14)))));
				}
			}

			// Token: 0x06000A02 RID: 2562 RVA: 0x000220B8 File Offset: 0x000202B8
			private static void TryInvokeContinuationT14(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, in UniTask<T14>.Awaiter awaiter)
			{
				T14 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>(13, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14>(result))));
				}
			}

			// Token: 0x06000A03 RID: 2563 RVA: 0x000221A0 File Offset: 0x000203A0
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				"result12",
				"result13",
				"result14",
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000A04 RID: 2564 RVA: 0x000221B4 File Offset: 0x000203B4
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000A05 RID: 2565 RVA: 0x000221C2 File Offset: 0x000203C2
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000A06 RID: 2566 RVA: 0x000221D2 File Offset: 0x000203D2
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000A07 RID: 2567 RVA: 0x000221DF File Offset: 0x000203DF
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000326 RID: 806
			private int completedCount;

			// Token: 0x04000327 RID: 807
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				"result12",
				"result13",
				"result14",
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14>>>> core;
		}

		// Token: 0x020001A2 RID: 418
		private sealed class WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IUniTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>>
		{
			// Token: 0x06000A08 RID: 2568 RVA: 0x000221EC File Offset: 0x000203EC
			public WhenAnyPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3, UniTask<T4> task4, UniTask<T5> task5, UniTask<T6> task6, UniTask<T7> task7, UniTask<T8> task8, UniTask<T9> task9, UniTask<T10> task10, UniTask<T11> task11, UniTask<T12> task12, UniTask<T13> task13, UniTask<T14> task14, UniTask<T15> task15)
			{
				this.completedCount = 0;
				UniTask<T1>.Awaiter awaiter = task1.GetAwaiter();
				if (awaiter.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT1(this, awaiter);
				}
				else
				{
					awaiter.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T1>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T1>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT1(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T1>.Awaiter>(this, awaiter));
				}
				UniTask<T2>.Awaiter awaiter2 = task2.GetAwaiter();
				if (awaiter2.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT2(this, awaiter2);
				}
				else
				{
					awaiter2.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T2>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T2>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT2(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T2>.Awaiter>(this, awaiter2));
				}
				UniTask<T3>.Awaiter awaiter3 = task3.GetAwaiter();
				if (awaiter3.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT3(this, awaiter3);
				}
				else
				{
					awaiter3.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T3>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T3>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT3(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T3>.Awaiter>(this, awaiter3));
				}
				UniTask<T4>.Awaiter awaiter4 = task4.GetAwaiter();
				if (awaiter4.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT4(this, awaiter4);
				}
				else
				{
					awaiter4.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T4>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T4>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT4(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T4>.Awaiter>(this, awaiter4));
				}
				UniTask<T5>.Awaiter awaiter5 = task5.GetAwaiter();
				if (awaiter5.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT5(this, awaiter5);
				}
				else
				{
					awaiter5.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T5>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T5>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT5(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T5>.Awaiter>(this, awaiter5));
				}
				UniTask<T6>.Awaiter awaiter6 = task6.GetAwaiter();
				if (awaiter6.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT6(this, awaiter6);
				}
				else
				{
					awaiter6.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T6>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T6>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT6(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T6>.Awaiter>(this, awaiter6));
				}
				UniTask<T7>.Awaiter awaiter7 = task7.GetAwaiter();
				if (awaiter7.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT7(this, awaiter7);
				}
				else
				{
					awaiter7.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T7>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T7>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT7(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T7>.Awaiter>(this, awaiter7));
				}
				UniTask<T8>.Awaiter awaiter8 = task8.GetAwaiter();
				if (awaiter8.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT8(this, awaiter8);
				}
				else
				{
					awaiter8.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T8>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T8>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT8(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T8>.Awaiter>(this, awaiter8));
				}
				UniTask<T9>.Awaiter awaiter9 = task9.GetAwaiter();
				if (awaiter9.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT9(this, awaiter9);
				}
				else
				{
					awaiter9.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T9>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T9>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT9(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T9>.Awaiter>(this, awaiter9));
				}
				UniTask<T10>.Awaiter awaiter10 = task10.GetAwaiter();
				if (awaiter10.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT10(this, awaiter10);
				}
				else
				{
					awaiter10.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T10>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T10>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT10(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T10>.Awaiter>(this, awaiter10));
				}
				UniTask<T11>.Awaiter awaiter11 = task11.GetAwaiter();
				if (awaiter11.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT11(this, awaiter11);
				}
				else
				{
					awaiter11.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T11>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T11>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT11(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T11>.Awaiter>(this, awaiter11));
				}
				UniTask<T12>.Awaiter awaiter12 = task12.GetAwaiter();
				if (awaiter12.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT12(this, awaiter12);
				}
				else
				{
					awaiter12.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T12>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T12>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT12(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T12>.Awaiter>(this, awaiter12));
				}
				UniTask<T13>.Awaiter awaiter13 = task13.GetAwaiter();
				if (awaiter13.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT13(this, awaiter13);
				}
				else
				{
					awaiter13.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T13>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T13>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT13(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T13>.Awaiter>(this, awaiter13));
				}
				UniTask<T14>.Awaiter awaiter14 = task14.GetAwaiter();
				if (awaiter14.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT14(this, awaiter14);
				}
				else
				{
					awaiter14.SourceOnCompleted(delegate(object state)
					{
						using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T14>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T14>.Awaiter>)state)
						{
							UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT14(stateTuple.Item1, stateTuple.Item2);
						}
					}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T14>.Awaiter>(this, awaiter14));
				}
				UniTask<T15>.Awaiter awaiter15 = task15.GetAwaiter();
				if (awaiter15.IsCompleted)
				{
					UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT15(this, awaiter15);
					return;
				}
				awaiter15.SourceOnCompleted(delegate(object state)
				{
					using (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T15>.Awaiter> stateTuple = (StateTuple<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T15>.Awaiter>)state)
					{
						UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>.TryInvokeContinuationT15(stateTuple.Item1, stateTuple.Item2);
					}
				}, StateTuple.Create<UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, UniTask<T15>.Awaiter>(this, awaiter15));
			}

			// Token: 0x06000A09 RID: 2569 RVA: 0x00022654 File Offset: 0x00020854
			private static void TryInvokeContinuationT1(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T1>.Awaiter awaiter)
			{
				T1 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(0, result, default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A0A RID: 2570 RVA: 0x00022748 File Offset: 0x00020948
			private static void TryInvokeContinuationT2(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T2>.Awaiter awaiter)
			{
				T2 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(1, default(T1), result, default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A0B RID: 2571 RVA: 0x0002283C File Offset: 0x00020A3C
			private static void TryInvokeContinuationT3(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T3>.Awaiter awaiter)
			{
				T3 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(2, default(T1), default(T2), result, default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A0C RID: 2572 RVA: 0x00022930 File Offset: 0x00020B30
			private static void TryInvokeContinuationT4(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T4>.Awaiter awaiter)
			{
				T4 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(3, default(T1), default(T2), default(T3), result, default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A0D RID: 2573 RVA: 0x00022A24 File Offset: 0x00020C24
			private static void TryInvokeContinuationT5(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T5>.Awaiter awaiter)
			{
				T5 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(4, default(T1), default(T2), default(T3), default(T4), result, default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A0E RID: 2574 RVA: 0x00022B18 File Offset: 0x00020D18
			private static void TryInvokeContinuationT6(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T6>.Awaiter awaiter)
			{
				T6 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(5, default(T1), default(T2), default(T3), default(T4), default(T5), result, new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A0F RID: 2575 RVA: 0x00022C0C File Offset: 0x00020E0C
			private static void TryInvokeContinuationT7(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T7>.Awaiter awaiter)
			{
				T7 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(6, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(result, default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A10 RID: 2576 RVA: 0x00022D00 File Offset: 0x00020F00
			private static void TryInvokeContinuationT8(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T8>.Awaiter awaiter)
			{
				T8 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(7, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), result, default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A11 RID: 2577 RVA: 0x00022DF4 File Offset: 0x00020FF4
			private static void TryInvokeContinuationT9(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T9>.Awaiter awaiter)
			{
				T9 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(8, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), result, default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A12 RID: 2578 RVA: 0x00022EE8 File Offset: 0x000210E8
			private static void TryInvokeContinuationT10(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T10>.Awaiter awaiter)
			{
				T10 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(9, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), result, default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A13 RID: 2579 RVA: 0x00022FDC File Offset: 0x000211DC
			private static void TryInvokeContinuationT11(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T11>.Awaiter awaiter)
			{
				T11 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(10, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), result, default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A14 RID: 2580 RVA: 0x000230D0 File Offset: 0x000212D0
			private static void TryInvokeContinuationT12(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T12>.Awaiter awaiter)
			{
				T12 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(11, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), result, default(T13), new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A15 RID: 2581 RVA: 0x000231C4 File Offset: 0x000213C4
			private static void TryInvokeContinuationT13(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T13>.Awaiter awaiter)
			{
				T13 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(12, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), result, new ValueTuple<T14, T15>(default(T14), default(T15)))));
				}
			}

			// Token: 0x06000A16 RID: 2582 RVA: 0x000232B8 File Offset: 0x000214B8
			private static void TryInvokeContinuationT14(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T14>.Awaiter awaiter)
			{
				T14 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(13, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(result, default(T15)))));
				}
			}

			// Token: 0x06000A17 RID: 2583 RVA: 0x000233AC File Offset: 0x000215AC
			private static void TryInvokeContinuationT15(UniTask.WhenAnyPromise<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, in UniTask<T15>.Awaiter awaiter)
			{
				T15 result;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception error)
				{
					self.core.TrySetException(error);
					return;
				}
				if (Interlocked.Increment(ref self.completedCount) == 1)
				{
					self.core.TrySetResult(new ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>(14, default(T1), default(T2), default(T3), default(T4), default(T5), default(T6), new ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>(default(T7), default(T8), default(T9), default(T10), default(T11), default(T12), default(T13), new ValueTuple<T14, T15>(default(T14), result))));
				}
			}

			// Token: 0x06000A18 RID: 2584 RVA: 0x000234A0 File Offset: 0x000216A0
			[return: TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				"result12",
				"result13",
				"result14",
				"result15",
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			})]
			public ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>> GetResult(short token)
			{
				GC.SuppressFinalize(this);
				return this.core.GetResult(token);
			}

			// Token: 0x06000A19 RID: 2585 RVA: 0x000234B4 File Offset: 0x000216B4
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000A1A RID: 2586 RVA: 0x000234C2 File Offset: 0x000216C2
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000A1B RID: 2587 RVA: 0x000234D2 File Offset: 0x000216D2
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000A1C RID: 2588 RVA: 0x000234DF File Offset: 0x000216DF
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x04000328 RID: 808
			private int completedCount;

			// Token: 0x04000329 RID: 809
			[TupleElementNames(new string[]
			{
				null,
				"result1",
				"result2",
				"result3",
				"result4",
				"result5",
				"result6",
				"result7",
				"result8",
				"result9",
				"result10",
				"result11",
				"result12",
				"result13",
				"result14",
				"result15",
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			})]
			private UniTaskCompletionSourceCore<ValueTuple<int, T1, T2, T3, T4, T5, T6, ValueTuple<T7, T8, T9, T10, T11, T12, T13, ValueTuple<T14, T15>>>> core;
		}
	}
}
