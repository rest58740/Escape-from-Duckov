using System;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000003 RID: 3
	[RequireComponent(typeof(Camera))]
	[ImageEffectAllowedInSceneView]
	[ExecuteInEditMode]
	public class FOWImageEffect : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		private void Awake()
		{
			this.SetCamera();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		private void SetCamera()
		{
			if (this.cam)
			{
				return;
			}
			this.cam = base.GetComponent<Camera>();
			this.cam.depthTextureMode = (DepthTextureMode.Depth | DepthTextureMode.DepthNormals);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020F0 File Offset: 0x000002F0
		private void OnPreRender()
		{
			if (!FogOfWarWorld.instance)
			{
				return;
			}
			if (!FogOfWarWorld.instance.is2D)
			{
				Matrix4x4 cameraToWorldMatrix = this.cam.cameraToWorldMatrix;
				FogOfWarWorld.instance.FogOfWarMaterial.SetMatrix("_camToWorldMatrix", cameraToWorldMatrix);
				return;
			}
			FogOfWarWorld.instance.FogOfWarMaterial.SetFloat("_cameraSize", this.cam.orthographicSize);
			FogOfWarWorld.instance.FogOfWarMaterial.SetVector("_cameraPosition", this.cam.transform.position);
			FogOfWarWorld.instance.FogOfWarMaterial.SetFloat("_cameraRotation", Mathf.DeltaAngle(0f, this.cam.transform.eulerAngles.z));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021B3 File Offset: 0x000003B3
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (!FogOfWarWorld.instance || !FogOfWarWorld.instance.enabled)
			{
				Graphics.Blit(src, dest);
				return;
			}
			Graphics.Blit(src, dest, FogOfWarWorld.instance.FogOfWarMaterial);
		}

		// Token: 0x04000001 RID: 1
		private Camera cam;
	}
}
