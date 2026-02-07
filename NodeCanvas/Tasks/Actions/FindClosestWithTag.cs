using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000095 RID: 149
	[Category("GameObject")]
	[Description("Find the closest game object of tag to the agent")]
	public class FindClosestWithTag : ActionTask<Transform>
	{
		// Token: 0x06000289 RID: 649 RVA: 0x0000A038 File Offset: 0x00008238
		protected override void OnExecute()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(this.searchTag.value);
			if (array.Length == 0)
			{
				this.saveObjectAs.value = null;
				this.saveDistanceAs.value = 0f;
				base.EndAction(false);
				return;
			}
			GameObject value = null;
			float num = float.PositiveInfinity;
			foreach (GameObject gameObject in array)
			{
				if (!(gameObject.transform == base.agent) && (!this.ignoreChildren.value || !gameObject.transform.IsChildOf(base.agent)))
				{
					float num2 = Vector3.Distance(gameObject.transform.position, base.agent.position);
					if (num2 < num)
					{
						num = num2;
						value = gameObject;
					}
				}
			}
			this.saveObjectAs.value = value;
			this.saveDistanceAs.value = num;
			base.EndAction();
		}

		// Token: 0x040001AB RID: 427
		[TagField]
		[RequiredField]
		public BBParameter<string> searchTag;

		// Token: 0x040001AC RID: 428
		public BBParameter<bool> ignoreChildren;

		// Token: 0x040001AD RID: 429
		[BlackboardOnly]
		public BBParameter<GameObject> saveObjectAs;

		// Token: 0x040001AE RID: 430
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;
	}
}
