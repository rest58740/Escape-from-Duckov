using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000012 RID: 18
	[Category("✫ Blackboard")]
	public class CheckString : ConditionTask
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000040 RID: 64 RVA: 0x0000283F File Offset: 0x00000A3F
		protected override string info
		{
			get
			{
				BBParameter<string> bbparameter = this.valueA;
				string text = (bbparameter != null) ? bbparameter.ToString() : null;
				string text2 = " == ";
				BBParameter<string> bbparameter2 = this.valueB;
				return text + text2 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000286F File Offset: 0x00000A6F
		protected override bool OnCheck()
		{
			return this.valueA.value == this.valueB.value;
		}

		// Token: 0x04000027 RID: 39
		[BlackboardOnly]
		public BBParameter<string> valueA;

		// Token: 0x04000028 RID: 40
		public BBParameter<string> valueB;
	}
}
