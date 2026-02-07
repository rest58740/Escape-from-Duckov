using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200007C RID: 124
public class SkillHUD : MonoBehaviour
{
	// Token: 0x060004C4 RID: 1220 RVA: 0x00015F24 File Offset: 0x00014124
	private void Awake()
	{
		this.SyncHud();
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x00015F2C File Offset: 0x0001412C
	private void SyncHud()
	{
		if (this.rangeCache < 0f)
		{
			this.rangeCache = this.skillJoystick.joystickRangePercent;
		}
		this.activeParent.SetActive(this.skillHudActive);
		if (this.skillHudActive)
		{
			this.skillIcon.sprite = this.skillKeeper.Skill.icon;
			if (this.skillKeeper.Skill.SkillContext.castRange > 0f)
			{
				this.skillJoystick.canCancle = true;
				this.skillJoystick.joystickRangePercent = this.rangeCache;
				return;
			}
			this.skillJoystick.canCancle = false;
			this.skillJoystick.joystickRangePercent = 0f;
		}
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00015FE4 File Offset: 0x000141E4
	private void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (!this.characterMainControl)
			{
				return;
			}
			this.OnInit();
		}
		if (this.skillHudActive && (this.skillKeeper == null || !this.skillKeeper.CheckSkillAndBinding()))
		{
			this.skillHudActive = false;
			this.SyncHud();
		}
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0001604C File Offset: 0x0001424C
	private void OnInit()
	{
		SkillTypes skillTypes = this.skillType;
		if (skillTypes != SkillTypes.itemSkill)
		{
			if (skillTypes == SkillTypes.characterSkill)
			{
				this.skillKeeper = this.characterMainControl.skillAction.characterSkillKeeper;
				this.skillJoystick.UpdateValueEvent.AddListener(new UnityAction<Vector2, bool>(this.touchInputController.SetCharacterSkillAimInput));
				this.skillJoystick.OnTouchEvent.AddListener(new UnityAction(this.touchInputController.StartCharacterSkillAim));
				this.skillJoystick.OnUpEvent.AddListener(new UnityAction<bool>(this.touchInputController.CharacterSkillRelease));
			}
		}
		else
		{
			this.skillKeeper = this.characterMainControl.skillAction.holdItemSkillKeeper;
			this.skillJoystick.UpdateValueEvent.AddListener(new UnityAction<Vector2, bool>(this.touchInputController.SetItemSkillAimInput));
			this.skillJoystick.OnTouchEvent.AddListener(new UnityAction(this.touchInputController.StartItemSkillAim));
			this.skillJoystick.OnUpEvent.AddListener(new UnityAction<bool>(this.touchInputController.ItemSkillRelease));
		}
		CharacterSkillKeeper characterSkillKeeper = this.skillKeeper;
		characterSkillKeeper.OnSkillChanged = (Action)Delegate.Combine(characterSkillKeeper.OnSkillChanged, new Action(this.OnSkillChanged));
		if (this.skillKeeper.CheckSkillAndBinding())
		{
			this.OnSkillChanged();
		}
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001619D File Offset: 0x0001439D
	private void OnSkillChanged()
	{
		this.skillHudActive = this.skillKeeper.CheckSkillAndBinding();
		if (this.skillJoystick.Holding)
		{
			this.skillJoystick.CancleTouch();
		}
		this.SyncHud();
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x000161CE File Offset: 0x000143CE
	private void OnDestroy()
	{
		if (this.skillKeeper != null)
		{
			CharacterSkillKeeper characterSkillKeeper = this.skillKeeper;
			characterSkillKeeper.OnSkillChanged = (Action)Delegate.Remove(characterSkillKeeper.OnSkillChanged, new Action(this.OnSkillChanged));
		}
	}

	// Token: 0x0400040B RID: 1035
	private CharacterMainControl characterMainControl;

	// Token: 0x0400040C RID: 1036
	public CharacterTouchInputControl touchInputController;

	// Token: 0x0400040D RID: 1037
	public Image skillIcon;

	// Token: 0x0400040E RID: 1038
	private bool skillHudActive;

	// Token: 0x0400040F RID: 1039
	public Soda_Joysticks skillJoystick;

	// Token: 0x04000410 RID: 1040
	public GameObject skillButton;

	// Token: 0x04000411 RID: 1041
	public GameObject activeParent;

	// Token: 0x04000412 RID: 1042
	[SerializeField]
	private SkillTypes skillType;

	// Token: 0x04000413 RID: 1043
	private CharacterSkillKeeper skillKeeper;

	// Token: 0x04000414 RID: 1044
	private float rangeCache = -1f;
}
