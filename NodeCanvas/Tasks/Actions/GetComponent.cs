using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200009B RID: 155
	[Category("GameObject")]
	public class GetComponent<T> : ActionTask<Transform> where T : Component
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000A404 File Offset: 0x00008604
		protected override string info
		{
			get
			{
				return string.Format("Get {0} as {1}", typeof(T).Name, this.saveAs.ToString());
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000A42C File Offset: 0x0000862C
		protected override void OnExecute()
		{
			T component = base.agent.GetComponent<T>();
			this.saveAs.value = component;
			base.EndAction(component != null);
		}

		// Token: 0x040001B9 RID: 441
		[BlackboardOnly]
		public BBParameter<T> saveAs;
	}
}
