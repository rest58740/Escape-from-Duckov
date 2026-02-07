using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class CharacterSoundMaker : MonoBehaviour
{
	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x0600039A RID: 922 RVA: 0x0000FCFD File Offset: 0x0000DEFD
	public float walkSoundDistance
	{
		get
		{
			if (!this.characterMainControl)
			{
				return 0f;
			}
			return this.characterMainControl.WalkSoundRange;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x0600039B RID: 923 RVA: 0x0000FD1D File Offset: 0x0000DF1D
	public float runSoundDistance
	{
		get
		{
			if (!this.characterMainControl)
			{
				return 0f;
			}
			return this.characterMainControl.RunSoundRange;
		}
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0000FD40 File Offset: 0x0000DF40
	private void Update()
	{
		if (this.characterMainControl.movementControl.Velocity.magnitude < 0.5f)
		{
			this.moveSoundTimer = 0f;
			return;
		}
		this.moveSoundTimer += Time.deltaTime;
		bool running = this.characterMainControl.Running;
		float num = 1f / (running ? this.runSoundFrequence : this.walkSoundFrequence);
		if (this.moveSoundTimer >= num)
		{
			this.moveSoundTimer = 0f;
			if (this.characterMainControl.IsInAdsInput)
			{
				return;
			}
			if (!this.characterMainControl.CharacterItem)
			{
				return;
			}
			bool flag = this.characterMainControl.CharacterItem.TotalWeight / this.characterMainControl.MaxWeight >= 0.75f;
			AISound sound = default(AISound);
			sound.pos = base.transform.position;
			sound.fromTeam = this.characterMainControl.Team;
			sound.soundType = SoundTypes.unknowNoise;
			sound.fromObject = this.characterMainControl.gameObject;
			sound.fromCharacter = this.characterMainControl;
			if (this.characterMainControl.Running)
			{
				if (this.runSoundDistance > 0f)
				{
					sound.radius = this.runSoundDistance * (flag ? 1.5f : 1f);
					Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl> onFootStepSound = CharacterSoundMaker.OnFootStepSound;
					if (onFootStepSound != null)
					{
						onFootStepSound(base.transform.position, flag ? CharacterSoundMaker.FootStepTypes.runHeavy : CharacterSoundMaker.FootStepTypes.runLight, this.characterMainControl);
					}
				}
			}
			else if (this.walkSoundDistance > 0f)
			{
				sound.radius = this.walkSoundDistance * (flag ? 1.5f : 1f);
				Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl> onFootStepSound2 = CharacterSoundMaker.OnFootStepSound;
				if (onFootStepSound2 != null)
				{
					onFootStepSound2(base.transform.position, flag ? CharacterSoundMaker.FootStepTypes.walkHeavy : CharacterSoundMaker.FootStepTypes.walkLight, this.characterMainControl);
				}
			}
			AIMainBrain.MakeSound(sound);
		}
	}

	// Token: 0x040002B1 RID: 689
	public CharacterMainControl characterMainControl;

	// Token: 0x040002B2 RID: 690
	private float moveSoundTimer;

	// Token: 0x040002B3 RID: 691
	public float walkSoundFrequence = 4f;

	// Token: 0x040002B4 RID: 692
	public float runSoundFrequence = 7f;

	// Token: 0x040002B5 RID: 693
	public static Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl> OnFootStepSound;

	// Token: 0x0200044F RID: 1103
	public enum FootStepTypes
	{
		// Token: 0x04001AF4 RID: 6900
		walkLight,
		// Token: 0x04001AF5 RID: 6901
		walkHeavy,
		// Token: 0x04001AF6 RID: 6902
		runLight,
		// Token: 0x04001AF7 RID: 6903
		runHeavy
	}
}
