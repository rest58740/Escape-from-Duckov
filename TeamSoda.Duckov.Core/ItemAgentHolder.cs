using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class ItemAgentHolder : MonoBehaviour
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0001185D File Offset: 0x0000FA5D
	public DuckovItemAgent CurrentHoldItemAgent
	{
		get
		{
			return this.currentHoldItemAgent;
		}
	}

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x060003F3 RID: 1011 RVA: 0x00011868 File Offset: 0x0000FA68
	// (remove) Token: 0x060003F4 RID: 1012 RVA: 0x000118A0 File Offset: 0x0000FAA0
	public event Action<DuckovItemAgent> OnHoldAgentChanged;

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060003F5 RID: 1013 RVA: 0x000118D5 File Offset: 0x0000FAD5
	public Transform CurrentUsingSocket
	{
		get
		{
			if (!this.currentHoldItemAgent)
			{
				this._currentUsingSocketCache = null;
			}
			return this._currentUsingSocketCache;
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000118F4 File Offset: 0x0000FAF4
	public ItemAgent_Gun CurrentHoldGun
	{
		get
		{
			if (this._gunRef && this.currentHoldItemAgent && this._gunRef.gameObject == this.currentHoldItemAgent.gameObject)
			{
				return this._gunRef;
			}
			this._gunRef = null;
			return null;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00011948 File Offset: 0x0000FB48
	public ItemAgent_MeleeWeapon CurrentHoldMeleeWeapon
	{
		get
		{
			if (this._meleeRef && this.currentHoldItemAgent && this._meleeRef.gameObject == this.currentHoldItemAgent.gameObject)
			{
				return this._meleeRef;
			}
			this._meleeRef = null;
			return null;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001199B File Offset: 0x0000FB9B
	public ItemSetting_Skill Skill
	{
		get
		{
			return this._skillRef;
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x000119A4 File Offset: 0x0000FBA4
	public DuckovItemAgent ChangeHoldItem(Item item)
	{
		this.DestroyCurrentItemAgent();
		if (item == null)
		{
			Action<DuckovItemAgent> onHoldAgentChanged = this.OnHoldAgentChanged;
			if (onHoldAgentChanged != null)
			{
				onHoldAgentChanged(null);
			}
			return null;
		}
		ItemAgent itemAgent = item.CreateHandheldAgent();
		if (itemAgent == null)
		{
			Action<DuckovItemAgent> onHoldAgentChanged2 = this.OnHoldAgentChanged;
			if (onHoldAgentChanged2 != null)
			{
				onHoldAgentChanged2(null);
			}
			return null;
		}
		this.currentHoldItemAgent = (itemAgent as DuckovItemAgent);
		if (this.currentHoldItemAgent == null)
		{
			Object.Destroy(itemAgent.gameObject);
			Action<DuckovItemAgent> onHoldAgentChanged3 = this.OnHoldAgentChanged;
			if (onHoldAgentChanged3 != null)
			{
				onHoldAgentChanged3(null);
			}
			return null;
		}
		this.currentHoldItemAgent.SetHolder(this.characterController);
		Transform transform;
		switch (this.currentHoldItemAgent.handheldSocket)
		{
		case HandheldSocketTypes.normalHandheld:
			transform = this.characterController.characterModel.RightHandSocket;
			break;
		case HandheldSocketTypes.meleeWeapon:
			transform = this.characterController.characterModel.MeleeWeaponSocket;
			break;
		case HandheldSocketTypes.leftHandSocket:
			transform = this.characterController.characterModel.LefthandSocket;
			if (transform == null)
			{
				transform = this.characterController.characterModel.RightHandSocket;
			}
			break;
		default:
			transform = this.characterController.characterModel.RightHandSocket;
			break;
		}
		this.currentHoldItemAgent.transform.SetParent(transform, false);
		this._currentUsingSocketCache = transform;
		this.currentHoldItemAgent.transform.localPosition = Vector3.zero;
		this.currentHoldItemAgent.transform.localRotation = Quaternion.identity;
		this.currentHoldItemAgent.Item.onItemTreeChanged += this.OnAgentItemTreeChanged;
		this._gunRef = (this.currentHoldItemAgent as ItemAgent_Gun);
		this._meleeRef = (this.currentHoldItemAgent as ItemAgent_MeleeWeapon);
		if (!this.IsSkillItem(item))
		{
			this._skillRef = null;
		}
		else
		{
			this._skillRef = item.GetComponent<ItemSetting_Skill>();
		}
		if (this._skillRef)
		{
			this.characterController.SetSkill(SkillTypes.itemSkill, this._skillRef.Skill, itemAgent.gameObject);
		}
		else
		{
			this.characterController.SetSkill(SkillTypes.itemSkill, null, null);
		}
		this.holdStadyTimer = 0f;
		this.holdStady = false;
		itemAgent.gameObject.SetActive(false);
		Action<DuckovItemAgent> onHoldAgentChanged4 = this.OnHoldAgentChanged;
		if (onHoldAgentChanged4 != null)
		{
			onHoldAgentChanged4(this.currentHoldItemAgent);
		}
		return this.currentHoldItemAgent;
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x00011BDE File Offset: 0x0000FDDE
	public void SetTrigger(bool trigger, bool triggerThisFrame, bool releaseThisFrame)
	{
		if (!this.currentHoldItemAgent)
		{
			return;
		}
		if (!this.characterController.CanUseHand())
		{
			return;
		}
		if (this.CurrentHoldGun != null)
		{
			this.CurrentHoldGun.SetTrigger(trigger, triggerThisFrame, releaseThisFrame);
		}
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x00011C18 File Offset: 0x0000FE18
	private void OnDestroy()
	{
		if (this.currentHoldItemAgent)
		{
			this.currentHoldItemAgent.Item.onItemTreeChanged -= this.OnAgentItemTreeChanged;
		}
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x00011C44 File Offset: 0x0000FE44
	private void DestroyCurrentItemAgent()
	{
		this._skillRef = null;
		if (this.currentHoldItemAgent == null)
		{
			return;
		}
		if (this.currentHoldItemAgent.Item != null)
		{
			this.currentHoldItemAgent.Item.onItemTreeChanged -= this.OnAgentItemTreeChanged;
			this.currentHoldItemAgent.Item.AgentUtilities.ReleaseActiveAgent();
		}
		this.currentHoldItemAgent = null;
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x00011CB4 File Offset: 0x0000FEB4
	private void OnAgentItemTreeChanged(Item item)
	{
		if (item == null || this.currentHoldItemAgent == null || this.currentHoldItemAgent.Item != item || item.GetCharacterItem() != this.characterController.CharacterItem)
		{
			this.DestroyCurrentItemAgent();
		}
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x00011D09 File Offset: 0x0000FF09
	private bool IsSkillItem(Item item)
	{
		return !(item == null) && item.GetBool("IsSkill", false);
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x00011D24 File Offset: 0x0000FF24
	private void Update()
	{
		if (this.currentHoldItemAgent != null && !this.holdStady)
		{
			this.holdStadyTimer += Time.deltaTime;
			if (this.holdStadyTimer > this.holdStadyTime)
			{
				this.holdStady = true;
				this.currentHoldItemAgent.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000307 RID: 775
	public CharacterMainControl characterController;

	// Token: 0x04000308 RID: 776
	private DuckovItemAgent currentHoldItemAgent;

	// Token: 0x0400030A RID: 778
	private Transform _currentUsingSocketCache;

	// Token: 0x0400030B RID: 779
	private static int handheldHash = "Handheld".GetHashCode();

	// Token: 0x0400030C RID: 780
	private ItemAgent_Gun _gunRef;

	// Token: 0x0400030D RID: 781
	private ItemAgent_MeleeWeapon _meleeRef;

	// Token: 0x0400030E RID: 782
	private ItemSetting_Skill _skillRef;

	// Token: 0x0400030F RID: 783
	private bool holdStady;

	// Token: 0x04000310 RID: 784
	private float holdStadyTime = 0.15f;

	// Token: 0x04000311 RID: 785
	private float holdStadyTimer;
}
