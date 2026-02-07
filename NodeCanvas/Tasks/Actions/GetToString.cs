using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200006C RID: 108
	[Name("Get Variable To String", 0)]
	[Category("✫ Blackboard")]
	public class GetToString : ActionTask
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00008D10 File Offset: 0x00006F10
		protected override string info
		{
			get
			{
				return string.Format("{0} = {1}.ToString()", this.toString, this.variable);
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00008D28 File Offset: 0x00006F28
		protected override void OnExecute()
		{
			this.toString.value = ((!this.variable.isNull) ? this.variable.value.ToString() : "NULL");
			base.EndAction();
		}

		// Token: 0x0400014F RID: 335
		[BlackboardOnly]
		public BBParameter<object> variable;

		// Token: 0x04000150 RID: 336
		[BlackboardOnly]
		public BBParameter<string> toString;
	}
}
