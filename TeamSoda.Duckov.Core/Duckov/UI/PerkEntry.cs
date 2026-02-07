using System;
using Duckov.PerkTrees;
using Duckov.Utilities;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003D7 RID: 983
	public class PerkEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPoolable
	{
		// Token: 0x060023C5 RID: 9157 RVA: 0x0007D973 File Offset: 0x0007BB73
		private void SwitchToActiveLook()
		{
			this.ApplyLook(this.activeLook);
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x0007D981 File Offset: 0x0007BB81
		private void SwitchToAvaliableLook()
		{
			this.ApplyLook(this.avaliableLook);
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x0007D98F File Offset: 0x0007BB8F
		private void SwitchToUnavaliableLook()
		{
			this.ApplyLook(this.unavaliableLook);
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x0007D99D File Offset: 0x0007BB9D
		public RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060023C9 RID: 9161 RVA: 0x0007D9BF File Offset: 0x0007BBBF
		public Perk Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x0007D9C8 File Offset: 0x0007BBC8
		public void Setup(PerkTreeView master, Perk target)
		{
			this.UnregisterEvents();
			this.master = master;
			this.target = target;
			this.icon.sprite = target.Icon;
			ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(target.DisplayQuality);
			this.iconShadow.IgnoreCasterColor = true;
			this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
			this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
			this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
			this.displayNameText.text = target.DisplayName;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0007DA68 File Offset: 0x0007BC68
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			bool unlocked = this.target.Unlocked;
			bool flag = this.target.AreAllParentsUnlocked();
			if (unlocked)
			{
				this.SwitchToActiveLook();
			}
			else if (flag)
			{
				this.SwitchToAvaliableLook();
			}
			else
			{
				this.SwitchToUnavaliableLook();
			}
			bool unlocking = this.target.Unlocking;
			bool flag2 = this.target.GetRemainingTime() <= TimeSpan.Zero;
			this.avaliableForResearchIndicator.SetActive(!unlocked && !unlocking && this.target.AreAllParentsUnlocked() && this.target.Requirement.AreSatisfied());
			this.inProgressIndicator.SetActive(!unlocked && unlocking && !flag2);
			this.timeUpIndicator.SetActive(!unlocked && unlocking && flag2);
			if (this.master == null)
			{
				return;
			}
			this.selectionIndicator.SetActive(this.master.GetSelection() == this);
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x0007DB63 File Offset: 0x0007BD63
		private void OnMasterSelectionChanged(PerkEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0007DB6C File Offset: 0x0007BD6C
		private void RegisterEvents()
		{
			if (this.master)
			{
				this.master.onSelectionChanged += this.OnMasterSelectionChanged;
			}
			if (this.target)
			{
				this.target.onUnlockStateChanged += this.OnTargetStateChanged;
			}
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0007DBC1 File Offset: 0x0007BDC1
		private void OnTargetStateChanged(Perk perk, bool state)
		{
			PunchReceiver punchReceiver = this.punchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			this.Refresh();
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x0007DBDC File Offset: 0x0007BDDC
		private void UnregisterEvents()
		{
			if (this.master)
			{
				this.master.onSelectionChanged -= this.OnMasterSelectionChanged;
			}
			if (this.target)
			{
				this.target.onUnlockStateChanged -= this.OnTargetStateChanged;
			}
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x0007DC31 File Offset: 0x0007BE31
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0007DC39 File Offset: 0x0007BE39
		public void NotifyPooled()
		{
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x0007DC3B File Offset: 0x0007BE3B
		public void NotifyReleased()
		{
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x0007DC3D File Offset: 0x0007BE3D
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.master == null)
			{
				return;
			}
			PunchReceiver punchReceiver = this.punchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			this.master.SetSelection(this);
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x0007DC6C File Offset: 0x0007BE6C
		internal Vector2 GetLayoutPosition()
		{
			if (this.target == null)
			{
				return Vector2.zero;
			}
			return this.target.GetLayoutPosition();
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x0007DC90 File Offset: 0x0007BE90
		private void ApplyLook(PerkEntry.Look look)
		{
			this.icon.material = look.material;
			this.icon.color = look.iconColor;
			this.frame.color = look.frameColor;
			this.frameGlow.enabled = (look.frameGlowColor.a > 0f);
			this.frameGlow.Color = look.frameGlowColor;
			this.background.color = look.backgroundColor;
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0007DD0F File Offset: 0x0007BF0F
		private void FixedUpdate()
		{
			if (this.inProgressIndicator.activeSelf && this.target.GetRemainingTime() <= TimeSpan.Zero)
			{
				this.Refresh();
			}
		}

		// Token: 0x0400185A RID: 6234
		[SerializeField]
		private Image icon;

		// Token: 0x0400185B RID: 6235
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x0400185C RID: 6236
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x0400185D RID: 6237
		[SerializeField]
		private Image frame;

		// Token: 0x0400185E RID: 6238
		[SerializeField]
		private TrueShadow frameGlow;

		// Token: 0x0400185F RID: 6239
		[SerializeField]
		private Image background;

		// Token: 0x04001860 RID: 6240
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001861 RID: 6241
		[SerializeField]
		private PunchReceiver punchReceiver;

		// Token: 0x04001862 RID: 6242
		[SerializeField]
		private GameObject inProgressIndicator;

		// Token: 0x04001863 RID: 6243
		[SerializeField]
		private GameObject timeUpIndicator;

		// Token: 0x04001864 RID: 6244
		[SerializeField]
		private GameObject avaliableForResearchIndicator;

		// Token: 0x04001865 RID: 6245
		[SerializeField]
		private PerkEntry.Look activeLook;

		// Token: 0x04001866 RID: 6246
		[SerializeField]
		private PerkEntry.Look avaliableLook;

		// Token: 0x04001867 RID: 6247
		[SerializeField]
		private PerkEntry.Look unavaliableLook;

		// Token: 0x04001868 RID: 6248
		private RectTransform _rectTransform;

		// Token: 0x04001869 RID: 6249
		private PerkTreeView master;

		// Token: 0x0400186A RID: 6250
		private Perk target;

		// Token: 0x02000654 RID: 1620
		[Serializable]
		public struct Look
		{
			// Token: 0x040022F0 RID: 8944
			public Color iconColor;

			// Token: 0x040022F1 RID: 8945
			public Material material;

			// Token: 0x040022F2 RID: 8946
			public Color frameColor;

			// Token: 0x040022F3 RID: 8947
			public Color frameGlowColor;

			// Token: 0x040022F4 RID: 8948
			public Color backgroundColor;
		}
	}
}
