using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000039 RID: 57
	public class ManualMixerState : AnimancerState, ICopyable<ManualMixerState>
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00009C0C File Offset: 0x00007E0C
		public override bool KeepChildrenConnected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00009C0F File Offset: 0x00007E0F
		public override AnimationClip Clip
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00009C12 File Offset: 0x00007E12
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00009C1A File Offset: 0x00007E1A
		private protected AnimancerState[] ChildStates { protected get; private set; } = Array.Empty<AnimancerState>();

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00009C23 File Offset: 0x00007E23
		public sealed override int ChildCount
		{
			get
			{
				return this._ChildCount;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00009C2B File Offset: 0x00007E2B
		// (set) Token: 0x0600039B RID: 923 RVA: 0x00009C38 File Offset: 0x00007E38
		public int ChildCapacity
		{
			get
			{
				return this.ChildStates.Length;
			}
			set
			{
				if (value == this.ChildStates.Length)
				{
					return;
				}
				AnimancerState[] array = new AnimancerState[value];
				if (value > this._ChildCount)
				{
					Array.Copy(this.ChildStates, array, this._ChildCount);
				}
				else
				{
					for (int i = value; i < this._ChildCount; i++)
					{
						this.ChildStates[i].Destroy();
					}
					Array.Copy(this.ChildStates, array, value);
					this._ChildCount = value;
				}
				this.ChildStates = array;
				if (this._Playable.IsValid<Playable>())
				{
					this._Playable.SetInputCount(value);
				}
				else if (base.Root != null)
				{
					this.CreatePlayable();
				}
				this.OnChildCapacityChanged();
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00009CDD File Offset: 0x00007EDD
		protected virtual void OnChildCapacityChanged()
		{
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00009CDF File Offset: 0x00007EDF
		// (set) Token: 0x0600039E RID: 926 RVA: 0x00009CE6 File Offset: 0x00007EE6
		public static int DefaultChildCapacity { get; set; } = 8;

		// Token: 0x0600039F RID: 927 RVA: 0x00009CF0 File Offset: 0x00007EF0
		public void EnsureRemainingChildCapacity(int minimumCapacity)
		{
			minimumCapacity += this._ChildCount;
			if (this.ChildCapacity < minimumCapacity)
			{
				int i;
				for (i = Math.Max(this.ChildCapacity, ManualMixerState.DefaultChildCapacity); i < minimumCapacity; i *= 2)
				{
				}
				this.ChildCapacity = i;
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00009D32 File Offset: 0x00007F32
		public sealed override AnimancerState GetChild(int index)
		{
			return this.ChildStates[index];
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00009D3C File Offset: 0x00007F3C
		public sealed override FastEnumerator<AnimancerState> GetEnumerator()
		{
			return new FastEnumerator<AnimancerState>(this.ChildStates, this._ChildCount);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00009D50 File Offset: 0x00007F50
		protected override void OnSetIsPlaying()
		{
			for (int i = this._ChildCount - 1; i >= 0; i--)
			{
				this.ChildStates[i].IsPlaying = base.IsPlaying;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00009D84 File Offset: 0x00007F84
		public override bool IsLooping
		{
			get
			{
				for (int i = this._ChildCount - 1; i >= 0; i--)
				{
					if (this.ChildStates[i].IsLooping)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00009DB8 File Offset: 0x00007FB8
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00009E00 File Offset: 0x00008000
		public override double RawTime
		{
			get
			{
				this.RecalculateWeights();
				float num;
				float num2;
				float num3;
				if (!this.GetSynchronizedTimeDetails(out num, out num2, out num3))
				{
					this.GetTimeDetails(out num, out num2, out num3);
				}
				if (num == 0f)
				{
					return base.RawTime;
				}
				num *= num;
				return (double)(num2 * num3 / num);
			}
			set
			{
				if (value != 0.0)
				{
					float length = this.Length;
					if (length != 0f)
					{
						value /= (double)length;
						for (int i = this._ChildCount - 1; i >= 0; i--)
						{
							this.ChildStates[i].NormalizedTimeD = value;
						}
						return;
					}
				}
				for (int j = this._ChildCount - 1; j >= 0; j--)
				{
					this.ChildStates[j].TimeD = 0.0;
				}
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00009E7C File Offset: 0x0000807C
		public override void MoveTime(double time, bool normalized)
		{
			base.MoveTime(time, normalized);
			for (int i = this._ChildCount - 1; i >= 0; i--)
			{
				this.ChildStates[i].MoveTime(time, normalized);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00009EB4 File Offset: 0x000080B4
		private bool GetSynchronizedTimeDetails(out float totalWeight, out float normalizedTime, out float length)
		{
			totalWeight = 0f;
			normalizedTime = 0f;
			length = 0f;
			if (this._SynchronizedChildren != null)
			{
				for (int i = this._SynchronizedChildren.Count - 1; i >= 0; i--)
				{
					AnimancerState animancerState = this._SynchronizedChildren[i];
					float weight = animancerState.Weight;
					if (weight != 0f)
					{
						float length2 = animancerState.Length;
						if (length2 != 0f)
						{
							totalWeight += weight;
							normalizedTime += animancerState.Time / length2 * weight;
							length += length2 * weight;
						}
					}
				}
			}
			return totalWeight > ManualMixerState.MinimumSynchronizeChildrenWeight;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00009F48 File Offset: 0x00008148
		private void GetTimeDetails(out float totalWeight, out float normalizedTime, out float length)
		{
			totalWeight = 0f;
			normalizedTime = 0f;
			length = 0f;
			for (int i = this._ChildCount - 1; i >= 0; i--)
			{
				AnimancerState animancerState = this.ChildStates[i];
				float weight = animancerState.Weight;
				if (weight != 0f)
				{
					float length2 = animancerState.Length;
					if (length2 != 0f)
					{
						totalWeight += weight;
						normalizedTime += animancerState.Time / length2 * weight;
						length += length2 * weight;
					}
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00009FC4 File Offset: 0x000081C4
		public override float Length
		{
			get
			{
				this.RecalculateWeights();
				float num = 0f;
				float num2 = 0f;
				if (this._SynchronizedChildren != null)
				{
					for (int i = this._SynchronizedChildren.Count - 1; i >= 0; i--)
					{
						AnimancerState animancerState = this._SynchronizedChildren[i];
						float weight = animancerState.Weight;
						if (weight != 0f)
						{
							float length = animancerState.Length;
							if (length != 0f)
							{
								num2 += weight;
								num += length * weight;
							}
						}
					}
				}
				if (num2 > 0f)
				{
					return num / num2;
				}
				num2 = ManualMixerState.CalculateTotalWeight(this.ChildStates, this._ChildCount);
				if (num2 <= 0f)
				{
					return 0f;
				}
				for (int j = this._ChildCount - 1; j >= 0; j--)
				{
					AnimancerState animancerState2 = this.ChildStates[j];
					num += animancerState2.Length * animancerState2.Weight;
				}
				return num / num2;
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000A0A3 File Offset: 0x000082A3
		protected override void CreatePlayable(out Playable playable)
		{
			playable = AnimationMixerPlayable.Create(base.Root._Graph, this.ChildCapacity);
			this.RecalculateWeights();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000A0D0 File Offset: 0x000082D0
		protected internal override void OnAddChild(AnimancerState state)
		{
			if (state.Index != this._ChildCount)
			{
				throw new ArgumentException("Mixer child index out of order. Mixer children must be added in sequence starting from 0 to ensure that they contain no nulls.");
			}
			int childCapacity = this.ChildCapacity;
			if (this._ChildCount >= childCapacity)
			{
				this.ChildCapacity = Math.Max(ManualMixerState.DefaultChildCapacity, childCapacity * 2);
			}
			base.OnAddChild(this.ChildStates, state);
			this._ChildCount++;
			if (ManualMixerState.SynchronizeNewChildren)
			{
				this.Synchronize(state);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000A144 File Offset: 0x00008344
		protected internal override void OnRemoveChild(AnimancerState state)
		{
			this.DontSynchronize(state);
			if (base.Root == null)
			{
				Array.Copy(this.ChildStates, state.Index + 1, this.ChildStates, state.Index, this._ChildCount - state.Index - 1);
				for (int i = state.Index; i < this._ChildCount - 1; i++)
				{
					this.ChildStates[i].Index = i;
				}
			}
			else
			{
				base.Root._Graph.Disconnect<Playable>(this._Playable, state.Index);
				for (int j = state.Index + 1; j < this._ChildCount; j++)
				{
					AnimancerState animancerState = this.ChildStates[j];
					base.Root._Graph.Disconnect<Playable>(this._Playable, animancerState.Index);
					animancerState.Index = j - 1;
					this.ChildStates[j - 1] = animancerState;
					animancerState.ConnectToGraph();
				}
			}
			this._ChildCount--;
			this.ChildStates[this._ChildCount] = null;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000A245 File Offset: 0x00008445
		public override void Destroy()
		{
			this.DestroyChildren();
			base.Destroy();
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000A253 File Offset: 0x00008453
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			ManualMixerState manualMixerState = new ManualMixerState();
			manualMixerState.SetNewCloneRoot(root);
			((ICopyable<ManualMixerState>)manualMixerState).CopyFrom(this);
			return manualMixerState;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000A268 File Offset: 0x00008468
		void ICopyable<ManualMixerState>.CopyFrom(ManualMixerState copyFrom)
		{
			((ICopyable<AnimancerState>)this).CopyFrom(copyFrom);
			this.DestroyChildren();
			bool synchronizeNewChildren = ManualMixerState.SynchronizeNewChildren;
			int childCount = copyFrom.ChildCount;
			this.EnsureRemainingChildCapacity(childCount);
			for (int i = 0; i < childCount; i++)
			{
				AnimancerState animancerState = copyFrom.ChildStates[i];
				ManualMixerState.SynchronizeNewChildren = copyFrom.IsSynchronized(animancerState);
				animancerState = animancerState.Clone(base.Root);
				this.Add(animancerState);
			}
			ManualMixerState.SynchronizeNewChildren = synchronizeNewChildren;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000A2D1 File Offset: 0x000084D1
		public void Add(AnimancerState state)
		{
			state.SetParent(this, this._ChildCount);
			state.IsPlaying = base.IsPlaying;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000A2EC File Offset: 0x000084EC
		public ClipState Add(AnimationClip clip)
		{
			ClipState clipState = new ClipState(clip);
			this.Add(clipState);
			return clipState;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000A308 File Offset: 0x00008508
		public AnimancerState Add(Animancer.ITransition transition)
		{
			AnimancerState animancerState = transition.CreateStateAndApply(base.Root);
			this.Add(animancerState);
			return animancerState;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000A32C File Offset: 0x0000852C
		public AnimancerState Add(object child)
		{
			AnimationClip animationClip = child as AnimationClip;
			if (animationClip != null)
			{
				return this.Add(animationClip);
			}
			Animancer.ITransition transition = child as Animancer.ITransition;
			if (transition != null)
			{
				return this.Add(transition);
			}
			AnimancerState animancerState = child as AnimancerState;
			if (animancerState != null)
			{
				this.Add(animancerState);
				return animancerState;
			}
			throw new ArgumentException(string.Concat(new string[]
			{
				"Failed to Add '",
				AnimancerUtilities.ToStringOrNull(child),
				"'",
				string.Format(" as child of '{0}' because it isn't an", this),
				" AnimationClip, ITransition, or AnimancerState."
			}));
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000A3B0 File Offset: 0x000085B0
		public void AddRange(IList<AnimationClip> clips)
		{
			int count = clips.Count;
			this.EnsureRemainingChildCapacity(count);
			for (int i = 0; i < count; i++)
			{
				this.Add(clips[i]);
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000A3E5 File Offset: 0x000085E5
		public void AddRange(params AnimationClip[] clips)
		{
			this.AddRange(clips);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000A3F0 File Offset: 0x000085F0
		public void AddRange(IList<Animancer.ITransition> transitions)
		{
			int count = transitions.Count;
			this.EnsureRemainingChildCapacity(count);
			for (int i = 0; i < count; i++)
			{
				this.Add(transitions[i]);
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000A425 File Offset: 0x00008625
		public void AddRange(params Animancer.ITransition[] clips)
		{
			this.AddRange(clips);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000A430 File Offset: 0x00008630
		public void AddRange(IList<object> children)
		{
			int count = children.Count;
			this.EnsureRemainingChildCapacity(count);
			for (int i = 0; i < count; i++)
			{
				this.Add(children[i]);
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000A465 File Offset: 0x00008665
		public void AddRange(params object[] clips)
		{
			this.AddRange(clips);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000A46E File Offset: 0x0000866E
		public void Remove(int index, bool destroy)
		{
			this.Remove(this.ChildStates[index], destroy);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000A47F File Offset: 0x0000867F
		public void Remove(AnimancerState child, bool destroy)
		{
			if (destroy)
			{
				child.Destroy();
				return;
			}
			child.SetParent(null, -1);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000A494 File Offset: 0x00008694
		public void Set(int index, AnimancerState child, bool destroyPrevious)
		{
			child.SetParent(null, -1);
			AnimancerState animancerState = this.ChildStates[index];
			this.DontSynchronize(animancerState);
			animancerState.SetParentInternal(null, -1);
			child.SetRoot(base.Root);
			this.ChildStates[index] = child;
			child.SetParentInternal(this, index);
			if (base.Root != null)
			{
				base.Root._Graph.Disconnect<Playable>(this._Playable, child.Index);
				child.ConnectToGraph();
			}
			child.CopyIKFlags(this);
			if (ManualMixerState.SynchronizeNewChildren)
			{
				this.Synchronize(child);
			}
			if (destroyPrevious)
			{
				animancerState.Destroy();
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000A528 File Offset: 0x00008728
		public ClipState Set(int index, AnimationClip clip, bool destroyPrevious)
		{
			ClipState clipState = new ClipState(clip);
			this.Set(index, clipState, destroyPrevious);
			return clipState;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000A548 File Offset: 0x00008748
		public AnimancerState Set(int index, Animancer.ITransition transition, bool destroyPrevious)
		{
			AnimancerState animancerState = transition.CreateStateAndApply(base.Root);
			this.Set(index, animancerState, destroyPrevious);
			return animancerState;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000A56C File Offset: 0x0000876C
		public AnimancerState Set(int index, object child, bool destroyPrevious)
		{
			AnimationClip animationClip = child as AnimationClip;
			if (animationClip != null)
			{
				return this.Set(index, animationClip, destroyPrevious);
			}
			Animancer.ITransition transition = child as Animancer.ITransition;
			if (transition != null)
			{
				return this.Set(index, transition, destroyPrevious);
			}
			AnimancerState animancerState = child as AnimancerState;
			if (animancerState != null)
			{
				this.Set(index, animancerState, destroyPrevious);
				return animancerState;
			}
			throw new ArgumentException(string.Concat(new string[]
			{
				"Failed to Set '",
				AnimancerUtilities.ToStringOrNull(child),
				"'",
				string.Format(" as child of '{0}' because it isn't an", this),
				" AnimationClip, ITransition, or AnimancerState."
			}));
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000A5F5 File Offset: 0x000087F5
		public int IndexOf(AnimancerState child)
		{
			return Array.IndexOf<AnimancerState>(this.ChildStates, child, 0, this._ChildCount);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000A60C File Offset: 0x0000880C
		public void DestroyChildren()
		{
			for (int i = this._ChildCount - 1; i >= 0; i--)
			{
				this.ChildStates[i].Destroy();
			}
			Array.Clear(this.ChildStates, 0, this._ChildCount);
			this._ChildCount = 0;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000A654 File Offset: 0x00008854
		public AnimationScriptPlayable CreatePlayable<T>(AnimancerPlayable root, T job, bool processInputs = false) where T : struct, IAnimationJob
		{
			base.SetRoot(null);
			base.Root = root;
			root.States.Register(this);
			AnimationScriptPlayable result = AnimationScriptPlayable.Create<T>(root._Graph, job, this._ChildCount);
			if (!processInputs)
			{
				result.SetProcessInputs(false);
			}
			for (int i = this._ChildCount - 1; i >= 0; i--)
			{
				this.ChildStates[i].SetRoot(root);
			}
			return result;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000A6BC File Offset: 0x000088BC
		protected void CreatePlayable<T>(out Playable playable, T job, bool processInputs = false) where T : struct, IAnimationJob
		{
			AnimationScriptPlayable playable2 = AnimationScriptPlayable.Create<T>(base.Root._Graph, job, this.ChildCount);
			if (!processInputs)
			{
				playable2.SetProcessInputs(false);
			}
			playable = playable2;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000A6F8 File Offset: 0x000088F8
		public T GetJobData<T>() where T : struct, IAnimationJob
		{
			return ((AnimationScriptPlayable)this._Playable).GetJobData<T>();
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000A718 File Offset: 0x00008918
		public void SetJobData<T>(T value) where T : struct, IAnimationJob
		{
			((AnimationScriptPlayable)this._Playable).SetJobData<T>(value);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000A73C File Offset: 0x0000893C
		protected internal override void Update(out bool needsMoreUpdates)
		{
			base.Update(out needsMoreUpdates);
			if (this.RecalculateWeights())
			{
				for (int i = this._ChildCount - 1; i >= 0; i--)
				{
					this.ChildStates[i].ApplyWeight();
				}
			}
			this.ApplySynchronizeChildren(ref needsMoreUpdates);
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0000A77F File Offset: 0x0000897F
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x0000A787 File Offset: 0x00008987
		public bool WeightsAreDirty { get; set; }

		// Token: 0x060003C9 RID: 969 RVA: 0x0000A790 File Offset: 0x00008990
		public bool RecalculateWeights()
		{
			if (!this.WeightsAreDirty)
			{
				return false;
			}
			this.ForceRecalculateWeights();
			return true;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000A7A3 File Offset: 0x000089A3
		protected virtual void ForceRecalculateWeights()
		{
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0000A7A5 File Offset: 0x000089A5
		// (set) Token: 0x060003CC RID: 972 RVA: 0x0000A7AC File Offset: 0x000089AC
		public static bool SynchronizeNewChildren { get; set; } = true;

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0000A7B4 File Offset: 0x000089B4
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0000A7BB File Offset: 0x000089BB
		public static float MinimumSynchronizeChildrenWeight { get; set; } = 0.01f;

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000A7C3 File Offset: 0x000089C3
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0000A7E0 File Offset: 0x000089E0
		public AnimancerState[] SynchronizedChildren
		{
			get
			{
				if (this.SynchronizedChildCount <= 0)
				{
					return Array.Empty<AnimancerState>();
				}
				return this._SynchronizedChildren.ToArray();
			}
			set
			{
				if (this._SynchronizedChildren == null)
				{
					this._SynchronizedChildren = new List<AnimancerState>();
				}
				else
				{
					this._SynchronizedChildren.Clear();
				}
				for (int i = 0; i < value.Length; i++)
				{
					this.Synchronize(value[i]);
				}
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000A824 File Offset: 0x00008A24
		public int SynchronizedChildCount
		{
			get
			{
				if (this._SynchronizedChildren == null)
				{
					return 0;
				}
				return this._SynchronizedChildren.Count;
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000A83C File Offset: 0x00008A3C
		public bool IsSynchronized(AnimancerState state)
		{
			ManualMixerState parentMixer = this.GetParentMixer();
			return parentMixer._SynchronizedChildren != null && parentMixer._SynchronizedChildren.Contains(state);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000A866 File Offset: 0x00008A66
		public void Synchronize(AnimancerState state)
		{
			if (state == null)
			{
				return;
			}
			this.GetParentMixer().SynchronizeDirect(state);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000A878 File Offset: 0x00008A78
		private void SynchronizeDirect(AnimancerState state)
		{
			if (state == null)
			{
				return;
			}
			ManualMixerState manualMixerState = state as ManualMixerState;
			if (manualMixerState != null)
			{
				if (manualMixerState._SynchronizedChildren != null)
				{
					for (int i = 0; i < manualMixerState._SynchronizedChildren.Count; i++)
					{
						this.Synchronize(manualMixerState._SynchronizedChildren[i]);
					}
					manualMixerState._SynchronizedChildren.Clear();
				}
				return;
			}
			if (this._SynchronizedChildren == null)
			{
				this._SynchronizedChildren = new List<AnimancerState>();
			}
			this._SynchronizedChildren.Add(state);
			base.RequireUpdate();
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		public void DontSynchronize(AnimancerState state)
		{
			ManualMixerState parentMixer = this.GetParentMixer();
			if (parentMixer._SynchronizedChildren != null && parentMixer._SynchronizedChildren.Remove(state) && state._Playable.IsValid<Playable>())
			{
				state._Playable.SetSpeed((double)state.Speed);
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000A940 File Offset: 0x00008B40
		public void DontSynchronizeChildren()
		{
			ManualMixerState parentMixer = this.GetParentMixer();
			List<AnimancerState> synchronizedChildren = parentMixer._SynchronizedChildren;
			if (synchronizedChildren == null)
			{
				return;
			}
			if (parentMixer == this)
			{
				for (int i = synchronizedChildren.Count - 1; i >= 0; i--)
				{
					AnimancerState animancerState = synchronizedChildren[i];
					if (animancerState._Playable.IsValid<Playable>())
					{
						animancerState._Playable.SetSpeed((double)animancerState.Speed);
					}
				}
				synchronizedChildren.Clear();
				return;
			}
			for (int j = synchronizedChildren.Count - 1; j >= 0; j--)
			{
				AnimancerState animancerState2 = synchronizedChildren[j];
				if (ManualMixerState.IsChildOf(animancerState2, this))
				{
					if (animancerState2._Playable.IsValid<Playable>())
					{
						animancerState2._Playable.SetSpeed((double)animancerState2.Speed);
					}
					synchronizedChildren.RemoveAt(j);
				}
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000A9FC File Offset: 0x00008BFC
		public void InitializeSynchronizedChildren(params bool[] synchronizeChildren)
		{
			int num;
			if (synchronizeChildren != null)
			{
				num = synchronizeChildren.Length;
				for (int i = 0; i < num; i++)
				{
					if (synchronizeChildren[i])
					{
						this.SynchronizeDirect(this.ChildStates[i]);
					}
				}
			}
			else
			{
				num = 0;
			}
			for (int j = num; j < this._ChildCount; j++)
			{
				this.SynchronizeDirect(this.ChildStates[j]);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000AA54 File Offset: 0x00008C54
		public ManualMixerState GetParentMixer()
		{
			ManualMixerState result = this;
			for (IPlayableWrapper parent = this.Parent; parent != null; parent = parent.Parent)
			{
				ManualMixerState manualMixerState = parent as ManualMixerState;
				if (manualMixerState != null)
				{
					result = manualMixerState;
				}
			}
			return result;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000AA84 File Offset: 0x00008C84
		public static ManualMixerState GetParentMixer(IPlayableWrapper node)
		{
			ManualMixerState result = null;
			while (node != null)
			{
				ManualMixerState manualMixerState = node as ManualMixerState;
				if (manualMixerState != null)
				{
					result = manualMixerState;
				}
				node = node.Parent;
			}
			return result;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000AAAD File Offset: 0x00008CAD
		public static bool IsChildOf(IPlayableWrapper child, IPlayableWrapper parent)
		{
			for (;;)
			{
				child = child.Parent;
				if (child == parent)
				{
					break;
				}
				if (child == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		protected void ApplySynchronizeChildren(ref bool needsMoreUpdates)
		{
			if (base.Weight == 0f || !base.IsPlaying || this._SynchronizedChildren == null || this._SynchronizedChildren.Count <= 1)
			{
				return;
			}
			needsMoreUpdates = true;
			float num = AnimancerPlayable.DeltaTime * this.CalculateRealEffectiveSpeed();
			if (num == 0f)
			{
				return;
			}
			int count = this._SynchronizedChildren.Count;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < count; i++)
			{
				AnimancerState animancerState = this._SynchronizedChildren[i];
				float num5 = animancerState.Weight;
				if (num5 != 0f)
				{
					float length = animancerState.Length;
					if (length != 0f)
					{
						num2 += num5;
						num5 /= length;
						num3 += animancerState.Time * num5;
						num4 += animancerState.Speed * num5;
					}
				}
			}
			if (num2 < ManualMixerState.MinimumSynchronizeChildrenWeight)
			{
				num3 = 0f;
				num4 = 0f;
				int num6 = 0;
				for (int j = 0; j < count; j++)
				{
					AnimancerState animancerState2 = this._SynchronizedChildren[j];
					float num7 = animancerState2.Length;
					if (num7 != 0f)
					{
						num7 = 1f / num7;
						num3 += animancerState2.Time * num7;
						num4 += animancerState2.Speed * num7;
						num6++;
					}
				}
				num2 = (float)num6;
			}
			num3 += num * num4;
			num3 /= num2;
			float num8 = 1f / num;
			for (int k = 0; k < count; k++)
			{
				AnimancerState animancerState3 = this._SynchronizedChildren[k];
				float length2 = animancerState3.Length;
				if (length2 != 0f)
				{
					float num9 = animancerState3.Time / length2;
					float num10 = (num3 - num9) * length2 * num8;
					animancerState3._Playable.SetSpeed((double)num10);
				}
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000AC88 File Offset: 0x00008E88
		public float CalculateRealEffectiveSpeed()
		{
			double num = this._Playable.GetSpeed<Playable>();
			for (IPlayableWrapper parent = this.Parent; parent != null; parent = parent.Parent)
			{
				num *= parent.Playable.GetSpeed<Playable>();
			}
			return (float)num;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0000ACC4 File Offset: 0x00008EC4
		// (set) Token: 0x060003DE RID: 990 RVA: 0x0000ACCC File Offset: 0x00008ECC
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

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000ACE9 File Offset: 0x00008EE9
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0000ACF4 File Offset: 0x00008EF4
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

		// Token: 0x060003E1 RID: 993 RVA: 0x0000AD14 File Offset: 0x00008F14
		public static float CalculateTotalWeight(AnimancerState[] states, int count)
		{
			float num = 0f;
			for (int i = count - 1; i >= 0; i--)
			{
				num += states[i].Weight;
			}
			return num;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000AD44 File Offset: 0x00008F44
		public void SetChildrenTime(float value, bool normalized = false)
		{
			for (int i = this._ChildCount - 1; i >= 0; i--)
			{
				AnimancerState animancerState = this.ChildStates[i];
				if (normalized)
				{
					animancerState.NormalizedTime = value;
				}
				else
				{
					animancerState.Time = value;
				}
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000AD80 File Offset: 0x00008F80
		protected void DisableRemainingStates(int previousIndex)
		{
			for (int i = previousIndex + 1; i < this._ChildCount; i++)
			{
				this.ChildStates[i].Weight = 0f;
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000ADB4 File Offset: 0x00008FB4
		public void NormalizeWeights(float totalWeight)
		{
			if (totalWeight == 1f)
			{
				return;
			}
			totalWeight = 1f / totalWeight;
			for (int i = this._ChildCount - 1; i >= 0; i--)
			{
				this.ChildStates[i].Weight *= totalWeight;
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000ADFB File Offset: 0x00008FFB
		public virtual string GetDisplayKey(AnimancerState state)
		{
			return string.Format("[{0}]", state.Index);
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0000AE14 File Offset: 0x00009014
		public override Vector3 AverageVelocity
		{
			get
			{
				Vector3 vector = default(Vector3);
				this.RecalculateWeights();
				for (int i = this._ChildCount - 1; i >= 0; i--)
				{
					AnimancerState animancerState = this.ChildStates[i];
					vector += animancerState.AverageVelocity * animancerState.Weight;
				}
				return vector;
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000AE68 File Offset: 0x00009068
		public void NormalizeDurations()
		{
			int num = 0;
			float num2 = 0f;
			for (int i = 0; i < this._ChildCount; i++)
			{
				num++;
				num2 += this.ChildStates[i].Duration;
			}
			num2 /= (float)num;
			for (int j = 0; j < this._ChildCount; j++)
			{
				this.ChildStates[j].Duration = num2;
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000AEC8 File Offset: 0x000090C8
		public override string ToString()
		{
			List<string> list = ObjectPool.AcquireList<string>();
			bool flag = true;
			for (int i = 0; i < this._ChildCount; i++)
			{
				AnimancerState animancerState = this.ChildStates[i];
				if (animancerState != null)
				{
					if (animancerState.MainObject != null)
					{
						list.Add(animancerState.MainObject.name);
					}
					else
					{
						list.Add(animancerState.ToString());
						flag = false;
					}
				}
			}
			int count = list.Count;
			int num;
			if (count <= 1 || !flag)
			{
				num = 0;
			}
			else
			{
				string text = list[0];
				int length;
				num = (length = text.Length);
				for (int j = 0; j < count; j++)
				{
					string text2 = list[j];
					if (length > text2.Length)
					{
						num = (length = text2.Length);
					}
					for (int k = 0; k < num; k++)
					{
						if (text2[k] != text[k])
						{
							num = k;
							break;
						}
					}
				}
				if (num < 3 || num >= length)
				{
					num = 0;
				}
			}
			StringBuilder stringBuilder = ObjectPool.AcquireStringBuilder();
			if (count > 0)
			{
				if (num > 0)
				{
					stringBuilder.Append(list[0], 0, num).Append('[');
				}
				for (int l = 0; l < count; l++)
				{
					if (l > 0)
					{
						stringBuilder.Append(", ");
					}
					string text3 = list[l];
					stringBuilder.Append(text3, num, text3.Length - num);
				}
				stringBuilder.Append((num > 0) ? "] (" : " (");
			}
			ObjectPool.Release<string>(list);
			string fullName = base.GetType().FullName;
			if (fullName.EndsWith("State"))
			{
				stringBuilder.Append(fullName, 0, fullName.Length - 5);
			}
			else
			{
				stringBuilder.Append(fullName);
			}
			if (count > 0)
			{
				stringBuilder.Append(')');
			}
			return stringBuilder.ReleaseToString();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000B094 File Offset: 0x00009294
		protected override void AppendDetails(StringBuilder text, string separator)
		{
			base.AppendDetails(text, separator);
			text.Append(separator).Append("SynchronizedChildren: ");
			if (this.SynchronizedChildCount == 0)
			{
				text.Append("0");
				return;
			}
			text.Append(this._SynchronizedChildren.Count);
			separator += "    ";
			for (int i = 0; i < this._SynchronizedChildren.Count; i++)
			{
				text.Append(separator).Append(this._SynchronizedChildren[i]);
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000B11E File Offset: 0x0000931E
		public override void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			clips.GatherFromSource(this.ChildStates);
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0000B12C File Offset: 0x0000932C
		protected virtual int ParameterCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000B12F File Offset: 0x0000932F
		protected virtual string GetParameterName(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000B136 File Offset: 0x00009336
		protected virtual AnimatorControllerParameterType GetParameterType(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000B13D File Offset: 0x0000933D
		protected virtual object GetParameterValue(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000B144 File Offset: 0x00009344
		protected virtual void SetParameterValue(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400009C RID: 156
		private int _ChildCount;

		// Token: 0x040000A1 RID: 161
		private List<AnimancerState> _SynchronizedChildren;

		// Token: 0x040000A2 RID: 162
		private bool _ApplyAnimatorIK;

		// Token: 0x040000A3 RID: 163
		private bool _ApplyFootIK;

		// Token: 0x02000091 RID: 145
		public interface ITransition : ITransition<ManualMixerState>, Animancer.ITransition, IHasKey, IPolymorphic
		{
		}

		// Token: 0x02000092 RID: 146
		public interface ITransition2D : ITransition<MixerState<Vector2>>, Animancer.ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
