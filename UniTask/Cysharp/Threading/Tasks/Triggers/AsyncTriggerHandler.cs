using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks.Sources;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ParticleSystemJobs;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x02000071 RID: 113
	public sealed class AsyncTriggerHandler<T> : IAsyncOneShotTrigger, IUniTaskSource<!0>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>, ITriggerHandler<!0>, IDisposable, IAsyncFixedUpdateHandler, IAsyncLateUpdateHandler, IAsyncOnAnimatorIKHandler, IAsyncOnAnimatorMoveHandler, IAsyncOnApplicationFocusHandler, IAsyncOnApplicationPauseHandler, IAsyncOnApplicationQuitHandler, IAsyncOnAudioFilterReadHandler, IAsyncOnBecameInvisibleHandler, IAsyncOnBecameVisibleHandler, IAsyncOnBeforeTransformParentChangedHandler, IAsyncOnCanvasGroupChangedHandler, IAsyncOnCollisionEnterHandler, IAsyncOnCollisionEnter2DHandler, IAsyncOnCollisionExitHandler, IAsyncOnCollisionExit2DHandler, IAsyncOnCollisionStayHandler, IAsyncOnCollisionStay2DHandler, IAsyncOnControllerColliderHitHandler, IAsyncOnDisableHandler, IAsyncOnDrawGizmosHandler, IAsyncOnDrawGizmosSelectedHandler, IAsyncOnEnableHandler, IAsyncOnGUIHandler, IAsyncOnJointBreakHandler, IAsyncOnJointBreak2DHandler, IAsyncOnMouseDownHandler, IAsyncOnMouseDragHandler, IAsyncOnMouseEnterHandler, IAsyncOnMouseExitHandler, IAsyncOnMouseOverHandler, IAsyncOnMouseUpHandler, IAsyncOnMouseUpAsButtonHandler, IAsyncOnParticleCollisionHandler, IAsyncOnParticleSystemStoppedHandler, IAsyncOnParticleTriggerHandler, IAsyncOnParticleUpdateJobScheduledHandler, IAsyncOnPostRenderHandler, IAsyncOnPreCullHandler, IAsyncOnPreRenderHandler, IAsyncOnRectTransformDimensionsChangeHandler, IAsyncOnRectTransformRemovedHandler, IAsyncOnRenderImageHandler, IAsyncOnRenderObjectHandler, IAsyncOnServerInitializedHandler, IAsyncOnTransformChildrenChangedHandler, IAsyncOnTransformParentChangedHandler, IAsyncOnTriggerEnterHandler, IAsyncOnTriggerEnter2DHandler, IAsyncOnTriggerExitHandler, IAsyncOnTriggerExit2DHandler, IAsyncOnTriggerStayHandler, IAsyncOnTriggerStay2DHandler, IAsyncOnValidateHandler, IAsyncOnWillRenderObjectHandler, IAsyncResetHandler, IAsyncUpdateHandler, IAsyncOnBeginDragHandler, IAsyncOnCancelHandler, IAsyncOnDeselectHandler, IAsyncOnDragHandler, IAsyncOnDropHandler, IAsyncOnEndDragHandler, IAsyncOnInitializePotentialDragHandler, IAsyncOnMoveHandler, IAsyncOnPointerClickHandler, IAsyncOnPointerDownHandler, IAsyncOnPointerEnterHandler, IAsyncOnPointerExitHandler, IAsyncOnPointerUpHandler, IAsyncOnScrollHandler, IAsyncOnSelectHandler, IAsyncOnSubmitHandler, IAsyncOnUpdateSelectedHandler
	{
		// Token: 0x06000390 RID: 912 RVA: 0x0000B02C File Offset: 0x0000922C
		UniTask IAsyncOneShotTrigger.OneShotAsync()
		{
			this.core.Reset();
			return new UniTask(this, this.core.Version);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000B04A File Offset: 0x0000924A
		internal CancellationToken CancellationToken
		{
			get
			{
				return this.cancellationToken;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000B052 File Offset: 0x00009252
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0000B05A File Offset: 0x0000925A
		ITriggerHandler<T> ITriggerHandler<!0>.Prev { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000B063 File Offset: 0x00009263
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000B06B File Offset: 0x0000926B
		ITriggerHandler<T> ITriggerHandler<!0>.Next { get; set; }

		// Token: 0x06000396 RID: 918 RVA: 0x0000B074 File Offset: 0x00009274
		internal AsyncTriggerHandler(AsyncTriggerBase<T> trigger, bool callOnce)
		{
			if (this.cancellationToken.IsCancellationRequested)
			{
				this.isDisposed = true;
				return;
			}
			this.trigger = trigger;
			this.cancellationToken = default(CancellationToken);
			this.registration = default(CancellationTokenRegistration);
			this.callOnce = callOnce;
			trigger.AddHandler(this);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000B0CC File Offset: 0x000092CC
		internal AsyncTriggerHandler(AsyncTriggerBase<T> trigger, CancellationToken cancellationToken, bool callOnce)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				this.isDisposed = true;
				return;
			}
			this.trigger = trigger;
			this.cancellationToken = cancellationToken;
			this.callOnce = callOnce;
			trigger.AddHandler(this);
			if (cancellationToken.CanBeCanceled)
			{
				this.registration = cancellationToken.RegisterWithoutCaptureExecutionContext(AsyncTriggerHandler<T>.cancellationCallback, this);
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000B128 File Offset: 0x00009328
		private static void CancellationCallback(object state)
		{
			AsyncTriggerHandler<T> asyncTriggerHandler = (AsyncTriggerHandler<T>)state;
			asyncTriggerHandler.Dispose();
			asyncTriggerHandler.core.TrySetCanceled(asyncTriggerHandler.cancellationToken);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000B154 File Offset: 0x00009354
		public void Dispose()
		{
			if (!this.isDisposed)
			{
				this.isDisposed = true;
				this.registration.Dispose();
				this.trigger.RemoveHandler(this);
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000B17C File Offset: 0x0000937C
		T IUniTaskSource<!0>.GetResult(short token)
		{
			T result;
			try
			{
				result = this.core.GetResult(token);
			}
			finally
			{
				if (this.callOnce)
				{
					this.Dispose();
				}
			}
			return result;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000B1B8 File Offset: 0x000093B8
		void ITriggerHandler<!0>.OnNext(T value)
		{
			this.core.TrySetResult(value);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000B1C7 File Offset: 0x000093C7
		void ITriggerHandler<!0>.OnCanceled(CancellationToken cancellationToken)
		{
			this.core.TrySetCanceled(cancellationToken);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000B1D6 File Offset: 0x000093D6
		void ITriggerHandler<!0>.OnCompleted()
		{
			this.core.TrySetCanceled(CancellationToken.None);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000B1E9 File Offset: 0x000093E9
		void ITriggerHandler<!0>.OnError(Exception ex)
		{
			this.core.TrySetException(ex);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000B1F8 File Offset: 0x000093F8
		void IUniTaskSource.GetResult(short token)
		{
			((IUniTaskSource<!0>)this).GetResult(token);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000B202 File Offset: 0x00009402
		UniTaskStatus IUniTaskSource.GetStatus(short token)
		{
			return this.core.GetStatus(token);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000B210 File Offset: 0x00009410
		UniTaskStatus IUniTaskSource.UnsafeGetStatus()
		{
			return this.core.UnsafeGetStatus();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000B21D File Offset: 0x0000941D
		void IUniTaskSource.OnCompleted(Action<object> continuation, object state, short token)
		{
			this.core.OnCompleted(continuation, state, token);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000B22D File Offset: 0x0000942D
		UniTask IAsyncFixedUpdateHandler.FixedUpdateAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000B250 File Offset: 0x00009450
		UniTask IAsyncLateUpdateHandler.LateUpdateAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000B273 File Offset: 0x00009473
		UniTask<int> IAsyncOnAnimatorIKHandler.OnAnimatorIKAsync()
		{
			this.core.Reset();
			return new UniTask<int>((IUniTaskSource<int>)this, this.core.Version);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000B296 File Offset: 0x00009496
		UniTask IAsyncOnAnimatorMoveHandler.OnAnimatorMoveAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000B2B9 File Offset: 0x000094B9
		UniTask<bool> IAsyncOnApplicationFocusHandler.OnApplicationFocusAsync()
		{
			this.core.Reset();
			return new UniTask<bool>((IUniTaskSource<bool>)this, this.core.Version);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000B2DC File Offset: 0x000094DC
		UniTask<bool> IAsyncOnApplicationPauseHandler.OnApplicationPauseAsync()
		{
			this.core.Reset();
			return new UniTask<bool>((IUniTaskSource<bool>)this, this.core.Version);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000B2FF File Offset: 0x000094FF
		UniTask IAsyncOnApplicationQuitHandler.OnApplicationQuitAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000B322 File Offset: 0x00009522
		[return: TupleElementNames(new string[]
		{
			"data",
			"channels"
		})]
		UniTask<ValueTuple<float[], int>> IAsyncOnAudioFilterReadHandler.OnAudioFilterReadAsync()
		{
			this.core.Reset();
			return new UniTask<ValueTuple<float[], int>>((IUniTaskSource<ValueTuple<float[], int>>)this, this.core.Version);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000B345 File Offset: 0x00009545
		UniTask IAsyncOnBecameInvisibleHandler.OnBecameInvisibleAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000B368 File Offset: 0x00009568
		UniTask IAsyncOnBecameVisibleHandler.OnBecameVisibleAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000B38B File Offset: 0x0000958B
		UniTask IAsyncOnBeforeTransformParentChangedHandler.OnBeforeTransformParentChangedAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000B3AE File Offset: 0x000095AE
		UniTask IAsyncOnCanvasGroupChangedHandler.OnCanvasGroupChangedAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000B3D1 File Offset: 0x000095D1
		UniTask<Collision> IAsyncOnCollisionEnterHandler.OnCollisionEnterAsync()
		{
			this.core.Reset();
			return new UniTask<Collision>((IUniTaskSource<Collision>)this, this.core.Version);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000B3F4 File Offset: 0x000095F4
		UniTask<Collision2D> IAsyncOnCollisionEnter2DHandler.OnCollisionEnter2DAsync()
		{
			this.core.Reset();
			return new UniTask<Collision2D>((IUniTaskSource<Collision2D>)this, this.core.Version);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000B417 File Offset: 0x00009617
		UniTask<Collision> IAsyncOnCollisionExitHandler.OnCollisionExitAsync()
		{
			this.core.Reset();
			return new UniTask<Collision>((IUniTaskSource<Collision>)this, this.core.Version);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000B43A File Offset: 0x0000963A
		UniTask<Collision2D> IAsyncOnCollisionExit2DHandler.OnCollisionExit2DAsync()
		{
			this.core.Reset();
			return new UniTask<Collision2D>((IUniTaskSource<Collision2D>)this, this.core.Version);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000B45D File Offset: 0x0000965D
		UniTask<Collision> IAsyncOnCollisionStayHandler.OnCollisionStayAsync()
		{
			this.core.Reset();
			return new UniTask<Collision>((IUniTaskSource<Collision>)this, this.core.Version);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000B480 File Offset: 0x00009680
		UniTask<Collision2D> IAsyncOnCollisionStay2DHandler.OnCollisionStay2DAsync()
		{
			this.core.Reset();
			return new UniTask<Collision2D>((IUniTaskSource<Collision2D>)this, this.core.Version);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000B4A3 File Offset: 0x000096A3
		UniTask<ControllerColliderHit> IAsyncOnControllerColliderHitHandler.OnControllerColliderHitAsync()
		{
			this.core.Reset();
			return new UniTask<ControllerColliderHit>((IUniTaskSource<ControllerColliderHit>)this, this.core.Version);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000B4C6 File Offset: 0x000096C6
		UniTask IAsyncOnDisableHandler.OnDisableAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000B4E9 File Offset: 0x000096E9
		UniTask IAsyncOnDrawGizmosHandler.OnDrawGizmosAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000B50C File Offset: 0x0000970C
		UniTask IAsyncOnDrawGizmosSelectedHandler.OnDrawGizmosSelectedAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000B52F File Offset: 0x0000972F
		UniTask IAsyncOnEnableHandler.OnEnableAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000B552 File Offset: 0x00009752
		UniTask IAsyncOnGUIHandler.OnGUIAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000B575 File Offset: 0x00009775
		UniTask<float> IAsyncOnJointBreakHandler.OnJointBreakAsync()
		{
			this.core.Reset();
			return new UniTask<float>((IUniTaskSource<float>)this, this.core.Version);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000B598 File Offset: 0x00009798
		UniTask<Joint2D> IAsyncOnJointBreak2DHandler.OnJointBreak2DAsync()
		{
			this.core.Reset();
			return new UniTask<Joint2D>((IUniTaskSource<Joint2D>)this, this.core.Version);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000B5BB File Offset: 0x000097BB
		UniTask IAsyncOnMouseDownHandler.OnMouseDownAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000B5DE File Offset: 0x000097DE
		UniTask IAsyncOnMouseDragHandler.OnMouseDragAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000B601 File Offset: 0x00009801
		UniTask IAsyncOnMouseEnterHandler.OnMouseEnterAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000B624 File Offset: 0x00009824
		UniTask IAsyncOnMouseExitHandler.OnMouseExitAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000B647 File Offset: 0x00009847
		UniTask IAsyncOnMouseOverHandler.OnMouseOverAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000B66A File Offset: 0x0000986A
		UniTask IAsyncOnMouseUpHandler.OnMouseUpAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000B68D File Offset: 0x0000988D
		UniTask IAsyncOnMouseUpAsButtonHandler.OnMouseUpAsButtonAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000B6B0 File Offset: 0x000098B0
		UniTask<GameObject> IAsyncOnParticleCollisionHandler.OnParticleCollisionAsync()
		{
			this.core.Reset();
			return new UniTask<GameObject>((IUniTaskSource<GameObject>)this, this.core.Version);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000B6D3 File Offset: 0x000098D3
		UniTask IAsyncOnParticleSystemStoppedHandler.OnParticleSystemStoppedAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000B6F6 File Offset: 0x000098F6
		UniTask IAsyncOnParticleTriggerHandler.OnParticleTriggerAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000B719 File Offset: 0x00009919
		UniTask<ParticleSystemJobData> IAsyncOnParticleUpdateJobScheduledHandler.OnParticleUpdateJobScheduledAsync()
		{
			this.core.Reset();
			return new UniTask<ParticleSystemJobData>((IUniTaskSource<ParticleSystemJobData>)this, this.core.Version);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000B73C File Offset: 0x0000993C
		UniTask IAsyncOnPostRenderHandler.OnPostRenderAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000B75F File Offset: 0x0000995F
		UniTask IAsyncOnPreCullHandler.OnPreCullAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000B782 File Offset: 0x00009982
		UniTask IAsyncOnPreRenderHandler.OnPreRenderAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000B7A5 File Offset: 0x000099A5
		UniTask IAsyncOnRectTransformDimensionsChangeHandler.OnRectTransformDimensionsChangeAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000B7C8 File Offset: 0x000099C8
		UniTask IAsyncOnRectTransformRemovedHandler.OnRectTransformRemovedAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000B7EB File Offset: 0x000099EB
		[return: TupleElementNames(new string[]
		{
			"source",
			"destination"
		})]
		UniTask<ValueTuple<RenderTexture, RenderTexture>> IAsyncOnRenderImageHandler.OnRenderImageAsync()
		{
			this.core.Reset();
			return new UniTask<ValueTuple<RenderTexture, RenderTexture>>((IUniTaskSource<ValueTuple<RenderTexture, RenderTexture>>)this, this.core.Version);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000B80E File Offset: 0x00009A0E
		UniTask IAsyncOnRenderObjectHandler.OnRenderObjectAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000B831 File Offset: 0x00009A31
		UniTask IAsyncOnServerInitializedHandler.OnServerInitializedAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000B854 File Offset: 0x00009A54
		UniTask IAsyncOnTransformChildrenChangedHandler.OnTransformChildrenChangedAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000B877 File Offset: 0x00009A77
		UniTask IAsyncOnTransformParentChangedHandler.OnTransformParentChangedAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000B89A File Offset: 0x00009A9A
		UniTask<Collider> IAsyncOnTriggerEnterHandler.OnTriggerEnterAsync()
		{
			this.core.Reset();
			return new UniTask<Collider>((IUniTaskSource<Collider>)this, this.core.Version);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000B8BD File Offset: 0x00009ABD
		UniTask<Collider2D> IAsyncOnTriggerEnter2DHandler.OnTriggerEnter2DAsync()
		{
			this.core.Reset();
			return new UniTask<Collider2D>((IUniTaskSource<Collider2D>)this, this.core.Version);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000B8E0 File Offset: 0x00009AE0
		UniTask<Collider> IAsyncOnTriggerExitHandler.OnTriggerExitAsync()
		{
			this.core.Reset();
			return new UniTask<Collider>((IUniTaskSource<Collider>)this, this.core.Version);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000B903 File Offset: 0x00009B03
		UniTask<Collider2D> IAsyncOnTriggerExit2DHandler.OnTriggerExit2DAsync()
		{
			this.core.Reset();
			return new UniTask<Collider2D>((IUniTaskSource<Collider2D>)this, this.core.Version);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000B926 File Offset: 0x00009B26
		UniTask<Collider> IAsyncOnTriggerStayHandler.OnTriggerStayAsync()
		{
			this.core.Reset();
			return new UniTask<Collider>((IUniTaskSource<Collider>)this, this.core.Version);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000B949 File Offset: 0x00009B49
		UniTask<Collider2D> IAsyncOnTriggerStay2DHandler.OnTriggerStay2DAsync()
		{
			this.core.Reset();
			return new UniTask<Collider2D>((IUniTaskSource<Collider2D>)this, this.core.Version);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000B96C File Offset: 0x00009B6C
		UniTask IAsyncOnValidateHandler.OnValidateAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000B98F File Offset: 0x00009B8F
		UniTask IAsyncOnWillRenderObjectHandler.OnWillRenderObjectAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000B9B2 File Offset: 0x00009BB2
		UniTask IAsyncResetHandler.ResetAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000B9D5 File Offset: 0x00009BD5
		UniTask IAsyncUpdateHandler.UpdateAsync()
		{
			this.core.Reset();
			return new UniTask((IUniTaskSource)this, this.core.Version);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		UniTask<PointerEventData> IAsyncOnBeginDragHandler.OnBeginDragAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000BA1B File Offset: 0x00009C1B
		UniTask<BaseEventData> IAsyncOnCancelHandler.OnCancelAsync()
		{
			this.core.Reset();
			return new UniTask<BaseEventData>((IUniTaskSource<BaseEventData>)this, this.core.Version);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000BA3E File Offset: 0x00009C3E
		UniTask<BaseEventData> IAsyncOnDeselectHandler.OnDeselectAsync()
		{
			this.core.Reset();
			return new UniTask<BaseEventData>((IUniTaskSource<BaseEventData>)this, this.core.Version);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000BA61 File Offset: 0x00009C61
		UniTask<PointerEventData> IAsyncOnDragHandler.OnDragAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000BA84 File Offset: 0x00009C84
		UniTask<PointerEventData> IAsyncOnDropHandler.OnDropAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000BAA7 File Offset: 0x00009CA7
		UniTask<PointerEventData> IAsyncOnEndDragHandler.OnEndDragAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000BACA File Offset: 0x00009CCA
		UniTask<PointerEventData> IAsyncOnInitializePotentialDragHandler.OnInitializePotentialDragAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000BAED File Offset: 0x00009CED
		UniTask<AxisEventData> IAsyncOnMoveHandler.OnMoveAsync()
		{
			this.core.Reset();
			return new UniTask<AxisEventData>((IUniTaskSource<AxisEventData>)this, this.core.Version);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000BB10 File Offset: 0x00009D10
		UniTask<PointerEventData> IAsyncOnPointerClickHandler.OnPointerClickAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000BB33 File Offset: 0x00009D33
		UniTask<PointerEventData> IAsyncOnPointerDownHandler.OnPointerDownAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000BB56 File Offset: 0x00009D56
		UniTask<PointerEventData> IAsyncOnPointerEnterHandler.OnPointerEnterAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000BB79 File Offset: 0x00009D79
		UniTask<PointerEventData> IAsyncOnPointerExitHandler.OnPointerExitAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000BB9C File Offset: 0x00009D9C
		UniTask<PointerEventData> IAsyncOnPointerUpHandler.OnPointerUpAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000BBBF File Offset: 0x00009DBF
		UniTask<PointerEventData> IAsyncOnScrollHandler.OnScrollAsync()
		{
			this.core.Reset();
			return new UniTask<PointerEventData>((IUniTaskSource<PointerEventData>)this, this.core.Version);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000BBE2 File Offset: 0x00009DE2
		UniTask<BaseEventData> IAsyncOnSelectHandler.OnSelectAsync()
		{
			this.core.Reset();
			return new UniTask<BaseEventData>((IUniTaskSource<BaseEventData>)this, this.core.Version);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000BC05 File Offset: 0x00009E05
		UniTask<BaseEventData> IAsyncOnSubmitHandler.OnSubmitAsync()
		{
			this.core.Reset();
			return new UniTask<BaseEventData>((IUniTaskSource<BaseEventData>)this, this.core.Version);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000BC28 File Offset: 0x00009E28
		UniTask<BaseEventData> IAsyncOnUpdateSelectedHandler.OnUpdateSelectedAsync()
		{
			this.core.Reset();
			return new UniTask<BaseEventData>((IUniTaskSource<BaseEventData>)this, this.core.Version);
		}

		// Token: 0x040000F3 RID: 243
		private static Action<object> cancellationCallback = new Action<object>(AsyncTriggerHandler<T>.CancellationCallback);

		// Token: 0x040000F4 RID: 244
		private readonly AsyncTriggerBase<T> trigger;

		// Token: 0x040000F5 RID: 245
		private CancellationToken cancellationToken;

		// Token: 0x040000F6 RID: 246
		private CancellationTokenRegistration registration;

		// Token: 0x040000F7 RID: 247
		private bool isDisposed;

		// Token: 0x040000F8 RID: 248
		private bool callOnce;

		// Token: 0x040000F9 RID: 249
		private UniTaskCompletionSourceCore<T> core;
	}
}
