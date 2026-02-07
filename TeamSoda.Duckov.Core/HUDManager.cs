using System;
using System.Collections.Generic;
using System.Linq;
using Dialogues;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class HUDManager : MonoBehaviour
{
	// Token: 0x14000073 RID: 115
	// (add) Token: 0x06000F5F RID: 3935 RVA: 0x0003DB8C File Offset: 0x0003BD8C
	// (remove) Token: 0x06000F60 RID: 3936 RVA: 0x0003DBC0 File Offset: 0x0003BDC0
	private static event Action onHideTokensChanged;

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0003DBF4 File Offset: 0x0003BDF4
	private bool ShouldDisplay
	{
		get
		{
			bool flag = HUDManager.hideTokens.Any((Object e) => e != null);
			bool flag2 = View.ActiveView != null;
			bool active = DialogueUI.Active;
			bool flag3 = CustomFaceUI.ActiveView != null;
			bool active2 = CameraMode.Active;
			return !flag && !flag2 && !active && !flag3 && !active2;
		}
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x0003DC60 File Offset: 0x0003BE60
	private void Awake()
	{
		View.OnActiveViewChanged += this.OnActiveViewChanged;
		DialogueUI.OnDialogueStatusChanged += this.OnDialogueStatusChanged;
		CustomFaceUI.OnCustomUIViewChanged += this.OnCustomFaceViewChange;
		CameraMode.OnCameraModeChanged = (Action<bool>)Delegate.Combine(CameraMode.OnCameraModeChanged, new Action<bool>(this.OnCameraModeChanged));
		HUDManager.onHideTokensChanged += this.OnHideTokensChanged;
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x0003DCD4 File Offset: 0x0003BED4
	private void OnDestroy()
	{
		View.OnActiveViewChanged -= this.OnActiveViewChanged;
		DialogueUI.OnDialogueStatusChanged -= this.OnDialogueStatusChanged;
		CustomFaceUI.OnCustomUIViewChanged -= this.OnCustomFaceViewChange;
		CameraMode.OnCameraModeChanged = (Action<bool>)Delegate.Remove(CameraMode.OnCameraModeChanged, new Action<bool>(this.OnCameraModeChanged));
		HUDManager.onHideTokensChanged -= this.OnHideTokensChanged;
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x0003DD45 File Offset: 0x0003BF45
	private void OnHideTokensChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x0003DD4D File Offset: 0x0003BF4D
	private void OnCameraModeChanged(bool value)
	{
		this.Refresh();
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x0003DD55 File Offset: 0x0003BF55
	private void OnDialogueStatusChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x0003DD5D File Offset: 0x0003BF5D
	private void OnActiveViewChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x0003DD65 File Offset: 0x0003BF65
	private void OnCustomFaceViewChange()
	{
		this.Refresh();
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x0003DD70 File Offset: 0x0003BF70
	private void Refresh()
	{
		if (this.ShouldDisplay)
		{
			this.canvasGroup.blocksRaycasts = true;
			if (this.fadeGroup.IsShown)
			{
				return;
			}
			this.fadeGroup.Show();
			return;
		}
		else
		{
			this.canvasGroup.blocksRaycasts = false;
			if (this.fadeGroup.IsHidden)
			{
				return;
			}
			this.fadeGroup.Hide();
			return;
		}
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x0003DDD0 File Offset: 0x0003BFD0
	public static void RegisterHideToken(Object obj)
	{
		HUDManager.hideTokens.Add(obj);
		Action action = HUDManager.onHideTokensChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0003DDEC File Offset: 0x0003BFEC
	public static void UnregisterHideToken(Object obj)
	{
		HUDManager.hideTokens.Remove(obj);
		Action action = HUDManager.onHideTokensChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x04000CBE RID: 3262
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000CBF RID: 3263
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04000CC0 RID: 3264
	private static List<Object> hideTokens = new List<Object>();
}
