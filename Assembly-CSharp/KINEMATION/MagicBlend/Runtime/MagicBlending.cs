using System;
using System.Collections.Generic;
using KINEMATION.KAnimationCore.Runtime.Rig;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x02000055 RID: 85
	[HelpURL("https://kinemation.gitbook.io/magic-blend-documentation/")]
	public class MagicBlending : MonoBehaviour
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000CD97 File Offset: 0x0000AF97
		public MagicBlendAsset BlendAsset
		{
			get
			{
				return this.blendAsset;
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000CDA0 File Offset: 0x0000AFA0
		public void UpdateMagicBlendAsset(MagicBlendAsset newAsset, bool useBlending = false, float blendTime = -1f, bool useCurve = false)
		{
			if (newAsset == null)
			{
				Debug.LogWarning("MagicBlending: input asset is NULL!");
				return;
			}
			this._desiredBlendAsset = newAsset;
			this._useBlendCurve = useCurve;
			this._desiredBlendTime = blendTime;
			if (!useBlending)
			{
				this.SetNewAsset();
				if (!this.alwaysAnimatePoses)
				{
					this._poseJob.readPose = true;
					this._overlayJob.cachePose = true;
					this._poseJobPlayable.SetJobData<PoseJob>(this._poseJob);
					this._overlayJobPlayable.SetJobData<OverlayJob>(this._overlayJob);
				}
				return;
			}
			this._layeringJob.cachePose = true;
			this._layeringJobPlayable.SetJobData<LayeringJob>(this._layeringJob);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000CE40 File Offset: 0x0000B040
		public float GetOverlayTime(bool isNormalized = true)
		{
			Playable input = this._overlayJobPlayable.GetInput(0);
			if (!input.IsValid<Playable>() || !this.blendAsset.isAnimation)
			{
				return 0f;
			}
			float num = (float)input.GetDuration<Playable>();
			if (Mathf.Approximately(num, 0f))
			{
				return 0f;
			}
			float num2 = (float)input.GetTime<Playable>();
			if (!isNormalized)
			{
				return num2;
			}
			return Mathf.Clamp01(num2 / num);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
		protected virtual void SetNewAsset()
		{
			this.blendAsset = this._desiredBlendAsset;
			this._blendCurve = (this._useBlendCurve ? this.blendAsset.blendCurve : null);
			this._blendTime = ((this._desiredBlendTime > 0f) ? this._desiredBlendTime : this.blendAsset.blendTime);
			MagicBlendLibrary.ConnectPose(this._poseJobPlayable, this.playableGraph, this.blendAsset.basePose);
			MagicBlendLibrary.ConnectPose(this._overlayJobPlayable, this.playableGraph, this.blendAsset.overlayPose);
			if (this.blendAsset.isAnimation)
			{
				this._overlayJobPlayable.GetInput(0).SetSpeed(1.0);
			}
			for (int i = 0; i < this._hierarchyMap.Count; i++)
			{
				BlendStreamAtom value = this._atoms[i];
				value.baseWeight = (value.additiveWeight = (value.localWeight = 0f));
				this._atoms[i] = value;
			}
			this._blendedIndexes.Clear();
			foreach (LayeredBlend layeredBlend in this.blendAsset.layeredBlends)
			{
				foreach (KRigElement krigElement in layeredBlend.layer.elementChain)
				{
					int item;
					this._hierarchyMap.TryGetValue(krigElement.name, out item);
					this._blendedIndexes.Add(item);
				}
			}
			this.UpdateBlendWeights();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000D06C File Offset: 0x0000B26C
		protected virtual void BuildMagicMixer()
		{
			if (this._playableMixer.IsValid<AnimationLayerMixerPlayable>())
			{
				this._playableMixer.DisconnectInput(2);
			}
			else
			{
				this._playableMixer = AnimationLayerMixerPlayable.Create(this.playableGraph, 3);
				this.InitializeJobs();
				this._playableMixer.ConnectInput(0, this._poseJobPlayable, 0, 1f);
				this._playableMixer.ConnectInput(1, this._overlayJobPlayable, 0, 1f);
				this._magicBlendOutput.SetSourcePlayable(this._playableMixer);
				this._magicBlendOutput.SetSortingOrder(900);
			}
			this._magicBlendOutput.SetSourcePlayable(this._playableMixer);
			this._magicBlendOutput.SetSortingOrder(900);
			int outputCount = this.playableGraph.GetOutputCount();
			int index = 0;
			for (int i = 0; i < outputCount; i++)
			{
				if (!(this.playableGraph.GetOutput(i).GetSourcePlayable<PlayableOutput>().GetPlayableType() != typeof(AnimatorControllerPlayable)))
				{
					index = i;
				}
			}
			Playable sourcePlayable = this.playableGraph.GetOutput(index).GetSourcePlayable<PlayableOutput>();
			this._layeringJobPlayable.ConnectInput(0, sourcePlayable, 0, 1f);
			this._playableMixer.ConnectInput(2, this._layeringJobPlayable, 0, 1f);
			if (this.blendAsset != null)
			{
				this.UpdateMagicBlendAsset(this.blendAsset, false, -1f, false);
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000D1C4 File Offset: 0x0000B3C4
		protected virtual void InitializeMagicBlending()
		{
			this.playableGraph = this._animator.playableGraph;
			this._atoms = MagicBlendLibrary.SetupBlendAtoms(this._animator, this._rigComponent);
			this._magicBlendOutput = AnimationPlayableOutput.Create(this.playableGraph, "MagicBlendOutput", this._animator);
			this.BuildMagicMixer();
			this.playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			this.playableGraph.Play();
			this._isInitialized = true;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000D23C File Offset: 0x0000B43C
		private void InitializeJobs()
		{
			this._poseJob = new PoseJob
			{
				atoms = this._atoms,
				alwaysAnimate = this.alwaysAnimatePoses,
				readPose = false
			};
			this._poseJobPlayable = AnimationScriptPlayable.Create<PoseJob>(this.playableGraph, this._poseJob, 1);
			this._overlayJob = new OverlayJob
			{
				atoms = this._atoms,
				alwaysAnimate = this.alwaysAnimatePoses,
				cachePose = false
			};
			this._overlayJobPlayable = AnimationScriptPlayable.Create<OverlayJob>(this.playableGraph, this._overlayJob, 1);
			this._layeringJob = new LayeringJob
			{
				atoms = this._atoms,
				blendWeight = 1f,
				cachePose = false
			};
			this._layeringJobPlayable = AnimationScriptPlayable.Create<LayeringJob>(this.playableGraph, this._layeringJob, 1);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000D323 File Offset: 0x0000B523
		private void OnEnable()
		{
			if (this._isInitialized)
			{
				this.BuildMagicMixer();
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000D334 File Offset: 0x0000B534
		private void Start()
		{
			this._animator = base.GetComponent<Animator>();
			this._cachedController = this._animator.runtimeAnimatorController;
			this._rigComponent = base.GetComponentInChildren<KRigComponent>();
			this._hierarchyMap = new Dictionary<string, int>();
			Transform[] rigTransforms = this._rigComponent.GetRigTransforms();
			for (int i = 0; i < rigTransforms.Length; i++)
			{
				this._hierarchyMap.Add(rigTransforms[i].name, i);
			}
			this.InitializeMagicBlending();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000D3AC File Offset: 0x0000B5AC
		protected virtual void UpdateBlendWeights()
		{
			int num = 0;
			foreach (LayeredBlend layeredBlend in this.blendAsset.layeredBlends)
			{
				foreach (KRigElement krigElement in layeredBlend.layer.elementChain)
				{
					int index = this._blendedIndexes[num] + 1;
					BlendStreamAtom value = this._atoms[index];
					value.baseWeight = layeredBlend.baseWeight * this.blendAsset.globalWeight;
					value.additiveWeight = layeredBlend.additiveWeight * this.blendAsset.globalWeight;
					value.localWeight = layeredBlend.localWeight * this.blendAsset.globalWeight;
					this._atoms[index] = value;
					num++;
				}
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000D4C8 File Offset: 0x0000B6C8
		protected virtual void Update()
		{
			RuntimeAnimatorController runtimeAnimatorController = this._animator.runtimeAnimatorController;
			if (this._cachedController != runtimeAnimatorController)
			{
				this.BuildMagicMixer();
			}
			this._cachedController = runtimeAnimatorController;
			if (this.blendAsset == null)
			{
				return;
			}
			if (this.blendAsset.isAnimation)
			{
				Playable input = this._overlayJobPlayable.GetInput(0);
				if (this.blendAsset.overlayPose.isLooping && input.GetTime<Playable>() > (double)this.blendAsset.overlayPose.length)
				{
					input.SetTime(0.0);
				}
			}
			if (this.forceUpdateWeights)
			{
				this.UpdateBlendWeights();
			}
			if (Mathf.Approximately(this._blendPlayback, 1f))
			{
				return;
			}
			this._blendPlayback = Mathf.Clamp01(this._blendPlayback + Time.deltaTime / this._blendTime);
			AnimationCurve blendCurve = this._blendCurve;
			this._layeringJob.blendWeight = ((blendCurve != null) ? blendCurve.Evaluate(this._blendPlayback) : this._blendPlayback);
			this._layeringJobPlayable.SetJobData<LayeringJob>(this._layeringJob);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000D5D8 File Offset: 0x0000B7D8
		protected virtual void LateUpdate()
		{
			if (!this.alwaysAnimatePoses && this._poseJob.readPose)
			{
				this._poseJob.readPose = false;
				this._overlayJob.cachePose = false;
				this._poseJobPlayable.SetJobData<PoseJob>(this._poseJob);
				this._overlayJobPlayable.SetJobData<OverlayJob>(this._overlayJob);
			}
			if (this._layeringJob.cachePose)
			{
				this.SetNewAsset();
				this._blendPlayback = 0f;
				this._layeringJob.cachePose = false;
				this._layeringJob.blendWeight = 0f;
				this._layeringJobPlayable.SetJobData<LayeringJob>(this._layeringJob);
				if (!this.alwaysAnimatePoses)
				{
					this._poseJob.readPose = true;
					this._overlayJob.cachePose = true;
					this._poseJobPlayable.SetJobData<PoseJob>(this._poseJob);
					this._overlayJobPlayable.SetJobData<OverlayJob>(this._overlayJob);
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000D6C4 File Offset: 0x0000B8C4
		protected virtual void OnDestroy()
		{
			if (this.playableGraph.IsValid() && this.playableGraph.IsPlaying())
			{
				this.playableGraph.Stop();
				this.playableGraph.Stop();
			}
			if (this._atoms.IsCreated)
			{
				this._atoms.Dispose();
			}
		}

		// Token: 0x040001E7 RID: 487
		public PlayableGraph playableGraph;

		// Token: 0x040001E8 RID: 488
		[Tooltip("This asset controls the blending weights.")]
		[SerializeField]
		private MagicBlendAsset blendAsset;

		// Token: 0x040001E9 RID: 489
		[Tooltip("Will update weights every frame.")]
		[SerializeField]
		private bool forceUpdateWeights = true;

		// Token: 0x040001EA RID: 490
		[Tooltip("Will process the Overlay pose. Keep it on most of the time.")]
		[SerializeField]
		private bool alwaysAnimatePoses = true;

		// Token: 0x040001EB RID: 491
		private const ushort PlayableSortingPriority = 900;

		// Token: 0x040001EC RID: 492
		private Animator _animator;

		// Token: 0x040001ED RID: 493
		private KRigComponent _rigComponent;

		// Token: 0x040001EE RID: 494
		private AnimationLayerMixerPlayable _playableMixer;

		// Token: 0x040001EF RID: 495
		private NativeArray<BlendStreamAtom> _atoms;

		// Token: 0x040001F0 RID: 496
		private PoseJob _poseJob;

		// Token: 0x040001F1 RID: 497
		private OverlayJob _overlayJob;

		// Token: 0x040001F2 RID: 498
		private LayeringJob _layeringJob;

		// Token: 0x040001F3 RID: 499
		private AnimationScriptPlayable _poseJobPlayable;

		// Token: 0x040001F4 RID: 500
		private AnimationScriptPlayable _overlayJobPlayable;

		// Token: 0x040001F5 RID: 501
		private AnimationScriptPlayable _layeringJobPlayable;

		// Token: 0x040001F6 RID: 502
		private bool _isInitialized;

		// Token: 0x040001F7 RID: 503
		private float _blendPlayback = 1f;

		// Token: 0x040001F8 RID: 504
		private float _blendTime;

		// Token: 0x040001F9 RID: 505
		private AnimationCurve _blendCurve;

		// Token: 0x040001FA RID: 506
		private MagicBlendAsset _desiredBlendAsset;

		// Token: 0x040001FB RID: 507
		private float _desiredBlendTime;

		// Token: 0x040001FC RID: 508
		private bool _useBlendCurve;

		// Token: 0x040001FD RID: 509
		private List<int> _blendedIndexes = new List<int>();

		// Token: 0x040001FE RID: 510
		private Dictionary<string, int> _hierarchyMap;

		// Token: 0x040001FF RID: 511
		private RuntimeAnimatorController _cachedController;

		// Token: 0x04000200 RID: 512
		private AnimationPlayableOutput _magicBlendOutput;
	}
}
