using System;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000087 RID: 135
	[Category("✫ Blackboard")]
	[Description("Triggers a boolean variable for 1 frame to True then back to False")]
	public class TriggerBoolean : ActionTask
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000992F File Offset: 0x00007B2F
		protected override string info
		{
			get
			{
				return string.Format("Trigger {0}", this.variable);
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00009941 File Offset: 0x00007B41
		protected override void OnExecute()
		{
			if (!this.variable.value)
			{
				this.variable.value = true;
				base.StartCoroutine(this.Flip());
			}
			base.EndAction();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000996F File Offset: 0x00007B6F
		private IEnumerator Flip()
		{
			yield return null;
			this.variable.value = false;
			yield break;
		}

		// Token: 0x0400018D RID: 397
		[RequiredField]
		[BlackboardOnly]
		public BBParameter<bool> variable;
	}
}
