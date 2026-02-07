using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Buffs;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000090 RID: 144
[CreateAssetMenu(fileName = "New Character Random Preset", menuName = "Character Random Preset", order = 51)]
public class CharacterRandomPreset : ScriptableObject
{
	// Token: 0x1700010F RID: 271
	// (get) Token: 0x0600050C RID: 1292 RVA: 0x00016D2F File Offset: 0x00014F2F
	public string Name
	{
		get
		{
			return LocalizationManager.ToPlainText(this.nameKey);
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x0600050D RID: 1293 RVA: 0x00016D3C File Offset: 0x00014F3C
	public string DisplayName
	{
		get
		{
			return LocalizationManager.ToPlainText(this.nameKey);
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x0600050E RID: 1294 RVA: 0x00016D49 File Offset: 0x00014F49
	private int characterItemTypeID
	{
		get
		{
			return GameplayDataSettings.ItemAssets.DefaultCharacterItemTypeID;
		}
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00016D58 File Offset: 0x00014F58
	public Sprite GetCharacterIcon()
	{
		switch (this.characterIconType)
		{
		case CharacterIconTypes.none:
			return null;
		case CharacterIconTypes.elete:
			return GameplayDataSettings.UIStyle.EleteCharacterIcon;
		case CharacterIconTypes.pmc:
			return GameplayDataSettings.UIStyle.PmcCharacterIcon;
		case CharacterIconTypes.boss:
			return GameplayDataSettings.UIStyle.BossCharacterIcon;
		case CharacterIconTypes.merchant:
			return GameplayDataSettings.UIStyle.MerchantCharacterIcon;
		case CharacterIconTypes.pet:
			return GameplayDataSettings.UIStyle.PetCharacterIcon;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x00016DCC File Offset: 0x00014FCC
	public UniTask<CharacterMainControl> CreateCharacterAsync(Vector3 pos, Vector3 dir, int relatedScene, CharacterSpawnerGroup group, bool isLeader)
	{
		CharacterRandomPreset.<CreateCharacterAsync>d__86 <CreateCharacterAsync>d__;
		<CreateCharacterAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateCharacterAsync>d__.<>4__this = this;
		<CreateCharacterAsync>d__.pos = pos;
		<CreateCharacterAsync>d__.dir = dir;
		<CreateCharacterAsync>d__.relatedScene = relatedScene;
		<CreateCharacterAsync>d__.group = group;
		<CreateCharacterAsync>d__.isLeader = isLeader;
		<CreateCharacterAsync>d__.<>1__state = -1;
		<CreateCharacterAsync>d__.<>t__builder.Start<CharacterRandomPreset.<CreateCharacterAsync>d__86>(ref <CreateCharacterAsync>d__);
		return <CreateCharacterAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00016E3C File Offset: 0x0001503C
	private UniTask<List<Item>> GenerateItems()
	{
		CharacterRandomPreset.<GenerateItems>d__87 <GenerateItems>d__;
		<GenerateItems>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<GenerateItems>d__.<>4__this = this;
		<GenerateItems>d__.<>1__state = -1;
		<GenerateItems>d__.<>t__builder.Start<CharacterRandomPreset.<GenerateItems>d__87>(ref <GenerateItems>d__);
		return <GenerateItems>d__.<>t__builder.Task;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00016E80 File Offset: 0x00015080
	private UniTask AddBullet(CharacterMainControl character)
	{
		CharacterRandomPreset.<AddBullet>d__88 <AddBullet>d__;
		<AddBullet>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<AddBullet>d__.<>4__this = this;
		<AddBullet>d__.character = character;
		<AddBullet>d__.<>1__state = -1;
		<AddBullet>d__.<>t__builder.Start<CharacterRandomPreset.<AddBullet>d__88>(ref <AddBullet>d__);
		return <AddBullet>d__.<>t__builder.Task;
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00017100 File Offset: 0x00015300
	[CompilerGenerated]
	internal static void <CreateCharacterAsync>g__SetCharacterStat|86_0(string statName, float value, ref CharacterRandomPreset.<>c__DisplayClass86_0 A_2)
	{
		Stat stat = A_2.characterItemInstance.GetStat(statName.GetHashCode());
		if (stat == null)
		{
			return;
		}
		stat.BaseValue = value;
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x0001712C File Offset: 0x0001532C
	[CompilerGenerated]
	internal static void <CreateCharacterAsync>g__MultiplyCharacterStat|86_1(string statName, float multiplier, ref CharacterRandomPreset.<>c__DisplayClass86_0 A_2)
	{
		Stat stat = A_2.characterItemInstance.GetStat(statName.GetHashCode());
		if (stat == null)
		{
			return;
		}
		stat.BaseValue *= multiplier;
	}

	// Token: 0x04000447 RID: 1095
	[LocalizationKey("Characters")]
	public string nameKey;

	// Token: 0x04000448 RID: 1096
	public AudioManager.VoiceType voiceType;

	// Token: 0x04000449 RID: 1097
	public AudioManager.FootStepMaterialType footstepMaterialType;

	// Token: 0x0400044A RID: 1098
	public bool dropBoxOnDead = true;

	// Token: 0x0400044B RID: 1099
	public InteractableLootbox lootBoxPrefab;

	// Token: 0x0400044C RID: 1100
	public List<AISpecialAttachmentBase> specialAttachmentBases;

	// Token: 0x0400044D RID: 1101
	public GameObject spawnFx;

	// Token: 0x0400044E RID: 1102
	public Teams team = Teams.scav;

	// Token: 0x0400044F RID: 1103
	public bool canDieIfNotRaidMap;

	// Token: 0x04000450 RID: 1104
	public bool showName;

	// Token: 0x04000451 RID: 1105
	[FormerlySerializedAs("iconType")]
	[SerializeField]
	private CharacterIconTypes characterIconType;

	// Token: 0x04000452 RID: 1106
	public float health;

	// Token: 0x04000453 RID: 1107
	public bool hasSoul = true;

	// Token: 0x04000454 RID: 1108
	public bool showHealthBar = true;

	// Token: 0x04000455 RID: 1109
	public int exp = 100;

	// Token: 0x04000456 RID: 1110
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x04000457 RID: 1111
	[SerializeField]
	private bool usePlayerPreset;

	// Token: 0x04000458 RID: 1112
	[SerializeField]
	private CustomFacePreset facePreset;

	// Token: 0x04000459 RID: 1113
	[SerializeField]
	private AICharacterController aiController;

	// Token: 0x0400045A RID: 1114
	[Tooltip("是否会挤压其他角色")]
	public bool pushCharacter;

	// Token: 0x0400045B RID: 1115
	public bool setActiveByPlayerDistance = true;

	// Token: 0x0400045C RID: 1116
	public float forceTracePlayerDistance;

	// Token: 0x0400045D RID: 1117
	public bool shootCanMove;

	// Token: 0x0400045E RID: 1118
	public float sightDistance = 17f;

	// Token: 0x0400045F RID: 1119
	public float sightAngle = 100f;

	// Token: 0x04000460 RID: 1120
	public float reactionTime = 0.2f;

	// Token: 0x04000461 RID: 1121
	public float nightReactionTimeFactor = 1.5f;

	// Token: 0x04000462 RID: 1122
	public float shootDelay = 0.2f;

	// Token: 0x04000463 RID: 1123
	public Vector2 shootTimeRange = new Vector2(0.4f, 1.5f);

	// Token: 0x04000464 RID: 1124
	public Vector2 shootTimeSpaceRange = new Vector2(2f, 3f);

	// Token: 0x04000465 RID: 1125
	public Vector2 combatMoveTimeRange = new Vector2(1f, 3f);

	// Token: 0x04000466 RID: 1126
	public float hearingAbility = 1f;

	// Token: 0x04000467 RID: 1127
	public float patrolRange = 8f;

	// Token: 0x04000468 RID: 1128
	[FormerlySerializedAs("combatRange")]
	public float combatMoveRange = 8f;

	// Token: 0x04000469 RID: 1129
	public bool canDash;

	// Token: 0x0400046A RID: 1130
	public Vector2 dashCoolTimeRange = new Vector2(2f, 4f);

	// Token: 0x0400046B RID: 1131
	[Range(0f, 1f)]
	public float minTraceTargetChance = 1f;

	// Token: 0x0400046C RID: 1132
	[Range(0f, 1f)]
	public float maxTraceTargetChance = 1f;

	// Token: 0x0400046D RID: 1133
	public float forgetTime = 8f;

	// Token: 0x0400046E RID: 1134
	public bool defaultWeaponOut = true;

	// Token: 0x0400046F RID: 1135
	public bool canTalk = true;

	// Token: 0x04000470 RID: 1136
	public float patrolTurnSpeed = 180f;

	// Token: 0x04000471 RID: 1137
	public float combatTurnSpeed = 1200f;

	// Token: 0x04000472 RID: 1138
	[ItemTypeID]
	public int wantItem = -1;

	// Token: 0x04000473 RID: 1139
	public float moveSpeedFactor = 1f;

	// Token: 0x04000474 RID: 1140
	public float bulletSpeedMultiplier = 1f;

	// Token: 0x04000475 RID: 1141
	[Range(1f, 2f)]
	public float gunDistanceMultiplier = 1f;

	// Token: 0x04000476 RID: 1142
	public float nightVisionAbility = 0.5f;

	// Token: 0x04000477 RID: 1143
	public float gunScatterMultiplier = 1f;

	// Token: 0x04000478 RID: 1144
	public float scatterMultiIfTargetRunning = 3f;

	// Token: 0x04000479 RID: 1145
	public float scatterMultiIfOffScreen = 4f;

	// Token: 0x0400047A RID: 1146
	[FormerlySerializedAs("gunDamageMultiplier")]
	public float damageMultiplier = 1f;

	// Token: 0x0400047B RID: 1147
	public float gunCritRateGain;

	// Token: 0x0400047C RID: 1148
	[Tooltip("用来决定双方造成伤害缩放")]
	public float aiCombatFactor = 1f;

	// Token: 0x0400047D RID: 1149
	public bool hasSkill;

	// Token: 0x0400047E RID: 1150
	public SkillBase skillPfb;

	// Token: 0x0400047F RID: 1151
	[Range(0.01f, 1f)]
	public float hasSkillChance = 1f;

	// Token: 0x04000480 RID: 1152
	public Vector2 skillCoolTimeRange = Vector2.one;

	// Token: 0x04000481 RID: 1153
	[Range(0.01f, 1f)]
	public float skillSuccessChance = 1f;

	// Token: 0x04000482 RID: 1154
	private float tryReleaseSkillTimeMarker = -1f;

	// Token: 0x04000483 RID: 1155
	[Range(0f, 1f)]
	public float itemSkillChance = 0.3f;

	// Token: 0x04000484 RID: 1156
	public float itemSkillCoolTime = 6f;

	// Token: 0x04000485 RID: 1157
	public List<Buff> buffs;

	// Token: 0x04000486 RID: 1158
	public List<Buff.BuffExclusiveTags> buffResist;

	// Token: 0x04000487 RID: 1159
	public float elementFactor_Physics = 1f;

	// Token: 0x04000488 RID: 1160
	public float elementFactor_Fire = 1f;

	// Token: 0x04000489 RID: 1161
	public float elementFactor_Ice = 1f;

	// Token: 0x0400048A RID: 1162
	public float elementFactor_Poison = 1f;

	// Token: 0x0400048B RID: 1163
	public float elementFactor_Electricity = 1f;

	// Token: 0x0400048C RID: 1164
	public float elementFactor_Space = 1f;

	// Token: 0x0400048D RID: 1165
	public float elementFactor_Ghost = 1f;

	// Token: 0x0400048E RID: 1166
	[SerializeField]
	private List<CharacterRandomPreset.SetCharacterStatInfo> setStats;

	// Token: 0x0400048F RID: 1167
	[Range(0f, 1f)]
	public float hasCashChance;

	// Token: 0x04000490 RID: 1168
	public Vector2Int cashRange;

	// Token: 0x04000491 RID: 1169
	[SerializeField]
	private List<RandomItemGenerateDescription> itemsToGenerate;

	// Token: 0x04000492 RID: 1170
	[Space(12f)]
	[SerializeField]
	private RandomContainer<int> bulletQualityDistribution;

	// Token: 0x04000493 RID: 1171
	[SerializeField]
	private Tag[] bulletExclusiveTags;

	// Token: 0x04000494 RID: 1172
	[HideInInspector]
	[SerializeField]
	private ItemFilter bulletFilter;

	// Token: 0x04000495 RID: 1173
	[SerializeField]
	private Vector2 bulletCountRange = Vector2.one;

	// Token: 0x0200045E RID: 1118
	[Serializable]
	private struct SetCharacterStatInfo
	{
		// Token: 0x04001B4C RID: 6988
		public string statName;

		// Token: 0x04001B4D RID: 6989
		public Vector2 statBaseValue;
	}
}
