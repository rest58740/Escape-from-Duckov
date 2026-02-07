using System;
using System.Text;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Endowment
{
	// Token: 0x02000305 RID: 773
	public class EndowmentEntry : MonoBehaviour
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001953 RID: 6483 RVA: 0x0005D18B File Offset: 0x0005B38B
		public EndowmentIndex Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x0005D193 File Offset: 0x0005B393
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x0005D1AA File Offset: 0x0005B3AA
		[LocalizationKey("Default")]
		private string displayNameKey
		{
			get
			{
				return string.Format("Endowmment_{0}", this.index);
			}
			set
			{
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x0005D1AC File Offset: 0x0005B3AC
		// (set) Token: 0x06001957 RID: 6487 RVA: 0x0005D1C3 File Offset: 0x0005B3C3
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return string.Format("Endowmment_{0}_Desc", this.index);
			}
			set
			{
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x0005D1C5 File Offset: 0x0005B3C5
		public string RequirementText
		{
			get
			{
				return LocalizationManager.ToPlainText(this.requirementTextKey);
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x0005D1D2 File Offset: 0x0005B3D2
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x0005D1DA File Offset: 0x0005B3DA
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayNameKey);
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x0005D1E7 File Offset: 0x0005B3E7
		public string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.descriptionKey);
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x0005D1F4 File Offset: 0x0005B3F4
		public string DescriptionAndEffects
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				string description = this.Description;
				stringBuilder.AppendLine(description);
				foreach (EndowmentEntry.ModifierDescription modifierDescription in this.Modifiers)
				{
					stringBuilder.AppendLine("- " + modifierDescription.DescriptionText);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600195D RID: 6493 RVA: 0x0005D252 File Offset: 0x0005B452
		public EndowmentEntry.ModifierDescription[] Modifiers
		{
			get
			{
				return this.modifiers;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x0600195E RID: 6494 RVA: 0x0005D25A File Offset: 0x0005B45A
		private Item CharacterItem
		{
			get
			{
				if (CharacterMainControl.Main == null)
				{
					return null;
				}
				return CharacterMainControl.Main.CharacterItem;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x0005D275 File Offset: 0x0005B475
		public bool UnlockedByDefault
		{
			get
			{
				return this.unlockedByDefault;
			}
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0005D27D File Offset: 0x0005B47D
		public void Activate()
		{
			this.ApplyModifiers();
			UnityEvent<EndowmentEntry> unityEvent = this.onActivate;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0005D296 File Offset: 0x0005B496
		public void Deactivate()
		{
			this.DeleteModifiers();
			UnityEvent<EndowmentEntry> unityEvent = this.onDeactivate;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0005D2B0 File Offset: 0x0005B4B0
		private void ApplyModifiers()
		{
			if (this.CharacterItem == null)
			{
				return;
			}
			this.DeleteModifiers();
			foreach (EndowmentEntry.ModifierDescription modifierDescription in this.modifiers)
			{
				this.CharacterItem.AddModifier(modifierDescription.statKey, new Modifier(modifierDescription.type, modifierDescription.value, this));
			}
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0005D313 File Offset: 0x0005B513
		private void DeleteModifiers()
		{
			if (this.CharacterItem == null)
			{
				return;
			}
			this.CharacterItem.RemoveAllModifiersFrom(this);
		}

		// Token: 0x0400127C RID: 4732
		[SerializeField]
		private EndowmentIndex index;

		// Token: 0x0400127D RID: 4733
		[SerializeField]
		private Sprite icon;

		// Token: 0x0400127E RID: 4734
		[SerializeField]
		[LocalizationKey("Default")]
		private string requirementTextKey;

		// Token: 0x0400127F RID: 4735
		[SerializeField]
		private bool unlockedByDefault;

		// Token: 0x04001280 RID: 4736
		[SerializeField]
		private EndowmentEntry.ModifierDescription[] modifiers;

		// Token: 0x04001281 RID: 4737
		public UnityEvent<EndowmentEntry> onActivate;

		// Token: 0x04001282 RID: 4738
		public UnityEvent<EndowmentEntry> onDeactivate;

		// Token: 0x020005AC RID: 1452
		[Serializable]
		public struct ModifierDescription
		{
			// Token: 0x1700079D RID: 1949
			// (get) Token: 0x060029AA RID: 10666 RVA: 0x0009A7DC File Offset: 0x000989DC
			// (set) Token: 0x060029AB RID: 10667 RVA: 0x0009A7EE File Offset: 0x000989EE
			[LocalizationKey("Default")]
			private string DisplayNameKey
			{
				get
				{
					return "Stat_" + this.statKey;
				}
				set
				{
				}
			}

			// Token: 0x1700079E RID: 1950
			// (get) Token: 0x060029AC RID: 10668 RVA: 0x0009A7F0 File Offset: 0x000989F0
			public string DescriptionText
			{
				get
				{
					string str = LocalizationManager.ToPlainText(this.DisplayNameKey);
					string str2 = "";
					ModifierType modifierType = this.type;
					if (modifierType != null)
					{
						if (modifierType != 100)
						{
							if (modifierType == 200)
							{
								str2 = string.Format("x{0:00.#}%", (1f + this.value) * 100f);
							}
						}
						else if (this.value >= 0f)
						{
							str2 = string.Format("+{0:00.#}%", this.value * 100f);
						}
						else
						{
							str2 = string.Format("-{0:00.#}%", -this.value * 100f);
						}
					}
					else if (this.value >= 0f)
					{
						str2 = string.Format("+{0}", this.value);
					}
					else
					{
						str2 = string.Format("{0}", this.value);
					}
					return str + " " + str2;
				}
			}

			// Token: 0x040020B8 RID: 8376
			public string statKey;

			// Token: 0x040020B9 RID: 8377
			public ModifierType type;

			// Token: 0x040020BA RID: 8378
			public float value;
		}
	}
}
