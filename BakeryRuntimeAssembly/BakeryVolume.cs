using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
[ExecuteInEditMode]
public class BakeryVolume : MonoBehaviour
{
	// Token: 0x0600000F RID: 15 RVA: 0x00002515 File Offset: 0x00000715
	public Vector3 GetMin()
	{
		return this.bounds.min;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002524 File Offset: 0x00000724
	public Vector3 GetInvSize()
	{
		Bounds bounds = this.bounds;
		return new Vector3(1f / bounds.size.x, 1f / bounds.size.y, 1f / bounds.size.z);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002574 File Offset: 0x00000774
	public Matrix4x4 GetMatrix()
	{
		if (this.tform == null)
		{
			this.tform = base.transform;
		}
		return Matrix4x4.TRS(this.tform.position, this.tform.rotation, Vector3.one).inverse;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000025C4 File Offset: 0x000007C4
	public void SetGlobalParams()
	{
		Shader.SetGlobalTexture("_Volume0", this.bakedTexture0);
		Shader.SetGlobalTexture("_Volume1", this.bakedTexture1);
		Shader.SetGlobalTexture("_Volume2", this.bakedTexture2);
		if (this.bakedTexture3 != null)
		{
			Shader.SetGlobalTexture("_Volume3", this.bakedTexture3);
		}
		Shader.SetGlobalTexture("_VolumeMask", this.bakedMask);
		Bounds bounds = this.bounds;
		Vector3 min = bounds.min;
		Vector3 v = new Vector3(1f / bounds.size.x, 1f / bounds.size.y, 1f / bounds.size.z);
		Shader.SetGlobalVector("_GlobalVolumeMin", min);
		Shader.SetGlobalVector("_GlobalVolumeInvSize", v);
		if (this.supportRotationAfterBake)
		{
			Shader.SetGlobalMatrix("_GlobalVolumeMatrix", this.GetMatrix());
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000026B4 File Offset: 0x000008B4
	public void UpdateBounds()
	{
		Vector3 position = base.transform.position;
		Vector3 size = this.bounds.size;
		this.bounds = new Bounds(position, size);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000026E6 File Offset: 0x000008E6
	public void OnEnable()
	{
		if (this.isGlobal)
		{
			BakeryVolume.globalVolume = this;
			this.SetGlobalParams();
		}
	}

	// Token: 0x04000090 RID: 144
	public bool enableBaking = true;

	// Token: 0x04000091 RID: 145
	public Bounds bounds = new Bounds(Vector3.zero, Vector3.one);

	// Token: 0x04000092 RID: 146
	public bool adaptiveRes = true;

	// Token: 0x04000093 RID: 147
	public float voxelsPerUnit = 0.5f;

	// Token: 0x04000094 RID: 148
	public int resolutionX = 16;

	// Token: 0x04000095 RID: 149
	public int resolutionY = 16;

	// Token: 0x04000096 RID: 150
	public int resolutionZ = 16;

	// Token: 0x04000097 RID: 151
	public BakeryVolume.Encoding encoding;

	// Token: 0x04000098 RID: 152
	public BakeryVolume.ShadowmaskEncoding shadowmaskEncoding;

	// Token: 0x04000099 RID: 153
	public bool firstLightIsAlwaysAlpha;

	// Token: 0x0400009A RID: 154
	public bool denoise;

	// Token: 0x0400009B RID: 155
	public bool isGlobal;

	// Token: 0x0400009C RID: 156
	public Texture3D bakedTexture0;

	// Token: 0x0400009D RID: 157
	public Texture3D bakedTexture1;

	// Token: 0x0400009E RID: 158
	public Texture3D bakedTexture2;

	// Token: 0x0400009F RID: 159
	public Texture3D bakedTexture3;

	// Token: 0x040000A0 RID: 160
	public Texture3D bakedMask;

	// Token: 0x040000A1 RID: 161
	public bool supportRotationAfterBake;

	// Token: 0x040000A2 RID: 162
	public static BakeryVolume globalVolume;

	// Token: 0x040000A3 RID: 163
	private Transform tform;

	// Token: 0x0200001D RID: 29
	public enum Encoding
	{
		// Token: 0x040000EE RID: 238
		Half4,
		// Token: 0x040000EF RID: 239
		RGBA8,
		// Token: 0x040000F0 RID: 240
		RGBA8Mono
	}

	// Token: 0x0200001E RID: 30
	public enum ShadowmaskEncoding
	{
		// Token: 0x040000F2 RID: 242
		RGBA8,
		// Token: 0x040000F3 RID: 243
		A8
	}
}
