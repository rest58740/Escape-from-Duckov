using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class OcclusionFadeManager : MonoBehaviour
{
	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06000C14 RID: 3092 RVA: 0x0003399F File Offset: 0x00031B9F
	public static OcclusionFadeManager Instance
	{
		get
		{
			if (!OcclusionFadeManager.instance)
			{
				OcclusionFadeManager.instance = Object.FindFirstObjectByType<OcclusionFadeManager>();
			}
			return OcclusionFadeManager.instance;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000C15 RID: 3093 RVA: 0x000339BC File Offset: 0x00031BBC
	public float startFadeHeight
	{
		get
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (!main || !main.gameObject.activeInHierarchy)
			{
				return 0.25f;
			}
			return main.transform.position.y + 0.25f;
		}
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00033A00 File Offset: 0x00031C00
	private void Awake()
	{
		this.materialDic = new Dictionary<Material, Material>();
		this.aimOcclusionFadeChecker.gameObject.layer = LayerMask.NameToLayer("VisualOcclusion");
		this.characterOcclusionFadeChecker.gameObject.layer = LayerMask.NameToLayer("VisualOcclusion");
		this.SetShader();
		Shader.SetGlobalTexture("FadeNoiseTexture", this.fadeNoiseTexture);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x00033A62 File Offset: 0x00031C62
	private void OnValidate()
	{
		this.SetShader();
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00033A6C File Offset: 0x00031C6C
	private void SetShader()
	{
		Shader.SetGlobalFloat(this.viewRangeHash, this.viewRange);
		Shader.SetGlobalFloat(this.viewFadeRangeHash, this.viewFadeRange);
		Shader.SetGlobalFloat(this.startFadeHeightHash, this.startFadeHeight);
		Shader.SetGlobalFloat(this.heightFadeRangeHash, this.heightFadeRange);
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00033AC0 File Offset: 0x00031CC0
	private void Update()
	{
		if (!this.character)
		{
			if (!LevelManager.Instance)
			{
				return;
			}
			this.character = LevelManager.Instance.MainCharacter;
			this.cam = LevelManager.Instance.GameCamera.renderCamera;
			return;
		}
		else
		{
			if (!OcclusionFadeManager.FadeEnabled)
			{
				Shader.SetGlobalVector(this.charactetrPosHash, Vector3.one * -9999f);
				Shader.SetGlobalVector(this.aimPosHash, Vector3.one * -9999f);
				if (this.aimOcclusionFadeChecker.gameObject.activeInHierarchy)
				{
					this.aimOcclusionFadeChecker.gameObject.SetActive(false);
				}
				if (this.characterOcclusionFadeChecker.gameObject.activeInHierarchy)
				{
					this.characterOcclusionFadeChecker.gameObject.SetActive(false);
				}
				return;
			}
			this.aimOcclusionFadeChecker.transform.position = LevelManager.Instance.InputManager.InputAimPoint;
			Vector3 normalized = (this.aimOcclusionFadeChecker.transform.position - this.cam.transform.position).normalized;
			this.aimOcclusionFadeChecker.transform.rotation = Quaternion.LookRotation(-normalized);
			Shader.SetGlobalVector(this.aimViewDirHash, normalized);
			Shader.SetGlobalVector(this.aimPosHash, this.aimOcclusionFadeChecker.transform.position);
			this.characterOcclusionFadeChecker.transform.position = this.character.transform.position;
			Vector3 normalized2 = (this.characterOcclusionFadeChecker.transform.position - this.cam.transform.position).normalized;
			this.characterOcclusionFadeChecker.transform.rotation = Quaternion.LookRotation(-normalized2);
			Shader.SetGlobalVector(this.characterViewDirHash, normalized2);
			Shader.SetGlobalFloat(this.startFadeHeightHash, this.startFadeHeight);
			Shader.SetGlobalVector(this.charactetrPosHash, this.character.transform.position);
			return;
		}
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00033CE0 File Offset: 0x00031EE0
	public Material GetMaskedMaterial(Material mat)
	{
		if (mat == null)
		{
			return null;
		}
		if (!this.supportedShaders.Contains(mat.shader))
		{
			return mat;
		}
		if (this.materialDic.ContainsKey(mat))
		{
			return this.materialDic[mat];
		}
		Material material = new Material(this.maskedShader);
		material.CopyPropertiesFromMaterial(mat);
		this.materialDic.Add(mat, material);
		return material;
	}

	// Token: 0x04000A60 RID: 2656
	private static OcclusionFadeManager instance;

	// Token: 0x04000A61 RID: 2657
	public OcclusionFadeChecker aimOcclusionFadeChecker;

	// Token: 0x04000A62 RID: 2658
	public OcclusionFadeChecker characterOcclusionFadeChecker;

	// Token: 0x04000A63 RID: 2659
	private CharacterMainControl character;

	// Token: 0x04000A64 RID: 2660
	private Camera cam;

	// Token: 0x04000A65 RID: 2661
	public Dictionary<Material, Material> materialDic;

	// Token: 0x04000A66 RID: 2662
	public List<Shader> supportedShaders;

	// Token: 0x04000A67 RID: 2663
	public Shader maskedShader;

	// Token: 0x04000A68 RID: 2664
	public Material testMat;

	// Token: 0x04000A69 RID: 2665
	[Range(0f, 4f)]
	public float viewRange;

	// Token: 0x04000A6A RID: 2666
	[Range(0f, 8f)]
	public float viewFadeRange;

	// Token: 0x04000A6B RID: 2667
	public Texture2D fadeNoiseTexture;

	// Token: 0x04000A6C RID: 2668
	public static bool FadeEnabled = true;

	// Token: 0x04000A6D RID: 2669
	public float heightFadeRange;

	// Token: 0x04000A6E RID: 2670
	private int aimViewDirHash = Shader.PropertyToID("OC_AimViewDir");

	// Token: 0x04000A6F RID: 2671
	private int aimPosHash = Shader.PropertyToID("OC_AimPos");

	// Token: 0x04000A70 RID: 2672
	private int characterViewDirHash = Shader.PropertyToID("OC_CharacterViewDir");

	// Token: 0x04000A71 RID: 2673
	private int charactetrPosHash = Shader.PropertyToID("OC_CharacterPos");

	// Token: 0x04000A72 RID: 2674
	private int viewRangeHash = Shader.PropertyToID("ViewRange");

	// Token: 0x04000A73 RID: 2675
	private int viewFadeRangeHash = Shader.PropertyToID("ViewFadeRange");

	// Token: 0x04000A74 RID: 2676
	private int startFadeHeightHash = Shader.PropertyToID("StartFadeHeight");

	// Token: 0x04000A75 RID: 2677
	private int heightFadeRangeHash = Shader.PropertyToID("HeightFadeRange");
}
