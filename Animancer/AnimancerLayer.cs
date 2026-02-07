using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000012 RID: 18
	public sealed class AnimancerLayer : AnimancerNode, IAnimationClipCollection
	{
		// Token: 0x0600017A RID: 378 RVA: 0x00004798 File Offset: 0x00002998
		internal AnimancerLayer(AnimancerPlayable root, int index)
		{
			base.Root = root;
			base.Index = index;
			this.CreatePlayable();
			if (AnimancerNode.ApplyParentAnimatorIK)
			{
				this._ApplyAnimatorIK = root.ApplyAnimatorIK;
			}
			if (AnimancerNode.ApplyParentFootIK)
			{
				this._ApplyFootIK = root.ApplyFootIK;
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000047F0 File Offset: 0x000029F0
		protected override void CreatePlayable(out Playable playable)
		{
			playable = AnimationMixerPlayable.Create(base.Root._Graph, 0);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000480E File Offset: 0x00002A0E
		public override AnimancerLayer Layer
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00004811 File Offset: 0x00002A11
		public override IPlayableWrapper Parent
		{
			get
			{
				return base.Root;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00004819 File Offset: 0x00002A19
		public override bool KeepChildrenConnected
		{
			get
			{
				return base.Root.KeepChildrenConnected;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00004826 File Offset: 0x00002A26
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00004830 File Offset: 0x00002A30
		public AnimancerState CurrentState
		{
			get
			{
				return this._CurrentState;
			}
			private set
			{
				this._CurrentState = value;
				int commandCount = this.CommandCount;
				this.CommandCount = commandCount + 1;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00004854 File Offset: 0x00002A54
		// (set) Token: 0x06000182 RID: 386 RVA: 0x0000485C File Offset: 0x00002A5C
		public int CommandCount { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00004865 File Offset: 0x00002A65
		// (set) Token: 0x06000184 RID: 388 RVA: 0x0000487D File Offset: 0x00002A7D
		public bool IsAdditive
		{
			get
			{
				return base.Root.Layers.IsAdditive(base.Index);
			}
			set
			{
				base.Root.Layers.SetAdditive(base.Index, value);
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00004896 File Offset: 0x00002A96
		public void SetMask(AvatarMask mask)
		{
			base.Root.Layers.SetMask(base.Index, mask);
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000048B0 File Offset: 0x00002AB0
		public Vector3 AverageVelocity
		{
			get
			{
				Vector3 vector = default(Vector3);
				for (int i = this.States.Count - 1; i >= 0; i--)
				{
					AnimancerState animancerState = this.States[i];
					vector += animancerState.AverageVelocity * animancerState.Weight;
				}
				return vector;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00004903 File Offset: 0x00002B03
		public override int ChildCount
		{
			get
			{
				return this.States.Count;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00004910 File Offset: 0x00002B10
		public override AnimancerState GetChild(int index)
		{
			return this.States[index];
		}

		// Token: 0x17000063 RID: 99
		public AnimancerState this[int index]
		{
			get
			{
				return this.States[index];
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000492C File Offset: 0x00002B2C
		public void AddChild(AnimancerState state)
		{
			if (state.Parent == this)
			{
				return;
			}
			state.SetRoot(base.Root);
			int count = this.States.Count;
			this.States.Add(null);
			this._Playable.SetInputCount(count + 1);
			state.SetParent(this, count);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000497D File Offset: 0x00002B7D
		protected internal override void OnAddChild(AnimancerState state)
		{
			base.OnAddChild(this.States, state);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000498C File Offset: 0x00002B8C
		protected internal override void OnRemoveChild(AnimancerState state)
		{
			int index = state.Index;
			if (this._Playable.GetInput(index).IsValid<Playable>())
			{
				base.Root._Graph.Disconnect<Playable>(this._Playable, index);
			}
			int num = this.States.Count - 1;
			if (index < num)
			{
				state = this.States[num];
				state.DisconnectFromGraph();
				this.States[index] = state;
				state.Index = index;
				if (state.Weight != 0f || base.Root.KeepChildrenConnected)
				{
					state.ConnectToGraph();
				}
			}
			this.States.RemoveAt(num);
			this._Playable.SetInputCount(num);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00004A3C File Offset: 0x00002C3C
		public override FastEnumerator<AnimancerState> GetEnumerator()
		{
			return new FastEnumerator<AnimancerState>(this.States);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00004A49 File Offset: 0x00002C49
		public ClipState CreateState(AnimationClip clip)
		{
			return this.CreateState(base.Root.GetKey(clip), clip);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00004A60 File Offset: 0x00002C60
		public ClipState CreateState(object key, AnimationClip clip)
		{
			ClipState clipState = new ClipState(clip)
			{
				_Key = key
			};
			this.AddChild(clipState);
			return clipState;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00004A84 File Offset: 0x00002C84
		public AnimancerState GetState(ref object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			object obj = key;
			AnimancerState animancerState;
			for (;;)
			{
				animancerState = (obj as AnimancerState);
				if (animancerState == null)
				{
					break;
				}
				if (animancerState.Parent == this)
				{
					goto Block_3;
				}
				if (animancerState.Parent == null)
				{
					goto Block_4;
				}
				obj = animancerState.Key;
			}
			AnimancerState animancerState2;
			while (base.Root.States.TryGet(key, out animancerState2))
			{
				if (animancerState2.Parent == this)
				{
					return animancerState2;
				}
				if (animancerState2.Parent == null)
				{
					this.AddChild(animancerState2);
					return animancerState2;
				}
				key = animancerState2;
			}
			return null;
			Block_3:
			key = animancerState.Key;
			return animancerState;
			Block_4:
			key = animancerState.Key;
			this.AddChild(animancerState);
			return animancerState;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00004B1A File Offset: 0x00002D1A
		public void CreateIfNew(AnimationClip clip0, AnimationClip clip1)
		{
			this.GetOrCreateState(clip0, false);
			this.GetOrCreateState(clip1, false);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00004B2E File Offset: 0x00002D2E
		public void CreateIfNew(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2)
		{
			this.GetOrCreateState(clip0, false);
			this.GetOrCreateState(clip1, false);
			this.GetOrCreateState(clip2, false);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00004B4B File Offset: 0x00002D4B
		public void CreateIfNew(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2, AnimationClip clip3)
		{
			this.GetOrCreateState(clip0, false);
			this.GetOrCreateState(clip1, false);
			this.GetOrCreateState(clip2, false);
			this.GetOrCreateState(clip3, false);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00004B74 File Offset: 0x00002D74
		public void CreateIfNew(params AnimationClip[] clips)
		{
			if (clips == null)
			{
				return;
			}
			int num = clips.Length;
			for (int i = 0; i < num; i++)
			{
				AnimationClip animationClip = clips[i];
				if (animationClip != null)
				{
					this.GetOrCreateState(animationClip, false);
				}
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00004BAB File Offset: 0x00002DAB
		public AnimancerState GetOrCreateState(AnimationClip clip, bool allowSetClip = false)
		{
			return this.GetOrCreateState(base.Root.GetKey(clip), clip, allowSetClip);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00004BC4 File Offset: 0x00002DC4
		public AnimancerState GetOrCreateState(ITransition transition)
		{
			object key = transition.Key;
			AnimancerState animancerState = this.GetState(ref key);
			if (animancerState == null)
			{
				animancerState = transition.CreateState();
				animancerState.Key = key;
				this.AddChild(animancerState);
			}
			return animancerState;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00004BFC File Offset: 0x00002DFC
		public AnimancerState GetOrCreateState(object key, AnimationClip clip, bool allowSetClip = false)
		{
			AnimancerState state = this.GetState(ref key);
			if (state == null)
			{
				return this.CreateState(key, clip);
			}
			if (state.Clip != clip)
			{
				if (!allowSetClip)
				{
					throw new ArgumentException(AnimancerPlayable.StateDictionary.GetClipMismatchError(key, state.Clip, clip));
				}
				state.Clip = clip;
			}
			return state;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00004C48 File Offset: 0x00002E48
		public AnimancerState GetOrCreateState(AnimancerState state)
		{
			if (state.Parent == this)
			{
				return state;
			}
			if (state.Parent == null)
			{
				this.AddChild(state);
				return state;
			}
			object obj = state.Key;
			if (obj == null)
			{
				obj = state;
			}
			AnimancerState animancerState = this.GetState(ref obj);
			if (animancerState == null)
			{
				animancerState = state.Clone(base.Root);
				animancerState.Key = obj;
				this.AddChild(animancerState);
			}
			return animancerState;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00004CA5 File Offset: 0x00002EA5
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00004CAC File Offset: 0x00002EAC
		public static float WeightlessThreshold { get; set; } = 0.1f;

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00004CB4 File Offset: 0x00002EB4
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00004CBB File Offset: 0x00002EBB
		public static int MaxCloneCount { get; private set; } = 3;

		// Token: 0x0600019D RID: 413 RVA: 0x00004CC4 File Offset: 0x00002EC4
		public AnimancerState GetOrCreateWeightlessState(AnimancerState state)
		{
			if (state.Parent == null)
			{
				state.Weight = 0f;
			}
			else if (state.Parent != this || state.Weight > AnimancerLayer.WeightlessThreshold)
			{
				float num = float.PositiveInfinity;
				AnimancerState animancerState = null;
				int num2 = 0;
				AnimancerState animancerState2 = state;
				for (;;)
				{
					animancerState2 = (animancerState2.Key as AnimancerState);
					if (animancerState2 == null)
					{
						goto IL_98;
					}
					if (animancerState2.Parent == this)
					{
						if (animancerState2.Weight <= AnimancerLayer.WeightlessThreshold)
						{
							break;
						}
						if (num > animancerState2.Weight)
						{
							num = animancerState2.Weight;
							animancerState = animancerState2;
						}
					}
					else if (animancerState2.Parent == null)
					{
						goto Block_7;
					}
					num2++;
				}
				state = animancerState2;
				goto IL_154;
				Block_7:
				this.AddChild(animancerState2);
				goto IL_154;
				IL_98:
				if (state.Parent == this)
				{
					num = state.Weight;
					animancerState = state;
				}
				animancerState2 = state;
				object key;
				for (;;)
				{
					key = state;
					if (!base.Root.States.TryGet(key, out state))
					{
						break;
					}
					if (state.Parent == this)
					{
						if (state.Weight <= AnimancerLayer.WeightlessThreshold)
						{
							goto IL_154;
						}
						if (num > state.Weight)
						{
							num = state.Weight;
							animancerState = state;
						}
					}
					else if (state.Parent == null)
					{
						goto Block_15;
					}
					num2++;
				}
				if (num2 >= AnimancerLayer.MaxCloneCount && animancerState != null)
				{
					state = animancerState;
					goto IL_154;
				}
				state = animancerState2.Clone(base.Root);
				state.Weight = 0f;
				state._Key = key;
				base.Root.States.Register(state);
				this.AddChild(state);
				goto IL_154;
				Block_15:
				this.AddChild(state);
			}
			IL_154:
			state.TimeD = 0.0;
			return state;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00004E38 File Offset: 0x00003038
		public void DestroyStates()
		{
			for (int i = this.States.Count - 1; i >= 0; i--)
			{
				this.States[i].Destroy();
			}
			this.States.Clear();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00004E7C File Offset: 0x0000307C
		protected internal override void OnStartFade()
		{
			for (int i = this.States.Count - 1; i >= 0; i--)
			{
				this.States[i].OnStartFade();
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00004EB2 File Offset: 0x000030B2
		public AnimancerState Play(AnimationClip clip)
		{
			return this.Play(this.GetOrCreateState(clip, false));
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00004EC4 File Offset: 0x000030C4
		public AnimancerState Play(AnimancerState state)
		{
			if (base.Weight == 0f && base.TargetWeight == 0f)
			{
				base.Weight = 1f;
			}
			state = this.GetOrCreateState(state);
			this.CurrentState = state;
			state.Play();
			for (int i = this.States.Count - 1; i >= 0; i--)
			{
				AnimancerState animancerState = this.States[i];
				if (animancerState != state)
				{
					animancerState.Stop();
				}
			}
			return state;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00004F3C File Offset: 0x0000313C
		public AnimancerState Play(AnimationClip clip, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			return this.Play(this.GetOrCreateState(clip, false), fadeDuration, mode);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00004F50 File Offset: 0x00003150
		public AnimancerState Play(AnimancerState state, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			if (fadeDuration <= 0f || (base.Root.SkipFirstFade && base.Index == 0 && base.Weight == 0f))
			{
				base.Weight = 1f;
				state = this.Play(state);
				if (mode == FadeMode.FromStart || mode == FadeMode.NormalizedFromStart)
				{
					state.TimeD = 0.0;
				}
				return state;
			}
			float fadeDuration2;
			this.EvaluateFadeMode(mode, ref state, ref fadeDuration, out fadeDuration2);
			base.StartFade(1f, fadeDuration2);
			if (base.Weight == 0f)
			{
				return this.Play(state);
			}
			state = this.GetOrCreateState(state);
			this.CurrentState = state;
			if (state.IsPlaying && state.TargetWeight == 1f && (state.Weight == 1f || state.FadeSpeed * fadeDuration > Math.Abs(1f - state.Weight)))
			{
				this.OnStartFade();
			}
			else
			{
				state.IsPlaying = true;
				state.StartFade(1f, fadeDuration);
				for (int i = this.States.Count - 1; i >= 0; i--)
				{
					AnimancerState animancerState = this.States[i];
					if (animancerState != state)
					{
						animancerState.StartFade(0f, fadeDuration);
					}
				}
			}
			return state;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00005080 File Offset: 0x00003280
		public AnimancerState Play(ITransition transition)
		{
			return this.Play(transition, transition.FadeDuration, transition.FadeMode);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00005098 File Offset: 0x00003298
		public AnimancerState Play(ITransition transition, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			AnimancerState animancerState = this.GetOrCreateState(transition);
			animancerState = this.Play(animancerState, fadeDuration, mode);
			transition.Apply(animancerState);
			return animancerState;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000050C0 File Offset: 0x000032C0
		public AnimancerState TryPlay(object key)
		{
			AnimancerState state;
			if (!base.Root.States.TryGet(key, out state))
			{
				return null;
			}
			return this.Play(state);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000050EC File Offset: 0x000032EC
		public AnimancerState TryPlay(object key, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			AnimancerState state;
			if (!base.Root.States.TryGet(key, out state))
			{
				return null;
			}
			return this.Play(state, fadeDuration, mode);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000511C File Offset: 0x0000331C
		private void EvaluateFadeMode(FadeMode mode, ref AnimancerState state, ref float fadeDuration, out float layerFadeDuration)
		{
			layerFadeDuration = fadeDuration;
			switch (mode)
			{
			case FadeMode.FixedSpeed:
				fadeDuration *= Math.Abs(1f - state.Weight);
				layerFadeDuration *= Math.Abs(1f - base.Weight);
				return;
			case FadeMode.FixedDuration:
				return;
			case FadeMode.FromStart:
				state = this.GetOrCreateWeightlessState(state);
				return;
			case FadeMode.NormalizedSpeed:
			{
				float length = state.Length;
				fadeDuration *= Math.Abs(1f - state.Weight) * length;
				layerFadeDuration *= Math.Abs(1f - base.Weight) * length;
				return;
			}
			case FadeMode.NormalizedDuration:
			{
				float length2 = state.Length;
				fadeDuration *= length2;
				layerFadeDuration *= length2;
				return;
			}
			case FadeMode.NormalizedFromStart:
			{
				state = this.GetOrCreateWeightlessState(state);
				float length3 = state.Length;
				fadeDuration *= length3;
				layerFadeDuration *= length3;
				return;
			}
			default:
				throw AnimancerUtilities.CreateUnsupportedArgumentException<FadeMode>(mode);
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00005208 File Offset: 0x00003408
		public override void Stop()
		{
			base.Stop();
			this.CurrentState = null;
			for (int i = this.States.Count - 1; i >= 0; i--)
			{
				this.States[i].Stop();
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000524C File Offset: 0x0000344C
		public bool IsPlayingClip(AnimationClip clip)
		{
			for (int i = this.States.Count - 1; i >= 0; i--)
			{
				AnimancerState animancerState = this.States[i];
				if (animancerState.Clip == clip && animancerState.IsPlaying)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005298 File Offset: 0x00003498
		public bool IsAnyStatePlaying()
		{
			for (int i = this.States.Count - 1; i >= 0; i--)
			{
				if (this.States[i].IsPlaying)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000052D3 File Offset: 0x000034D3
		public override bool IsPlayingAndNotEnding()
		{
			return this._CurrentState != null && this._CurrentState.IsPlayingAndNotEnding();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000052EC File Offset: 0x000034EC
		public float GetTotalWeight()
		{
			float num = 0f;
			for (int i = this.States.Count - 1; i >= 0; i--)
			{
				num += this.States[i].Weight;
			}
			return num;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000532C File Offset: 0x0000352C
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00005334 File Offset: 0x00003534
		public override bool ApplyAnimatorIK
		{
			get
			{
				return this._ApplyAnimatorIK;
			}
			set
			{
				this._ApplyAnimatorIK = value;
				base.ApplyAnimatorIK = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00005351 File Offset: 0x00003551
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000535C File Offset: 0x0000355C
		public override bool ApplyFootIK
		{
			get
			{
				return this._ApplyFootIK;
			}
			set
			{
				this._ApplyFootIK = value;
				base.ApplyFootIK = value;
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005379 File Offset: 0x00003579
		public void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			clips.GatherFromSource(this.States);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00005388 File Offset: 0x00003588
		public override string ToString()
		{
			return "Layer " + base.Index.ToString();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000053B0 File Offset: 0x000035B0
		protected override void AppendDetails(StringBuilder text, string separator)
		{
			base.AppendDetails(text, separator);
			text.Append(separator).Append("CurrentState: ").Append(this.CurrentState);
			text.Append(separator).Append("CommandCount: ").Append(this.CommandCount);
			text.Append(separator).Append("IsAdditive: ").Append(this.IsAdditive);
		}

		// Token: 0x04000025 RID: 37
		private readonly List<AnimancerState> States = new List<AnimancerState>();

		// Token: 0x04000026 RID: 38
		private AnimancerState _CurrentState;

		// Token: 0x0400002A RID: 42
		private bool _ApplyAnimatorIK;

		// Token: 0x0400002B RID: 43
		private bool _ApplyFootIK;
	}
}
