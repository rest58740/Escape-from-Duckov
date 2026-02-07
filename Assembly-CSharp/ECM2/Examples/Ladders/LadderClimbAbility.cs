using System;
using UnityEngine;

namespace ECM2.Examples.Ladders
{
	// Token: 0x02000089 RID: 137
	public class LadderClimbAbility : MonoBehaviour
	{
		// Token: 0x0600041C RID: 1052 RVA: 0x00011B3C File Offset: 0x0000FD3C
		public bool IsClimbing()
		{
			return this._character.movementMode == Character.MovementMode.Custom && this._character.customMovementMode == 1;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00011B5C File Offset: 0x0000FD5C
		private bool CanClimb()
		{
			if (this._character.IsCrouched())
			{
				return false;
			}
			int num;
			Collider[] array = this._character.characterMovement.OverlapTest(this.ladderMask, QueryTriggerInteraction.Collide, out num);
			if (num == 0)
			{
				return false;
			}
			Ladder activeLadder;
			if (!array[0].TryGetComponent<Ladder>(out activeLadder))
			{
				return false;
			}
			this._activeLadder = activeLadder;
			return true;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00011BB4 File Offset: 0x0000FDB4
		public void Climb()
		{
			if (this.IsClimbing() || !this.CanClimb())
			{
				return;
			}
			this._character.SetMovementMode(Character.MovementMode.Custom, 1);
			this._ladderStartPosition = this._character.GetPosition();
			this._ladderTargetPosition = this._activeLadder.ClosestPointOnPath(this._ladderStartPosition, out this._ladderPathPosition);
			this._ladderStartRotation = this._character.GetRotation();
			this._ladderTargetRotation = this._activeLadder.transform.rotation;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00011C34 File Offset: 0x0000FE34
		public void StopClimbing()
		{
			if (!this.IsClimbing() || this._climbingState != LadderClimbAbility.ClimbingState.Grabbed)
			{
				return;
			}
			this._climbingState = LadderClimbAbility.ClimbingState.Releasing;
			this._ladderStartPosition = this._character.GetPosition();
			this._ladderStartRotation = this._character.GetRotation();
			this._ladderTargetPosition = this._ladderStartPosition;
			this._ladderTargetRotation = this._activeLadder.BottomPoint.rotation;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00011CA0 File Offset: 0x0000FEA0
		private void ClimbingMovementMode(float deltaTime)
		{
			Vector3 velocity = Vector3.zero;
			switch (this._climbingState)
			{
			case LadderClimbAbility.ClimbingState.Grabbing:
			case LadderClimbAbility.ClimbingState.Releasing:
				this._ladderTime += deltaTime;
				if (this._ladderTime <= this.grabbingTime)
				{
					velocity = (Vector3.Lerp(this._ladderStartPosition, this._ladderTargetPosition, this._ladderTime / this.grabbingTime) - base.transform.position) / deltaTime;
				}
				else
				{
					this._ladderTime = 0f;
					if (this._climbingState == LadderClimbAbility.ClimbingState.Grabbing)
					{
						this._climbingState = LadderClimbAbility.ClimbingState.Grabbed;
					}
					else if (this._climbingState == LadderClimbAbility.ClimbingState.Releasing)
					{
						this._character.SetMovementMode(Character.MovementMode.Falling, 0);
					}
				}
				break;
			case LadderClimbAbility.ClimbingState.Grabbed:
				this._activeLadder.ClosestPointOnPath(this._character.GetPosition(), out this._ladderPathPosition);
				if (Mathf.Abs(this._ladderPathPosition) < 0.05f)
				{
					Vector3 movementDirection = this._character.GetMovementDirection();
					velocity = this._activeLadder.transform.up * (movementDirection.z * this.climbingSpeed);
				}
				else
				{
					this._climbingState = LadderClimbAbility.ClimbingState.Releasing;
					this._ladderStartPosition = this._character.GetPosition();
					this._ladderStartRotation = this._character.GetRotation();
					if (this._ladderPathPosition > 0f)
					{
						this._ladderTargetPosition = this._activeLadder.TopPoint.position;
						this._ladderTargetRotation = this._activeLadder.TopPoint.rotation;
					}
					else if (this._ladderPathPosition < 0f)
					{
						this._ladderTargetPosition = this._activeLadder.BottomPoint.position;
						this._ladderTargetRotation = this._activeLadder.BottomPoint.rotation;
					}
				}
				break;
			}
			this._character.SetVelocity(velocity);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00011E74 File Offset: 0x00010074
		private void OnMovementModeChanged(Character.MovementMode prevMovementMode, int prevCustomMovementMode)
		{
			if (this.IsClimbing())
			{
				this._climbingState = LadderClimbAbility.ClimbingState.Grabbing;
				this._character.StopJumping();
				this._character.EnableGroundConstraint(false);
				this._previousRotationMode = this._character.rotationMode;
				this._character.SetRotationMode(Character.RotationMode.Custom);
			}
			if (prevMovementMode == Character.MovementMode.Custom && prevCustomMovementMode == 1)
			{
				this._climbingState = LadderClimbAbility.ClimbingState.None;
				this._character.EnableGroundConstraint(true);
				this._character.SetRotationMode(this._previousRotationMode);
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00011EF5 File Offset: 0x000100F5
		private void OnCustomMovementModeUpdated(float deltaTime)
		{
			if (this.IsClimbing())
			{
				this.ClimbingMovementMode(deltaTime);
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00011F08 File Offset: 0x00010108
		private void OnCustomRotationModeUpdated(float deltaTime)
		{
			if (this.IsClimbing() && (this._climbingState == LadderClimbAbility.ClimbingState.Grabbing || this._climbingState == LadderClimbAbility.ClimbingState.Releasing))
			{
				Quaternion rotation = Quaternion.Slerp(this._ladderStartRotation, this._ladderTargetRotation, this._ladderTime / this.grabbingTime);
				this._character.SetRotation(rotation);
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00011F5A File Offset: 0x0001015A
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00011F68 File Offset: 0x00010168
		private void OnEnable()
		{
			this._character.MovementModeChanged += this.OnMovementModeChanged;
			this._character.CustomMovementModeUpdated += this.OnCustomMovementModeUpdated;
			this._character.CustomRotationModeUpdated += this.OnCustomRotationModeUpdated;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00011FBC File Offset: 0x000101BC
		private void OnDisable()
		{
			this._character.MovementModeChanged -= this.OnMovementModeChanged;
			this._character.CustomMovementModeUpdated -= this.OnCustomMovementModeUpdated;
			this._character.CustomRotationModeUpdated -= this.OnCustomRotationModeUpdated;
		}

		// Token: 0x040002B0 RID: 688
		public float climbingSpeed = 5f;

		// Token: 0x040002B1 RID: 689
		public float grabbingTime = 0.25f;

		// Token: 0x040002B2 RID: 690
		public LayerMask ladderMask;

		// Token: 0x040002B3 RID: 691
		private Character _character;

		// Token: 0x040002B4 RID: 692
		private Ladder _activeLadder;

		// Token: 0x040002B5 RID: 693
		private float _ladderPathPosition;

		// Token: 0x040002B6 RID: 694
		private Vector3 _ladderStartPosition;

		// Token: 0x040002B7 RID: 695
		private Vector3 _ladderTargetPosition;

		// Token: 0x040002B8 RID: 696
		private Quaternion _ladderStartRotation;

		// Token: 0x040002B9 RID: 697
		private Quaternion _ladderTargetRotation;

		// Token: 0x040002BA RID: 698
		private float _ladderTime;

		// Token: 0x040002BB RID: 699
		private LadderClimbAbility.ClimbingState _climbingState;

		// Token: 0x040002BC RID: 700
		private Character.RotationMode _previousRotationMode;

		// Token: 0x020000D1 RID: 209
		public enum CustomMovementMode
		{
			// Token: 0x0400042B RID: 1067
			Climbing = 1
		}

		// Token: 0x020000D2 RID: 210
		public enum ClimbingState
		{
			// Token: 0x0400042D RID: 1069
			None,
			// Token: 0x0400042E RID: 1070
			Grabbing,
			// Token: 0x0400042F RID: 1071
			Grabbed,
			// Token: 0x04000430 RID: 1072
			Releasing
		}
	}
}
