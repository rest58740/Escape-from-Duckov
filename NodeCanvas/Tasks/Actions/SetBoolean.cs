using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200007D RID: 125
	[Category("✫ Blackboard")]
	[Description("Set a blackboard boolean variable")]
	public class SetBoolean : ActionTask
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600023C RID: 572 RVA: 0x000093C8 File Offset: 0x000075C8
		protected override string info
		{
			get
			{
				if (this.setTo == SetBoolean.BoolSetModes.Toggle)
				{
					return "Toggle " + this.boolVariable.ToString();
				}
				return "Set " + this.boolVariable.ToString() + " to " + this.setTo.ToString();
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00009420 File Offset: 0x00007620
		protected override void OnExecute()
		{
			if (this.setTo == SetBoolean.BoolSetModes.Toggle)
			{
				this.boolVariable.value = !this.boolVariable.value;
			}
			else
			{
				bool value = this.setTo == SetBoolean.BoolSetModes.True;
				this.boolVariable.value = value;
			}
			base.EndAction();
		}

		// Token: 0x04000173 RID: 371
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<bool> boolVariable;

		// Token: 0x04000174 RID: 372
		public SetBoolean.BoolSetModes setTo = SetBoolean.BoolSetModes.True;

		// Token: 0x02000132 RID: 306
		public enum BoolSetModes
		{
			// Token: 0x04000368 RID: 872
			False,
			// Token: 0x04000369 RID: 873
			True,
			// Token: 0x0400036A RID: 874
			Toggle
		}
	}
}
