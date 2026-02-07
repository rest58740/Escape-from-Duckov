using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000022 RID: 34
	[Name("Any Target Within Distance 2D", 0)]
	[Category("GameObject")]
	public class CheckDistanceToGameObjectAny2D : ConditionTask<Transform>
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003F54 File Offset: 0x00002154
		protected override string info
		{
			get
			{
				string[] array = new string[5];
				array[0] = "Distance Any";
				array[1] = OperationTools.GetCompareString(this.checkType);
				int num = 2;
				BBParameter<float> bbparameter = this.distance;
				array[num] = ((bbparameter != null) ? bbparameter.ToString() : null);
				array[3] = " in ";
				int num2 = 4;
				BBParameter<List<GameObject>> bbparameter2 = this.targetObjects;
				array[num2] = ((bbparameter2 != null) ? bbparameter2.ToString() : null);
				return string.Concat(array);
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003FB4 File Offset: 0x000021B4
		protected override bool OnCheck()
		{
			bool result = false;
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in this.targetObjects.value)
			{
				if (!(gameObject == base.agent.gameObject) && OperationTools.Compare(Vector2.Distance(base.agent.position, gameObject.transform.position), this.distance.value, this.checkType, this.floatingPoint))
				{
					list.Add(gameObject);
					result = true;
				}
			}
			if (!this.allResults.isNone || !this.closerResult.isNone)
			{
				IOrderedEnumerable<GameObject> source = from x in list
				orderby Vector2.Distance(base.agent.position, x.transform.position)
				select x;
				if (!this.allResults.isNone)
				{
					this.allResults.value = source.ToList<GameObject>();
				}
				if (!this.closerResult.isNone)
				{
					this.closerResult.value = source.FirstOrDefault<GameObject>();
				}
			}
			return result;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000040DC File Offset: 0x000022DC
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.DrawWireSphere(base.agent.position, this.distance.value);
			}
		}

		// Token: 0x04000067 RID: 103
		public BBParameter<List<GameObject>> targetObjects;

		// Token: 0x04000068 RID: 104
		public CompareMethod checkType = CompareMethod.LessThan;

		// Token: 0x04000069 RID: 105
		public BBParameter<float> distance = 10f;

		// Token: 0x0400006A RID: 106
		[SliderField(0f, 0.1f)]
		public float floatingPoint = 0.05f;

		// Token: 0x0400006B RID: 107
		[BlackboardOnly]
		public BBParameter<List<GameObject>> allResults;

		// Token: 0x0400006C RID: 108
		[BlackboardOnly]
		public BBParameter<GameObject> closerResult;
	}
}
