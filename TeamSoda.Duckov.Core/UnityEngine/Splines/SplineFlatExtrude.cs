using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000214 RID: 532
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[AddComponentMenu("Splines/Spline Flat Extrude")]
	public class SplineFlatExtrude : MonoBehaviour
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0003E77E File Offset: 0x0003C97E
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0003E786 File Offset: 0x0003C986
		// (set) Token: 0x06000FB6 RID: 4022 RVA: 0x0003E78E File Offset: 0x0003C98E
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

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0003E797 File Offset: 0x0003C997
		[Obsolete("Use RebuildOnSplineChange instead.", false)]
		public bool rebuildOnSplineChange
		{
			get
			{
				return this.RebuildOnSplineChange;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x0003E79F File Offset: 0x0003C99F
		// (set) Token: 0x06000FB9 RID: 4025 RVA: 0x0003E7A7 File Offset: 0x0003C9A7
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

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0003E7B0 File Offset: 0x0003C9B0
		// (set) Token: 0x06000FBB RID: 4027 RVA: 0x0003E7B8 File Offset: 0x0003C9B8
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

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0003E7C7 File Offset: 0x0003C9C7
		// (set) Token: 0x06000FBD RID: 4029 RVA: 0x0003E7CF File Offset: 0x0003C9CF
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

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0003E7E2 File Offset: 0x0003C9E2
		// (set) Token: 0x06000FBF RID: 4031 RVA: 0x0003E7EA File Offset: 0x0003C9EA
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

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0003E7FD File Offset: 0x0003C9FD
		// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x0003E805 File Offset: 0x0003CA05
		public int ProfileSeg
		{
			get
			{
				return this.m_ProfileSeg;
			}
			set
			{
				this.m_ProfileSeg = value;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0003E80E File Offset: 0x0003CA0E
		// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x0003E816 File Offset: 0x0003CA16
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

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0003E81F File Offset: 0x0003CA1F
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x0003E827 File Offset: 0x0003CA27
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

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0003E856 File Offset: 0x0003CA56
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

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0003E869 File Offset: 0x0003CA69
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

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0003E87C File Offset: 0x0003CA7C
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

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0003E8F4 File Offset: 0x0003CAF4
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

		// Token: 0x06000FCA RID: 4042 RVA: 0x0003E953 File Offset: 0x0003CB53
		private void OnEnable()
		{
			Spline.Changed += this.OnSplineChanged;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0003E966 File Offset: 0x0003CB66
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0003E979 File Offset: 0x0003CB79
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (this.m_Container != null && this.Splines.Contains(spline) && this.m_RebuildOnSplineChange)
			{
				this.m_RebuildRequested = true;
			}
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0003E9A6 File Offset: 0x0003CBA6
		private void Update()
		{
			if (this.m_RebuildRequested && Time.time >= this.m_NextScheduledRebuild)
			{
				this.Rebuild();
			}
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0003E9C4 File Offset: 0x0003CBC4
		public void Rebuild()
		{
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				return;
			}
			this.Extrude<Spline>(this.Splines[0], this.m_Mesh, this.m_Width, this.m_ProfileSeg, this.m_Height, this.m_SegmentsPerUnit, this.m_Range);
			this.m_NextScheduledRebuild = Time.time + 1f / (float)this.m_RebuildFrequency;
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0003EA44 File Offset: 0x0003CC44
		private void Extrude<T>(T spline, Mesh mesh, float width, int profileSegments, float height, float segmentsPerUnit, float2 range) where T : ISpline
		{
			if (profileSegments < 2)
			{
				return;
			}
			float num = Mathf.Abs(range.y - range.x);
			int num2 = Mathf.Max((int)Mathf.Ceil(spline.GetLength() * num * segmentsPerUnit), 1);
			float num3 = 0f;
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<Vector2> list3 = new List<Vector2>();
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < num2; i++)
			{
				float num4 = math.lerp(range.x, range.y, (float)i / ((float)num2 - 1f));
				if (num4 > 1f)
				{
					num4 = 1f;
				}
				float3 @float;
				float3 float2;
				float3 float3;
				SplineUtility.Evaluate<T>(spline, num4, ref @float, ref float2, ref float3);
				Vector3 normalized = float2.normalized;
				Vector3 normalized2 = float3.normalized;
				Vector3 vector2 = Vector3.Cross(normalized, normalized2);
				float num5 = 1f / (float)(profileSegments - 1);
				if (i > 0)
				{
					num3 += (@float - vector).magnitude;
				}
				for (int j = 0; j < profileSegments; j++)
				{
					float num6 = num5 * (float)j;
					float num7 = (num6 - 0.5f) * 2f;
					float num8 = Mathf.Cos(num7 * 3.1415927f * 0.5f) * height;
					float num9 = num7 * width;
					Vector3 item = @float + num9 * vector2 + num8 * normalized2;
					list.Add(item);
					list3.Add(new Vector2(num6 * this.uFactor, num3 * this.vFactor));
					list2.Add(normalized2);
				}
				vector = @float;
			}
			SplineFlatExtrude.<>c__DisplayClass53_0<T> CS$<>8__locals1;
			CS$<>8__locals1.triangles = new List<int>();
			for (int k = 0; k < num2 - 1; k++)
			{
				int num10 = k * profileSegments;
				for (int l = 0; l < profileSegments - 1; l++)
				{
					int num11 = num10 + l;
					SplineFlatExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num11,
						num11 + 1,
						num11 + profileSegments
					}, ref CS$<>8__locals1);
					SplineFlatExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num11 + 1,
						num11 + 1 + profileSegments,
						num11 + profileSegments
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

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0003ECC2 File Offset: 0x0003CEC2
		private void OnValidate()
		{
			this.Rebuild();
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0003ECCA File Offset: 0x0003CECA
		internal Mesh CreateMeshAsset()
		{
			return new Mesh
			{
				name = base.name
			};
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0003ECDD File Offset: 0x0003CEDD
		private void FlattenSpline()
		{
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0003ED55 File Offset: 0x0003CF55
		[CompilerGenerated]
		internal static void <Extrude>g__AddTriangles|53_0<T>(int[] indicies, ref SplineFlatExtrude.<>c__DisplayClass53_0<T> A_1) where T : ISpline
		{
			A_1.triangles.AddRange(indicies);
		}

		// Token: 0x04000CDF RID: 3295
		[SerializeField]
		[Tooltip("The Spline to extrude.")]
		private SplineContainer m_Container;

		// Token: 0x04000CE0 RID: 3296
		[SerializeField]
		[Tooltip("Enable to regenerate the extruded mesh when the target Spline is modified. Disable this option if the Spline will not be modified at runtime.")]
		private bool m_RebuildOnSplineChange;

		// Token: 0x04000CE1 RID: 3297
		[SerializeField]
		[Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
		private int m_RebuildFrequency = 30;

		// Token: 0x04000CE2 RID: 3298
		[SerializeField]
		[Tooltip("Automatically update any Mesh, Box, or Sphere collider components when the mesh is extruded.")]
		private bool m_UpdateColliders = true;

		// Token: 0x04000CE3 RID: 3299
		[SerializeField]
		[Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
		private float m_SegmentsPerUnit = 4f;

		// Token: 0x04000CE4 RID: 3300
		[SerializeField]
		[Tooltip("The radius of the extruded mesh.")]
		private float m_Width = 0.25f;

		// Token: 0x04000CE5 RID: 3301
		[SerializeField]
		private int m_ProfileSeg = 2;

		// Token: 0x04000CE6 RID: 3302
		[SerializeField]
		private float m_Height = 0.05f;

		// Token: 0x04000CE7 RID: 3303
		[SerializeField]
		[Tooltip("The section of the Spline to extrude.")]
		private Vector2 m_Range = new Vector2(0f, 0.999f);

		// Token: 0x04000CE8 RID: 3304
		[SerializeField]
		private float uFactor = 1f;

		// Token: 0x04000CE9 RID: 3305
		[SerializeField]
		private float vFactor = 1f;

		// Token: 0x04000CEA RID: 3306
		private Mesh m_Mesh;

		// Token: 0x04000CEB RID: 3307
		private bool m_RebuildRequested;

		// Token: 0x04000CEC RID: 3308
		private float m_NextScheduledRebuild;
	}
}
