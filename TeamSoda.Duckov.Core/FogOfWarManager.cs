using System;
using FOW;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000182 RID: 386
public class FogOfWarManager : MonoBehaviour
{
	// Token: 0x06000BEA RID: 3050 RVA: 0x00032CC5 File Offset: 0x00030EC5
	private void Start()
	{
		LevelManager.OnMainCharacterDead += this.OnCharacterDie;
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x00032CD8 File Offset: 0x00030ED8
	private void OnDestroy()
	{
		LevelManager.OnMainCharacterDead -= this.OnCharacterDie;
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00032CEB File Offset: 0x00030EEB
	private void Init()
	{
		this.inited = true;
		if (!LevelManager.Instance.IsRaidMap || !LevelManager.Rule.FogOfWar)
		{
			this.allVision = true;
		}
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x00032D14 File Offset: 0x00030F14
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.character)
		{
			this.character = CharacterMainControl.Main;
			if (!this.character)
			{
				return;
			}
		}
		if (!this.inited)
		{
			this.Init();
		}
		if (!this.timeOfDayController)
		{
			this.timeOfDayController = LevelManager.Instance.TimeOfDayController;
			if (!this.timeOfDayController)
			{
				return;
			}
		}
		Vector3 vector = this.character.transform.position + Vector3.up * this.mianVisYOffset;
		this.mainVis.transform.position = vector;
		vector..ctor((float)Mathf.RoundToInt(vector.x), (float)Mathf.RoundToInt(vector.y), (float)Mathf.RoundToInt(vector.z));
		this.fogOfWar.UpdateWorldBounds(vector, new Vector3(128f, 1f, 128f));
		Vector3 vector2 = this.character.GetCurrentAimPoint() - this.character.transform.position;
		Debug.DrawLine(this.character.GetCurrentAimPoint(), this.character.GetCurrentAimPoint() + Vector3.up * 2f, Color.green, 0.2f);
		vector2.y = 0f;
		vector2.Normalize();
		float num = Mathf.Clamp01(this.character.NightVisionAbility + (this.character.FlashLight ? 0.3f : 0f));
		float num2 = this.character.ViewAngle;
		float num3 = this.character.SenseRange;
		float num4 = this.character.ViewDistance;
		num2 *= Mathf.Lerp(TimeOfDayController.NightViewAngleFactor, 1f, num);
		num3 *= Mathf.Lerp(TimeOfDayController.NightSenseRangeFactor, 1f, num);
		num4 *= Mathf.Lerp(TimeOfDayController.NightViewDistanceFactor, 1f, num);
		if (num4 < num3 - 2.5f)
		{
			num4 = num3 - 2.5f;
		}
		if (this.allVision)
		{
			num2 = 360f;
			num3 = 50f;
			num4 = 50f;
		}
		if (num2 != this.viewAgnel)
		{
			if (this.viewAgnel < 0f)
			{
				this.viewAgnel = num2;
			}
			this.viewAgnel = Mathf.MoveTowards(this.viewAgnel, num2, 180f * Time.deltaTime);
			this.mainVis.ViewAngle = this.viewAgnel;
		}
		if (num3 != this.senseRange)
		{
			if (this.senseRange < 0f)
			{
				this.senseRange = num3;
			}
			this.senseRange = Mathf.MoveTowards(this.senseRange, num3, 15f * Time.deltaTime);
			this.mainVis.UnobscuredRadius = this.senseRange;
		}
		if (num4 != this.viewDistance)
		{
			if (this.viewDistance < 0f)
			{
				this.viewDistance = num4;
			}
			this.viewDistance = Mathf.MoveTowards(this.viewDistance, num4, 30f * Time.deltaTime);
			this.mainVis.ViewRadius = this.viewDistance;
		}
		this.mainVis.transform.rotation = Quaternion.LookRotation(vector2, Vector3.up);
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00033039 File Offset: 0x00031239
	private void OnCharacterDie(DamageInfo dmgInfo)
	{
		LevelManager.OnMainCharacterDead -= this.OnCharacterDie;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000A32 RID: 2610
	[FormerlySerializedAs("mianVis")]
	public FogOfWarRevealer3D mainVis;

	// Token: 0x04000A33 RID: 2611
	public float mianVisYOffset = 1f;

	// Token: 0x04000A34 RID: 2612
	private CharacterMainControl character;

	// Token: 0x04000A35 RID: 2613
	public FogOfWarWorld fogOfWar;

	// Token: 0x04000A36 RID: 2614
	private float viewAgnel = -1f;

	// Token: 0x04000A37 RID: 2615
	private float senseRange = -1f;

	// Token: 0x04000A38 RID: 2616
	private float viewDistance = -1f;

	// Token: 0x04000A39 RID: 2617
	private TimeOfDayController timeOfDayController;

	// Token: 0x04000A3A RID: 2618
	private bool allVision;

	// Token: 0x04000A3B RID: 2619
	private bool inited;
}
