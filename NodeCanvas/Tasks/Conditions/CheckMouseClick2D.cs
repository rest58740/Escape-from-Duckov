using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200004A RID: 74
	[Category("System Events")]
	[Name("Check Mouse Click 2D", 0)]
	public class CheckMouseClick2D : ConditionTask<Collider2D>
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00006EBF File Offset: 0x000050BF
		protected override string info
		{
			get
			{
				return this.checkType.ToString();
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006ED2 File Offset: 0x000050D2
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006ED5 File Offset: 0x000050D5
		protected override void OnEnable()
		{
			base.router.onMouseDown += this.OnMouseDown;
			base.router.onMouseUp += this.OnMouseUp;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006F05 File Offset: 0x00005105
		protected override void OnDisable()
		{
			base.router.onMouseDown -= this.OnMouseDown;
			base.router.onMouseUp -= this.OnMouseUp;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006F35 File Offset: 0x00005135
		private void OnMouseDown(EventData msg)
		{
			if (this.checkType == MouseClickEvent.MouseDown)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006F46 File Offset: 0x00005146
		private void OnMouseUp(EventData msg)
		{
			if (this.checkType == MouseClickEvent.MouseUp)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x040000E2 RID: 226
		public MouseClickEvent checkType;
	}
}
