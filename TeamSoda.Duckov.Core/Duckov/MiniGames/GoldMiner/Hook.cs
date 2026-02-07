using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A2 RID: 674
	public class Hook : MiniGameBehaviour
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600164A RID: 5706 RVA: 0x00053043 File Offset: 0x00051243
		public Transform Axis
		{
			get
			{
				return this.hookAxis;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600164B RID: 5707 RVA: 0x0005304B File Offset: 0x0005124B
		public Hook.HookStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600164C RID: 5708 RVA: 0x00053053 File Offset: 0x00051253
		private float RopeDistance
		{
			get
			{
				return Mathf.Lerp(this.minDist, this.maxDist, this.ropeControl);
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600164D RID: 5709 RVA: 0x0005306C File Offset: 0x0005126C
		private float AxisAngle
		{
			get
			{
				return Mathf.Lerp(-this.maxAngle, this.maxAngle, (this.axisControl + 1f) / 2f);
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x0600164E RID: 5710 RVA: 0x00053094 File Offset: 0x00051294
		private bool RopeOutOfBound
		{
			get
			{
				Vector3 vector = Quaternion.Euler(0f, 0f, this.AxisAngle) * Vector2.down * this.RopeDistance;
				return !this.bounds.Contains(vector);
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x0600164F RID: 5711 RVA: 0x000530E0 File Offset: 0x000512E0
		// (set) Token: 0x06001650 RID: 5712 RVA: 0x000530E8 File Offset: 0x000512E8
		public GoldMinerEntity GrabbingTarget
		{
			get
			{
				return this._grabbingTarget;
			}
			private set
			{
				this._grabbingTarget = value;
			}
		}

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06001651 RID: 5713 RVA: 0x000530F4 File Offset: 0x000512F4
		// (remove) Token: 0x06001652 RID: 5714 RVA: 0x0005312C File Offset: 0x0005132C
		public event Action<Hook, GoldMinerEntity> OnResolveTarget;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06001653 RID: 5715 RVA: 0x00053164 File Offset: 0x00051364
		// (remove) Token: 0x06001654 RID: 5716 RVA: 0x0005319C File Offset: 0x0005139C
		public event Action<Hook> OnLaunch;

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06001655 RID: 5717 RVA: 0x000531D4 File Offset: 0x000513D4
		// (remove) Token: 0x06001656 RID: 5718 RVA: 0x0005320C File Offset: 0x0005140C
		public event Action<Hook> OnBeginRetrieve;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06001657 RID: 5719 RVA: 0x00053244 File Offset: 0x00051444
		// (remove) Token: 0x06001658 RID: 5720 RVA: 0x0005327C File Offset: 0x0005147C
		public event Action<Hook, GoldMinerEntity> OnAttach;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06001659 RID: 5721 RVA: 0x000532B4 File Offset: 0x000514B4
		// (remove) Token: 0x0600165A RID: 5722 RVA: 0x000532EC File Offset: 0x000514EC
		public event Action<Hook> OnEndRetrieve;

		// Token: 0x0600165B RID: 5723 RVA: 0x00053321 File Offset: 0x00051521
		public void SetParameters(float swingFreqFactor, float emptySpeed, float strength)
		{
			this.swingFreqFactor = swingFreqFactor;
			this.emptySpeed = emptySpeed;
			this.strength = strength;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00053338 File Offset: 0x00051538
		public void Tick(float deltaTime)
		{
			this.UpdateStatus(deltaTime);
			this.UpdateHookHeadPosition();
			this.UpdateAxis();
			this.ropeLineRenderer.SetPositions(new Vector3[]
			{
				this.hookAxis.transform.position,
				this.hookHead.transform.position
			});
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00053397 File Offset: 0x00051597
		private void UpdateHookHeadPosition()
		{
			this.hookHead.transform.localPosition = this.GetHookHeadPosition(this.RopeDistance);
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x000533B5 File Offset: 0x000515B5
		private Vector3 GetHookHeadPosition(float ropeDistance)
		{
			return -Vector3.up * this.RopeDistance;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x000533CC File Offset: 0x000515CC
		private void UpdateAxis()
		{
			this.hookAxis.transform.localRotation = Quaternion.Euler(0f, 0f, this.AxisAngle);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x000533F3 File Offset: 0x000515F3
		private void OnValidate()
		{
			this.UpdateHookHeadPosition();
			this.UpdateAxis();
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00053404 File Offset: 0x00051604
		private void UpdateStatus(float deltaTime)
		{
			switch (this.status)
			{
			case Hook.HookStatus.Idle:
				break;
			case Hook.HookStatus.Swinging:
				this.UpdateSwinging(deltaTime);
				this.UpdateClaw();
				return;
			case Hook.HookStatus.Launching:
				this.UpdateClaw();
				this.UpdateLaunching(deltaTime);
				return;
			case Hook.HookStatus.Attaching:
				this.UpdateAttaching(deltaTime);
				return;
			case Hook.HookStatus.Retrieving:
				this.UpdateRetreving(deltaTime);
				this.UpdateClaw();
				return;
			case Hook.HookStatus.Retrieved:
				this.UpdateRetrieved();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0005346F File Offset: 0x0005166F
		public void Launch()
		{
			if (this.status != Hook.HookStatus.Swinging)
			{
				return;
			}
			this.EnterStatus(Hook.HookStatus.Launching);
			Action<Hook> onLaunch = this.OnLaunch;
			if (onLaunch == null)
			{
				return;
			}
			onLaunch(this);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00053493 File Offset: 0x00051693
		public void Reset()
		{
			this.ropeControl = 0f;
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x000534A0 File Offset: 0x000516A0
		private void UpdateClaw()
		{
			this.clawAnimator.SetBool("Grabbing", this.GrabbingTarget);
			if (!this.GrabbingTarget)
			{
				this.claw.localRotation = Quaternion.Euler(0f, 0f, -180f);
				this.claw.localPosition = Vector3.zero;
				return;
			}
			Vector2 vector = this.GrabbingTarget.transform.position - this.hookHead.transform.position;
			this.claw.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, vector));
			this.claw.position = this.hookHead.transform.position + vector.normalized * this.clawOffset;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0005358C File Offset: 0x0005178C
		private void UpdateSwinging(float deltaTime)
		{
			this.t += deltaTime * 90f * this.swingFreqFactor * 0.017453292f;
			this.axisControl = Mathf.Sin(this.t);
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x000535C0 File Offset: 0x000517C0
		private void UpdateLaunching(float deltaTime)
		{
			float num = this.emptySpeed;
			if (this.GrabbingTarget != null)
			{
				num = this.GrabbingTarget.Speed;
			}
			float num2 = (100f + this.strength) / 100f;
			num *= num2;
			float num3 = num * deltaTime / (this.maxDist - this.minDist);
			Vector3 hookHeadPosition = this.GetHookHeadPosition(this.RopeDistance);
			this.ropeControl = Mathf.MoveTowards(this.ropeControl, 1f, num3);
			this.GetHookHeadPosition(this.RopeDistance);
			Vector3 oldWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			Vector3 newWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			if (this.RopeOutOfBound || this.ropeControl >= 1f)
			{
				this.EnterStatus(Hook.HookStatus.Retrieving);
			}
			this.CheckGrab(oldWorldPos, newWorldPos);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0005369C File Offset: 0x0005189C
		private void CheckGrab(Vector3 oldWorldPos, Vector3 newWorldPos)
		{
			if (this.GrabbingTarget)
			{
				return;
			}
			Vector3 vector = newWorldPos - oldWorldPos;
			foreach (RaycastHit2D raycastHit2D in Physics2D.CircleCastAll(oldWorldPos, 8f, vector.normalized, vector.magnitude))
			{
				if (!(raycastHit2D.collider == null))
				{
					GoldMinerEntity component = raycastHit2D.collider.gameObject.GetComponent<GoldMinerEntity>();
					if (!(component == null))
					{
						this.Grab(component);
						return;
					}
				}
			}
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x00053730 File Offset: 0x00051930
		private void Grab(GoldMinerEntity target)
		{
			this.GrabbingTarget = target;
			this.EnterStatus(Hook.HookStatus.Attaching);
			this.relativePos = target.transform.position - this.hookHead.transform.position;
			this.targetDist = this.relativePos.magnitude;
			this.targetRelativeRotation = Quaternion.FromToRotation(this.relativePos, this.GrabbingTarget.transform.up);
			this.retrieveETA = this.grabAnimationTime;
			Vector2 vector = this.GrabbingTarget.transform.position - this.hookHead.transform.position;
			Vector3 vector2;
			vector2..ctor(0f, 0f, Vector2.SignedAngle(Vector2.up, vector));
			Vector3 vector3 = this.hookHead.transform.position + vector.normalized * this.clawOffset;
			TweenSettingsExtensions.SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(ShortcutExtensions.DORotate(this.claw, vector2, this.retrieveETA, 0), this.grabAnimationEase);
			TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOMove(this.claw, vector3, this.retrieveETA, false), this.grabAnimationEase);
			this.clawAnimator.SetBool("Grabbing", this.GrabbingTarget);
			this.GrabbingTarget.NotifyAttached(this);
			Action<Hook, GoldMinerEntity> onAttach = this.OnAttach;
			if (onAttach == null)
			{
				return;
			}
			onAttach(this, target);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000538A4 File Offset: 0x00051AA4
		private void UpdateAttaching(float deltaTime)
		{
			if (this.GrabbingTarget == null)
			{
				this.EnterStatus(Hook.HookStatus.Retrieving);
				return;
			}
			this.retrieveETA -= deltaTime;
			if (this.retrieveETA <= 0f)
			{
				this.EnterStatus(Hook.HookStatus.Retrieving);
			}
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x000538E0 File Offset: 0x00051AE0
		private void UpdateRetreving(float deltaTime)
		{
			float num = this.emptySpeed;
			if (this.GrabbingTarget != null)
			{
				num = this.GrabbingTarget.Speed;
			}
			float num2 = (100f + this.strength) / 100f;
			num *= num2;
			float num3 = num * deltaTime / (this.maxDist - this.minDist);
			this.maxDeltaWatch = num3;
			Vector3 hookHeadPosition = this.GetHookHeadPosition(this.RopeDistance);
			this.ropeControl = Mathf.MoveTowards(this.ropeControl, 0f, num3);
			this.GetHookHeadPosition(this.RopeDistance);
			Vector3 oldWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			Vector3 newWorldPos = this.hookAxis.localToWorldMatrix.MultiplyPoint(hookHeadPosition);
			if (this.ropeControl <= 0f)
			{
				this.ropeControl = 0f;
				this.EnterStatus(Hook.HookStatus.Retrieved);
			}
			if (this.GrabbingTarget)
			{
				Vector3 vector = this.GrabbingTarget.transform.position - this.hookHead.transform.position;
				if (vector.magnitude > this.targetDist)
				{
					this.GrabbingTarget.transform.position = this.hookHead.transform.position + vector.normalized * this.targetDist;
					Vector3 vector2 = this.targetRelativeRotation * vector;
					this.GrabbingTarget.transform.rotation = Quaternion.FromToRotation(Vector3.up, vector2);
					return;
				}
			}
			else
			{
				this.CheckGrab(oldWorldPos, newWorldPos);
			}
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00053A6F File Offset: 0x00051C6F
		private void UpdateRetrieved()
		{
			if (this.GrabbingTarget)
			{
				this.ResolveRetrievedObject(this.GrabbingTarget);
				this.GrabbingTarget = null;
			}
			this.EnterStatus(Hook.HookStatus.Swinging);
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x00053A98 File Offset: 0x00051C98
		private void ResolveRetrievedObject(GoldMinerEntity grabingTarget)
		{
			Action<Hook, GoldMinerEntity> onResolveTarget = this.OnResolveTarget;
			if (onResolveTarget == null)
			{
				return;
			}
			onResolveTarget(this, grabingTarget);
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00053AAC File Offset: 0x00051CAC
		private void OnExitStatus(Hook.HookStatus status)
		{
			switch (status)
			{
			case Hook.HookStatus.Idle:
			case Hook.HookStatus.Swinging:
			case Hook.HookStatus.Launching:
			case Hook.HookStatus.Attaching:
			case Hook.HookStatus.Retrieved:
				break;
			case Hook.HookStatus.Retrieving:
			{
				Action<Hook> onEndRetrieve = this.OnEndRetrieve;
				if (onEndRetrieve == null)
				{
					return;
				}
				onEndRetrieve(this);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00053ADE File Offset: 0x00051CDE
		private void EnterStatus(Hook.HookStatus status)
		{
			this.OnExitStatus(this.status);
			this.status = status;
			this.OnEnterStatus(this.status);
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00053B00 File Offset: 0x00051D00
		private void OnEnterStatus(Hook.HookStatus status)
		{
			switch (status)
			{
			case Hook.HookStatus.Idle:
			case Hook.HookStatus.Launching:
			case Hook.HookStatus.Attaching:
			case Hook.HookStatus.Retrieved:
				break;
			case Hook.HookStatus.Swinging:
				this.ropeControl = 0f;
				return;
			case Hook.HookStatus.Retrieving:
			{
				if (this.GrabbingTarget)
				{
					this.GrabbingTarget.NotifyBeginRetrieving();
				}
				Action<Hook> onBeginRetrieve = this.OnBeginRetrieve;
				if (onBeginRetrieve == null)
				{
					return;
				}
				onBeginRetrieve(this);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x00053B61 File Offset: 0x00051D61
		internal Vector3 Direction
		{
			get
			{
				return -this.hookAxis.transform.up;
			}
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00053B78 File Offset: 0x00051D78
		internal void ReleaseClaw()
		{
			this.GrabbingTarget = null;
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00053B81 File Offset: 0x00051D81
		internal void BeginSwing()
		{
			this.EnterStatus(Hook.HookStatus.Swinging);
		}

		// Token: 0x0400107B RID: 4219
		public float emptySpeed = 1000f;

		// Token: 0x0400107C RID: 4220
		public float strength;

		// Token: 0x0400107D RID: 4221
		public float swingFreqFactor = 1f;

		// Token: 0x0400107E RID: 4222
		[SerializeField]
		private Transform hookAxis;

		// Token: 0x0400107F RID: 4223
		[SerializeField]
		private HookHead hookHead;

		// Token: 0x04001080 RID: 4224
		[SerializeField]
		private Transform claw;

		// Token: 0x04001081 RID: 4225
		[SerializeField]
		private float clawOffset = 4f;

		// Token: 0x04001082 RID: 4226
		[SerializeField]
		private Animator clawAnimator;

		// Token: 0x04001083 RID: 4227
		[SerializeField]
		private LineRenderer ropeLineRenderer;

		// Token: 0x04001084 RID: 4228
		[SerializeField]
		private Bounds bounds;

		// Token: 0x04001085 RID: 4229
		[SerializeField]
		private float grabAnimationTime = 0.5f;

		// Token: 0x04001086 RID: 4230
		[SerializeField]
		private Ease grabAnimationEase = 30;

		// Token: 0x04001087 RID: 4231
		[SerializeField]
		private float maxAngle;

		// Token: 0x04001088 RID: 4232
		[SerializeField]
		private float minDist;

		// Token: 0x04001089 RID: 4233
		[SerializeField]
		private float maxDist;

		// Token: 0x0400108A RID: 4234
		[Range(0f, 1f)]
		private float ropeControl;

		// Token: 0x0400108B RID: 4235
		[Range(-1f, 1f)]
		private float axisControl;

		// Token: 0x0400108C RID: 4236
		private Hook.HookStatus status;

		// Token: 0x0400108D RID: 4237
		private float t;

		// Token: 0x0400108E RID: 4238
		private GoldMinerEntity _grabbingTarget;

		// Token: 0x0400108F RID: 4239
		private Vector2 relativePos;

		// Token: 0x04001090 RID: 4240
		private Quaternion targetRelativeRotation;

		// Token: 0x04001091 RID: 4241
		private float targetDist;

		// Token: 0x04001092 RID: 4242
		private float retrieveETA;

		// Token: 0x04001098 RID: 4248
		public float forceModification;

		// Token: 0x04001099 RID: 4249
		private float maxDeltaWatch;

		// Token: 0x02000592 RID: 1426
		public enum HookStatus
		{
			// Token: 0x0400205D RID: 8285
			Idle,
			// Token: 0x0400205E RID: 8286
			Swinging,
			// Token: 0x0400205F RID: 8287
			Launching,
			// Token: 0x04002060 RID: 8288
			Attaching,
			// Token: 0x04002061 RID: 8289
			Retrieving,
			// Token: 0x04002062 RID: 8290
			Retrieved
		}
	}
}
