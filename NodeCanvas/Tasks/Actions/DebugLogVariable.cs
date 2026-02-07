using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000CB RID: 203
	[Category("✫ Utility")]
	[Description("Logs the value of a variable in the console")]
	[Obsolete("Use Debug Log Text")]
	public class DebugLogVariable : ActionTask
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000DD1C File Offset: 0x0000BF1C
		protected override string info
		{
			get
			{
				string text = "Log '";
				BBParameter<object> bbparameter = this.log;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null) + "'" + ((this.secondsToRun > 0f) ? (" for " + this.secondsToRun.ToString() + " sec.") : "");
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000DD78 File Offset: 0x0000BF78
		protected override void OnExecute()
		{
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000DD7A File Offset: 0x0000BF7A
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.secondsToRun)
			{
				base.EndAction(this.finishStatus == CompactStatus.Success);
			}
		}

		// Token: 0x04000261 RID: 609
		[BlackboardOnly]
		public BBParameter<object> log;

		// Token: 0x04000262 RID: 610
		public BBParameter<string> prefix;

		// Token: 0x04000263 RID: 611
		public float secondsToRun = 1f;

		// Token: 0x04000264 RID: 612
		public CompactStatus finishStatus = CompactStatus.Success;
	}
}
