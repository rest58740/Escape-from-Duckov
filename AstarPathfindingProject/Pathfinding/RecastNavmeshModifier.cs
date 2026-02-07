using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000107 RID: 263
	[AddComponentMenu("Pathfinding/Navmesh/RecastNavmeshModifier")]
	[DisallowMultipleComponent]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/recastnavmeshmodifier.html")]
	public class RecastNavmeshModifier : VersionedMonoBehaviour, RecastMeshObj
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0002CE3C File Offset: 0x0002B03C
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x0002CE7B File Offset: 0x0002B07B
		[Obsolete("Use mode and surfaceID instead")]
		public int area
		{
			get
			{
				switch (this.mode)
				{
				case RecastNavmeshModifier.Mode.UnwalkableSurface:
					return -1;
				case RecastNavmeshModifier.Mode.WalkableSurfaceWithSeam:
					return this.surfaceID;
				case RecastNavmeshModifier.Mode.WalkableSurfaceWithTag:
					return this.surfaceID;
				}
				return 0;
			}
			set
			{
				if (value <= -1)
				{
					this.mode = RecastNavmeshModifier.Mode.UnwalkableSurface;
				}
				if (value == 0)
				{
					this.mode = RecastNavmeshModifier.Mode.WalkableSurface;
				}
				if (value > 0)
				{
					this.mode = RecastNavmeshModifier.Mode.WalkableSurfaceWithSeam;
					this.surfaceID = value;
				}
			}
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0002CEA4 File Offset: 0x0002B0A4
		private void OnEnable()
		{
			this.surfaceID = Mathf.Clamp(this.surfaceID, 0, 33554432);
			if (!this.treeKey.isValid)
			{
				this.treeKey = RecastNavmeshModifier.tree.Add(this.CalculateBounds(), this);
				if (this.dynamic)
				{
					BatchedEvents.Add<RecastNavmeshModifier>(this, BatchedEvents.Event.Custom, new Action<RecastNavmeshModifier[], int>(RecastNavmeshModifier.OnUpdate), 0);
				}
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0002CF08 File Offset: 0x0002B108
		private void OnDisable()
		{
			BatchedEvents.Remove<RecastNavmeshModifier>(this);
			Bounds bounds = RecastNavmeshModifier.tree.Remove(this.treeKey);
			this.treeKey = default(AABBTree<RecastNavmeshModifier>.Key);
			if (!this.dynamic)
			{
				Bounds bounds2 = this.CalculateBounds();
				bounds.Expand(0.001f);
				bounds2.Encapsulate(bounds);
				if ((bounds2.center - bounds.center).sqrMagnitude > 0.0001f || (bounds2.extents - bounds.extents).sqrMagnitude > 0.0001f)
				{
					string str = "The RecastNavmeshModifier has been moved or resized since it was enabled. You should set dynamic to true for moving objects, or disable the component while moving it. The bounds changed from ";
					Bounds bounds3 = bounds;
					string str2 = bounds3.ToString();
					string str3 = " to ";
					bounds3 = bounds2;
					Debug.LogError(str + str2 + str3 + bounds3.ToString(), this);
				}
			}
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0002CFD8 File Offset: 0x0002B1D8
		private static void OnUpdate(RecastNavmeshModifier[] components, int _)
		{
			foreach (RecastNavmeshModifier recastNavmeshModifier in components)
			{
				if (recastNavmeshModifier != null && recastNavmeshModifier.transform.hasChanged)
				{
					Bounds bounds = recastNavmeshModifier.CalculateBounds();
					if (RecastNavmeshModifier.tree.GetBounds(recastNavmeshModifier.treeKey) != bounds)
					{
						RecastNavmeshModifier.tree.Move(recastNavmeshModifier.treeKey, bounds);
					}
					recastNavmeshModifier.transform.hasChanged = false;
				}
			}
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0002D04C File Offset: 0x0002B24C
		public static void GetAllInBounds(List<RecastNavmeshModifier> buffer, Bounds bounds)
		{
			BatchedEvents.ProcessEvent<RecastNavmeshModifier>(BatchedEvents.Event.Custom);
			if (!Application.isPlaying)
			{
				RecastNavmeshModifier[] array = UnityCompatibility.FindObjectsByTypeSorted<RecastNavmeshModifier>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].enabled && bounds.Intersects(array[i].CalculateBounds()))
					{
						buffer.Add(array[i]);
					}
				}
				return;
			}
			if (Time.timeSinceLevelLoad == 0f)
			{
				RecastNavmeshModifier[] array2 = UnityCompatibility.FindObjectsByTypeUnsorted<RecastNavmeshModifier>();
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].OnEnable();
				}
			}
			RecastNavmeshModifier.tree.Query(bounds, buffer);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0002D0D4 File Offset: 0x0002B2D4
		public void ResolveMeshSource(out MeshFilter meshFilter, out Collider collider, out Collider2D collider2D)
		{
			meshFilter = null;
			collider = null;
			collider2D = null;
			switch (this.geometrySource)
			{
			case RecastNavmeshModifier.GeometrySource.Auto:
			{
				MeshRenderer meshRenderer;
				if (base.TryGetComponent<MeshRenderer>(out meshRenderer) && base.TryGetComponent<MeshFilter>(out meshFilter) && meshFilter.sharedMesh != null)
				{
					return;
				}
				if (base.TryGetComponent<Collider>(out collider))
				{
					return;
				}
				base.TryGetComponent<Collider2D>(out collider2D);
				return;
			}
			case RecastNavmeshModifier.GeometrySource.MeshFilter:
				base.TryGetComponent<MeshFilter>(out meshFilter);
				return;
			case RecastNavmeshModifier.GeometrySource.Collider:
				if (base.TryGetComponent<Collider>(out collider))
				{
					return;
				}
				base.TryGetComponent<Collider2D>(out collider2D);
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0002D15C File Offset: 0x0002B35C
		private Bounds CalculateBounds()
		{
			MeshFilter x;
			Collider collider;
			Collider2D collider2D;
			this.ResolveMeshSource(out x, out collider, out collider2D);
			if (collider != null)
			{
				return collider.bounds;
			}
			if (collider2D != null)
			{
				return collider2D.bounds;
			}
			if (!(x != null))
			{
				Debug.LogError("Could not find an attached mesh source", this);
				return new Bounds(Vector3.zero, Vector3.one);
			}
			MeshRenderer meshRenderer;
			if (base.TryGetComponent<MeshRenderer>(out meshRenderer))
			{
				return meshRenderer.bounds;
			}
			Debug.LogError("Cannot use a MeshFilter as a geomtry source without a MeshRenderer attached to the same GameObject.", this);
			return new Bounds(Vector3.zero, Vector3.one);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0002D1E4 File Offset: 0x0002B3E4
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num))
			{
				if (num == 1)
				{
					this.area = this.surfaceID;
				}
				if (num <= 2)
				{
					this.includeInScan = RecastNavmeshModifier.ScanInclusion.AlwaysInclude;
				}
				if (this.mode == (RecastNavmeshModifier.Mode)0)
				{
					this.includeInScan = RecastNavmeshModifier.ScanInclusion.AlwaysExclude;
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0002D225 File Offset: 0x0002B425
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x0002D22D File Offset: 0x0002B42D
		bool RecastMeshObj.dynamic
		{
			get
			{
				return this.dynamic;
			}
			set
			{
				this.dynamic = value;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0002D236 File Offset: 0x0002B436
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x0002D23E File Offset: 0x0002B43E
		bool RecastMeshObj.solid
		{
			get
			{
				return this.solid;
			}
			set
			{
				this.solid = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x0002D247 File Offset: 0x0002B447
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x0002D24F File Offset: 0x0002B44F
		RecastNavmeshModifier.GeometrySource RecastMeshObj.geometrySource
		{
			get
			{
				return this.geometrySource;
			}
			set
			{
				this.geometrySource = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0002D258 File Offset: 0x0002B458
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x0002D260 File Offset: 0x0002B460
		RecastNavmeshModifier.ScanInclusion RecastMeshObj.includeInScan
		{
			get
			{
				return this.includeInScan;
			}
			set
			{
				this.includeInScan = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0002D269 File Offset: 0x0002B469
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x0002D271 File Offset: 0x0002B471
		int RecastMeshObj.surfaceID
		{
			get
			{
				return this.surfaceID;
			}
			set
			{
				this.surfaceID = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0002D27A File Offset: 0x0002B47A
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x0002D282 File Offset: 0x0002B482
		RecastNavmeshModifier.Mode RecastMeshObj.mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0002D2B4 File Offset: 0x0002B4B4
		bool RecastMeshObj.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0002D2BC File Offset: 0x0002B4BC
		void RecastMeshObj.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x04000581 RID: 1409
		protected static AABBTree<RecastNavmeshModifier> tree = new AABBTree<RecastNavmeshModifier>();

		// Token: 0x04000582 RID: 1410
		public bool dynamic = true;

		// Token: 0x04000583 RID: 1411
		public bool solid;

		// Token: 0x04000584 RID: 1412
		public RecastNavmeshModifier.GeometrySource geometrySource;

		// Token: 0x04000585 RID: 1413
		public RecastNavmeshModifier.ScanInclusion includeInScan;

		// Token: 0x04000586 RID: 1414
		[FormerlySerializedAs("area")]
		public int surfaceID = 1;

		// Token: 0x04000587 RID: 1415
		public RecastNavmeshModifier.Mode mode = RecastNavmeshModifier.Mode.WalkableSurface;

		// Token: 0x04000588 RID: 1416
		private AABBTree<RecastNavmeshModifier>.Key treeKey;

		// Token: 0x02000108 RID: 264
		public enum ScanInclusion
		{
			// Token: 0x0400058A RID: 1418
			Auto,
			// Token: 0x0400058B RID: 1419
			AlwaysExclude,
			// Token: 0x0400058C RID: 1420
			AlwaysInclude
		}

		// Token: 0x02000109 RID: 265
		public enum GeometrySource
		{
			// Token: 0x0400058E RID: 1422
			Auto,
			// Token: 0x0400058F RID: 1423
			MeshFilter,
			// Token: 0x04000590 RID: 1424
			Collider
		}

		// Token: 0x0200010A RID: 266
		public enum Mode
		{
			// Token: 0x04000592 RID: 1426
			UnwalkableSurface = 1,
			// Token: 0x04000593 RID: 1427
			WalkableSurface,
			// Token: 0x04000594 RID: 1428
			WalkableSurfaceWithSeam,
			// Token: 0x04000595 RID: 1429
			WalkableSurfaceWithTag
		}
	}
}
