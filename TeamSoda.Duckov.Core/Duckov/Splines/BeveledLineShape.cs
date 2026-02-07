using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.Splines
{
	// Token: 0x02000341 RID: 833
	[RequireComponent(typeof(PipeRenderer))]
	public class BeveledLineShape : ShapeProvider
	{
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x00067035 File Offset: 0x00065235
		[HideInInspector]
		public List<Vector3> points
		{
			get
			{
				if (this.pointsComponent)
				{
					return this.pointsComponent.points;
				}
				return null;
			}
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x00067054 File Offset: 0x00065254
		public override PipeRenderer.OrientedPoint[] GenerateShape()
		{
			List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
			if (!this.pointsComponent || this.points.Count <= 1)
			{
				return list.ToArray();
			}
			if (this.pointsComponent.worldSpace)
			{
				this.pointsComponent.worldSpace = false;
			}
			int count = this.points.Count;
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = this.points[i];
				if (i == 0 || i == count - 1)
				{
					PipeRenderer.OrientedPoint orientedPoint = new PipeRenderer.OrientedPoint
					{
						position = vector,
						tangent = ((i == 0) ? (this.points[i + 1] - vector).normalized : (vector - this.points[i - 1]).normalized),
						normal = Vector3.up,
						rotationalAxisVector = Vector3.forward
					};
					PipeRenderer.OrientedPoint item = orientedPoint;
					list.Add(item);
				}
				else
				{
					Vector3 vector2 = this.points[i - 1];
					Vector3 vector3 = this.points[i + 1];
					Vector3 vector4 = vector - vector2;
					Vector3 vector5 = vector3 - vector2;
					Vector3 vector6 = Vector3.Cross(vector3 - vector, vector2 - vector);
					if (vector4.magnitude == 0f || vector5.magnitude == 0f || vector4.normalized == vector5.normalized || vector4.normalized == -vector5.normalized)
					{
						Vector3 normalized = (vector3 - vector).normalized;
						PipeRenderer.OrientedPoint orientedPoint = new PipeRenderer.OrientedPoint
						{
							position = vector,
							tangent = normalized,
							normal = vector6,
							rotationalAxisVector = Vector3.forward,
							rotation = Quaternion.LookRotation(normalized, vector6),
							uv = Vector2.zero
						};
						PipeRenderer.OrientedPoint item2 = orientedPoint;
						list.Add(item2);
					}
					else
					{
						float num = (i >= 2) ? ((vector - vector2).magnitude / 2f) : (vector - vector2).magnitude;
						float num2 = (i < count - 2) ? ((vector - vector3).magnitude / 2f) : (vector - vector3).magnitude;
						float clipDistance = Mathf.Min(num, num2);
						Vector3 vector7;
						Vector3 vector8;
						Vector3[] array = Bevel.Evaluate(vector, vector2, vector3, this.subdivision, this.bevelSize, out vector7, out vector8, this.protectionOffset, this.useProtectionOffset, clipDistance);
						for (int j = 0; j < array.Length; j++)
						{
							Vector3 vector9 = array[j];
							Vector3 vector10 = ((j < array.Length - 1) ? array[j + 1] : vector3) - vector9;
							Vector3 vector11 = vector7 - vector9;
							Vector3 vector12 = (this.useProtectionOffset && (j == 0 || j == array.Length - 1)) ? vector10.normalized : (Quaternion.AngleAxis(-90f, vector8) * vector11);
							float num3 = 0.001f;
							if (vector10.magnitude >= num3)
							{
								Quaternion rotation = Quaternion.LookRotation(vector12, vector6);
								Vector3 forward = Vector3.forward;
								PipeRenderer.OrientedPoint orientedPoint = new PipeRenderer.OrientedPoint
								{
									position = vector9,
									tangent = vector12,
									normal = vector6,
									rotationalAxisVector = forward,
									rotation = rotation,
									uv = Vector2.zero
								};
								PipeRenderer.OrientedPoint item3 = orientedPoint;
								list.Add(item3);
							}
						}
					}
				}
			}
			if (this.subdivideByLength && this.subdivisionLength > 0f)
			{
				for (int k = 0; k < list.Count - 1; k++)
				{
					PipeRenderer.OrientedPoint orientedPoint2 = list[k];
					PipeRenderer.OrientedPoint orientedPoint3 = list[k + 1];
					Vector3 vector13 = orientedPoint3.position - orientedPoint2.position;
					Vector3 normalized2 = vector13.normalized;
					float magnitude = vector13.magnitude;
					if (magnitude > this.subdivisionLength)
					{
						int num4 = Mathf.FloorToInt(magnitude / this.subdivisionLength);
						for (int l = 1; l <= num4; l++)
						{
							Vector3 vector14 = orientedPoint2.position + (float)l * normalized2 * this.subdivisionLength;
							if ((vector14 - orientedPoint3.position).magnitude < this.subdivisionLength)
							{
								break;
							}
							PipeRenderer.OrientedPoint orientedPoint = new PipeRenderer.OrientedPoint
							{
								position = vector14,
								normal = orientedPoint2.normal,
								rotation = orientedPoint2.rotation,
								rotationalAxisVector = orientedPoint2.rotationalAxisVector,
								tangent = orientedPoint2.tangent,
								uv = orientedPoint2.uv
							};
							PipeRenderer.OrientedPoint item4 = orientedPoint;
							list.Insert(k + l, item4);
						}
					}
				}
			}
			PipeRenderer.OrientedPoint[] array2 = list.ToArray();
			array2 = PipeHelperFunctions.RemoveDuplicates(array2, 0.0001f);
			PipeHelperFunctions.RecalculateNormals(ref array2);
			PipeHelperFunctions.RecalculateUvs(ref array2, this.uvMultiplier, this.uvOffset);
			return array2;
		}

		// Token: 0x04001415 RID: 5141
		public PipeRenderer pipeRenderer;

		// Token: 0x04001416 RID: 5142
		public Points pointsComponent;

		// Token: 0x04001417 RID: 5143
		[Header("Shape")]
		public float bevelSize = 0.5f;

		// Token: 0x04001418 RID: 5144
		[Header("Subdivide")]
		public int subdivision = 2;

		// Token: 0x04001419 RID: 5145
		public bool subdivideByLength;

		// Token: 0x0400141A RID: 5146
		public float subdivisionLength = 0.1f;

		// Token: 0x0400141B RID: 5147
		[Header("UV")]
		public float uvMultiplier = 1f;

		// Token: 0x0400141C RID: 5148
		public float uvOffset;

		// Token: 0x0400141D RID: 5149
		[Header("Extra")]
		public bool useProtectionOffset;

		// Token: 0x0400141E RID: 5150
		public float protectionOffset = 0.2f;

		// Token: 0x0400141F RID: 5151
		public bool edit;
	}
}
