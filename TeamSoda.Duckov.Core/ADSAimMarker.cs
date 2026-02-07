using System;
using System.Collections.Generic;
using Duckov.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000075 RID: 117
public class ADSAimMarker : MonoBehaviour
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x0600045B RID: 1115 RVA: 0x00013F34 File Offset: 0x00012134
	public float CanvasAlpha
	{
		get
		{
			return this.canvasAlpha;
		}
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x00013F3C File Offset: 0x0001213C
	public void CollectCrosshairs()
	{
		this.crosshairs.Clear();
		foreach (SingleCrosshair item in base.GetComponentsInChildren<SingleCrosshair>())
		{
			this.crosshairs.Add(item);
		}
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00013F7C File Offset: 0x0001217C
	private void Awake()
	{
		this.proceduralImageCanvasGroups = new List<CanvasGroup>();
		for (int i = 0; i < this.proceduralImages.Count; i++)
		{
			this.proceduralImageCanvasGroups.Add(this.proceduralImages[i].GetComponent<CanvasGroup>());
		}
		this.selfRect = base.GetComponent<RectTransform>();
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00013FD4 File Offset: 0x000121D4
	private bool LockTraceTarget()
	{
		if (!this.gun)
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (!main)
			{
				return false;
			}
			this.gun = main.GetGun();
		}
		if (!this.gun)
		{
			return false;
		}
		if (!this.camera)
		{
			this.camera = Camera.main;
		}
		if (!this.gun.TraceTarget)
		{
			this.followUI.position = Vector3.Lerp(this.followUI.position, this.aimMarkerUI.position, Time.deltaTime * this.followSpeed);
		}
		else
		{
			Vector3 position = this.gun.TraceTarget.characterModel.HelmatSocket.position;
			this.followUI.position = Vector3.Lerp(this.followUI.position, RectTransformUtility.WorldToScreenPoint(this.camera, position), Time.deltaTime * this.followSpeed);
		}
		return true;
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x000140CC File Offset: 0x000122CC
	private void LateUpdate()
	{
		if (this.selfRect)
		{
			this.selfRect.localScale = Vector3.one;
		}
		if (!this.LockTraceTarget())
		{
			this.followUI.position = Vector3.Lerp(this.followUI.position, this.aimMarkerUI.position, Time.deltaTime * this.followSpeed);
			if (Vector3.Distance(this.followUI.position, this.aimMarkerUI.position) > this.followMaxDistance)
			{
				this.followUI.position = Vector3.MoveTowards(this.aimMarkerUI.position, this.followUI.position, this.followMaxDistance);
			}
		}
		foreach (SingleCrosshair singleCrosshair in this.crosshairs)
		{
			if (singleCrosshair)
			{
				singleCrosshair.UpdateScatter(this.scatter);
			}
		}
		this.SetSniperRenderer();
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x000141DC File Offset: 0x000123DC
	public void SetAimMarkerPos(Vector3 pos)
	{
		this.aimMarkerUI.position = pos;
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x000141EC File Offset: 0x000123EC
	public void OnShoot()
	{
		foreach (PunchReceiver punchReceiver in this.shootPunchReceivers)
		{
			if (punchReceiver)
			{
				punchReceiver.Punch();
			}
		}
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x00014248 File Offset: 0x00012448
	public void SetScatter(float _currentScatter, float _minScatter)
	{
		this.scatter = _currentScatter;
		this.minScatter = _minScatter;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00014258 File Offset: 0x00012458
	public void SetAdsValue(float _adsValue)
	{
		this.adsValue = _adsValue;
		this.canvasAlpha = _adsValue;
		if (this.adsAlphaRemap.y > this.adsAlphaRemap.x)
		{
			this.canvasAlpha = Mathf.Clamp01((_adsValue - this.adsAlphaRemap.x) / (this.adsAlphaRemap.y - this.adsAlphaRemap.x));
		}
		this.canvasGroup.alpha = this.canvasAlpha;
		for (int i = 0; i < this.proceduralImages.Count; i++)
		{
			ProceduralImage proceduralImage = this.proceduralImages[i];
			if (proceduralImage)
			{
				float num = Mathf.Clamp(this.scatter - this.minScatter, 0f, 10f) * 2f;
				proceduralImage.FalloffDistance = Mathf.Lerp(25f, 1f, this.canvasAlpha) + num;
				CanvasGroup canvasGroup = this.proceduralImageCanvasGroups[i];
				if (canvasGroup)
				{
					canvasGroup.alpha = Mathf.Clamp(1f - (num - 2f) / 15f, 0.3f, 1f);
				}
			}
		}
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001437C File Offset: 0x0001257C
	private void SetSniperRenderer()
	{
		if (this.sniperRoundRenderer)
		{
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(null, this.aimMarkerUI.position) / new Vector2((float)Screen.width, (float)Screen.height);
			this.sniperRoundRenderer.material.SetVector(this.sniperCenterShaderHash, vector);
		}
		if (this.followSniperRoundRenderer)
		{
			Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(null, this.followUI.position) / new Vector2((float)Screen.width, (float)Screen.height);
			this.followSniperRoundRenderer.material.SetVector(this.sniperCenterShaderHash, vector2);
		}
	}

	// Token: 0x040003AF RID: 943
	[HideInInspector]
	public ADSAimMarker selfPrefab;

	// Token: 0x040003B0 RID: 944
	public bool hideNormalCrosshair = true;

	// Token: 0x040003B1 RID: 945
	public AimMarker parentAimMarker;

	// Token: 0x040003B2 RID: 946
	public RectTransform aimMarkerUI;

	// Token: 0x040003B3 RID: 947
	public RectTransform followUI;

	// Token: 0x040003B4 RID: 948
	public CanvasGroup canvasGroup;

	// Token: 0x040003B5 RID: 949
	public float followSpeed;

	// Token: 0x040003B6 RID: 950
	public float followMaxDistance = 30f;

	// Token: 0x040003B7 RID: 951
	private float adsValue = -1f;

	// Token: 0x040003B8 RID: 952
	private float canvasAlpha;

	// Token: 0x040003B9 RID: 953
	public Vector2 adsAlphaRemap = new Vector2(0f, 1f);

	// Token: 0x040003BA RID: 954
	public List<ProceduralImage> proceduralImages;

	// Token: 0x040003BB RID: 955
	private List<CanvasGroup> proceduralImageCanvasGroups;

	// Token: 0x040003BC RID: 956
	public List<PunchReceiver> shootPunchReceivers;

	// Token: 0x040003BD RID: 957
	public List<SingleCrosshair> crosshairs;

	// Token: 0x040003BE RID: 958
	public Graphic sniperRoundRenderer;

	// Token: 0x040003BF RID: 959
	public Graphic followSniperRoundRenderer;

	// Token: 0x040003C0 RID: 960
	private float scatter;

	// Token: 0x040003C1 RID: 961
	private float minScatter;

	// Token: 0x040003C2 RID: 962
	private RectTransform selfRect;

	// Token: 0x040003C3 RID: 963
	private ItemAgent_Gun gun;

	// Token: 0x040003C4 RID: 964
	private Camera camera;

	// Token: 0x040003C5 RID: 965
	private int sniperCenterShaderHash = Shader.PropertyToID("_RoundCenter");
}
