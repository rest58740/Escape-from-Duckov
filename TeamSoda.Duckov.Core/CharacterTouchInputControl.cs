using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class CharacterTouchInputControl : MonoBehaviour
{
	// Token: 0x060004A7 RID: 1191 RVA: 0x00015815 File Offset: 0x00013A15
	public void SetMoveInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetMoveInput(axisInput);
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x00015823 File Offset: 0x00013A23
	public void SetRunInput(bool holding)
	{
		this.characterInputManager.SetRunInput(holding);
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x00015831 File Offset: 0x00013A31
	public void SetAdsInput(bool holding)
	{
		this.characterInputManager.SetAdsInput(holding);
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0001583F File Offset: 0x00013A3F
	public void SetGunAimInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetAimInputUsingJoystick(axisInput);
		this.characterInputManager.SetAimType(AimTypes.normalAim);
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00015859 File Offset: 0x00013A59
	public void SetCharacterSkillAimInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetAimInputUsingJoystick(axisInput);
		this.characterInputManager.SetAimType(AimTypes.characterSkill);
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00015873 File Offset: 0x00013A73
	public void StartCharacterSkillAim()
	{
		this.characterInputManager.StartCharacterSkillAim();
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00015880 File Offset: 0x00013A80
	public void CharacterSkillRelease(bool trigger)
	{
		if (!trigger)
		{
			this.characterInputManager.CancleSkill();
			return;
		}
		this.characterInputManager.ReleaseCharacterSkill();
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0001589D File Offset: 0x00013A9D
	public void SetItemSkillAimInput(Vector2 axisInput, bool holding)
	{
		this.characterInputManager.SetAimInputUsingJoystick(axisInput);
		this.characterInputManager.SetAimType(AimTypes.handheldSkill);
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x000158B7 File Offset: 0x00013AB7
	public void StartItemSkillAim()
	{
		this.characterInputManager.StartItemSkillAim();
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x000158C4 File Offset: 0x00013AC4
	public void ItemSkillRelease(bool trigger)
	{
		if (!trigger)
		{
			this.characterInputManager.CancleSkill();
			return;
		}
		this.characterInputManager.ReleaseItemSkill();
	}

	// Token: 0x040003F3 RID: 1011
	public InputManager characterInputManager;
}
