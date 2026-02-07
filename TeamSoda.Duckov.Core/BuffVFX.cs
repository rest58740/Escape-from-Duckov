using System;
using Duckov.Buffs;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200017C RID: 380
public class BuffVFX : MonoBehaviour
{
	// Token: 0x06000BC7 RID: 3015 RVA: 0x00032663 File Offset: 0x00030863
	private void Awake()
	{
		if (!this.buff)
		{
			this.buff = base.GetComponent<Buff>();
		}
		this.buff.OnSetupEvent.AddListener(new UnityAction(this.OnSetup));
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x0003269C File Offset: 0x0003089C
	private void OnSetup()
	{
		if (this.shockFxInstance != null)
		{
			Object.Destroy(this.shockFxInstance);
		}
		if (!this.buff || !this.buff.Character || !this.shockFxPfb)
		{
			return;
		}
		this.shockFxInstance = Object.Instantiate<GameObject>(this.shockFxPfb, this.buff.Character.transform);
		this.shockFxInstance.transform.localPosition = this.offsetFromCharacter;
		this.shockFxInstance.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x0003273B File Offset: 0x0003093B
	private void OnDestroy()
	{
		if (this.shockFxInstance != null)
		{
			Object.Destroy(this.shockFxInstance);
		}
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00032756 File Offset: 0x00030956
	public void AutoSetup()
	{
		this.buff = base.GetComponent<Buff>();
	}

	// Token: 0x04000A16 RID: 2582
	public Buff buff;

	// Token: 0x04000A17 RID: 2583
	public GameObject shockFxPfb;

	// Token: 0x04000A18 RID: 2584
	private GameObject shockFxInstance;

	// Token: 0x04000A19 RID: 2585
	public Vector3 offsetFromCharacter;
}
