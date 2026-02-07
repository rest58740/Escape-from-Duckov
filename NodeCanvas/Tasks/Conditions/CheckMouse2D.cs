using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000048 RID: 72
	[Category("System Events")]
	[Name("Check Mouse 2D", 0)]
	public class CheckMouse2D : ConditionTask<Collider2D>
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00006D23 File Offset: 0x00004F23
		protected override string info
		{
			get
			{
				return this.checkType.ToString();
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006D38 File Offset: 0x00004F38
		protected override void OnEnable()
		{
			base.router.onMouseEnter += this.OnMouseEnter;
			base.router.onMouseExit += this.OnMouseExit;
			base.router.onMouseOver += this.OnMouseOver;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006D8C File Offset: 0x00004F8C
		protected override void OnDisable()
		{
			base.router.onMouseEnter -= this.OnMouseEnter;
			base.router.onMouseExit -= this.OnMouseExit;
			base.router.onMouseOver -= this.OnMouseOver;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006DDE File Offset: 0x00004FDE
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006DE1 File Offset: 0x00004FE1
		private void OnMouseEnter(EventData msg)
		{
			if (this.checkType == MouseInteractionTypes.MouseEnter)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006DF2 File Offset: 0x00004FF2
		private void OnMouseExit(EventData msg)
		{
			if (this.checkType == MouseInteractionTypes.MouseExit)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006E04 File Offset: 0x00005004
		private void OnMouseOver(EventData msg)
		{
			if (this.checkType == MouseInteractionTypes.MouseOver)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x040000E0 RID: 224
		public MouseInteractionTypes checkType;
	}
}
