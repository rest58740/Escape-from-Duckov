using System;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class PetHouse : MonoBehaviour
{
	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000997 RID: 2455 RVA: 0x0002AEAC File Offset: 0x000290AC
	public static PetHouse Instance
	{
		get
		{
			return PetHouse.instance;
		}
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0002AEB3 File Offset: 0x000290B3
	private void Awake()
	{
		PetHouse.instance = this;
		if (LevelManager.LevelInited)
		{
			this.OnLevelInited();
			return;
		}
		LevelManager.OnLevelInitialized += this.OnLevelInited;
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0002AEDA File Offset: 0x000290DA
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.OnLevelInited;
		if (this.petTarget)
		{
			this.petTarget.SetStandBy(false, this.petTarget.transform.position);
		}
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0002AF18 File Offset: 0x00029118
	private void OnLevelInited()
	{
		CharacterMainControl petCharacter = LevelManager.Instance.PetCharacter;
		petCharacter.SetPosition(this.petMarker.position);
		this.petTarget = petCharacter.GetComponentInChildren<PetAI>();
		if (this.petTarget != null)
		{
			this.petTarget.SetStandBy(true, this.petMarker.position);
		}
	}

	// Token: 0x040008A3 RID: 2211
	private static PetHouse instance;

	// Token: 0x040008A4 RID: 2212
	public Transform petMarker;

	// Token: 0x040008A5 RID: 2213
	private PetAI petTarget;
}
