using System;
using System.Collections.Generic;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Buffs
{
	// Token: 0x02000419 RID: 1049
	public class Buff : MonoBehaviour
	{
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002624 RID: 9764 RVA: 0x00084568 File Offset: 0x00082768
		public Buff.BuffExclusiveTags ExclusiveTag
		{
			get
			{
				return this.exclusiveTag;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002625 RID: 9765 RVA: 0x00084570 File Offset: 0x00082770
		public int ExclusiveTagPriority
		{
			get
			{
				return this.exclusiveTagPriority;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002626 RID: 9766 RVA: 0x00084578 File Offset: 0x00082778
		public bool Hide
		{
			get
			{
				return this.hide;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002627 RID: 9767 RVA: 0x00084580 File Offset: 0x00082780
		public CharacterMainControl Character
		{
			get
			{
				CharacterBuffManager characterBuffManager = this.master;
				if (characterBuffManager == null)
				{
					return null;
				}
				return characterBuffManager.Master;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002628 RID: 9768 RVA: 0x00084593 File Offset: 0x00082793
		private Item CharacterItem
		{
			get
			{
				CharacterBuffManager characterBuffManager = this.master;
				if (characterBuffManager == null)
				{
					return null;
				}
				CharacterMainControl characterMainControl = characterBuffManager.Master;
				if (characterMainControl == null)
				{
					return null;
				}
				return characterMainControl.CharacterItem;
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002629 RID: 9769 RVA: 0x000845B1 File Offset: 0x000827B1
		// (set) Token: 0x0600262A RID: 9770 RVA: 0x000845B9 File Offset: 0x000827B9
		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x0600262B RID: 9771 RVA: 0x000845C2 File Offset: 0x000827C2
		// (set) Token: 0x0600262C RID: 9772 RVA: 0x000845CA File Offset: 0x000827CA
		public int CurrentLayers
		{
			get
			{
				return this.currentLayers;
			}
			set
			{
				this.currentLayers = value;
				Action onLayerChangedEvent = this.OnLayerChangedEvent;
				if (onLayerChangedEvent == null)
				{
					return;
				}
				onLayerChangedEvent();
			}
		}

		// Token: 0x14000100 RID: 256
		// (add) Token: 0x0600262D RID: 9773 RVA: 0x000845E4 File Offset: 0x000827E4
		// (remove) Token: 0x0600262E RID: 9774 RVA: 0x0008461C File Offset: 0x0008281C
		public event Action OnLayerChangedEvent;

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x0600262F RID: 9775 RVA: 0x00084651 File Offset: 0x00082851
		public int MaxLayers
		{
			get
			{
				return this.maxLayers;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x00084659 File Offset: 0x00082859
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayName);
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002631 RID: 9777 RVA: 0x00084666 File Offset: 0x00082866
		public string DisplayNameKey
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x0008466E File Offset: 0x0008286E
		public string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.description);
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002633 RID: 9779 RVA: 0x0008467B File Offset: 0x0008287B
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x00084683 File Offset: 0x00082883
		public bool LimitedLifeTime
		{
			get
			{
				return this.limitedLifeTime;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002635 RID: 9781 RVA: 0x0008468B File Offset: 0x0008288B
		public float TotalLifeTime
		{
			get
			{
				return this.totalLifeTime;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x00084693 File Offset: 0x00082893
		public float CurrentLifeTime
		{
			get
			{
				return Time.time - this.timeWhenStarted;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002637 RID: 9783 RVA: 0x000846A1 File Offset: 0x000828A1
		public float RemainingTime
		{
			get
			{
				if (!this.limitedLifeTime)
				{
					return float.PositiveInfinity;
				}
				return this.totalLifeTime - this.CurrentLifeTime;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x000846BE File Offset: 0x000828BE
		public bool IsOutOfTime
		{
			get
			{
				return this.limitedLifeTime && this.CurrentLifeTime >= this.totalLifeTime;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002639 RID: 9785 RVA: 0x000846DB File Offset: 0x000828DB
		public bool DisplayInExtraView
		{
			get
			{
				return this.displayInExtraView;
			}
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000846E4 File Offset: 0x000828E4
		internal void Setup(CharacterBuffManager manager)
		{
			this.master = manager;
			this.timeWhenStarted = Time.time;
			base.transform.SetParent(this.CharacterItem.transform, false);
			if (this.buffFxInstance)
			{
				Object.Destroy(this.buffFxInstance.gameObject);
			}
			if (this.buffFxPfb && manager.Master && manager.Master.characterModel)
			{
				this.buffFxInstance = Object.Instantiate<GameObject>(this.buffFxPfb);
				Transform transform = manager.Master.characterModel.ArmorSocket;
				if (transform == null)
				{
					transform = manager.Master.transform;
				}
				this.buffFxInstance.transform.SetParent(transform);
				this.buffFxInstance.transform.position = transform.position;
				this.buffFxInstance.transform.localRotation = Quaternion.identity;
			}
			foreach (Effect effect in this.effects)
			{
				effect.SetItem(this.CharacterItem);
			}
			this.OnSetup();
			UnityEvent onSetupEvent = this.OnSetupEvent;
			if (onSetupEvent == null)
			{
				return;
			}
			onSetupEvent.Invoke();
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x0008483C File Offset: 0x00082A3C
		internal void NotifyUpdate()
		{
			this.OnUpdate();
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x00084844 File Offset: 0x00082A44
		internal void NotifyOutOfTime()
		{
			this.OnNotifiedOutOfTime();
			Object.Destroy(base.gameObject);
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x00084857 File Offset: 0x00082A57
		internal virtual void NotifyIncomingBuffWithSameID(Buff incomingPrefab)
		{
			this.timeWhenStarted = Time.time;
			if (this.CurrentLayers < this.maxLayers)
			{
				this.CurrentLayers += incomingPrefab.CurrentLayers;
			}
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x00084885 File Offset: 0x00082A85
		protected virtual void OnSetup()
		{
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x00084887 File Offset: 0x00082A87
		protected virtual void OnUpdate()
		{
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x00084889 File Offset: 0x00082A89
		protected virtual void OnNotifiedOutOfTime()
		{
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x0008488B File Offset: 0x00082A8B
		private void OnDestroy()
		{
			if (this.buffFxInstance)
			{
				Object.Destroy(this.buffFxInstance.gameObject);
			}
		}

		// Token: 0x04001A06 RID: 6662
		[SerializeField]
		private int id;

		// Token: 0x04001A07 RID: 6663
		[SerializeField]
		private int maxLayers = 1;

		// Token: 0x04001A08 RID: 6664
		[SerializeField]
		private Buff.BuffExclusiveTags exclusiveTag;

		// Token: 0x04001A09 RID: 6665
		[Tooltip("优先级高的代替优先级低的。同优先级，选剩余时间长的。如果一方不限制时长，选后来的")]
		[SerializeField]
		private int exclusiveTagPriority;

		// Token: 0x04001A0A RID: 6666
		[LocalizationKey("Buffs")]
		[SerializeField]
		private string displayName;

		// Token: 0x04001A0B RID: 6667
		[LocalizationKey("Buffs")]
		[SerializeField]
		private string description;

		// Token: 0x04001A0C RID: 6668
		[SerializeField]
		private Sprite icon;

		// Token: 0x04001A0D RID: 6669
		[SerializeField]
		private bool limitedLifeTime;

		// Token: 0x04001A0E RID: 6670
		[SerializeField]
		private float totalLifeTime;

		// Token: 0x04001A0F RID: 6671
		[SerializeField]
		private List<Effect> effects = new List<Effect>();

		// Token: 0x04001A10 RID: 6672
		[SerializeField]
		private bool hide;

		// Token: 0x04001A11 RID: 6673
		[SerializeField]
		private int currentLayers = 1;

		// Token: 0x04001A12 RID: 6674
		private CharacterBuffManager master;

		// Token: 0x04001A13 RID: 6675
		public UnityEvent OnSetupEvent;

		// Token: 0x04001A15 RID: 6677
		[SerializeField]
		private GameObject buffFxPfb;

		// Token: 0x04001A16 RID: 6678
		private GameObject buffFxInstance;

		// Token: 0x04001A17 RID: 6679
		[HideInInspector]
		public CharacterMainControl fromWho;

		// Token: 0x04001A18 RID: 6680
		public int fromWeaponID;

		// Token: 0x04001A19 RID: 6681
		private float timeWhenStarted;

		// Token: 0x04001A1A RID: 6682
		[SerializeField]
		private bool displayInExtraView;

		// Token: 0x02000691 RID: 1681
		public enum BuffExclusiveTags
		{
			// Token: 0x0400241D RID: 9245
			NotExclusive,
			// Token: 0x0400241E RID: 9246
			Bleeding,
			// Token: 0x0400241F RID: 9247
			Starve,
			// Token: 0x04002420 RID: 9248
			Thirsty,
			// Token: 0x04002421 RID: 9249
			Weight,
			// Token: 0x04002422 RID: 9250
			Poison,
			// Token: 0x04002423 RID: 9251
			Pain,
			// Token: 0x04002424 RID: 9252
			Electric,
			// Token: 0x04002425 RID: 9253
			Burning,
			// Token: 0x04002426 RID: 9254
			Space,
			// Token: 0x04002427 RID: 9255
			StormProtection,
			// Token: 0x04002428 RID: 9256
			Nauseous,
			// Token: 0x04002429 RID: 9257
			Stun,
			// Token: 0x0400242A RID: 9258
			Ghost,
			// Token: 0x0400242B RID: 9259
			Freeze
		}
	}
}
