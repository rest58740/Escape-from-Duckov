using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class DestroyOvertime : MonoBehaviour
{
	// Token: 0x06000A8B RID: 2699 RVA: 0x0002DA11 File Offset: 0x0002BC11
	private void Awake()
	{
		if (this.life <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x0002DA2B File Offset: 0x0002BC2B
	private void Update()
	{
		this.life -= Time.deltaTime;
		if (this.life <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0002DA57 File Offset: 0x0002BC57
	private void OnValidate()
	{
		this.ProcessParticleSystem();
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0002DA60 File Offset: 0x0002BC60
	private void ProcessParticleSystem()
	{
		float num = 0f;
		ParticleSystem component = base.GetComponent<ParticleSystem>();
		if (!component)
		{
			return;
		}
		if (component != null)
		{
			ParticleSystem.MainModule main = component.main;
			main.stopAction = 0;
			if (main.startLifetime.constant > num)
			{
				num = main.startLifetime.constant;
			}
		}
		ParticleSystem[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleSystem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ParticleSystem.MainModule main2 = componentsInChildren[i].main;
			main2.stopAction = 0;
			if (main2.startLifetime.constant > num)
			{
				num = main2.startLifetime.constant;
			}
		}
		this.life = num + 0.2f;
	}

	// Token: 0x0400094A RID: 2378
	public float life = 1f;
}
