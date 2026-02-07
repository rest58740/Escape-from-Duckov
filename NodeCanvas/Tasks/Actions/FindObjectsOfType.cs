using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000097 RID: 151
	[Category("GameObject")]
	[Description("Note that this is very slow")]
	public class FindObjectsOfType<T> : ActionTask where T : Component
	{
		// Token: 0x0600028D RID: 653 RVA: 0x0000A180 File Offset: 0x00008380
		protected override void OnExecute()
		{
			T[] array = Object.FindObjectsByType<T>(FindObjectsSortMode.None);
			if (array != null && array.Length != 0)
			{
				this.saveGameObjects.value = (from o in array
				select o.gameObject).ToList<GameObject>();
				this.saveComponents.value = array.ToList<T>();
				base.EndAction(true);
				return;
			}
			base.EndAction(false);
		}

		// Token: 0x040001B1 RID: 433
		[BlackboardOnly]
		public BBParameter<List<GameObject>> saveGameObjects;

		// Token: 0x040001B2 RID: 434
		[BlackboardOnly]
		public BBParameter<List<T>> saveComponents;
	}
}
