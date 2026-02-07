using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class ATMView : View
{
	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00035DFA File Offset: 0x00033FFA
	public static ATMView Instance
	{
		get
		{
			return View.GetViewInstance<ATMView>();
		}
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00035E01 File Offset: 0x00034001
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x00035E0C File Offset: 0x0003400C
	public static void Show()
	{
		ATMView instance = ATMView.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Open(null);
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x00035E30 File Offset: 0x00034030
	protected override void OnOpen()
	{
		base.OnOpen();
		this.fadeGroup.Show();
		this.atmPanel.ShowSelectPanel(true);
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x00035E4F File Offset: 0x0003404F
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x04000B06 RID: 2822
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000B07 RID: 2823
	[SerializeField]
	private ATMPanel atmPanel;
}
