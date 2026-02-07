using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000094 RID: 148
	[Category("GameObject")]
	[Description("Find a transform child by name within the agent's transform")]
	public class FindChildByName : ActionTask<Transform>
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000286 RID: 646 RVA: 0x00009FD2 File Offset: 0x000081D2
		protected override string info
		{
			get
			{
				return string.Format("{0} = {1}.FindChild({2})", this.saveAs, base.agentInfo, this.childName);
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00009FF0 File Offset: 0x000081F0
		protected override void OnExecute()
		{
			Transform transform = base.agent.Find(this.childName.value);
			this.saveAs.value = transform;
			base.EndAction(transform != null);
		}

		// Token: 0x040001A9 RID: 425
		[RequiredField]
		public BBParameter<string> childName;

		// Token: 0x040001AA RID: 426
		[BlackboardOnly]
		public BBParameter<Transform> saveAs;
	}
}
