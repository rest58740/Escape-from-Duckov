using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000017 RID: 23
	[Category("✫ Blackboard/Lists")]
	[Description("Check if an element is contained in the target list")]
	public class ListContainsElement<T> : ConditionTask
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002A36 File Offset: 0x00000C36
		protected override string info
		{
			get
			{
				BBParameter<List<T>> bbparameter = this.targetList;
				string text = (bbparameter != null) ? bbparameter.ToString() : null;
				string text2 = " contains ";
				BBParameter<T> bbparameter2 = this.checkElement;
				return text + text2 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002A66 File Offset: 0x00000C66
		protected override bool OnCheck()
		{
			return this.targetList.value.Contains(this.checkElement.value);
		}

		// Token: 0x04000034 RID: 52
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<List<T>> targetList;

		// Token: 0x04000035 RID: 53
		public BBParameter<T> checkElement;
	}
}
