using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200001C RID: 28
	[ExecuteInEditMode]
	public class MeshCombinerData : MonoBehaviour
	{
		// Token: 0x06000091 RID: 145 RVA: 0x0000716E File Offset: 0x0000536E
		private void OnValidate()
		{
			base.hideFlags = HideFlags.HideInInspector;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007177 File Offset: 0x00005377
		private void OnEnable()
		{
			base.hideFlags = HideFlags.HideInInspector;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00007180 File Offset: 0x00005380
		public void ClearAll()
		{
			this.combinedGameObjects.Clear();
			this.foundObjects.Clear();
			this.foundLodObjects.Clear();
			this.foundLodGroups.Clear();
			this.foundColliders.Clear();
			this.colliderLookup.Clear();
			this.lodGroupLookup.Clear();
		}

		// Token: 0x040000E4 RID: 228
		public Dictionary<Collider, CachedGameObject> colliderLookup = new Dictionary<Collider, CachedGameObject>();

		// Token: 0x040000E5 RID: 229
		public Dictionary<LODGroup, CachedGameObject> lodGroupLookup = new Dictionary<LODGroup, CachedGameObject>();

		// Token: 0x040000E6 RID: 230
		[HideInInspector]
		public List<GameObject> combinedGameObjects = new List<GameObject>();

		// Token: 0x040000E7 RID: 231
		[HideInInspector]
		public List<CachedGameObject> foundObjects = new List<CachedGameObject>();

		// Token: 0x040000E8 RID: 232
		[HideInInspector]
		public List<CachedLodGameObject> foundLodObjects = new List<CachedLodGameObject>();

		// Token: 0x040000E9 RID: 233
		[HideInInspector]
		public List<LODGroup> foundLodGroups = new List<LODGroup>();

		// Token: 0x040000EA RID: 234
		[HideInInspector]
		public List<Collider> foundColliders = new List<Collider>();
	}
}
