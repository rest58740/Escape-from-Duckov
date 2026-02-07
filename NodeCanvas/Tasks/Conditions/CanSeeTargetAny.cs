using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200001D RID: 29
	[Category("GameObject")]
	[Description("A combination of line of sight and view angle check")]
	public class CanSeeTargetAny : ConditionTask<Transform>
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000067 RID: 103 RVA: 0x0000323C File Offset: 0x0000143C
		protected override string info
		{
			get
			{
				string text = "Can See Any ";
				BBParameter<List<GameObject>> bbparameter = this.targetObjects;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000325C File Offset: 0x0000145C
		protected override bool OnCheck()
		{
			bool result = false;
			bool flag = !this.allResults.isNone || !this.closerResult.isNone;
			List<GameObject> list = flag ? new List<GameObject>() : null;
			foreach (GameObject gameObject in this.targetObjects.value)
			{
				if (!(gameObject == base.agent.gameObject))
				{
					Transform transform = gameObject.transform;
					if (transform.gameObject.activeInHierarchy)
					{
						if (Vector3.Distance(base.agent.position, transform.position) < this.awarnessDistance.value)
						{
							if (!Physics.Linecast(base.agent.position + this.offset, transform.position + this.offset, out this.hit, this.layerMask.value) || !(this.hit.collider != transform.GetComponent<Collider>()))
							{
								if (flag)
								{
									list.Add(gameObject);
								}
								result = true;
							}
						}
						else if (Vector3.Distance(base.agent.position, transform.position) <= this.maxDistance.value && Vector3.Angle(transform.position - base.agent.position, base.agent.forward) <= this.viewAngle.value && (!Physics.Linecast(base.agent.position + this.offset, transform.position + this.offset, out this.hit, this.layerMask.value) || !(this.hit.collider != transform.GetComponent<Collider>())))
						{
							if (flag)
							{
								list.Add(gameObject);
							}
							result = true;
						}
					}
				}
			}
			if (flag)
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

		// Token: 0x06000069 RID: 105 RVA: 0x000034D4 File Offset: 0x000016D4
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.DrawLine(base.agent.position, base.agent.position + this.offset);
				Gizmos.DrawLine(base.agent.position + this.offset, base.agent.position + this.offset + base.agent.forward * this.maxDistance.value);
				Gizmos.DrawWireSphere(base.agent.position + this.offset + base.agent.forward * this.maxDistance.value, 0.1f);
				Gizmos.DrawWireSphere(base.agent.position, this.awarnessDistance.value);
				Gizmos.matrix = Matrix4x4.TRS(base.agent.position + this.offset, base.agent.rotation, Vector3.one);
				Gizmos.DrawFrustum(Vector3.zero, this.viewAngle.value, 5f, 0f, 1f);
			}
		}

		// Token: 0x04000048 RID: 72
		public BBParameter<List<GameObject>> targetObjects;

		// Token: 0x04000049 RID: 73
		public BBParameter<float> maxDistance = 50f;

		// Token: 0x0400004A RID: 74
		public BBParameter<LayerMask> layerMask = -1;

		// Token: 0x0400004B RID: 75
		public BBParameter<float> awarnessDistance = 0f;

		// Token: 0x0400004C RID: 76
		[SliderField(1, 180)]
		public BBParameter<float> viewAngle = 70f;

		// Token: 0x0400004D RID: 77
		public Vector3 offset;

		// Token: 0x0400004E RID: 78
		[BlackboardOnly]
		public BBParameter<List<GameObject>> allResults;

		// Token: 0x0400004F RID: 79
		[BlackboardOnly]
		public BBParameter<GameObject> closerResult;

		// Token: 0x04000050 RID: 80
		private RaycastHit hit;
	}
}
