using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200006F RID: 111
	[Category("✫ Blackboard/Lists")]
	[Description("Get the closer game object to the agent from within a list of game objects and save it in the blackboard.")]
	public class GetCloserGameObjectInList : ActionTask<Transform>
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00008DDD File Offset: 0x00006FDD
		protected override string info
		{
			get
			{
				string text = "Get Closer from '";
				BBParameter<List<GameObject>> bbparameter = this.list;
				string text2 = (bbparameter != null) ? bbparameter.ToString() : null;
				string text3 = "' as ";
				BBParameter<GameObject> bbparameter2 = this.saveAs;
				return text + text2 + text3 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00008E14 File Offset: 0x00007014
		protected override void OnExecute()
		{
			if (this.list.value.Count == 0)
			{
				base.EndAction(false);
				return;
			}
			float num = float.PositiveInfinity;
			GameObject value = null;
			foreach (GameObject gameObject in this.list.value)
			{
				float num2 = Vector3.Distance(base.agent.position, gameObject.transform.position);
				if (num2 < num)
				{
					num = num2;
					value = gameObject;
				}
			}
			this.saveAs.value = value;
			base.EndAction(true);
		}

		// Token: 0x04000154 RID: 340
		[RequiredField]
		public BBParameter<List<GameObject>> list;

		// Token: 0x04000155 RID: 341
		[BlackboardOnly]
		public BBParameter<GameObject> saveAs;
	}
}
