using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x0200000D RID: 13
	public class ControllerState : AnimancerState, ICopyable<ControllerState>
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00003526 File Offset: 0x00001726
		// (set) Token: 0x060000EC RID: 236 RVA: 0x0000352E File Offset: 0x0000172E
		public RuntimeAnimatorController Controller
		{
			get
			{
				return this._Controller;
			}
			set
			{
				base.ChangeMainObject<RuntimeAnimatorController>(ref this._Controller, value);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000ED RID: 237 RVA: 0x0000353D File Offset: 0x0000173D
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00003545 File Offset: 0x00001745
		public override UnityEngine.Object MainObject
		{
			get
			{
				return this.Controller;
			}
			set
			{
				this.Controller = (RuntimeAnimatorController)value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00003553 File Offset: 0x00001753
		public new AnimatorControllerPlayable Playable
		{
			get
			{
				return this._Playable;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000355B File Offset: 0x0000175B
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00003563 File Offset: 0x00001763
		public ControllerState.ActionOnStop[] ActionsOnStop
		{
			get
			{
				return this._ActionsOnStop;
			}
			set
			{
				this._ActionsOnStop = value;
				if (this._Playable.IsValid<AnimatorControllerPlayable>())
				{
					this.GatherDefaultStates();
				}
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000357F File Offset: 0x0000177F
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00003587 File Offset: 0x00001787
		public int[] DefaultStateHashes { get; set; }

		// Token: 0x060000F4 RID: 244 RVA: 0x00003590 File Offset: 0x00001790
		[Conditional("UNITY_ASSERTIONS")]
		public void AssertParameterValue(float value, [CallerMemberName] string parameterName = null)
		{
			if (!value.IsFinite())
			{
				throw new ArgumentOutOfRangeException(parameterName, "must not be NaN or Infinity");
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000035A6 File Offset: 0x000017A6
		public override void CopyIKFlags(AnimancerNode copyFrom)
		{
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000035A8 File Offset: 0x000017A8
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x000035AB File Offset: 0x000017AB
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000035AD File Offset: 0x000017AD
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000035B0 File Offset: 0x000017B0
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000035B2 File Offset: 0x000017B2
		public virtual int ParameterCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000035B5 File Offset: 0x000017B5
		public virtual int GetParameterHash(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000035BC File Offset: 0x000017BC
		public ControllerState(RuntimeAnimatorController controller)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			this._Controller = controller;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000035DF File Offset: 0x000017DF
		public ControllerState(RuntimeAnimatorController controller, params ControllerState.ActionOnStop[] actionsOnStop) : this(controller)
		{
			this._ActionsOnStop = actionsOnStop;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000035F0 File Offset: 0x000017F0
		protected override void CreatePlayable(out Playable playable)
		{
			playable = (this._Playable = AnimatorControllerPlayable.Create(base.Root._Graph, this._Controller));
			this.GatherDefaultStates();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003630 File Offset: 0x00001830
		public override void RecreatePlayable()
		{
			if (!this._Playable.IsValid<AnimatorControllerPlayable>())
			{
				this.CreatePlayable();
				return;
			}
			int parameterCount = this._Playable.GetParameterCount();
			object[] array = new object[parameterCount];
			for (int i = 0; i < parameterCount; i++)
			{
				array[i] = AnimancerUtilities.GetParameterValue(this._Playable, this._Playable.GetParameter(i));
			}
			base.RecreatePlayable();
			for (int j = 0; j < parameterCount; j++)
			{
				AnimancerUtilities.SetParameterValue(this._Playable, this._Playable.GetParameter(j), array[j]);
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000036B6 File Offset: 0x000018B6
		public AnimatorStateInfo GetStateInfo(int layerIndex)
		{
			if (!this._Playable.IsInTransition(layerIndex))
			{
				return this._Playable.GetCurrentAnimatorStateInfo(layerIndex);
			}
			return this._Playable.GetNextAnimatorStateInfo(layerIndex);
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000036E0 File Offset: 0x000018E0
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00003705 File Offset: 0x00001905
		public override double RawTime
		{
			get
			{
				AnimatorStateInfo stateInfo = this.GetStateInfo(0);
				return (double)(stateInfo.normalizedTime * stateInfo.length);
			}
			set
			{
				this._Playable.PlayInFixedTime(0, 0, (float)value);
				if (!base.IsPlaying)
				{
					this._Playable.Play<AnimatorControllerPlayable>();
					AnimancerState.DelayedPause.Register(this);
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00003730 File Offset: 0x00001930
		public override float Length
		{
			get
			{
				return this.GetStateInfo(0).length;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000374C File Offset: 0x0000194C
		public override bool IsLooping
		{
			get
			{
				return this.GetStateInfo(0).loop;
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00003768 File Offset: 0x00001968
		public void GatherDefaultStates()
		{
			int num = this._Playable.GetLayerCount();
			if (this.DefaultStateHashes == null || this.DefaultStateHashes.Length != num)
			{
				this.DefaultStateHashes = new int[num];
			}
			while (--num >= 0)
			{
				this.DefaultStateHashes[num] = this._Playable.GetCurrentAnimatorStateInfo(num).shortNameHash;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000037C6 File Offset: 0x000019C6
		public override void Stop()
		{
			base.Weight = 0f;
			base.IsPlaying = false;
			if (AnimancerState.AutomaticallyClearEvents)
			{
				base.Events = null;
			}
			this.ApplyActionsOnStop();
			if (this._SmoothingVelocities != null)
			{
				this._SmoothingVelocities.Clear();
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003804 File Offset: 0x00001A04
		public void ApplyActionsOnStop()
		{
			int num = Math.Min(this.DefaultStateHashes.Length, this._Playable.GetLayerCount());
			if (this._ActionsOnStop == null || this._ActionsOnStop.Length == 0)
			{
				for (int i = num - 1; i >= 0; i--)
				{
					this._Playable.Play(this.DefaultStateHashes[i], i, 0f);
				}
			}
			else
			{
				for (int j = num - 1; j >= 0; j--)
				{
					int num2 = (j < this._ActionsOnStop.Length) ? j : (this._ActionsOnStop.Length - 1);
					switch (this._ActionsOnStop[num2])
					{
					case ControllerState.ActionOnStop.DefaultState:
						this._Playable.Play(this.DefaultStateHashes[j], j, 0f);
						break;
					case ControllerState.ActionOnStop.RewindTime:
						this._Playable.Play(0, j, 0f);
						break;
					}
				}
			}
			base.CancelSetTime();
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000038DD File Offset: 0x00001ADD
		public override void GatherAnimationClips(ICollection<AnimationClip> clips)
		{
			if (this._Controller != null)
			{
				clips.Gather(this._Controller.animationClips);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000038FE File Offset: 0x00001AFE
		public override void Destroy()
		{
			this._Controller = null;
			base.Destroy();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000390D File Offset: 0x00001B0D
		public override AnimancerState Clone(AnimancerPlayable root)
		{
			ControllerState controllerState = new ControllerState(this._Controller);
			controllerState.SetNewCloneRoot(root);
			((ICopyable<ControllerState>)controllerState).CopyFrom(this);
			return controllerState;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00003928 File Offset: 0x00001B28
		void ICopyable<ControllerState>.CopyFrom(ControllerState copyFrom)
		{
			this._ActionsOnStop = copyFrom._ActionsOnStop;
			if (copyFrom.Root != null && base.Root != null)
			{
				int layerCount = copyFrom._Playable.GetLayerCount();
				for (int i = 0; i < layerCount; i++)
				{
					AnimatorStateInfo currentAnimatorStateInfo = copyFrom._Playable.GetCurrentAnimatorStateInfo(i);
					this._Playable.Play(currentAnimatorStateInfo.shortNameHash, i, currentAnimatorStateInfo.normalizedTime);
				}
				int parameterCount = copyFrom._Playable.GetParameterCount();
				for (int j = 0; j < parameterCount; j++)
				{
					AnimancerUtilities.CopyParameterValue(copyFrom._Playable, this._Playable, copyFrom._Playable.GetParameter(j));
				}
			}
			((ICopyable<AnimancerState>)this).CopyFrom(copyFrom);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000039D4 File Offset: 0x00001BD4
		public static float GetFadeDuration(float fadeDuration)
		{
			if (fadeDuration < 0f)
			{
				return AnimancerPlayable.DefaultFadeDuration;
			}
			return fadeDuration;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000039E8 File Offset: 0x00001BE8
		public void CrossFade(int stateNameHash, float fadeDuration = -1f, int layer = -1, float normalizedTime = float.NegativeInfinity)
		{
			this.Playable.CrossFade(stateNameHash, ControllerState.GetFadeDuration(fadeDuration), layer, normalizedTime);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00003A10 File Offset: 0x00001C10
		public void CrossFade(string stateName, float fadeDuration = -1f, int layer = -1, float normalizedTime = float.NegativeInfinity)
		{
			this.Playable.CrossFade(stateName, ControllerState.GetFadeDuration(fadeDuration), layer, normalizedTime);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00003A38 File Offset: 0x00001C38
		public void CrossFadeInFixedTime(int stateNameHash, float fadeDuration = -1f, int layer = -1, float fixedTime = 0f)
		{
			this.Playable.CrossFadeInFixedTime(stateNameHash, ControllerState.GetFadeDuration(fadeDuration), layer, fixedTime);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003A60 File Offset: 0x00001C60
		public void CrossFadeInFixedTime(string stateName, float fadeDuration = -1f, int layer = -1, float fixedTime = 0f)
		{
			this.Playable.CrossFadeInFixedTime(stateName, ControllerState.GetFadeDuration(fadeDuration), layer, fixedTime);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00003A88 File Offset: 0x00001C88
		public void Play(int stateNameHash, int layer = -1, float normalizedTime = float.NegativeInfinity)
		{
			this.Playable.Play(stateNameHash, layer, normalizedTime);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00003AA8 File Offset: 0x00001CA8
		public void Play(string stateName, int layer = -1, float normalizedTime = float.NegativeInfinity)
		{
			this.Playable.Play(stateName, layer, normalizedTime);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00003AC8 File Offset: 0x00001CC8
		public void PlayInFixedTime(int stateNameHash, int layer = -1, float fixedTime = 0f)
		{
			this.Playable.PlayInFixedTime(stateNameHash, layer, fixedTime);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00003AE8 File Offset: 0x00001CE8
		public void PlayInFixedTime(string stateName, int layer = -1, float fixedTime = 0f)
		{
			this.Playable.PlayInFixedTime(stateName, layer, fixedTime);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00003B08 File Offset: 0x00001D08
		public bool GetBool(int id)
		{
			return this.Playable.GetBool(id);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00003B24 File Offset: 0x00001D24
		public bool GetBool(string name)
		{
			return this.Playable.GetBool(name);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00003B40 File Offset: 0x00001D40
		public void SetBool(int id, bool value)
		{
			this.Playable.SetBool(id, value);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00003B60 File Offset: 0x00001D60
		public void SetBool(string name, bool value)
		{
			this.Playable.SetBool(name, value);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00003B80 File Offset: 0x00001D80
		public float GetFloat(int id)
		{
			return this.Playable.GetFloat(id);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00003B9C File Offset: 0x00001D9C
		public float GetFloat(string name)
		{
			return this.Playable.GetFloat(name);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public void SetFloat(int id, float value)
		{
			this.Playable.SetFloat(id, value);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00003BD8 File Offset: 0x00001DD8
		public void SetFloat(string name, float value)
		{
			this.Playable.SetFloat(name, value);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public int GetInteger(int id)
		{
			return this.Playable.GetInteger(id);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00003C14 File Offset: 0x00001E14
		public int GetInteger(string name)
		{
			return this.Playable.GetInteger(name);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00003C30 File Offset: 0x00001E30
		public void SetInteger(int id, int value)
		{
			this.Playable.SetInteger(id, value);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00003C50 File Offset: 0x00001E50
		public void SetInteger(string name, int value)
		{
			this.Playable.SetInteger(name, value);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00003C70 File Offset: 0x00001E70
		public void SetTrigger(int id)
		{
			this.Playable.SetTrigger(id);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00003C8C File Offset: 0x00001E8C
		public void SetTrigger(string name)
		{
			this.Playable.SetTrigger(name);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00003CA8 File Offset: 0x00001EA8
		public void ResetTrigger(int id)
		{
			this.Playable.ResetTrigger(id);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00003CC4 File Offset: 0x00001EC4
		public void ResetTrigger(string name)
		{
			this.Playable.ResetTrigger(name);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00003CE0 File Offset: 0x00001EE0
		public bool IsParameterControlledByCurve(int id)
		{
			return this.Playable.IsParameterControlledByCurve(id);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00003CFC File Offset: 0x00001EFC
		public bool IsParameterControlledByCurve(string name)
		{
			return this.Playable.IsParameterControlledByCurve(name);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00003D18 File Offset: 0x00001F18
		public AnimatorControllerParameter GetParameter(int index)
		{
			return this.Playable.GetParameter(index);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00003D34 File Offset: 0x00001F34
		public int GetParameterCount()
		{
			return this.Playable.GetParameterCount();
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00003D50 File Offset: 0x00001F50
		public int parameterCount
		{
			get
			{
				return this.Playable.GetParameterCount();
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00003D6C File Offset: 0x00001F6C
		public AnimatorControllerParameter[] parameters
		{
			get
			{
				if (this._Parameters == null)
				{
					int parameterCount = this.GetParameterCount();
					this._Parameters = new AnimatorControllerParameter[parameterCount];
					for (int i = 0; i < parameterCount; i++)
					{
						this._Parameters[i] = this.GetParameter(i);
					}
				}
				return this._Parameters;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00003DB5 File Offset: 0x00001FB5
		public float SetFloat(string name, float value, float dampTime, float deltaTime, float maxSpeed = float.PositiveInfinity)
		{
			return this.SetFloat(Animator.StringToHash(name), value, dampTime, deltaTime, maxSpeed);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00003DCC File Offset: 0x00001FCC
		public float SetFloat(int id, float value, float dampTime, float deltaTime, float maxSpeed = float.PositiveInfinity)
		{
			if (this._SmoothingVelocities == null)
			{
				this._SmoothingVelocities = new Dictionary<int, float>();
			}
			float value2;
			this._SmoothingVelocities.TryGetValue(id, out value2);
			value = Mathf.SmoothDamp(this.GetFloat(id), value, ref value2, dampTime, maxSpeed, deltaTime);
			this.SetFloat(id, value);
			this._SmoothingVelocities[id] = value2;
			return value;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00003E28 File Offset: 0x00002028
		public float GetLayerWeight(int layerIndex)
		{
			return this.Playable.GetLayerWeight(layerIndex);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00003E44 File Offset: 0x00002044
		public void SetLayerWeight(int layerIndex, float weight)
		{
			this.Playable.SetLayerWeight(layerIndex, weight);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00003E64 File Offset: 0x00002064
		public int GetLayerCount()
		{
			return this.Playable.GetLayerCount();
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00003E80 File Offset: 0x00002080
		public int layerCount
		{
			get
			{
				return this.Playable.GetLayerCount();
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00003E9C File Offset: 0x0000209C
		public int GetLayerIndex(string layerName)
		{
			return this.Playable.GetLayerIndex(layerName);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00003EB8 File Offset: 0x000020B8
		public string GetLayerName(int layerIndex)
		{
			return this.Playable.GetLayerName(layerIndex);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00003ED4 File Offset: 0x000020D4
		public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex = 0)
		{
			return this.Playable.GetCurrentAnimatorStateInfo(layerIndex);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00003EF0 File Offset: 0x000020F0
		public AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex = 0)
		{
			return this.Playable.GetNextAnimatorStateInfo(layerIndex);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00003F0C File Offset: 0x0000210C
		public bool HasState(int layerIndex, int stateID)
		{
			return this.Playable.HasState(layerIndex, stateID);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00003F2C File Offset: 0x0000212C
		public bool IsInTransition(int layerIndex = 0)
		{
			return this.Playable.IsInTransition(layerIndex);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00003F48 File Offset: 0x00002148
		public AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex = 0)
		{
			return this.Playable.GetAnimatorTransitionInfo(layerIndex);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00003F64 File Offset: 0x00002164
		public AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex = 0)
		{
			return this.Playable.GetCurrentAnimatorClipInfo(layerIndex);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00003F80 File Offset: 0x00002180
		public void GetCurrentAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			this.Playable.GetCurrentAnimatorClipInfo(layerIndex, clips);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00003FA0 File Offset: 0x000021A0
		public int GetCurrentAnimatorClipInfoCount(int layerIndex = 0)
		{
			return this.Playable.GetCurrentAnimatorClipInfoCount(layerIndex);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00003FBC File Offset: 0x000021BC
		public AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex = 0)
		{
			return this.Playable.GetNextAnimatorClipInfo(layerIndex);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00003FD8 File Offset: 0x000021D8
		public void GetNextAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			this.Playable.GetNextAnimatorClipInfo(layerIndex, clips);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00003FF8 File Offset: 0x000021F8
		public int GetNextAnimatorClipInfoCount(int layerIndex = 0)
		{
			return this.Playable.GetNextAnimatorClipInfoCount(layerIndex);
		}

		// Token: 0x04000012 RID: 18
		private RuntimeAnimatorController _Controller;

		// Token: 0x04000013 RID: 19
		private new AnimatorControllerPlayable _Playable;

		// Token: 0x04000014 RID: 20
		private ControllerState.ActionOnStop[] _ActionsOnStop;

		// Token: 0x04000016 RID: 22
		public const float DefaultFadeDuration = -1f;

		// Token: 0x04000017 RID: 23
		private AnimatorControllerParameter[] _Parameters;

		// Token: 0x04000018 RID: 24
		private Dictionary<int, float> _SmoothingVelocities;

		// Token: 0x0200007E RID: 126
		public interface ITransition : ITransition<ControllerState>, Animancer.ITransition, IHasKey, IPolymorphic
		{
		}

		// Token: 0x0200007F RID: 127
		public enum ActionOnStop
		{
			// Token: 0x04000110 RID: 272
			DefaultState,
			// Token: 0x04000111 RID: 273
			RewindTime,
			// Token: 0x04000112 RID: 274
			Continue
		}

		// Token: 0x02000080 RID: 128
		public class DampedFloatParameter
		{
			// Token: 0x060005BD RID: 1469 RVA: 0x0000F400 File Offset: 0x0000D600
			public DampedFloatParameter(ControllerState.ParameterID parameter, float smoothTime, float defaultValue = 0f, float maxSpeed = float.PositiveInfinity)
			{
				this.parameter = parameter;
				this.smoothTime = smoothTime;
				this.targetValue = defaultValue;
				this.currentValue = defaultValue;
				this.maxSpeed = maxSpeed;
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x0000F439 File Offset: 0x0000D639
			public void Apply(ControllerState controller)
			{
				this.Apply(controller, UnityEngine.Time.deltaTime);
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x0000F448 File Offset: 0x0000D648
			public void Apply(ControllerState controller, float deltaTime)
			{
				this.currentValue = Mathf.SmoothDamp(this.currentValue, this.targetValue, ref this.velocity, this.smoothTime, this.maxSpeed, deltaTime);
				controller.SetFloat(this.parameter, this.currentValue);
			}

			// Token: 0x04000113 RID: 275
			public ControllerState.ParameterID parameter;

			// Token: 0x04000114 RID: 276
			public float smoothTime;

			// Token: 0x04000115 RID: 277
			public float currentValue;

			// Token: 0x04000116 RID: 278
			public float targetValue;

			// Token: 0x04000117 RID: 279
			public float maxSpeed;

			// Token: 0x04000118 RID: 280
			public float velocity;
		}

		// Token: 0x02000081 RID: 129
		public readonly struct ParameterID
		{
			// Token: 0x060005C0 RID: 1472 RVA: 0x0000F496 File Offset: 0x0000D696
			public ParameterID(string name)
			{
				this.Name = name;
				this.Hash = Animator.StringToHash(name);
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x0000F4AB File Offset: 0x0000D6AB
			public ParameterID(int hash)
			{
				this.Name = null;
				this.Hash = hash;
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x0000F4BB File Offset: 0x0000D6BB
			public ParameterID(string name, int hash)
			{
				this.Name = name;
				this.Hash = hash;
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x0000F4CB File Offset: 0x0000D6CB
			public static implicit operator ControllerState.ParameterID(string name)
			{
				return new ControllerState.ParameterID(name);
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x0000F4D3 File Offset: 0x0000D6D3
			public static implicit operator ControllerState.ParameterID(int hash)
			{
				return new ControllerState.ParameterID(hash);
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x0000F4DB File Offset: 0x0000D6DB
			public static implicit operator int(ControllerState.ParameterID parameter)
			{
				return parameter.Hash;
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x0000F4E3 File Offset: 0x0000D6E3
			[Conditional("UNITY_EDITOR")]
			public void ValidateHasParameter(RuntimeAnimatorController controller, AnimatorControllerParameterType type)
			{
			}

			// Token: 0x060005C7 RID: 1479 RVA: 0x0000F4E5 File Offset: 0x0000D6E5
			public override string ToString()
			{
				return "ControllerState.ParameterID(Name: '" + this.Name + "'" + string.Format(", {0}: {1})", "Hash", this.Hash);
			}

			// Token: 0x04000119 RID: 281
			public readonly string Name;

			// Token: 0x0400011A RID: 282
			public readonly int Hash;
		}
	}
}
