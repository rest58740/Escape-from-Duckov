using System;
using System.Collections.Generic;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000076 RID: 118
public class AimMarker : MonoBehaviour
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000466 RID: 1126 RVA: 0x00014484 File Offset: 0x00012684
	private Camera MainCam
	{
		get
		{
			if (!this._cam)
			{
				if (LevelManager.Instance == null)
				{
					return null;
				}
				if (LevelManager.Instance.GameCamera == null)
				{
					return null;
				}
				this._cam = LevelManager.Instance.GameCamera.renderCamera;
			}
			return this._cam;
		}
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x000144DC File Offset: 0x000126DC
	private void Awake()
	{
		if (!this.currentAdsAimMarker)
		{
			this.SwitchAdsAimMarker(this.defaultAdsAimMarker);
		}
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x000144F7 File Offset: 0x000126F7
	private void Start()
	{
		this.rootCanvasGroup.alpha = 1f;
		ItemAgent_Gun.OnMainCharacterShootEvent += this.OnMainCharacterShoot;
		Health.OnDead += this.OnKill;
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0001452B File Offset: 0x0001272B
	private void OnDestroy()
	{
		ItemAgent_Gun.OnMainCharacterShootEvent -= this.OnMainCharacterShoot;
		Health.OnDead -= this.OnKill;
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00014550 File Offset: 0x00012750
	private void Update()
	{
		this.aimMarkerAnimator.SetBool(this.inProgressHash, this.reloadProgressBar.InProgress);
		if (this.killMarkerTimer > 0f)
		{
			this.killMarkerTimer -= Time.deltaTime;
			this.aimMarkerAnimator.SetBool(this.killMarkerHash, this.killMarkerTimer > 0f);
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		if (main.Health.IsDead)
		{
			this.rootCanvasGroup.alpha = 0f;
			return;
		}
		InputManager inputManager = LevelManager.Instance.InputManager;
		if (inputManager == null)
		{
			return;
		}
		Vector3 inputAimPoint = inputManager.InputAimPoint;
		Vector3 aimMarkerPosScreenSpace = this.MainCam.WorldToScreenPoint(inputAimPoint);
		aimMarkerPosScreenSpace = inputManager.AimScreenPoint;
		this.SetAimMarkerPosScreenSpace(aimMarkerPosScreenSpace);
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x00014620 File Offset: 0x00012820
	private void LateUpdate()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		InputManager inputManager = LevelManager.Instance.InputManager;
		if (inputManager == null)
		{
			return;
		}
		Vector3 inputAimPoint = inputManager.InputAimPoint;
		ItemAgent_Gun gun = main.GetGun();
		Color color = this.distanceTextColorFull;
		float num;
		if (gun != null)
		{
			if (this.adsValue == 0f && gun.AdsValue > 0f)
			{
				this.OnStartAdsWithGun(gun);
			}
			this.adsValue = gun.AdsValue;
			this.scatter = Mathf.MoveTowards(this.scatter, gun.CurrentScatter, 500f * Time.deltaTime);
			this.minScatter = Mathf.MoveTowards(this.minScatter, gun.MinScatter, 500f * Time.deltaTime);
			this.left.anchoredPosition = Vector3.left * (20f + this.scatter * 5f);
			this.right.anchoredPosition = Vector3.right * (20f + this.scatter * 5f);
			this.up.anchoredPosition = Vector3.up * (20f + this.scatter * 5f);
			this.down.anchoredPosition = Vector3.down * (20f + this.scatter * 5f);
			num = Vector3.Distance(inputAimPoint, gun.muzzle.position);
			float bulletDistance = gun.BulletDistance;
			if (num < bulletDistance * 0.495f)
			{
				color = this.distanceTextColorFull;
			}
			else if (num < bulletDistance)
			{
				color = this.distanceTextColorHalf;
			}
			else
			{
				color = this.distanceTextColorOver;
			}
		}
		else
		{
			this.adsValue = 0f;
			this.scatter = 0f;
			this.minScatter = 0f;
			num = Vector3.Distance(inputAimPoint, main.transform.position + Vector3.up * 0.5f);
			color = this.distanceTextColorFull;
		}
		float alpha = Mathf.Clamp01((0.5f - this.adsValue) * 2f);
		if (this.currentAdsAimMarker)
		{
			this.currentAdsAimMarker.SetScatter(this.scatter, this.minScatter);
			this.currentAdsAimMarker.SetAdsValue(this.adsValue);
			if (!this.currentAdsAimMarker.hideNormalCrosshair)
			{
				alpha = 1f;
			}
		}
		else
		{
			alpha = 1f;
		}
		this.normalAimCanvasGroup.alpha = alpha;
		if (this.distanceText)
		{
			this.distanceText.text = num.ToString("00") + " M";
			this.distanceText.color = color;
			this.distanceGlow.Color = color;
		}
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x000148FB File Offset: 0x00012AFB
	public void SetAimMarkerPosScreenSpace(Vector3 pos)
	{
		this.aimMarkerUI.position = pos;
		if (this.currentAdsAimMarker)
		{
			this.currentAdsAimMarker.SetAimMarkerPos(pos);
		}
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00014924 File Offset: 0x00012B24
	private void OnStartAdsWithGun(ItemAgent_Gun gun)
	{
		ADSAimMarker aimMarkerPfb = gun.GetAimMarkerPfb();
		if (!aimMarkerPfb)
		{
			return;
		}
		this.SwitchAdsAimMarker(aimMarkerPfb);
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00014948 File Offset: 0x00012B48
	private void SwitchAdsAimMarker(ADSAimMarker newAimMarkerPfb)
	{
		if (newAimMarkerPfb == null)
		{
			Object.Destroy(this.currentAdsAimMarker.gameObject);
			this.currentAdsAimMarker = null;
			return;
		}
		if (this.currentAdsAimMarker && newAimMarkerPfb == this.currentAdsAimMarker.selfPrefab)
		{
			return;
		}
		if (this.currentAdsAimMarker)
		{
			Object.Destroy(this.currentAdsAimMarker.gameObject);
		}
		this.currentAdsAimMarker = Object.Instantiate<ADSAimMarker>(newAimMarkerPfb);
		this.currentAdsAimMarker.selfPrefab = newAimMarkerPfb;
		this.currentAdsAimMarker.transform.SetParent(base.transform);
		this.currentAdsAimMarker.parentAimMarker = this;
		RectTransform rectTransform = this.currentAdsAimMarker.transform as RectTransform;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.sizeDelta = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.offsetMin = Vector2.zero;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x00014A34 File Offset: 0x00012C34
	private void SetAimMarkerColor(Color col)
	{
		int count = this.aimMarkerImages.Count;
		for (int i = 0; i < count; i++)
		{
			this.aimMarkerImages[i].color = col;
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00014A6B File Offset: 0x00012C6B
	private void OnKill(Health _health, DamageInfo dmgInfo)
	{
		if (_health == null || _health.team == Teams.player)
		{
			return;
		}
		this.killMarkerTimer = this.killMarkerTime;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00014A8B File Offset: 0x00012C8B
	private void OnMainCharacterShoot(ItemAgent_Gun gunAgnet)
	{
		UnityEvent unityEvent = this.onShoot;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		if (this.currentAdsAimMarker)
		{
			this.currentAdsAimMarker.OnShoot();
		}
	}

	// Token: 0x040003C6 RID: 966
	public RectTransform aimMarkerUI;

	// Token: 0x040003C7 RID: 967
	public List<Image> aimMarkerImages;

	// Token: 0x040003C8 RID: 968
	public RectTransform left;

	// Token: 0x040003C9 RID: 969
	public RectTransform right;

	// Token: 0x040003CA RID: 970
	public RectTransform up;

	// Token: 0x040003CB RID: 971
	public RectTransform down;

	// Token: 0x040003CC RID: 972
	private float scatter;

	// Token: 0x040003CD RID: 973
	private float minScatter;

	// Token: 0x040003CE RID: 974
	public CanvasGroup rootCanvasGroup;

	// Token: 0x040003CF RID: 975
	public CanvasGroup normalAimCanvasGroup;

	// Token: 0x040003D0 RID: 976
	public Animator aimMarkerAnimator;

	// Token: 0x040003D1 RID: 977
	public ActionProgressHUD reloadProgressBar;

	// Token: 0x040003D2 RID: 978
	public UnityEvent onShoot;

	// Token: 0x040003D3 RID: 979
	private ADSAimMarker currentAdsAimMarker;

	// Token: 0x040003D4 RID: 980
	[SerializeField]
	private ADSAimMarker defaultAdsAimMarker;

	// Token: 0x040003D5 RID: 981
	private readonly int inProgressHash = Animator.StringToHash("InProgress");

	// Token: 0x040003D6 RID: 982
	private readonly int killMarkerHash = Animator.StringToHash("KillMarkerShow");

	// Token: 0x040003D7 RID: 983
	[SerializeField]
	private TextMeshProUGUI distanceText;

	// Token: 0x040003D8 RID: 984
	[SerializeField]
	private TrueShadow distanceGlow;

	// Token: 0x040003D9 RID: 985
	[SerializeField]
	private Color distanceTextColorFull;

	// Token: 0x040003DA RID: 986
	[SerializeField]
	private Color distanceTextColorHalf;

	// Token: 0x040003DB RID: 987
	[SerializeField]
	private Color distanceTextColorOver;

	// Token: 0x040003DC RID: 988
	private float adsValue;

	// Token: 0x040003DD RID: 989
	private float killMarkerTime = 0.6f;

	// Token: 0x040003DE RID: 990
	private float killMarkerTimer;

	// Token: 0x040003DF RID: 991
	private Camera _cam;
}
