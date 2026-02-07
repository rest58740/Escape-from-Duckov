using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000049 RID: 73
	[Category("System Events")]
	public class CheckMouseClick : ConditionTask<Collider>
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00006E1E File Offset: 0x0000501E
		protected override string info
		{
			get
			{
				return this.checkType.ToString();
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006E31 File Offset: 0x00005031
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006E34 File Offset: 0x00005034
		protected override void OnEnable()
		{
			base.router.onMouseDown += this.OnMouseDown;
			base.router.onMouseUp += this.OnMouseUp;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006E64 File Offset: 0x00005064
		protected override void OnDisable()
		{
			base.router.onMouseDown -= this.OnMouseDown;
			base.router.onMouseUp -= this.OnMouseUp;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006E94 File Offset: 0x00005094
		private void OnMouseDown(EventData msg)
		{
			if (this.checkType == MouseClickEvent.MouseDown)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006EA5 File Offset: 0x000050A5
		private void OnMouseUp(EventData msg)
		{
			if (this.checkType == MouseClickEvent.MouseUp)
			{
				base.YieldReturn(true);
			}
		}

		// Token: 0x040000E1 RID: 225
		public MouseClickEvent checkType;
	}
}
