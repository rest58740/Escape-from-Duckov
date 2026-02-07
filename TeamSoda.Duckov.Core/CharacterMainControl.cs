using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Duckov;
using Duckov.Buffs;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.UI.DialogueBubbles;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200005D RID: 93
public class CharacterMainControl : MonoBehaviour
{
	// Token: 0x17000072 RID: 114
	// (get) Token: 0x0600029C RID: 668 RVA: 0x0000BD74 File Offset: 0x00009F74
	// (set) Token: 0x0600029D RID: 669 RVA: 0x0000BD7C File Offset: 0x00009F7C
	public AudioManager.VoiceType AudioVoiceType
	{
		get
		{
			return this.audioVoiceType;
		}
		set
		{
			this.audioVoiceType = value;
			if (base.gameObject.activeInHierarchy)
			{
				AudioManager.SetVoiceType(base.gameObject, this.audioVoiceType);
			}
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x0600029E RID: 670 RVA: 0x0000BDA3 File Offset: 0x00009FA3
	// (set) Token: 0x0600029F RID: 671 RVA: 0x0000BDAB File Offset: 0x00009FAB
	public AudioManager.FootStepMaterialType FootStepMaterialType
	{
		get
		{
			return this.footStepMaterialType;
		}
		set
		{
			this.footStepMaterialType = value;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000BDB4 File Offset: 0x00009FB4
	public static CharacterMainControl Main
	{
		get
		{
			if (LevelManager.Instance == null)
			{
				return null;
			}
			return LevelManager.Instance.MainCharacter;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000BDCF File Offset: 0x00009FCF
	public Teams Team
	{
		get
		{
			return this.team;
		}
	}

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060002A2 RID: 674 RVA: 0x0000BDD8 File Offset: 0x00009FD8
	// (remove) Token: 0x060002A3 RID: 675 RVA: 0x0000BE10 File Offset: 0x0000A010
	public event Action<Teams> OnTeamChanged;

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000BE45 File Offset: 0x0000A045
	public Item CharacterItem
	{
		get
		{
			return this.characterItem;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000BE4D File Offset: 0x0000A04D
	public DuckovItemAgent CurrentHoldItemAgent
	{
		get
		{
			return this.agentHolder.CurrentHoldItemAgent;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000BE5A File Offset: 0x0000A05A
	public bool Hidden
	{
		get
		{
			return this.hidden;
		}
	}

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060002A7 RID: 679 RVA: 0x0000BE64 File Offset: 0x0000A064
	// (remove) Token: 0x060002A8 RID: 680 RVA: 0x0000BE9C File Offset: 0x0000A09C
	public event Action<CharacterMainControl, Vector3> OnSetPositionEvent;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x060002A9 RID: 681 RVA: 0x0000BED4 File Offset: 0x0000A0D4
	// (remove) Token: 0x060002AA RID: 682 RVA: 0x0000BF0C File Offset: 0x0000A10C
	public event Action<DamageInfo> BeforeCharacterSpawnLootOnDead;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x060002AB RID: 683 RVA: 0x0000BF44 File Offset: 0x0000A144
	// (remove) Token: 0x060002AC RID: 684 RVA: 0x0000BF7C File Offset: 0x0000A17C
	public event Action<bool, bool, bool> OnTriggerInputUpdateEvent;

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x060002AD RID: 685 RVA: 0x0000BFB1 File Offset: 0x0000A1B1
	public Transform CurrentUsingAimSocket
	{
		get
		{
			if (this.agentHolder.CurrentUsingSocket == null || this.GetMeleeWeapon() != null)
			{
				return base.transform;
			}
			return this.agentHolder.CurrentUsingSocket;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060002AE RID: 686 RVA: 0x0000BFE6 File Offset: 0x0000A1E6
	public Transform RightHandSocket
	{
		get
		{
			if (this.characterModel && this.characterModel.RightHandSocket)
			{
				return this.characterModel.RightHandSocket;
			}
			return null;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060002AF RID: 687 RVA: 0x0000C014 File Offset: 0x0000A214
	public Vector3 CurrentAimDirection
	{
		get
		{
			return this.modelRoot.forward;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000C021 File Offset: 0x0000A221
	public Vector3 CurrentMoveDirection
	{
		get
		{
			return this.movementControl.CurrentMoveDirectionXZ;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000C02E File Offset: 0x0000A22E
	public float AnimationMoveSpeedValue
	{
		get
		{
			return this.movementControl.GetMoveAnimationValue();
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000C03B File Offset: 0x0000A23B
	public Vector2 AnimationLocalMoveDirectionValue
	{
		get
		{
			return this.movementControl.GetLocalMoveDirectionAnimationValue();
		}
	}

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x060002B3 RID: 691 RVA: 0x0000C048 File Offset: 0x0000A248
	// (remove) Token: 0x060002B4 RID: 692 RVA: 0x0000C07C File Offset: 0x0000A27C
	public static event Action<Item> OnMainCharacterStartUseItem;

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000C0AF File Offset: 0x0000A2AF
	public bool Running
	{
		get
		{
			return this.movementControl.Running;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000C0BC File Offset: 0x0000A2BC
	public bool IsOnGround
	{
		get
		{
			return this.movementControl.IsOnGround;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000C0C9 File Offset: 0x0000A2C9
	public Vector3 Velocity
	{
		get
		{
			return this.movementControl.Velocity;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
	public bool ThermalOn
	{
		get
		{
			int num = Mathf.RoundToInt(this.NightVisionType);
			return GameManager.NightVision.nightVisionTypes[num].thermalOn;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000C106 File Offset: 0x0000A306
	public bool IsInAdsInput
	{
		get
		{
			return this.adsInput && (!(this.CurrentAction != null) || !this.CurrentAction.Running) && !this.Running;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060002BA RID: 698 RVA: 0x0000C13C File Offset: 0x0000A33C
	public float AdsValue
	{
		get
		{
			ItemAgent_Gun gun = this.GetGun();
			if (gun)
			{
				return gun.AdsValue;
			}
			if (this.CurrentAction != null && this.CurrentAction.Running)
			{
				return 0f;
			}
			if (this.Running)
			{
				return 0f;
			}
			return (float)(this.adsInput ? 1 : 0);
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060002BB RID: 699 RVA: 0x0000C19B File Offset: 0x0000A39B
	public AimTypes AimType
	{
		get
		{
			return this.aimType;
		}
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0000C1A4 File Offset: 0x0000A3A4
	public float GetAimRange()
	{
		float num = 8f;
		switch (this.aimType)
		{
		case AimTypes.normalAim:
		{
			ItemAgent_Gun gun = this.GetGun();
			if (gun != null)
			{
				num = gun.BulletDistance;
				num -= 0.4f;
			}
			else
			{
				ItemAgent_MeleeWeapon meleeWeapon = this.GetMeleeWeapon();
				if (meleeWeapon != null)
				{
					num = meleeWeapon.AttackRange;
				}
			}
			break;
		}
		case AimTypes.characterSkill:
		{
			SkillBase skill = this.skillAction.characterSkillKeeper.Skill;
			if (skill)
			{
				num = skill.SkillContext.castRange;
			}
			break;
		}
		case AimTypes.handheldSkill:
		{
			ItemSetting_Skill skill2 = this.agentHolder.Skill;
			if (skill2)
			{
				SkillBase skill3 = skill2.Skill;
				if (skill3)
				{
					num = skill3.SkillContext.castRange;
				}
			}
			break;
		}
		}
		return num;
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060002BD RID: 701 RVA: 0x0000C26E File Offset: 0x0000A46E
	public bool NeedToSearchTarget
	{
		get
		{
			return InputManager.InputDevice == InputManager.InputDevices.touch && (this.GetGun() || this.GetMeleeWeapon());
		}
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0000C297 File Offset: 0x0000A497
	public Vector3 GetCurrentAimPoint()
	{
		return this.inputAimPoint;
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0000C2A0 File Offset: 0x0000A4A0
	public Vector3 GetCurrentSkillAimPoint()
	{
		SkillBase currentRunningSkill = this.skillAction.CurrentRunningSkill;
		if (!currentRunningSkill)
		{
			return this.inputAimPoint;
		}
		float castRange = currentRunningSkill.SkillContext.castRange;
		float y = this.inputAimPoint.y;
		Vector3 vector = this.inputAimPoint - base.transform.position;
		vector.y = 0f;
		float num = vector.magnitude;
		vector.Normalize();
		if (num > castRange)
		{
			num = castRange;
		}
		Vector3 result = base.transform.position + vector * num;
		result.y = y;
		return result;
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000C33F File Offset: 0x0000A53F
	public CharacterActionBase CurrentAction
	{
		get
		{
			return this.currentAction;
		}
	}

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x060002C1 RID: 705 RVA: 0x0000C348 File Offset: 0x0000A548
	// (remove) Token: 0x060002C2 RID: 706 RVA: 0x0000C380 File Offset: 0x0000A580
	public event Action<CharacterActionBase> OnActionStartEvent;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x060002C3 RID: 707 RVA: 0x0000C3B8 File Offset: 0x0000A5B8
	// (remove) Token: 0x060002C4 RID: 708 RVA: 0x0000C3F0 File Offset: 0x0000A5F0
	public event Action<CharacterActionBase> OnActionProgressFinishEvent;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x060002C5 RID: 709 RVA: 0x0000C428 File Offset: 0x0000A628
	// (remove) Token: 0x060002C6 RID: 710 RVA: 0x0000C460 File Offset: 0x0000A660
	public event Action<DuckovItemAgent> OnHoldAgentChanged;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x060002C7 RID: 711 RVA: 0x0000C498 File Offset: 0x0000A698
	// (remove) Token: 0x060002C8 RID: 712 RVA: 0x0000C4D0 File Offset: 0x0000A6D0
	public event Action<DuckovItemAgent> OnShootEvent;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x060002C9 RID: 713 RVA: 0x0000C508 File Offset: 0x0000A708
	// (remove) Token: 0x060002CA RID: 714 RVA: 0x0000C540 File Offset: 0x0000A740
	public event Action TryCatchFishInputEvent;

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060002CB RID: 715 RVA: 0x0000C575 File Offset: 0x0000A775
	public Health Health
	{
		get
		{
			return this.health;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060002CC RID: 716 RVA: 0x0000C57D File Offset: 0x0000A77D
	public Vector3 MoveInput
	{
		get
		{
			return this.movementControl.MoveInput;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060002CD RID: 717 RVA: 0x0000C58A File Offset: 0x0000A78A
	public CharacterEquipmentController EquipmentController
	{
		get
		{
			return this.equipmentController;
		}
	}

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x060002CE RID: 718 RVA: 0x0000C594 File Offset: 0x0000A794
	// (remove) Token: 0x060002CF RID: 719 RVA: 0x0000C5CC File Offset: 0x0000A7CC
	public event Action<DuckovItemAgent> OnAttackEvent;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x060002D0 RID: 720 RVA: 0x0000C604 File Offset: 0x0000A804
	// (remove) Token: 0x060002D1 RID: 721 RVA: 0x0000C63C File Offset: 0x0000A83C
	public event Action OnSkillStartReleaseEvent;

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000C671 File Offset: 0x0000A871
	public bool Dashing
	{
		get
		{
			return this.dashAction != null && this.dashAction.Running;
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x0000C68E File Offset: 0x0000A88E
	private void OnMainCharacterInventoryChanged(Inventory inventory, int index)
	{
		ItemUtilities.NotifyPlayerItemOperation();
		Action<CharacterMainControl, Inventory, int> onMainCharacterInventoryChangedEvent = CharacterMainControl.OnMainCharacterInventoryChangedEvent;
		if (onMainCharacterInventoryChangedEvent == null)
		{
			return;
		}
		onMainCharacterInventoryChangedEvent(this, inventory, index);
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0000C6A7 File Offset: 0x0000A8A7
	private void OnMainCharacterSlotContentChanged(Item item, Slot slot)
	{
		Action<CharacterMainControl, Slot> onMainCharacterSlotContentChangedEvent = CharacterMainControl.OnMainCharacterSlotContentChangedEvent;
		if (onMainCharacterSlotContentChangedEvent != null)
		{
			onMainCharacterSlotContentChangedEvent(this, slot);
		}
		this.CheckTakeoutWeaponWhileEquip(slot);
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0000C6C4 File Offset: 0x0000A8C4
	private void CheckTakeoutWeaponWhileEquip(Slot slot)
	{
		if (slot.Content == null)
		{
			return;
		}
		if (this.CurrentHoldItemAgent != null)
		{
			return;
		}
		if (!slot.Content.Tags.Contains("Weapon"))
		{
			return;
		}
		this.agentHolder.ChangeHoldItem(slot.Content);
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0000C71C File Offset: 0x0000A91C
	public void SwitchWeapon(int dir)
	{
		this.weaponsTemp.Clear();
		this.weaponSwitchIndex = -1;
		Item item = null;
		if (this.CurrentHoldItemAgent != null)
		{
			item = this.CurrentHoldItemAgent.Item;
		}
		Item content = this.PrimWeaponSlot().Content;
		if (content)
		{
			this.weaponsTemp.Add(content);
			if (item == content)
			{
				this.weaponSwitchIndex = this.weaponsTemp.Count - 1;
			}
		}
		content = this.SecWeaponSlot().Content;
		if (content)
		{
			this.weaponsTemp.Add(content);
			if (item == content)
			{
				this.weaponSwitchIndex = this.weaponsTemp.Count - 1;
			}
		}
		content = this.MeleeWeaponSlot().Content;
		if (content)
		{
			this.weaponsTemp.Add(content);
			if (item == content)
			{
				this.weaponSwitchIndex = this.weaponsTemp.Count - 1;
			}
		}
		if (this.weaponsTemp.Count <= 0)
		{
			return;
		}
		this.weaponSwitchIndex -= dir;
		if (this.weaponSwitchIndex < 0)
		{
			this.weaponSwitchIndex = this.weaponsTemp.Count - 1;
		}
		if (this.weaponSwitchIndex >= this.weaponsTemp.Count)
		{
			this.weaponSwitchIndex = 0;
		}
		this.ChangeHoldItem(this.weaponsTemp[this.weaponSwitchIndex]);
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000C876 File Offset: 0x0000AA76
	public bool IsMainCharacter
	{
		get
		{
			return !(LevelManager.Instance == null) && LevelManager.Instance.MainCharacter == this;
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000C897 File Offset: 0x0000AA97
	public bool CanEditInventory()
	{
		return !this.currentAction || !this.currentAction.Running || this.currentAction.CanEditInventory();
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0000C8C3 File Offset: 0x0000AAC3
	public void SetMoveInput(Vector3 moveInput)
	{
		this.movementControl.SetMoveInput(moveInput);
	}

	// Token: 0x060002DA RID: 730 RVA: 0x0000C8D1 File Offset: 0x0000AAD1
	public Slot MeleeWeaponSlot()
	{
		return this.GetSlot(this.meleeWeaponSlotHash);
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0000C8DF File Offset: 0x0000AADF
	public Slot PrimWeaponSlot()
	{
		return this.GetSlot(this.primWeaponSlotHash);
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0000C8ED File Offset: 0x0000AAED
	public Slot SecWeaponSlot()
	{
		return this.GetSlot(this.secWeaponSlotHash);
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0000C8FB File Offset: 0x0000AAFB
	public Slot GetSlot(int hash)
	{
		if (this.characterItem == null)
		{
			return null;
		}
		return this.characterItem.Slots.GetSlot(hash);
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0000C91E File Offset: 0x0000AB1E
	private void Awake()
	{
		this.nearByHalfObsticles = new HashSet<GameObject>();
		this.agentHolder.OnHoldAgentChanged += this.OnChangeItemAgentChangedFunc;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x0000C944 File Offset: 0x0000AB44
	private void StoreHoldWeaponBeforeUse()
	{
		if (this.agentHolder.CurrentHoldItemAgent)
		{
			Item item = this.agentHolder.CurrentHoldItemAgent.Item;
			if (item == this.MeleeWeaponSlot().Content)
			{
				this.holdWeaponBeforeUse = -1;
				return;
			}
			if (item == this.PrimWeaponSlot().Content)
			{
				this.holdWeaponBeforeUse = 0;
				return;
			}
			if (item == this.SecWeaponSlot().Content)
			{
				this.holdWeaponBeforeUse = 1;
				return;
			}
		}
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0000C9C5 File Offset: 0x0000ABC5
	public bool SwitchToFirstAvailableWeapon()
	{
		return this.SwitchToWeapon(0) || this.SwitchToWeapon(1) || this.SwitchToWeapon(-1);
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0000C9E4 File Offset: 0x0000ABE4
	public bool SwitchToWeapon(int index)
	{
		Slot slot = null;
		if (index == -1)
		{
			slot = this.MeleeWeaponSlot();
		}
		if (index == 0)
		{
			slot = this.PrimWeaponSlot();
		}
		else if (index == 1)
		{
			slot = this.SecWeaponSlot();
		}
		if (slot == null)
		{
			return false;
		}
		Item content = slot.Content;
		if (content == null)
		{
			return false;
		}
		this.ChangeHoldItem(content);
		return true;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0000CA38 File Offset: 0x0000AC38
	public void ToggleNightVision()
	{
		Item faceMaskItem = this.GetFaceMaskItem();
		if (!faceMaskItem)
		{
			return;
		}
		ItemSetting_NightVision component = faceMaskItem.GetComponent<ItemSetting_NightVision>();
		if (!component)
		{
			return;
		}
		component.ToggleNightVison();
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x0000CA6C File Offset: 0x0000AC6C
	public void Dash()
	{
		if (this.dashAction == null)
		{
			return;
		}
		if (this.attackAction.Running && !this.attackAction.DamageDealed)
		{
			return;
		}
		if (this.StartAction(this.dashAction) && !this.DashCanControl && this.disableTriggerTimer < 0.6f)
		{
			this.disableTriggerTimer = 0.6f;
		}
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0000CAD4 File Offset: 0x0000ACD4
	public void TryCatchFishInput()
	{
		if (!this.currentAction || !this.currentAction.Running)
		{
			return;
		}
		Action_FishingV2 action_FishingV = this.currentAction as Action_FishingV2;
		if (action_FishingV)
		{
			action_FishingV.TryCatch();
		}
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0000CB18 File Offset: 0x0000AD18
	public bool HasNearByHalfObsticle()
	{
		if (this.nearByHalfObsticles.Count <= 0)
		{
			return false;
		}
		using (HashSet<GameObject>.Enumerator enumerator = this.nearByHalfObsticles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x0000CB84 File Offset: 0x0000AD84
	public void SwitchToWeaponBeforeUse()
	{
		this.SwitchToWeapon(this.holdWeaponBeforeUse);
		this.holdWeaponBeforeUse = -1;
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0000CB9A File Offset: 0x0000AD9A
	public void SetForceMoveVelocity(Vector3 _velocity)
	{
		this.movementControl.SetForceMoveVelocity(_velocity);
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0000CBA8 File Offset: 0x0000ADA8
	public void SetAimPoint(Vector3 _aimPoint)
	{
		this.inputAimPoint = _aimPoint;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0000CBB4 File Offset: 0x0000ADB4
	public bool Attack()
	{
		if (this.GetMeleeWeapon() == null)
		{
			return false;
		}
		if (!this.attackAction.IsReady())
		{
			return false;
		}
		bool result = this.StartAction(this.attackAction);
		Action<DuckovItemAgent> onAttackEvent = this.OnAttackEvent;
		if (onAttackEvent == null)
		{
			return result;
		}
		onAttackEvent(this.GetMeleeWeapon());
		return result;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0000CC02 File Offset: 0x0000AE02
	public void SetAimType(AimTypes _aimType)
	{
		this.aimType = _aimType;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0000CC0B File Offset: 0x0000AE0B
	public void SetRunInput(bool _runInput)
	{
		this.runInput = _runInput;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0000CC14 File Offset: 0x0000AE14
	public void SetAdsInput(bool _adsInput)
	{
		this.adsInput = _adsInput;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0000CC1D File Offset: 0x0000AE1D
	public bool TryToReload(Item preferedBulletToLoad = null)
	{
		this.reloadAction.preferedBulletToReload = preferedBulletToLoad;
		bool flag = this.StartAction(this.reloadAction);
		if (!flag)
		{
			this.reloadAction.preferedBulletToReload = null;
		}
		return flag;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0000CC46 File Offset: 0x0000AE46
	public bool SetSkill(SkillTypes skillType, SkillBase skill, GameObject bindingObject)
	{
		return this.skillAction.SetSkillOfType(skillType, skill, bindingObject);
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0000CC56 File Offset: 0x0000AE56
	public bool StartSkillAim(SkillTypes skillType)
	{
		if (this.skillAction.Running)
		{
			return false;
		}
		this.skillAction.SetNextSkillType(skillType);
		return this.StartAction(this.skillAction);
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0000CC7F File Offset: 0x0000AE7F
	public bool ReleaseSkill(SkillTypes skillType)
	{
		Action onSkillStartReleaseEvent = this.OnSkillStartReleaseEvent;
		if (onSkillStartReleaseEvent != null)
		{
			onSkillStartReleaseEvent();
		}
		return this.skillAction.ReleaseSkill(skillType);
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0000CC9E File Offset: 0x0000AE9E
	public bool CancleSkill()
	{
		return this.skillAction.StopAction();
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0000CCAB File Offset: 0x0000AEAB
	public SkillBase GetCurrentRunningSkill()
	{
		if (!this.skillAction.Running)
		{
			return null;
		}
		return this.skillAction.CurrentRunningSkill;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0000CCC7 File Offset: 0x0000AEC7
	public bool GetGunReloadable()
	{
		return this.reloadAction.GetGunReloadable();
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0000CCD4 File Offset: 0x0000AED4
	public bool CanUseHand()
	{
		return !(this.currentAction != null) || this.currentAction.CanUseHand();
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0000CCF4 File Offset: 0x0000AEF4
	public bool CanControlAim()
	{
		return !(this.currentAction != null) || this.currentAction.CanControlAim();
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0000CD14 File Offset: 0x0000AF14
	public bool StartAction(CharacterActionBase newAction)
	{
		if (!newAction.IsReady())
		{
			return false;
		}
		bool flag = true;
		if (this.currentAction && this.currentAction.Running)
		{
			flag = (newAction.ActionPriority() > this.currentAction.ActionPriority() && this.currentAction.StopAction());
		}
		if (flag)
		{
			this.currentAction = null;
			if (newAction.StartActionByCharacter(this))
			{
				this.currentAction = newAction;
				Action<CharacterActionBase> onActionStartEvent = this.OnActionStartEvent;
				if (onActionStartEvent != null)
				{
					onActionStartEvent(this.currentAction);
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0000CD9E File Offset: 0x0000AF9E
	public void SwitchHoldAgentInSlot(int slotHash)
	{
		Slot slot = this.characterItem.Slots.GetSlot(slotHash);
		this.ChangeHoldItem((slot != null) ? slot.Content : null);
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
	public void SwitchInteractSelection(int dir)
	{
		this.interactAction.SwitchInteractable(dir);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x0000CDD4 File Offset: 0x0000AFD4
	public void SetTeam(Teams _team)
	{
		this.team = _team;
		this.health.team = this.team;
		Action<Teams> onTeamChanged = this.OnTeamChanged;
		if (onTeamChanged != null)
		{
			onTeamChanged(_team);
		}
		if (CharacterMainControl.Main == this)
		{
			this.characterItem.Inventory.onContentChanged -= this.OnMainCharacterInventoryChanged;
			this.characterItem.Inventory.onContentChanged += this.OnMainCharacterInventoryChanged;
			this.characterItem.onSlotContentChanged -= this.OnMainCharacterSlotContentChanged;
			this.characterItem.onSlotContentChanged += this.OnMainCharacterSlotContentChanged;
		}
		if (this.characterModel)
		{
			this.characterModel.SyncHiddenToMainCharacter();
		}
	}

	// Token: 0x060002FA RID: 762 RVA: 0x0000CE96 File Offset: 0x0000B096
	public ItemAgent_Gun GetGun()
	{
		return this.agentHolder.CurrentHoldGun;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0000CEA3 File Offset: 0x0000B0A3
	public ItemAgent_MeleeWeapon GetMeleeWeapon()
	{
		return this.agentHolder.CurrentHoldMeleeWeapon;
	}

	// Token: 0x060002FC RID: 764 RVA: 0x0000CEB0 File Offset: 0x0000B0B0
	public bool ChangeHoldItem(Item item)
	{
		if (!this.CanEditInventory())
		{
			return false;
		}
		if (this.agentHolder.CurrentHoldItemAgent != null && item == this.agentHolder.CurrentHoldItemAgent.Item)
		{
			return false;
		}
		this.agentHolder.ChangeHoldItem(item);
		return true;
	}

	// Token: 0x060002FD RID: 765 RVA: 0x0000CF04 File Offset: 0x0000B104
	public void Quack()
	{
		AudioManager.Post("Char/Voice/PlayerQuak", base.gameObject);
		AIMainBrain.MakeSound(new AISound
		{
			fromCharacter = this,
			fromObject = base.gameObject,
			pos = base.transform.position,
			fromTeam = this.Team,
			soundType = SoundTypes.unknowNoise,
			radius = 15f
		});
		HardwareSyncingManager.SetEvent("Quack");
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0000CF82 File Offset: 0x0000B182
	private void OnChangeItemAgentChangedFunc(DuckovItemAgent agent)
	{
		Action<DuckovItemAgent> onHoldAgentChanged = this.OnHoldAgentChanged;
		if (onHoldAgentChanged != null)
		{
			onHoldAgentChanged(agent);
		}
		if (this.IsMainCharacter)
		{
			Action<CharacterMainControl, DuckovItemAgent> onMainCharacterChangeHoldItemAgentEvent = CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent;
			if (onMainCharacterChangeHoldItemAgentEvent == null)
			{
				return;
			}
			onMainCharacterChangeHoldItemAgentEvent(this, agent);
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0000CFB0 File Offset: 0x0000B1B0
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		this.interactAction.SearchInteractableAround();
		this.UpdateAction(Time.deltaTime);
		this.movementControl.UpdateMovement();
		this.UpdateStats(Time.deltaTime);
		this.TickVariables(Time.deltaTime, 1f);
		if (this.IsMainCharacter)
		{
			this.UpdateThirstyAndStarve();
			this.UpdateWeightState();
			this.UpdateCold();
		}
		this.disableTriggerTimer -= Time.deltaTime;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0000D02D File Offset: 0x0000B22D
	private void LateUpdate()
	{
		this.UpdateInventoryCapacity();
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0000D038 File Offset: 0x0000B238
	public void SetItem(Item _item)
	{
		if (_item == null)
		{
			return;
		}
		this.characterItem = _item;
		_item.transform.SetParent(base.transform, false);
		this.currentStamina = this.MaxStamina;
		this.health.SetItemAndCharacter(_item, this);
		this.health.OnDeadEvent.AddListener(new UnityAction<DamageInfo>(this.OnDead));
		this.equipmentController.SetItem(_item);
		_item.Inventory.SetCapacity(Mathf.RoundToInt(this.InventoryCapacity));
		this.health.Init();
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0000D0CC File Offset: 0x0000B2CC
	private void UpdateInventoryCapacity()
	{
		if (LevelManager.Instance.MainCharacter != this)
		{
			return;
		}
		if (this.characterItem == null || this.characterItem.Inventory == null || this.characterItem.Inventory.Loading)
		{
			return;
		}
		int num = Mathf.RoundToInt(this.InventoryCapacity);
		int capacity = this.characterItem.Inventory.Capacity;
		if (capacity == num)
		{
			return;
		}
		this.characterItem.Inventory.SetCapacity(num);
		if (capacity > num)
		{
			int count = this.characterItem.Inventory.Content.Count;
			if (count >= num)
			{
				List<Item> list = new List<Item>();
				for (int i = num; i < count; i++)
				{
					Item item = this.characterItem.Inventory.Content[i];
					if (item != null)
					{
						list.Add(item);
						item.Detach();
					}
				}
				foreach (Item item2 in list)
				{
					if (!this.characterItem.Inventory.AddAndMerge(item2, 0))
					{
						item2.Drop(base.transform.position, true, Vector3.forward, 360f);
					}
				}
			}
		}
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0000D230 File Offset: 0x0000B430
	private void OnDead(DamageInfo dmgInfo)
	{
		if (LevelManager.Instance.MainCharacter != this)
		{
			Quaternion rotation = Quaternion.identity;
			if (this.characterModel)
			{
				rotation = this.characterModel.transform.rotation;
			}
			if (dmgInfo.fromCharacter && dmgInfo.fromCharacter.IsMainCharacter && this.characterPreset && this.characterPreset.nameKey != "")
			{
				SavesCounter.AddKillCount(this.characterPreset.nameKey);
			}
			Action<DamageInfo> beforeCharacterSpawnLootOnDead = this.BeforeCharacterSpawnLootOnDead;
			if (beforeCharacterSpawnLootOnDead != null)
			{
				beforeCharacterSpawnLootOnDead(dmgInfo);
			}
			if (this.dropBoxOnDead)
			{
				InteractableLootbox.CreateFromItem(this.characterItem, base.transform.position + Vector3.up * 0.1f, rotation, true, this.deadLootBoxPrefab, this.IsMainCharacter);
			}
		}
		if (this.relatedScene != -1)
		{
			SetActiveByPlayerDistance.Unregister(base.gameObject, this.relatedScene);
		}
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0000D334 File Offset: 0x0000B534
	public void Trigger(bool trigger, bool triggerThisFrame, bool releaseThisFrame)
	{
		if (this.Running || this.disableTriggerTimer > 0f)
		{
			trigger = false;
			triggerThisFrame = false;
		}
		else if (trigger && this.CharacterMoveability > 0.5f)
		{
			this.movementControl.ForceSetAimDirectionToAimPoint();
		}
		Action<bool, bool, bool> onTriggerInputUpdateEvent = this.OnTriggerInputUpdateEvent;
		if (onTriggerInputUpdateEvent != null)
		{
			onTriggerInputUpdateEvent(trigger, triggerThisFrame, releaseThisFrame);
		}
		this.agentHolder.SetTrigger(trigger, triggerThisFrame, releaseThisFrame);
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0000D39B File Offset: 0x0000B59B
	public bool CanMove()
	{
		return (!(this.currentAction != null) || this.currentAction.CanMove()) && this.CharacterWalkSpeed > 0f;
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0000D3CC File Offset: 0x0000B5CC
	public void PopText(string text, float speed = -1f)
	{
		if (!LevelManager.LevelInited || !CharacterMainControl.Main)
		{
			return;
		}
		if (Vector3.Distance(base.transform.position, CharacterMainControl.Main.transform.position) > 55f)
		{
			return;
		}
		float yOffset = 2f;
		if (this.characterModel && this.characterModel.HelmatSocket)
		{
			yOffset = Vector3.Distance(base.transform.position, this.characterModel.HelmatSocket.position) + 0.5f;
		}
		DialogueBubblesManager.Show(text, base.transform, yOffset, false, false, speed, 2f).Forget();
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0000D47C File Offset: 0x0000B67C
	public bool CanRun()
	{
		if (this.currentAction != null && !this.currentAction.CanRun())
		{
			return false;
		}
		float num = this.currentStamina / this.MaxStamina;
		return (num >= 0.2f || this.Running) && num > 0f && this.runInput;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0000D4D6 File Offset: 0x0000B6D6
	public bool IsAiming()
	{
		return !this.movementControl.Running && (this.currentAction == null || !this.currentAction.Running || this.currentAction.CanControlAim());
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0000D510 File Offset: 0x0000B710
	public void DestroyCharacter()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0000D51D File Offset: 0x0000B71D
	public void TriggerShootEvent(DuckovItemAgent shootByAgent)
	{
		Action<DuckovItemAgent> onShootEvent = this.OnShootEvent;
		if (onShootEvent == null)
		{
			return;
		}
		onShootEvent(shootByAgent);
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0000D530 File Offset: 0x0000B730
	public void SetCharacterModel(CharacterModel _characterModel)
	{
		bool flag = true;
		if (this.characterModel != null)
		{
			flag = false;
			Object.Destroy(this.characterModel.gameObject);
		}
		this.characterModel = _characterModel;
		_characterModel.OnMainCharacterSetted(this);
		_characterModel.transform.SetParent(this.modelRoot, false);
		_characterModel.transform.localPosition = Vector3.zero;
		_characterModel.transform.localRotation = quaternion.identity;
		Transform helmatSocket = _characterModel.HelmatSocket;
		if (helmatSocket)
		{
			HeadCollider headCollider = Object.Instantiate<HeadCollider>(GameplayDataSettings.Prefabs.HeadCollider, helmatSocket);
			headCollider.transform.localPosition = Vector3.zero;
			headCollider.transform.localScale = Vector3.one;
			headCollider.Init(this);
			CapsuleCollider component = this.mainDamageReceiver.GetComponent<CapsuleCollider>();
			if (component)
			{
				float num = headCollider.transform.localScale.y * 0.5f + headCollider.transform.position.y - base.transform.position.y + 0.5f;
				component.height = num;
				component.center = Vector3.up * num * 0.5f;
			}
		}
		if (LevelManager.LevelInited && !flag && this.characterItem != null)
		{
			foreach (Slot slot in this.characterItem.Slots)
			{
				if (slot.Content)
				{
					slot.ForceInvokeSlotContentChangedEvent();
				}
			}
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0000D6D8 File Offset: 0x0000B8D8
	private void OnDestroy()
	{
		if (this.characterItem && this.characterItem.Inventory)
		{
			this.characterItem.Inventory.onContentChanged -= this.OnMainCharacterInventoryChanged;
		}
		if (this.characterItem)
		{
			ItemTreeExtensions.DestroyTree(this.characterItem);
		}
		if (this.health)
		{
			this.health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnDead));
		}
		if (this.relatedScene != -1)
		{
			SetActiveByPlayerDistance.Unregister(base.gameObject, this.relatedScene);
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0000D77C File Offset: 0x0000B97C
	private void UpdateAction(float deltaTime)
	{
		if (this.currentAction)
		{
			this.currentAction.UpdateAction(deltaTime);
			if (this.currentAction && !this.currentAction.Running)
			{
				this.currentAction = null;
			}
		}
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0000D7B8 File Offset: 0x0000B9B8
	private void UpdateStats(float deltaTime)
	{
		if (this.movementControl.Running)
		{
			this.UseStamina(this.StaminaDrainRate * deltaTime);
			return;
		}
		this.staminaRecoverTimer += deltaTime;
		if (this.staminaRecoverTimer >= this.StaminaRecoverTime)
		{
			this.currentStamina = Mathf.MoveTowards(this.currentStamina, this.MaxStamina, this.StaminaRecoverRate * deltaTime);
		}
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0000D81C File Offset: 0x0000BA1C
	public void TickVariables(float deltaTime, float tickTime)
	{
		this.variableTickTimer += deltaTime;
		if (this.variableTickTimer < tickTime)
		{
			return;
		}
		this.variableTickTimer = 0f;
		if (!this.IsMainCharacter)
		{
			return;
		}
		float num = this.CurrentEnergy;
		if (!LevelManager.Instance.IsRaidMap || this.health.Invincible)
		{
			num += 10f * this.WaterEnergyRecoverMultiplier * tickTime / 60f;
			if (num < this.MaxEnergy * 0.25f)
			{
				num = this.MaxEnergy * 0.25f;
			}
			else if (num > this.MaxEnergy)
			{
				num = this.MaxEnergy;
			}
		}
		else
		{
			num -= this.EnergyCostPerMin * tickTime / 60f;
			if (num < 0f)
			{
				num = 0f;
			}
		}
		this.CurrentEnergy = num;
		float num2 = this.CurrentWater;
		if (!LevelManager.Instance.IsRaidMap || this.health.Invincible)
		{
			num2 += 10f * this.WaterEnergyRecoverMultiplier * tickTime / 60f;
			if (num2 < this.MaxWater * 0.25f)
			{
				num2 = this.MaxWater * 0.25f;
			}
			else if (num2 > this.MaxWater)
			{
				num2 = this.MaxWater;
			}
		}
		else
		{
			num2 -= this.WaterCostPerMin * tickTime / 60f;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
		}
		this.CurrentWater = num2;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0000D970 File Offset: 0x0000BB70
	public void UpdateCold()
	{
		this.coldTickTimer -= Time.deltaTime;
		this.coldDialogTimer -= Time.deltaTime;
		if (this.coldTickTimer > 0f)
		{
			return;
		}
		float coldLevel = TimeOfDayController.coldLevel;
		float coldProtection = this.ColdProtection;
		bool flag = coldLevel - coldProtection > 0.1f;
		if (this.health.Invincible)
		{
			flag = false;
		}
		if (flag && coldLevel > 3.9f)
		{
			this.coldTickTimer = 0.25f;
			this.AddBuff(GameplayDataSettings.Buffs.SuperCold, null, 0);
			this.AddBuff(GameplayDataSettings.Buffs.Cold, null, 0);
			this.<UpdateCold>g__TryPopColdText|206_0(true);
			return;
		}
		if (flag)
		{
			this.coldTickTimer = 1f / Mathf.Max(1f, coldLevel - coldProtection);
			this.AddBuff(GameplayDataSettings.Buffs.Cold, null, 0);
			this.<UpdateCold>g__TryPopColdText|206_0(false);
			return;
		}
		this.coldTickTimer = 0.5f / Mathf.Max(1f, coldProtection - coldLevel + 1f);
		this.coldDialogTimer = Mathf.Max(this.coldDialogTimer, 2f);
		this.RemoveBuff(GameplayDataSettings.Buffs.Cold.ID, true);
		this.RemoveBuff(GameplayDataSettings.Buffs.Cold.ID, true);
		this.RemoveBuff(GameplayDataSettings.Buffs.SuperCold.ID, true);
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0000DACC File Offset: 0x0000BCCC
	public void UpdateThirstyAndStarve()
	{
		if (this.CurrentWater <= 0f != this.thirsty)
		{
			this.thirsty = !this.thirsty;
			if (this.thirsty)
			{
				this.AddBuff(GameplayDataSettings.Buffs.Thirsty, this, 0);
			}
			else
			{
				this.RemoveBuffsByTag(Buff.BuffExclusiveTags.Thirsty, false);
			}
		}
		if (this.CurrentEnergy <= 0f != this.starve)
		{
			this.starve = !this.starve;
			if (this.starve)
			{
				this.AddBuff(GameplayDataSettings.Buffs.Starve, this, 0);
				return;
			}
			this.RemoveBuffsByTag(Buff.BuffExclusiveTags.Starve, false);
		}
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0000DB70 File Offset: 0x0000BD70
	public void UpdateWeightState()
	{
		float num = this.CharacterItem.TotalWeight;
		if (this.carryAction.Running)
		{
			num += this.carryAction.GetWeight();
		}
		float num2 = num / this.MaxWeight;
		CharacterMainControl.WeightStates weightStates = CharacterMainControl.WeightStates.light;
		if (!LevelManager.Instance.IsRaidMap)
		{
			weightStates = CharacterMainControl.WeightStates.normal;
		}
		else if (num2 > 1f)
		{
			weightStates = CharacterMainControl.WeightStates.overWeight;
		}
		else if (num2 > 0.75f)
		{
			weightStates = CharacterMainControl.WeightStates.superHeavy;
		}
		else if (num2 > 0.25f)
		{
			weightStates = CharacterMainControl.WeightStates.normal;
		}
		if (weightStates != this.weightState)
		{
			this.weightState = weightStates;
			this.RemoveBuffsByTag(Buff.BuffExclusiveTags.Weight, false);
			switch (weightStates)
			{
			case CharacterMainControl.WeightStates.light:
				this.AddBuff(GameplayDataSettings.Buffs.Weight_Light, this, 0);
				return;
			case CharacterMainControl.WeightStates.normal:
				break;
			case CharacterMainControl.WeightStates.heavy:
				this.AddBuff(GameplayDataSettings.Buffs.Weight_Heavy, this, 0);
				return;
			case CharacterMainControl.WeightStates.superHeavy:
				this.AddBuff(GameplayDataSettings.Buffs.Weight_SuperHeavy, this, 0);
				return;
			case CharacterMainControl.WeightStates.overWeight:
				this.AddBuff(GameplayDataSettings.Buffs.Weight_Overweight, this, 0);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0000DC5F File Offset: 0x0000BE5F
	public bool PickupItem(Item item)
	{
		if (this.health.IsDead)
		{
			return false;
		}
		item.Inspected = true;
		return this.itemControl.PickupItem(item);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0000DC83 File Offset: 0x0000BE83
	public InteractableBase GetInteractableTargetToInteract()
	{
		if (this.currentAction && this.currentAction.ActionPriority() >= this.interactAction.ActionPriority())
		{
			return null;
		}
		return this.interactAction.InteractTarget;
	}

	// Token: 0x06000315 RID: 789 RVA: 0x0000DCB7 File Offset: 0x0000BEB7
	public void Interact(InteractableBase _target)
	{
		if (this.currentAction && this.currentAction.ActionPriority() >= this.interactAction.ActionPriority())
		{
			return;
		}
		this.interactAction.SetInteractableTarget(_target);
		this.Interact();
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0000DCF4 File Offset: 0x0000BEF4
	public void Interact()
	{
		if (this.health.IsDead)
		{
			return;
		}
		if (this.carryAction.Running)
		{
			this.carryAction.StopAction();
			return;
		}
		if (this.currentAction)
		{
			return;
		}
		if (this.GetInteractableTargetToInteract() != null)
		{
			this.StartAction(this.interactAction);
		}
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0000DD52 File Offset: 0x0000BF52
	public void AddHealth(float healthValue)
	{
		this.health.AddHealth(healthValue * (1f + this.HealGain));
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0000DD6D File Offset: 0x0000BF6D
	public void SetRelatedScene(int _relatedScene, bool setActiveByPlayerDistance = true)
	{
		this.relatedScene = _relatedScene;
		if (MultiSceneCore.Instance)
		{
			MultiSceneCore.MoveToActiveWithScene(base.gameObject, _relatedScene);
			if (setActiveByPlayerDistance)
			{
				SetActiveByPlayerDistance.Register(base.gameObject, this.relatedScene);
			}
		}
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0000DDA4 File Offset: 0x0000BFA4
	public void Carry(Carriable target)
	{
		if (!this.carryAction)
		{
			return;
		}
		if (this.currentAction != null && this.currentAction.Running)
		{
			return;
		}
		this.carryAction.carryTarget = target;
		this.StartAction(this.carryAction);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0000DDF4 File Offset: 0x0000BFF4
	public void AddEnergy(float energyValue)
	{
		float num = this.CurrentEnergy;
		num += energyValue * (1f + this.FoodGain);
		if (num > this.MaxEnergy)
		{
			num = this.MaxEnergy;
		}
		if (num < 0f)
		{
			num = 0f;
		}
		this.CurrentEnergy = num;
	}

	// Token: 0x0600031B RID: 795 RVA: 0x0000DE40 File Offset: 0x0000C040
	public void AddWater(float waterValue)
	{
		float num = this.CurrentWater;
		num += waterValue * (1f + this.FoodGain);
		if (num > this.MaxWater)
		{
			num = this.MaxWater;
		}
		if (num < 0f)
		{
			num = 0f;
		}
		this.CurrentWater = num;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0000DE8C File Offset: 0x0000C08C
	public void DropAllItems()
	{
		if (this.characterItem == null)
		{
			return;
		}
		List<Item> list = new List<Item>();
		if (this.characterItem.Inventory != null)
		{
			foreach (Item item in this.characterItem.Inventory)
			{
				if ((!this.IsMainCharacter || !item.Tags.Contains(GameplayDataSettings.Tags.DontDropOnDeadInSlot)) && (!this.IsMainCharacter || !item.Sticky))
				{
					list.Add(item);
				}
			}
		}
		foreach (Slot slot in this.characterItem.Slots)
		{
			if (slot.Content != null && (!this.IsMainCharacter || !slot.Content.Tags.Contains(GameplayDataSettings.Tags.DontDropOnDeadInSlot)) && (!this.IsMainCharacter || !slot.Content.Sticky))
			{
				list.Add(slot.Content);
			}
		}
		foreach (Item item2 in list)
		{
			if (!this.IsMainCharacter || !item2.Sticky)
			{
				item2.Drop(base.transform.position, true, Vector3.forward, 360f);
			}
		}
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0000E030 File Offset: 0x0000C230
	public void DestroyAllItem()
	{
		if (this.characterItem == null)
		{
			return;
		}
		List<Item> list = new List<Item>();
		if (this.characterItem.Inventory != null)
		{
			foreach (Item item in this.characterItem.Inventory)
			{
				list.Add(item);
			}
		}
		foreach (Slot slot in this.characterItem.Slots)
		{
			if (slot.Content != null && (!this.IsMainCharacter || !slot.Content.Tags.Contains(GameplayDataSettings.Tags.DontDropOnDeadInSlot)) && (!this.IsMainCharacter || !slot.Content.Sticky))
			{
				list.Add(slot.Content);
			}
		}
		foreach (Item item2 in list)
		{
			if (!this.IsMainCharacter || !item2.Sticky)
			{
				ItemTreeExtensions.DestroyTree(item2);
			}
		}
	}

	// Token: 0x0600031E RID: 798 RVA: 0x0000E18C File Offset: 0x0000C38C
	public void DestroyItemsThatNeededToBeDestriedInBase()
	{
		if (this.characterItem == null)
		{
			return;
		}
		List<Item> list = new List<Item>();
		if (this.characterItem.Inventory != null)
		{
			foreach (Item item in this.characterItem.Inventory)
			{
				list.Add(item);
			}
		}
		foreach (Slot slot in this.characterItem.Slots)
		{
			if (slot.Content != null)
			{
				list.Add(slot.Content);
			}
		}
		foreach (Item item2 in list)
		{
			if (item2.Tags.Contains("DestroyInBase"))
			{
				ItemTreeExtensions.DestroyTree(item2);
			}
		}
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0000E2B0 File Offset: 0x0000C4B0
	public void AddSubVisuals(CharacterSubVisuals subVisuals)
	{
		if (!this.characterModel)
		{
			return;
		}
		this.characterModel.AddSubVisuals(subVisuals);
	}

	// Token: 0x06000320 RID: 800 RVA: 0x0000E2CC File Offset: 0x0000C4CC
	public void RemoveVisual(CharacterSubVisuals subVisuals)
	{
		if (!this.characterModel)
		{
			return;
		}
		this.characterModel.RemoveVisual(subVisuals);
	}

	// Token: 0x06000321 RID: 801 RVA: 0x0000E2E8 File Offset: 0x0000C4E8
	public void Hide()
	{
		if (this.hidden)
		{
			return;
		}
		if (this.Team == Teams.player)
		{
			return;
		}
		this.hidden = true;
		if (!this.characterModel)
		{
			return;
		}
		this.characterModel.SyncHiddenToMainCharacter();
	}

	// Token: 0x06000322 RID: 802 RVA: 0x0000E31C File Offset: 0x0000C51C
	public void Show()
	{
		Health health = this.health;
		if (health != null)
		{
			health.RequestHealthBar();
		}
		if (!this.hidden)
		{
			return;
		}
		this.hidden = false;
		if (!this.characterModel)
		{
			return;
		}
		this.characterModel.SyncHiddenToMainCharacter();
	}

	// Token: 0x06000323 RID: 803 RVA: 0x0000E358 File Offset: 0x0000C558
	private void OnEnable()
	{
		if (this.IsMainCharacter && this.health)
		{
			this.health.showHealthBar = true;
			this.health.RequestHealthBar();
		}
		AudioManager.SetVoiceType(base.gameObject, this.audioVoiceType);
	}

	// Token: 0x06000324 RID: 804 RVA: 0x0000E397 File Offset: 0x0000C597
	public bool IsNearByHalfObsticle(GameObject target)
	{
		return !(target == null) && this.nearByHalfObsticles.Count != 0 && this.nearByHalfObsticles.Contains(target);
	}

	// Token: 0x06000325 RID: 805 RVA: 0x0000E3BD File Offset: 0x0000C5BD
	public GameObject[] GetNearByHalfObsticles()
	{
		this.nearByHalfObsticles.RemoveWhere((GameObject go) => go == null);
		return this.nearByHalfObsticles.ToArray<GameObject>();
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
	public void AddnearByHalfObsticles(List<GameObject> objs)
	{
		foreach (GameObject gameObject in objs)
		{
			if (!(gameObject == null) && !this.nearByHalfObsticles.Contains(gameObject))
			{
				this.nearByHalfObsticles.Add(gameObject);
			}
		}
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0000E464 File Offset: 0x0000C664
	public void RemoveNearByHalfObsticles(List<GameObject> objs)
	{
		foreach (GameObject gameObject in objs)
		{
			if (!(gameObject == null) && this.nearByHalfObsticles.Contains(gameObject))
			{
				this.nearByHalfObsticles.Remove(gameObject);
			}
		}
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0000E4D0 File Offset: 0x0000C6D0
	public void UseItem(Item item)
	{
		if (this.IsMainCharacter && !item.UsageUtilities.IsUsable(item, this))
		{
			NotificationText.Push(LocalizationManager.ToPlainText("UI_Item_NotUsable"));
			return;
		}
		this.StoreHoldWeaponBeforeUse();
		if (ItemTreeExtensions.GetRoot(item) != this.characterItem)
		{
			Debug.Log("pick fail");
			item.Detach();
			item.AgentUtilities.ReleaseActiveAgent();
			item.transform.SetParent(base.transform);
		}
		if (this.interactAction.Running && this.interactAction.InteractingTarget is InteractableLootbox)
		{
			this.interactAction.StopAction();
		}
		this.useItemAction.SetUseItem(item);
		bool flag = this.StartAction(this.useItemAction);
		Debug.Log(string.Format("UseItemSuccess:{0}", flag));
		if (flag && this.IsMainCharacter)
		{
			Action<Item> onMainCharacterStartUseItem = CharacterMainControl.OnMainCharacterStartUseItem;
			if (onMainCharacterStartUseItem == null)
			{
				return;
			}
			onMainCharacterStartUseItem(item);
		}
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0000E5BD File Offset: 0x0000C7BD
	public CharacterBuffManager GetBuffManager()
	{
		return this.buffManager;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x0000E5C8 File Offset: 0x0000C7C8
	public void AddBuff(Buff buffPrefab, CharacterMainControl fromWho = null, int overrideWeaponID = 0)
	{
		if (!buffPrefab)
		{
			return;
		}
		if (this.buffResist.Contains(buffPrefab.ExclusiveTag))
		{
			return;
		}
		Buff.BuffExclusiveTags exclusiveTag = buffPrefab.ExclusiveTag;
		if (exclusiveTag != Buff.BuffExclusiveTags.NotExclusive)
		{
			Buff buffByTag = this.buffManager.GetBuffByTag(exclusiveTag);
			if (buffByTag != null && buffByTag.ID != buffPrefab.ID)
			{
				if (buffByTag.ExclusiveTagPriority > buffPrefab.ExclusiveTagPriority)
				{
					return;
				}
				if (buffByTag.ExclusiveTagPriority == buffPrefab.ExclusiveTagPriority && buffByTag.LimitedLifeTime && buffPrefab.LimitedLifeTime && buffByTag.CurrentLifeTime > buffPrefab.TotalLifeTime)
				{
					buffByTag.fromWho = fromWho;
					if (overrideWeaponID > 0)
					{
						buffByTag.fromWeaponID = overrideWeaponID;
					}
					return;
				}
				this.buffManager.RemoveBuff(buffByTag, false);
			}
		}
		this.buffManager.AddBuff(buffPrefab, fromWho, overrideWeaponID);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0000E689 File Offset: 0x0000C889
	public void RemoveBuff(int buffID, bool removeOneLayer)
	{
		this.buffManager.RemoveBuff(buffID, removeOneLayer);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0000E698 File Offset: 0x0000C898
	public void RemoveBuffsByTag(Buff.BuffExclusiveTags tag, bool removeOneLayer)
	{
		this.buffManager.RemoveBuffsByTag(tag, removeOneLayer);
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0000E6A7 File Offset: 0x0000C8A7
	public bool HasBuff(int buffID)
	{
		return this.buffManager.HasBuff(buffID);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0000E6B5 File Offset: 0x0000C8B5
	public void SetPosition(Vector3 pos)
	{
		this.movementControl.ForceSetPosition(pos);
		Action<CharacterMainControl, Vector3> onSetPositionEvent = this.OnSetPositionEvent;
		if (onSetPositionEvent == null)
		{
			return;
		}
		onSetPositionEvent(this, pos);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0000E6D8 File Offset: 0x0000C8D8
	public Item GetArmorItem()
	{
		Slot slot = this.characterItem.Slots["Armor"];
		if (slot == null)
		{
			return null;
		}
		return slot.Content;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0000E708 File Offset: 0x0000C908
	public Item GetHelmatItem()
	{
		Slot slot = this.characterItem.Slots["Helmat"];
		if (slot == null)
		{
			return null;
		}
		return slot.Content;
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0000E738 File Offset: 0x0000C938
	public Item GetFaceMaskItem()
	{
		Slot slot = this.characterItem.Slots["FaceMask"];
		if (slot == null)
		{
			return null;
		}
		return slot.Content;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0000E768 File Offset: 0x0000C968
	public static float WeaponRepairLossFactor()
	{
		if (!CharacterMainControl.Main || CharacterMainControl.Main.characterItem == null)
		{
			return 1f;
		}
		return CharacterMainControl.Main.characterItem.Constants.GetFloat("WeaponRepairLossFactor", 1f);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
	public static float EquipmentRepairLossFactor()
	{
		if (!CharacterMainControl.Main || CharacterMainControl.Main.characterItem == null)
		{
			return 1f;
		}
		return CharacterMainControl.Main.characterItem.Constants.GetFloat("EquipmentRepairLossFactor", 1f);
	}

	// Token: 0x06000334 RID: 820 RVA: 0x0000E807 File Offset: 0x0000CA07
	private float GetFloatStatValue(int hash)
	{
		if (this.characterItem)
		{
			return this.characterItem.GetStatValue(hash);
		}
		return 0f;
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000335 RID: 821 RVA: 0x0000E828 File Offset: 0x0000CA28
	public float CharacterWalkSpeed
	{
		get
		{
			float num = this.GetFloatStatValue(this.walkSpeedHash);
			num *= this.CharacterMoveability;
			ItemAgent_Gun gun = this.GetGun();
			if (gun)
			{
				float moveSpeedMultiplier = gun.MoveSpeedMultiplier;
				if (moveSpeedMultiplier > 0f)
				{
					num *= moveSpeedMultiplier;
				}
			}
			else
			{
				ItemAgent_MeleeWeapon meleeWeapon = this.GetMeleeWeapon();
				if (meleeWeapon)
				{
					float moveSpeedMultiplier2 = meleeWeapon.MoveSpeedMultiplier;
					if (moveSpeedMultiplier2 > 0f)
					{
						num *= moveSpeedMultiplier2;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x06000336 RID: 822 RVA: 0x0000E898 File Offset: 0x0000CA98
	public float AdsWalkSpeedMultiplier
	{
		get
		{
			ItemAgent_Gun gun = this.GetGun();
			if (gun)
			{
				return gun.AdsWalkSpeedMultiplier;
			}
			return 0.5f;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000337 RID: 823 RVA: 0x0000E8C0 File Offset: 0x0000CAC0
	public float CharacterOriginWalkSpeed
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStat(this.walkSpeedHash).BaseValue;
			}
			return 0f;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000338 RID: 824 RVA: 0x0000E8EB File Offset: 0x0000CAEB
	public float CharacterOriginRunSpeed
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStat(this.runSpeedHash).BaseValue;
			}
			return 0f;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000339 RID: 825 RVA: 0x0000E918 File Offset: 0x0000CB18
	public float CharacterRunSpeed
	{
		get
		{
			float num = this.GetFloatStatValue(this.runSpeedHash);
			num *= this.CharacterMoveability;
			ItemAgent_Gun gun = this.GetGun();
			if (gun)
			{
				float moveSpeedMultiplier = gun.MoveSpeedMultiplier;
				if (moveSpeedMultiplier > 0f)
				{
					num *= moveSpeedMultiplier;
				}
			}
			else
			{
				ItemAgent_MeleeWeapon meleeWeapon = this.GetMeleeWeapon();
				if (meleeWeapon)
				{
					float moveSpeedMultiplier2 = meleeWeapon.MoveSpeedMultiplier;
					if (moveSpeedMultiplier2 > 0f)
					{
						num *= moveSpeedMultiplier2;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600033A RID: 826 RVA: 0x0000E985 File Offset: 0x0000CB85
	public float StormProtection
	{
		get
		{
			return this.GetFloatStatValue(this.stormProtectionHash);
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x0600033B RID: 827 RVA: 0x0000E993 File Offset: 0x0000CB93
	public float WaterEnergyRecoverMultiplier
	{
		get
		{
			return this.GetFloatStatValue(this.waterEnergyRecoverMultiplierHash);
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x0600033C RID: 828 RVA: 0x0000E9A1 File Offset: 0x0000CBA1
	public float GunDistanceMultiplier
	{
		get
		{
			return this.GetFloatStatValue(this.gunDistanceMultiplierHash);
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x0600033D RID: 829 RVA: 0x0000E9AF File Offset: 0x0000CBAF
	public float ColdProtection
	{
		get
		{
			return this.GetFloatStatValue(this.ColdProtectionHash);
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x0600033E RID: 830 RVA: 0x0000E9BD File Offset: 0x0000CBBD
	public float HeatProtection
	{
		get
		{
			return this.GetFloatStatValue(this.HeatProtectionHash);
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x0600033F RID: 831 RVA: 0x0000E9CB File Offset: 0x0000CBCB
	public float CharacterMoveability
	{
		get
		{
			return this.GetFloatStatValue(this.moveabilityHash);
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000340 RID: 832 RVA: 0x0000E9D9 File Offset: 0x0000CBD9
	public float CharacterRunAcc
	{
		get
		{
			return this.GetFloatStatValue(this.runAccHash);
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000341 RID: 833 RVA: 0x0000E9E7 File Offset: 0x0000CBE7
	public float CharacterTurnSpeed
	{
		get
		{
			return this.GetFloatStatValue(this.turnSpeedHash) * this.CharacterMoveability;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000342 RID: 834 RVA: 0x0000E9FC File Offset: 0x0000CBFC
	public float CharacterAimTurnSpeed
	{
		get
		{
			return this.GetFloatStatValue(this.aimTurnSpeedHash) * this.CharacterMoveability;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06000343 RID: 835 RVA: 0x0000EA11 File Offset: 0x0000CC11
	public float DashSpeed
	{
		get
		{
			return this.GetFloatStatValue(this.dashSpeedHash);
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000344 RID: 836 RVA: 0x0000EA1F File Offset: 0x0000CC1F
	public int PetCapcity
	{
		get
		{
			return Mathf.RoundToInt(this.GetFloatStatValue(this.PetCapcityHash));
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000345 RID: 837 RVA: 0x0000EA32 File Offset: 0x0000CC32
	// (set) Token: 0x06000346 RID: 838 RVA: 0x0000EA5B File Offset: 0x0000CC5B
	public bool DashCanControl
	{
		get
		{
			return !this.characterItem || this.characterItem.GetStatValue(this.dashCanControlHash) > 0f;
		}
		set
		{
			this.characterItem.GetStat(this.dashCanControlHash).BaseValue = (float)(value ? 1 : 0);
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000347 RID: 839 RVA: 0x0000EA7B File Offset: 0x0000CC7B
	public float MaxStamina
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.maxStaminaHash);
			}
			return 0f;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000348 RID: 840 RVA: 0x0000EAA1 File Offset: 0x0000CCA1
	public float CurrentStamina
	{
		get
		{
			return this.currentStamina;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000349 RID: 841 RVA: 0x0000EAA9 File Offset: 0x0000CCA9
	public float StaminaDrainRate
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.staminaDrainRateHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x0600034A RID: 842 RVA: 0x0000EACF File Offset: 0x0000CCCF
	public float StaminaRecoverRate
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.staminaRecoverRateHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x0600034B RID: 843 RVA: 0x0000EAF5 File Offset: 0x0000CCF5
	public float StaminaRecoverTime
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.staminaRecoverTimeHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x0600034C RID: 844 RVA: 0x0000EB1B File Offset: 0x0000CD1B
	public float CharacterWalkAcc
	{
		get
		{
			return this.GetFloatStatValue(this.walkAccHash);
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x0600034D RID: 845 RVA: 0x0000EB29 File Offset: 0x0000CD29
	public float VisableDistanceFactor
	{
		get
		{
			return this.GetFloatStatValue(this.visableDistanceFactorHash);
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x0600034E RID: 846 RVA: 0x0000EB37 File Offset: 0x0000CD37
	public float MaxWeight
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.maxWeightHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600034F RID: 847 RVA: 0x0000EB5D File Offset: 0x0000CD5D
	public float FoodGain
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.foodGainHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x06000350 RID: 848 RVA: 0x0000EB83 File Offset: 0x0000CD83
	public float HealGain
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.healGainHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000351 RID: 849 RVA: 0x0000EBA9 File Offset: 0x0000CDA9
	public float MaxEnergy
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.maxEnergyHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000352 RID: 850 RVA: 0x0000EBCF File Offset: 0x0000CDCF
	public float EnergyCostPerMin
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.energyCostPerMinHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000353 RID: 851 RVA: 0x0000EBF5 File Offset: 0x0000CDF5
	// (set) Token: 0x06000354 RID: 852 RVA: 0x0000EC25 File Offset: 0x0000CE25
	public float CurrentEnergy
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.Variables.GetFloat(this.currentEnergyHash, 0f);
			}
			return 0f;
		}
		set
		{
			if (this.characterItem)
			{
				this.characterItem.Variables.SetFloat(this.currentEnergyHash, value);
			}
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000355 RID: 853 RVA: 0x0000EC4B File Offset: 0x0000CE4B
	public float MaxWater
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.maxWaterHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000356 RID: 854 RVA: 0x0000EC71 File Offset: 0x0000CE71
	public float WaterCostPerMin
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.waterCostPerMinHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000357 RID: 855 RVA: 0x0000EC97 File Offset: 0x0000CE97
	// (set) Token: 0x06000358 RID: 856 RVA: 0x0000ECC7 File Offset: 0x0000CEC7
	public float CurrentWater
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.Variables.GetFloat(this.currentWaterHash, 0f);
			}
			return 0f;
		}
		set
		{
			if (this.characterItem)
			{
				this.characterItem.Variables.SetFloat(this.currentWaterHash, value);
			}
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000359 RID: 857 RVA: 0x0000ECED File Offset: 0x0000CEED
	public float NightVisionAbility
	{
		get
		{
			return this.GetFloatStatValue(this.NightVisionAbilityHash);
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x0600035A RID: 858 RVA: 0x0000ECFB File Offset: 0x0000CEFB
	public float NightVisionType
	{
		get
		{
			return this.GetFloatStatValue(this.NightVisionTypeHash);
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x0600035B RID: 859 RVA: 0x0000ED09 File Offset: 0x0000CF09
	public float HearingAbility
	{
		get
		{
			return this.GetFloatStatValue(this.HearingAbilityHash);
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x0600035C RID: 860 RVA: 0x0000ED17 File Offset: 0x0000CF17
	public float SoundVisable
	{
		get
		{
			return this.GetFloatStatValue(this.SoundVisableHash);
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x0600035D RID: 861 RVA: 0x0000ED25 File Offset: 0x0000CF25
	public float ViewAngle
	{
		get
		{
			return this.GetFloatStatValue(this.viewAngleHash);
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x0600035E RID: 862 RVA: 0x0000ED33 File Offset: 0x0000CF33
	public float ViewDistance
	{
		get
		{
			return this.GetFloatStatValue(this.viewDistanceHash);
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x0600035F RID: 863 RVA: 0x0000ED41 File Offset: 0x0000CF41
	public float SenseRange
	{
		get
		{
			return this.GetFloatStatValue(this.senseRangeHash);
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000360 RID: 864 RVA: 0x0000ED4F File Offset: 0x0000CF4F
	public float MeleeDamageMultiplier
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.meleeDamageMultiplierHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000361 RID: 865 RVA: 0x0000ED74 File Offset: 0x0000CF74
	public float MeleeCritRateGain
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.meleeCritRateGainHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000362 RID: 866 RVA: 0x0000ED99 File Offset: 0x0000CF99
	public float MeleeCritDamageGain
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.meleeCritDamageGainHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000363 RID: 867 RVA: 0x0000EDBE File Offset: 0x0000CFBE
	public float GunDamageMultiplier
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.gunDamageMultiplierHash);
			}
			return 1f;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000364 RID: 868 RVA: 0x0000EDE3 File Offset: 0x0000CFE3
	public float GunShootSpeedMultiplier
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.gunShootSpeedMultiplierHash);
			}
			return 1f;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000365 RID: 869 RVA: 0x0000EE08 File Offset: 0x0000D008
	public float ReloadSpeedGain
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.reloadSpeedGainHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000366 RID: 870 RVA: 0x0000EE2D File Offset: 0x0000D02D
	public float GunCritRateGain
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.gunCritRateGainHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000367 RID: 871 RVA: 0x0000EE52 File Offset: 0x0000D052
	public float GunCritDamageGain
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.gunCritDamageGainHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000368 RID: 872 RVA: 0x0000EE77 File Offset: 0x0000D077
	public float GunBulletSpeedMultiplier
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.gunBulletSpeedMultiplierHash);
			}
			return 1f;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000369 RID: 873 RVA: 0x0000EE9C File Offset: 0x0000D09C
	public float RecoilControl
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.recoilControlHash);
			}
			return 1f;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600036A RID: 874 RVA: 0x0000EEC1 File Offset: 0x0000D0C1
	public float GunScatterMultiplier
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.GunScatterMultiplierHash);
			}
			return 1f;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x0600036B RID: 875 RVA: 0x0000EEE6 File Offset: 0x0000D0E6
	public float InventoryCapacity
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(CharacterMainControl.InventoryCapacityHash);
			}
			return 16f;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x0600036C RID: 876 RVA: 0x0000EF0C File Offset: 0x0000D10C
	public bool HasGasMask
	{
		get
		{
			float num = 0f;
			if (this.characterItem)
			{
				num = this.characterItem.GetStatValue(CharacterMainControl.GasMaskHash);
			}
			return num > 0.1f;
		}
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0000EF48 File Offset: 0x0000D148
	public void UseStamina(float value)
	{
		if (!LevelManager.Instance || LevelManager.Instance.IsBaseLevel)
		{
			return;
		}
		if (value <= 0f)
		{
			return;
		}
		this.staminaRecoverTimer = 0f;
		this.currentStamina -= value;
		if (this.currentStamina < 0f)
		{
			this.currentStamina = 0f;
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600036E RID: 878 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
	public float WalkSoundRange
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.walkSoundRangeHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x0600036F RID: 879 RVA: 0x0000EFCE File Offset: 0x0000D1CE
	public float RunSoundRange
	{
		get
		{
			if (this.characterItem)
			{
				return this.characterItem.GetStatValue(this.runSoundRangeHash);
			}
			return 0f;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000370 RID: 880 RVA: 0x0000EFF4 File Offset: 0x0000D1F4
	public bool FlashLight
	{
		get
		{
			return this.CurrentHoldItemAgent && this.CurrentHoldItemAgent.Item.GetStatValue(CharacterMainControl.flashLightHash) > 0f;
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000371 RID: 881 RVA: 0x0000F021 File Offset: 0x0000D221
	public string SoundKey
	{
		get
		{
			return "Default";
		}
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0000F3DC File Offset: 0x0000D5DC
	[CompilerGenerated]
	private void <UpdateCold>g__TryPopColdText|206_0(bool superCold)
	{
		if (this.coldDialogTimer > 0f)
		{
			return;
		}
		this.coldDialogTimer = Random.Range(8f, 12f);
		if (superCold)
		{
			this.PopText(GameplayDataSettings.DialogDatas.GetSuperColdDialog(), -1f);
			return;
		}
		this.PopText(GameplayDataSettings.DialogDatas.GetColdDialog(), -1f);
	}

	// Token: 0x0400021C RID: 540
	public CharacterRandomPreset characterPreset;

	// Token: 0x0400021D RID: 541
	private AudioManager.VoiceType audioVoiceType;

	// Token: 0x0400021E RID: 542
	private AudioManager.FootStepMaterialType footStepMaterialType;

	// Token: 0x0400021F RID: 543
	[SerializeField]
	private Teams team;

	// Token: 0x04000221 RID: 545
	private Item characterItem;

	// Token: 0x04000222 RID: 546
	public Movement movementControl;

	// Token: 0x04000223 RID: 547
	public ItemAgentHolder agentHolder;

	// Token: 0x04000224 RID: 548
	public CA_Carry carryAction;

	// Token: 0x04000225 RID: 549
	public CharacterModel characterModel;

	// Token: 0x04000226 RID: 550
	public AICharacterController aiCharacterController;

	// Token: 0x04000227 RID: 551
	private bool hidden;

	// Token: 0x04000229 RID: 553
	public bool dropBoxOnDead = true;

	// Token: 0x0400022A RID: 554
	public InteractableLootbox deadLootBoxPrefab;

	// Token: 0x0400022D RID: 557
	public Transform modelRoot;

	// Token: 0x0400022F RID: 559
	private Vector3 moveInput;

	// Token: 0x04000230 RID: 560
	private bool runInput;

	// Token: 0x04000231 RID: 561
	private bool adsInput;

	// Token: 0x04000232 RID: 562
	private const float defaultAimRange = 8f;

	// Token: 0x04000233 RID: 563
	private AimTypes aimType;

	// Token: 0x04000234 RID: 564
	private float disableTriggerTimer;

	// Token: 0x04000235 RID: 565
	public List<Buff.BuffExclusiveTags> buffResist;

	// Token: 0x04000236 RID: 566
	private Vector3 inputAimPoint;

	// Token: 0x04000237 RID: 567
	private CharacterActionBase currentAction;

	// Token: 0x04000238 RID: 568
	public CA_Reload reloadAction;

	// Token: 0x04000239 RID: 569
	public CA_Skill skillAction;

	// Token: 0x0400023A RID: 570
	public CA_UseItem useItemAction;

	// Token: 0x0400023F RID: 575
	public static Action<CharacterMainControl, Inventory, int> OnMainCharacterInventoryChangedEvent;

	// Token: 0x04000240 RID: 576
	public static Action<CharacterMainControl, Slot> OnMainCharacterSlotContentChangedEvent;

	// Token: 0x04000242 RID: 578
	private int relatedScene;

	// Token: 0x04000243 RID: 579
	[SerializeField]
	private Health health;

	// Token: 0x04000244 RID: 580
	[SerializeField]
	private CharacterItemControl itemControl;

	// Token: 0x04000245 RID: 581
	public CA_Interact interactAction;

	// Token: 0x04000246 RID: 582
	[SerializeField]
	private CharacterEquipmentController equipmentController;

	// Token: 0x04000247 RID: 583
	public static Action<CharacterMainControl, DuckovItemAgent> OnMainCharacterChangeHoldItemAgentEvent;

	// Token: 0x04000248 RID: 584
	[SerializeField]
	private CharacterBuffManager buffManager;

	// Token: 0x04000249 RID: 585
	private int holdWeaponBeforeUse;

	// Token: 0x0400024A RID: 586
	public CA_Dash dashAction;

	// Token: 0x0400024B RID: 587
	public CA_Attack attackAction;

	// Token: 0x0400024E RID: 590
	public DamageReceiver mainDamageReceiver;

	// Token: 0x0400024F RID: 591
	private HashSet<GameObject> nearByHalfObsticles;

	// Token: 0x04000250 RID: 592
	private List<Item> weaponsTemp = new List<Item>();

	// Token: 0x04000251 RID: 593
	private int weaponSwitchIndex;

	// Token: 0x04000252 RID: 594
	private CharacterMainControl.WeightStates weightState = CharacterMainControl.WeightStates.normal;

	// Token: 0x04000253 RID: 595
	private int meleeWeaponSlotHash = "MeleeWeapon".GetHashCode();

	// Token: 0x04000254 RID: 596
	private int primWeaponSlotHash = "PrimaryWeapon".GetHashCode();

	// Token: 0x04000255 RID: 597
	private int secWeaponSlotHash = "SecondaryWeapon".GetHashCode();

	// Token: 0x04000256 RID: 598
	private float staminaRecoverTimer;

	// Token: 0x04000257 RID: 599
	private float variableTickTimer;

	// Token: 0x04000258 RID: 600
	private float coldTickTimer = 1f;

	// Token: 0x04000259 RID: 601
	private float coldDialogTimer = 2f;

	// Token: 0x0400025A RID: 602
	public const float weightThreshold_Light = 0.25f;

	// Token: 0x0400025B RID: 603
	public const float weightThreshold_Heavy = 0.5f;

	// Token: 0x0400025C RID: 604
	public const float weightThreshold_superWeight = 0.75f;

	// Token: 0x0400025D RID: 605
	private string hideShowRecorder;

	// Token: 0x0400025E RID: 606
	private int walkSpeedHash = "WalkSpeed".GetHashCode();

	// Token: 0x0400025F RID: 607
	private int walkAccHash = "WalkAcc".GetHashCode();

	// Token: 0x04000260 RID: 608
	private int runSpeedHash = "RunSpeed".GetHashCode();

	// Token: 0x04000261 RID: 609
	private int stormProtectionHash = "StormProtection".GetHashCode();

	// Token: 0x04000262 RID: 610
	private int waterEnergyRecoverMultiplierHash = "WaterEnergyRecoverMultiplier".GetHashCode();

	// Token: 0x04000263 RID: 611
	private int gunDistanceMultiplierHash = "GunDistanceMultiplier".GetHashCode();

	// Token: 0x04000264 RID: 612
	private int ColdProtectionHash = "ColdProtection".GetHashCode();

	// Token: 0x04000265 RID: 613
	private int HeatProtectionHash = "HeatProtection".GetHashCode();

	// Token: 0x04000266 RID: 614
	private int moveabilityHash = "Moveability".GetHashCode();

	// Token: 0x04000267 RID: 615
	private int runAccHash = "RunAcc".GetHashCode();

	// Token: 0x04000268 RID: 616
	private int turnSpeedHash = "TurnSpeed".GetHashCode();

	// Token: 0x04000269 RID: 617
	private int aimTurnSpeedHash = "AimTurnSpeed".GetHashCode();

	// Token: 0x0400026A RID: 618
	private int dashSpeedHash = "DashSpeed".GetHashCode();

	// Token: 0x0400026B RID: 619
	private int dashCanControlHash = "DashCanControl".GetHashCode();

	// Token: 0x0400026C RID: 620
	private int PetCapcityHash = "PetCapcity".GetHashCode();

	// Token: 0x0400026D RID: 621
	private int maxStaminaHash = "Stamina".GetHashCode();

	// Token: 0x0400026E RID: 622
	private float currentStamina;

	// Token: 0x0400026F RID: 623
	private int staminaDrainRateHash = "StaminaDrainRate".GetHashCode();

	// Token: 0x04000270 RID: 624
	private int staminaRecoverRateHash = "StaminaRecoverRate".GetHashCode();

	// Token: 0x04000271 RID: 625
	private int staminaRecoverTimeHash = "StaminaRecoverTime".GetHashCode();

	// Token: 0x04000272 RID: 626
	private int visableDistanceFactorHash = "VisableDistanceFactor".GetHashCode();

	// Token: 0x04000273 RID: 627
	private int maxWeightHash = "MaxWeight".GetHashCode();

	// Token: 0x04000274 RID: 628
	private int foodGainHash = "FoodGain".GetHashCode();

	// Token: 0x04000275 RID: 629
	private int healGainHash = "HealGain".GetHashCode();

	// Token: 0x04000276 RID: 630
	private int maxEnergyHash = "MaxEnergy".GetHashCode();

	// Token: 0x04000277 RID: 631
	private int energyCostPerMinHash = "EnergyCost".GetHashCode();

	// Token: 0x04000278 RID: 632
	private int currentEnergyHash = "CurrentEnergy".GetHashCode();

	// Token: 0x04000279 RID: 633
	private int maxWaterHash = "MaxWater".GetHashCode();

	// Token: 0x0400027A RID: 634
	private int waterCostPerMinHash = "WaterCost".GetHashCode();

	// Token: 0x0400027B RID: 635
	private bool starve;

	// Token: 0x0400027C RID: 636
	private bool thirsty;

	// Token: 0x0400027D RID: 637
	private int currentWaterHash = "CurrentWater".GetHashCode();

	// Token: 0x0400027E RID: 638
	private int NightVisionAbilityHash = "NightVisionAbility".GetHashCode();

	// Token: 0x0400027F RID: 639
	private int NightVisionTypeHash = "NightVisionType".GetHashCode();

	// Token: 0x04000280 RID: 640
	private int HearingAbilityHash = "HearingAbility".GetHashCode();

	// Token: 0x04000281 RID: 641
	private int SoundVisableHash = "SoundVisable".GetHashCode();

	// Token: 0x04000282 RID: 642
	private int viewAngleHash = "ViewAngle".GetHashCode();

	// Token: 0x04000283 RID: 643
	private int viewDistanceHash = "ViewDistance".GetHashCode();

	// Token: 0x04000284 RID: 644
	private int senseRangeHash = "SenseRange".GetHashCode();

	// Token: 0x04000285 RID: 645
	private static int meleeDamageMultiplierHash = "MeleeDamageMultiplier".GetHashCode();

	// Token: 0x04000286 RID: 646
	private static int meleeCritRateGainHash = "MeleeCritRateGain".GetHashCode();

	// Token: 0x04000287 RID: 647
	private static int meleeCritDamageGainHash = "MeleeCritDamageGain".GetHashCode();

	// Token: 0x04000288 RID: 648
	private static int gunDamageMultiplierHash = "GunDamageMultiplier".GetHashCode();

	// Token: 0x04000289 RID: 649
	private static int gunShootSpeedMultiplierHash = "GunShootSpeedMultiplier".GetHashCode();

	// Token: 0x0400028A RID: 650
	private static int reloadSpeedGainHash = "ReloadSpeedGain".GetHashCode();

	// Token: 0x0400028B RID: 651
	private static int gunCritRateGainHash = "GunCritRateGain".GetHashCode();

	// Token: 0x0400028C RID: 652
	private static int gunBulletSpeedMultiplierHash = "BulletSpeedMultiplier".GetHashCode();

	// Token: 0x0400028D RID: 653
	private static int gunCritDamageGainHash = "GunCritDamageGain".GetHashCode();

	// Token: 0x0400028E RID: 654
	private static int recoilControlHash = "RecoilControl".GetHashCode();

	// Token: 0x0400028F RID: 655
	private static int GunScatterMultiplierHash = "GunScatterMultiplier".GetHashCode();

	// Token: 0x04000290 RID: 656
	private static int InventoryCapacityHash = "InventoryCapacity".GetHashCode();

	// Token: 0x04000291 RID: 657
	private static int GasMaskHash = "GasMask".GetHashCode();

	// Token: 0x04000292 RID: 658
	private int walkSoundRangeHash = "WalkSoundRange".GetHashCode();

	// Token: 0x04000293 RID: 659
	private int runSoundRangeHash = "RunSoundRange".GetHashCode();

	// Token: 0x04000294 RID: 660
	private static int flashLightHash = "FlashLight".GetHashCode();

	// Token: 0x0200044D RID: 1101
	public enum WeightStates
	{
		// Token: 0x04001AEC RID: 6892
		light,
		// Token: 0x04001AED RID: 6893
		normal,
		// Token: 0x04001AEE RID: 6894
		heavy,
		// Token: 0x04001AEF RID: 6895
		superHeavy,
		// Token: 0x04001AF0 RID: 6896
		overWeight
	}
}
