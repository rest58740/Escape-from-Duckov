using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A3 RID: 163
	[Name("Set Visibility", 0)]
	[Category("GameObject")]
	[Description("Set the Renderer active state, thus making the object visible or invisible.")]
	public class SetObjectVisibility : ActionTask<Renderer>
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000A80A File Offset: 0x00008A0A
		protected override string info
		{
			get
			{
				return string.Format("{0} {1}", this.setTo, base.agentInfo);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000A828 File Offset: 0x00008A28
		protected override void OnExecute()
		{
			bool enabled;
			if (this.setTo == SetObjectVisibility.SetVisibleMode.Toggle)
			{
				enabled = !base.agent.enabled;
			}
			else
			{
				enabled = (this.setTo == SetObjectVisibility.SetVisibleMode.Show);
			}
			base.agent.enabled = enabled;
			base.EndAction();
		}

		// Token: 0x040001C6 RID: 454
		public SetObjectVisibility.SetVisibleMode setTo = SetObjectVisibility.SetVisibleMode.Toggle;

		// Token: 0x0200013A RID: 314
		public enum SetVisibleMode
		{
			// Token: 0x04000383 RID: 899
			Hide,
			// Token: 0x04000384 RID: 900
			Show,
			// Token: 0x04000385 RID: 901
			Toggle
		}
	}
}
