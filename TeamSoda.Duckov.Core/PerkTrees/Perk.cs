using System;
using System.Text;
using ItemStatsSystem;
using Sirenix.OdinInspector;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000257 RID: 599
	public class Perk : MonoBehaviour, ISelfValidator
	{
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x000484DB File Offset: 0x000466DB
		public bool LockInDemo
		{
			get
			{
				return this.lockInDemo;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x000484E3 File Offset: 0x000466E3
		public DisplayQuality DisplayQuality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x000484EB File Offset: 0x000466EB
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x000484F5 File Offset: 0x000466F5
		// (set) Token: 0x060012DD RID: 4829 RVA: 0x000484F3 File Offset: 0x000466F3
		[LocalizationKey("Perks")]
		private string description
		{
			get
			{
				if (!this.hasDescription)
				{
					return string.Empty;
				}
				return this.displayName + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x00048515 File Offset: 0x00046715
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayName);
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x00048524 File Offset: 0x00046724
		public string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				string value = LocalizationManager.ToPlainText(this.description);
				if (!string.IsNullOrEmpty(value))
				{
					stringBuilder.AppendLine(value);
				}
				PerkBehaviour[] components = base.GetComponents<PerkBehaviour>();
				for (int i = 0; i < components.Length; i++)
				{
					string description = components[i].Description;
					if (!string.IsNullOrEmpty(description))
					{
						stringBuilder.AppendLine(description);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x0004858A File Offset: 0x0004678A
		public PerkRequirement Requirement
		{
			get
			{
				return this.requirement;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x00048592 File Offset: 0x00046792
		public bool DefaultUnlocked
		{
			get
			{
				return this.defaultUnlocked;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x0004859C File Offset: 0x0004679C
		private DateTime UnlockingBeginTime
		{
			get
			{
				DateTime dateTime = DateTime.FromBinary(this.unlockingBeginTimeRaw);
				if (dateTime > DateTime.UtcNow)
				{
					dateTime = DateTime.UtcNow;
					this.unlockingBeginTimeRaw = DateTime.UtcNow.ToBinary();
					GameManager.TimeTravelDetected();
				}
				return dateTime;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x000485E1 File Offset: 0x000467E1
		// (set) Token: 0x060012E5 RID: 4837 RVA: 0x000485E9 File Offset: 0x000467E9
		public bool Unlocked
		{
			get
			{
				return this._unlocked;
			}
			internal set
			{
				this._unlocked = value;
				Action<Perk, bool> action = this.onUnlockStateChanged;
				if (action == null)
				{
					return;
				}
				action(this, value);
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00048604 File Offset: 0x00046804
		public bool Unlocking
		{
			get
			{
				return this.unlocking;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0004860C File Offset: 0x0004680C
		// (set) Token: 0x060012E8 RID: 4840 RVA: 0x00048614 File Offset: 0x00046814
		public PerkTree Master
		{
			get
			{
				return this.master;
			}
			internal set
			{
				this.master = value;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x0004861D File Offset: 0x0004681D
		public string DisplayNameRaw
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x00048625 File Offset: 0x00046825
		public string DescriptionRaw
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x060012EB RID: 4843 RVA: 0x00048630 File Offset: 0x00046830
		// (remove) Token: 0x060012EC RID: 4844 RVA: 0x00048668 File Offset: 0x00046868
		public event Action<Perk, bool> onUnlockStateChanged;

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x060012ED RID: 4845 RVA: 0x000486A0 File Offset: 0x000468A0
		// (remove) Token: 0x060012EE RID: 4846 RVA: 0x000486D4 File Offset: 0x000468D4
		public static event Action<Perk> OnPerkUnlockConfirmed;

		// Token: 0x060012EF RID: 4847 RVA: 0x00048707 File Offset: 0x00046907
		public bool AreAllParentsUnlocked()
		{
			return this.master.AreAllParentsUnlocked(this);
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00048715 File Offset: 0x00046915
		private void OnValidate()
		{
			if (this.master == null)
			{
				this.master = base.GetComponentInParent<PerkTree>();
			}
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00048731 File Offset: 0x00046931
		private bool CheckAndPay()
		{
			return this.requirement == null || (EXPManager.Level >= this.requirement.level && this.requirement.cost.Pay(true, true));
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00048768 File Offset: 0x00046968
		public bool SubmitItemsAndBeginUnlocking()
		{
			if (this.Unlocked)
			{
				Debug.LogError("Perk " + this.displayName + " already unlocked!");
				return false;
			}
			if (!this.CheckAndPay())
			{
				return false;
			}
			this.unlocking = true;
			this.unlockingBeginTimeRaw = DateTime.UtcNow.ToBinary();
			this.master.NotifyChildStateChanged(this);
			Action<Perk, bool> action = this.onUnlockStateChanged;
			if (action != null)
			{
				action(this, this._unlocked);
			}
			return true;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x000487E4 File Offset: 0x000469E4
		public bool ConfirmUnlock()
		{
			if (this.Unlocked)
			{
				return false;
			}
			if (!this.unlocking)
			{
				return false;
			}
			if (DateTime.UtcNow - this.UnlockingBeginTime < this.requirement.RequireTime)
			{
				return false;
			}
			this.Unlocked = true;
			this.unlocking = false;
			this.master.NotifyChildStateChanged(this);
			Action<Perk> onPerkUnlockConfirmed = Perk.OnPerkUnlockConfirmed;
			if (onPerkUnlockConfirmed != null)
			{
				onPerkUnlockConfirmed(this);
			}
			return true;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00048855 File Offset: 0x00046A55
		public bool ForceUnlock()
		{
			if (this.Unlocked)
			{
				return false;
			}
			Debug.Log("Unlock default:" + this.displayName);
			this.Unlocked = true;
			this.unlocking = false;
			this.master.NotifyChildStateChanged(this);
			return true;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00048894 File Offset: 0x00046A94
		public TimeSpan GetRemainingTime()
		{
			if (this.Unlocked)
			{
				return TimeSpan.Zero;
			}
			if (!this.unlocking)
			{
				return TimeSpan.Zero;
			}
			TimeSpan t = DateTime.UtcNow - this.UnlockingBeginTime;
			TimeSpan timeSpan = this.requirement.RequireTime - t;
			if (timeSpan < TimeSpan.Zero)
			{
				return TimeSpan.Zero;
			}
			return timeSpan;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x000488F4 File Offset: 0x00046AF4
		public float GetProgress01()
		{
			TimeSpan remainingTime = this.GetRemainingTime();
			double totalSeconds = this.requirement.RequireTime.TotalSeconds;
			if (totalSeconds <= 0.0)
			{
				return 1f;
			}
			double totalSeconds2 = remainingTime.TotalSeconds;
			return 1f - (float)(totalSeconds2 / totalSeconds);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00048940 File Offset: 0x00046B40
		public void Validate(SelfValidationResult result)
		{
			if (this.master == null)
			{
				result.AddWarning("未指定PerkTree");
			}
			if (this.master)
			{
				if (!this.master.Perks.Contains(this))
				{
					SelfValidationResultItemExtensions.WithFix(result.AddError("PerkTree未包含此Perk"), delegate()
					{
						this.master.perks.Add(this);
					}, true);
				}
				PerkTree perkTree = this.master;
				bool flag;
				if (perkTree == null)
				{
					flag = (null != null);
				}
				else
				{
					PerkTreeRelationGraphOwner relationGraphOwner = perkTree.RelationGraphOwner;
					flag = (((relationGraphOwner != null) ? relationGraphOwner.GetRelatedNode(this) : null) != null);
				}
				if (!flag)
				{
					result.AddError("未在Graph中指定技能的关系");
				}
			}
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000489D2 File Offset: 0x00046BD2
		internal Vector2 GetLayoutPosition()
		{
			if (this.master == null)
			{
				return Vector2.zero;
			}
			PerkTreeRelationGraphOwner relationGraphOwner = this.master.RelationGraphOwner;
			return ((relationGraphOwner != null) ? relationGraphOwner.GetRelatedNode(this) : null).cachedPosition;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00048A05 File Offset: 0x00046C05
		internal void NotifyParentStateChanged()
		{
			Action<Perk, bool> action = this.onUnlockStateChanged;
			if (action == null)
			{
				return;
			}
			action(this, this.Unlocked);
		}

		// Token: 0x04000E7E RID: 3710
		[SerializeField]
		private PerkTree master;

		// Token: 0x04000E7F RID: 3711
		[SerializeField]
		private bool lockInDemo;

		// Token: 0x04000E80 RID: 3712
		[SerializeField]
		private Sprite icon;

		// Token: 0x04000E81 RID: 3713
		[SerializeField]
		private DisplayQuality quality;

		// Token: 0x04000E82 RID: 3714
		[LocalizationKey("Perks")]
		[SerializeField]
		private string displayName = "未命名技能";

		// Token: 0x04000E83 RID: 3715
		[SerializeField]
		private bool hasDescription;

		// Token: 0x04000E84 RID: 3716
		[SerializeField]
		private PerkRequirement requirement;

		// Token: 0x04000E85 RID: 3717
		[SerializeField]
		private bool defaultUnlocked;

		// Token: 0x04000E86 RID: 3718
		[SerializeField]
		internal bool unlocking;

		// Token: 0x04000E87 RID: 3719
		[DateTime]
		[SerializeField]
		internal long unlockingBeginTimeRaw;

		// Token: 0x04000E88 RID: 3720
		[SerializeField]
		private bool _unlocked;
	}
}
