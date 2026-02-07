using System;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003DB RID: 987
	public class PlayerStatsView : View
	{
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060023FB RID: 9211 RVA: 0x0007E9B3 File Offset: 0x0007CBB3
		public static PlayerStatsView Instance
		{
			get
			{
				return View.GetViewInstance<PlayerStatsView>();
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x0007E9BA File Offset: 0x0007CBBA
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x0007E9C2 File Offset: 0x0007CBC2
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0007E9D5 File Offset: 0x0007CBD5
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x0007E9E8 File Offset: 0x0007CBE8
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0007E9F0 File Offset: 0x0007CBF0
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x0007E9F8 File Offset: 0x0007CBF8
		private void RegisterEvents()
		{
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x0007E9FA File Offset: 0x0007CBFA
		private void UnregisterEvents()
		{
		}

		// Token: 0x04001881 RID: 6273
		[SerializeField]
		private FadeGroup fadeGroup;
	}
}
