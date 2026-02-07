using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000090 RID: 144
	[Category("GameObject")]
	public class DestroyGameObject : ActionTask<Transform>
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00009DD1 File Offset: 0x00007FD1
		protected override string info
		{
			get
			{
				return string.Format("Destroy {0}", base.agentInfo);
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00009DE3 File Offset: 0x00007FE3
		protected override void OnUpdate()
		{
			if (this.immediately)
			{
				Object.DestroyImmediate(base.agent.gameObject);
			}
			else
			{
				Object.Destroy(base.agent.gameObject);
			}
			base.EndAction();
		}

		// Token: 0x040001A2 RID: 418
		[Tooltip("DestroyImmediately is recomended if you are destroying objects in use of the framework.")]
		public bool immediately;
	}
}
