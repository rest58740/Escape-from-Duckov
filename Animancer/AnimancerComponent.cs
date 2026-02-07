using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000003 RID: 3
	[AddComponentMenu("Animancer/Animancer Component")]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/AnimancerComponent")]
	[DefaultExecutionOrder(-5000)]
	public class AnimancerComponent : MonoBehaviour, IAnimancerComponent, IEnumerator, IAnimationClipSource, IAnimationClipCollection
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C3 File Offset: 0x000002C3
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020CB File Offset: 0x000002CB
		public Animator Animator
		{
			get
			{
				return this._Animator;
			}
			set
			{
				this._Animator = value;
				if (this.IsPlayableInitialized)
				{
					this._Playable.DestroyOutput();
					this._Playable.CreateOutput(value, this);
				}
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020F5 File Offset: 0x000002F5
		public AnimancerPlayable Playable
		{
			get
			{
				this.InitializePlayable();
				return this._Playable;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002103 File Offset: 0x00000303
		public bool IsPlayableInitialized
		{
			get
			{
				return this._Playable != null && this._Playable.IsValid;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000211A File Offset: 0x0000031A
		public AnimancerPlayable.StateDictionary States
		{
			get
			{
				return this.Playable.States;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002127 File Offset: 0x00000327
		public AnimancerPlayable.LayerList Layers
		{
			get
			{
				return this.Playable.Layers;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002134 File Offset: 0x00000334
		public static implicit operator AnimancerPlayable(AnimancerComponent animancer)
		{
			return animancer.Playable;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000213C File Offset: 0x0000033C
		public static implicit operator AnimancerLayer(AnimancerComponent animancer)
		{
			return animancer.Playable.Layers[0];
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000214F File Offset: 0x0000034F
		public ref AnimancerComponent.DisableAction ActionOnDisable
		{
			get
			{
				return ref this._ActionOnDisable;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002157 File Offset: 0x00000357
		bool IAnimancerComponent.ResetOnDisable
		{
			get
			{
				return this._ActionOnDisable == AnimancerComponent.DisableAction.Reset;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002162 File Offset: 0x00000362
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000216F File Offset: 0x0000036F
		public AnimatorUpdateMode UpdateMode
		{
			get
			{
				return this._Animator.updateMode;
			}
			set
			{
				this._Animator.updateMode = value;
				if (!this.IsPlayableInitialized)
				{
					return;
				}
				this._Playable.UpdateMode = ((value == AnimatorUpdateMode.UnscaledTime) ? DirectorUpdateMode.UnscaledGameTime : DirectorUpdateMode.GameTime);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002199 File Offset: 0x00000399
		protected virtual void OnEnable()
		{
			if (this.IsPlayableInitialized)
			{
				this._Playable.UnpauseGraph();
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021B0 File Offset: 0x000003B0
		protected virtual void OnDisable()
		{
			if (!this.IsPlayableInitialized)
			{
				return;
			}
			switch (this._ActionOnDisable)
			{
			case AnimancerComponent.DisableAction.Stop:
				this.Stop();
				this._Playable.PauseGraph();
				return;
			case AnimancerComponent.DisableAction.Pause:
				this._Playable.PauseGraph();
				return;
			case AnimancerComponent.DisableAction.Continue:
				return;
			case AnimancerComponent.DisableAction.Reset:
				this.Stop();
				this._Animator.Rebind();
				this._Playable.PauseGraph();
				return;
			case AnimancerComponent.DisableAction.Destroy:
				this._Playable.DestroyGraph();
				this._Playable = null;
				return;
			default:
				throw new ArgumentOutOfRangeException("ActionOnDisable");
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002242 File Offset: 0x00000442
		public void InitializePlayable()
		{
			if (this.IsPlayableInitialized)
			{
				return;
			}
			this.TryGetAnimator();
			this._Playable = AnimancerPlayable.Create();
			this._Playable.CreateOutput(this._Animator, this);
			this.OnInitializePlayable();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002277 File Offset: 0x00000477
		public void InitializePlayable(AnimancerPlayable playable)
		{
			if (this.IsPlayableInitialized)
			{
				throw new InvalidOperationException("The AnimancerPlayable is already initialized. Either call this method before anything else uses it or call animancerComponent.Playable.DestroyGraph before re-initializing it.");
			}
			this.TryGetAnimator();
			this._Playable = playable;
			this._Playable.CreateOutput(this._Animator, this);
			this.OnInitializePlayable();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022B2 File Offset: 0x000004B2
		protected virtual void OnInitializePlayable()
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022B4 File Offset: 0x000004B4
		public bool TryGetAnimator()
		{
			return this._Animator != null || base.TryGetComponent<Animator>(out this._Animator);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022D2 File Offset: 0x000004D2
		protected virtual void OnDestroy()
		{
			if (this.IsPlayableInitialized)
			{
				this._Playable.DestroyGraph();
				this._Playable = null;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022EE File Offset: 0x000004EE
		public virtual object GetKey(AnimationClip clip)
		{
			return clip;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000022F1 File Offset: 0x000004F1
		public AnimancerState Play(AnimationClip clip)
		{
			return this.Playable.Play(this.States.GetOrCreate(clip, false));
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000230B File Offset: 0x0000050B
		public AnimancerState Play(AnimancerState state)
		{
			return this.Playable.Play(state);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002319 File Offset: 0x00000519
		public AnimancerState Play(AnimationClip clip, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			return this.Playable.Play(this.States.GetOrCreate(clip, false), fadeDuration, mode);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002335 File Offset: 0x00000535
		public AnimancerState Play(AnimancerState state, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			return this.Playable.Play(state, fadeDuration, mode);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002345 File Offset: 0x00000545
		public AnimancerState Play(ITransition transition)
		{
			return this.Playable.Play(transition);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002353 File Offset: 0x00000553
		public AnimancerState Play(ITransition transition, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			return this.Playable.Play(transition, fadeDuration, mode);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002363 File Offset: 0x00000563
		public AnimancerState TryPlay(object key)
		{
			return this.Playable.TryPlay(key);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002371 File Offset: 0x00000571
		public AnimancerState TryPlay(object key, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			return this.Playable.TryPlay(key, fadeDuration, mode);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002381 File Offset: 0x00000581
		public AnimancerState Stop(AnimationClip clip)
		{
			return this.Stop(this.GetKey(clip));
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002390 File Offset: 0x00000590
		public AnimancerState Stop(IHasKey hasKey)
		{
			AnimancerPlayable playable = this._Playable;
			if (playable == null)
			{
				return null;
			}
			return playable.Stop(hasKey);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000023A4 File Offset: 0x000005A4
		public AnimancerState Stop(object key)
		{
			AnimancerPlayable playable = this._Playable;
			if (playable == null)
			{
				return null;
			}
			return playable.Stop(key);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023B8 File Offset: 0x000005B8
		public void Stop()
		{
			if (this.IsPlayableInitialized)
			{
				this._Playable.Stop();
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023CD File Offset: 0x000005CD
		public bool IsPlaying(AnimationClip clip)
		{
			return this.IsPlaying(this.GetKey(clip));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000023DC File Offset: 0x000005DC
		public bool IsPlaying(IHasKey hasKey)
		{
			return this.IsPlayableInitialized && this._Playable.IsPlaying(hasKey);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000023F4 File Offset: 0x000005F4
		public bool IsPlaying(object key)
		{
			return this.IsPlayableInitialized && this._Playable.IsPlaying(key);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000240C File Offset: 0x0000060C
		public bool IsPlaying()
		{
			return this.IsPlayableInitialized && this._Playable.IsPlaying();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002423 File Offset: 0x00000623
		public bool IsPlayingClip(AnimationClip clip)
		{
			return this.IsPlayableInitialized && this._Playable.IsPlayingClip(clip);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000243B File Offset: 0x0000063B
		public void Evaluate()
		{
			this.Playable.Evaluate();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002448 File Offset: 0x00000648
		public void Evaluate(float deltaTime)
		{
			this.Playable.Evaluate(deltaTime);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002456 File Offset: 0x00000656
		bool IEnumerator.MoveNext()
		{
			return this.IsPlayableInitialized && ((IEnumerator)this._Playable).MoveNext();
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002B RID: 43 RVA: 0x0000246D File Offset: 0x0000066D
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002470 File Offset: 0x00000670
		void IEnumerator.Reset()
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002474 File Offset: 0x00000674
		public void GetAnimationClips(List<AnimationClip> clips)
		{
			HashSet<AnimationClip> hashSet = ObjectPool.AcquireSet<AnimationClip>();
			hashSet.UnionWith(clips);
			this.GatherAnimationClips(hashSet);
			clips.Clear();
			clips.AddRange(hashSet);
			ObjectPool.Release<AnimationClip>(hashSet);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000024A8 File Offset: 0x000006A8
		public virtual void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			if (this.IsPlayableInitialized)
			{
				this._Playable.GatherAnimationClips(clips);
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000024C6 File Offset: 0x000006C6
		bool IAnimancerComponent.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000024CE File Offset: 0x000006CE
		GameObject IAnimancerComponent.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x04000001 RID: 1
		public const int DefaultExecutionOrder = -5000;

		// Token: 0x04000002 RID: 2
		[SerializeField]
		[Tooltip("The Animator component which this script controls")]
		private Animator _Animator;

		// Token: 0x04000003 RID: 3
		private AnimancerPlayable _Playable;

		// Token: 0x04000004 RID: 4
		[SerializeField]
		[Tooltip("Determines what happens when this component is disabled or its GameObject becomes inactive (i.e. in OnDisable):\n• Stop all animations\n• Pause all animations\n• Continue playing\n• Reset to the original values\n• Destroy all layers and states")]
		private AnimancerComponent.DisableAction _ActionOnDisable;

		// Token: 0x02000079 RID: 121
		public enum DisableAction
		{
			// Token: 0x04000105 RID: 261
			Stop,
			// Token: 0x04000106 RID: 262
			Pause,
			// Token: 0x04000107 RID: 263
			Continue,
			// Token: 0x04000108 RID: 264
			Reset,
			// Token: 0x04000109 RID: 265
			Destroy
		}
	}
}
