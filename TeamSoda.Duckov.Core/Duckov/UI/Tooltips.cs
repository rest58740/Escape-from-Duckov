using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x0200039A RID: 922
	public class Tooltips : MonoBehaviour
	{
		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x00071016 File Offset: 0x0006F216
		// (set) Token: 0x06002014 RID: 8212 RVA: 0x0007101D File Offset: 0x0006F21D
		public static ITooltipsProvider CurrentProvider { get; private set; }

		// Token: 0x06002015 RID: 8213 RVA: 0x00071025 File Offset: 0x0006F225
		public static void NotifyEnterTooltipsProvider(ITooltipsProvider provider)
		{
			Tooltips.CurrentProvider = provider;
			Action<ITooltipsProvider> onEnterProvider = Tooltips.OnEnterProvider;
			if (onEnterProvider == null)
			{
				return;
			}
			onEnterProvider(provider);
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x0007103D File Offset: 0x0006F23D
		public static void NotifyExitTooltipsProvider(ITooltipsProvider provider)
		{
			if (Tooltips.CurrentProvider != provider)
			{
				return;
			}
			Tooltips.CurrentProvider = null;
			Action<ITooltipsProvider> onExitProvider = Tooltips.OnExitProvider;
			if (onExitProvider == null)
			{
				return;
			}
			onExitProvider(provider);
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x00071060 File Offset: 0x0006F260
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			Tooltips.OnEnterProvider = (Action<ITooltipsProvider>)Delegate.Combine(Tooltips.OnEnterProvider, new Action<ITooltipsProvider>(this.DoOnEnterProvider));
			Tooltips.OnExitProvider = (Action<ITooltipsProvider>)Delegate.Combine(Tooltips.OnExitProvider, new Action<ITooltipsProvider>(this.DoOnExitProvider));
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x000710C8 File Offset: 0x0006F2C8
		private void OnDestroy()
		{
			Tooltips.OnEnterProvider = (Action<ITooltipsProvider>)Delegate.Remove(Tooltips.OnEnterProvider, new Action<ITooltipsProvider>(this.DoOnEnterProvider));
			Tooltips.OnExitProvider = (Action<ITooltipsProvider>)Delegate.Remove(Tooltips.OnExitProvider, new Action<ITooltipsProvider>(this.DoOnExitProvider));
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x00071115 File Offset: 0x0006F315
		private void Update()
		{
			if (this.contents.gameObject.activeSelf)
			{
				this.RefreshPosition();
			}
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0007112F File Offset: 0x0006F32F
		private void DoOnExitProvider(ITooltipsProvider provider)
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0007113C File Offset: 0x0006F33C
		private void DoOnEnterProvider(ITooltipsProvider provider)
		{
			this.text.text = provider.GetTooltipsText();
			this.fadeGroup.Show();
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x0007115C File Offset: 0x0006F35C
		private unsafe void RefreshPosition()
		{
			Vector2 vector = *Mouse.current.position.value;
			Vector2 vector2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, vector, null, ref vector2);
			this.contents.localPosition = vector2;
		}

		// Token: 0x040015F4 RID: 5620
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x040015F5 RID: 5621
		[SerializeField]
		private RectTransform contents;

		// Token: 0x040015F6 RID: 5622
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040015F7 RID: 5623
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040015F9 RID: 5625
		private static Action<ITooltipsProvider> OnEnterProvider;

		// Token: 0x040015FA RID: 5626
		private static Action<ITooltipsProvider> OnExitProvider;
	}
}
