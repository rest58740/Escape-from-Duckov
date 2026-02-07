using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Modding;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001CD RID: 461
public class ModUploadPanel : MonoBehaviour
{
	// Token: 0x06000DEF RID: 3567 RVA: 0x0003A043 File Offset: 0x00038243
	private void Awake()
	{
		this.btnCancel.onClick.AddListener(new UnityAction(this.OnCancelBtnClick));
		this.btnUpload.onClick.AddListener(new UnityAction(this.OnUploadBtnClick));
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x0003A07D File Offset: 0x0003827D
	private void OnUploadBtnClick()
	{
		this.uploadClicked = true;
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0003A086 File Offset: 0x00038286
	private void OnCancelBtnClick()
	{
		this.cancelClicked = true;
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x0003A090 File Offset: 0x00038290
	public UniTask Execute(ModInfo info)
	{
		ModUploadPanel.<Execute>d__29 <Execute>d__;
		<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Execute>d__.<>4__this = this;
		<Execute>d__.info = info;
		<Execute>d__.<>1__state = -1;
		<Execute>d__.<>t__builder.Start<ModUploadPanel.<Execute>d__29>(ref <Execute>d__);
		return <Execute>d__.<>t__builder.Task;
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x0003A0DC File Offset: 0x000382DC
	private void Update()
	{
		if (this.waitingForUpload)
		{
			this.progressBarFill.fillAmount = SteamWorkshopManager.UploadingProgress;
			ulong punBytesProcess = SteamWorkshopManager.punBytesProcess;
			ulong punBytesTotal = SteamWorkshopManager.punBytesTotal;
			this.progressText.text = ModUploadPanel.FormatBytes(punBytesProcess) + " / " + ModUploadPanel.FormatBytes(punBytesTotal);
		}
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x0003A130 File Offset: 0x00038330
	private static string FormatBytes(ulong bytes)
	{
		if (bytes < 1024UL)
		{
			return string.Format("{0}bytes", bytes);
		}
		if (bytes < 1048576UL)
		{
			return string.Format("{0:0.0}KB", bytes / 1024f);
		}
		if (bytes < 1073741824UL)
		{
			return string.Format("{0:0.0}MB", bytes / 1048576f);
		}
		return string.Format("{0:0.0}GB", bytes / 1.0737418E+09f);
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x0003A1B4 File Offset: 0x000383B4
	private void Clean()
	{
		this.fgLoading.SkipHide();
		this.fgContent.SkipHide();
		this.indicatorNew.SetActive(false);
		this.indicatorUpdate.SetActive(false);
		this.indicatorOwnershipWarning.SetActive(false);
		this.indicatorInvalidContent.SetActive(false);
		this.txtPublishedFileID.text = "-";
		this.txtPath.text = "-";
		this.fgButtonMain.SkipHide();
		this.fgProgressBar.SkipHide();
		this.fgSucceed.SkipHide();
		this.fgFailed.SkipHide();
		this.waitingForUpload = false;
	}

	// Token: 0x04000BE5 RID: 3045
	[SerializeField]
	private FadeGroup fgMain;

	// Token: 0x04000BE6 RID: 3046
	[SerializeField]
	private FadeGroup fgLoading;

	// Token: 0x04000BE7 RID: 3047
	[SerializeField]
	private FadeGroup fgContent;

	// Token: 0x04000BE8 RID: 3048
	[SerializeField]
	private TextMeshProUGUI txtTitle;

	// Token: 0x04000BE9 RID: 3049
	[SerializeField]
	private TextMeshProUGUI txtDescription;

	// Token: 0x04000BEA RID: 3050
	[SerializeField]
	private RawImage preview;

	// Token: 0x04000BEB RID: 3051
	[SerializeField]
	private TextMeshProUGUI txtModName;

	// Token: 0x04000BEC RID: 3052
	[SerializeField]
	private TextMeshProUGUI txtPath;

	// Token: 0x04000BED RID: 3053
	[SerializeField]
	private TextMeshProUGUI txtPublishedFileID;

	// Token: 0x04000BEE RID: 3054
	[SerializeField]
	private GameObject indicatorNew;

	// Token: 0x04000BEF RID: 3055
	[SerializeField]
	private GameObject indicatorUpdate;

	// Token: 0x04000BF0 RID: 3056
	[SerializeField]
	private GameObject indicatorOwnershipWarning;

	// Token: 0x04000BF1 RID: 3057
	[SerializeField]
	private GameObject indicatorInvalidContent;

	// Token: 0x04000BF2 RID: 3058
	[SerializeField]
	private Button btnUpload;

	// Token: 0x04000BF3 RID: 3059
	[SerializeField]
	private Button btnCancel;

	// Token: 0x04000BF4 RID: 3060
	[SerializeField]
	private FadeGroup fgButtonMain;

	// Token: 0x04000BF5 RID: 3061
	[SerializeField]
	private FadeGroup fgProgressBar;

	// Token: 0x04000BF6 RID: 3062
	[SerializeField]
	private TextMeshProUGUI progressText;

	// Token: 0x04000BF7 RID: 3063
	[SerializeField]
	private Image progressBarFill;

	// Token: 0x04000BF8 RID: 3064
	[SerializeField]
	private FadeGroup fgSucceed;

	// Token: 0x04000BF9 RID: 3065
	[SerializeField]
	private FadeGroup fgFailed;

	// Token: 0x04000BFA RID: 3066
	[SerializeField]
	private float closeAfterSeconds = 2f;

	// Token: 0x04000BFB RID: 3067
	[SerializeField]
	private Texture2D defaultPreviewTexture;

	// Token: 0x04000BFC RID: 3068
	private bool cancelClicked;

	// Token: 0x04000BFD RID: 3069
	private bool uploadClicked;

	// Token: 0x04000BFE RID: 3070
	private bool waitingForUpload;
}
