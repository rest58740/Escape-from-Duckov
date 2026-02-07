using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000082 RID: 130
	[Name("Set Integer", 0)]
	[Category("✫ Blackboard")]
	[Description("Set a blackboard integer variable")]
	public class SetInt : ActionTask
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000096A5 File Offset: 0x000078A5
		protected override string info
		{
			get
			{
				BBParameter<int> bbparameter = this.valueA;
				string text = (bbparameter != null) ? bbparameter.ToString() : null;
				string operationString = OperationTools.GetOperationString(this.Operation);
				BBParameter<int> bbparameter2 = this.valueB;
				return text + operationString + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000096DB File Offset: 0x000078DB
		protected override void OnExecute()
		{
			this.valueA.value = OperationTools.Operate(this.valueA.value, this.valueB.value, this.Operation);
			base.EndAction();
		}

		// Token: 0x0400017F RID: 383
		[BlackboardOnly]
		public BBParameter<int> valueA;

		// Token: 0x04000180 RID: 384
		public OperationMethod Operation;

		// Token: 0x04000181 RID: 385
		public BBParameter<int> valueB;
	}
}
