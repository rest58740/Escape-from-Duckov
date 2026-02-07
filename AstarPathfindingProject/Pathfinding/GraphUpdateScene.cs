using System;
using Pathfinding.Drawing;
using Pathfinding.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000062 RID: 98
	[AddComponentMenu("Pathfinding/GraphUpdateScene")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/graphupdatescene.html")]
	public class GraphUpdateScene : GraphModifier
	{
		// Token: 0x06000370 RID: 880 RVA: 0x0001056A File Offset: 0x0000E76A
		public void Start()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (!this.firstApplied && this.applyOnStart)
			{
				this.Apply();
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001058A File Offset: 0x0000E78A
		public override void OnPostScan()
		{
			if (this.applyOnScan)
			{
				this.Apply();
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0001059C File Offset: 0x0000E79C
		public virtual void InvertSettings()
		{
			this.setWalkability = !this.setWalkability;
			this.penaltyDelta = -this.penaltyDelta;
			if (this.setTagInvert == 0U)
			{
				this.setTagInvert = this.setTag;
				this.setTag = 0U;
				return;
			}
			this.setTag = this.setTagInvert;
			this.setTagInvert = 0U;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00010603 File Offset: 0x0000E803
		public void RecalcConvex()
		{
			this.convexPoints = (this.convex ? Polygon.ConvexHullXZ(this.points) : null);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00010624 File Offset: 0x0000E824
		public Bounds GetBounds()
		{
			if (this.points == null || this.points.Length == 0)
			{
				Collider component = base.GetComponent<Collider>();
				Collider2D component2 = base.GetComponent<Collider2D>();
				Renderer component3 = base.GetComponent<Renderer>();
				Bounds bounds;
				if (component != null)
				{
					bounds = component.bounds;
				}
				else if (component2 != null)
				{
					bounds = component2.bounds;
					bounds.size = new Vector3(bounds.size.x, bounds.size.y, Mathf.Max(bounds.size.z, 1f));
				}
				else
				{
					if (!(component3 != null))
					{
						return new Bounds(Vector3.zero, Vector3.zero);
					}
					bounds = component3.bounds;
				}
				if (this.legacyMode && bounds.size.y < this.minBoundsHeight)
				{
					bounds.size = new Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
				}
				return bounds;
			}
			if (this.convexPoints == null)
			{
				this.RecalcConvex();
			}
			return GraphUpdateShape.GetBounds(this.convex ? this.convexPoints : this.points, (this.legacyMode && this.legacyUseWorldSpace) ? Matrix4x4.identity : base.transform.localToWorldMatrix, this.minBoundsHeight);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00010774 File Offset: 0x0000E974
		public virtual GraphUpdateObject GetGraphUpdate()
		{
			GraphUpdateObject graphUpdateObject;
			if (this.points == null || this.points.Length == 0)
			{
				PolygonCollider2D component = base.GetComponent<PolygonCollider2D>();
				if (component != null)
				{
					Vector2[] array = component.points;
					Vector3[] array2 = new Vector3[array.Length];
					for (int i = 0; i < array2.Length; i++)
					{
						Vector2 vector = array[i] + component.offset;
						array2[i] = new Vector3(vector.x, 0f, vector.y);
					}
					Matrix4x4 matrix = base.transform.localToWorldMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90f, 0f, 0f), Vector3.one);
					GraphUpdateShape shape = new GraphUpdateShape(array2, this.convex, matrix, this.minBoundsHeight);
					graphUpdateObject = new GraphUpdateObject(this.GetBounds());
					graphUpdateObject.shape = shape;
				}
				else
				{
					Bounds bounds = this.GetBounds();
					if (bounds.center == Vector3.zero && bounds.size == Vector3.zero)
					{
						Debug.LogError("Cannot apply GraphUpdateScene, no points defined and no renderer or collider attached", this);
						return null;
					}
					if (bounds.size == Vector3.zero)
					{
						Debug.LogWarning("Collider bounding box was empty. Are you trying to apply the GraphUpdateScene before the collider has been enabled or initialized?", this);
					}
					graphUpdateObject = new GraphUpdateObject(bounds);
				}
			}
			else
			{
				GraphUpdateShape graphUpdateShape;
				if (this.legacyMode && !this.legacyUseWorldSpace)
				{
					Vector3[] array3 = new Vector3[this.points.Length];
					for (int j = 0; j < this.points.Length; j++)
					{
						array3[j] = base.transform.TransformPoint(this.points[j]);
					}
					graphUpdateShape = new GraphUpdateShape(array3, this.convex, Matrix4x4.identity, this.minBoundsHeight);
				}
				else
				{
					graphUpdateShape = new GraphUpdateShape(this.points, this.convex, (this.legacyMode && this.legacyUseWorldSpace) ? Matrix4x4.identity : base.transform.localToWorldMatrix, this.minBoundsHeight);
				}
				graphUpdateObject = new GraphUpdateObject(graphUpdateShape.GetBounds());
				graphUpdateObject.shape = graphUpdateShape;
			}
			this.firstApplied = true;
			graphUpdateObject.modifyWalkability = this.modifyWalkability;
			graphUpdateObject.setWalkability = this.setWalkability;
			graphUpdateObject.addPenalty = this.penaltyDelta;
			graphUpdateObject.updatePhysics = this.updatePhysics;
			graphUpdateObject.updateErosion = this.updateErosion;
			graphUpdateObject.resetPenaltyOnPhysics = this.resetPenaltyOnPhysics;
			graphUpdateObject.modifyTag = this.modifyTag;
			graphUpdateObject.setTag = this.setTag;
			return graphUpdateObject;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000109F4 File Offset: 0x0000EBF4
		public void Apply()
		{
			if (AstarPath.active == null)
			{
				Debug.LogError("There is no AstarPath object in the scene", this);
				return;
			}
			GraphUpdateObject graphUpdate = this.GetGraphUpdate();
			if (graphUpdate != null)
			{
				AstarPath.active.UpdateGraphs(graphUpdate);
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00010A30 File Offset: 0x0000EC30
		public override void DrawGizmos()
		{
			bool flag = GizmoContext.InActiveSelection(this);
			Color color = flag ? GraphUpdateScene.GizmoColorSelected : GraphUpdateScene.GizmoColorUnselected;
			if (flag)
			{
				Color color2 = Color.Lerp(color, new Color(1f, 1f, 1f, 0.2f), 0.9f);
				Bounds bounds = this.GetBounds();
				Draw.SolidBox(bounds.center, bounds.size, color2);
				Draw.WireBox(bounds.center, bounds.size, color2);
			}
			if (this.points == null)
			{
				return;
			}
			if (this.convex)
			{
				color.a *= 0.5f;
			}
			Matrix4x4 matrix = (this.legacyMode && this.legacyUseWorldSpace) ? Matrix4x4.identity : base.transform.localToWorldMatrix;
			if (this.convex)
			{
				color.r -= 0.1f;
				color.g -= 0.2f;
				color.b -= 0.1f;
			}
			using (Draw.WithMatrix(matrix))
			{
				if (flag || !this.convex)
				{
					Color color3 = color;
					color3.a *= 0.7f;
					Draw.Polyline(this.points, true, this.convex ? color3 : color);
				}
				if (this.convex)
				{
					if (this.convexPoints == null)
					{
						this.RecalcConvex();
					}
					Draw.Polyline(this.convexPoints, true, flag ? GraphUpdateScene.GizmoColorSelected : GraphUpdateScene.GizmoColorUnselected);
				}
				Vector3[] array = this.convex ? this.convexPoints : this.points;
				if (flag && array != null && array.Length != 0)
				{
					float num = array[0].y;
					float num2 = array[0].y;
					for (int i = 0; i < array.Length; i++)
					{
						num = Mathf.Min(num, array[i].y);
						num2 = Mathf.Max(num2, array[i].y);
					}
					float num3 = Mathf.Max(this.minBoundsHeight - (num2 - num), 0f) * 0.5f;
					num -= num3;
					num2 += num3;
					using (Draw.WithColor(new Color(1f, 1f, 1f, 0.2f)))
					{
						for (int j = 0; j < array.Length; j++)
						{
							int num4 = (j + 1) % array.Length;
							Vector3 a = array[j] + Vector3.up * (num - array[j].y);
							Vector3 vector = array[j] + Vector3.up * (num2 - array[j].y);
							Vector3 b = array[num4] + Vector3.up * (num - array[num4].y);
							Vector3 b2 = array[num4] + Vector3.up * (num2 - array[num4].y);
							Draw.Line(a, vector);
							Draw.Line(a, b);
							Draw.Line(vector, b2);
						}
					}
				}
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00010DBC File Offset: 0x0000EFBC
		public void DisableLegacyMode()
		{
			if (this.legacyMode)
			{
				this.legacyMode = false;
				if (this.legacyUseWorldSpace)
				{
					this.legacyUseWorldSpace = false;
					for (int i = 0; i < this.points.Length; i++)
					{
						this.points[i] = base.transform.InverseTransformPoint(this.points[i]);
					}
					this.RecalcConvex();
				}
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00010E24 File Offset: 0x0000F024
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num))
			{
				if (num == 0 && this.points != null && this.points.Length != 0)
				{
					this.legacyMode = true;
				}
				if (this.setTagCompatibility != -1)
				{
					this.setTag = (uint)this.setTagCompatibility;
					this.setTagCompatibility = -1;
				}
			}
		}

		// Token: 0x04000207 RID: 519
		public Vector3[] points;

		// Token: 0x04000208 RID: 520
		private Vector3[] convexPoints;

		// Token: 0x04000209 RID: 521
		public bool convex = true;

		// Token: 0x0400020A RID: 522
		public float minBoundsHeight = 1f;

		// Token: 0x0400020B RID: 523
		public int penaltyDelta;

		// Token: 0x0400020C RID: 524
		public bool modifyWalkability;

		// Token: 0x0400020D RID: 525
		public bool setWalkability;

		// Token: 0x0400020E RID: 526
		public bool applyOnStart = true;

		// Token: 0x0400020F RID: 527
		public bool applyOnScan = true;

		// Token: 0x04000210 RID: 528
		public bool updatePhysics;

		// Token: 0x04000211 RID: 529
		public bool resetPenaltyOnPhysics = true;

		// Token: 0x04000212 RID: 530
		public bool updateErosion = true;

		// Token: 0x04000213 RID: 531
		public bool modifyTag;

		// Token: 0x04000214 RID: 532
		public PathfindingTag setTag;

		// Token: 0x04000215 RID: 533
		[HideInInspector]
		public bool legacyMode;

		// Token: 0x04000216 RID: 534
		private PathfindingTag setTagInvert;

		// Token: 0x04000217 RID: 535
		private bool firstApplied;

		// Token: 0x04000218 RID: 536
		[SerializeField]
		[FormerlySerializedAs("useWorldSpace")]
		private bool legacyUseWorldSpace;

		// Token: 0x04000219 RID: 537
		[SerializeField]
		[FormerlySerializedAs("setTag")]
		private int setTagCompatibility = -1;

		// Token: 0x0400021A RID: 538
		private static readonly Color GizmoColorSelected = new Color(0.8901961f, 0.23921569f, 0.08627451f, 1f);

		// Token: 0x0400021B RID: 539
		private static readonly Color GizmoColorUnselected = new Color(0.8901961f, 0.23921569f, 0.08627451f, 0.9f);
	}
}
