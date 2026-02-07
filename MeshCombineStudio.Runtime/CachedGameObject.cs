using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200004D RID: 77
	[Serializable]
	public class CachedGameObject
	{
		// Token: 0x06000195 RID: 405 RVA: 0x0000E7C4 File Offset: 0x0000C9C4
		public CachedGameObject(Transform searchParentT, GameObject go, Transform t, MeshRenderer mr, MeshFilter mf, Mesh mesh)
		{
			this.searchParentT = searchParentT;
			this.go = go;
			this.t = t;
			this.mr = mr;
			this.mf = mf;
			this.mesh = mesh;
			this.mt = t.localToWorldMatrix;
			this.mrEnabled = mr.enabled;
			this.mtNormals = this.mt.inverse.transpose;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000E840 File Offset: 0x0000CA40
		public CachedGameObject(CachedComponents cachedComponent)
		{
			this.go = cachedComponent.go;
			this.t = cachedComponent.t;
			this.mr = cachedComponent.mr;
			this.mf = cachedComponent.mf;
			this.mesh = cachedComponent.mf.sharedMesh;
			this.mt = this.t.localToWorldMatrix;
			this.mtNormals = this.mt.inverse.transpose;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000E8C5 File Offset: 0x0000CAC5
		public void GetRoot()
		{
			this.rootT = Methods.GetChildRootTransform(this.t, this.searchParentT);
			this.rootInstanceId = this.rootT.GetInstanceID();
			this.rootTLossyScale = this.rootT.lossyScale;
		}

		// Token: 0x040001FC RID: 508
		public Transform searchParentT;

		// Token: 0x040001FD RID: 509
		public GameObject go;

		// Token: 0x040001FE RID: 510
		public Transform t;

		// Token: 0x040001FF RID: 511
		public MeshRenderer mr;

		// Token: 0x04000200 RID: 512
		public MeshFilterRevert mfr;

		// Token: 0x04000201 RID: 513
		public MeshFilter mf;

		// Token: 0x04000202 RID: 514
		public Mesh mesh;

		// Token: 0x04000203 RID: 515
		public Matrix4x4 mt;

		// Token: 0x04000204 RID: 516
		public Matrix4x4 mtNormals;

		// Token: 0x04000205 RID: 517
		public Transform rootT;

		// Token: 0x04000206 RID: 518
		public Vector3 rootTLossyScale;

		// Token: 0x04000207 RID: 519
		public int rootInstanceId = -1;

		// Token: 0x04000208 RID: 520
		public bool excludeCombine;

		// Token: 0x04000209 RID: 521
		public bool mrEnabled;
	}
}
