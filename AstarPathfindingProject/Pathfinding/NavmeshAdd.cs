using System;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000120 RID: 288
	[HelpURL("https://arongranberg.com/astar/documentation/stable/navmeshadd.html")]
	public class NavmeshAdd : NavmeshClipper
	{
		// Token: 0x060008FC RID: 2300 RVA: 0x000305AC File Offset: 0x0002E7AC
		public override bool RequiresUpdate(GridLookup<NavmeshClipper>.Root previousState)
		{
			return (this.tr.position - previousState.previousPosition).sqrMagnitude > this.updateDistance * this.updateDistance || (this.useRotationAndScale && Quaternion.Angle(previousState.previousRotation, this.tr.rotation) > this.updateRotationDistance);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00030610 File Offset: 0x0002E810
		public override void ForceUpdate()
		{
			if (AstarPath.active != null)
			{
				AstarPath.active.navmeshUpdates.ForceUpdateAround(this);
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0003062F File Offset: 0x0002E82F
		protected override void Awake()
		{
			base.Awake();
			this.tr = base.transform;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00030643 File Offset: 0x0002E843
		public override void NotifyUpdated(GridLookup<NavmeshClipper>.Root previousState)
		{
			previousState.previousPosition = this.tr.position;
			if (this.useRotationAndScale)
			{
				previousState.previousRotation = this.tr.rotation;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0003066F File Offset: 0x0002E86F
		public Vector3 Center
		{
			get
			{
				return this.tr.position + (this.useRotationAndScale ? this.tr.TransformPoint(this.center) : this.center);
			}
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x000306A4 File Offset: 0x0002E8A4
		[ContextMenu("Rebuild Mesh")]
		public void RebuildMesh()
		{
			if (this.type != NavmeshAdd.MeshType.CustomMesh)
			{
				if (this.verts == null || this.verts.Length != 4 || this.tris == null || this.tris.Length != 6)
				{
					this.verts = new Vector3[4];
					this.tris = new int[6];
				}
				this.tris[0] = 0;
				this.tris[1] = 1;
				this.tris[2] = 2;
				this.tris[3] = 0;
				this.tris[4] = 2;
				this.tris[5] = 3;
				this.verts[0] = new Vector3(-this.rectangleSize.x * 0.5f, 0f, -this.rectangleSize.y * 0.5f);
				this.verts[1] = new Vector3(this.rectangleSize.x * 0.5f, 0f, -this.rectangleSize.y * 0.5f);
				this.verts[2] = new Vector3(this.rectangleSize.x * 0.5f, 0f, this.rectangleSize.y * 0.5f);
				this.verts[3] = new Vector3(-this.rectangleSize.x * 0.5f, 0f, this.rectangleSize.y * 0.5f);
				return;
			}
			if (this.mesh == null)
			{
				this.verts = null;
				this.tris = null;
				return;
			}
			this.verts = this.mesh.vertices;
			this.tris = this.mesh.triangles;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00030854 File Offset: 0x0002EA54
		public override Rect GetBounds(GraphTransform inverseTransform, float radiusMargin)
		{
			if (this.verts == null)
			{
				this.RebuildMesh();
			}
			Int3[] array = ArrayPool<Int3>.Claim((this.verts != null) ? this.verts.Length : 0);
			int[] array2;
			int num;
			this.GetMesh(ref array, out array2, out num, inverseTransform);
			Rect result = default(Rect);
			for (int i = 0; i < array2.Length; i++)
			{
				Vector3 vector = (Vector3)array[array2[i]];
				if (i == 0)
				{
					result = new Rect(vector.x, vector.z, 0f, 0f);
				}
				else
				{
					result.xMax = Math.Max(result.xMax, vector.x);
					result.yMax = Math.Max(result.yMax, vector.z);
					result.xMin = Math.Min(result.xMin, vector.x);
					result.yMin = Math.Min(result.yMin, vector.z);
				}
			}
			ArrayPool<Int3>.Release(ref array, false);
			return result;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0003095C File Offset: 0x0002EB5C
		public void GetMesh(ref Int3[] vbuffer, out int[] tbuffer, out int vertexCount, GraphTransform inverseTransform = null)
		{
			if (this.verts == null)
			{
				this.RebuildMesh();
			}
			if (this.verts == null)
			{
				tbuffer = ArrayPool<int>.Claim(0);
				vertexCount = 0;
				return;
			}
			if (vbuffer == null || vbuffer.Length < this.verts.Length)
			{
				if (vbuffer != null)
				{
					ArrayPool<Int3>.Release(ref vbuffer, false);
				}
				vbuffer = ArrayPool<Int3>.Claim(this.verts.Length);
			}
			tbuffer = this.tris;
			vertexCount = this.verts.Length;
			if (this.useRotationAndScale)
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(this.tr.position + this.center, this.tr.rotation, this.tr.localScale * this.meshScale);
				for (int i = 0; i < this.verts.Length; i++)
				{
					Vector3 vector = matrix4x.MultiplyPoint3x4(this.verts[i]);
					if (inverseTransform != null)
					{
						vector = inverseTransform.InverseTransform(vector);
					}
					vbuffer[i] = (Int3)vector;
				}
				return;
			}
			Vector3 a = this.tr.position + this.center;
			for (int j = 0; j < this.verts.Length; j++)
			{
				Vector3 vector2 = a + this.verts[j] * this.meshScale;
				if (inverseTransform != null)
				{
					vector2 = inverseTransform.InverseTransform(vector2);
				}
				vbuffer[j] = (Int3)vector2;
			}
		}

		// Token: 0x0400060D RID: 1549
		public NavmeshAdd.MeshType type;

		// Token: 0x0400060E RID: 1550
		public Mesh mesh;

		// Token: 0x0400060F RID: 1551
		private Vector3[] verts;

		// Token: 0x04000610 RID: 1552
		private int[] tris;

		// Token: 0x04000611 RID: 1553
		public Vector2 rectangleSize = new Vector2(1f, 1f);

		// Token: 0x04000612 RID: 1554
		public float meshScale = 1f;

		// Token: 0x04000613 RID: 1555
		public Vector3 center;

		// Token: 0x04000614 RID: 1556
		[FormerlySerializedAs("useRotation")]
		public bool useRotationAndScale;

		// Token: 0x04000615 RID: 1557
		[Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
		public float updateDistance = 0.4f;

		// Token: 0x04000616 RID: 1558
		[Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
		public float updateRotationDistance = 10f;

		// Token: 0x04000617 RID: 1559
		protected Transform tr;

		// Token: 0x04000618 RID: 1560
		public static readonly Color GizmoColor = new Color(0.6039216f, 0.13725491f, 0.9372549f);

		// Token: 0x02000121 RID: 289
		public enum MeshType
		{
			// Token: 0x0400061A RID: 1562
			Rectangle,
			// Token: 0x0400061B RID: 1563
			CustomMesh
		}
	}
}
