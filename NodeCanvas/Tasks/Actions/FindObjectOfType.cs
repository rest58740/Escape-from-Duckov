using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000096 RID: 150
	[Category("GameObject")]
	[Description("Note that this is very slow")]
	public class FindObjectOfType<T> : ActionTask where T : Component
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0000A124 File Offset: 0x00008324
		protected override void OnExecute()
		{
			T t = Object.FindAnyObjectByType<T>();
			if (t != null)
			{
				this.saveComponentAs.value = t;
				this.saveGameObjectAs.value = t.gameObject;
				base.EndAction(true);
				return;
			}
			base.EndAction(false);
		}

		// Token: 0x040001AF RID: 431
		[BlackboardOnly]
		public BBParameter<T> saveComponentAs;

		// Token: 0x040001B0 RID: 432
		[BlackboardOnly]
		public BBParameter<GameObject> saveGameObjectAs;
	}
}
