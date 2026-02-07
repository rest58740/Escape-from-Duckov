using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000085 RID: 133
	[Category("✫ Blackboard")]
	public class SetVariable<T> : ActionTask
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000982A File Offset: 0x00007A2A
		protected override string info
		{
			get
			{
				BBParameter<T> bbparameter = this.valueA;
				string text = (bbparameter != null) ? bbparameter.ToString() : null;
				string text2 = " = ";
				BBParameter<T> bbparameter2 = this.valueB;
				return text + text2 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000985A File Offset: 0x00007A5A
		protected override void OnExecute()
		{
			this.valueA.value = this.valueB.value;
			base.EndAction();
		}

		// Token: 0x04000187 RID: 391
		[BlackboardOnly]
		public BBParameter<T> valueA;

		// Token: 0x04000188 RID: 392
		public BBParameter<T> valueB;
	}
}
