using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000216 RID: 534
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[AddComponentMenu("Splines/Spline Sidewalk Extrude")]
	public class SplineSidewalkExtrude : MonoBehaviour
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x0003F378 File Offset: 0x0003D578
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x0003F380 File Offset: 0x0003D580
		// (set) Token: 0x06000FF7 RID: 4087 RVA: 0x0003F388 File Offset: 0x0003D588
		public SplineContainer Container
		{
			get
			{
				return this.m_Container;
			}
			set
			{
				this.m_Container = value;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x0003F391 File Offset: 0x0003D591
		[Obsolete("Use RebuildOnSplineChange instead.", false)]
		public bool rebuildOnSplineChange
		{
			get
			{
				return this.RebuildOnSplineChange;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x0003F399 File Offset: 0x0003D599
		// (set) Token: 0x06000FFA RID: 4090 RVA: 0x0003F3A1 File Offset: 0x0003D5A1
		public bool RebuildOnSplineChange
		{
			get
			{
				return this.m_RebuildOnSplineChange;
			}
			set
			{
				this.m_RebuildOnSplineChange = value;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0003F3AA File Offset: 0x0003D5AA
		// (set) Token: 0x06000FFC RID: 4092 RVA: 0x0003F3B2 File Offset: 0x0003D5B2
		public int RebuildFrequency
		{
			get
			{
				return this.m_RebuildFrequency;
			}
			set
			{
				this.m_RebuildFrequency = Mathf.Max(value, 1);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0003F3C1 File Offset: 0x0003D5C1
		// (set) Token: 0x06000FFE RID: 4094 RVA: 0x0003F3C9 File Offset: 0x0003D5C9
		public float SegmentsPerUnit
		{
			get
			{
				return this.m_SegmentsPerUnit;
			}
			set
			{
				this.m_SegmentsPerUnit = Mathf.Max(value, 0.0001f);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000FFF RID: 4095 RVA: 0x0003F3DC File Offset: 0x0003D5DC
		// (set) Token: 0x06001000 RID: 4096 RVA: 0x0003F3E4 File Offset: 0x0003D5E4
		public float Width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = Mathf.Max(value, 1E-05f);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x0003F3F7 File Offset: 0x0003D5F7
		// (set) Token: 0x06001002 RID: 4098 RVA: 0x0003F3FF File Offset: 0x0003D5FF
		public float Height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x0003F408 File Offset: 0x0003D608
		// (set) Token: 0x06001004 RID: 4100 RVA: 0x0003F410 File Offset: 0x0003D610
		public Vector2 Range
		{
			get
			{
				return this.m_Range;
			}
			set
			{
				this.m_Range = new Vector2(Mathf.Min(value.x, value.y), Mathf.Max(value.x, value.y));
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06001005 RID: 4101 RVA: 0x0003F43F File Offset: 0x0003D63F
		public Spline Spline
		{
			get
			{
				SplineContainer container = this.m_Container;
				if (container == null)
				{
					return null;
				}
				return container.Spline;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x0003F452 File Offset: 0x0003D652
		public IReadOnlyList<Spline> Splines
		{
			get
			{
				SplineContainer container = this.m_Container;
				if (container == null)
				{
					return null;
				}
				return container.Splines;
			}
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0003F468 File Offset: 0x0003D668
		internal void Reset()
		{
			base.TryGetComponent<SplineContainer>(ref this.m_Container);
			MeshFilter meshFilter;
			if (base.TryGetComponent<MeshFilter>(ref meshFilter))
			{
				meshFilter.sharedMesh = (this.m_Mesh = this.CreateMeshAsset());
			}
			MeshRenderer meshRenderer;
			if (base.TryGetComponent<MeshRenderer>(ref meshRenderer) && meshRenderer.sharedMaterial == null)
			{
				GameObject gameObject = GameObject.CreatePrimitive(3);
				Material sharedMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
				Object.DestroyImmediate(gameObject);
				meshRenderer.sharedMaterial = sharedMaterial;
			}
			this.Rebuild();
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0003F4E0 File Offset: 0x0003D6E0
		private void Start()
		{
			if (this.m_Container == null || this.m_Container.Spline == null)
			{
				Debug.LogError("Spline Extrude does not have a valid SplineContainer set.");
				return;
			}
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				Debug.LogError("SplineExtrude.createMeshInstance is disabled, but there is no valid mesh assigned. Please create or assign a writable mesh asset.");
			}
			this.Rebuild();
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0003F53F File Offset: 0x0003D73F
		private void OnEnable()
		{
			Spline.Changed += this.OnSplineChanged;
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0003F552 File Offset: 0x0003D752
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0003F565 File Offset: 0x0003D765
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (this.m_Container != null && this.Splines.Contains(spline) && this.m_RebuildOnSplineChange)
			{
				this.m_RebuildRequested = true;
			}
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0003F592 File Offset: 0x0003D792
		private void Update()
		{
			if (this.m_RebuildRequested && Time.time >= this.m_NextScheduledRebuild)
			{
				this.Rebuild();
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0003F5B0 File Offset: 0x0003D7B0
		public void Rebuild()
		{
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				return;
			}
			this.Extrude<Spline>(this.Splines[0], this.m_Mesh, this.m_SegmentsPerUnit, this.m_Range);
			this.m_NextScheduledRebuild = Time.time + 1f / (float)this.m_RebuildFrequency;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0003F61C File Offset: 0x0003D81C
		private void Extrude<T>(T spline, Mesh mesh, float segmentsPerUnit, float2 range) where T : ISpline
		{
			SplineSidewalkExtrude.<>c__DisplayClass55_0<T> CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			mesh.Clear();
			if (this.sides == SplineSidewalkExtrude.Sides.None)
			{
				return;
			}
			float num = Mathf.Abs(range.y - range.x);
			int num2 = Mathf.Max((int)Mathf.Ceil(spline.GetLength() * num * segmentsPerUnit), 1);
			CS$<>8__locals1.v = 0f;
			CS$<>8__locals1.verts = new List<Vector3>();
			CS$<>8__locals1.n = new List<Vector3>();
			CS$<>8__locals1.uv = new List<Vector2>();
			CS$<>8__locals1.triangles = new List<int>();
			Vector3 vector = Vector3.zero;
			SplineSidewalkExtrude.ProfileLine[] array = this.GenerateProfile();
			CS$<>8__locals1.profileVertexCount = array.Length * 2;
			for (int i = 0; i < num2; i++)
			{
				SplineSidewalkExtrude.<>c__DisplayClass55_1<T> CS$<>8__locals2;
				CS$<>8__locals2.isLastSegment = (i == num2 - 1);
				float num3 = math.lerp(range.x, range.y, (float)i / ((float)num2 - 1f));
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				if (num3 < 1E-07f)
				{
					num3 = 1E-07f;
				}
				float3 @float;
				float3 float2;
				SplineUtility.Evaluate<T>(spline, num3, ref CS$<>8__locals2.center, ref @float, ref float2);
				CS$<>8__locals2.forward = @float.normalized;
				CS$<>8__locals2.up = float2.normalized;
				CS$<>8__locals2.right = Vector3.Cross(CS$<>8__locals2.forward, CS$<>8__locals2.up);
				if (i > 0)
				{
					CS$<>8__locals1.v += (CS$<>8__locals2.center - vector).magnitude;
				}
				foreach (SplineSidewalkExtrude.ProfileLine profileLine in array)
				{
					this.<Extrude>g__DrawLine|55_2<T>(profileLine.start, profileLine.end, profileLine.u0, profileLine.u1, ref CS$<>8__locals1, ref CS$<>8__locals2);
				}
				vector = CS$<>8__locals2.center;
			}
			mesh.vertices = CS$<>8__locals1.verts.ToArray();
			mesh.uv = CS$<>8__locals1.uv.ToArray();
			mesh.triangles = CS$<>8__locals1.triangles.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0003F850 File Offset: 0x0003DA50
		private SplineSidewalkExtrude.ProfileLine[] GenerateProfile()
		{
			SplineSidewalkExtrude.<>c__DisplayClass57_0 CS$<>8__locals1;
			CS$<>8__locals1.lines = new List<SplineSidewalkExtrude.ProfileLine>();
			float num = this.height - this.bevel;
			float num2 = Mathf.Sqrt(2f * this.bevel * this.bevel);
			float num3 = this.width - 2f * this.bevel;
			CS$<>8__locals1.uFactor = num + num2 + num3 + num2 + num;
			if ((this.sides | SplineSidewalkExtrude.Sides.Left) == this.sides)
			{
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset - this.width, 0f, -this.offset - this.width, this.height - this.bevel, 0f, num, ref CS$<>8__locals1);
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset - this.width + this.bevel, this.height, -this.offset - this.bevel, this.height, num + num2, num + num2 + num3, ref CS$<>8__locals1);
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset, this.height - this.bevel, -this.offset, 0f, num + num2 + num3 + num2, num + num2 + num3 + num2 + num, ref CS$<>8__locals1);
				if (this.bevel > 0f)
				{
					SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset - this.width, this.height - this.bevel, -this.offset - this.width + this.bevel, this.height, num, num + num2, ref CS$<>8__locals1);
					SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset - this.bevel, this.height, -this.offset, this.height - this.bevel, num + num2 + num3, num + num2 + num3 + num2, ref CS$<>8__locals1);
				}
			}
			if ((this.sides | SplineSidewalkExtrude.Sides.Right) == this.sides)
			{
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset, 0f, this.offset, this.height - this.bevel, num + num2 + num3 + num2 + num, num + num2 + num3 + num2, ref CS$<>8__locals1);
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset + this.bevel, this.height, this.offset + this.width - this.bevel, this.height, num + num2 + num3, num + num2, ref CS$<>8__locals1);
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset + this.width, this.height - this.bevel, this.offset + this.width, 0f, num, 0f, ref CS$<>8__locals1);
				if (this.bevel > 0f)
				{
					SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset, this.height - this.bevel, this.offset + this.bevel, this.height, num + num2 + num3 + num2, num + num2 + num3, ref CS$<>8__locals1);
					SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset + this.width - this.bevel, this.height, this.offset + this.width, this.height - this.bevel, num + num2, num, ref CS$<>8__locals1);
				}
			}
			return CS$<>8__locals1.lines.ToArray();
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0003FB4D File Offset: 0x0003DD4D
		private void OnValidate()
		{
			this.Rebuild();
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0003FB55 File Offset: 0x0003DD55
		internal Mesh CreateMeshAsset()
		{
			return new Mesh
			{
				name = base.name
			};
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0003FB68 File Offset: 0x0003DD68
		private void FlattenSpline()
		{
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0003FBE4 File Offset: 0x0003DDE4
		[CompilerGenerated]
		internal static Vector3 <Extrude>g__ProfileToObject|55_1<T>(Vector3 profilePos, ref SplineSidewalkExtrude.<>c__DisplayClass55_1<T> A_1) where T : ISpline
		{
			return A_1.center + profilePos.x * A_1.right + profilePos.y * A_1.up + profilePos.z * A_1.forward;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0003FC40 File Offset: 0x0003DE40
		[CompilerGenerated]
		private void <Extrude>g__DrawLine|55_2<T>(Vector3 p0, Vector3 p1, float u0, float u1, ref SplineSidewalkExtrude.<>c__DisplayClass55_0<T> A_5, ref SplineSidewalkExtrude.<>c__DisplayClass55_1<T> A_6) where T : ISpline
		{
			Vector3 vector = SplineSidewalkExtrude.<Extrude>g__ProfileToObject|55_1<T>(p0, ref A_6);
			Vector3 vector2 = SplineSidewalkExtrude.<Extrude>g__ProfileToObject|55_1<T>(p1, ref A_6);
			Vector3 item = Vector3.Cross(vector2 - vector, A_6.forward);
			int count = A_5.verts.Count;
			A_5.verts.Add(vector);
			A_5.verts.Add(vector2);
			A_5.n.Add(item);
			A_5.n.Add(item);
			A_5.uv.Add(new Vector2(u0 * this.uFactor, A_5.v * this.vFactor));
			A_5.uv.Add(new Vector2(u1 * this.uFactor, A_5.v * this.vFactor));
			if (!A_6.isLastSegment)
			{
				this.<Extrude>g__AddTriangles|55_0<T>(new int[]
				{
					count,
					count + 1,
					count + A_5.profileVertexCount
				}, ref A_5);
				this.<Extrude>g__AddTriangles|55_0<T>(new int[]
				{
					count + 1,
					count + 1 + A_5.profileVertexCount,
					count + A_5.profileVertexCount
				}, ref A_5);
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0003FD5E File Offset: 0x0003DF5E
		[CompilerGenerated]
		private void <Extrude>g__AddTriangles|55_0<T>(int[] indicies, ref SplineSidewalkExtrude.<>c__DisplayClass55_0<T> A_2) where T : ISpline
		{
			A_2.triangles.AddRange(indicies);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0003FD6C File Offset: 0x0003DF6C
		[CompilerGenerated]
		internal static void <GenerateProfile>g__Add|57_0(float x0, float y0, float x1, float y1, float u0, float u1, ref SplineSidewalkExtrude.<>c__DisplayClass57_0 A_6)
		{
			A_6.lines.Add(new SplineSidewalkExtrude.ProfileLine(new Vector3(x0, y0), new Vector3(x1, y1), u0 / A_6.uFactor, u1 / A_6.uFactor));
		}

		// Token: 0x04000CFB RID: 3323
		[SerializeField]
		[Tooltip("The Spline to extrude.")]
		private SplineContainer m_Container;

		// Token: 0x04000CFC RID: 3324
		[SerializeField]
		private float offset;

		// Token: 0x04000CFD RID: 3325
		[SerializeField]
		private float height;

		// Token: 0x04000CFE RID: 3326
		[SerializeField]
		private float width;

		// Token: 0x04000CFF RID: 3327
		[SerializeField]
		private float bevel;

		// Token: 0x04000D00 RID: 3328
		[SerializeField]
		private SplineSidewalkExtrude.Sides sides = SplineSidewalkExtrude.Sides.Both;

		// Token: 0x04000D01 RID: 3329
		[SerializeField]
		[Tooltip("Enable to regenerate the extruded mesh when the target Spline is modified. Disable this option if the Spline will not be modified at runtime.")]
		private bool m_RebuildOnSplineChange;

		// Token: 0x04000D02 RID: 3330
		[SerializeField]
		[Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
		private int m_RebuildFrequency = 30;

		// Token: 0x04000D03 RID: 3331
		[SerializeField]
		[Tooltip("Automatically update any Mesh, Box, or Sphere collider components when the mesh is extruded.")]
		private bool m_UpdateColliders = true;

		// Token: 0x04000D04 RID: 3332
		[SerializeField]
		[Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
		private float m_SegmentsPerUnit = 4f;

		// Token: 0x04000D05 RID: 3333
		[SerializeField]
		[Tooltip("The radius of the extruded mesh.")]
		private float m_Width = 0.25f;

		// Token: 0x04000D06 RID: 3334
		[SerializeField]
		private float m_Height = 0.05f;

		// Token: 0x04000D07 RID: 3335
		[SerializeField]
		[Tooltip("The section of the Spline to extrude.")]
		private Vector2 m_Range = new Vector2(0f, 0.999f);

		// Token: 0x04000D08 RID: 3336
		[SerializeField]
		private float uFactor = 1f;

		// Token: 0x04000D09 RID: 3337
		[SerializeField]
		private float vFactor = 1f;

		// Token: 0x04000D0A RID: 3338
		private Mesh m_Mesh;

		// Token: 0x04000D0B RID: 3339
		private bool m_RebuildRequested;

		// Token: 0x04000D0C RID: 3340
		private float m_NextScheduledRebuild;

		// Token: 0x0200050C RID: 1292
		[Flags]
		public enum Sides
		{
			// Token: 0x04001E39 RID: 7737
			None = 0,
			// Token: 0x04001E3A RID: 7738
			Left = 1,
			// Token: 0x04001E3B RID: 7739
			Right = 2,
			// Token: 0x04001E3C RID: 7740
			Both = 3
		}

		// Token: 0x0200050D RID: 1293
		private struct ProfileLine
		{
			// Token: 0x06002879 RID: 10361 RVA: 0x00094033 File Offset: 0x00092233
			public ProfileLine(Vector3 start, Vector3 end, float u0, float u1)
			{
				this.start = start;
				this.end = end;
				this.u0 = u0;
				this.u1 = u1;
			}

			// Token: 0x04001E3D RID: 7741
			public Vector3 start;

			// Token: 0x04001E3E RID: 7742
			public Vector3 end;

			// Token: 0x04001E3F RID: 7743
			public float u0;

			// Token: 0x04001E40 RID: 7744
			public float u1;
		}
	}
}
