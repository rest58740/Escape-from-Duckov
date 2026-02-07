using System;
using System.Collections;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000024 RID: 36
	public abstract class ConditionTask : Task
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00006664 File Offset: 0x00004864
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000666C File Offset: 0x0000486C
		public bool invert
		{
			get
			{
				return this._invert;
			}
			set
			{
				this._invert = value;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00006675 File Offset: 0x00004875
		public void Enable(Component agent, IBlackboard bb)
		{
			if (!this.isRuntimeEnabled && base.isUserEnabled && base.Set(agent, bb))
			{
				this.isRuntimeEnabled = true;
				this.OnEnable();
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000669E File Offset: 0x0000489E
		public void Disable()
		{
			if (this.isRuntimeEnabled && base.isUserEnabled)
			{
				this.isRuntimeEnabled = false;
				this.OnDisable();
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000066BD File Offset: 0x000048BD
		[Obsolete("Use 'Check'")]
		public bool CheckCondition(Component agent, IBlackboard blackboard)
		{
			return this.Check(agent, blackboard);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000066C8 File Offset: 0x000048C8
		public bool Check(Component agent, IBlackboard blackboard)
		{
			if (!base.isUserEnabled)
			{
				return false;
			}
			if (!base.Set(agent, blackboard))
			{
				return false;
			}
			if (this.yieldReturn != -1)
			{
				bool result = this.invert ? (this.yieldReturn != 1) : (this.yieldReturn == 1);
				this.yieldReturn = -1;
				return result;
			}
			if (!this.invert)
			{
				return this.OnCheck();
			}
			return !this.OnCheck();
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00006733 File Offset: 0x00004933
		public bool CheckOnce(Component agent, IBlackboard blackboard)
		{
			this.Enable(agent, blackboard);
			bool result = this.Check(agent, blackboard);
			this.Disable();
			return result;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000674B File Offset: 0x0000494B
		protected void YieldReturn(bool value)
		{
			if (this.isRuntimeEnabled)
			{
				this.yieldReturn = (value ? 1 : 0);
				base.StartCoroutine(this.Flip());
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000676F File Offset: 0x0000496F
		private IEnumerator Flip()
		{
			this.yields++;
			yield return null;
			this.yields--;
			if (this.yields == 0)
			{
				this.yieldReturn = -1;
			}
			yield break;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000677E File Offset: 0x0000497E
		protected virtual void OnEnable()
		{
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00006780 File Offset: 0x00004980
		protected virtual void OnDisable()
		{
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00006782 File Offset: 0x00004982
		protected virtual bool OnCheck()
		{
			return true;
		}

		// Token: 0x0400006D RID: 109
		[SerializeField]
		private bool _invert;

		// Token: 0x0400006E RID: 110
		private int yieldReturn = -1;

		// Token: 0x0400006F RID: 111
		private int yields;

		// Token: 0x04000070 RID: 112
		private bool isRuntimeEnabled;
	}
}
