using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000014 RID: 20
	[Category("✫ Blackboard")]
	[Description("It's best to use the respective Condition for a type if existant since they support operations as well")]
	public class CheckVariable<T> : ConditionTask
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000028E9 File Offset: 0x00000AE9
		protected override string info
		{
			get
			{
				BBParameter<T> bbparameter = this.valueA;
				string text = (bbparameter != null) ? bbparameter.ToString() : null;
				string text2 = " == ";
				BBParameter<T> bbparameter2 = this.valueB;
				return text + text2 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002919 File Offset: 0x00000B19
		protected override bool OnCheck()
		{
			return ObjectUtils.AnyEquals(this.valueA.value, this.valueB.value);
		}

		// Token: 0x0400002B RID: 43
		[BlackboardOnly]
		public BBParameter<T> valueA;

		// Token: 0x0400002C RID: 44
		public BBParameter<T> valueB;
	}
}
