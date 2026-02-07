using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Duckov.Achievements;
using Duckov.Buffs;
using Duckov.Buildings;
using Duckov.Crops;
using Duckov.Quests;
using Duckov.Quests.Relations;
using Duckov.UI;
using Eflatun.SceneReference;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Duckov.Utilities
{
	// Token: 0x02000414 RID: 1044
	[CreateAssetMenu(menuName = "Settings/Gameplay Data Settings")]
	public class GameplayDataSettings : ScriptableObject
	{
		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060025D9 RID: 9689 RVA: 0x00083AA7 File Offset: 0x00081CA7
		private static GameplayDataSettings Default
		{
			get
			{
				if (GameplayDataSettings.cachedDefault == null)
				{
					GameplayDataSettings.cachedDefault = Resources.Load<GameplayDataSettings>("GameplayDataSettings");
				}
				return GameplayDataSettings.cachedDefault;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060025DA RID: 9690 RVA: 0x00083ACA File Offset: 0x00081CCA
		public static InputActionAsset InputActions
		{
			get
			{
				return GameplayDataSettings.Default.inputActions;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060025DB RID: 9691 RVA: 0x00083AD6 File Offset: 0x00081CD6
		public static CustomFaceData CustomFaceData
		{
			get
			{
				return GameplayDataSettings.Default.customFaceData;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060025DC RID: 9692 RVA: 0x00083AE2 File Offset: 0x00081CE2
		public static GameplayDataSettings.TagsData Tags
		{
			get
			{
				return GameplayDataSettings.Default.tags;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060025DD RID: 9693 RVA: 0x00083AEE File Offset: 0x00081CEE
		public static GameplayDataSettings.PrefabsData Prefabs
		{
			get
			{
				return GameplayDataSettings.Default.prefabs;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x060025DE RID: 9694 RVA: 0x00083AFA File Offset: 0x00081CFA
		public static UIPrefabsReference UIPrefabs
		{
			get
			{
				return GameplayDataSettings.Default.uiPrefabs;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x00083B06 File Offset: 0x00081D06
		public static GameplayDataSettings.ItemAssetsData ItemAssets
		{
			get
			{
				return GameplayDataSettings.Default.itemAssets;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x00083B12 File Offset: 0x00081D12
		public static GameplayDataSettings.StringListsData StringLists
		{
			get
			{
				return GameplayDataSettings.Default.stringLists;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060025E1 RID: 9697 RVA: 0x00083B1E File Offset: 0x00081D1E
		public static GameplayDataSettings.LayersData Layers
		{
			get
			{
				return GameplayDataSettings.Default.layers;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x00083B2A File Offset: 0x00081D2A
		public static GameplayDataSettings.SceneManagementData SceneManagement
		{
			get
			{
				return GameplayDataSettings.Default.sceneManagement;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x060025E3 RID: 9699 RVA: 0x00083B36 File Offset: 0x00081D36
		public static GameplayDataSettings.BuffsData Buffs
		{
			get
			{
				return GameplayDataSettings.Default.buffs;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060025E4 RID: 9700 RVA: 0x00083B42 File Offset: 0x00081D42
		public static GameplayDataSettings.QuestsData Quests
		{
			get
			{
				return GameplayDataSettings.Default.quests;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060025E5 RID: 9701 RVA: 0x00083B4E File Offset: 0x00081D4E
		public static QuestCollection QuestCollection
		{
			get
			{
				return GameplayDataSettings.Default.quests.QuestCollection;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060025E6 RID: 9702 RVA: 0x00083B5F File Offset: 0x00081D5F
		public static QuestRelationGraph QuestRelation
		{
			get
			{
				return GameplayDataSettings.Default.quests.QuestRelation;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060025E7 RID: 9703 RVA: 0x00083B70 File Offset: 0x00081D70
		public static GameplayDataSettings.EconomyData Economy
		{
			get
			{
				return GameplayDataSettings.Default.economyData;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x00083B7C File Offset: 0x00081D7C
		public static GameplayDataSettings.UIStyleData UIStyle
		{
			get
			{
				return GameplayDataSettings.Default.uiStyleData;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060025E9 RID: 9705 RVA: 0x00083B88 File Offset: 0x00081D88
		public static BuildingDataCollection BuildingDataCollection
		{
			get
			{
				return GameplayDataSettings.Default.buildingDataCollection;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060025EA RID: 9706 RVA: 0x00083B94 File Offset: 0x00081D94
		public static CraftingFormulaCollection CraftingFormulas
		{
			get
			{
				return GameplayDataSettings.Default.craftingFormulas;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060025EB RID: 9707 RVA: 0x00083BA0 File Offset: 0x00081DA0
		public static DecomposeDatabase DecomposeDatabase
		{
			get
			{
				return GameplayDataSettings.Default.decomposeDatabase;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060025EC RID: 9708 RVA: 0x00083BAC File Offset: 0x00081DAC
		public static StatInfoDatabase StatInfo
		{
			get
			{
				return GameplayDataSettings.Default.statInfo;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x00083BB8 File Offset: 0x00081DB8
		public static StockShopDatabase StockshopDatabase
		{
			get
			{
				return GameplayDataSettings.Default.stockShopDatabase;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060025EE RID: 9710 RVA: 0x00083BC4 File Offset: 0x00081DC4
		public static GameplayDataSettings.LootingData Looting
		{
			get
			{
				return GameplayDataSettings.Default.looting;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060025EF RID: 9711 RVA: 0x00083BD0 File Offset: 0x00081DD0
		public static AchievementDatabase AchievementDatabase
		{
			get
			{
				return GameplayDataSettings.Default.achivementDatabase;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060025F0 RID: 9712 RVA: 0x00083BDC File Offset: 0x00081DDC
		public static CropDatabase CropDatabase
		{
			get
			{
				return GameplayDataSettings.Default.cropDatabase;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060025F1 RID: 9713 RVA: 0x00083BE8 File Offset: 0x00081DE8
		public static GameplayDataSettings.CharacterRandomPresets CharacterRandomPresetData
		{
			get
			{
				return GameplayDataSettings.Default.characterRandomPresets;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060025F2 RID: 9714 RVA: 0x00083BF4 File Offset: 0x00081DF4
		public static GameplayDataSettings.DialogData DialogDatas
		{
			get
			{
				return GameplayDataSettings.Default.dialogDatas;
			}
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x00083C00 File Offset: 0x00081E00
		internal static Sprite GetSprite(string key)
		{
			return GameplayDataSettings.Default.spriteData.GetSprite(key);
		}

		// Token: 0x040019C1 RID: 6593
		private static GameplayDataSettings cachedDefault;

		// Token: 0x040019C2 RID: 6594
		[SerializeField]
		private GameplayDataSettings.TagsData tags;

		// Token: 0x040019C3 RID: 6595
		[SerializeField]
		private GameplayDataSettings.PrefabsData prefabs;

		// Token: 0x040019C4 RID: 6596
		[SerializeField]
		private UIPrefabsReference uiPrefabs;

		// Token: 0x040019C5 RID: 6597
		[SerializeField]
		private GameplayDataSettings.ItemAssetsData itemAssets;

		// Token: 0x040019C6 RID: 6598
		[SerializeField]
		private GameplayDataSettings.StringListsData stringLists;

		// Token: 0x040019C7 RID: 6599
		[SerializeField]
		private GameplayDataSettings.LayersData layers;

		// Token: 0x040019C8 RID: 6600
		[SerializeField]
		private GameplayDataSettings.SceneManagementData sceneManagement;

		// Token: 0x040019C9 RID: 6601
		[SerializeField]
		private GameplayDataSettings.BuffsData buffs;

		// Token: 0x040019CA RID: 6602
		[SerializeField]
		private GameplayDataSettings.QuestsData quests;

		// Token: 0x040019CB RID: 6603
		[SerializeField]
		private GameplayDataSettings.EconomyData economyData;

		// Token: 0x040019CC RID: 6604
		[SerializeField]
		private GameplayDataSettings.UIStyleData uiStyleData;

		// Token: 0x040019CD RID: 6605
		[SerializeField]
		private InputActionAsset inputActions;

		// Token: 0x040019CE RID: 6606
		[SerializeField]
		private BuildingDataCollection buildingDataCollection;

		// Token: 0x040019CF RID: 6607
		[SerializeField]
		private CustomFaceData customFaceData;

		// Token: 0x040019D0 RID: 6608
		[SerializeField]
		private CraftingFormulaCollection craftingFormulas;

		// Token: 0x040019D1 RID: 6609
		[SerializeField]
		private DecomposeDatabase decomposeDatabase;

		// Token: 0x040019D2 RID: 6610
		[SerializeField]
		private StatInfoDatabase statInfo;

		// Token: 0x040019D3 RID: 6611
		[SerializeField]
		private StockShopDatabase stockShopDatabase;

		// Token: 0x040019D4 RID: 6612
		[SerializeField]
		private GameplayDataSettings.LootingData looting;

		// Token: 0x040019D5 RID: 6613
		[SerializeField]
		private AchievementDatabase achivementDatabase;

		// Token: 0x040019D6 RID: 6614
		[SerializeField]
		private CropDatabase cropDatabase;

		// Token: 0x040019D7 RID: 6615
		[SerializeField]
		private GameplayDataSettings.SpritesData spriteData;

		// Token: 0x040019D8 RID: 6616
		[SerializeField]
		private GameplayDataSettings.CharacterRandomPresets characterRandomPresets;

		// Token: 0x040019D9 RID: 6617
		[SerializeField]
		private GameplayDataSettings.DialogData dialogDatas;

		// Token: 0x0200067B RID: 1659
		[Serializable]
		public class LootingData
		{
			// Token: 0x06002B76 RID: 11126 RVA: 0x000A59D4 File Offset: 0x000A3BD4
			public float MGetInspectingTime(Item item)
			{
				int num = item.Quality;
				if (num < 0)
				{
					num = 0;
				}
				if (num >= this.inspectingTimes.Length)
				{
					num = this.inspectingTimes.Length - 1;
				}
				return this.inspectingTimes[num];
			}

			// Token: 0x06002B77 RID: 11127 RVA: 0x000A5A0C File Offset: 0x000A3C0C
			public static float GetInspectingTime(Item item)
			{
				GameplayDataSettings.LootingData looting = GameplayDataSettings.Looting;
				if (looting == null)
				{
					return 1f;
				}
				return looting.MGetInspectingTime(item);
			}

			// Token: 0x040023A1 RID: 9121
			public float[] inspectingTimes;
		}

		// Token: 0x0200067C RID: 1660
		[Serializable]
		public class TagsData
		{
			// Token: 0x170007CA RID: 1994
			// (get) Token: 0x06002B79 RID: 11129 RVA: 0x000A5A37 File Offset: 0x000A3C37
			public Tag Character
			{
				get
				{
					return this.character;
				}
			}

			// Token: 0x170007CB RID: 1995
			// (get) Token: 0x06002B7A RID: 11130 RVA: 0x000A5A3F File Offset: 0x000A3C3F
			public Tag LockInDemoTag
			{
				get
				{
					return this.lockInDemoTag;
				}
			}

			// Token: 0x170007CC RID: 1996
			// (get) Token: 0x06002B7B RID: 11131 RVA: 0x000A5A47 File Offset: 0x000A3C47
			public Tag Helmat
			{
				get
				{
					return this.helmat;
				}
			}

			// Token: 0x170007CD RID: 1997
			// (get) Token: 0x06002B7C RID: 11132 RVA: 0x000A5A4F File Offset: 0x000A3C4F
			public Tag Armor
			{
				get
				{
					return this.armor;
				}
			}

			// Token: 0x170007CE RID: 1998
			// (get) Token: 0x06002B7D RID: 11133 RVA: 0x000A5A57 File Offset: 0x000A3C57
			public Tag Backpack
			{
				get
				{
					return this.backpack;
				}
			}

			// Token: 0x170007CF RID: 1999
			// (get) Token: 0x06002B7E RID: 11134 RVA: 0x000A5A5F File Offset: 0x000A3C5F
			public Tag Bullet
			{
				get
				{
					return this.bullet;
				}
			}

			// Token: 0x170007D0 RID: 2000
			// (get) Token: 0x06002B7F RID: 11135 RVA: 0x000A5A67 File Offset: 0x000A3C67
			public Tag Bait
			{
				get
				{
					return this.bait;
				}
			}

			// Token: 0x170007D1 RID: 2001
			// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000A5A6F File Offset: 0x000A3C6F
			public Tag AdvancedDebuffMode
			{
				get
				{
					return this.advancedDebuffMode;
				}
			}

			// Token: 0x170007D2 RID: 2002
			// (get) Token: 0x06002B81 RID: 11137 RVA: 0x000A5A77 File Offset: 0x000A3C77
			public Tag Special
			{
				get
				{
					return this.special;
				}
			}

			// Token: 0x170007D3 RID: 2003
			// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000A5A7F File Offset: 0x000A3C7F
			public Tag DestroyOnLootBox
			{
				get
				{
					return this.destroyOnLootBox;
				}
			}

			// Token: 0x170007D4 RID: 2004
			// (get) Token: 0x06002B83 RID: 11139 RVA: 0x000A5A87 File Offset: 0x000A3C87
			public Tag DontDropOnDeadInSlot
			{
				get
				{
					return this.dontDropOnDeadInSlot;
				}
			}

			// Token: 0x170007D5 RID: 2005
			// (get) Token: 0x06002B84 RID: 11140 RVA: 0x000A5A8F File Offset: 0x000A3C8F
			public ReadOnlyCollection<Tag> AllTags
			{
				get
				{
					if (this.tagsReadOnly == null)
					{
						this.tagsReadOnly = this.allTags.AsReadOnly();
					}
					return this.tagsReadOnly;
				}
			}

			// Token: 0x170007D6 RID: 2006
			// (get) Token: 0x06002B85 RID: 11141 RVA: 0x000A5AB0 File Offset: 0x000A3CB0
			public Tag Gun
			{
				get
				{
					if (this.gun == null)
					{
						this.gun = this.Get("Gun");
					}
					return this.gun;
				}
			}

			// Token: 0x06002B86 RID: 11142 RVA: 0x000A5AD8 File Offset: 0x000A3CD8
			internal Tag Get(string name)
			{
				foreach (Tag tag in this.AllTags)
				{
					if (tag.name == name)
					{
						return tag;
					}
				}
				return null;
			}

			// Token: 0x040023A2 RID: 9122
			[SerializeField]
			private Tag character;

			// Token: 0x040023A3 RID: 9123
			[SerializeField]
			private Tag lockInDemoTag;

			// Token: 0x040023A4 RID: 9124
			[SerializeField]
			private Tag helmat;

			// Token: 0x040023A5 RID: 9125
			[SerializeField]
			private Tag armor;

			// Token: 0x040023A6 RID: 9126
			[SerializeField]
			private Tag backpack;

			// Token: 0x040023A7 RID: 9127
			[SerializeField]
			private Tag bullet;

			// Token: 0x040023A8 RID: 9128
			[SerializeField]
			private Tag bait;

			// Token: 0x040023A9 RID: 9129
			[SerializeField]
			private Tag advancedDebuffMode;

			// Token: 0x040023AA RID: 9130
			[SerializeField]
			private Tag special;

			// Token: 0x040023AB RID: 9131
			[SerializeField]
			private Tag destroyOnLootBox;

			// Token: 0x040023AC RID: 9132
			[FormerlySerializedAs("dontDropOnDead")]
			[SerializeField]
			private Tag dontDropOnDeadInSlot;

			// Token: 0x040023AD RID: 9133
			[SerializeField]
			private List<Tag> allTags = new List<Tag>();

			// Token: 0x040023AE RID: 9134
			private ReadOnlyCollection<Tag> tagsReadOnly;

			// Token: 0x040023AF RID: 9135
			private Tag gun;
		}

		// Token: 0x0200067D RID: 1661
		[Serializable]
		public class PrefabsData
		{
			// Token: 0x170007D7 RID: 2007
			// (get) Token: 0x06002B88 RID: 11144 RVA: 0x000A5B47 File Offset: 0x000A3D47
			public LevelManager LevelManagerPrefab
			{
				get
				{
					return this.levelManagerPrefab;
				}
			}

			// Token: 0x170007D8 RID: 2008
			// (get) Token: 0x06002B89 RID: 11145 RVA: 0x000A5B4F File Offset: 0x000A3D4F
			public CharacterMainControl CharacterPrefab
			{
				get
				{
					return this.characterPrefab;
				}
			}

			// Token: 0x170007D9 RID: 2009
			// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000A5B57 File Offset: 0x000A3D57
			public GameObject BulletHitObsticleFx
			{
				get
				{
					return this.bulletHitObsticleFx;
				}
			}

			// Token: 0x170007DA RID: 2010
			// (get) Token: 0x06002B8B RID: 11147 RVA: 0x000A5B5F File Offset: 0x000A3D5F
			public GameObject QuestMarker
			{
				get
				{
					return this.questMarker;
				}
			}

			// Token: 0x170007DB RID: 2011
			// (get) Token: 0x06002B8C RID: 11148 RVA: 0x000A5B67 File Offset: 0x000A3D67
			public DuckovItemAgent PickupAgentPrefab
			{
				get
				{
					return this.pickupAgentPrefab;
				}
			}

			// Token: 0x170007DC RID: 2012
			// (get) Token: 0x06002B8D RID: 11149 RVA: 0x000A5B6F File Offset: 0x000A3D6F
			public DuckovItemAgent PickupAgentNoRendererPrefab
			{
				get
				{
					return this.pickupAgentNoRendererPrefab;
				}
			}

			// Token: 0x170007DD RID: 2013
			// (get) Token: 0x06002B8E RID: 11150 RVA: 0x000A5B77 File Offset: 0x000A3D77
			public DuckovItemAgent HandheldAgentPrefab
			{
				get
				{
					return this.handheldAgentPrefab;
				}
			}

			// Token: 0x170007DE RID: 2014
			// (get) Token: 0x06002B8F RID: 11151 RVA: 0x000A5B7F File Offset: 0x000A3D7F
			public InteractableLootbox LootBoxPrefab
			{
				get
				{
					return this.lootBoxPrefab;
				}
			}

			// Token: 0x170007DF RID: 2015
			// (get) Token: 0x06002B90 RID: 11152 RVA: 0x000A5B87 File Offset: 0x000A3D87
			public InteractableLootbox LootBoxPrefab_Tomb
			{
				get
				{
					return this.lootBoxPrefab_Tomb;
				}
			}

			// Token: 0x170007E0 RID: 2016
			// (get) Token: 0x06002B91 RID: 11153 RVA: 0x000A5B8F File Offset: 0x000A3D8F
			public InteractMarker InteractMarker
			{
				get
				{
					return this.interactMarker;
				}
			}

			// Token: 0x170007E1 RID: 2017
			// (get) Token: 0x06002B92 RID: 11154 RVA: 0x000A5B97 File Offset: 0x000A3D97
			public HeadCollider HeadCollider
			{
				get
				{
					return this.headCollider;
				}
			}

			// Token: 0x170007E2 RID: 2018
			// (get) Token: 0x06002B93 RID: 11155 RVA: 0x000A5B9F File Offset: 0x000A3D9F
			public Projectile DefaultBullet
			{
				get
				{
					return this.defaultBullet;
				}
			}

			// Token: 0x170007E3 RID: 2019
			// (get) Token: 0x06002B94 RID: 11156 RVA: 0x000A5BA7 File Offset: 0x000A3DA7
			public GameObject BuildingBlockAreaMesh
			{
				get
				{
					return this.buildingBlockAreaMesh;
				}
			}

			// Token: 0x170007E4 RID: 2020
			// (get) Token: 0x06002B95 RID: 11157 RVA: 0x000A5BAF File Offset: 0x000A3DAF
			public GameObject AlertFxPrefab
			{
				get
				{
					return this.alertFxPrefab;
				}
			}

			// Token: 0x170007E5 RID: 2021
			// (get) Token: 0x06002B96 RID: 11158 RVA: 0x000A5BB7 File Offset: 0x000A3DB7
			public GameObject KazooUi
			{
				get
				{
					return this.kazooUi;
				}
			}

			// Token: 0x170007E6 RID: 2022
			// (get) Token: 0x06002B97 RID: 11159 RVA: 0x000A5BBF File Offset: 0x000A3DBF
			public UIInputManager UIInputManagerPrefab
			{
				get
				{
					return this.uiInputManagerPrefab;
				}
			}

			// Token: 0x040023B0 RID: 9136
			[SerializeField]
			private LevelManager levelManagerPrefab;

			// Token: 0x040023B1 RID: 9137
			[SerializeField]
			private CharacterMainControl characterPrefab;

			// Token: 0x040023B2 RID: 9138
			[SerializeField]
			private GameObject bulletHitObsticleFx;

			// Token: 0x040023B3 RID: 9139
			[SerializeField]
			private GameObject questMarker;

			// Token: 0x040023B4 RID: 9140
			[SerializeField]
			private DuckovItemAgent pickupAgentPrefab;

			// Token: 0x040023B5 RID: 9141
			[SerializeField]
			private DuckovItemAgent pickupAgentNoRendererPrefab;

			// Token: 0x040023B6 RID: 9142
			[SerializeField]
			private DuckovItemAgent handheldAgentPrefab;

			// Token: 0x040023B7 RID: 9143
			[SerializeField]
			private InteractableLootbox lootBoxPrefab;

			// Token: 0x040023B8 RID: 9144
			[SerializeField]
			private InteractableLootbox lootBoxPrefab_Tomb;

			// Token: 0x040023B9 RID: 9145
			[SerializeField]
			private InteractMarker interactMarker;

			// Token: 0x040023BA RID: 9146
			[SerializeField]
			private HeadCollider headCollider;

			// Token: 0x040023BB RID: 9147
			[SerializeField]
			private Projectile defaultBullet;

			// Token: 0x040023BC RID: 9148
			[SerializeField]
			private UIInputManager uiInputManagerPrefab;

			// Token: 0x040023BD RID: 9149
			[SerializeField]
			private GameObject buildingBlockAreaMesh;

			// Token: 0x040023BE RID: 9150
			[SerializeField]
			private GameObject alertFxPrefab;

			// Token: 0x040023BF RID: 9151
			[SerializeField]
			private GameObject kazooUi;
		}

		// Token: 0x0200067E RID: 1662
		[Serializable]
		public class BuffsData
		{
			// Token: 0x170007E7 RID: 2023
			// (get) Token: 0x06002B99 RID: 11161 RVA: 0x000A5BCF File Offset: 0x000A3DCF
			public Buff BleedSBuff
			{
				get
				{
					return this.bleedSBuff;
				}
			}

			// Token: 0x170007E8 RID: 2024
			// (get) Token: 0x06002B9A RID: 11162 RVA: 0x000A5BD7 File Offset: 0x000A3DD7
			public Buff UnlimitBleedBuff
			{
				get
				{
					return this.unlimitBleedBuff;
				}
			}

			// Token: 0x170007E9 RID: 2025
			// (get) Token: 0x06002B9B RID: 11163 RVA: 0x000A5BDF File Offset: 0x000A3DDF
			public Buff BoneCrackBuff
			{
				get
				{
					return this.boneCrackBuff;
				}
			}

			// Token: 0x170007EA RID: 2026
			// (get) Token: 0x06002B9C RID: 11164 RVA: 0x000A5BE7 File Offset: 0x000A3DE7
			public Buff WoundBuff
			{
				get
				{
					return this.woundBuff;
				}
			}

			// Token: 0x170007EB RID: 2027
			// (get) Token: 0x06002B9D RID: 11165 RVA: 0x000A5BEF File Offset: 0x000A3DEF
			public Buff Weight_Light
			{
				get
				{
					return this.weight_Light;
				}
			}

			// Token: 0x170007EC RID: 2028
			// (get) Token: 0x06002B9E RID: 11166 RVA: 0x000A5BF7 File Offset: 0x000A3DF7
			public Buff Weight_Heavy
			{
				get
				{
					return this.weight_Heavy;
				}
			}

			// Token: 0x170007ED RID: 2029
			// (get) Token: 0x06002B9F RID: 11167 RVA: 0x000A5BFF File Offset: 0x000A3DFF
			public Buff Weight_SuperHeavy
			{
				get
				{
					return this.weight_SuperHeavy;
				}
			}

			// Token: 0x170007EE RID: 2030
			// (get) Token: 0x06002BA0 RID: 11168 RVA: 0x000A5C07 File Offset: 0x000A3E07
			public Buff Weight_Overweight
			{
				get
				{
					return this.weight_Overweight;
				}
			}

			// Token: 0x170007EF RID: 2031
			// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x000A5C0F File Offset: 0x000A3E0F
			public Buff Pain
			{
				get
				{
					return this.pain;
				}
			}

			// Token: 0x170007F0 RID: 2032
			// (get) Token: 0x06002BA2 RID: 11170 RVA: 0x000A5C17 File Offset: 0x000A3E17
			public Buff BaseBuff
			{
				get
				{
					return this.baseBuff;
				}
			}

			// Token: 0x170007F1 RID: 2033
			// (get) Token: 0x06002BA3 RID: 11171 RVA: 0x000A5C1F File Offset: 0x000A3E1F
			public Buff Starve
			{
				get
				{
					return this.starve;
				}
			}

			// Token: 0x170007F2 RID: 2034
			// (get) Token: 0x06002BA4 RID: 11172 RVA: 0x000A5C27 File Offset: 0x000A3E27
			public Buff Thirsty
			{
				get
				{
					return this.thirsty;
				}
			}

			// Token: 0x170007F3 RID: 2035
			// (get) Token: 0x06002BA5 RID: 11173 RVA: 0x000A5C2F File Offset: 0x000A3E2F
			public Buff Burn
			{
				get
				{
					return this.burn;
				}
			}

			// Token: 0x170007F4 RID: 2036
			// (get) Token: 0x06002BA6 RID: 11174 RVA: 0x000A5C37 File Offset: 0x000A3E37
			public Buff Poison
			{
				get
				{
					return this.poison;
				}
			}

			// Token: 0x170007F5 RID: 2037
			// (get) Token: 0x06002BA7 RID: 11175 RVA: 0x000A5C3F File Offset: 0x000A3E3F
			public Buff Electric
			{
				get
				{
					return this.electric;
				}
			}

			// Token: 0x170007F6 RID: 2038
			// (get) Token: 0x06002BA8 RID: 11176 RVA: 0x000A5C47 File Offset: 0x000A3E47
			public Buff Space
			{
				get
				{
					return this.space;
				}
			}

			// Token: 0x170007F7 RID: 2039
			// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x000A5C4F File Offset: 0x000A3E4F
			public Buff Cold
			{
				get
				{
					return this.cold;
				}
			}

			// Token: 0x170007F8 RID: 2040
			// (get) Token: 0x06002BAA RID: 11178 RVA: 0x000A5C57 File Offset: 0x000A3E57
			public Buff SuperCold
			{
				get
				{
					return this.superCold;
				}
			}

			// Token: 0x06002BAB RID: 11179 RVA: 0x000A5C60 File Offset: 0x000A3E60
			public string GetBuffDisplayName(int id)
			{
				Buff buff = this.allBuffs.Find((Buff e) => e != null && e.ID == id);
				if (buff == null)
				{
					return "?";
				}
				return buff.DisplayName;
			}

			// Token: 0x040023C0 RID: 9152
			[SerializeField]
			private Buff bleedSBuff;

			// Token: 0x040023C1 RID: 9153
			[SerializeField]
			private Buff unlimitBleedBuff;

			// Token: 0x040023C2 RID: 9154
			[SerializeField]
			private Buff boneCrackBuff;

			// Token: 0x040023C3 RID: 9155
			[SerializeField]
			private Buff woundBuff;

			// Token: 0x040023C4 RID: 9156
			[SerializeField]
			private Buff weight_Light;

			// Token: 0x040023C5 RID: 9157
			[SerializeField]
			private Buff weight_Heavy;

			// Token: 0x040023C6 RID: 9158
			[SerializeField]
			private Buff weight_SuperHeavy;

			// Token: 0x040023C7 RID: 9159
			[SerializeField]
			private Buff weight_Overweight;

			// Token: 0x040023C8 RID: 9160
			[SerializeField]
			private Buff pain;

			// Token: 0x040023C9 RID: 9161
			[SerializeField]
			private Buff baseBuff;

			// Token: 0x040023CA RID: 9162
			[SerializeField]
			private Buff starve;

			// Token: 0x040023CB RID: 9163
			[SerializeField]
			private Buff thirsty;

			// Token: 0x040023CC RID: 9164
			[SerializeField]
			private Buff burn;

			// Token: 0x040023CD RID: 9165
			[SerializeField]
			private Buff poison;

			// Token: 0x040023CE RID: 9166
			[SerializeField]
			private Buff electric;

			// Token: 0x040023CF RID: 9167
			[SerializeField]
			private Buff space;

			// Token: 0x040023D0 RID: 9168
			[SerializeField]
			private Buff cold;

			// Token: 0x040023D1 RID: 9169
			[SerializeField]
			private Buff superCold;

			// Token: 0x040023D2 RID: 9170
			[SerializeField]
			private List<Buff> allBuffs;
		}

		// Token: 0x0200067F RID: 1663
		[Serializable]
		public class ItemAssetsData
		{
			// Token: 0x170007F9 RID: 2041
			// (get) Token: 0x06002BAD RID: 11181 RVA: 0x000A5CAF File Offset: 0x000A3EAF
			public int DefaultCharacterItemTypeID
			{
				get
				{
					return this.defaultCharacterItemTypeID;
				}
			}

			// Token: 0x170007FA RID: 2042
			// (get) Token: 0x06002BAE RID: 11182 RVA: 0x000A5CB7 File Offset: 0x000A3EB7
			public int CashItemTypeID
			{
				get
				{
					return this.cashItemTypeID;
				}
			}

			// Token: 0x040023D3 RID: 9171
			[SerializeField]
			[ItemTypeID]
			private int defaultCharacterItemTypeID;

			// Token: 0x040023D4 RID: 9172
			[SerializeField]
			[ItemTypeID]
			private int cashItemTypeID;
		}

		// Token: 0x02000680 RID: 1664
		public class StringListsData
		{
			// Token: 0x040023D5 RID: 9173
			public static StringList StatKeys;

			// Token: 0x040023D6 RID: 9174
			public static StringList SlotTypes;

			// Token: 0x040023D7 RID: 9175
			public static StringList ItemAgentKeys;
		}

		// Token: 0x02000681 RID: 1665
		[Serializable]
		public class LayersData
		{
			// Token: 0x06002BB1 RID: 11185 RVA: 0x000A5CCF File Offset: 0x000A3ECF
			public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
			{
				return (1 << layer & layerMask) != 0;
			}

			// Token: 0x040023D8 RID: 9176
			public LayerMask damageReceiverLayerMask;

			// Token: 0x040023D9 RID: 9177
			public LayerMask wallLayerMask;

			// Token: 0x040023DA RID: 9178
			public LayerMask groundLayerMask;

			// Token: 0x040023DB RID: 9179
			public LayerMask halfObsticleLayer;

			// Token: 0x040023DC RID: 9180
			public LayerMask fowBlockLayers;

			// Token: 0x040023DD RID: 9181
			public LayerMask fowBlockLayersWithThermal;
		}

		// Token: 0x02000682 RID: 1666
		[Serializable]
		public class DialogData
		{
			// Token: 0x06002BB3 RID: 11187 RVA: 0x000A5CEB File Offset: 0x000A3EEB
			public string GetColdDialog()
			{
				return LocalizationManager.ToPlainText(this.coldDialogKeys.GetRandom<string>());
			}

			// Token: 0x06002BB4 RID: 11188 RVA: 0x000A5CFD File Offset: 0x000A3EFD
			public string GetSuperColdDialog()
			{
				return LocalizationManager.ToPlainText(this.superColdDialogKeys.GetRandom<string>());
			}

			// Token: 0x040023DE RID: 9182
			[SerializeField]
			[LocalizationKey("Default")]
			private List<string> coldDialogKeys;

			// Token: 0x040023DF RID: 9183
			[SerializeField]
			[LocalizationKey("Default")]
			private List<string> superColdDialogKeys;
		}

		// Token: 0x02000683 RID: 1667
		[Serializable]
		public class SceneManagementData
		{
			// Token: 0x170007FB RID: 2043
			// (get) Token: 0x06002BB6 RID: 11190 RVA: 0x000A5D17 File Offset: 0x000A3F17
			public SceneInfoCollection SceneInfoCollection
			{
				get
				{
					return this.sceneInfoCollection;
				}
			}

			// Token: 0x170007FC RID: 2044
			// (get) Token: 0x06002BB7 RID: 11191 RVA: 0x000A5D1F File Offset: 0x000A3F1F
			public SceneReference PrologueScene
			{
				get
				{
					return this.prologueScene;
				}
			}

			// Token: 0x170007FD RID: 2045
			// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x000A5D27 File Offset: 0x000A3F27
			public SceneReference MainMenuScene
			{
				get
				{
					return this.mainMenuScene;
				}
			}

			// Token: 0x170007FE RID: 2046
			// (get) Token: 0x06002BB9 RID: 11193 RVA: 0x000A5D2F File Offset: 0x000A3F2F
			public SceneReference BaseScene
			{
				get
				{
					return this.baseScene;
				}
			}

			// Token: 0x170007FF RID: 2047
			// (get) Token: 0x06002BBA RID: 11194 RVA: 0x000A5D37 File Offset: 0x000A3F37
			public SceneReference FailLoadingScreenScene
			{
				get
				{
					return this.failLoadingScreenScene;
				}
			}

			// Token: 0x17000800 RID: 2048
			// (get) Token: 0x06002BBB RID: 11195 RVA: 0x000A5D3F File Offset: 0x000A3F3F
			public SceneReference EvacuateScreenScene
			{
				get
				{
					return this.evacuateScreenScene;
				}
			}

			// Token: 0x040023E0 RID: 9184
			[SerializeField]
			private SceneInfoCollection sceneInfoCollection;

			// Token: 0x040023E1 RID: 9185
			[SerializeField]
			private SceneReference prologueScene;

			// Token: 0x040023E2 RID: 9186
			[SerializeField]
			private SceneReference mainMenuScene;

			// Token: 0x040023E3 RID: 9187
			[SerializeField]
			private SceneReference baseScene;

			// Token: 0x040023E4 RID: 9188
			[SerializeField]
			private SceneReference failLoadingScreenScene;

			// Token: 0x040023E5 RID: 9189
			[SerializeField]
			private SceneReference evacuateScreenScene;
		}

		// Token: 0x02000684 RID: 1668
		[Serializable]
		public class QuestsData
		{
			// Token: 0x17000801 RID: 2049
			// (get) Token: 0x06002BBD RID: 11197 RVA: 0x000A5D4F File Offset: 0x000A3F4F
			private string DefaultQuestGiverDisplayName
			{
				get
				{
					return this.defaultQuestGiverDisplayName;
				}
			}

			// Token: 0x17000802 RID: 2050
			// (get) Token: 0x06002BBE RID: 11198 RVA: 0x000A5D57 File Offset: 0x000A3F57
			public QuestCollection QuestCollection
			{
				get
				{
					return this.questCollection;
				}
			}

			// Token: 0x17000803 RID: 2051
			// (get) Token: 0x06002BBF RID: 11199 RVA: 0x000A5D5F File Offset: 0x000A3F5F
			public QuestRelationGraph QuestRelation
			{
				get
				{
					return this.questRelation;
				}
			}

			// Token: 0x06002BC0 RID: 11200 RVA: 0x000A5D68 File Offset: 0x000A3F68
			public GameplayDataSettings.QuestsData.QuestGiverInfo GetInfo(QuestGiverID id)
			{
				return this.questGiverInfos.Find((GameplayDataSettings.QuestsData.QuestGiverInfo e) => e != null && e.id == id);
			}

			// Token: 0x06002BC1 RID: 11201 RVA: 0x000A5D9C File Offset: 0x000A3F9C
			public string GetDisplayName(QuestGiverID id)
			{
				return LocalizationManager.ToPlainText(string.Format("Character_{0}", id));
			}

			// Token: 0x040023E6 RID: 9190
			[SerializeField]
			private QuestCollection questCollection;

			// Token: 0x040023E7 RID: 9191
			[SerializeField]
			private QuestRelationGraph questRelation;

			// Token: 0x040023E8 RID: 9192
			[SerializeField]
			private List<GameplayDataSettings.QuestsData.QuestGiverInfo> questGiverInfos;

			// Token: 0x040023E9 RID: 9193
			[SerializeField]
			private string defaultQuestGiverDisplayName = "佚名";

			// Token: 0x020006A2 RID: 1698
			[Serializable]
			public class QuestGiverInfo
			{
				// Token: 0x17000810 RID: 2064
				// (get) Token: 0x06002BFA RID: 11258 RVA: 0x000A6DD9 File Offset: 0x000A4FD9
				public string DisplayName
				{
					get
					{
						return this.displayName;
					}
				}

				// Token: 0x04002447 RID: 9287
				public QuestGiverID id;

				// Token: 0x04002448 RID: 9288
				[SerializeField]
				private string displayName;
			}
		}

		// Token: 0x02000685 RID: 1669
		[Serializable]
		public class EconomyData
		{
			// Token: 0x17000804 RID: 2052
			// (get) Token: 0x06002BC3 RID: 11203 RVA: 0x000A5DD1 File Offset: 0x000A3FD1
			public ReadOnlyCollection<int> UnlockedItemByDefault
			{
				get
				{
					return this.unlockItemByDefault.AsReadOnly();
				}
			}

			// Token: 0x040023EA RID: 9194
			[SerializeField]
			[ItemTypeID]
			private List<int> unlockItemByDefault = new List<int>();
		}

		// Token: 0x02000686 RID: 1670
		[Serializable]
		public class UIStyleData
		{
			// Token: 0x17000805 RID: 2053
			// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x000A5DF1 File Offset: 0x000A3FF1
			public Sprite CritPopSprite
			{
				get
				{
					return this.critPopSprite;
				}
			}

			// Token: 0x17000806 RID: 2054
			// (get) Token: 0x06002BC6 RID: 11206 RVA: 0x000A5DF9 File Offset: 0x000A3FF9
			public Sprite DefaultTeleporterIcon
			{
				get
				{
					return this.defaultTeleporterIcon;
				}
			}

			// Token: 0x17000807 RID: 2055
			// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x000A5E01 File Offset: 0x000A4001
			public Sprite EleteCharacterIcon
			{
				get
				{
					return this.eleteCharacterIcon;
				}
			}

			// Token: 0x17000808 RID: 2056
			// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x000A5E09 File Offset: 0x000A4009
			public Sprite BossCharacterIcon
			{
				get
				{
					return this.bossCharacterIcon;
				}
			}

			// Token: 0x17000809 RID: 2057
			// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x000A5E11 File Offset: 0x000A4011
			public Sprite PmcCharacterIcon
			{
				get
				{
					return this.pmcCharacterIcon;
				}
			}

			// Token: 0x1700080A RID: 2058
			// (get) Token: 0x06002BCA RID: 11210 RVA: 0x000A5E19 File Offset: 0x000A4019
			public Sprite MerchantCharacterIcon
			{
				get
				{
					return this.merchantCharacterIcon;
				}
			}

			// Token: 0x1700080B RID: 2059
			// (get) Token: 0x06002BCB RID: 11211 RVA: 0x000A5E21 File Offset: 0x000A4021
			public Sprite PetCharacterIcon
			{
				get
				{
					return this.petCharacterIcon;
				}
			}

			// Token: 0x1700080C RID: 2060
			// (get) Token: 0x06002BCC RID: 11212 RVA: 0x000A5E29 File Offset: 0x000A4029
			public float TeleporterIconScale
			{
				get
				{
					return this.teleporterIconScale;
				}
			}

			// Token: 0x1700080D RID: 2061
			// (get) Token: 0x06002BCD RID: 11213 RVA: 0x000A5E31 File Offset: 0x000A4031
			public Sprite FallbackItemIcon
			{
				get
				{
					return this.fallbackItemIcon;
				}
			}

			// Token: 0x1700080E RID: 2062
			// (get) Token: 0x06002BCE RID: 11214 RVA: 0x000A5E39 File Offset: 0x000A4039
			public TextMeshProUGUI TemplateTextUGUI
			{
				get
				{
					return this.templateTextUGUI;
				}
			}

			// Token: 0x1700080F RID: 2063
			// (get) Token: 0x06002BCF RID: 11215 RVA: 0x000A5E41 File Offset: 0x000A4041
			[SerializeField]
			private TMP_Asset DefaultFont
			{
				get
				{
					return this.defaultFont;
				}
			}

			// Token: 0x06002BD0 RID: 11216 RVA: 0x000A5E4C File Offset: 0x000A404C
			[return: TupleElementNames(new string[]
			{
				"shadowOffset",
				"color",
				"innerGlow"
			})]
			public ValueTuple<float, Color, bool> GetShadowOffsetAndColorOfQuality(DisplayQuality displayQuality)
			{
				GameplayDataSettings.UIStyleData.DisplayQualityLook displayQualityLook = this.displayQualityLooks.Find((GameplayDataSettings.UIStyleData.DisplayQualityLook e) => e != null && e.quality == displayQuality);
				if (displayQualityLook == null)
				{
					return new ValueTuple<float, Color, bool>(this.defaultDisplayQualityShadowOffset, this.defaultDisplayQualityShadowColor, this.defaultDIsplayQualityShadowInnerGlow);
				}
				return new ValueTuple<float, Color, bool>(displayQualityLook.shadowOffset, displayQualityLook.shadowColor, displayQualityLook.innerGlow);
			}

			// Token: 0x06002BD1 RID: 11217 RVA: 0x000A5EB0 File Offset: 0x000A40B0
			public void ApplyDisplayQualityShadow(DisplayQuality displayQuality, TrueShadow target)
			{
				ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = this.GetShadowOffsetAndColorOfQuality(displayQuality);
				target.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
				target.Color = shadowOffsetAndColorOfQuality.Item2;
				target.Inset = shadowOffsetAndColorOfQuality.Item3;
			}

			// Token: 0x06002BD2 RID: 11218 RVA: 0x000A5EEC File Offset: 0x000A40EC
			public GameplayDataSettings.UIStyleData.DisplayQualityLook GetDisplayQualityLook(DisplayQuality q)
			{
				GameplayDataSettings.UIStyleData.DisplayQualityLook displayQualityLook = this.displayQualityLooks.Find((GameplayDataSettings.UIStyleData.DisplayQualityLook e) => e != null && e.quality == q);
				if (displayQualityLook == null)
				{
					return new GameplayDataSettings.UIStyleData.DisplayQualityLook
					{
						quality = q,
						shadowOffset = this.defaultDisplayQualityShadowOffset,
						shadowColor = this.defaultDisplayQualityShadowColor,
						innerGlow = this.defaultDIsplayQualityShadowInnerGlow
					};
				}
				return displayQualityLook;
			}

			// Token: 0x06002BD3 RID: 11219 RVA: 0x000A5F58 File Offset: 0x000A4158
			public GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook GetElementDamagePopTextLook(ElementTypes elementType)
			{
				GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook displayElementDamagePopTextLook = this.elementDamagePopTextLook.Find((GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook e) => e != null && e.elementType == elementType);
				if (displayElementDamagePopTextLook == null)
				{
					return new GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook
					{
						elementType = ElementTypes.physics,
						normalSize = 1f,
						critSize = 1.6f,
						color = Color.white
					};
				}
				return displayElementDamagePopTextLook;
			}

			// Token: 0x040023EB RID: 9195
			[SerializeField]
			private List<GameplayDataSettings.UIStyleData.DisplayQualityLook> displayQualityLooks = new List<GameplayDataSettings.UIStyleData.DisplayQualityLook>();

			// Token: 0x040023EC RID: 9196
			[SerializeField]
			private List<GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook> elementDamagePopTextLook = new List<GameplayDataSettings.UIStyleData.DisplayElementDamagePopTextLook>();

			// Token: 0x040023ED RID: 9197
			[SerializeField]
			private float defaultDisplayQualityShadowOffset = 8f;

			// Token: 0x040023EE RID: 9198
			[SerializeField]
			private Color defaultDisplayQualityShadowColor = Color.black;

			// Token: 0x040023EF RID: 9199
			[SerializeField]
			private bool defaultDIsplayQualityShadowInnerGlow;

			// Token: 0x040023F0 RID: 9200
			[SerializeField]
			private Sprite defaultTeleporterIcon;

			// Token: 0x040023F1 RID: 9201
			[SerializeField]
			private float teleporterIconScale = 0.5f;

			// Token: 0x040023F2 RID: 9202
			[SerializeField]
			private Sprite critPopSprite;

			// Token: 0x040023F3 RID: 9203
			[SerializeField]
			private Sprite fallbackItemIcon;

			// Token: 0x040023F4 RID: 9204
			[SerializeField]
			private Sprite eleteCharacterIcon;

			// Token: 0x040023F5 RID: 9205
			[SerializeField]
			private Sprite bossCharacterIcon;

			// Token: 0x040023F6 RID: 9206
			[SerializeField]
			private Sprite pmcCharacterIcon;

			// Token: 0x040023F7 RID: 9207
			[SerializeField]
			private Sprite merchantCharacterIcon;

			// Token: 0x040023F8 RID: 9208
			[SerializeField]
			private Sprite petCharacterIcon;

			// Token: 0x040023F9 RID: 9209
			[SerializeField]
			private TMP_Asset defaultFont;

			// Token: 0x040023FA RID: 9210
			[SerializeField]
			private TextMeshProUGUI templateTextUGUI;

			// Token: 0x020006A4 RID: 1700
			[Serializable]
			public class DisplayQualityLook
			{
				// Token: 0x06002BFE RID: 11262 RVA: 0x000A6E06 File Offset: 0x000A5006
				public void Apply(TrueShadow trueShadow)
				{
					trueShadow.OffsetDistance = this.shadowOffset;
					trueShadow.Color = this.shadowColor;
					trueShadow.Inset = this.innerGlow;
				}

				// Token: 0x0400244A RID: 9290
				public DisplayQuality quality;

				// Token: 0x0400244B RID: 9291
				public float shadowOffset;

				// Token: 0x0400244C RID: 9292
				public Color shadowColor;

				// Token: 0x0400244D RID: 9293
				public bool innerGlow;
			}

			// Token: 0x020006A5 RID: 1701
			[Serializable]
			public class DisplayElementDamagePopTextLook
			{
				// Token: 0x0400244E RID: 9294
				public ElementTypes elementType;

				// Token: 0x0400244F RID: 9295
				public float normalSize;

				// Token: 0x04002450 RID: 9296
				public float critSize;

				// Token: 0x04002451 RID: 9297
				public Color color;
			}
		}

		// Token: 0x02000687 RID: 1671
		[Serializable]
		public class SpritesData
		{
			// Token: 0x06002BD5 RID: 11221 RVA: 0x000A5FFC File Offset: 0x000A41FC
			public Sprite GetSprite(string key)
			{
				foreach (GameplayDataSettings.SpritesData.Entry entry in this.entries)
				{
					if (entry.key == key)
					{
						return entry.sprite;
					}
				}
				return null;
			}

			// Token: 0x040023FB RID: 9211
			public List<GameplayDataSettings.SpritesData.Entry> entries;

			// Token: 0x020006A9 RID: 1705
			[Serializable]
			public struct Entry
			{
				// Token: 0x04002455 RID: 9301
				public string key;

				// Token: 0x04002456 RID: 9302
				public Sprite sprite;
			}
		}

		// Token: 0x02000688 RID: 1672
		[Serializable]
		public class CharacterRandomPresets
		{
			// Token: 0x040023FC RID: 9212
			public List<CharacterRandomPreset> presets;
		}
	}
}
