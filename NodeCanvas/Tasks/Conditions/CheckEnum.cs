using System;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200000E RID: 14
	[Category("✫ Blackboard")]
	public class CheckEnum : ConditionTask
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000026B2 File Offset: 0x000008B2
		protected override string info
		{
			get
			{
				BBObjectParameter bbobjectParameter = this.valueA;
				string text = (bbobjectParameter != null) ? bbobjectParameter.ToString() : null;
				string text2 = " == ";
				BBObjectParameter bbobjectParameter2 = this.valueB;
				return text + text2 + ((bbobjectParameter2 != null) ? bbobjectParameter2.ToString() : null);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000026E2 File Offset: 0x000008E2
		protected override bool OnCheck()
		{
			return object.Equals(this.valueA.value, this.valueB.value);
		}

		// Token: 0x0400001D RID: 29
		[BlackboardOnly]
		public BBObjectParameter valueA = new BBObjectParameter(typeof(Enum));

		// Token: 0x0400001E RID: 30
		public BBObjectParameter valueB = new BBObjectParameter(typeof(Enum));
	}
}
