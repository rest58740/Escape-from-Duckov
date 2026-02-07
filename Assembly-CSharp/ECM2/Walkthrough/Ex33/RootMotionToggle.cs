using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex33
{
	// Token: 0x02000075 RID: 117
	public class RootMotionToggle : MonoBehaviour
	{
		// Token: 0x060003BA RID: 954 RVA: 0x00010181 File Offset: 0x0000E381
		private void OnMovementModeChanged(Character.MovementMode prevMovementMode, int prevCustomMovementMode)
		{
			this._character.useRootMotion = this._character.IsWalking();
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00010199 File Offset: 0x0000E399
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000101A7 File Offset: 0x0000E3A7
		private void OnEnable()
		{
			this._character.MovementModeChanged += this.OnMovementModeChanged;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000101C0 File Offset: 0x0000E3C0
		private void OnDisable()
		{
			this._character.MovementModeChanged -= this.OnMovementModeChanged;
		}

		// Token: 0x0400027D RID: 637
		private Character _character;
	}
}
