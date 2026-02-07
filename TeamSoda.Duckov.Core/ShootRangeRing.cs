using System;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class ShootRangeRing : MonoBehaviour
{
	// Token: 0x06000C3B RID: 3131 RVA: 0x0003447A File Offset: 0x0003267A
	private void Awake()
	{
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0003447C File Offset: 0x0003267C
	private void Update()
	{
		if (!this.character)
		{
			this.character = LevelManager.Instance.MainCharacter;
			this.character.OnHoldAgentChanged += this.OnAgentChanged;
			this.OnAgentChanged(this.character.CurrentHoldItemAgent);
			return;
		}
		if (this.ringRenderer.gameObject.activeInHierarchy && !this.gunAgent)
		{
			this.ringRenderer.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x00034500 File Offset: 0x00032700
	private void LateUpdate()
	{
		if (!this.character)
		{
			return;
		}
		base.transform.rotation = Quaternion.LookRotation(this.character.CurrentAimDirection, Vector3.up);
		base.transform.position = this.character.transform.position;
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x00034556 File Offset: 0x00032756
	private void OnDestroy()
	{
		if (this.character)
		{
			this.character.OnHoldAgentChanged -= this.OnAgentChanged;
		}
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x0003457C File Offset: 0x0003277C
	private void OnAgentChanged(DuckovItemAgent agent)
	{
		if (agent == null)
		{
			return;
		}
		this.gunAgent = this.character.GetGun();
		if (this.gunAgent)
		{
			this.ringRenderer.gameObject.SetActive(true);
			this.ringRenderer.transform.localScale = Vector3.one * this.character.GetAimRange() * 0.5f;
			return;
		}
		this.ringRenderer.gameObject.SetActive(false);
	}

	// Token: 0x04000A8D RID: 2701
	private CharacterMainControl character;

	// Token: 0x04000A8E RID: 2702
	public MeshRenderer ringRenderer;

	// Token: 0x04000A8F RID: 2703
	private ItemAgent_Gun gunAgent;
}
