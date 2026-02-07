using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000079 RID: 121
	[Category("✫ Blackboard")]
	[Description("Loads the blackboard variables previously saved in the provided PlayerPrefs key if at all. Returns false if no saves found or load was failed")]
	public class LoadBlackboard : ActionTask<Blackboard>
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000232 RID: 562 RVA: 0x000092B8 File Offset: 0x000074B8
		protected override string info
		{
			get
			{
				return string.Format("Load Blackboard [{0}]", this.saveKey.ToString());
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000092CF File Offset: 0x000074CF
		protected override void OnExecute()
		{
			base.EndAction(base.agent.Load(this.saveKey.value));
		}

		// Token: 0x0400016C RID: 364
		[RequiredField]
		public BBParameter<string> saveKey;
	}
}
