using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Options;
using Sirenix.Utilities;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class ResolutionSetter : MonoBehaviour
{
	// Token: 0x06000E17 RID: 3607 RVA: 0x0003A524 File Offset: 0x00038724
	private void Test()
	{
		this.debugDisplayRes = new Vector2Int(Display.main.systemWidth, Display.main.systemHeight);
		this.debugmMaxRes = new Vector2Int(ResolutionSetter.MaxResolution.width, ResolutionSetter.MaxResolution.height);
		this.debugScreenRes = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
		this.testRes = ResolutionSetter.GetResolutions();
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0003A5A0 File Offset: 0x000387A0
	public static DuckovResolution MaxResolution
	{
		get
		{
			Resolution[] resolutions = Screen.resolutions;
			LinqExtensions.Sort<Resolution>(resolutions, delegate(Resolution A, Resolution B)
			{
				if (A.height > B.height)
				{
					return -1;
				}
				if (A.height < B.height)
				{
					return 1;
				}
				if (A.width > B.width)
				{
					return -1;
				}
				if (A.width < B.width)
				{
					return 1;
				}
				return 0;
			});
			Resolution res = default(Resolution);
			res.width = Screen.currentResolution.width;
			res.height = Screen.currentResolution.height;
			Resolution res2 = Screen.resolutions[resolutions.Length - 1];
			DuckovResolution duckovResolution;
			if (res.width > res2.width)
			{
				duckovResolution = new DuckovResolution(res);
			}
			else
			{
				duckovResolution = new DuckovResolution(res2);
			}
			if ((float)duckovResolution.width / (float)duckovResolution.height < 1.4f)
			{
				duckovResolution.width = Mathf.RoundToInt((float)(duckovResolution.height * 16 / 9));
			}
			return duckovResolution;
		}
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x0003A66C File Offset: 0x0003886C
	public static Resolution GetResByHeight(int height, DuckovResolution maxRes)
	{
		Resolution result = default(Resolution);
		result.height = height;
		result.width = (int)((float)maxRes.width * (float)height / (float)maxRes.height);
		return result;
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x0003A6A4 File Offset: 0x000388A4
	public static DuckovResolution[] GetResolutions()
	{
		DuckovResolution maxResolution = ResolutionSetter.MaxResolution;
		List<Resolution> list = Screen.resolutions.ToList<Resolution>();
		list.Add(ResolutionSetter.GetResByHeight(1080, maxResolution));
		list.Add(ResolutionSetter.GetResByHeight(900, maxResolution));
		list.Add(ResolutionSetter.GetResByHeight(720, maxResolution));
		list.Add(ResolutionSetter.GetResByHeight(540, maxResolution));
		List<DuckovResolution> list2 = new List<DuckovResolution>();
		bool flag = OptionsManager.Load<ResolutionSetter.screenModes>(ResolutionSetter.Key_ScreenMode, ResolutionSetter.screenModes.Window) != ResolutionSetter.screenModes.Window;
		foreach (Resolution res in list)
		{
			DuckovResolution duckovResolution = new DuckovResolution(res);
			if (!list2.Contains(duckovResolution) && (float)duckovResolution.width / (float)duckovResolution.height >= 1.4f && (!flag || duckovResolution.CheckRotioFit(duckovResolution, maxResolution)))
			{
				list2.Add(duckovResolution);
			}
		}
		list2.Sort(delegate(DuckovResolution A, DuckovResolution B)
		{
			if (A.height > B.height)
			{
				return -1;
			}
			if (A.height < B.height)
			{
				return 1;
			}
			if (A.width > B.width)
			{
				return -1;
			}
			if (A.width < B.width)
			{
				return 1;
			}
			return 0;
		});
		return list2.ToArray();
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x0003A7C4 File Offset: 0x000389C4
	private void Update()
	{
		this.UpdateFullScreenCheck();
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x0003A7CC File Offset: 0x000389CC
	private void UpdateFullScreenCheck()
	{
		ResolutionSetter.fullScreenChangeCheckCoolTimer -= Time.unscaledDeltaTime;
		if (ResolutionSetter.fullScreenChangeCheckCoolTimer > 0f)
		{
			return;
		}
		if (ResolutionSetter.currentFullScreen != (Screen.fullScreenMode == 1 || Screen.fullScreenMode == 0))
		{
			ResolutionSetter.currentFullScreen = !ResolutionSetter.currentFullScreen;
			OptionsManager.Save<ResolutionSetter.screenModes>(ResolutionSetter.Key_ScreenMode, ResolutionSetter.currentFullScreen ? ResolutionSetter.screenModes.Borderless : ResolutionSetter.screenModes.Window);
			ResolutionSetter.fullScreenChangeCheckCoolTimer = ResolutionSetter.fullScreenChangeCheckCoolTime;
		}
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x0003A83C File Offset: 0x00038A3C
	public static void UpdateResolutionAndScreenMode()
	{
		ResolutionSetter.fullScreenChangeCheckCoolTimer = ResolutionSetter.fullScreenChangeCheckCoolTime;
		DuckovResolution duckovResolution = OptionsManager.Load<DuckovResolution>(ResolutionSetter.Key_Resolution, new DuckovResolution(Screen.resolutions[Screen.resolutions.Length - 1]));
		if ((float)duckovResolution.width / (float)duckovResolution.height < 1.3666667f)
		{
			duckovResolution.width = Mathf.RoundToInt((float)(duckovResolution.height * 16 / 9));
		}
		ResolutionSetter.screenModes screenModes = OptionsManager.Load<ResolutionSetter.screenModes>(ResolutionSetter.Key_ScreenMode, ResolutionSetter.screenModes.Borderless);
		ResolutionSetter.currentFullScreen = (screenModes == ResolutionSetter.screenModes.Borderless);
		Screen.SetResolution(duckovResolution.width, duckovResolution.height, ResolutionSetter.ScreenModeToFullScreenMode(screenModes));
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x0003A8D1 File Offset: 0x00038AD1
	private static FullScreenMode ScreenModeToFullScreenMode(ResolutionSetter.screenModes screenMode)
	{
		if (screenMode == ResolutionSetter.screenModes.Borderless)
		{
			return 1;
		}
		if (screenMode != ResolutionSetter.screenModes.Window)
		{
			return 0;
		}
		return 3;
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0003A8E4 File Offset: 0x00038AE4
	public static string[] GetScreenModes()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText("Option_ScreenMode_" + ResolutionSetter.screenModes.Borderless.ToString()),
			LocalizationManager.ToPlainText("Option_ScreenMode_" + ResolutionSetter.screenModes.Window.ToString())
		};
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x0003A939 File Offset: 0x00038B39
	public static string ScreenModeToName(ResolutionSetter.screenModes mode)
	{
		return LocalizationManager.ToPlainText("Option_ScreenMode_" + mode.ToString());
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x0003A957 File Offset: 0x00038B57
	private void Awake()
	{
		ResolutionSetter.UpdateResolutionAndScreenMode();
		OptionsManager.OnOptionsChanged += this.OnOptionsChanged;
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x0003A96F File Offset: 0x00038B6F
	private void OnDestroy()
	{
		OptionsManager.OnOptionsChanged -= this.OnOptionsChanged;
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x0003A982 File Offset: 0x00038B82
	private void OnOptionsChanged(string key)
	{
		if (key == ResolutionSetter.Key_Resolution || key == ResolutionSetter.Key_ScreenMode)
		{
			ResolutionSetter.UpdateResolutionAndScreenMode();
		}
	}

	// Token: 0x04000C01 RID: 3073
	public static string Key_Resolution = "Resolution";

	// Token: 0x04000C02 RID: 3074
	public static string Key_ScreenMode = "ScreenMode";

	// Token: 0x04000C03 RID: 3075
	public static bool currentFullScreen = false;

	// Token: 0x04000C04 RID: 3076
	private static float fullScreenChangeCheckCoolTimer = 1f;

	// Token: 0x04000C05 RID: 3077
	private static float fullScreenChangeCheckCoolTime = 1f;

	// Token: 0x04000C06 RID: 3078
	public Vector2Int debugDisplayRes = new Vector2Int(0, 0);

	// Token: 0x04000C07 RID: 3079
	public Vector2Int debugScreenRes = new Vector2Int(0, 0);

	// Token: 0x04000C08 RID: 3080
	public Vector2Int debugmMaxRes = new Vector2Int(0, 0);

	// Token: 0x04000C09 RID: 3081
	public DuckovResolution[] testRes;

	// Token: 0x020004F8 RID: 1272
	public enum screenModes
	{
		// Token: 0x04001DE7 RID: 7655
		Borderless,
		// Token: 0x04001DE8 RID: 7656
		Window
	}
}
