using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000019 RID: 25
	public class PlayableAssetState : AnimancerState, ICopyable<PlayableAssetState>
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x000086FD File Offset: 0x000068FD
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x00008705 File Offset: 0x00006905
		public PlayableAsset Asset
		{
			get
			{
				return this._Asset;
			}
			set
			{
				base.ChangeMainObject<PlayableAsset>(ref this._Asset, value);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00008714 File Offset: 0x00006914
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000871C File Offset: 0x0000691C
		public override UnityEngine.Object MainObject
		{
			get
			{
				return this._Asset;
			}
			set
			{
				this._Asset = (PlayableAsset)value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000872A File Offset: 0x0000692A
		public override float Length
		{
			get
			{
				return this._Length;
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00008734 File Offset: 0x00006934
		protected override void OnSetIsPlaying()
		{
			int inputCount = this._Playable.GetInputCount<Playable>();
			for (int i = 0; i < inputCount; i++)
			{
				Playable input = this._Playable.GetInput(i);
				if (input.IsValid<Playable>())
				{
					if (base.IsPlaying)
					{
						input.Play<Playable>();
					}
					else
					{
						input.Pause<Playable>();
					}
				}
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00008784 File Offset: 0x00006984
		public override void CopyIKFlags(AnimancerNode copyFrom)
		{
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00008786 File Offset: 0x00006986
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00008789 File Offset: 0x00006989
		public override bool ApplyAnimatorIK
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000878B File Offset: 0x0000698B
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000878E File Offset: 0x0000698E
		public override bool ApplyFootIK
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00008790 File Offset: 0x00006990
		public PlayableAssetState(PlayableAsset asset)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			this._Asset = asset;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000087B4 File Offset: 0x000069B4
		protected override void CreatePlayable(out Playable playable)
		{
			playable = this._Asset.CreatePlayable(base.Root._Graph, base.Root.Component.gameObject);
			playable.SetDuration(9223372.03685477);
			this._Length = (float)this._Asset.duration;
			if (!this._HasInitializedBindings)
			{
				this.InitializeBindings();
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00008821 File Offset: 0x00006A21
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x00008829 File Offset: 0x00006A29
		public IList<UnityEngine.Object> Bindings
		{
			get
			{
				return this._Bindings;
			}
			set
			{
				this._Bindings = value;
				this.InitializeBindings();
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00008838 File Offset: 0x00006A38
		public void SetBindings(params UnityEngine.Object[] bindings)
		{
			this.Bindings = bindings;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00008844 File Offset: 0x00006A44
		private void InitializeBindings()
		{
			if (base.Root == null)
			{
				return;
			}
			this._HasInitializedBindings = true;
			PlayableGraph graph = base.Root._Graph;
			int num = 0;
			int num2 = (this._Bindings != null) ? this._Bindings.Count : 0;
			foreach (PlayableBinding binding in this._Asset.outputs)
			{
				string name;
				Type left;
				bool flag;
				PlayableAssetState.GetBindingDetails(binding, out name, out left, out flag);
				UnityEngine.Object @object = (num < num2) ? this._Bindings[num] : null;
				Playable input = this._Playable.GetInput(num);
				if (left == typeof(Animator))
				{
					if (@object != null)
					{
						AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, name, (Animator)@object);
						output.SetReferenceObject(binding.sourceObject);
						output.SetSourcePlayable(input);
						output.SetWeight(1f);
					}
				}
				else if (left == typeof(AudioSource))
				{
					if (@object != null)
					{
						AudioPlayableOutput output2 = AudioPlayableOutput.Create(graph, name, (AudioSource)@object);
						output2.SetReferenceObject(binding.sourceObject);
						output2.SetSourcePlayable(input);
						output2.SetWeight(1f);
					}
				}
				else
				{
					if (flag)
					{
						Component component = base.Root.Component as Component;
						ScriptPlayableOutput output3 = ScriptPlayableOutput.Create(graph, name);
						output3.SetReferenceObject(binding.sourceObject);
						output3.SetSourcePlayable(input);
						output3.SetWeight(1f);
						output3.SetUserData(component);
						List<INotificationReceiver> list = ObjectPool.AcquireList<INotificationReceiver>();
						component.GetComponents<INotificationReceiver>(list);
						for (int i = 0; i < list.Count; i++)
						{
							output3.AddNotificationReceiver(list[i]);
						}
						ObjectPool.Release<INotificationReceiver>(list);
						continue;
					}
					ScriptPlayableOutput output4 = ScriptPlayableOutput.Create(graph, name);
					output4.SetReferenceObject(binding.sourceObject);
					output4.SetSourcePlayable(input);
					output4.SetWeight(1f);
					output4.SetUserData(@object);
					INotificationReceiver notificationReceiver = @object as INotificationReceiver;
					if (notificationReceiver != null)
					{
						output4.AddNotificationReceiver(notificationReceiver);
					}
				}
				num++;
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00008A8C File Offset: 0x00006C8C
		public static void GetBindingDetails(PlayableBinding binding, out string name, out Type type, out bool isMarkers)
		{
			name = binding.streamName;
			type = binding.outputTargetType;
			isMarkers = (type == typeof(GameObject) && name == "Markers");
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00008AC4 File Offset: 0x00006CC4
		public override void Destroy()
		{
			this._Asset = null;
			base.Destroy();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00008AD3 File Offset: 0x00006CD3
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			PlayableAssetState playableAssetState = new PlayableAssetState(this._Asset);
			playableAssetState.SetNewCloneRoot(root);
			((ICopyable<PlayableAssetState>)playableAssetState).CopyFrom(this);
			return playableAssetState;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00008AEE File Offset: 0x00006CEE
		void ICopyable<PlayableAssetState>.CopyFrom(PlayableAssetState copyFrom)
		{
			this._Length = copyFrom._Length;
			((ICopyable<AnimancerState>)this).CopyFrom(copyFrom);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00008B04 File Offset: 0x00006D04
		protected override void AppendDetails(StringBuilder text, string separator)
		{
			base.AppendDetails(text, separator);
			text.Append(separator).Append("Bindings: ");
			int num;
			if (this._Bindings == null)
			{
				text.Append("Null");
				num = 0;
			}
			else
			{
				num = this._Bindings.Count;
				text.Append('[').Append(num).Append(']');
			}
			text.Append(this._HasInitializedBindings ? " (Initialized)" : " (Not Initialized)");
			for (int i = 0; i < num; i++)
			{
				text.Append(separator).Append("Bindings[").Append(i).Append("] = ").Append(AnimancerUtilities.ToStringOrNull(this._Bindings[i]));
			}
		}

		// Token: 0x0400005F RID: 95
		private PlayableAsset _Asset;

		// Token: 0x04000060 RID: 96
		private float _Length;

		// Token: 0x04000061 RID: 97
		private IList<UnityEngine.Object> _Bindings;

		// Token: 0x04000062 RID: 98
		private bool _HasInitializedBindings;

		// Token: 0x0200008C RID: 140
		public interface ITransition : ITransition<PlayableAssetState>, Animancer.ITransition, IHasKey, IPolymorphic
		{
		}
	}
}
