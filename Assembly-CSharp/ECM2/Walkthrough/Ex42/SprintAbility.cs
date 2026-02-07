using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex42
{
	// Token: 0x02000073 RID: 115
	public class SprintAbility : MonoBehaviour
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x0000FEA3 File Offset: 0x0000E0A3
		public void Sprint()
		{
			this._sprintInputPressed = true;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000FEAC File Offset: 0x0000E0AC
		public void StopSprinting()
		{
			this._sprintInputPressed = false;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000FEB5 File Offset: 0x0000E0B5
		public bool IsSprinting()
		{
			return this._isSprinting;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000FEBD File Offset: 0x0000E0BD
		private bool CanSprint()
		{
			return this._character.IsWalking() && !this._character.IsCrouched();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000FEDC File Offset: 0x0000E0DC
		private void CheckSprintInput()
		{
			if (!this._isSprinting && this._sprintInputPressed && this.CanSprint())
			{
				this._isSprinting = true;
				this._cachedMaxWalkSpeed = this._character.maxWalkSpeed;
				this._character.maxWalkSpeed = this.maxSprintSpeed;
				return;
			}
			if (this._isSprinting && (!this._sprintInputPressed || !this.CanSprint()))
			{
				this._isSprinting = false;
				this._character.maxWalkSpeed = this._cachedMaxWalkSpeed;
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000FF5B File Offset: 0x0000E15B
		private void OnBeforeSimulationUpdated(float deltaTime)
		{
			this.CheckSprintInput();
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000FF63 File Offset: 0x0000E163
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000FF71 File Offset: 0x0000E171
		private void OnEnable()
		{
			this._character.BeforeSimulationUpdated += this.OnBeforeSimulationUpdated;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000FF8A File Offset: 0x0000E18A
		private void OnDisable()
		{
			this._character.BeforeSimulationUpdated -= this.OnBeforeSimulationUpdated;
		}

		// Token: 0x04000275 RID: 629
		[Space(15f)]
		public float maxSprintSpeed = 10f;

		// Token: 0x04000276 RID: 630
		private Character _character;

		// Token: 0x04000277 RID: 631
		private bool _isSprinting;

		// Token: 0x04000278 RID: 632
		private bool _sprintInputPressed;

		// Token: 0x04000279 RID: 633
		private float _cachedMaxWalkSpeed;
	}
}
