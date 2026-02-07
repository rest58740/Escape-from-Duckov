using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200001E RID: 30
	[Category("GameObject")]
	[Description("A combination of line of sight and view angle check")]
	public class CanSeeTargetAny2D : ConditionTask<Transform>
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003689 File Offset: 0x00001889
		protected override string info
		{
			get
			{
				string text = "Can See Any ";
				BBParameter<List<GameObject>> bbparameter = this.targetObjects;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000036A8 File Offset: 0x000018A8
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
						if (Vector2.Distance(base.agent.position, transform.position) < this.awarnessDistance.value)
						{
							if (!(Physics2D.Linecast(base.agent.position + this.offset, transform.position + this.offset, this.layerMask.value).collider != transform.GetComponent<Collider2D>()))
							{
								if (flag)
								{
									list.Add(gameObject);
								}
								result = true;
							}
						}
						else if (Vector2.Distance(base.agent.position, transform.position) <= this.maxDistance.value && Vector2.Angle(transform.position - base.agent.position, base.agent.right) <= this.viewAngle.value && !(Physics2D.Linecast(base.agent.position + this.offset, transform.position + this.offset, this.layerMask.value).collider != transform.GetComponent<Collider2D>()))
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

		// Token: 0x0600006E RID: 110 RVA: 0x00003940 File Offset: 0x00001B40
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.DrawLine(base.agent.position, base.agent.position + this.offset);
				Gizmos.DrawLine(base.agent.position + this.offset, base.agent.position + this.offset + base.agent.right * this.maxDistance.value);
				Gizmos.DrawWireSphere(base.agent.position + this.offset + base.agent.right * this.maxDistance.value, 0.1f);
				Gizmos.DrawWireSphere(base.agent.position, this.awarnessDistance.value);
				Gizmos.matrix = Matrix4x4.TRS(base.agent.position + this.offset, base.agent.rotation, Vector3.one);
				Gizmos.DrawFrustum(Vector3.zero, this.viewAngle.value, 5f, 0f, 1f);
			}
		}

		// Token: 0x04000051 RID: 81
		public BBParameter<List<GameObject>> targetObjects;

		// Token: 0x04000052 RID: 82
		public BBParameter<float> maxDistance = 50f;

		// Token: 0x04000053 RID: 83
		public BBParameter<LayerMask> layerMask = -1;

		// Token: 0x04000054 RID: 84
		public BBParameter<float> awarnessDistance = 0f;

		// Token: 0x04000055 RID: 85
		[SliderField(1, 180)]
		public BBParameter<float> viewAngle = 70f;

		// Token: 0x04000056 RID: 86
		public Vector2 offset;

		// Token: 0x04000057 RID: 87
		[BlackboardOnly]
		public BBParameter<List<GameObject>> allResults;

		// Token: 0x04000058 RID: 88
		[BlackboardOnly]
		public BBParameter<GameObject> closerResult;
	}
}
