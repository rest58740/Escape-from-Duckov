using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A0 RID: 160
	[Category("GameObject")]
	public class RemoveComponent<T> : ActionTask<Transform> where T : Component
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000A6A5 File Offset: 0x000088A5
		protected override string info
		{
			get
			{
				return string.Format("Remove '{0}'", typeof(T).Name);
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000A6C0 File Offset: 0x000088C0
		protected override void OnExecute()
		{
			T component = base.agent.GetComponent<T>();
			if (component != null)
			{
				if (this.immediately)
				{
					Object.DestroyImmediate(component);
				}
				else
				{
					Object.Destroy(component);
				}
				base.EndAction(true);
				return;
			}
			base.EndAction(false);
		}

		// Token: 0x040001C3 RID: 451
		[Tooltip("DestroyImmediately is recomended if you are destroying objects in use of the framework.")]
		public bool immediately;
	}
}
