using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200007C RID: 124
	[Category("✫ Blackboard")]
	[Description("Saves the blackboard variables in the provided key and to be loaded later on")]
	public class SaveBlackboard : ActionTask<Blackboard>
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00009388 File Offset: 0x00007588
		protected override string info
		{
			get
			{
				return string.Format("Save Blackboard [{0}]", this.saveKey.ToString());
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000939F File Offset: 0x0000759F
		protected override void OnExecute()
		{
			base.agent.Save(this.saveKey.value);
			base.EndAction();
		}

		// Token: 0x04000172 RID: 370
		[RequiredField]
		public BBParameter<string> saveKey;
	}
}
