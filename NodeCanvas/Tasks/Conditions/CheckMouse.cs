using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000047 RID: 71
	[Category("System Events")]
	public class CheckMouse : ConditionTask<Collider>
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00006C27 File Offset: 0x00004E27
		protected override string info
		{
			get
			{
				return this.checkType.ToString();
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006C3A File Offset: 0x00004E3A
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006C40 File Offset: 0x00004E40
		protected override void OnEnable()
		{
			base.router.onMouseEnter += this.OnMouseEnter;
			base.router.onMouseExit += this.OnMouseExit;
			base.router.onMouseOver += this.OnMouseOver;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006C94 File Offset: 0x00004E94
		protected override void OnDisable()
		{
			base.router.onMouseEnter -= this.OnMouseEnter;
			base.router.onMouseExit -= this.OnMouseExit;
			base.router.onMouseOver -= this.OnMouseOver;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006CE6 File Offset: 0x00004EE6
		private void OnMouseEnter(EventData msg)
		{
			if (this.checkType == MouseInteractionTypes.MouseEnter)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006CF7 File Offset: 0x00004EF7
		private void OnMouseExit(EventData msg)
		{
			if (this.checkType == MouseInteractionTypes.MouseExit)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00006D09 File Offset: 0x00004F09
		private void OnMouseOver(EventData msg)
		{
			if (this.checkType == MouseInteractionTypes.MouseOver)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x040000DF RID: 223
		public MouseInteractionTypes checkType;
	}
}
