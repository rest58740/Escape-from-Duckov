using System;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class HalfObsticle : MonoBehaviour
{
	// Token: 0x060003A6 RID: 934 RVA: 0x00010344 File Offset: 0x0000E544
	private void Awake()
	{
		this.outline.enabled = false;
		this.defaultVisuals.SetActive(true);
		this.deadVisuals.SetActive(false);
		this.health.OnDeadEvent += this.Dead;
		if (this.airWallCollider)
		{
			this.airWallCollider.gameObject.SetActive(true);
		}
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x000103AA File Offset: 0x0000E5AA
	private void OnValidate()
	{
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x000103AC File Offset: 0x0000E5AC
	public void Dead(DamageInfo dmgInfo)
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.defaultVisuals.SetActive(false);
		this.deadVisuals.SetActive(true);
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x000103D8 File Offset: 0x0000E5D8
	public void OnTriggerEnter(Collider other)
	{
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (!component)
		{
			return;
		}
		component.AddnearByHalfObsticles(this.parts);
		if (component.IsMainCharacter)
		{
			this.outline.enabled = true;
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00010418 File Offset: 0x0000E618
	public void OnTriggerExit(Collider other)
	{
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (!component)
		{
			return;
		}
		component.RemoveNearByHalfObsticles(this.parts);
		if (component.IsMainCharacter)
		{
			this.outline.enabled = false;
		}
	}

	// Token: 0x040002C3 RID: 707
	public Outlinable outline;

	// Token: 0x040002C4 RID: 708
	public HealthSimpleBase health;

	// Token: 0x040002C5 RID: 709
	public List<GameObject> parts;

	// Token: 0x040002C6 RID: 710
	public GameObject defaultVisuals;

	// Token: 0x040002C7 RID: 711
	public GameObject deadVisuals;

	// Token: 0x040002C8 RID: 712
	public Collider airWallCollider;

	// Token: 0x040002C9 RID: 713
	private bool dead;
}
