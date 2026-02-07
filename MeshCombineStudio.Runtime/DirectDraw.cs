using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000030 RID: 48
	public class DirectDraw : MonoBehaviour
	{
		// Token: 0x0600011B RID: 283 RVA: 0x0000AF64 File Offset: 0x00009164
		private void Awake()
		{
			this.mrs = base.GetComponentsInChildren<MeshRenderer>(false);
			this.SetMeshRenderersEnabled(false);
			this.meshes = new Mesh[this.mrs.Length];
			this.mats = new Material[this.mrs.Length];
			this.positions = new Vector3[this.mrs.Length];
			this.rotations = new Quaternion[this.mrs.Length];
			for (int i = 0; i < this.mrs.Length; i++)
			{
				MeshFilter component = this.mrs[i].GetComponent<MeshFilter>();
				this.meshes[i] = component.sharedMesh;
				this.mats[i] = this.mrs[i].sharedMaterial;
				this.positions[i] = this.mrs[i].transform.position;
				this.rotations[i] = this.mrs[i].transform.rotation;
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000B054 File Offset: 0x00009254
		private void SetMeshRenderersEnabled(bool enabled)
		{
			for (int i = 0; i < this.mrs.Length; i++)
			{
				this.mrs[i].enabled = enabled;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000B084 File Offset: 0x00009284
		private void Update()
		{
			for (int i = 0; i < this.mrs.Length; i++)
			{
				Graphics.DrawMesh(this.meshes[i], this.positions[i], this.rotations[i], this.mats[i], 0);
			}
		}

		// Token: 0x04000131 RID: 305
		private MeshRenderer[] mrs;

		// Token: 0x04000132 RID: 306
		private Mesh[] meshes;

		// Token: 0x04000133 RID: 307
		private Material[] mats;

		// Token: 0x04000134 RID: 308
		private Vector3[] positions;

		// Token: 0x04000135 RID: 309
		private Quaternion[] rotations;
	}
}
