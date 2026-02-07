using System;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003AD RID: 941
	public class ItemDetailsDisplay : MonoBehaviour
	{
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002127 RID: 8487 RVA: 0x00074597 File Offset: 0x00072797
		private string DurabilityToolTipsFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.durabilityToolTipsFormatKey);
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002128 RID: 8488 RVA: 0x000745A4 File Offset: 0x000727A4
		public ItemSlotCollectionDisplay SlotCollectionDisplay
		{
			get
			{
				return this.slotCollectionDisplay;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002129 RID: 8489 RVA: 0x000745AC File Offset: 0x000727AC
		private PrefabPool<ItemVariableEntry> VariablePool
		{
			get
			{
				if (this._variablePool == null)
				{
					this._variablePool = new PrefabPool<ItemVariableEntry>(this.variableEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._variablePool;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x000745EC File Offset: 0x000727EC
		private PrefabPool<ItemStatEntry> StatPool
		{
			get
			{
				if (this._statPool == null)
				{
					this._statPool = new PrefabPool<ItemStatEntry>(this.statEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._statPool;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600212B RID: 8491 RVA: 0x0007462C File Offset: 0x0007282C
		private PrefabPool<ItemModifierEntry> ModifierPool
		{
			get
			{
				if (this._modifierPool == null)
				{
					this._modifierPool = new PrefabPool<ItemModifierEntry>(this.modifierEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._modifierPool;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600212C RID: 8492 RVA: 0x0007466C File Offset: 0x0007286C
		private PrefabPool<ItemEffectEntry> EffectPool
		{
			get
			{
				if (this._effectPool == null)
				{
					this._effectPool = new PrefabPool<ItemEffectEntry>(this.effectEntryPrefab, this.propertiesParent, null, null, null, true, 10, 10000, null);
				}
				return this._effectPool;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x000746AA File Offset: 0x000728AA
		public Item Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000746B4 File Offset: 0x000728B4
		internal void Setup(Item target)
		{
			this.UnregisterEvents();
			this.Clear();
			if (target == null)
			{
				return;
			}
			this.target = target;
			Sprite fallbackItemIcon = target.Icon;
			if (fallbackItemIcon == null)
			{
				fallbackItemIcon = GameplayDataSettings.UIStyle.FallbackItemIcon;
			}
			this.icon.sprite = fallbackItemIcon;
			ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(target.DisplayQuality);
			this.iconShadow.IgnoreCasterColor = true;
			this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
			this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
			this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
			this.displayName.text = target.DisplayName;
			this.itemID.text = string.Format("#{0}", target.TypeID);
			this.description.text = target.Description;
			this.countContainer.SetActive(target.Stackable);
			this.count.text = target.StackCount.ToString();
			this.tagsDisplay.Setup(target);
			this.usageUtilitiesDisplay.Setup(target);
			this.usableIndicator.gameObject.SetActive(target.UsageUtilities != null);
			this.RefreshDurability();
			this.slotCollectionDisplay.Setup(target, false);
			this.registeredIndicator.SetActive(target.IsRegistered());
			this.RefreshWeightText();
			this.SetupGunDisplays();
			this.SetupVariables();
			this.SetupConstants();
			this.SetupStats();
			this.SetupModifiers();
			this.SetupEffects();
			this.RegisterEvents();
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x00074849 File Offset: 0x00072A49
		private void Awake()
		{
			this.SlotCollectionDisplay.onElementDoubleClicked += this.OnElementDoubleClicked;
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00074864 File Offset: 0x00072A64
		private void OnElementDoubleClicked(ItemSlotCollectionDisplay collectionDisplay, SlotDisplay slotDisplay)
		{
			if (!collectionDisplay.Editable)
			{
				return;
			}
			Item item = slotDisplay.GetItem();
			if (item == null)
			{
				return;
			}
			ItemUtilities.SendToPlayer(item, false, PlayerStorage.Instance != null);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x0007489D File Offset: 0x00072A9D
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000748A5 File Offset: 0x00072AA5
		private void Clear()
		{
			this.tagsDisplay.Clear();
			this.VariablePool.ReleaseAll();
			this.StatPool.ReleaseAll();
			this.ModifierPool.ReleaseAll();
			this.EffectPool.ReleaseAll();
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000748E0 File Offset: 0x00072AE0
		private void SetupGunDisplays()
		{
			Item item = this.Target;
			ItemSetting_Gun itemSetting_Gun = (item != null) ? item.GetComponent<ItemSetting_Gun>() : null;
			if (itemSetting_Gun == null)
			{
				this.bulletTypeDisplay.gameObject.SetActive(false);
				return;
			}
			this.bulletTypeDisplay.gameObject.SetActive(true);
			this.bulletTypeDisplay.Setup(itemSetting_Gun.TargetBulletID);
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x00074940 File Offset: 0x00072B40
		private void SetupVariables()
		{
			if (this.target.Variables == null)
			{
				return;
			}
			foreach (CustomData customData in this.target.Variables)
			{
				if (customData.Display)
				{
					ItemVariableEntry itemVariableEntry = this.VariablePool.Get(this.propertiesParent);
					itemVariableEntry.Setup(customData);
					itemVariableEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000749C4 File Offset: 0x00072BC4
		private void SetupConstants()
		{
			if (this.target.Constants == null)
			{
				return;
			}
			foreach (CustomData customData in this.target.Constants)
			{
				if (customData.Display)
				{
					ItemVariableEntry itemVariableEntry = this.VariablePool.Get(this.propertiesParent);
					itemVariableEntry.Setup(customData);
					itemVariableEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00074A48 File Offset: 0x00072C48
		private void SetupStats()
		{
			if (this.target.Stats == null)
			{
				return;
			}
			foreach (Stat stat in this.target.Stats)
			{
				if (stat.Display)
				{
					ItemStatEntry itemStatEntry = this.StatPool.Get(this.propertiesParent);
					itemStatEntry.Setup(stat);
					itemStatEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00074AD4 File Offset: 0x00072CD4
		private void SetupModifiers()
		{
			if (this.target.Modifiers == null)
			{
				return;
			}
			foreach (ModifierDescription modifierDescription in this.target.Modifiers)
			{
				if (modifierDescription.Display)
				{
					ItemModifierEntry itemModifierEntry = this.ModifierPool.Get(this.propertiesParent);
					itemModifierEntry.Setup(modifierDescription);
					itemModifierEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x00074B60 File Offset: 0x00072D60
		private void SetupEffects()
		{
			foreach (Effect effect in this.target.Effects)
			{
				if (effect.Display)
				{
					ItemEffectEntry itemEffectEntry = this.EffectPool.Get(this.propertiesParent);
					itemEffectEntry.Setup(effect);
					itemEffectEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x00074BDC File Offset: 0x00072DDC
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onDestroy += this.OnTargetDestroy;
			this.target.onChildChanged += this.OnTargetChildChanged;
			this.target.onSetStackCount += this.OnTargetSetStackCount;
			this.target.onDurabilityChanged += this.OnTargetDurabilityChanged;
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x00074C54 File Offset: 0x00072E54
		private void RefreshWeightText()
		{
			this.weightText.text = string.Format(this.weightFormat, this.target.TotalWeight);
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x00074C7C File Offset: 0x00072E7C
		private void OnTargetSetStackCount(Item item)
		{
			this.RefreshWeightText();
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x00074C84 File Offset: 0x00072E84
		private void OnTargetChildChanged(Item obj)
		{
			this.RefreshWeightText();
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x00074C8C File Offset: 0x00072E8C
		internal void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onDestroy -= this.OnTargetDestroy;
			this.target.onChildChanged -= this.OnTargetChildChanged;
			this.target.onSetStackCount -= this.OnTargetSetStackCount;
			this.target.onDurabilityChanged -= this.OnTargetDurabilityChanged;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x00074D04 File Offset: 0x00072F04
		private void OnTargetDurabilityChanged(Item item)
		{
			this.RefreshDurability();
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x00074D0C File Offset: 0x00072F0C
		private void RefreshDurability()
		{
			bool useDurability = this.target.UseDurability;
			this.durabilityContainer.SetActive(useDurability);
			if (useDurability)
			{
				float durability = this.target.Durability;
				float maxDurability = this.target.MaxDurability;
				float maxDurabilityWithLoss = this.target.MaxDurabilityWithLoss;
				string lossPercentage = string.Format("{0:0}%", this.target.DurabilityLoss * 100f);
				float num = durability / maxDurability;
				this.durabilityText.text = string.Format("{0:0} / {1:0}", durability, maxDurabilityWithLoss);
				this.durabilityToolTips.text = StringExtensions.Format(this.DurabilityToolTipsFormat, new
				{
					curDurability = durability,
					maxDurability = maxDurability,
					maxDurabilityWithLoss = maxDurabilityWithLoss,
					lossPercentage = lossPercentage
				});
				this.durabilityFill.fillAmount = num;
				this.durabilityFill.color = this.durabilityColorOverT.Evaluate(num);
				this.durabilityLoss.fillAmount = this.target.DurabilityLoss;
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x00074DFE File Offset: 0x00072FFE
		private void OnTargetDestroy(Item item)
		{
		}

		// Token: 0x04001696 RID: 5782
		[SerializeField]
		private Image icon;

		// Token: 0x04001697 RID: 5783
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x04001698 RID: 5784
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001699 RID: 5785
		[SerializeField]
		private TextMeshProUGUI itemID;

		// Token: 0x0400169A RID: 5786
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x0400169B RID: 5787
		[SerializeField]
		private GameObject countContainer;

		// Token: 0x0400169C RID: 5788
		[SerializeField]
		private TextMeshProUGUI count;

		// Token: 0x0400169D RID: 5789
		[SerializeField]
		private GameObject durabilityContainer;

		// Token: 0x0400169E RID: 5790
		[SerializeField]
		private TextMeshProUGUI durabilityText;

		// Token: 0x0400169F RID: 5791
		[SerializeField]
		private TooltipsProvider durabilityToolTips;

		// Token: 0x040016A0 RID: 5792
		[SerializeField]
		[LocalizationKey("Default")]
		private string durabilityToolTipsFormatKey = "UI_DurabilityToolTips";

		// Token: 0x040016A1 RID: 5793
		[SerializeField]
		private Image durabilityFill;

		// Token: 0x040016A2 RID: 5794
		[SerializeField]
		private Image durabilityLoss;

		// Token: 0x040016A3 RID: 5795
		[SerializeField]
		private Gradient durabilityColorOverT;

		// Token: 0x040016A4 RID: 5796
		[SerializeField]
		private TextMeshProUGUI weightText;

		// Token: 0x040016A5 RID: 5797
		[SerializeField]
		private ItemSlotCollectionDisplay slotCollectionDisplay;

		// Token: 0x040016A6 RID: 5798
		[SerializeField]
		private RectTransform propertiesParent;

		// Token: 0x040016A7 RID: 5799
		[SerializeField]
		private BulletTypeDisplay bulletTypeDisplay;

		// Token: 0x040016A8 RID: 5800
		[SerializeField]
		private TagsDisplay tagsDisplay;

		// Token: 0x040016A9 RID: 5801
		[SerializeField]
		private GameObject usableIndicator;

		// Token: 0x040016AA RID: 5802
		[SerializeField]
		private UsageUtilitiesDisplay usageUtilitiesDisplay;

		// Token: 0x040016AB RID: 5803
		[SerializeField]
		private GameObject registeredIndicator;

		// Token: 0x040016AC RID: 5804
		[SerializeField]
		private ItemVariableEntry variableEntryPrefab;

		// Token: 0x040016AD RID: 5805
		[SerializeField]
		private ItemStatEntry statEntryPrefab;

		// Token: 0x040016AE RID: 5806
		[SerializeField]
		private ItemModifierEntry modifierEntryPrefab;

		// Token: 0x040016AF RID: 5807
		[SerializeField]
		private ItemEffectEntry effectEntryPrefab;

		// Token: 0x040016B0 RID: 5808
		[SerializeField]
		private string weightFormat = "{0:0.#} kg";

		// Token: 0x040016B1 RID: 5809
		private Item target;

		// Token: 0x040016B2 RID: 5810
		private PrefabPool<ItemVariableEntry> _variablePool;

		// Token: 0x040016B3 RID: 5811
		private PrefabPool<ItemStatEntry> _statPool;

		// Token: 0x040016B4 RID: 5812
		private PrefabPool<ItemModifierEntry> _modifierPool;

		// Token: 0x040016B5 RID: 5813
		private PrefabPool<ItemEffectEntry> _effectPool;
	}
}
