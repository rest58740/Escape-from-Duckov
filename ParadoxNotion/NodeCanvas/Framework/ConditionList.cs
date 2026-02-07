using System;
using System.Collections.Generic;
using ParadoxNotion.Design;

namespace NodeCanvas.Framework
{
	// Token: 0x02000022 RID: 34
	[DoNotList]
	public class ConditionList : ConditionTask
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x000062FB File Offset: 0x000044FB
		private bool allTrueRequired
		{
			get
			{
				return this.checkMode == ConditionList.ConditionsCheckMode.AllTrueRequired;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00006308 File Offset: 0x00004508
		protected override string info
		{
			get
			{
				if (this.conditions.Count == 0)
				{
					return "No Conditions";
				}
				string text = (this.conditions.Count > 1) ? ("<b>(" + (this.allTrueRequired ? "ALL True" : "ANY True") + ")</b>\n") : string.Empty;
				for (int i = 0; i < this.conditions.Count; i++)
				{
					if (this.conditions[i] != null && this.conditions[i].isUserEnabled)
					{
						string text2 = "▪";
						text = text + text2 + this.conditions[i].summaryInfo + ((i == this.conditions.Count - 1) ? "" : "\n");
					}
				}
				return text;
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000063D4 File Offset: 0x000045D4
		public override Task Duplicate(ITaskSystem newOwnerSystem)
		{
			ConditionList conditionList = (ConditionList)base.Duplicate(newOwnerSystem);
			conditionList.conditions.Clear();
			foreach (ConditionTask conditionTask in this.conditions)
			{
				conditionList.AddCondition((ConditionTask)conditionTask.Duplicate(newOwnerSystem));
			}
			return conditionList;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000644C File Offset: 0x0000464C
		protected override void OnEnable()
		{
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00006450 File Offset: 0x00004650
		protected override void OnDisable()
		{
			for (int i = 0; i < this.conditions.Count; i++)
			{
				this.conditions[i].Disable();
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00006484 File Offset: 0x00004684
		protected override bool OnCheck()
		{
			int num = 0;
			for (int i = 0; i < this.conditions.Count; i++)
			{
				if (!this.conditions[i].isUserEnabled)
				{
					num++;
				}
				else
				{
					this.conditions[i].Enable(base.agent, base.blackboard);
					if (this.conditions[i].Check(base.agent, base.blackboard))
					{
						if (!this.allTrueRequired)
						{
							return true;
						}
						num++;
					}
					else if (this.allTrueRequired)
					{
						return false;
					}
				}
			}
			return num == this.conditions.Count;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000652C File Offset: 0x0000472C
		public override void OnDrawGizmosSelected()
		{
			for (int i = 0; i < this.conditions.Count; i++)
			{
				if (this.conditions[i].isUserEnabled)
				{
					this.conditions[i].OnDrawGizmosSelected();
				}
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00006574 File Offset: 0x00004774
		public void AddCondition(ConditionTask condition)
		{
			if (condition is ConditionList)
			{
				foreach (ConditionTask condition2 in (condition as ConditionList).conditions)
				{
					this.AddCondition(condition2);
				}
				return;
			}
			this.conditions.Add(condition);
			condition.SetOwnerSystem(base.ownerSystem);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000065F0 File Offset: 0x000047F0
		internal override string GetWarningOrError()
		{
			for (int i = 0; i < this.conditions.Count; i++)
			{
				string warningOrError = this.conditions[i].GetWarningOrError();
				if (warningOrError != null)
				{
					return warningOrError;
				}
			}
			return null;
		}

		// Token: 0x0400006B RID: 107
		public ConditionList.ConditionsCheckMode checkMode;

		// Token: 0x0400006C RID: 108
		public List<ConditionTask> conditions = new List<ConditionTask>();

		// Token: 0x02000107 RID: 263
		public enum ConditionsCheckMode
		{
			// Token: 0x0400029D RID: 669
			AllTrueRequired,
			// Token: 0x0400029E RID: 670
			AnyTrueSuffice
		}
	}
}
