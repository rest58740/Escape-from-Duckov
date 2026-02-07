using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000014 RID: 20
	public class AnimancerPlayable : PlayableBehaviour, IEnumerator, IPlayableWrapper, IAnimationClipCollection
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00005F69 File Offset: 0x00004169
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00005F70 File Offset: 0x00004170
		public static float DefaultFadeDuration
		{
			get
			{
				return AnimancerPlayable._DefaultFadeDuration;
			}
			set
			{
				AnimancerPlayable._DefaultFadeDuration = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00005F78 File Offset: 0x00004178
		public PlayableGraph Graph
		{
			get
			{
				return this._Graph;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00005F80 File Offset: 0x00004180
		Playable IPlayableWrapper.Playable
		{
			get
			{
				return this._LayerMixer;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00005F88 File Offset: 0x00004188
		IPlayableWrapper IPlayableWrapper.Parent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00005F8B File Offset: 0x0000418B
		float IPlayableWrapper.Weight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00005F92 File Offset: 0x00004192
		int IPlayableWrapper.ChildCount
		{
			get
			{
				return this.Layers.Count;
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00005F9F File Offset: 0x0000419F
		AnimancerNode IPlayableWrapper.GetChild(int index)
		{
			return this.Layers[index];
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00005FAD File Offset: 0x000041AD
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00005FB5 File Offset: 0x000041B5
		public AnimancerPlayable.LayerList Layers { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00005FBE File Offset: 0x000041BE
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00005FC6 File Offset: 0x000041C6
		public AnimancerPlayable.StateDictionary States { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00005FCF File Offset: 0x000041CF
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00005FD7 File Offset: 0x000041D7
		public IAnimancerComponent Component { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00005FE0 File Offset: 0x000041E0
		public int CommandCount
		{
			get
			{
				return this.Layers[0].CommandCount;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00005FF3 File Offset: 0x000041F3
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00006000 File Offset: 0x00004200
		public DirectorUpdateMode UpdateMode
		{
			get
			{
				return this._Graph.GetTimeUpdateMode();
			}
			set
			{
				this._Graph.SetTimeUpdateMode(value);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000600E File Offset: 0x0000420E
		// (set) Token: 0x06000211 RID: 529 RVA: 0x00006018 File Offset: 0x00004218
		public float Speed
		{
			get
			{
				return this._Speed;
			}
			set
			{
				Playable layerMixer = this._LayerMixer;
				this._Speed = value;
				layerMixer.SetSpeed((double)value);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000603B File Offset: 0x0000423B
		// (set) Token: 0x06000213 RID: 531 RVA: 0x00006044 File Offset: 0x00004244
		public bool KeepChildrenConnected
		{
			get
			{
				return this._KeepChildrenConnected;
			}
			set
			{
				if (this._KeepChildrenConnected == value)
				{
					return;
				}
				this._KeepChildrenConnected = value;
				if (value)
				{
					this._PostUpdate.IsConnected = true;
					for (int i = this.Layers.Count - 1; i >= 0; i--)
					{
						this.Layers.GetLayer(i).ConnectAllChildrenToGraph();
					}
					return;
				}
				for (int j = this.Layers.Count - 1; j >= 0; j--)
				{
					this.Layers.GetLayer(j).DisconnectWeightlessChildrenFromGraph();
				}
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000214 RID: 532 RVA: 0x000060C4 File Offset: 0x000042C4
		// (set) Token: 0x06000215 RID: 533 RVA: 0x000060CC File Offset: 0x000042CC
		public bool SkipFirstFade
		{
			get
			{
				return this._SkipFirstFade;
			}
			set
			{
				this._SkipFirstFade = value;
				if (!value && this.Layers.Count < 2)
				{
					this.Layers.Count = 1;
					this._LayerMixer.SetInputCount(2);
				}
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000060FE File Offset: 0x000042FE
		public static AnimancerPlayable Create()
		{
			return AnimancerPlayable.Create(PlayableGraph.Create());
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000610A File Offset: 0x0000430A
		public static AnimancerPlayable Create(PlayableGraph graph)
		{
			return AnimancerPlayable.Create<AnimancerPlayable>(graph, AnimancerPlayable.Template);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00006118 File Offset: 0x00004318
		protected static T Create<T>(PlayableGraph graph, T template) where T : AnimancerPlayable, new()
		{
			return ScriptPlayable<T>.Create(graph, template, 2).GetBehaviour();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00006138 File Offset: 0x00004338
		public override void OnPlayableCreate(Playable playable)
		{
			this._RootPlayable = playable;
			this._Graph = playable.GetGraph<Playable>();
			this._PostUpdatables = new Key.KeyedList<IUpdatable>();
			this._PreUpdatables = new Key.KeyedList<IUpdatable>();
			this._PostUpdate = AnimancerPlayable.PostUpdate.Create(this);
			this.Layers = new AnimancerPlayable.LayerList(this, ref this._LayerMixer);
			this.States = new AnimancerPlayable.StateDictionary(this);
			playable.SetInputWeight(0, 1f);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000061A4 File Offset: 0x000043A4
		[Conditional("UNITY_EDITOR")]
		public static void SetNextGraphName(string name)
		{
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000061A8 File Offset: 0x000043A8
		public bool TryGetOutput(out PlayableOutput output)
		{
			int outputCount = this._Graph.GetOutputCount();
			for (int i = 0; i < outputCount; i++)
			{
				output = this._Graph.GetOutput(i);
				if (output.GetSourcePlayable<PlayableOutput>().Equals(this._RootPlayable))
				{
					return true;
				}
			}
			output = default(PlayableOutput);
			return false;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00006204 File Offset: 0x00004404
		public void CreateOutput(IAnimancerComponent animancer)
		{
			this.CreateOutput(animancer.Animator, animancer);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00006214 File Offset: 0x00004414
		public void CreateOutput(Animator animator, IAnimancerComponent animancer)
		{
			this.Component = animancer;
			bool isHuman = animator.isHuman;
			this.KeepChildrenConnected = !isHuman;
			this.SkipFirstFade = (isHuman || animator.runtimeAnimatorController == null);
			AnimationPlayableUtilities.Play(animator, this._RootPlayable, this._Graph);
			this._IsGraphPlaying = true;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000626C File Offset: 0x0000446C
		public void InsertOutputPlayable(Playable playable)
		{
			PlayableOutput output = this._Graph.GetOutput(0);
			this._Graph.Connect<Playable, Playable>(output.GetSourcePlayable<PlayableOutput>(), 0, playable, 0);
			playable.SetInputWeight(0, 1f);
			output.SetSourcePlayable(playable);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000062B0 File Offset: 0x000044B0
		public AnimationScriptPlayable InsertOutputJob<T>(T data) where T : struct, IAnimationJob
		{
			AnimationScriptPlayable animationScriptPlayable = AnimationScriptPlayable.Create<T>(this._Graph, data, 1);
			PlayableOutput output = this._Graph.GetOutput(0);
			this._Graph.Connect<Playable, AnimationScriptPlayable>(output.GetSourcePlayable<PlayableOutput>(), 0, animationScriptPlayable, 0);
			animationScriptPlayable.SetInputWeight(0, 1f);
			output.SetSourcePlayable(animationScriptPlayable);
			return animationScriptPlayable;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00006301 File Offset: 0x00004501
		public bool IsValid
		{
			get
			{
				return this._Graph.IsValid();
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000630E File Offset: 0x0000450E
		public void DestroyGraph()
		{
			if (this._Graph.IsValid())
			{
				this._Graph.Destroy();
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00006328 File Offset: 0x00004528
		public bool DestroyOutput()
		{
			PlayableOutput output;
			if (this.TryGetOutput(out output))
			{
				this._Graph.DestroyOutput<PlayableOutput>(output);
				return true;
			}
			return false;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000634E File Offset: 0x0000454E
		public override void OnPlayableDestroy(Playable playable)
		{
			AnimancerPlayable value = AnimancerPlayable.Current;
			AnimancerPlayable.Current = this;
			this.DisposeAll();
			GC.SuppressFinalize(this);
			this.Layers = null;
			this.States = null;
			AnimancerPlayable.Current = value;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000637C File Offset: 0x0000457C
		public List<IDisposable> Disposables
		{
			get
			{
				List<IDisposable> result;
				if ((result = this._Disposables) == null)
				{
					result = (this._Disposables = new List<IDisposable>());
				}
				return result;
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000063A4 File Offset: 0x000045A4
		~AnimancerPlayable()
		{
			this.DisposeAll();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x000063D0 File Offset: 0x000045D0
		private void DisposeAll()
		{
			if (this._Disposables == null)
			{
				return;
			}
			int num = this._Disposables.Count;
			for (;;)
			{
				try
				{
					while (--num >= 0)
					{
						this._Disposables[num].Dispose();
					}
					this._Disposables.Clear();
					this._Disposables = null;
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception, this.Component as UnityEngine.Object);
					continue;
				}
				break;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00006444 File Offset: 0x00004644
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000644C File Offset: 0x0000464C
		public bool ApplyAnimatorIK
		{
			get
			{
				return this._ApplyAnimatorIK;
			}
			set
			{
				this._ApplyAnimatorIK = value;
				for (int i = this.Layers.Count - 1; i >= 0; i--)
				{
					this.Layers.GetLayer(i).ApplyAnimatorIK = value;
				}
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000648A File Offset: 0x0000468A
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00006494 File Offset: 0x00004694
		public bool ApplyFootIK
		{
			get
			{
				return this._ApplyFootIK;
			}
			set
			{
				this._ApplyFootIK = value;
				for (int i = this.Layers.Count - 1; i >= 0; i--)
				{
					this.Layers.GetLayer(i).ApplyFootIK = value;
				}
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000064D2 File Offset: 0x000046D2
		public object GetKey(AnimationClip clip)
		{
			if (this.Component == null)
			{
				return clip;
			}
			return this.Component.GetKey(clip);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000064EA File Offset: 0x000046EA
		public AnimancerState Play(AnimationClip clip)
		{
			return this.Play(this.States.GetOrCreate(clip, false));
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000064FF File Offset: 0x000046FF
		public AnimancerState Play(AnimancerState state)
		{
			return this.GetLocalLayer(state).Play(state);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000650E File Offset: 0x0000470E
		public AnimancerState Play(AnimationClip clip, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			return this.Play(this.States.GetOrCreate(clip, false), fadeDuration, mode);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00006525 File Offset: 0x00004725
		public AnimancerState Play(AnimancerState state, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			return this.GetLocalLayer(state).Play(state, fadeDuration, mode);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00006536 File Offset: 0x00004736
		public AnimancerState Play(ITransition transition)
		{
			return this.Play(transition, transition.FadeDuration, transition.FadeMode);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000654C File Offset: 0x0000474C
		public AnimancerState Play(ITransition transition, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			AnimancerState animancerState = this.States.GetOrCreate(transition);
			animancerState = this.Play(animancerState, fadeDuration, mode);
			transition.Apply(animancerState);
			return animancerState;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00006578 File Offset: 0x00004778
		public AnimancerState TryPlay(object key)
		{
			AnimancerState state;
			if (!this.States.TryGet(key, out state))
			{
				return null;
			}
			return this.Play(state);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000065A0 File Offset: 0x000047A0
		public AnimancerState TryPlay(object key, float fadeDuration, FadeMode mode = FadeMode.FixedSpeed)
		{
			AnimancerState state;
			if (!this.States.TryGet(key, out state))
			{
				return null;
			}
			return this.Play(state, fadeDuration, mode);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000065C8 File Offset: 0x000047C8
		private AnimancerLayer GetLocalLayer(AnimancerState state)
		{
			if (state.Root == this)
			{
				AnimancerLayer layer = state.Layer;
				if (layer != null)
				{
					return layer;
				}
			}
			return this.Layers[0];
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000065F6 File Offset: 0x000047F6
		public AnimancerState Stop(IHasKey hasKey)
		{
			return this.Stop(hasKey.Key);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00006604 File Offset: 0x00004804
		public AnimancerState Stop(object key)
		{
			AnimancerState animancerState;
			if (this.States.TryGet(key, out animancerState))
			{
				animancerState.Stop();
			}
			return animancerState;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00006628 File Offset: 0x00004828
		public void Stop()
		{
			for (int i = this.Layers.Count - 1; i >= 0; i--)
			{
				this.Layers.GetLayer(i).Stop();
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000665E File Offset: 0x0000485E
		public bool IsPlaying(IHasKey hasKey)
		{
			return this.IsPlaying(hasKey.Key);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000666C File Offset: 0x0000486C
		public bool IsPlaying(object key)
		{
			AnimancerState animancerState;
			return this.States.TryGet(key, out animancerState) && animancerState.IsPlaying;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00006694 File Offset: 0x00004894
		public bool IsPlaying()
		{
			if (!this._IsGraphPlaying)
			{
				return false;
			}
			for (int i = this.Layers.Count - 1; i >= 0; i--)
			{
				if (this.Layers.GetLayer(i).IsAnyStatePlaying())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000066DC File Offset: 0x000048DC
		public bool IsPlayingClip(AnimationClip clip)
		{
			if (!this._IsGraphPlaying)
			{
				return false;
			}
			for (int i = this.Layers.Count - 1; i >= 0; i--)
			{
				if (this.Layers.GetLayer(i).IsPlayingClip(clip))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00006724 File Offset: 0x00004924
		public float GetTotalWeight()
		{
			float num = 0f;
			for (int i = this.Layers.Count - 1; i >= 0; i--)
			{
				num += this.Layers.GetLayer(i).GetTotalWeight();
			}
			return num;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00006764 File Offset: 0x00004964
		public void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			this.Layers.GatherAnimationClips(clips);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00006774 File Offset: 0x00004974
		bool IEnumerator.MoveNext()
		{
			for (int i = this.Layers.Count - 1; i >= 0; i--)
			{
				if (this.Layers.GetLayer(i).IsPlayingAndNotEnding())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600023F RID: 575 RVA: 0x000067AF File Offset: 0x000049AF
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x000067B2 File Offset: 0x000049B2
		void IEnumerator.Reset()
		{
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000067B4 File Offset: 0x000049B4
		// (set) Token: 0x06000242 RID: 578 RVA: 0x000067BC File Offset: 0x000049BC
		public bool IsGraphPlaying
		{
			get
			{
				return this._IsGraphPlaying;
			}
			set
			{
				if (value)
				{
					this.UnpauseGraph();
					return;
				}
				this.PauseGraph();
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000067CE File Offset: 0x000049CE
		public void UnpauseGraph()
		{
			if (!this._IsGraphPlaying)
			{
				this._Graph.Play();
				this._IsGraphPlaying = true;
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000067EA File Offset: 0x000049EA
		public void PauseGraph()
		{
			if (this._IsGraphPlaying)
			{
				this._Graph.Stop();
				this._IsGraphPlaying = false;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00006806 File Offset: 0x00004A06
		public void Evaluate()
		{
			this._Graph.Evaluate();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00006813 File Offset: 0x00004A13
		public void Evaluate(float deltaTime)
		{
			this._Graph.Evaluate(deltaTime);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00006824 File Offset: 0x00004A24
		public string GetDescription()
		{
			StringBuilder stringBuilder = ObjectPool.AcquireStringBuilder();
			this.AppendDescription(stringBuilder);
			return stringBuilder.ReleaseToString();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00006844 File Offset: 0x00004A44
		public void AppendDescription(StringBuilder text)
		{
			text.Append("AnimancerPlayable (").Append(this.Component).Append(") Layer Count: ").Append(this.Layers.Count);
			AnimancerNode.AppendIKDetails(text, "\n    ", this);
			int count = this.Layers.Count;
			for (int i = 0; i < count; i++)
			{
				text.Append("\n    ");
				this.Layers[i].AppendDescription(text, "\n    ");
			}
			text.AppendLine();
			this.AppendInternalDetails(text, "    ", "        ");
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000068E4 File Offset: 0x00004AE4
		public void AppendInternalDetails(StringBuilder text, string sectionPrefix, string itemPrefix)
		{
			AnimancerPlayable.AppendAll(text, sectionPrefix, itemPrefix, this._PreUpdatables, "Pre Updatables");
			text.AppendLine();
			AnimancerPlayable.AppendAll(text, sectionPrefix, itemPrefix, this._PostUpdatables, "Post Updatables");
			text.AppendLine();
			AnimancerPlayable.AppendAll(text, sectionPrefix, itemPrefix, this._Disposables, "Disposables");
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00006938 File Offset: 0x00004B38
		private static void AppendAll(StringBuilder text, string sectionPrefix, string itemPrefix, ICollection collection, string name)
		{
			int value = (collection != null) ? collection.Count : 0;
			text.Append(sectionPrefix).Append(name).Append(": ").Append(value);
			if (collection != null)
			{
				foreach (object value2 in collection)
				{
					text.AppendLine().Append(itemPrefix).Append(value2);
				}
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000069C4 File Offset: 0x00004BC4
		public void RequirePreUpdate(IUpdatable updatable)
		{
			this._PreUpdatables.AddNew(updatable);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000069D2 File Offset: 0x00004BD2
		public void RequirePostUpdate(IUpdatable updatable)
		{
			this._PostUpdatables.AddNew(updatable);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000069E0 File Offset: 0x00004BE0
		private void CancelUpdate(Key.KeyedList<IUpdatable> updatables, IUpdatable updatable)
		{
			int num = updatables.IndexOf(updatable);
			if (num < 0)
			{
				return;
			}
			updatables.RemoveAtSwap(num);
			if (AnimancerPlayable._CurrentUpdatable < num && updatables == AnimancerPlayable._CurrentUpdatables)
			{
				AnimancerPlayable._CurrentUpdatable--;
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00006A1D File Offset: 0x00004C1D
		public void CancelPreUpdate(IUpdatable updatable)
		{
			this.CancelUpdate(this._PreUpdatables, updatable);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00006A2C File Offset: 0x00004C2C
		public void CancelPostUpdate(IUpdatable updatable)
		{
			this.CancelUpdate(this._PostUpdatables, updatable);
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00006A3B File Offset: 0x00004C3B
		public int PreUpdatableCount
		{
			get
			{
				return this._PreUpdatables.Count;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00006A48 File Offset: 0x00004C48
		public int PostUpdatableCount
		{
			get
			{
				return this._PostUpdatables.Count;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00006A55 File Offset: 0x00004C55
		public IUpdatable GetPreUpdatable(int index)
		{
			return this._PreUpdatables[index];
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00006A63 File Offset: 0x00004C63
		public IUpdatable GetPostUpdatable(int index)
		{
			return this._PostUpdatables[index];
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00006A71 File Offset: 0x00004C71
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00006A78 File Offset: 0x00004C78
		public static AnimancerPlayable Current { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00006A80 File Offset: 0x00004C80
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00006A87 File Offset: 0x00004C87
		public static float DeltaTime { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00006A8F File Offset: 0x00004C8F
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00006A97 File Offset: 0x00004C97
		public ulong FrameID { get; private set; }

		// Token: 0x0600025A RID: 602 RVA: 0x00006AA0 File Offset: 0x00004CA0
		public override void PrepareFrame(Playable playable, FrameData info)
		{
			this.UpdateAll(this._PreUpdatables, info.deltaTime * info.effectiveParentSpeed);
			if (!this._KeepChildrenConnected)
			{
				this._PostUpdate.IsConnected = (this._PostUpdatables.Count != 0);
			}
			this.FrameID = info.frameId;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00006AF8 File Offset: 0x00004CF8
		private void UpdateAll(Key.KeyedList<IUpdatable> updatables, float deltaTime)
		{
			AnimancerPlayable value = AnimancerPlayable.Current;
			AnimancerPlayable.Current = this;
			Key.KeyedList<IUpdatable> currentUpdatables = AnimancerPlayable._CurrentUpdatables;
			AnimancerPlayable._CurrentUpdatables = updatables;
			AnimancerPlayable.DeltaTime = deltaTime;
			int currentUpdatable = AnimancerPlayable._CurrentUpdatable;
			AnimancerPlayable._CurrentUpdatable = updatables.Count;
			for (;;)
			{
				try
				{
					while (--AnimancerPlayable._CurrentUpdatable >= 0)
					{
						updatables[AnimancerPlayable._CurrentUpdatable].Update();
					}
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception, this.Component as UnityEngine.Object);
					continue;
				}
				break;
			}
			AnimancerPlayable._CurrentUpdatable = currentUpdatable;
			AnimancerPlayable._CurrentUpdatables = currentUpdatables;
			AnimancerPlayable.Current = value;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00006B90 File Offset: 0x00004D90
		public static bool IsRunningPostUpdate(AnimancerPlayable animancer)
		{
			return AnimancerPlayable._CurrentUpdatables == animancer._PostUpdatables;
		}

		// Token: 0x04000036 RID: 54
		private static float _DefaultFadeDuration = 0.25f;

		// Token: 0x04000037 RID: 55
		internal PlayableGraph _Graph;

		// Token: 0x04000038 RID: 56
		internal Playable _RootPlayable;

		// Token: 0x04000039 RID: 57
		internal Playable _LayerMixer;

		// Token: 0x0400003C RID: 60
		private Key.KeyedList<IUpdatable> _PreUpdatables;

		// Token: 0x0400003D RID: 61
		private Key.KeyedList<IUpdatable> _PostUpdatables;

		// Token: 0x0400003E RID: 62
		private AnimancerPlayable.PostUpdate _PostUpdate;

		// Token: 0x04000040 RID: 64
		private float _Speed = 1f;

		// Token: 0x04000041 RID: 65
		private bool _KeepChildrenConnected;

		// Token: 0x04000042 RID: 66
		private bool _SkipFirstFade;

		// Token: 0x04000043 RID: 67
		private static readonly AnimancerPlayable Template = new AnimancerPlayable();

		// Token: 0x04000044 RID: 68
		private List<IDisposable> _Disposables;

		// Token: 0x04000045 RID: 69
		private bool _ApplyAnimatorIK;

		// Token: 0x04000046 RID: 70
		private bool _ApplyFootIK;

		// Token: 0x04000047 RID: 71
		private bool _IsGraphPlaying = true;

		// Token: 0x0400004B RID: 75
		private static Key.KeyedList<IUpdatable> _CurrentUpdatables;

		// Token: 0x0400004C RID: 76
		private static int _CurrentUpdatable = -1;

		// Token: 0x02000086 RID: 134
		private class PostUpdate : PlayableBehaviour
		{
			// Token: 0x06000608 RID: 1544 RVA: 0x00010320 File Offset: 0x0000E520
			public static AnimancerPlayable.PostUpdate Create(AnimancerPlayable root)
			{
				AnimancerPlayable.PostUpdate behaviour = ScriptPlayable<AnimancerPlayable.PostUpdate>.Create(root._Graph, AnimancerPlayable.PostUpdate.Template, 0).GetBehaviour();
				behaviour._Root = root;
				return behaviour;
			}

			// Token: 0x06000609 RID: 1545 RVA: 0x0001034D File Offset: 0x0000E54D
			public override void OnPlayableCreate(Playable playable)
			{
				this._Playable = playable;
			}

			// Token: 0x17000181 RID: 385
			// (get) Token: 0x0600060A RID: 1546 RVA: 0x00010356 File Offset: 0x0000E556
			// (set) Token: 0x0600060B RID: 1547 RVA: 0x00010360 File Offset: 0x0000E560
			public bool IsConnected
			{
				get
				{
					return this._IsConnected;
				}
				set
				{
					if (value)
					{
						if (!this._IsConnected)
						{
							this._IsConnected = true;
							this._Root._Graph.Connect<Playable, Playable>(this._Playable, 0, this._Root._RootPlayable, 1);
							return;
						}
					}
					else if (this._IsConnected)
					{
						this._IsConnected = false;
						this._Root._Graph.Disconnect<Playable>(this._Root._RootPlayable, 1);
					}
				}
			}

			// Token: 0x0600060C RID: 1548 RVA: 0x000103CF File Offset: 0x0000E5CF
			public override void PrepareFrame(Playable playable, FrameData info)
			{
				this._Root.UpdateAll(this._Root._PostUpdatables, info.deltaTime * info.effectiveParentSpeed);
			}

			// Token: 0x04000122 RID: 290
			private static readonly AnimancerPlayable.PostUpdate Template = new AnimancerPlayable.PostUpdate();

			// Token: 0x04000123 RID: 291
			private AnimancerPlayable _Root;

			// Token: 0x04000124 RID: 292
			private Playable _Playable;

			// Token: 0x04000125 RID: 293
			private bool _IsConnected;
		}

		// Token: 0x02000087 RID: 135
		public class LayerList : IEnumerable<AnimancerLayer>, IEnumerable, IAnimationClipCollection
		{
			// Token: 0x0600060F RID: 1551 RVA: 0x0001040A File Offset: 0x0000E60A
			protected LayerList(AnimancerPlayable root)
			{
				this.Root = root;
				this._Layers = new AnimancerLayer[AnimancerPlayable.LayerList.DefaultCapacity];
			}

			// Token: 0x06000610 RID: 1552 RVA: 0x0001042C File Offset: 0x0000E62C
			internal LayerList(AnimancerPlayable root, out Playable layerMixer) : this(root)
			{
				layerMixer = (this.LayerMixer = AnimationLayerMixerPlayable.Create(root._Graph, 1));
				this.Root._Graph.Connect<Playable, Playable>(layerMixer, 0, this.Root._RootPlayable, 0);
			}

			// Token: 0x06000611 RID: 1553 RVA: 0x00010484 File Offset: 0x0000E684
			public virtual void Activate(AnimancerPlayable root)
			{
				this.Activate(root, this.LayerMixer);
			}

			// Token: 0x06000612 RID: 1554 RVA: 0x00010498 File Offset: 0x0000E698
			protected void Activate(AnimancerPlayable root, Playable mixer)
			{
				this._Layers = root.Layers._Layers;
				this._Count = root.Layers._Count;
				root._RootPlayable.DisconnectInput(0);
				root.Graph.Connect<Playable, Playable>(mixer, 0, root._RootPlayable, 0);
				root.Layers = this;
				root._LayerMixer = mixer;
			}

			// Token: 0x17000182 RID: 386
			// (get) Token: 0x06000613 RID: 1555 RVA: 0x000104F9 File Offset: 0x0000E6F9
			// (set) Token: 0x06000614 RID: 1556 RVA: 0x00010504 File Offset: 0x0000E704
			public int Count
			{
				get
				{
					return this._Count;
				}
				set
				{
					int num = this._Count;
					if (value == num)
					{
						return;
					}
					while (value > num)
					{
						this.Add();
						num++;
					}
					while (value < num--)
					{
						AnimancerLayer animancerLayer = this._Layers[num];
						if (animancerLayer._Playable.IsValid<Playable>())
						{
							this.Root._Graph.DestroySubgraph<Playable>(animancerLayer._Playable);
						}
						animancerLayer.DestroyStates();
					}
					Array.Clear(this._Layers, value, this._Count - value);
					this._Count = value;
					this.Root._LayerMixer.SetInputCount(value);
				}
			}

			// Token: 0x06000615 RID: 1557 RVA: 0x00010594 File Offset: 0x0000E794
			public void SetMinCount(int min)
			{
				if (this.Count < min)
				{
					this.Count = min;
				}
			}

			// Token: 0x17000183 RID: 387
			// (get) Token: 0x06000616 RID: 1558 RVA: 0x000105A6 File Offset: 0x0000E7A6
			// (set) Token: 0x06000617 RID: 1559 RVA: 0x000105AD File Offset: 0x0000E7AD
			public static int DefaultCapacity { get; set; } = 4;

			// Token: 0x06000618 RID: 1560 RVA: 0x000105B5 File Offset: 0x0000E7B5
			public static void SetMinDefaultCapacity(int min)
			{
				if (AnimancerPlayable.LayerList.DefaultCapacity < min)
				{
					AnimancerPlayable.LayerList.DefaultCapacity = min;
				}
			}

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x06000619 RID: 1561 RVA: 0x000105C5 File Offset: 0x0000E7C5
			// (set) Token: 0x0600061A RID: 1562 RVA: 0x000105CF File Offset: 0x0000E7CF
			public int Capacity
			{
				get
				{
					return this._Layers.Length;
				}
				set
				{
					if (value <= 0)
					{
						throw new ArgumentOutOfRangeException("value", string.Format("must be greater than 0 ({0} <= 0)", value));
					}
					if (this._Count > value)
					{
						this.Count = value;
					}
					Array.Resize<AnimancerLayer>(ref this._Layers, value);
				}
			}

			// Token: 0x0600061B RID: 1563 RVA: 0x0001060C File Offset: 0x0000E80C
			public AnimancerLayer Add()
			{
				int count = this._Count;
				if (count >= this._Layers.Length)
				{
					this.Capacity *= 2;
				}
				this._Count = count + 1;
				this.Root._LayerMixer.SetInputCount(this._Count);
				AnimancerLayer animancerLayer = new AnimancerLayer(this.Root, count);
				this._Layers[count] = animancerLayer;
				return animancerLayer;
			}

			// Token: 0x17000185 RID: 389
			public AnimancerLayer this[int index]
			{
				get
				{
					this.SetMinCount(index + 1);
					return this._Layers[index];
				}
			}

			// Token: 0x0600061D RID: 1565 RVA: 0x00010682 File Offset: 0x0000E882
			public AnimancerLayer GetLayer(int index)
			{
				return this._Layers[index];
			}

			// Token: 0x0600061E RID: 1566 RVA: 0x0001068C File Offset: 0x0000E88C
			public FastEnumerator<AnimancerLayer> GetEnumerator()
			{
				return new FastEnumerator<AnimancerLayer>(this._Layers, this._Count);
			}

			// Token: 0x0600061F RID: 1567 RVA: 0x0001069F File Offset: 0x0000E89F
			IEnumerator<AnimancerLayer> IEnumerable<AnimancerLayer>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000620 RID: 1568 RVA: 0x000106AC File Offset: 0x0000E8AC
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000621 RID: 1569 RVA: 0x000106B9 File Offset: 0x0000E8B9
			public void GatherAnimationClips(ICollection<AnimationClip> clips)
			{
				clips.GatherFromSource(this._Layers);
			}

			// Token: 0x06000622 RID: 1570 RVA: 0x000106C8 File Offset: 0x0000E8C8
			public virtual bool IsAdditive(int index)
			{
				return this.LayerMixer.IsLayerAdditive((uint)index);
			}

			// Token: 0x06000623 RID: 1571 RVA: 0x000106E4 File Offset: 0x0000E8E4
			public virtual void SetAdditive(int index, bool value)
			{
				this.SetMinCount(index + 1);
				this.LayerMixer.SetLayerAdditive((uint)index, value);
			}

			// Token: 0x06000624 RID: 1572 RVA: 0x0001070C File Offset: 0x0000E90C
			public virtual void SetMask(int index, AvatarMask mask)
			{
				this.SetMinCount(index + 1);
				if (mask == null)
				{
					mask = new AvatarMask();
				}
				this.LayerMixer.SetLayerMaskFromAvatarMask((uint)index, mask);
			}

			// Token: 0x06000625 RID: 1573 RVA: 0x00010742 File Offset: 0x0000E942
			[Conditional("UNITY_EDITOR")]
			public void SetDebugName(int index, string name)
			{
			}

			// Token: 0x17000186 RID: 390
			// (get) Token: 0x06000626 RID: 1574 RVA: 0x00010744 File Offset: 0x0000E944
			public Vector3 AverageVelocity
			{
				get
				{
					Vector3 vector = default(Vector3);
					for (int i = 0; i < this._Count; i++)
					{
						AnimancerLayer animancerLayer = this._Layers[i];
						vector += animancerLayer.AverageVelocity * animancerLayer.Weight;
					}
					return vector;
				}
			}

			// Token: 0x04000126 RID: 294
			protected readonly AnimancerPlayable Root;

			// Token: 0x04000127 RID: 295
			private AnimancerLayer[] _Layers;

			// Token: 0x04000128 RID: 296
			protected readonly AnimationLayerMixerPlayable LayerMixer;

			// Token: 0x04000129 RID: 297
			private int _Count;
		}

		// Token: 0x02000088 RID: 136
		public class StateDictionary : IEnumerable<AnimancerState>, IEnumerable, IAnimationClipCollection
		{
			// Token: 0x17000187 RID: 391
			// (get) Token: 0x06000628 RID: 1576 RVA: 0x00010794 File Offset: 0x0000E994
			// (set) Token: 0x06000629 RID: 1577 RVA: 0x0001079B File Offset: 0x0000E99B
			public static IEqualityComparer<object> EqualityComparer { get; set; } = FastComparer.Instance;

			// Token: 0x0600062A RID: 1578 RVA: 0x000107A3 File Offset: 0x0000E9A3
			internal StateDictionary(AnimancerPlayable root)
			{
				this.Root = root;
			}

			// Token: 0x17000188 RID: 392
			// (get) Token: 0x0600062B RID: 1579 RVA: 0x000107C2 File Offset: 0x0000E9C2
			public int Count
			{
				get
				{
					return this.States.Count;
				}
			}

			// Token: 0x0600062C RID: 1580 RVA: 0x000107CF File Offset: 0x0000E9CF
			public ClipState Create(AnimationClip clip)
			{
				return this.Create(this.Root.GetKey(clip), clip);
			}

			// Token: 0x0600062D RID: 1581 RVA: 0x000107E4 File Offset: 0x0000E9E4
			public ClipState Create(object key, AnimationClip clip)
			{
				ClipState clipState = new ClipState(clip);
				clipState.SetRoot(this.Root);
				clipState._Key = key;
				this.Register(clipState);
				return clipState;
			}

			// Token: 0x0600062E RID: 1582 RVA: 0x00010813 File Offset: 0x0000EA13
			public void CreateIfNew(AnimationClip clip0, AnimationClip clip1)
			{
				this.GetOrCreate(clip0, false);
				this.GetOrCreate(clip1, false);
			}

			// Token: 0x0600062F RID: 1583 RVA: 0x00010827 File Offset: 0x0000EA27
			public void CreateIfNew(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2)
			{
				this.GetOrCreate(clip0, false);
				this.GetOrCreate(clip1, false);
				this.GetOrCreate(clip2, false);
			}

			// Token: 0x06000630 RID: 1584 RVA: 0x00010844 File Offset: 0x0000EA44
			public void CreateIfNew(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2, AnimationClip clip3)
			{
				this.GetOrCreate(clip0, false);
				this.GetOrCreate(clip1, false);
				this.GetOrCreate(clip2, false);
				this.GetOrCreate(clip3, false);
			}

			// Token: 0x06000631 RID: 1585 RVA: 0x0001086C File Offset: 0x0000EA6C
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
						this.GetOrCreate(animationClip, false);
					}
				}
			}

			// Token: 0x17000189 RID: 393
			// (get) Token: 0x06000632 RID: 1586 RVA: 0x000108A3 File Offset: 0x0000EAA3
			public AnimancerState Current
			{
				get
				{
					return this.Root.Layers[0].CurrentState;
				}
			}

			// Token: 0x1700018A RID: 394
			public AnimancerState this[AnimationClip clip]
			{
				get
				{
					return this.States[this.Root.GetKey(clip)];
				}
			}

			// Token: 0x1700018B RID: 395
			public AnimancerState this[IHasKey hasKey]
			{
				get
				{
					return this.States[hasKey.Key];
				}
			}

			// Token: 0x1700018C RID: 396
			public AnimancerState this[object key]
			{
				get
				{
					return this.States[key];
				}
			}

			// Token: 0x06000636 RID: 1590 RVA: 0x000108F5 File Offset: 0x0000EAF5
			public bool TryGet(AnimationClip clip, out AnimancerState state)
			{
				if (clip == null)
				{
					state = null;
					return false;
				}
				return this.TryGet(this.Root.GetKey(clip), out state);
			}

			// Token: 0x06000637 RID: 1591 RVA: 0x00010918 File Offset: 0x0000EB18
			public bool TryGet(IHasKey hasKey, out AnimancerState state)
			{
				if (hasKey == null)
				{
					state = null;
					return false;
				}
				return this.TryGet(hasKey.Key, out state);
			}

			// Token: 0x06000638 RID: 1592 RVA: 0x0001092F File Offset: 0x0000EB2F
			public bool TryGet(object key, out AnimancerState state)
			{
				if (key == null)
				{
					state = null;
					return false;
				}
				return this.States.TryGetValue(key, out state);
			}

			// Token: 0x06000639 RID: 1593 RVA: 0x00010946 File Offset: 0x0000EB46
			public AnimancerState GetOrCreate(AnimationClip clip, bool allowSetClip = false)
			{
				return this.GetOrCreate(this.Root.GetKey(clip), clip, allowSetClip);
			}

			// Token: 0x0600063A RID: 1594 RVA: 0x0001095C File Offset: 0x0000EB5C
			public AnimancerState GetOrCreate(ITransition transition)
			{
				object key = transition.Key;
				AnimancerState animancerState;
				if (!this.TryGet(key, out animancerState))
				{
					animancerState = transition.CreateState();
					animancerState.SetRoot(this.Root);
					animancerState._Key = key;
					this.Register(animancerState);
				}
				return animancerState;
			}

			// Token: 0x0600063B RID: 1595 RVA: 0x000109A0 File Offset: 0x0000EBA0
			public AnimancerState GetOrCreate(object key, AnimationClip clip, bool allowSetClip = false)
			{
				AnimancerState animancerState;
				if (this.TryGet(key, out animancerState))
				{
					if (animancerState.Clip != clip)
					{
						if (!allowSetClip)
						{
							throw new ArgumentException(AnimancerPlayable.StateDictionary.GetClipMismatchError(key, animancerState.Clip, clip));
						}
						animancerState.Clip = clip;
					}
				}
				else
				{
					animancerState = this.Create(key, clip);
				}
				return animancerState;
			}

			// Token: 0x0600063C RID: 1596 RVA: 0x000109EA File Offset: 0x0000EBEA
			public static string GetClipMismatchError(object key, AnimationClip oldClip, AnimationClip newClip)
			{
				return "A state already exists using the specified 'key', but has a different AnimationClip:" + string.Format("\n• Key: {0}", key) + string.Format("\n• Old Clip: {0}", oldClip) + string.Format("\n• New Clip: {0}", newClip);
			}

			// Token: 0x0600063D RID: 1597 RVA: 0x00010A18 File Offset: 0x0000EC18
			internal void Register(AnimancerState state)
			{
				object key = state._Key;
				if (key != null)
				{
					this.States.Add(key, state);
				}
			}

			// Token: 0x0600063E RID: 1598 RVA: 0x00010A3C File Offset: 0x0000EC3C
			internal void Unregister(AnimancerState state)
			{
				object key = state._Key;
				if (key != null)
				{
					this.States.Remove(key);
				}
			}

			// Token: 0x0600063F RID: 1599 RVA: 0x00010A60 File Offset: 0x0000EC60
			public Dictionary<object, AnimancerState>.ValueCollection.Enumerator GetEnumerator()
			{
				return this.States.Values.GetEnumerator();
			}

			// Token: 0x06000640 RID: 1600 RVA: 0x00010A72 File Offset: 0x0000EC72
			IEnumerator<AnimancerState> IEnumerable<AnimancerState>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000641 RID: 1601 RVA: 0x00010A7F File Offset: 0x0000EC7F
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000642 RID: 1602 RVA: 0x00010A8C File Offset: 0x0000EC8C
			public void GatherAnimationClips(ICollection<AnimationClip> clips)
			{
				foreach (AnimancerState source in this.States.Values)
				{
					clips.GatherFromSource(source);
				}
			}

			// Token: 0x06000643 RID: 1603 RVA: 0x00010AE4 File Offset: 0x0000ECE4
			public bool Destroy(AnimationClip clip)
			{
				return !(clip == null) && this.Destroy(this.Root.GetKey(clip));
			}

			// Token: 0x06000644 RID: 1604 RVA: 0x00010B03 File Offset: 0x0000ED03
			public bool Destroy(IHasKey hasKey)
			{
				return hasKey != null && this.Destroy(hasKey.Key);
			}

			// Token: 0x06000645 RID: 1605 RVA: 0x00010B18 File Offset: 0x0000ED18
			public bool Destroy(object key)
			{
				AnimancerState animancerState;
				if (!this.TryGet(key, out animancerState))
				{
					return false;
				}
				animancerState.Destroy();
				return true;
			}

			// Token: 0x06000646 RID: 1606 RVA: 0x00010B3C File Offset: 0x0000ED3C
			public void DestroyAll(IList<AnimationClip> clips)
			{
				if (clips == null)
				{
					return;
				}
				for (int i = clips.Count - 1; i >= 0; i--)
				{
					this.Destroy(clips[i]);
				}
			}

			// Token: 0x06000647 RID: 1607 RVA: 0x00010B70 File Offset: 0x0000ED70
			public void DestroyAll(IEnumerable<AnimationClip> clips)
			{
				if (clips == null)
				{
					return;
				}
				foreach (AnimationClip clip in clips)
				{
					this.Destroy(clip);
				}
			}

			// Token: 0x06000648 RID: 1608 RVA: 0x00010BC0 File Offset: 0x0000EDC0
			public void DestroyAll(IAnimationClipSource source)
			{
				if (source == null)
				{
					return;
				}
				List<AnimationClip> list = ObjectPool.AcquireList<AnimationClip>();
				source.GetAnimationClips(list);
				this.DestroyAll(list);
				ObjectPool.Release<AnimationClip>(list);
			}

			// Token: 0x06000649 RID: 1609 RVA: 0x00010BEC File Offset: 0x0000EDEC
			public void DestroyAll(IAnimationClipCollection source)
			{
				if (source == null)
				{
					return;
				}
				HashSet<AnimationClip> hashSet = ObjectPool.AcquireSet<AnimationClip>();
				source.GatherAnimationClips(hashSet);
				this.DestroyAll(hashSet);
				ObjectPool.Release<AnimationClip>(hashSet);
			}

			// Token: 0x0400012B RID: 299
			private readonly AnimancerPlayable Root;

			// Token: 0x0400012D RID: 301
			private readonly Dictionary<object, AnimancerState> States = new Dictionary<object, AnimancerState>(AnimancerPlayable.StateDictionary.EqualityComparer);
		}
	}
}
