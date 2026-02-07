using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200002A RID: 42
	[Category("Input (Legacy System)")]
	public class CheckButtonInput : ConditionTask
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009A RID: 154 RVA: 0x0000469F File Offset: 0x0000289F
		protected override string info
		{
			get
			{
				return this.pressType.ToString() + " " + this.buttonName.ToString();
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000046C8 File Offset: 0x000028C8
		protected override bool OnCheck()
		{
			if (this.pressType == PressTypes.Down)
			{
				return Input.GetButtonDown(this.buttonName.value);
			}
			if (this.pressType == PressTypes.Up)
			{
				return Input.GetButtonUp(this.buttonName.value);
			}
			return this.pressType == PressTypes.Pressed && Input.GetButton(this.buttonName.value);
		}

		// Token: 0x0400007B RID: 123
		public PressTypes pressType;

		// Token: 0x0400007C RID: 124
		[RequiredField]
		public BBParameter<string> buttonName = "Fire1";
	}
}
