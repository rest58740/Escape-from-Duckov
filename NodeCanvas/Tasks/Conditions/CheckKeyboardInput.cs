using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200002B RID: 43
	[Category("Input (Legacy System)")]
	public class CheckKeyboardInput : ConditionTask
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000473B File Offset: 0x0000293B
		protected override string info
		{
			get
			{
				return this.pressType.ToString() + " " + this.key.ToString();
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000476C File Offset: 0x0000296C
		protected override bool OnCheck()
		{
			if (this.pressType == PressTypes.Down)
			{
				return Input.GetKeyDown(this.key);
			}
			if (this.pressType == PressTypes.Up)
			{
				return Input.GetKeyUp(this.key);
			}
			return this.pressType == PressTypes.Pressed && Input.GetKey(this.key);
		}

		// Token: 0x0400007D RID: 125
		public PressTypes pressType;

		// Token: 0x0400007E RID: 126
		public KeyCode key = KeyCode.Space;
	}
}
