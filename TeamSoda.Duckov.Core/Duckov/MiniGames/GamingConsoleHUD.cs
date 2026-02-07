using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000290 RID: 656
	public class GamingConsoleHUD : View
	{
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x0004F169 File Offset: 0x0004D369
		private static GamingConsoleHUD Instance
		{
			get
			{
				if (GamingConsoleHUD._instance_cache == null)
				{
					GamingConsoleHUD._instance_cache = View.GetViewInstance<GamingConsoleHUD>();
				}
				return GamingConsoleHUD._instance_cache;
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0004F187 File Offset: 0x0004D387
		public static void Show()
		{
			if (GamingConsoleHUD.Instance == null)
			{
				return;
			}
			GamingConsoleHUD.Instance.LocalShow();
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0004F1A1 File Offset: 0x0004D3A1
		public static void Hide()
		{
			if (GamingConsoleHUD.Instance == null)
			{
				return;
			}
			GamingConsoleHUD.Instance.LocalHide();
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0004F1BB File Offset: 0x0004D3BB
		private void LocalShow()
		{
			this.contentFadeGroup.Show();
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0004F1C8 File Offset: 0x0004D3C8
		private void LocalHide()
		{
			this.contentFadeGroup.Hide();
		}

		// Token: 0x04000F92 RID: 3986
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000F93 RID: 3987
		private static GamingConsoleHUD _instance_cache;
	}
}
