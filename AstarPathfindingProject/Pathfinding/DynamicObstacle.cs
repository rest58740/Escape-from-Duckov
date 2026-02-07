using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000140 RID: 320
	[AddComponentMenu("Pathfinding/Dynamic Obstacle")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/dynamicobstacle.html")]
	public class DynamicObstacle : GraphModifier, DynamicGridObstacle
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x00034C84 File Offset: 0x00032E84
		private Bounds bounds
		{
			get
			{
				if (this.coll != null)
				{
					return this.coll.bounds;
				}
				if (this.coll2D != null)
				{
					Bounds bounds = this.coll2D.bounds;
					bounds.extents += new Vector3(0f, 0f, 10000f);
					return bounds;
				}
				return default(Bounds);
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x00034CF6 File Offset: 0x00032EF6
		private bool colliderEnabled
		{
			get
			{
				if (!(this.coll != null))
				{
					return this.coll2D.enabled;
				}
				return this.coll.enabled;
			}
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00034D20 File Offset: 0x00032F20
		protected override void Awake()
		{
			base.Awake();
			this.coll = base.GetComponent<Collider>();
			this.coll2D = base.GetComponent<Collider2D>();
			this.tr = base.transform;
			if (this.coll == null && this.coll2D == null && Application.isPlaying)
			{
				throw new Exception("A collider or 2D collider must be attached to the GameObject(" + base.gameObject.name + ") for the DynamicObstacle to work");
			}
			this.prevBounds = this.bounds;
			this.prevRotation = this.tr.rotation;
			this.prevEnabled = false;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00034DBE File Offset: 0x00032FBE
		public override void OnPostScan()
		{
			if (this.coll == null)
			{
				this.Awake();
			}
			if (this.coll != null)
			{
				this.prevEnabled = this.colliderEnabled;
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00034DF0 File Offset: 0x00032FF0
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.coll == null && this.coll2D == null)
			{
				Debug.LogError("No collider attached to this GameObject. The DynamicObstacle component requires a collider.", this);
				base.enabled = false;
				return;
			}
			while (this.pendingGraphUpdates.Count > 0 && this.pendingGraphUpdates.Peek().stage != GraphUpdateStage.Pending)
			{
				this.pendingGraphUpdates.Dequeue();
			}
			if (AstarPath.active == null || AstarPath.active.isScanning || Time.realtimeSinceStartup - this.lastCheckTime < this.checkTime || !Application.isPlaying || this.pendingGraphUpdates.Count > 0)
			{
				return;
			}
			this.lastCheckTime = Time.realtimeSinceStartup;
			if (this.colliderEnabled)
			{
				Bounds bounds = this.bounds;
				Quaternion rotation = this.tr.rotation;
				Vector3 vector = this.prevBounds.min - bounds.min;
				Vector3 vector2 = this.prevBounds.max - bounds.max;
				float num = bounds.extents.magnitude * Quaternion.Angle(this.prevRotation, rotation) * 0.017453292f;
				if (vector.sqrMagnitude > this.updateError * this.updateError || vector2.sqrMagnitude > this.updateError * this.updateError || num > this.updateError || !this.prevEnabled)
				{
					this.DoUpdateGraphs();
					return;
				}
			}
			else if (this.prevEnabled)
			{
				this.DoUpdateGraphs();
			}
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00034F78 File Offset: 0x00033178
		protected override void OnDisable()
		{
			base.OnDisable();
			if (AstarPath.active != null && Application.isPlaying)
			{
				GraphUpdateObject graphUpdateObject = new GraphUpdateObject(this.prevBounds);
				this.pendingGraphUpdates.Enqueue(graphUpdateObject);
				AstarPath.active.UpdateGraphs(graphUpdateObject);
				this.prevEnabled = false;
			}
			this.pendingGraphUpdates.Clear();
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00034FD4 File Offset: 0x000331D4
		public void DoUpdateGraphs()
		{
			if (this.coll == null && this.coll2D == null)
			{
				return;
			}
			Physics.SyncTransforms();
			Physics2D.SyncTransforms();
			if (!this.colliderEnabled)
			{
				GraphUpdateObject graphUpdateObject = new GraphUpdateObject(this.prevBounds);
				this.pendingGraphUpdates.Enqueue(graphUpdateObject);
				AstarPath.active.UpdateGraphs(graphUpdateObject);
			}
			else
			{
				Bounds bounds = this.bounds;
				Bounds b = bounds;
				b.Encapsulate(this.prevBounds);
				if (DynamicObstacle.BoundsVolume(b) < DynamicObstacle.BoundsVolume(bounds) + DynamicObstacle.BoundsVolume(this.prevBounds))
				{
					GraphUpdateObject graphUpdateObject2 = new GraphUpdateObject(b);
					this.pendingGraphUpdates.Enqueue(graphUpdateObject2);
					AstarPath.active.UpdateGraphs(graphUpdateObject2);
				}
				else
				{
					GraphUpdateObject graphUpdateObject3 = new GraphUpdateObject(this.prevBounds);
					GraphUpdateObject graphUpdateObject4 = new GraphUpdateObject(bounds);
					this.pendingGraphUpdates.Enqueue(graphUpdateObject3);
					this.pendingGraphUpdates.Enqueue(graphUpdateObject4);
					AstarPath.active.UpdateGraphs(graphUpdateObject3);
					AstarPath.active.UpdateGraphs(graphUpdateObject4);
				}
				this.prevBounds = bounds;
			}
			this.prevEnabled = this.colliderEnabled;
			this.prevRotation = this.tr.rotation;
			this.lastCheckTime = Time.realtimeSinceStartup;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x000350FE File Offset: 0x000332FE
		private static float BoundsVolume(Bounds b)
		{
			return Math.Abs(b.size.x * b.size.y * b.size.z);
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0003512B File Offset: 0x0003332B
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x00035133 File Offset: 0x00033333
		float DynamicGridObstacle.updateError
		{
			get
			{
				return this.updateError;
			}
			set
			{
				this.updateError = value;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0003513C File Offset: 0x0003333C
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x00035144 File Offset: 0x00033344
		float DynamicGridObstacle.checkTime
		{
			get
			{
				return this.checkTime;
			}
			set
			{
				this.checkTime = value;
			}
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0002D2B4 File Offset: 0x0002B4B4
		bool DynamicGridObstacle.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0002D2BC File Offset: 0x0002B4BC
		void DynamicGridObstacle.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x0400069B RID: 1691
		private Collider coll;

		// Token: 0x0400069C RID: 1692
		private Collider2D coll2D;

		// Token: 0x0400069D RID: 1693
		private Transform tr;

		// Token: 0x0400069E RID: 1694
		public float updateError = 1f;

		// Token: 0x0400069F RID: 1695
		public float checkTime = 0.2f;

		// Token: 0x040006A0 RID: 1696
		private Bounds prevBounds;

		// Token: 0x040006A1 RID: 1697
		private Quaternion prevRotation;

		// Token: 0x040006A2 RID: 1698
		private bool prevEnabled;

		// Token: 0x040006A3 RID: 1699
		private float lastCheckTime = -9999f;

		// Token: 0x040006A4 RID: 1700
		private Queue<GraphUpdateObject> pendingGraphUpdates = new Queue<GraphUpdateObject>();
	}
}
