using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000048 RID: 72
	[Serializable]
	public class MeshObjectsHolder
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0000E038 File Offset: 0x0000C238
		public MeshObjectsHolder(ref CombineCondition combineCondition, Material mat)
		{
			this.mat = mat;
			this.combineCondition = combineCondition;
		}

		// Token: 0x040001C2 RID: 450
		public FastList<MeshObject> meshObjects = new FastList<MeshObject>();

		// Token: 0x040001C3 RID: 451
		public ObjectOctree.LODParent lodParent;

		// Token: 0x040001C4 RID: 452
		public FastList<CachedGameObject> newCachedGOs;

		// Token: 0x040001C5 RID: 453
		public int lodLevel;

		// Token: 0x040001C6 RID: 454
		public Material mat;

		// Token: 0x040001C7 RID: 455
		public bool hasChanged;

		// Token: 0x040001C8 RID: 456
		public CombineCondition combineCondition;
	}
}
