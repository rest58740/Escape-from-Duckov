using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200001A RID: 26
	[Category("✫ Blackboard")]
	public class StringContains : ConditionTask
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002B3E File Offset: 0x00000D3E
		protected override string info
		{
			get
			{
				return string.Format("{0} Contains {1}", this.targetString, this.checkString);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002B56 File Offset: 0x00000D56
		protected override bool OnCheck()
		{
			return this.targetString.value.Contains(this.checkString.value);
		}

		// Token: 0x04000038 RID: 56
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<string> targetString;

		// Token: 0x04000039 RID: 57
		public BBParameter<string> checkString;
	}
}
