using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000013 RID: 19
	public abstract class AnimancerNode : Key, IUpdatable, Key.IListItem, IEnumerable<AnimancerState>, IEnumerable, IEnumerator, IPlayableWrapper, ICopyable<AnimancerNode>
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000542E File Offset: 0x0000362E
		public Playable Playable
		{
			get
			{
				return this._Playable;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00005436 File Offset: 0x00003636
		public bool IsValid
		{
			get
			{
				return this._Playable.IsValid<Playable>();
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005444 File Offset: 0x00003644
		public virtual void CreatePlayable()
		{
			this.CreatePlayable(out this._Playable);
			if (this._Speed != 1f)
			{
				this._Playable.SetSpeed((double)this._Speed);
			}
			IPlayableWrapper parent = this.Parent;
			if (parent != null)
			{
				this.ApplyConnectedState(parent);
			}
		}

		// Token: 0x060001B9 RID: 441
		protected abstract void CreatePlayable(out Playable playable);

		// Token: 0x060001BA RID: 442 RVA: 0x0000548D File Offset: 0x0000368D
		public void DestroyPlayable()
		{
			if (this._Playable.IsValid<Playable>())
			{
				this.Root._Graph.DestroyPlayable<Playable>(this._Playable);
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000054B2 File Offset: 0x000036B2
		public virtual void RecreatePlayable()
		{
			this.DestroyPlayable();
			this.CreatePlayable();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000054C0 File Offset: 0x000036C0
		public void RecreatePlayableRecursive()
		{
			this.RecreatePlayable();
			for (int i = this.ChildCount - 1; i >= 0; i--)
			{
				AnimancerState child = this.GetChild(i);
				if (child != null)
				{
					child.RecreatePlayableRecursive();
				}
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000054F8 File Offset: 0x000036F8
		void ICopyable<AnimancerNode>.CopyFrom(AnimancerNode copyFrom)
		{
			this._Weight = copyFrom._Weight;
			this._IsWeightDirty = true;
			this.TargetWeight = copyFrom.TargetWeight;
			this.FadeSpeed = copyFrom.FadeSpeed;
			this.Speed = copyFrom.Speed;
			this.CopyIKFlags(copyFrom);
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00005538 File Offset: 0x00003738
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00005540 File Offset: 0x00003740
		public AnimancerPlayable Root
		{
			get
			{
				return this._Root;
			}
			internal set
			{
				this._Root = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001C0 RID: 448
		public abstract AnimancerLayer Layer { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001C1 RID: 449
		public abstract IPlayableWrapper Parent { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00005549 File Offset: 0x00003749
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00005551 File Offset: 0x00003751
		public int Index { get; internal set; } = int.MinValue;

		// Token: 0x060001C5 RID: 453 RVA: 0x00005580 File Offset: 0x00003780
		internal void ConnectToGraph()
		{
			IPlayableWrapper parent = this.Parent;
			if (parent == null)
			{
				return;
			}
			Playable playable = parent.Playable;
			this.Root._Graph.Connect<Playable, Playable>(this._Playable, 0, playable, this.Index);
			playable.SetInputWeight(this.Index, this._Weight);
			this._IsWeightDirty = false;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000055D8 File Offset: 0x000037D8
		internal void DisconnectFromGraph()
		{
			IPlayableWrapper parent = this.Parent;
			if (parent == null)
			{
				return;
			}
			Playable playable = parent.Playable;
			if (playable.GetInput(this.Index).IsValid<Playable>())
			{
				this.Root._Graph.Disconnect<Playable>(playable, this.Index);
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00005621 File Offset: 0x00003821
		private void ApplyConnectedState(IPlayableWrapper parent)
		{
			this._IsWeightDirty = true;
			if (this._Weight != 0f || parent.KeepChildrenConnected)
			{
				this.ConnectToGraph();
				return;
			}
			this.Root.RequirePreUpdate(this);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00005652 File Offset: 0x00003852
		protected void RequireUpdate()
		{
			AnimancerPlayable root = this.Root;
			if (root == null)
			{
				return;
			}
			root.RequirePreUpdate(this);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00005668 File Offset: 0x00003868
		void IUpdatable.Update()
		{
			if (this._Playable.IsValid<Playable>())
			{
				bool flag;
				this.Update(out flag);
				if (flag)
				{
					return;
				}
			}
			this.Root.CancelPreUpdate(this);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000569A File Offset: 0x0000389A
		protected internal virtual void Update(out bool needsMoreUpdates)
		{
			this.UpdateFade(out needsMoreUpdates);
			this.ApplyWeight();
		}

		// Token: 0x060001CB RID: 459
		public abstract bool IsPlayingAndNotEnding();

		// Token: 0x060001CC RID: 460 RVA: 0x000056A9 File Offset: 0x000038A9
		bool IEnumerator.MoveNext()
		{
			return this.IsPlayingAndNotEnding();
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000056B1 File Offset: 0x000038B1
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000056B4 File Offset: 0x000038B4
		void IEnumerator.Reset()
		{
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001CF RID: 463 RVA: 0x000056B6 File Offset: 0x000038B6
		public virtual int ChildCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000056B9 File Offset: 0x000038B9
		AnimancerNode IPlayableWrapper.GetChild(int index)
		{
			return this.GetChild(index);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000056C2 File Offset: 0x000038C2
		public virtual AnimancerState GetChild(int index)
		{
			throw new NotSupportedException(((this != null) ? this.ToString() : null) + " can't have children.");
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000056E0 File Offset: 0x000038E0
		protected internal virtual void OnAddChild(AnimancerState state)
		{
			state.SetParentInternal(null, -1);
			throw new NotSupportedException(((this != null) ? this.ToString() : null) + " can't have children.");
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00005706 File Offset: 0x00003906
		protected internal virtual void OnRemoveChild(AnimancerState state)
		{
			state.SetParentInternal(null, -1);
			throw new NotSupportedException(((this != null) ? this.ToString() : null) + " can't have children.");
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000572C File Offset: 0x0000392C
		protected void OnAddChild(IList<AnimancerState> states, AnimancerState state)
		{
			int index = state.Index;
			if (states[index] != null)
			{
				state.SetParentInternal(null, -1);
				throw new InvalidOperationException(string.Format("Tried to add a state to an already occupied port on {0}:", this) + string.Format("\n• {0}: {1}", "Index", index) + string.Format("\n• Old State: {0} ", states[index]) + string.Format("\n• New State: {0}", state));
			}
			states[index] = state;
			if (this.Root != null)
			{
				state.ApplyConnectedState(this);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x000057AF File Offset: 0x000039AF
		public virtual bool KeepChildrenConnected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000057B4 File Offset: 0x000039B4
		internal void ConnectAllChildrenToGraph()
		{
			if (!this.Parent.Playable.GetInput(this.Index).IsValid<Playable>())
			{
				this.ConnectToGraph();
			}
			for (int i = this.ChildCount - 1; i >= 0; i--)
			{
				AnimancerState child = this.GetChild(i);
				if (child != null)
				{
					child.ConnectAllChildrenToGraph();
				}
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000580C File Offset: 0x00003A0C
		internal void DisconnectWeightlessChildrenFromGraph()
		{
			if (this.Weight == 0f)
			{
				this.DisconnectFromGraph();
			}
			for (int i = this.ChildCount - 1; i >= 0; i--)
			{
				AnimancerState child = this.GetChild(i);
				if (child != null)
				{
					child.DisconnectWeightlessChildrenFromGraph();
				}
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00005854 File Offset: 0x00003A54
		public virtual FastEnumerator<AnimancerState> GetEnumerator()
		{
			return default(FastEnumerator<AnimancerState>);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000586A File Offset: 0x00003A6A
		IEnumerator<AnimancerState> IEnumerable<AnimancerState>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00005877 File Offset: 0x00003A77
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00005884 File Offset: 0x00003A84
		// (set) Token: 0x060001DC RID: 476 RVA: 0x0000588C File Offset: 0x00003A8C
		public float Weight
		{
			get
			{
				return this._Weight;
			}
			set
			{
				this.SetWeight(value);
				this.TargetWeight = value;
				this.FadeSpeed = 0f;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000058A7 File Offset: 0x00003AA7
		public void SetWeight(float value)
		{
			if (this._Weight == value)
			{
				return;
			}
			this._Weight = value;
			this.SetWeightDirty();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000058C0 File Offset: 0x00003AC0
		protected internal void SetWeightDirty()
		{
			this._IsWeightDirty = true;
			this.RequireUpdate();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000058D0 File Offset: 0x00003AD0
		public void ApplyWeight()
		{
			if (!this._IsWeightDirty)
			{
				return;
			}
			this._IsWeightDirty = false;
			IPlayableWrapper parent = this.Parent;
			if (parent == null)
			{
				return;
			}
			Playable playable;
			if (!parent.KeepChildrenConnected)
			{
				if (this._Weight == 0f)
				{
					this.DisconnectFromGraph();
					return;
				}
				playable = parent.Playable;
				if (!playable.GetInput(this.Index).IsValid<Playable>())
				{
					this.ConnectToGraph();
				}
			}
			else
			{
				playable = parent.Playable;
			}
			playable.SetInputWeight(this.Index, this._Weight);
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00005950 File Offset: 0x00003B50
		public float EffectiveWeight
		{
			get
			{
				float num = this.Weight;
				for (IPlayableWrapper parent = this.Parent; parent != null; parent = parent.Parent)
				{
					num *= parent.Weight;
				}
				return num;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00005981 File Offset: 0x00003B81
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00005989 File Offset: 0x00003B89
		public float TargetWeight { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00005992 File Offset: 0x00003B92
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000599A File Offset: 0x00003B9A
		public float FadeSpeed { get; set; }

		// Token: 0x060001E5 RID: 485 RVA: 0x000059A3 File Offset: 0x00003BA3
		public void StartFade(float targetWeight)
		{
			this.StartFade(targetWeight, AnimancerPlayable.DefaultFadeDuration);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000059B4 File Offset: 0x00003BB4
		public void StartFade(float targetWeight, float fadeDuration)
		{
			this.TargetWeight = targetWeight;
			if (targetWeight != this.Weight)
			{
				if (fadeDuration <= 0f)
				{
					this.FadeSpeed = float.PositiveInfinity;
				}
				else
				{
					this.FadeSpeed = Math.Abs(this.Weight - targetWeight) / fadeDuration;
				}
				this.OnStartFade();
				this.RequireUpdate();
				return;
			}
			if (targetWeight == 0f)
			{
				this.Stop();
				return;
			}
			this.FadeSpeed = 0f;
			this.OnStartFade();
		}

		// Token: 0x060001E7 RID: 487
		protected internal abstract void OnStartFade();

		// Token: 0x060001E8 RID: 488 RVA: 0x00005A28 File Offset: 0x00003C28
		public virtual void Stop()
		{
			this.Weight = 0f;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00005A38 File Offset: 0x00003C38
		private void UpdateFade(out bool needsMoreUpdates)
		{
			float num = this.FadeSpeed;
			if (num == 0f)
			{
				needsMoreUpdates = false;
				return;
			}
			this._IsWeightDirty = true;
			num *= this.ParentEffectiveSpeed * AnimancerPlayable.DeltaTime;
			if (num < 0f)
			{
				num = -num;
			}
			float targetWeight = this.TargetWeight;
			float weight = this._Weight;
			float num2 = targetWeight - weight;
			if (num2 > 0f)
			{
				if (num2 > num)
				{
					this._Weight = weight + num;
					needsMoreUpdates = true;
					return;
				}
			}
			else if (-num2 > num)
			{
				this._Weight = weight - num;
				needsMoreUpdates = true;
				return;
			}
			this._Weight = targetWeight;
			needsMoreUpdates = false;
			if (targetWeight == 0f)
			{
				this.Stop();
				return;
			}
			this.FadeSpeed = 0f;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00005ADA File Offset: 0x00003CDA
		// (set) Token: 0x060001EB RID: 491 RVA: 0x00005AE2 File Offset: 0x00003CE2
		public float Speed
		{
			get
			{
				return this._Speed;
			}
			set
			{
				if (this._Speed == value)
				{
					return;
				}
				this._Speed = value;
				if (this._Playable.IsValid<Playable>())
				{
					this._Playable.SetSpeed((double)value);
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00005B10 File Offset: 0x00003D10
		private float ParentEffectiveSpeed
		{
			get
			{
				IPlayableWrapper parent = this.Parent;
				if (parent == null)
				{
					return 1f;
				}
				float num = parent.Speed;
				while ((parent = parent.Parent) != null)
				{
					num *= parent.Speed;
				}
				return num;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00005B4A File Offset: 0x00003D4A
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00005B59 File Offset: 0x00003D59
		public float EffectiveSpeed
		{
			get
			{
				return this.Speed * this.ParentEffectiveSpeed;
			}
			set
			{
				this.Speed = value / this.ParentEffectiveSpeed;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00005B69 File Offset: 0x00003D69
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00005B70 File Offset: 0x00003D70
		public static bool ApplyParentAnimatorIK { get; set; } = true;

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00005B78 File Offset: 0x00003D78
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00005B7F File Offset: 0x00003D7F
		public static bool ApplyParentFootIK { get; set; } = true;

		// Token: 0x060001F3 RID: 499 RVA: 0x00005B87 File Offset: 0x00003D87
		public virtual void CopyIKFlags(AnimancerNode copyFrom)
		{
			if (this.Root == null)
			{
				return;
			}
			if (AnimancerNode.ApplyParentAnimatorIK)
			{
				this.ApplyAnimatorIK = copyFrom.ApplyAnimatorIK;
			}
			if (AnimancerNode.ApplyParentFootIK)
			{
				this.ApplyFootIK = copyFrom.ApplyFootIK;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00005BB8 File Offset: 0x00003DB8
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00005BF0 File Offset: 0x00003DF0
		public virtual bool ApplyAnimatorIK
		{
			get
			{
				for (int i = this.ChildCount - 1; i >= 0; i--)
				{
					AnimancerState child = this.GetChild(i);
					if (child != null && child.ApplyAnimatorIK)
					{
						return true;
					}
				}
				return false;
			}
			set
			{
				for (int i = this.ChildCount - 1; i >= 0; i--)
				{
					AnimancerState child = this.GetChild(i);
					if (child != null)
					{
						child.ApplyAnimatorIK = value;
					}
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00005C24 File Offset: 0x00003E24
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00005C5C File Offset: 0x00003E5C
		public virtual bool ApplyFootIK
		{
			get
			{
				for (int i = this.ChildCount - 1; i >= 0; i--)
				{
					AnimancerState child = this.GetChild(i);
					if (child != null && child.ApplyFootIK)
					{
						return true;
					}
				}
				return false;
			}
			set
			{
				for (int i = this.ChildCount - 1; i >= 0; i--)
				{
					AnimancerState child = this.GetChild(i);
					if (child != null)
					{
						child.ApplyFootIK = value;
					}
				}
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00005C8E File Offset: 0x00003E8E
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00005C96 File Offset: 0x00003E96
		[Conditional("UNITY_ASSERTIONS")]
		public void SetDebugName(string name)
		{
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00005C98 File Offset: 0x00003E98
		public string GetDescription(string separator = "\n")
		{
			StringBuilder stringBuilder = ObjectPool.AcquireStringBuilder();
			this.AppendDescription(stringBuilder, separator);
			return stringBuilder.ReleaseToString();
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00005CBC File Offset: 0x00003EBC
		public void AppendDescription(StringBuilder text, string separator = "\n")
		{
			text.Append(this.ToString());
			this.AppendDetails(text, separator);
			if (this.ChildCount > 0)
			{
				text.Append(separator).Append("ChildCount: ").Append(this.ChildCount);
				string separator2 = separator + "    ";
				int num = 0;
				foreach (AnimancerState animancerState in this)
				{
					text.Append(separator).Append('[').Append(num++).Append("] ");
					if (animancerState != null)
					{
						animancerState.AppendDescription(text, separator2);
					}
					else
					{
						text.Append("null");
					}
				}
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00005D8C File Offset: 0x00003F8C
		protected virtual void AppendDetails(StringBuilder text, string separator)
		{
			text.Append(separator).Append("Playable: ");
			if (this._Playable.IsValid<Playable>())
			{
				text.Append(this._Playable.GetPlayableType());
			}
			else
			{
				text.Append("Invalid");
			}
			text.Append(separator).Append("Index: ").Append(this.Index);
			double num = this._Playable.IsValid<Playable>() ? this._Playable.GetSpeed<Playable>() : ((double)this._Speed);
			if (num == (double)this._Speed)
			{
				text.Append(separator).Append("Speed: ").Append(this._Speed);
			}
			else
			{
				text.Append(separator).Append("Speed (Real): ").Append(this._Speed).Append(" (").Append(num).Append(')');
			}
			text.Append(separator).Append("Weight: ").Append(this.Weight);
			if (this.Weight != this.TargetWeight)
			{
				text.Append(separator).Append("TargetWeight: ").Append(this.TargetWeight);
				text.Append(separator).Append("FadeSpeed: ").Append(this.FadeSpeed);
			}
			AnimancerNode.AppendIKDetails(text, separator, this);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00005EE4 File Offset: 0x000040E4
		public static void AppendIKDetails(StringBuilder text, string separator, IPlayableWrapper node)
		{
			if (!node.Playable.IsValid<Playable>())
			{
				return;
			}
			text.Append(separator).Append("InverseKinematics: ");
			if (node.ApplyAnimatorIK)
			{
				text.Append("OnAnimatorIK");
				if (node.ApplyFootIK)
				{
					text.Append(", FootIK");
					return;
				}
			}
			else
			{
				if (node.ApplyFootIK)
				{
					text.Append("FootIK");
					return;
				}
				text.Append("None");
			}
		}

		// Token: 0x0400002C RID: 44
		protected internal Playable _Playable;

		// Token: 0x0400002D RID: 45
		private AnimancerPlayable _Root;

		// Token: 0x0400002F RID: 47
		private float _Weight;

		// Token: 0x04000030 RID: 48
		private bool _IsWeightDirty = true;

		// Token: 0x04000033 RID: 51
		private float _Speed = 1f;
	}
}
