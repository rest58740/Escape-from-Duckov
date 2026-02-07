using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000215 RID: 533
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[AddComponentMenu("Splines/Spline Profile Extrude")]
	public class SplineProfileExtrude : MonoBehaviour
	{
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x0003ED63 File Offset: 0x0003CF63
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x0003ED6B File Offset: 0x0003CF6B
		// (set) Token: 0x06000FD7 RID: 4055 RVA: 0x0003ED73 File Offset: 0x0003CF73
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

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x0003ED7C File Offset: 0x0003CF7C
		[Obsolete("Use RebuildOnSplineChange instead.", false)]
		public bool rebuildOnSplineChange
		{
			get
			{
				return this.RebuildOnSplineChange;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x0003ED84 File Offset: 0x0003CF84
		// (set) Token: 0x06000FDA RID: 4058 RVA: 0x0003ED8C File Offset: 0x0003CF8C
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

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x0003ED95 File Offset: 0x0003CF95
		// (set) Token: 0x06000FDC RID: 4060 RVA: 0x0003ED9D File Offset: 0x0003CF9D
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

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x0003EDAC File Offset: 0x0003CFAC
		// (set) Token: 0x06000FDE RID: 4062 RVA: 0x0003EDB4 File Offset: 0x0003CFB4
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

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000FDF RID: 4063 RVA: 0x0003EDC7 File Offset: 0x0003CFC7
		// (set) Token: 0x06000FE0 RID: 4064 RVA: 0x0003EDCF File Offset: 0x0003CFCF
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

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x0003EDE2 File Offset: 0x0003CFE2
		public int ProfileSeg
		{
			get
			{
				return this.profile.Length;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x0003EDEC File Offset: 0x0003CFEC
		// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x0003EDF4 File Offset: 0x0003CFF4
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

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0003EDFD File Offset: 0x0003CFFD
		// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x0003EE05 File Offset: 0x0003D005
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

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0003EE34 File Offset: 0x0003D034
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

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0003EE47 File Offset: 0x0003D047
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

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0003EE5C File Offset: 0x0003D05C
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

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0003EED4 File Offset: 0x0003D0D4
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

		// Token: 0x06000FEA RID: 4074 RVA: 0x0003EF33 File Offset: 0x0003D133
		private void OnEnable()
		{
			Spline.Changed += this.OnSplineChanged;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0003EF46 File Offset: 0x0003D146
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0003EF59 File Offset: 0x0003D159
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (this.m_Container != null && this.Splines.Contains(spline) && this.m_RebuildOnSplineChange)
			{
				this.m_RebuildRequested = true;
			}
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0003EF86 File Offset: 0x0003D186
		private void Update()
		{
			if (this.m_RebuildRequested && Time.time >= this.m_NextScheduledRebuild)
			{
				this.Rebuild();
			}
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0003EFA4 File Offset: 0x0003D1A4
		public void Rebuild()
		{
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				return;
			}
			this.Extrude<Spline>(this.Splines[0], this.profile, this.m_Mesh, this.m_SegmentsPerUnit, this.m_Range);
			this.m_NextScheduledRebuild = Time.time + 1f / (float)this.m_RebuildFrequency;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0003F018 File Offset: 0x0003D218
		private void Extrude<T>(T spline, SplineProfileExtrude.Vertex[] profile, Mesh mesh, float segmentsPerUnit, float2 range) where T : ISpline
		{
			int num = profile.Length;
			if (num < 2)
			{
				return;
			}
			float num2 = Mathf.Abs(range.y - range.x);
			int num3 = Mathf.Max((int)Mathf.Ceil(spline.GetLength() * num2 * segmentsPerUnit), 1);
			float num4 = 0f;
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<Vector2> list3 = new List<Vector2>();
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < num3; i++)
			{
				float num5 = math.lerp(range.x, range.y, (float)i / ((float)num3 - 1f));
				if (num5 > 1f)
				{
					num5 = 1f;
				}
				if (num5 < 1E-07f)
				{
					num5 = 1E-07f;
				}
				float3 @float;
				float3 float2;
				float3 float3;
				SplineUtility.Evaluate<T>(spline, num5, ref @float, ref float2, ref float3);
				Vector3 normalized = float2.normalized;
				Vector3 normalized2 = float3.normalized;
				Vector3 vector2 = Vector3.Cross(normalized, normalized2);
				float num6 = 1f / (float)(num - 1);
				if (i > 0)
				{
					num4 += (@float - vector).magnitude;
				}
				for (int j = 0; j < num; j++)
				{
					SplineProfileExtrude.Vertex vertex = profile[j];
					float u = vertex.u;
					float y = vertex.position.y;
					float x = vertex.position.x;
					float z = vertex.position.z;
					Vector3 item = Quaternion.FromToRotation(Vector3.up, normalized2) * vertex.normal;
					Vector3 item2 = @float + x * vector2 + y * normalized2 + z * normalized;
					list.Add(item2);
					list3.Add(new Vector2(u * this.uFactor, num4 * this.vFactor));
					list2.Add(item);
				}
				vector = @float;
			}
			SplineProfileExtrude.<>c__DisplayClass53_0<T> CS$<>8__locals1;
			CS$<>8__locals1.triangles = new List<int>();
			for (int k = 0; k < num3 - 1; k++)
			{
				int num7 = k * num;
				for (int l = 0; l < num - 1; l++)
				{
					int num8 = num7 + l;
					SplineProfileExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num8,
						num8 + 1,
						num8 + num
					}, ref CS$<>8__locals1);
					SplineProfileExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num8 + 1,
						num8 + 1 + num,
						num8 + num
					}, ref CS$<>8__locals1);
				}
			}
			mesh.Clear();
			mesh.vertices = list.ToArray();
			mesh.uv = list3.ToArray();
			mesh.triangles = CS$<>8__locals1.triangles.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0003F2DD File Offset: 0x0003D4DD
		private void OnValidate()
		{
			this.Rebuild();
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0003F2E5 File Offset: 0x0003D4E5
		internal Mesh CreateMeshAsset()
		{
			return new Mesh
			{
				name = base.name
			};
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003F2F8 File Offset: 0x0003D4F8
		private void FlattenSpline()
		{
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003F36A File Offset: 0x0003D56A
		[CompilerGenerated]
		internal static void <Extrude>g__AddTriangles|53_0<T>(int[] indicies, ref SplineProfileExtrude.<>c__DisplayClass53_0<T> A_1) where T : ISpline
		{
			A_1.triangles.AddRange(indicies);
		}

		// Token: 0x04000CED RID: 3309
		[SerializeField]
		[Tooltip("The Spline to extrude.")]
		private SplineContainer m_Container;

		// Token: 0x04000CEE RID: 3310
		[SerializeField]
		private SplineProfileExtrude.Vertex[] profile;

		// Token: 0x04000CEF RID: 3311
		[SerializeField]
		[Tooltip("Enable to regenerate the extruded mesh when the target Spline is modified. Disable this option if the Spline will not be modified at runtime.")]
		private bool m_RebuildOnSplineChange;

		// Token: 0x04000CF0 RID: 3312
		[SerializeField]
		[Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
		private int m_RebuildFrequency = 30;

		// Token: 0x04000CF1 RID: 3313
		[SerializeField]
		[Tooltip("Automatically update any Mesh, Box, or Sphere collider components when the mesh is extruded.")]
		private bool m_UpdateColliders = true;

		// Token: 0x04000CF2 RID: 3314
		[SerializeField]
		[Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
		private float m_SegmentsPerUnit = 4f;

		// Token: 0x04000CF3 RID: 3315
		[SerializeField]
		[Tooltip("The radius of the extruded mesh.")]
		private float m_Width = 0.25f;

		// Token: 0x04000CF4 RID: 3316
		[SerializeField]
		private float m_Height = 0.05f;

		// Token: 0x04000CF5 RID: 3317
		[SerializeField]
		[Tooltip("The section of the Spline to extrude.")]
		private Vector2 m_Range = new Vector2(0f, 1f);

		// Token: 0x04000CF6 RID: 3318
		[SerializeField]
		private float uFactor = 1f;

		// Token: 0x04000CF7 RID: 3319
		[SerializeField]
		private float vFactor = 1f;

		// Token: 0x04000CF8 RID: 3320
		private Mesh m_Mesh;

		// Token: 0x04000CF9 RID: 3321
		private bool m_RebuildRequested;

		// Token: 0x04000CFA RID: 3322
		private float m_NextScheduledRebuild;

		// Token: 0x0200050A RID: 1290
		[Serializable]
		private struct Vertex
		{
			// Token: 0x04001E34 RID: 7732
			public Vector3 position;

			// Token: 0x04001E35 RID: 7733
			public Vector3 normal;

			// Token: 0x04001E36 RID: 7734
			public float u;
		}
	}
}
