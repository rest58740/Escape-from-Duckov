using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000015 RID: 21
	public abstract class AnimancerState : AnimancerNode, IAnimationClipCollection, ICopyable<AnimancerState>
	{
		// Token: 0x0600025F RID: 607 RVA: 0x00006BD8 File Offset: 0x00004DD8
		public void SetRoot(AnimancerPlayable root)
		{
			if (base.Root == root)
			{
				return;
			}
			if (base.Root != null)
			{
				base.Root.CancelPreUpdate(this);
				base.Root.States.Unregister(this);
				if (this._EventDispatcher != null)
				{
					base.Root.CancelPostUpdate(this._EventDispatcher);
				}
				if (this._Parent != null && this._Parent.Root != root)
				{
					this._Parent.OnRemoveChild(this);
					this._Parent = null;
					base.Index = -1;
				}
				base.DestroyPlayable();
			}
			base.Root = root;
			if (root != null)
			{
				root.States.Register(this);
				if (this._EventDispatcher != null)
				{
					root.RequirePostUpdate(this._EventDispatcher);
				}
				this.CreatePlayable();
			}
			for (int i = this.ChildCount - 1; i >= 0; i--)
			{
				AnimancerState child = this.GetChild(i);
				if (child != null)
				{
					child.SetRoot(root);
				}
			}
			if (this._Parent != null)
			{
				this.CopyIKFlags(this._Parent);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00006CCD File Offset: 0x00004ECD
		public sealed override IPlayableWrapper Parent
		{
			get
			{
				return this._Parent;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00006CD8 File Offset: 0x00004ED8
		public void SetParent(AnimancerNode parent, int index)
		{
			if (this._Parent != null)
			{
				this._Parent.OnRemoveChild(this);
				this._Parent = null;
			}
			if (parent == null)
			{
				base.Index = -1;
				return;
			}
			this.SetRoot(parent.Root);
			base.Index = index;
			this._Parent = parent;
			parent.OnAddChild(this);
			this.CopyIKFlags(parent);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00006D33 File Offset: 0x00004F33
		internal void SetParentInternal(AnimancerNode parent, int index = -1)
		{
			this._Parent = parent;
			base.Index = index;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00006D43 File Offset: 0x00004F43
		public override AnimancerLayer Layer
		{
			get
			{
				AnimancerNode parent = this._Parent;
				if (parent == null)
				{
					return null;
				}
				return parent.Layer;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00006D58 File Offset: 0x00004F58
		// (set) Token: 0x06000265 RID: 613 RVA: 0x00006D86 File Offset: 0x00004F86
		public int LayerIndex
		{
			get
			{
				if (this._Parent == null)
				{
					return -1;
				}
				AnimancerLayer layer = this._Parent.Layer;
				if (layer == null)
				{
					return -1;
				}
				return layer.Index;
			}
			set
			{
				base.Root.Layers[value].AddChild(this);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00006D9F File Offset: 0x00004F9F
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00006DA7 File Offset: 0x00004FA7
		public object Key
		{
			get
			{
				return this._Key;
			}
			set
			{
				if (base.Root == null)
				{
					this._Key = value;
					return;
				}
				base.Root.States.Unregister(this);
				this._Key = value;
				base.Root.States.Register(this);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00006DE2 File Offset: 0x00004FE2
		// (set) Token: 0x06000269 RID: 617 RVA: 0x00006DE5 File Offset: 0x00004FE5
		public virtual AnimationClip Clip
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException(string.Format("{0} does not support setting the {1}.", base.GetType(), "Clip"));
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00006E01 File Offset: 0x00005001
		// (set) Token: 0x0600026B RID: 619 RVA: 0x00006E04 File Offset: 0x00005004
		public virtual UnityEngine.Object MainObject
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException(string.Format("{0} does not support setting the {1}.", base.GetType(), "MainObject"));
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00006E20 File Offset: 0x00005020
		protected void ChangeMainObject<T>(ref T currentObject, T newObject) where T : UnityEngine.Object
		{
			if (newObject == null)
			{
				throw new ArgumentNullException("newObject");
			}
			if (currentObject == newObject)
			{
				return;
			}
			if (this._Key == currentObject)
			{
				this.Key = newObject;
			}
			currentObject = newObject;
			this.RecreatePlayable();
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00006E88 File Offset: 0x00005088
		public virtual Vector3 AverageVelocity
		{
			get
			{
				return default(Vector3);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00006E9E File Offset: 0x0000509E
		// (set) Token: 0x0600026F RID: 623 RVA: 0x00006EA6 File Offset: 0x000050A6
		public bool IsPlaying
		{
			get
			{
				return this._IsPlaying;
			}
			set
			{
				if (this._IsPlaying == value)
				{
					return;
				}
				this._IsPlaying = value;
				if (this._IsPlayingDirty)
				{
					this._IsPlayingDirty = false;
				}
				else
				{
					this._IsPlayingDirty = true;
					base.RequireUpdate();
				}
				this.OnSetIsPlaying();
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00006EDD File Offset: 0x000050DD
		protected virtual void OnSetIsPlaying()
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00006EDF File Offset: 0x000050DF
		public sealed override void CreatePlayable()
		{
			base.CreatePlayable();
			if (this._MustSetTime)
			{
				this._MustSetTime = false;
				this.RawTime = this._Time;
			}
			if (!this._IsPlaying)
			{
				this._Playable.Pause<Playable>();
			}
			this._IsPlayingDirty = false;
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000272 RID: 626 RVA: 0x00006F1C File Offset: 0x0000511C
		public bool IsActive
		{
			get
			{
				return this._IsPlaying && base.TargetWeight > 0f;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00006F35 File Offset: 0x00005135
		public bool IsStopped
		{
			get
			{
				return !this._IsPlaying && base.Weight == 0f;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00006F4E File Offset: 0x0000514E
		public void Play()
		{
			this.IsPlaying = true;
			base.Weight = 1f;
			if (AnimancerState.AutomaticallyClearEvents)
			{
				AnimancerState.EventDispatcher.TryClear(this._EventDispatcher);
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00006F74 File Offset: 0x00005174
		public override void Stop()
		{
			base.Stop();
			this.IsPlaying = false;
			this.TimeD = 0.0;
			if (AnimancerState.AutomaticallyClearEvents)
			{
				AnimancerState.EventDispatcher.TryClear(this._EventDispatcher);
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00006FA4 File Offset: 0x000051A4
		protected internal override void OnStartFade()
		{
			if (AnimancerState.AutomaticallyClearEvents)
			{
				AnimancerState.EventDispatcher.TryClear(this._EventDispatcher);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00006FB8 File Offset: 0x000051B8
		// (set) Token: 0x06000278 RID: 632 RVA: 0x00006FC1 File Offset: 0x000051C1
		public float Time
		{
			get
			{
				return (float)this.TimeD;
			}
			set
			{
				this.TimeD = (double)value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00006FCC File Offset: 0x000051CC
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000701C File Offset: 0x0000521C
		public double TimeD
		{
			get
			{
				AnimancerPlayable root = base.Root;
				if (root == null || this._MustSetTime)
				{
					return this._Time;
				}
				ulong frameID = root.FrameID;
				if (this._TimeFrameID != frameID)
				{
					this._TimeFrameID = frameID;
					this._Time = this.RawTime;
				}
				return this._Time;
			}
			set
			{
				this._Time = value;
				AnimancerPlayable root = base.Root;
				if (root == null)
				{
					this._MustSetTime = true;
				}
				else
				{
					this._TimeFrameID = root.FrameID;
					if (AnimancerPlayable.IsRunningPostUpdate(root))
					{
						this._MustSetTime = true;
						root.RequirePreUpdate(this);
					}
					else
					{
						this.RawTime = value;
					}
				}
				AnimancerState.EventDispatcher eventDispatcher = this._EventDispatcher;
				if (eventDispatcher == null)
				{
					return;
				}
				eventDispatcher.OnTimeChanged();
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000707E File Offset: 0x0000527E
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000708C File Offset: 0x0000528C
		public virtual double RawTime
		{
			get
			{
				return this._Playable.GetTime<Playable>();
			}
			set
			{
				this._Playable.SetTime(value);
				this._Playable.SetTime(value);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600027D RID: 637 RVA: 0x000070B3 File Offset: 0x000052B3
		// (set) Token: 0x0600027E RID: 638 RVA: 0x000070BC File Offset: 0x000052BC
		public float NormalizedTime
		{
			get
			{
				return (float)this.NormalizedTimeD;
			}
			set
			{
				this.NormalizedTimeD = (double)value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600027F RID: 639 RVA: 0x000070C6 File Offset: 0x000052C6
		// (set) Token: 0x06000280 RID: 640 RVA: 0x000070ED File Offset: 0x000052ED
		public double NormalizedTimeD
		{
			get
			{
				if (this.Length != 0f)
				{
					return this.TimeD / (double)this.Length;
				}
				return 0.0;
			}
			set
			{
				this.TimeD = value * (double)this.Length;
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x000070FE File Offset: 0x000052FE
		public void MoveTime(float time, bool normalized)
		{
			this.MoveTime((double)time, normalized);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000710C File Offset: 0x0000530C
		public virtual void MoveTime(double time, bool normalized)
		{
			AnimancerPlayable root = base.Root;
			if (root != null)
			{
				this._TimeFrameID = root.FrameID;
			}
			if (normalized)
			{
				time *= (double)this.Length;
			}
			this._Time = time;
			this._Playable.SetTime(time);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00007150 File Offset: 0x00005350
		protected void CancelSetTime()
		{
			this._MustSetTime = false;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000715C File Offset: 0x0000535C
		// (set) Token: 0x06000285 RID: 645 RVA: 0x00007197 File Offset: 0x00005397
		public float NormalizedEndTime
		{
			get
			{
				if (this._EventDispatcher != null)
				{
					float normalizedEndTime = this._EventDispatcher.Events.NormalizedEndTime;
					if (!float.IsNaN(normalizedEndTime))
					{
						return normalizedEndTime;
					}
				}
				return AnimancerEvent.Sequence.GetDefaultNormalizedEndTime(base.EffectiveSpeed);
			}
			set
			{
				this.Events.NormalizedEndTime = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000286 RID: 646 RVA: 0x000071A8 File Offset: 0x000053A8
		// (set) Token: 0x06000287 RID: 647 RVA: 0x00007210 File Offset: 0x00005410
		public float Duration
		{
			get
			{
				float effectiveSpeed = base.EffectiveSpeed;
				if (this._EventDispatcher != null)
				{
					float normalizedEndTime = this._EventDispatcher.Events.NormalizedEndTime;
					if (!float.IsNaN(normalizedEndTime))
					{
						if (effectiveSpeed > 0f)
						{
							return this.Length * normalizedEndTime / effectiveSpeed;
						}
						return this.Length * (1f - normalizedEndTime) / -effectiveSpeed;
					}
				}
				return this.Length / Math.Abs(effectiveSpeed);
			}
			set
			{
				float num = this.Length;
				if (this._EventDispatcher != null)
				{
					float normalizedEndTime = this._EventDispatcher.Events.NormalizedEndTime;
					if (!float.IsNaN(normalizedEndTime))
					{
						if (base.EffectiveSpeed > 0f)
						{
							num *= normalizedEndTime;
						}
						else
						{
							num *= 1f - normalizedEndTime;
						}
					}
				}
				base.EffectiveSpeed = num / value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000726B File Offset: 0x0000546B
		// (set) Token: 0x06000289 RID: 649 RVA: 0x00007288 File Offset: 0x00005488
		public float RemainingDuration
		{
			get
			{
				return (this.Length * this.NormalizedEndTime - this.Time) / base.EffectiveSpeed;
			}
			set
			{
				base.EffectiveSpeed = (this.Length * this.NormalizedEndTime - this.Time) / value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600028A RID: 650
		public abstract float Length { get; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600028B RID: 651 RVA: 0x000072A6 File Offset: 0x000054A6
		public virtual bool IsLooping
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000072AC File Offset: 0x000054AC
		protected internal override void Update(out bool needsMoreUpdates)
		{
			base.Update(out needsMoreUpdates);
			if (this._IsPlayingDirty)
			{
				this._IsPlayingDirty = false;
				if (this._IsPlaying)
				{
					this._Playable.Play<Playable>();
				}
				else
				{
					this._Playable.Pause<Playable>();
				}
			}
			if (this._MustSetTime)
			{
				this._MustSetTime = false;
				this.RawTime = this._Time;
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000730C File Offset: 0x0000550C
		public virtual void Destroy()
		{
			if (this._Parent != null)
			{
				this._Parent.OnRemoveChild(this);
				this._Parent = null;
			}
			base.Index = -1;
			AnimancerState.EventDispatcher.TryClear(this._EventDispatcher);
			AnimancerPlayable root = base.Root;
			if (root != null)
			{
				root.States.Unregister(this);
				if (this._Playable.IsValid<Playable>())
				{
					root._Graph.DestroyPlayable<Playable>(this._Playable);
				}
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000737A File Offset: 0x0000557A
		public AnimancerState Clone()
		{
			return this.Clone(base.Root);
		}

		// Token: 0x0600028F RID: 655
		public abstract AnimancerState Clone(AnimancerPlayable root);

		// Token: 0x06000290 RID: 656 RVA: 0x00007388 File Offset: 0x00005588
		protected void SetNewCloneRoot(AnimancerPlayable root)
		{
			if (root == null)
			{
				return;
			}
			base.Root = root;
			this.CreatePlayable();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000739B File Offset: 0x0000559B
		void ICopyable<AnimancerState>.CopyFrom(AnimancerState copyFrom)
		{
			this.Events = (copyFrom.HasEvents ? copyFrom.Events : null);
			this.TimeD = copyFrom.TimeD;
			((ICopyable<AnimancerNode>)this).CopyFrom(copyFrom);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000073C8 File Offset: 0x000055C8
		public virtual void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			clips.Gather(this.Clip);
			for (int i = this.ChildCount - 1; i >= 0; i--)
			{
				this.GetChild(i).GatherAnimationClips(clips);
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00007404 File Offset: 0x00005604
		public override bool IsPlayingAndNotEnding()
		{
			if (!this.IsPlaying || !this._Playable.IsValid<Playable>())
			{
				return false;
			}
			float effectiveSpeed = base.EffectiveSpeed;
			if (effectiveSpeed > 0f)
			{
				float num;
				if (this._EventDispatcher != null)
				{
					num = this._EventDispatcher.Events.NormalizedEndTime;
					if (float.IsNaN(num))
					{
						num = this.Length;
					}
					else
					{
						num *= this.Length;
					}
				}
				else
				{
					num = this.Length;
				}
				return this.Time <= num;
			}
			if (effectiveSpeed < 0f)
			{
				float num2;
				if (this._EventDispatcher != null)
				{
					num2 = this._EventDispatcher.Events.NormalizedEndTime;
					if (float.IsNaN(num2))
					{
						num2 = 0f;
					}
					else
					{
						num2 *= this.Length;
					}
				}
				else
				{
					num2 = 0f;
				}
				return this.Time >= num2;
			}
			return true;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000074D0 File Offset: 0x000056D0
		public override string ToString()
		{
			string name = base.GetType().Name;
			UnityEngine.Object mainObject = this.MainObject;
			if (mainObject != null)
			{
				return mainObject.name + " (" + name + ")";
			}
			return name;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00007514 File Offset: 0x00005714
		protected override void AppendDetails(StringBuilder text, string separator)
		{
			text.Append(separator).Append("Key: ").Append(AnimancerUtilities.ToStringOrNull(this._Key));
			UnityEngine.Object mainObject = this.MainObject;
			if (mainObject != this._Key as UnityEngine.Object)
			{
				text.Append(separator).Append("MainObject: ").Append(AnimancerUtilities.ToStringOrNull(mainObject));
			}
			base.AppendDetails(text, separator);
			text.Append(separator).Append("IsPlaying: ").Append(this.IsPlaying);
			try
			{
				text.Append(separator).Append("Time (Normalized): ").Append(this.Time);
				text.Append(" (").Append(this.NormalizedTime).Append(')');
				text.Append(separator).Append("Length: ").Append(this.Length);
				text.Append(separator).Append("IsLooping: ").Append(this.IsLooping);
			}
			catch (Exception value)
			{
				text.Append(separator).Append(value);
			}
			text.Append(separator).Append("Events: ");
			if (this._EventDispatcher != null && this._EventDispatcher.Events != null)
			{
				text.Append(this._EventDispatcher.Events.DeepToString(false));
				return;
			}
			text.Append("null");
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00007684 File Offset: 0x00005884
		public string GetPath()
		{
			if (this._Parent == null)
			{
				return null;
			}
			StringBuilder stringBuilder = ObjectPool.AcquireStringBuilder();
			AnimancerState.AppendPath(stringBuilder, this._Parent);
			this.AppendPortAndType(stringBuilder);
			return stringBuilder.ReleaseToString();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000076BC File Offset: 0x000058BC
		private static void AppendPath(StringBuilder path, AnimancerNode parent)
		{
			AnimancerState animancerState = parent as AnimancerState;
			if (animancerState == null || animancerState._Parent == null)
			{
				path.Append("Layers[").Append(parent.Layer.Index).Append("].States");
				return;
			}
			AnimancerState.AppendPath(path, animancerState._Parent);
			AnimancerState animancerState2 = parent as AnimancerState;
			if (animancerState2 != null)
			{
				animancerState2.AppendPortAndType(path);
				return;
			}
			path.Append(" -> ").Append(parent.GetType());
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00007739 File Offset: 0x00005939
		private void AppendPortAndType(StringBuilder path)
		{
			path.Append('[').Append(base.Index).Append("] -> ").Append(base.GetType().Name);
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00007769 File Offset: 0x00005969
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000777C File Offset: 0x0000597C
		public AnimancerEvent.Sequence Events
		{
			get
			{
				AnimancerState.EventDispatcher.Acquire(this);
				return this._EventDispatcher.Events;
			}
			set
			{
				if (value != null)
				{
					AnimancerState.EventDispatcher.Acquire(this);
					this._EventDispatcher.Events = value;
					return;
				}
				if (this._EventDispatcher != null)
				{
					this._EventDispatcher.Events = null;
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600029B RID: 667 RVA: 0x000077A8 File Offset: 0x000059A8
		public bool HasEvents
		{
			get
			{
				return this._EventDispatcher != null && this._EventDispatcher.HasEvents;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600029C RID: 668 RVA: 0x000077BF File Offset: 0x000059BF
		// (set) Token: 0x0600029D RID: 669 RVA: 0x000077C6 File Offset: 0x000059C6
		public static bool AutomaticallyClearEvents { get; set; } = true;

		// Token: 0x0400004D RID: 77
		private AnimancerNode _Parent;

		// Token: 0x0400004E RID: 78
		internal object _Key;

		// Token: 0x0400004F RID: 79
		private bool _IsPlaying;

		// Token: 0x04000050 RID: 80
		private bool _IsPlayingDirty = true;

		// Token: 0x04000051 RID: 81
		private double _Time;

		// Token: 0x04000052 RID: 82
		private bool _MustSetTime;

		// Token: 0x04000053 RID: 83
		private ulong _TimeFrameID;

		// Token: 0x04000054 RID: 84
		private AnimancerState.EventDispatcher _EventDispatcher;

		// Token: 0x02000089 RID: 137
		public class DelayedPause : Key, IUpdatable, Key.IListItem
		{
			// Token: 0x1700018D RID: 397
			// (get) Token: 0x0600064B RID: 1611 RVA: 0x00010C23 File Offset: 0x0000EE23
			// (set) Token: 0x0600064C RID: 1612 RVA: 0x00010C2B File Offset: 0x0000EE2B
			public AnimancerPlayable Root { get; set; }

			// Token: 0x1700018E RID: 398
			// (get) Token: 0x0600064D RID: 1613 RVA: 0x00010C34 File Offset: 0x0000EE34
			// (set) Token: 0x0600064E RID: 1614 RVA: 0x00010C3C File Offset: 0x0000EE3C
			public AnimancerState State { get; set; }

			// Token: 0x0600064F RID: 1615 RVA: 0x00010C48 File Offset: 0x0000EE48
			public static void Register(AnimancerState state)
			{
				AnimancerPlayable root = state.Root;
				if (root == null)
				{
					return;
				}
				AnimancerState.DelayedPause delayedPause = ObjectPool.Acquire<AnimancerState.DelayedPause>();
				delayedPause.Root = root;
				delayedPause.State = state;
				root.RequirePostUpdate(delayedPause);
			}

			// Token: 0x06000650 RID: 1616 RVA: 0x00010C7B File Offset: 0x0000EE7B
			public void Update()
			{
				if (!this.State.IsPlaying)
				{
					this.State._Playable.Pause<Playable>();
				}
				this.Root.CancelPostUpdate(this);
				this.Root = null;
				this.State = null;
				ObjectPool.Release<AnimancerState.DelayedPause>(this);
			}
		}

		// Token: 0x0200008A RID: 138
		public class EventDispatcher : Key, IUpdatable, Key.IListItem
		{
			// Token: 0x06000652 RID: 1618 RVA: 0x00010CC4 File Offset: 0x0000EEC4
			internal static void Acquire(AnimancerState state)
			{
				ref AnimancerState.EventDispatcher ptr = ref state._EventDispatcher;
				if (ptr != null)
				{
					return;
				}
				ObjectPool.Acquire<AnimancerState.EventDispatcher>(out ptr);
				ptr._IsLooping = state.IsLooping;
				ptr._PreviousTime = state.NormalizedTime;
				ptr._State = state;
				AnimancerPlayable root = state.Root;
				if (root == null)
				{
					return;
				}
				root.RequirePostUpdate(ptr);
			}

			// Token: 0x06000653 RID: 1619 RVA: 0x00010D18 File Offset: 0x0000EF18
			private void Release()
			{
				if (this._State == null)
				{
					return;
				}
				AnimancerPlayable root = this._State.Root;
				if (root != null)
				{
					root.CancelPostUpdate(this);
				}
				this._State._EventDispatcher = null;
				this._State = null;
				this.Events = null;
				ObjectPool.Release<AnimancerState.EventDispatcher>(this);
			}

			// Token: 0x06000654 RID: 1620 RVA: 0x00010D65 File Offset: 0x0000EF65
			internal static void TryClear(AnimancerState.EventDispatcher events)
			{
				if (events != null)
				{
					events.Events = null;
				}
			}

			// Token: 0x1700018F RID: 399
			// (get) Token: 0x06000655 RID: 1621 RVA: 0x00010D71 File Offset: 0x0000EF71
			public bool HasEvents
			{
				get
				{
					return this._Events != null;
				}
			}

			// Token: 0x17000190 RID: 400
			// (get) Token: 0x06000656 RID: 1622 RVA: 0x00010D7C File Offset: 0x0000EF7C
			// (set) Token: 0x06000657 RID: 1623 RVA: 0x00010D9E File Offset: 0x0000EF9E
			internal AnimancerEvent.Sequence Events
			{
				get
				{
					if (this._Events == null)
					{
						ObjectPool.Acquire<AnimancerEvent.Sequence>(out this._Events);
						this._GotEventsFromPool = true;
					}
					return this._Events;
				}
				set
				{
					if (this._GotEventsFromPool)
					{
						this._Events.Clear();
						ObjectPool.Release<AnimancerEvent.Sequence>(this._Events);
						this._GotEventsFromPool = false;
					}
					this._Events = value;
					this._NextEventIndex = int.MinValue;
				}
			}

			// Token: 0x06000658 RID: 1624 RVA: 0x00010DD8 File Offset: 0x0000EFD8
			void IUpdatable.Update()
			{
				if (this._Events == null || this._Events.IsEmpty)
				{
					this.Release();
					return;
				}
				float length = this._State.Length;
				if (length == 0f)
				{
					this.UpdateZeroLength();
					return;
				}
				float num = this._State.Time / length;
				if (this._PreviousTime == num)
				{
					return;
				}
				this.CheckGeneralEvents(num);
				if (this._Events == null)
				{
					this.Release();
					return;
				}
				AnimancerEvent endEvent = this._Events.EndEvent;
				if (endEvent.callback != null)
				{
					if (num > this._PreviousTime)
					{
						float num2 = float.IsNaN(endEvent.normalizedTime) ? 1f : endEvent.normalizedTime;
						if (num > num2)
						{
							endEvent.Invoke(this._State);
						}
					}
					else
					{
						float num3 = float.IsNaN(endEvent.normalizedTime) ? 0f : endEvent.normalizedTime;
						if (num < num3)
						{
							endEvent.Invoke(this._State);
						}
					}
				}
				if (this._NextEventIndex != -2147483648)
				{
					this._PreviousTime = num;
				}
			}

			// Token: 0x06000659 RID: 1625 RVA: 0x00010ED7 File Offset: 0x0000F0D7
			[Conditional("UNITY_ASSERTIONS")]
			private void ValidateBeforeEndEvent()
			{
			}

			// Token: 0x0600065A RID: 1626 RVA: 0x00010ED9 File Offset: 0x0000F0D9
			[Conditional("UNITY_ASSERTIONS")]
			private void ValidateAfterEndEvent(Action callback)
			{
			}

			// Token: 0x0600065B RID: 1627 RVA: 0x00010EDB File Offset: 0x0000F0DB
			internal void OnTimeChanged()
			{
				this._PreviousTime = this._State.NormalizedTime;
				this._NextEventIndex = int.MinValue;
			}

			// Token: 0x0600065C RID: 1628 RVA: 0x00010EFC File Offset: 0x0000F0FC
			private void UpdateZeroLength()
			{
				float effectiveSpeed = this._State.EffectiveSpeed;
				if (effectiveSpeed == 0f)
				{
					return;
				}
				if (this._Events.Count > 0)
				{
					int version = this._Events.Version;
					int playDirectionInt;
					if (effectiveSpeed < 0f)
					{
						playDirectionInt = -1;
						if (this._NextEventIndex == -2147483648 || this._SequenceVersion != version || this._WasPlayingForwards)
						{
							this._NextEventIndex = this.Events.Count - 1;
							this._SequenceVersion = version;
							this._WasPlayingForwards = false;
						}
					}
					else
					{
						playDirectionInt = 1;
						if (this._NextEventIndex == -2147483648 || this._SequenceVersion != version || !this._WasPlayingForwards)
						{
							this._NextEventIndex = 0;
							this._SequenceVersion = version;
							this._WasPlayingForwards = true;
						}
					}
					if (!this.InvokeAllEvents(1, playDirectionInt))
					{
						return;
					}
				}
				AnimancerEvent endEvent = this._Events.EndEvent;
				if (endEvent.callback != null)
				{
					endEvent.Invoke(this._State);
				}
			}

			// Token: 0x0600065D RID: 1629 RVA: 0x00010FE8 File Offset: 0x0000F1E8
			private void CheckGeneralEvents(float currentTime)
			{
				int count = this._Events.Count;
				if (count == 0)
				{
					this._NextEventIndex = 0;
					return;
				}
				float num;
				int playDirectionInt;
				this.ValidateNextEventIndex(ref currentTime, out num, out playDirectionInt);
				if (this._IsLooping)
				{
					AnimancerEvent animancerEvent = this._Events[this._NextEventIndex];
					float eventTime = animancerEvent.normalizedTime * num;
					int loopDelta = AnimancerState.EventDispatcher.GetLoopDelta(this._PreviousTime, currentTime, eventTime);
					if (loopDelta == 0)
					{
						return;
					}
					if (!this.InvokeAllEvents(loopDelta - 1, playDirectionInt))
					{
						return;
					}
					int nextEventIndex = this._NextEventIndex;
					for (;;)
					{
						animancerEvent.Invoke(this._State);
						if (!this.NextEventLooped(playDirectionInt) || this._NextEventIndex == nextEventIndex)
						{
							break;
						}
						animancerEvent = this._Events[this._NextEventIndex];
						eventTime = animancerEvent.normalizedTime * num;
						if (loopDelta != AnimancerState.EventDispatcher.GetLoopDelta(this._PreviousTime, currentTime, eventTime))
						{
							return;
						}
					}
					return;
				}
				else
				{
					while (this._NextEventIndex < count)
					{
						AnimancerEvent animancerEvent2 = this._Events[this._NextEventIndex];
						float num2 = animancerEvent2.normalizedTime * num;
						if (currentTime <= num2)
						{
							return;
						}
						animancerEvent2.Invoke(this._State);
						if (!this.NextEvent(playDirectionInt))
						{
							return;
						}
					}
				}
			}

			// Token: 0x0600065E RID: 1630 RVA: 0x00011100 File Offset: 0x0000F300
			private void ValidateNextEventIndex(ref float currentTime, out float playDirectionFloat, out int playDirectionInt)
			{
				int version = this._Events.Version;
				if (currentTime < this._PreviousTime)
				{
					float num = this._PreviousTime;
					this._PreviousTime = -num;
					currentTime = -currentTime;
					playDirectionFloat = -1f;
					playDirectionInt = -1;
					if (this._NextEventIndex == -2147483648 || this._SequenceVersion != version || this._WasPlayingForwards)
					{
						this._NextEventIndex = this._Events.Count - 1;
						this._SequenceVersion = version;
						this._WasPlayingForwards = false;
						if (this._IsLooping)
						{
							num = AnimancerUtilities.Wrap01(num);
						}
						while (this._Events[this._NextEventIndex].normalizedTime > num)
						{
							this._NextEventIndex--;
							if (this._NextEventIndex < 0)
							{
								if (this._IsLooping)
								{
									this._NextEventIndex = this._Events.Count - 1;
									return;
								}
								return;
							}
						}
						return;
					}
				}
				else
				{
					playDirectionFloat = 1f;
					playDirectionInt = 1;
					if (this._NextEventIndex == -2147483648 || this._SequenceVersion != version || !this._WasPlayingForwards)
					{
						this._NextEventIndex = 0;
						this._SequenceVersion = version;
						this._WasPlayingForwards = true;
						float num2 = this._PreviousTime;
						if (this._IsLooping)
						{
							num2 = AnimancerUtilities.Wrap01(num2);
						}
						int num3 = this._Events.Count - 1;
						while (this._Events[this._NextEventIndex].normalizedTime < num2)
						{
							this._NextEventIndex++;
							if (this._NextEventIndex > num3)
							{
								if (this._IsLooping)
								{
									this._NextEventIndex = 0;
									return;
								}
								break;
							}
						}
					}
				}
			}

			// Token: 0x0600065F RID: 1631 RVA: 0x0001128C File Offset: 0x0000F48C
			private static int GetLoopDelta(float previousTime, float nextTime, float eventTime)
			{
				previousTime -= eventTime;
				nextTime -= eventTime;
				int num = Mathf.FloorToInt(previousTime);
				int num2 = Mathf.FloorToInt(nextTime);
				int num3 = num2 - num;
				if (previousTime == (float)num)
				{
					num3++;
				}
				if (nextTime == (float)num2)
				{
					num3--;
				}
				return num3;
			}

			// Token: 0x06000660 RID: 1632 RVA: 0x000112C8 File Offset: 0x0000F4C8
			private bool InvokeAllEvents(int count, int playDirectionInt)
			{
				int nextEventIndex = this._NextEventIndex;
				IL_3C:
				while (count-- > 0)
				{
					for (;;)
					{
						this._Events[this._NextEventIndex].Invoke(this._State);
						if (!this.NextEventLooped(playDirectionInt))
						{
							break;
						}
						if (this._NextEventIndex == nextEventIndex)
						{
							goto IL_3C;
						}
					}
					return false;
				}
				return true;
			}

			// Token: 0x06000661 RID: 1633 RVA: 0x0001131B File Offset: 0x0000F51B
			private bool NextEvent(int playDirectionInt)
			{
				if (this._NextEventIndex == -2147483648)
				{
					return false;
				}
				if (this._Events.Version != this._SequenceVersion)
				{
					throw new InvalidOperationException("AnimancerState.Events sequence was modified while iterating through it. Events in a sequence must not modify that sequence.");
				}
				this._NextEventIndex += playDirectionInt;
				return true;
			}

			// Token: 0x06000662 RID: 1634 RVA: 0x0001135C File Offset: 0x0000F55C
			private bool NextEventLooped(int playDirectionInt)
			{
				if (!this.NextEvent(playDirectionInt))
				{
					return false;
				}
				int count = this._Events.Count;
				if (this._NextEventIndex >= count)
				{
					this._NextEventIndex = 0;
				}
				else if (this._NextEventIndex < 0)
				{
					this._NextEventIndex = count - 1;
				}
				return true;
			}

			// Token: 0x06000663 RID: 1635 RVA: 0x000113A5 File Offset: 0x0000F5A5
			public override string ToString()
			{
				if (this._State == null)
				{
					return "EventDispatcher (No Target State)";
				}
				return string.Format("{0} ({1})", "EventDispatcher", this._State);
			}

			// Token: 0x04000130 RID: 304
			private AnimancerState _State;

			// Token: 0x04000131 RID: 305
			private AnimancerEvent.Sequence _Events;

			// Token: 0x04000132 RID: 306
			private bool _GotEventsFromPool;

			// Token: 0x04000133 RID: 307
			private bool _IsLooping;

			// Token: 0x04000134 RID: 308
			private float _PreviousTime;

			// Token: 0x04000135 RID: 309
			private int _NextEventIndex = int.MinValue;

			// Token: 0x04000136 RID: 310
			private int _SequenceVersion;

			// Token: 0x04000137 RID: 311
			private bool _WasPlayingForwards;

			// Token: 0x04000138 RID: 312
			private const int RecalculateEventIndex = -2147483648;

			// Token: 0x04000139 RID: 313
			private const string SequenceVersionException = "AnimancerState.Events sequence was modified while iterating through it. Events in a sequence must not modify that sequence.";
		}
	}
}
