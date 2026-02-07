using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000010 RID: 16
	[Category("✫ Blackboard")]
	public class CheckInt : ConditionTask
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000027A5 File Offset: 0x000009A5
		protected override string info
		{
			get
			{
				BBParameter<int> bbparameter = this.valueA;
				string text = (bbparameter != null) ? bbparameter.ToString() : null;
				string compareString = OperationTools.GetCompareString(this.checkType);
				BBParameter<int> bbparameter2 = this.valueB;
				return text + compareString + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000027DB File Offset: 0x000009DB
		protected override bool OnCheck()
		{
			return OperationTools.Compare(this.valueA.value, this.valueB.value, this.checkType);
		}

		// Token: 0x04000023 RID: 35
		[BlackboardOnly]
		public BBParameter<int> valueA;

		// Token: 0x04000024 RID: 36
		public CompareMethod checkType;

		// Token: 0x04000025 RID: 37
		public BBParameter<int> valueB;
	}
}
