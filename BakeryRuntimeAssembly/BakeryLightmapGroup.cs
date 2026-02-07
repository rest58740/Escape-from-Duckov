using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
[CreateAssetMenu(menuName = "Bakery lightmap group")]
public class BakeryLightmapGroup : ScriptableObject
{
	// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
	public BakeryLightmapGroupPlain GetPlainStruct()
	{
		BakeryLightmapGroupPlain result;
		result.name = base.name;
		result.id = this.id;
		result.resolution = this.resolution;
		result.vertexBake = (this.mode == BakeryLightmapGroup.ftLMGroupMode.Vertex);
		result.isImplicit = this.isImplicit;
		result.renderMode = (int)this.renderMode;
		result.renderDirMode = (int)this.renderDirMode;
		result.atlasPacker = (int)this.atlasPacker;
		result.holeFilling = (int)this.holeFilling;
		result.computeSSS = this.computeSSS;
		result.sssSamples = this.sssSamples;
		result.sssDensity = this.sssDensity;
		result.sssR = this.sssColor.r * this.sssScale;
		result.sssG = this.sssColor.g * this.sssScale;
		result.sssB = this.sssColor.b * this.sssScale;
		result.containsTerrains = this.containsTerrains;
		result.probes = this.probes;
		result.fakeShadowBias = this.fakeShadowBias;
		result.transparentSelfShadow = this.transparentSelfShadow;
		result.flipNormal = this.flipNormal;
		result.parentName = this.parentName;
		result.sceneLodLevel = this.sceneLodLevel;
		result.autoResolution = this.autoResolution;
		return result;
	}

	// Token: 0x0400002A RID: 42
	[SerializeField]
	[Range(1f, 8192f)]
	public int resolution = 512;

	// Token: 0x0400002B RID: 43
	[SerializeField]
	public int bitmask = 1;

	// Token: 0x0400002C RID: 44
	[SerializeField]
	public int id = -1;

	// Token: 0x0400002D RID: 45
	public int sortingID = -1;

	// Token: 0x0400002E RID: 46
	[SerializeField]
	public bool isImplicit;

	// Token: 0x0400002F RID: 47
	[SerializeField]
	public float area;

	// Token: 0x04000030 RID: 48
	[SerializeField]
	public int totalVertexCount;

	// Token: 0x04000031 RID: 49
	[SerializeField]
	public int vertexCounter;

	// Token: 0x04000032 RID: 50
	[SerializeField]
	public int sceneLodLevel = -1;

	// Token: 0x04000033 RID: 51
	[SerializeField]
	public bool autoResolution;

	// Token: 0x04000034 RID: 52
	[SerializeField]
	public string sceneName;

	// Token: 0x04000035 RID: 53
	[SerializeField]
	public int tag = -1;

	// Token: 0x04000036 RID: 54
	[SerializeField]
	public bool containsTerrains;

	// Token: 0x04000037 RID: 55
	[SerializeField]
	public bool probes;

	// Token: 0x04000038 RID: 56
	[SerializeField]
	public BakeryLightmapGroup.ftLMGroupMode mode = BakeryLightmapGroup.ftLMGroupMode.PackAtlas;

	// Token: 0x04000039 RID: 57
	[SerializeField]
	public BakeryLightmapGroup.RenderMode renderMode = BakeryLightmapGroup.RenderMode.Auto;

	// Token: 0x0400003A RID: 58
	[SerializeField]
	public BakeryLightmapGroup.RenderDirMode renderDirMode = BakeryLightmapGroup.RenderDirMode.Auto;

	// Token: 0x0400003B RID: 59
	[SerializeField]
	public BakeryLightmapGroup.AtlasPacker atlasPacker = BakeryLightmapGroup.AtlasPacker.Auto;

	// Token: 0x0400003C RID: 60
	[SerializeField]
	public BakeryLightmapGroup.HoleFilling holeFilling;

	// Token: 0x0400003D RID: 61
	[SerializeField]
	public bool computeSSS;

	// Token: 0x0400003E RID: 62
	[SerializeField]
	public int sssSamples = 16;

	// Token: 0x0400003F RID: 63
	[SerializeField]
	public float sssDensity = 10f;

	// Token: 0x04000040 RID: 64
	[SerializeField]
	public Color sssColor = Color.white;

	// Token: 0x04000041 RID: 65
	[SerializeField]
	public float sssScale = 1f;

	// Token: 0x04000042 RID: 66
	[SerializeField]
	public float fakeShadowBias;

	// Token: 0x04000043 RID: 67
	[SerializeField]
	public bool transparentSelfShadow;

	// Token: 0x04000044 RID: 68
	[SerializeField]
	public bool flipNormal;

	// Token: 0x04000045 RID: 69
	[SerializeField]
	public string parentName;

	// Token: 0x04000046 RID: 70
	[SerializeField]
	public string overridePath = "";

	// Token: 0x04000047 RID: 71
	[SerializeField]
	public bool fixPos3D;

	// Token: 0x04000048 RID: 72
	[SerializeField]
	public Vector3 voxelSize = Vector3.one;

	// Token: 0x04000049 RID: 73
	public int passedFilter;

	// Token: 0x02000015 RID: 21
	public enum ftLMGroupMode
	{
		// Token: 0x040000C4 RID: 196
		OriginalUV,
		// Token: 0x040000C5 RID: 197
		PackAtlas,
		// Token: 0x040000C6 RID: 198
		Vertex
	}

	// Token: 0x02000016 RID: 22
	public enum RenderMode
	{
		// Token: 0x040000C8 RID: 200
		FullLighting,
		// Token: 0x040000C9 RID: 201
		Indirect,
		// Token: 0x040000CA RID: 202
		Shadowmask,
		// Token: 0x040000CB RID: 203
		Subtractive,
		// Token: 0x040000CC RID: 204
		AmbientOcclusionOnly,
		// Token: 0x040000CD RID: 205
		Auto = 1000
	}

	// Token: 0x02000017 RID: 23
	public enum RenderDirMode
	{
		// Token: 0x040000CF RID: 207
		None,
		// Token: 0x040000D0 RID: 208
		BakedNormalMaps,
		// Token: 0x040000D1 RID: 209
		DominantDirection,
		// Token: 0x040000D2 RID: 210
		RNM,
		// Token: 0x040000D3 RID: 211
		SH,
		// Token: 0x040000D4 RID: 212
		ProbeSH,
		// Token: 0x040000D5 RID: 213
		MonoSH,
		// Token: 0x040000D6 RID: 214
		Auto = 1000
	}

	// Token: 0x02000018 RID: 24
	public enum AtlasPacker
	{
		// Token: 0x040000D8 RID: 216
		Default,
		// Token: 0x040000D9 RID: 217
		xatlas,
		// Token: 0x040000DA RID: 218
		Auto = 1000
	}

	// Token: 0x02000019 RID: 25
	public enum HoleFilling
	{
		// Token: 0x040000DC RID: 220
		Auto,
		// Token: 0x040000DD RID: 221
		Yes,
		// Token: 0x040000DE RID: 222
		No
	}
}
