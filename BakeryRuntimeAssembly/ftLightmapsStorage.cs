using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000011 RID: 17
[ExecuteInEditMode]
public class ftLightmapsStorage : MonoBehaviour
{
	// Token: 0x06000021 RID: 33 RVA: 0x000034FA File Offset: 0x000016FA
	private void Awake()
	{
		ftLightmaps.RefreshScene(base.gameObject.scene, this, false);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x0000350E File Offset: 0x0000170E
	private void Start()
	{
		ftLightmaps.RefreshScene2(base.gameObject.scene, this);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00003521 File Offset: 0x00001721
	private void OnDestroy()
	{
		ftLightmaps.UnloadScene(this);
	}

	// Token: 0x040000A7 RID: 167
	public List<Texture2D> maps = new List<Texture2D>();

	// Token: 0x040000A8 RID: 168
	public List<Texture2D> masks = new List<Texture2D>();

	// Token: 0x040000A9 RID: 169
	public List<Texture2D> dirMaps = new List<Texture2D>();

	// Token: 0x040000AA RID: 170
	public List<Texture2D> rnmMaps0 = new List<Texture2D>();

	// Token: 0x040000AB RID: 171
	public List<Texture2D> rnmMaps1 = new List<Texture2D>();

	// Token: 0x040000AC RID: 172
	public List<Texture2D> rnmMaps2 = new List<Texture2D>();

	// Token: 0x040000AD RID: 173
	public List<int> mapsMode = new List<int>();

	// Token: 0x040000AE RID: 174
	public List<Renderer> bakedRenderers = new List<Renderer>();

	// Token: 0x040000AF RID: 175
	public List<int> bakedIDs = new List<int>();

	// Token: 0x040000B0 RID: 176
	public List<Vector4> bakedScaleOffset = new List<Vector4>();

	// Token: 0x040000B1 RID: 177
	public List<Mesh> bakedVertexColorMesh = new List<Mesh>();

	// Token: 0x040000B2 RID: 178
	public List<Renderer> nonBakedRenderers = new List<Renderer>();

	// Token: 0x040000B3 RID: 179
	public List<Light> bakedLights = new List<Light>();

	// Token: 0x040000B4 RID: 180
	public List<int> bakedLightChannels = new List<int>();

	// Token: 0x040000B5 RID: 181
	public List<Terrain> bakedRenderersTerrain = new List<Terrain>();

	// Token: 0x040000B6 RID: 182
	public List<int> bakedIDsTerrain = new List<int>();

	// Token: 0x040000B7 RID: 183
	public List<Vector4> bakedScaleOffsetTerrain = new List<Vector4>();

	// Token: 0x040000B8 RID: 184
	public List<string> assetList = new List<string>();

	// Token: 0x040000B9 RID: 185
	public List<int> uvOverlapAssetList = new List<int>();

	// Token: 0x040000BA RID: 186
	public int[] idremap;

	// Token: 0x040000BB RID: 187
	public bool usesRealtimeGI;

	// Token: 0x040000BC RID: 188
	public Texture2D emptyDirectionTex;

	// Token: 0x040000BD RID: 189
	public bool anyVolumes;

	// Token: 0x040000BE RID: 190
	public bool compressedVolumes;
}
