using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A1 RID: 161
	[Name("Set Active", 0)]
	[Category("GameObject")]
	[Description("Set the gameobject active state.")]
	public class SetObjectActive : ActionTask<Transform>
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000A71E File Offset: 0x0000891E
		protected override string info
		{
			get
			{
				return string.Format("{0} {1}", this.setTo, base.agentInfo);
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000A73C File Offset: 0x0000893C
		protected override void OnExecute()
		{
			bool active;
			if (this.setTo == SetObjectActive.SetActiveMode.Toggle)
			{
				active = !base.agent.gameObject.activeSelf;
			}
			else
			{
				active = (this.setTo == SetObjectActive.SetActiveMode.Activate);
			}
			base.agent.gameObject.SetActive(active);
			base.EndAction();
		}

		// Token: 0x040001C4 RID: 452
		public SetObjectActive.SetActiveMode setTo = SetObjectActive.SetActiveMode.Toggle;

		// Token: 0x02000138 RID: 312
		public enum SetActiveMode
		{
			// Token: 0x0400037B RID: 891
			Deactivate,
			// Token: 0x0400037C RID: 892
			Activate,
			// Token: 0x0400037D RID: 893
			Toggle
		}
	}
}
