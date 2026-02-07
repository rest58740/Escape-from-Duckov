using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000004 RID: 4
	[AddComponentMenu("Animancer/Hybrid Animancer Component")]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/HybridAnimancerComponent")]
	public class HybridAnimancerComponent : NamedAnimancerComponent
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000024D6 File Offset: 0x000006D6
		public ref ControllerTransition Controller
		{
			get
			{
				return ref this._Controller;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000024DE File Offset: 0x000006DE
		public ControllerState PlayController()
		{
			if (!this._Controller.IsValid())
			{
				return null;
			}
			base.Play(this._Controller);
			return this._Controller.State;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002507 File Offset: 0x00000707
		public AnimatorControllerPlayable ControllerPlayable
		{
			get
			{
				return this._Controller.State.Playable;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002519 File Offset: 0x00000719
		protected override void OnEnable()
		{
			if (!base.TryGetAnimator())
			{
				return;
			}
			this.PlayController();
			base.OnEnable();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002531 File Offset: 0x00000731
		protected override void OnInitializePlayable()
		{
			base.OnInitializePlayable();
			base.Playable.KeepChildrenConnected = true;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002545 File Offset: 0x00000745
		public override void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			base.GatherAnimationClips(clips);
			clips.GatherFromSource(this._Controller);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000255B File Offset: 0x0000075B
		public PlayableGraph playableGraph
		{
			get
			{
				return base.Playable.Graph;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002568 File Offset: 0x00000768
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002577 File Offset: 0x00000777
		public unsafe RuntimeAnimatorController runtimeAnimatorController
		{
			get
			{
				return *this.Controller->Controller;
			}
			set
			{
				*this.Controller->Controller = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002587 File Offset: 0x00000787
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002594 File Offset: 0x00000794
		public float speed
		{
			get
			{
				return base.Animator.speed;
			}
			set
			{
				base.Animator.speed = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000025A2 File Offset: 0x000007A2
		// (set) Token: 0x0600003E RID: 62 RVA: 0x000025AF File Offset: 0x000007AF
		public bool applyRootMotion
		{
			get
			{
				return base.Animator.applyRootMotion;
			}
			set
			{
				base.Animator.applyRootMotion = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000025BD File Offset: 0x000007BD
		// (set) Token: 0x06000040 RID: 64 RVA: 0x000025CA File Offset: 0x000007CA
		public Quaternion bodyRotation
		{
			get
			{
				return base.Animator.bodyRotation;
			}
			set
			{
				base.Animator.bodyRotation = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000025D8 File Offset: 0x000007D8
		// (set) Token: 0x06000042 RID: 66 RVA: 0x000025E5 File Offset: 0x000007E5
		public Vector3 bodyPosition
		{
			get
			{
				return base.Animator.bodyPosition;
			}
			set
			{
				base.Animator.bodyPosition = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000025F3 File Offset: 0x000007F3
		public float gravityWeight
		{
			get
			{
				return base.Animator.gravityWeight;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002600 File Offset: 0x00000800
		public bool hasRootMotion
		{
			get
			{
				return base.Animator.hasRootMotion;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000260D File Offset: 0x0000080D
		// (set) Token: 0x06000046 RID: 70 RVA: 0x0000261A File Offset: 0x0000081A
		public bool layersAffectMassCenter
		{
			get
			{
				return base.Animator.layersAffectMassCenter;
			}
			set
			{
				base.Animator.layersAffectMassCenter = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002628 File Offset: 0x00000828
		public Vector3 pivotPosition
		{
			get
			{
				return base.Animator.pivotPosition;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002635 File Offset: 0x00000835
		public float pivotWeight
		{
			get
			{
				return base.Animator.pivotWeight;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002642 File Offset: 0x00000842
		// (set) Token: 0x0600004A RID: 74 RVA: 0x0000264F File Offset: 0x0000084F
		public Quaternion rootRotation
		{
			get
			{
				return base.Animator.rootRotation;
			}
			set
			{
				base.Animator.rootRotation = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000265D File Offset: 0x0000085D
		// (set) Token: 0x0600004C RID: 76 RVA: 0x0000266A File Offset: 0x0000086A
		public Vector3 rootPosition
		{
			get
			{
				return base.Animator.rootPosition;
			}
			set
			{
				base.Animator.rootPosition = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002678 File Offset: 0x00000878
		public Vector3 angularVelocity
		{
			get
			{
				return base.Animator.angularVelocity;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002685 File Offset: 0x00000885
		public Vector3 velocity
		{
			get
			{
				return base.Animator.velocity;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002692 File Offset: 0x00000892
		public Quaternion deltaRotation
		{
			get
			{
				return base.Animator.deltaRotation;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000269F File Offset: 0x0000089F
		public Vector3 deltaPosition
		{
			get
			{
				return base.Animator.deltaPosition;
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000026AC File Offset: 0x000008AC
		public void ApplyBuiltinRootMotion()
		{
			base.Animator.ApplyBuiltinRootMotion();
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000026B9 File Offset: 0x000008B9
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000026C6 File Offset: 0x000008C6
		public float feetPivotActive
		{
			get
			{
				return base.Animator.feetPivotActive;
			}
			set
			{
				base.Animator.feetPivotActive = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000026D4 File Offset: 0x000008D4
		// (set) Token: 0x06000055 RID: 85 RVA: 0x000026E1 File Offset: 0x000008E1
		public bool stabilizeFeet
		{
			get
			{
				return base.Animator.stabilizeFeet;
			}
			set
			{
				base.Animator.stabilizeFeet = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000026EF File Offset: 0x000008EF
		public float rightFeetBottomHeight
		{
			get
			{
				return base.Animator.rightFeetBottomHeight;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000026FC File Offset: 0x000008FC
		public float leftFeetBottomHeight
		{
			get
			{
				return base.Animator.leftFeetBottomHeight;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000270C File Offset: 0x0000090C
		public void CrossFade(int stateNameHash, float fadeDuration = -1f, int layer = -1, float normalizedTime = float.NegativeInfinity)
		{
			fadeDuration = ControllerState.GetFadeDuration(fadeDuration);
			this.PlayController().Playable.CrossFade(stateNameHash, fadeDuration, layer, normalizedTime);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000273C File Offset: 0x0000093C
		public AnimancerState CrossFade(string stateName, float fadeDuration = -1f, int layer = -1, float normalizedTime = float.NegativeInfinity)
		{
			fadeDuration = ControllerState.GetFadeDuration(fadeDuration);
			AnimancerState animancerState;
			if (base.States.TryGet(base.name, out animancerState))
			{
				base.Play(animancerState, fadeDuration, FadeMode.FixedSpeed);
				if (layer >= 0)
				{
					animancerState.LayerIndex = layer;
				}
				if (normalizedTime != float.NegativeInfinity)
				{
					animancerState.NormalizedTime = normalizedTime;
				}
				return animancerState;
			}
			ControllerState controllerState = this.PlayController();
			controllerState.Playable.CrossFade(stateName, fadeDuration, layer, normalizedTime);
			return controllerState;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000027A8 File Offset: 0x000009A8
		public void CrossFadeInFixedTime(int stateNameHash, float fadeDuration = -1f, int layer = -1, float fixedTime = 0f)
		{
			fadeDuration = ControllerState.GetFadeDuration(fadeDuration);
			this.PlayController().Playable.CrossFadeInFixedTime(stateNameHash, fadeDuration, layer, fixedTime);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000027D8 File Offset: 0x000009D8
		public AnimancerState CrossFadeInFixedTime(string stateName, float fadeDuration = -1f, int layer = -1, float fixedTime = 0f)
		{
			fadeDuration = ControllerState.GetFadeDuration(fadeDuration);
			AnimancerState animancerState;
			if (base.States.TryGet(base.name, out animancerState))
			{
				base.Play(animancerState, fadeDuration, FadeMode.FixedSpeed);
				if (layer >= 0)
				{
					animancerState.LayerIndex = layer;
				}
				animancerState.Time = fixedTime;
				return animancerState;
			}
			ControllerState controllerState = this.PlayController();
			controllerState.Playable.CrossFadeInFixedTime(stateName, fadeDuration, layer, fixedTime);
			return controllerState;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000283C File Offset: 0x00000A3C
		public void Play(int stateNameHash, int layer = -1, float normalizedTime = float.NegativeInfinity)
		{
			this.PlayController().Playable.Play(stateNameHash, layer, normalizedTime);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002860 File Offset: 0x00000A60
		public AnimancerState Play(string stateName, int layer = -1, float normalizedTime = float.NegativeInfinity)
		{
			AnimancerState animancerState;
			if (base.States.TryGet(base.name, out animancerState))
			{
				base.Play(animancerState);
				if (layer >= 0)
				{
					animancerState.LayerIndex = layer;
				}
				if (normalizedTime != float.NegativeInfinity)
				{
					animancerState.NormalizedTime = normalizedTime;
				}
				return animancerState;
			}
			ControllerState controllerState = this.PlayController();
			controllerState.Playable.Play(stateName, layer, normalizedTime);
			return controllerState;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000028C0 File Offset: 0x00000AC0
		public void PlayInFixedTime(int stateNameHash, int layer = -1, float fixedTime = 0f)
		{
			this.PlayController().Playable.PlayInFixedTime(stateNameHash, layer, fixedTime);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000028E4 File Offset: 0x00000AE4
		public AnimancerState PlayInFixedTime(string stateName, int layer = -1, float fixedTime = 0f)
		{
			AnimancerState animancerState;
			if (base.States.TryGet(base.name, out animancerState))
			{
				base.Play(animancerState);
				if (layer >= 0)
				{
					animancerState.LayerIndex = layer;
				}
				animancerState.Time = fixedTime;
				return animancerState;
			}
			ControllerState controllerState = this.PlayController();
			controllerState.Playable.PlayInFixedTime(stateName, layer, fixedTime);
			return controllerState;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000293C File Offset: 0x00000B3C
		public bool GetBool(int id)
		{
			return this.ControllerPlayable.GetBool(id);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002958 File Offset: 0x00000B58
		public bool GetBool(string name)
		{
			return this.ControllerPlayable.GetBool(name);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002974 File Offset: 0x00000B74
		public void SetBool(int id, bool value)
		{
			this.ControllerPlayable.SetBool(id, value);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002994 File Offset: 0x00000B94
		public void SetBool(string name, bool value)
		{
			this.ControllerPlayable.SetBool(name, value);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000029B4 File Offset: 0x00000BB4
		public float GetFloat(int id)
		{
			return this.ControllerPlayable.GetFloat(id);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000029D0 File Offset: 0x00000BD0
		public float GetFloat(string name)
		{
			return this.ControllerPlayable.GetFloat(name);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000029EC File Offset: 0x00000BEC
		public void SetFloat(int id, float value)
		{
			this.ControllerPlayable.SetFloat(id, value);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002A0C File Offset: 0x00000C0C
		public void SetFloat(string name, float value)
		{
			this.ControllerPlayable.SetFloat(name, value);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002A29 File Offset: 0x00000C29
		public float SetFloat(string name, float value, float dampTime, float deltaTime, float maxSpeed = float.PositiveInfinity)
		{
			return this._Controller.State.SetFloat(name, value, dampTime, deltaTime, maxSpeed);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002A42 File Offset: 0x00000C42
		public float SetFloat(int id, float value, float dampTime, float deltaTime, float maxSpeed = float.PositiveInfinity)
		{
			return this._Controller.State.SetFloat(base.name, value, dampTime, deltaTime, maxSpeed);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002A60 File Offset: 0x00000C60
		public int GetInteger(int id)
		{
			return this.ControllerPlayable.GetInteger(id);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002A7C File Offset: 0x00000C7C
		public int GetInteger(string name)
		{
			return this.ControllerPlayable.GetInteger(name);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002A98 File Offset: 0x00000C98
		public void SetInteger(int id, int value)
		{
			this.ControllerPlayable.SetInteger(id, value);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public void SetInteger(string name, int value)
		{
			this.ControllerPlayable.SetInteger(name, value);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public void SetTrigger(int id)
		{
			this.ControllerPlayable.SetTrigger(id);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public void SetTrigger(string name)
		{
			this.ControllerPlayable.SetTrigger(name);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002B10 File Offset: 0x00000D10
		public void ResetTrigger(int id)
		{
			this.ControllerPlayable.ResetTrigger(id);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002B2C File Offset: 0x00000D2C
		public void ResetTrigger(string name)
		{
			this.ControllerPlayable.ResetTrigger(name);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002B48 File Offset: 0x00000D48
		public bool IsParameterControlledByCurve(int id)
		{
			return this.ControllerPlayable.IsParameterControlledByCurve(id);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002B64 File Offset: 0x00000D64
		public bool IsParameterControlledByCurve(string name)
		{
			return this.ControllerPlayable.IsParameterControlledByCurve(name);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002B80 File Offset: 0x00000D80
		public AnimatorControllerParameter GetParameter(int index)
		{
			return this.ControllerPlayable.GetParameter(index);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002B9C File Offset: 0x00000D9C
		public int GetParameterCount()
		{
			return this.ControllerPlayable.GetParameterCount();
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public int parameterCount
		{
			get
			{
				return this.ControllerPlayable.GetParameterCount();
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002BD3 File Offset: 0x00000DD3
		public AnimatorControllerParameter[] parameters
		{
			get
			{
				return this._Controller.State.parameters;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex = 0)
		{
			return this.ControllerPlayable.GetCurrentAnimatorClipInfo(layerIndex);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002C04 File Offset: 0x00000E04
		public void GetCurrentAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			this.ControllerPlayable.GetCurrentAnimatorClipInfo(layerIndex, clips);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002C24 File Offset: 0x00000E24
		public int GetCurrentAnimatorClipInfoCount(int layerIndex = 0)
		{
			return this.ControllerPlayable.GetCurrentAnimatorClipInfoCount(layerIndex);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002C40 File Offset: 0x00000E40
		public AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex = 0)
		{
			return this.ControllerPlayable.GetNextAnimatorClipInfo(layerIndex);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002C5C File Offset: 0x00000E5C
		public void GetNextAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			this.ControllerPlayable.GetNextAnimatorClipInfo(layerIndex, clips);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002C7C File Offset: 0x00000E7C
		public int GetNextAnimatorClipInfoCount(int layerIndex = 0)
		{
			return this.ControllerPlayable.GetNextAnimatorClipInfoCount(layerIndex);
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002C98 File Offset: 0x00000E98
		public float humanScale
		{
			get
			{
				return base.Animator.humanScale;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002CA5 File Offset: 0x00000EA5
		public bool isHuman
		{
			get
			{
				return base.Animator.isHuman;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002CB2 File Offset: 0x00000EB2
		public Transform GetBoneTransform(HumanBodyBones humanBoneId)
		{
			return base.Animator.GetBoneTransform(humanBoneId);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public void SetBoneLocalRotation(HumanBodyBones humanBoneId, Quaternion rotation)
		{
			base.Animator.SetBoneLocalRotation(humanBoneId, rotation);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002CD0 File Offset: 0x00000ED0
		public int GetLayerCount()
		{
			return this.ControllerPlayable.GetLayerCount();
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00002CEC File Offset: 0x00000EEC
		public int layerCount
		{
			get
			{
				return this.ControllerPlayable.GetLayerCount();
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002D08 File Offset: 0x00000F08
		public int GetLayerIndex(string layerName)
		{
			return this.ControllerPlayable.GetLayerIndex(layerName);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002D24 File Offset: 0x00000F24
		public string GetLayerName(int layerIndex)
		{
			return this.ControllerPlayable.GetLayerName(layerIndex);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002D40 File Offset: 0x00000F40
		public float GetLayerWeight(int layerIndex)
		{
			return this.ControllerPlayable.GetLayerWeight(layerIndex);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002D5C File Offset: 0x00000F5C
		public void SetLayerWeight(int layerIndex, float weight)
		{
			this.ControllerPlayable.SetLayerWeight(layerIndex, weight);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002D79 File Offset: 0x00000F79
		public T GetBehaviour<T>() where T : StateMachineBehaviour
		{
			return base.Animator.GetBehaviour<T>();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002D86 File Offset: 0x00000F86
		public T[] GetBehaviours<T>() where T : StateMachineBehaviour
		{
			return base.Animator.GetBehaviours<T>();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002D93 File Offset: 0x00000F93
		public StateMachineBehaviour[] GetBehaviours(int fullPathHash, int layerIndex)
		{
			return base.Animator.GetBehaviours(fullPathHash, layerIndex);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex = 0)
		{
			return this.ControllerPlayable.GetCurrentAnimatorStateInfo(layerIndex);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex = 0)
		{
			return this.ControllerPlayable.GetNextAnimatorStateInfo(layerIndex);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002DDC File Offset: 0x00000FDC
		public bool HasState(int layerIndex, int stateID)
		{
			return this.ControllerPlayable.HasState(layerIndex, stateID);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002DFC File Offset: 0x00000FFC
		public bool IsInTransition(int layerIndex = 0)
		{
			return this.ControllerPlayable.IsInTransition(layerIndex);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002E18 File Offset: 0x00001018
		public AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex = 0)
		{
			return this.ControllerPlayable.GetAnimatorTransitionInfo(layerIndex);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00002E34 File Offset: 0x00001034
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00002E41 File Offset: 0x00001041
		public Avatar avatar
		{
			get
			{
				return base.Animator.avatar;
			}
			set
			{
				base.Animator.avatar = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00002E4F File Offset: 0x0000104F
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00002E5C File Offset: 0x0000105C
		public AnimatorCullingMode cullingMode
		{
			get
			{
				return base.Animator.cullingMode;
			}
			set
			{
				base.Animator.cullingMode = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00002E6A File Offset: 0x0000106A
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00002E77 File Offset: 0x00001077
		public bool fireEvents
		{
			get
			{
				return base.Animator.fireEvents;
			}
			set
			{
				base.Animator.fireEvents = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00002E85 File Offset: 0x00001085
		public bool hasBoundPlayables
		{
			get
			{
				return base.Animator.hasBoundPlayables;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00002E92 File Offset: 0x00001092
		public bool hasTransformHierarchy
		{
			get
			{
				return base.Animator.hasTransformHierarchy;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00002E9F File Offset: 0x0000109F
		public bool isInitialized
		{
			get
			{
				return base.Animator.isInitialized;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00002EAC File Offset: 0x000010AC
		public bool isOptimizable
		{
			get
			{
				return base.Animator.isOptimizable;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00002EB9 File Offset: 0x000010B9
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00002EC6 File Offset: 0x000010C6
		public bool logWarnings
		{
			get
			{
				return base.Animator.logWarnings;
			}
			set
			{
				base.Animator.logWarnings = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00002ED4 File Offset: 0x000010D4
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00002EE1 File Offset: 0x000010E1
		public AnimatorUpdateMode updateMode
		{
			get
			{
				return base.Animator.updateMode;
			}
			set
			{
				base.Animator.updateMode = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00002EEF File Offset: 0x000010EF
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00002EFC File Offset: 0x000010FC
		public bool keepAnimatorStateOnDisable
		{
			get
			{
				return base.Animator.keepAnimatorStateOnDisable;
			}
			set
			{
				base.Animator.keepAnimatorStateOnDisable = value;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002F0A File Offset: 0x0000110A
		public void Rebind()
		{
			base.Animator.Rebind();
		}

		// Token: 0x04000005 RID: 5
		[SerializeField]
		[Tooltip("The main Animator Controller that this object will play")]
		private ControllerTransition _Controller;
	}
}
