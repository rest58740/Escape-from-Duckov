using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014E RID: 334
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PipeRenderer : MonoBehaviour
{
	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x0002E888 File Offset: 0x0002CA88
	public PipeRenderer.OrientedPoint[] extrudeShape
	{
		get
		{
			if (this.extrudeShapeProvider == null)
			{
				Debug.LogWarning("Extrude shape is null, please add an extrude shape such as \"CircularExtrudeShape\"");
				return new PipeRenderer.OrientedPoint[]
				{
					new PipeRenderer.OrientedPoint
					{
						position = Vector3.zero
					}
				};
			}
			return this.extrudeShapeProvider.GenerateShape();
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0002E8DC File Offset: 0x0002CADC
	public PipeRenderer.OrientedPoint[] splineShape
	{
		get
		{
			if (this.splineShapeProvider == null)
			{
				Debug.LogWarning("Spline shape is null, please add a spline shape such as \"Beveled Line Shape\"");
				return new PipeRenderer.OrientedPoint[]
				{
					new PipeRenderer.OrientedPoint
					{
						position = Vector3.zero
					}
				};
			}
			return this.splineShapeProvider.GenerateShape();
		}
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0002E92F File Offset: 0x0002CB2F
	public float GetTotalLength()
	{
		return PipeHelperFunctions.GetTotalLength(this.splineInUse);
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0002E93C File Offset: 0x0002CB3C
	public static Mesh GeneratePipeMesh(PipeRenderer.OrientedPoint[] extrudeShape, PipeRenderer.OrientedPoint[] splineShape, Color vertexColor, float uvTwist = 0f, float extrudeShapeScale = 1f, AnimationCurve extrudeShapeScaleCurve = null, float sectionLength = 0f, bool caps = false, bool recalculateNormal = true, bool revertFaces = false)
	{
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<int> list3 = new List<int>();
		List<Vector2> list4 = new List<Vector2>();
		float num = 0f;
		float totalLength = PipeHelperFunctions.GetTotalLength(splineShape);
		if (sectionLength <= 0f)
		{
			sectionLength = totalLength;
		}
		for (int i = 0; i < splineShape.Length; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint = splineShape[i];
			Vector3 position = orientedPoint.position;
			Quaternion rotation = orientedPoint.rotation;
			if (i > 0)
			{
				PipeRenderer.OrientedPoint orientedPoint2 = splineShape[i - 1];
				num += (position - orientedPoint2.position).magnitude;
			}
			float num2 = num % sectionLength / sectionLength;
			float num3 = (extrudeShapeScaleCurve != null) ? extrudeShapeScaleCurve.Evaluate(num2) : 1f;
			foreach (PipeRenderer.OrientedPoint orientedPoint3 in extrudeShape)
			{
				Vector3 vector = position + extrudeShapeScale * num3 * (rotation * orientedPoint3.position);
				Vector3 vector2 = recalculateNormal ? (vector - position).normalized : (rotation * orientedPoint3.normal);
				Vector2 item;
				item..ctor(orientedPoint3.uv.y + num * uvTwist, orientedPoint.uv.x);
				list.Add(vector);
				list2.Add(revertFaces ? (-vector2) : vector2);
				list4.Add(item);
			}
			if (i > 0)
			{
				int num4 = i * extrudeShape.Length;
				for (int k = 0; k < extrudeShape.Length - 1; k++)
				{
					int num5 = num4 + k;
					int num6 = num4 + k + 1;
					int item2 = num6 - extrudeShape.Length;
					int item3 = num5 - extrudeShape.Length;
					if (revertFaces)
					{
						list3.Add(num6);
						list3.Add(item3);
						list3.Add(num5);
						list3.Add(item2);
						list3.Add(item3);
						list3.Add(num6);
					}
					else
					{
						list3.Add(num5);
						list3.Add(item3);
						list3.Add(num6);
						list3.Add(num6);
						list3.Add(item3);
						list3.Add(item2);
					}
				}
			}
		}
		if (caps)
		{
			Vector3 item4 = -splineShape[0].tangent;
			int num7 = 0;
			int[] array = new int[extrudeShape.Length];
			for (int l = 0; l < extrudeShape.Length; l++)
			{
				int index = num7 + l;
				list.Add(list[index]);
				list2.Add(item4);
				list4.Add(list4[index]);
				array[l] = list.Count - 1;
			}
			Vector3 vector3 = Vector3.zero;
			for (int m = 0; m < array.Length; m++)
			{
				vector3 += list[m];
			}
			vector3 /= (float)array.Length;
			list.Add(vector3);
			list2.Add(item4);
			list4.Add(Vector2.one * 0f / 5f);
			int item5 = list.Count - 1;
			for (int n = 0; n < array.Length - 1; n++)
			{
				list3.Add(item5);
				list3.Add(array[n + 1]);
				list3.Add(array[n]);
			}
			item4 = splineShape[splineShape.Length - 1].tangent;
			num7 = extrudeShape.Length * (splineShape.Length - 1);
			for (int num8 = 0; num8 < extrudeShape.Length; num8++)
			{
				int index2 = num7 + num8;
				list.Add(list[index2]);
				list2.Add(item4);
				list4.Add(list4[index2]);
				array[num8] = list.Count - 1;
			}
			vector3 = Vector3.zero;
			for (int num9 = 0; num9 < array.Length; num9++)
			{
				vector3 += list[array[num9]];
			}
			vector3 /= (float)array.Length;
			list.Add(vector3);
			list2.Add(item4);
			list4.Add(Vector2.one * 0f / 5f);
			item5 = list.Count - 1;
			for (int num10 = 0; num10 < array.Length - 1; num10++)
			{
				list3.Add(array[num10]);
				list3.Add(array[num10 + 1]);
				list3.Add(item5);
			}
		}
		Color[] array2 = new Color[list.Count];
		for (int num11 = 0; num11 < array2.Length; num11++)
		{
			array2[num11] = vertexColor;
		}
		Mesh mesh = new Mesh();
		mesh.vertices = list.ToArray();
		mesh.uv = list4.ToArray();
		mesh.triangles = list3.ToArray();
		mesh.normals = list2.ToArray();
		mesh.RecalculateTangents();
		mesh.colors = array2;
		mesh.name = "Generated Mesh";
		return mesh;
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x0002EE04 File Offset: 0x0002D004
	private void OnDrawGizmosSelected()
	{
		if (this.meshFilter == null)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
		this.splineInUse = this.splineShape;
		this.meshFilter.mesh = PipeRenderer.GeneratePipeMesh(this.extrudeShape, this.splineInUse, this.vertexColor, this.uvTwist, this.extrudeShapeScale, this.useExtrudeShapeScaleCurve ? this.extrudeShapeScaleCurve : null, this.sectionLength, this.caps, this.recalculateNormal, this.revertFaces);
		if (this.drawSplinePoints)
		{
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			for (int i = 0; i < this.splineInUse.Length; i++)
			{
				PipeRenderer.OrientedPoint orientedPoint = this.splineInUse[i];
				Vector3 vector = localToWorldMatrix.MultiplyPoint(orientedPoint.position);
				Gizmos.DrawWireCube(vector, Vector3.one * 0.01f);
				Vector3 vector2 = localToWorldMatrix.MultiplyVector(orientedPoint.tangent);
				Gizmos.DrawLine(vector, vector + vector2 * 0.02f);
			}
		}
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0002EF08 File Offset: 0x0002D108
	private void Start()
	{
		if (this.meshFilter == null)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x0002EF24 File Offset: 0x0002D124
	public Vector3 GetPositionByOffset(float offset, out Quaternion rotation)
	{
		if (this.splineInUse == null || this.splineInUse.Length < 1)
		{
			rotation = Quaternion.identity;
			return Vector3.zero;
		}
		if (offset <= 0f)
		{
			rotation = this.splineInUse[0].rotation;
			return this.splineInUse[0].position;
		}
		float num = 0f;
		for (int i = 1; i < this.splineInUse.Length; i++)
		{
			PipeRenderer.OrientedPoint orientedPoint = this.splineInUse[i];
			PipeRenderer.OrientedPoint orientedPoint2 = this.splineInUse[i - 1];
			float num2 = num;
			float magnitude = (orientedPoint.position - orientedPoint2.position).magnitude;
			num += magnitude;
			float num3 = num;
			if (num3 > offset)
			{
				float num4 = num3 - num2;
				float num5 = (offset - num2) / num4;
				rotation = Quaternion.Lerp(orientedPoint2.rotation, orientedPoint.rotation, num5);
				return orientedPoint2.position + (orientedPoint.position - orientedPoint2.position) * num5;
			}
		}
		rotation = this.splineInUse[this.splineInUse.Length - 1].rotation;
		return this.splineInUse[this.splineInUse.Length - 1].position;
	}

	// Token: 0x0400095C RID: 2396
	public MeshFilter meshFilter;

	// Token: 0x0400095D RID: 2397
	public ShapeProvider splineShapeProvider;

	// Token: 0x0400095E RID: 2398
	public ShapeProvider extrudeShapeProvider;

	// Token: 0x0400095F RID: 2399
	[Header("UV")]
	public float uvTwist;

	// Token: 0x04000960 RID: 2400
	[Header("Options")]
	public float extrudeShapeScale = 1f;

	// Token: 0x04000961 RID: 2401
	public float sectionLength = 10f;

	// Token: 0x04000962 RID: 2402
	public bool caps;

	// Token: 0x04000963 RID: 2403
	public bool useExtrudeShapeScaleCurve;

	// Token: 0x04000964 RID: 2404
	public AnimationCurve extrudeShapeScaleCurve = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x04000965 RID: 2405
	public Color vertexColor = Color.white;

	// Token: 0x04000966 RID: 2406
	public bool recalculateNormal = true;

	// Token: 0x04000967 RID: 2407
	public bool revertFaces;

	// Token: 0x04000968 RID: 2408
	[Header("Gizmos")]
	public bool drawSplinePoints;

	// Token: 0x04000969 RID: 2409
	public PipeRenderer.OrientedPoint[] splineInUse;

	// Token: 0x020004C5 RID: 1221
	[Serializable]
	public struct OrientedPoint
	{
		// Token: 0x04001D14 RID: 7444
		public Vector3 position;

		// Token: 0x04001D15 RID: 7445
		public Quaternion rotation;

		// Token: 0x04001D16 RID: 7446
		public Vector3 tangent;

		// Token: 0x04001D17 RID: 7447
		public Vector3 rotationalAxisVector;

		// Token: 0x04001D18 RID: 7448
		public Vector3 normal;

		// Token: 0x04001D19 RID: 7449
		public Vector2 uv;
	}
}
