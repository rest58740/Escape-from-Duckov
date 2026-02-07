using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000123 RID: 291
	[AddComponentMenu("Pathfinding/Navmesh/Navmesh Cut")]
	[ExecuteAlways]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/navmeshcut.html")]
	public class NavmeshCut : NavmeshClipper
	{
		// Token: 0x06000912 RID: 2322 RVA: 0x00030CA9 File Offset: 0x0002EEA9
		protected override void Awake()
		{
			base.Awake();
			this.tr = base.transform;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00030CBD File Offset: 0x0002EEBD
		protected override void OnDisable()
		{
			if (this.meshContourVertices.IsCreated)
			{
				this.meshContourVertices.Dispose();
			}
			if (this.meshContours.IsCreated)
			{
				this.meshContours.Dispose();
			}
			this.lastMesh = null;
			base.OnDisable();
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00030610 File Offset: 0x0002E810
		public override void ForceUpdate()
		{
			if (AstarPath.active != null)
			{
				AstarPath.active.navmeshUpdates.ForceUpdateAround(this);
			}
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00030CFC File Offset: 0x0002EEFC
		public override bool RequiresUpdate(GridLookup<NavmeshClipper>.Root previousState)
		{
			return (this.tr.position - previousState.previousPosition).sqrMagnitude > this.updateDistance * this.updateDistance || (this.useRotationAndScale && Quaternion.Angle(previousState.previousRotation, this.tr.rotation) > this.updateRotationDistance);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void UsedForCut()
		{
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00030D60 File Offset: 0x0002EF60
		public override void NotifyUpdated(GridLookup<NavmeshClipper>.Root previousState)
		{
			previousState.previousPosition = this.tr.position;
			if (this.useRotationAndScale)
			{
				previousState.previousRotation = this.tr.rotation;
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00030D8C File Offset: 0x0002EF8C
		private void CalculateMeshContour()
		{
			if (this.mesh == null)
			{
				return;
			}
			NavmeshCut.edges.Clear();
			NavmeshCut.pointers.Clear();
			Vector3[] vertices = this.mesh.vertices;
			int[] triangles = this.mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (VectorMath.IsClockwiseXZ(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]))
				{
					int num = triangles[i];
					triangles[i] = triangles[i + 2];
					triangles[i + 2] = num;
				}
				NavmeshCut.edges[new Vector2Int(triangles[i], triangles[i + 1])] = i;
				NavmeshCut.edges[new Vector2Int(triangles[i + 1], triangles[i + 2])] = i;
				NavmeshCut.edges[new Vector2Int(triangles[i + 2], triangles[i])] = i;
			}
			for (int j = 0; j < triangles.Length; j += 3)
			{
				for (int k = 0; k < 3; k++)
				{
					if (!NavmeshCut.edges.ContainsKey(new Vector2Int(triangles[j + (k + 1) % 3], triangles[j + k % 3])))
					{
						NavmeshCut.pointers[triangles[j + k % 3]] = triangles[j + (k + 1) % 3];
					}
				}
			}
			NativeList<float3> nativeList = new NativeList<float3>(Allocator.Persistent);
			NativeList<NavmeshCut.ContourBurst> nativeList2 = new NativeList<NavmeshCut.ContourBurst>(Allocator.Persistent);
			for (int l = 0; l < vertices.Length; l++)
			{
				if (NavmeshCut.pointers.ContainsKey(l))
				{
					int length = nativeList.Length;
					int num2 = l;
					do
					{
						int num3 = NavmeshCut.pointers[num2];
						if (num3 == -1)
						{
							break;
						}
						NavmeshCut.pointers[num2] = -1;
						float3 @float = vertices[num2];
						nativeList.Add(@float);
						num2 = num3;
					}
					while (num2 != l);
					if (nativeList.Length != length)
					{
						NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
						contourBurst.startIndex = length;
						contourBurst.endIndex = nativeList.Length;
						contourBurst.ymin = 0f;
						contourBurst.ymax = 0f;
						nativeList2.Add(contourBurst);
					}
				}
			}
			if (this.meshContourVertices.IsCreated)
			{
				this.meshContourVertices.Dispose();
			}
			if (this.meshContours.IsCreated)
			{
				this.meshContours.Dispose();
			}
			this.meshContourVertices = nativeList;
			this.meshContours = nativeList2;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00031000 File Offset: 0x0002F200
		public override Rect GetBounds(GraphTransform inverseTransform, float radiusMargin)
		{
			List<NavmeshCut.Contour> list = ListPool<NavmeshCut.Contour>.Claim();
			this.GetContour(list, inverseTransform.inverseMatrix, radiusMargin);
			Rect result = default(Rect);
			for (int i = 0; i < list.Count; i++)
			{
				List<Vector2> contour = list[i].contour;
				for (int j = 0; j < contour.Count; j++)
				{
					Vector2 vector = contour[j];
					if (j == 0 && i == 0)
					{
						result = new Rect(vector.x, vector.y, 0f, 0f);
					}
					else
					{
						result.xMax = Math.Max(result.xMax, vector.x);
						result.yMax = Math.Max(result.yMax, vector.y);
						result.xMin = Math.Min(result.xMin, vector.x);
						result.yMin = Math.Min(result.yMin, vector.y);
					}
				}
				ListPool<Vector2>.Release(ref contour);
			}
			ListPool<NavmeshCut.Contour>.Release(ref list);
			return result;
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00031114 File Offset: 0x0002F314
		private Matrix4x4 contourTransformationMatrix
		{
			get
			{
				if (this.useRotationAndScale)
				{
					return this.tr.localToWorldMatrix * Matrix4x4.Translate(this.center);
				}
				return Matrix4x4.Translate(this.tr.position + this.center);
			}
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00031160 File Offset: 0x0002F360
		public unsafe void GetContour(List<NavmeshCut.Contour> buffer, Matrix4x4 matrix, float radiusMargin)
		{
			UnsafeList<float2> unsafeList = new UnsafeList<float2>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<NavmeshCut.ContourBurst> unsafeList2 = new UnsafeList<NavmeshCut.ContourBurst>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			this.GetContourBurst(&unsafeList, &unsafeList2, matrix, radiusMargin);
			for (int i = 0; i < unsafeList2.Length; i++)
			{
				List<Vector2> list = ListPool<Vector2>.Claim();
				NavmeshCut.ContourBurst contourBurst = unsafeList2[i];
				for (int j = contourBurst.startIndex; j < contourBurst.endIndex; j++)
				{
					list.Add(unsafeList[j]);
				}
				buffer.Add(new NavmeshCut.Contour
				{
					ymin = contourBurst.ymin,
					ymax = contourBurst.ymax,
					contour = list
				});
			}
			unsafeList.Dispose();
			unsafeList2.Dispose();
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00031230 File Offset: 0x0002F430
		public unsafe void GetContourBurst(UnsafeList<float2>* outputVertices, UnsafeList<NavmeshCut.ContourBurst>* outputContours, Matrix4x4 matrix, float radiusMargin)
		{
			if (this.radiusExpansionMode == NavmeshCut.RadiusExpansionMode.DontExpand)
			{
				radiusMargin = 0f;
			}
			if (this.type == NavmeshCut.MeshType.CustomMesh && (this.mesh != this.lastMesh || !this.meshContours.IsCreated || !this.meshContourVertices.IsCreated))
			{
				this.CalculateMeshContour();
				this.lastMesh = this.mesh;
			}
			NavmeshCutJobs.JobCalculateContour jobCalculateContour = new NavmeshCutJobs.JobCalculateContour
			{
				outputVertices = outputVertices,
				outputContours = outputContours,
				matrix = matrix,
				localToWorldMatrix = this.contourTransformationMatrix,
				radiusMargin = radiusMargin,
				circleResolution = this.circleResolution,
				circleRadius = this.circleRadius,
				rectangleSize = this.rectangleSize,
				height = this.height,
				meshType = this.type,
				meshContours = this.meshContours.GetUnsafeList(),
				meshContourVertices = this.meshContourVertices.GetUnsafeList(),
				meshScale = this.meshScale
			};
			NavmeshCutJobs.CalculateContour(ref jobCalculateContour);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00031354 File Offset: 0x0002F554
		private static NavmeshBase ClosestGraph(Vector3 position)
		{
			AstarPath active = AstarPath.active;
			NavGraph[] array = (active != null) ? active.data.graphs : null;
			if (array == null)
			{
				return null;
			}
			NavmeshBase result = null;
			float num = float.PositiveInfinity;
			for (int i = 0; i < array.Length; i++)
			{
				NavmeshBase navmeshBase = array[i] as NavmeshBase;
				if (navmeshBase != null)
				{
					float num2 = navmeshBase.IsInsideBounds(position) ? -1f : navmeshBase.bounds.SqrDistance(position);
					if (num2 < num)
					{
						num = num2;
						result = navmeshBase;
					}
				}
			}
			return result;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x000313D4 File Offset: 0x0002F5D4
		public unsafe override void DrawGizmos()
		{
			if (this.tr == null)
			{
				this.tr = base.transform;
			}
			bool flag = GizmoContext.InActiveSelection(this.tr);
			NavmeshBase navmeshBase = NavmeshCut.ClosestGraph(this.tr.position);
			GraphTransform graphTransform = (navmeshBase != null) ? navmeshBase.transform : GraphTransform.identityTransform;
			float radiusMargin = (navmeshBase != null) ? navmeshBase.NavmeshCuttingCharacterRadius : 0f;
			UnsafeList<float2> unsafeList = new UnsafeList<float2>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<NavmeshCut.ContourBurst> unsafeList2 = new UnsafeList<NavmeshCut.ContourBurst>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			this.GetContourBurst(&unsafeList, &unsafeList2, graphTransform.inverseMatrix, radiusMargin);
			Color color = Color.Lerp(NavmeshCut.GizmoColor, Color.white, 0.5f);
			color.a *= 0.5f;
			using (Draw.WithColor(color))
			{
				for (int i = 0; i < unsafeList2.Length; i++)
				{
					NavmeshCut.ContourBurst contourBurst = unsafeList2[i];
					float y = (contourBurst.ymin + contourBurst.ymax) * 0.5f;
					int num = contourBurst.endIndex - contourBurst.startIndex;
					for (int j = 0; j < num; j++)
					{
						float2 @float = unsafeList[contourBurst.startIndex + j];
						float2 float2 = unsafeList[contourBurst.startIndex + (j + 1) % num];
						Vector3 vector = new Vector3(@float.x, y, @float.y);
						Vector3 vector2 = new Vector3(float2.x, y, float2.y);
						Draw.Line(graphTransform.Transform(vector), graphTransform.Transform(vector2), NavmeshCut.GizmoColor);
						if (flag)
						{
							Vector3 point = vector;
							Vector3 point2 = vector2;
							Vector3 point3 = vector;
							Vector3 point4 = vector2;
							point.y = (point2.y = contourBurst.ymin);
							point3.y = (point4.y = contourBurst.ymax);
							Draw.Line(graphTransform.Transform(point), graphTransform.Transform(point2));
							Draw.Line(graphTransform.Transform(point3), graphTransform.Transform(point4));
							Draw.Line(graphTransform.Transform(point), graphTransform.Transform(point3));
						}
					}
				}
			}
			if (flag)
			{
				switch (this.type)
				{
				case NavmeshCut.MeshType.CustomMesh:
					goto IL_45C;
				case NavmeshCut.MeshType.Box:
					using (Draw.WithMatrix(this.contourTransformationMatrix * Matrix4x4.Scale(new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y))))
					{
						Draw.WireBox(Vector3.zero, Vector3.one, NavmeshCut.GizmoColor2);
						goto IL_4B1;
					}
					break;
				case NavmeshCut.MeshType.Sphere:
				{
					float d = this.useRotationAndScale ? math.cmax(this.tr.lossyScale) : 1f;
					using (Draw.WithMatrix(Matrix4x4.TRS(this.tr.position, this.useRotationAndScale ? this.tr.rotation : Quaternion.identity, Vector3.one * d) * Matrix4x4.Translate(this.center)))
					{
						Draw.WireSphere(Vector3.zero, this.circleRadius, NavmeshCut.GizmoColor2);
						goto IL_4B1;
					}
					goto IL_45C;
				}
				case NavmeshCut.MeshType.Capsule:
					break;
				default:
					goto IL_4B1;
				}
				Matrix4x4 contourTransformationMatrix = this.contourTransformationMatrix;
				float num2 = Mathf.Max(this.height, this.circleRadius * 2f);
				float x = math.length(contourTransformationMatrix.GetColumn(0));
				float y2 = math.length(contourTransformationMatrix.GetColumn(2));
				float num3 = this.circleRadius * math.max(x, y2);
				Vector3 normalized = contourTransformationMatrix.GetColumn(1).normalized;
				Vector3 v = this.contourTransformationMatrix.MultiplyPoint3x4(new Vector3(0f, num2 * 0.5f, 0f)) - normalized * num3;
				Vector3 v2 = this.contourTransformationMatrix.MultiplyPoint3x4(-new Vector3(0f, num2 * 0.5f, 0f)) + normalized * num3;
				Draw.WireCapsule(v, v2, num3, NavmeshCut.GizmoColor2);
				goto IL_4B1;
				IL_45C:
				if (this.mesh != null)
				{
					using (Draw.WithMatrix(this.contourTransformationMatrix * Matrix4x4.Scale(Vector3.one * this.meshScale)))
					{
						Draw.WireMesh(this.mesh, NavmeshCut.GizmoColor2);
					}
				}
			}
			IL_4B1:
			unsafeList.Dispose();
			unsafeList2.Dispose();
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00031904 File Offset: 0x0002FB04
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num) && num < 2)
			{
				this.radiusExpansionMode = NavmeshCut.RadiusExpansionMode.DontExpand;
			}
		}

		// Token: 0x04000621 RID: 1569
		[Tooltip("Shape of the cut")]
		public NavmeshCut.MeshType type = NavmeshCut.MeshType.Box;

		// Token: 0x04000622 RID: 1570
		[Tooltip("The contour(s) of the mesh will be extracted. This mesh should only be a 2D surface, not a volume (see documentation).")]
		public Mesh mesh;

		// Token: 0x04000623 RID: 1571
		public Vector2 rectangleSize = new Vector2(1f, 1f);

		// Token: 0x04000624 RID: 1572
		public float circleRadius = 1f;

		// Token: 0x04000625 RID: 1573
		public int circleResolution = 6;

		// Token: 0x04000626 RID: 1574
		public float height = 1f;

		// Token: 0x04000627 RID: 1575
		[Tooltip("Scale of the custom mesh")]
		public float meshScale = 1f;

		// Token: 0x04000628 RID: 1576
		public Vector3 center;

		// Token: 0x04000629 RID: 1577
		[Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
		public float updateDistance = 0.4f;

		// Token: 0x0400062A RID: 1578
		[Tooltip("Only makes a split in the navmesh, but does not remove the geometry to make a hole")]
		public bool isDual;

		// Token: 0x0400062B RID: 1579
		public NavmeshCut.RadiusExpansionMode radiusExpansionMode = NavmeshCut.RadiusExpansionMode.ExpandByAgentRadius;

		// Token: 0x0400062C RID: 1580
		public bool cutsAddedGeom = true;

		// Token: 0x0400062D RID: 1581
		[Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
		public float updateRotationDistance = 10f;

		// Token: 0x0400062E RID: 1582
		[Tooltip("Includes rotation in calculations. This is slower since a lot more matrix multiplications are needed but gives more flexibility.")]
		[FormerlySerializedAs("useRotation")]
		public bool useRotationAndScale;

		// Token: 0x0400062F RID: 1583
		private NativeList<float3> meshContourVertices;

		// Token: 0x04000630 RID: 1584
		private NativeList<NavmeshCut.ContourBurst> meshContours;

		// Token: 0x04000631 RID: 1585
		protected Transform tr;

		// Token: 0x04000632 RID: 1586
		private Mesh lastMesh;

		// Token: 0x04000633 RID: 1587
		private static readonly Dictionary<Vector2Int, int> edges = new Dictionary<Vector2Int, int>();

		// Token: 0x04000634 RID: 1588
		private static readonly Dictionary<int, int> pointers = new Dictionary<int, int>();

		// Token: 0x04000635 RID: 1589
		public static readonly Color GizmoColor = new Color(0.14509805f, 0.72156864f, 0.9372549f);

		// Token: 0x04000636 RID: 1590
		public static readonly Color GizmoColor2 = new Color(0.6627451f, 0.36078432f, 0.9490196f);

		// Token: 0x02000124 RID: 292
		public enum MeshType
		{
			// Token: 0x04000638 RID: 1592
			Rectangle,
			// Token: 0x04000639 RID: 1593
			Circle,
			// Token: 0x0400063A RID: 1594
			CustomMesh,
			// Token: 0x0400063B RID: 1595
			Box,
			// Token: 0x0400063C RID: 1596
			Sphere,
			// Token: 0x0400063D RID: 1597
			Capsule
		}

		// Token: 0x02000125 RID: 293
		public enum RadiusExpansionMode
		{
			// Token: 0x0400063F RID: 1599
			DontExpand,
			// Token: 0x04000640 RID: 1600
			ExpandByAgentRadius
		}

		// Token: 0x02000126 RID: 294
		public struct Contour
		{
			// Token: 0x04000641 RID: 1601
			public float ymin;

			// Token: 0x04000642 RID: 1602
			public float ymax;

			// Token: 0x04000643 RID: 1603
			public List<Vector2> contour;
		}

		// Token: 0x02000127 RID: 295
		public struct ContourBurst
		{
			// Token: 0x04000644 RID: 1604
			public int startIndex;

			// Token: 0x04000645 RID: 1605
			public int endIndex;

			// Token: 0x04000646 RID: 1606
			public float ymin;

			// Token: 0x04000647 RID: 1607
			public float ymax;
		}
	}
}
