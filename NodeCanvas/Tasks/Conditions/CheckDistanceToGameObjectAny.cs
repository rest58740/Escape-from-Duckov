using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000021 RID: 33
	[Name("Any Target Within Distance", 0)]
	[Category("GameObject")]
	public class CheckDistanceToGameObjectAny : ConditionTask<Transform>
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003D64 File Offset: 0x00001F64
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

		// Token: 0x0600007A RID: 122 RVA: 0x00003DC4 File Offset: 0x00001FC4
		protected override bool OnCheck()
		{
			bool result = false;
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in this.targetObjects.value)
			{
				if (!(gameObject == base.agent.gameObject) && OperationTools.Compare(Vector3.Distance(base.agent.position, gameObject.transform.position), this.distance.value, this.checkType, this.floatingPoint))
				{
					list.Add(gameObject);
					result = true;
				}
			}
			if (!this.allResults.isNone || !this.closerResult.isNone)
			{
				IOrderedEnumerable<GameObject> source = from x in list
				orderby Vector3.Distance(base.agent.position, x.transform.position)
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

		// Token: 0x0600007B RID: 123 RVA: 0x00003EE0 File Offset: 0x000020E0
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.DrawWireSphere(base.agent.position, this.distance.value);
			}
		}

		// Token: 0x04000061 RID: 97
		public BBParameter<List<GameObject>> targetObjects;

		// Token: 0x04000062 RID: 98
		public CompareMethod checkType = CompareMethod.LessThan;

		// Token: 0x04000063 RID: 99
		public BBParameter<float> distance = 10f;

		// Token: 0x04000064 RID: 100
		[SliderField(0f, 0.1f)]
		public float floatingPoint = 0.05f;

		// Token: 0x04000065 RID: 101
		[BlackboardOnly]
		public BBParameter<List<GameObject>> allResults;

		// Token: 0x04000066 RID: 102
		[BlackboardOnly]
		public BBParameter<GameObject> closerResult;
	}
}
