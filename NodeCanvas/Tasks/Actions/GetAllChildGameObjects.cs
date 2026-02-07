using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200009A RID: 154
	[Category("GameObject")]
	public class GetAllChildGameObjects : ActionTask<Transform>
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000A2B2 File Offset: 0x000084B2
		protected override string info
		{
			get
			{
				return string.Format("{0} = {1} Children Of {2}", this.saveAs, this.recursive ? "All" : "First", base.agentInfo);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000A2E0 File Offset: 0x000084E0
		protected override void OnExecute()
		{
			List<Transform> list = new List<Transform>();
			foreach (object obj in base.agent.transform)
			{
				Transform transform = (Transform)obj;
				list.Add(transform);
				if (this.recursive)
				{
					list.AddRange(this.Get(transform));
				}
			}
			this.saveAs.value = (from t in list
			select t.gameObject).ToList<GameObject>();
			base.EndAction();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000A394 File Offset: 0x00008594
		private List<Transform> Get(Transform parent)
		{
			List<Transform> list = new List<Transform>();
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				list.Add(transform);
				list.AddRange(this.Get(transform));
			}
			return list;
		}

		// Token: 0x040001B7 RID: 439
		[BlackboardOnly]
		public BBParameter<List<GameObject>> saveAs;

		// Token: 0x040001B8 RID: 440
		public bool recursive;
	}
}
