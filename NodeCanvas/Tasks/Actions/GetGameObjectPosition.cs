using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200009D RID: 157
	[Obsolete("Use Get Property instead")]
	[Category("GameObject")]
	public class GetGameObjectPosition : ActionTask<Transform>
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000A4C2 File Offset: 0x000086C2
		protected override string info
		{
			get
			{
				string text = "Get ";
				string agentInfo = base.agentInfo;
				string text2 = " position as ";
				BBParameter<Vector3> bbparameter = this.saveAs;
				return text + agentInfo + text2 + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000A4EB File Offset: 0x000086EB
		protected override void OnExecute()
		{
			this.saveAs.value = base.agent.position;
			base.EndAction();
		}

		// Token: 0x040001BC RID: 444
		[BlackboardOnly]
		public BBParameter<Vector3> saveAs;
	}
}
