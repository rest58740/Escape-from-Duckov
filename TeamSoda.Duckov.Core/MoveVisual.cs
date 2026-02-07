using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class MoveVisual : MonoBehaviour
{
	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06000C06 RID: 3078 RVA: 0x00033585 File Offset: 0x00031785
	private CharacterMainControl Character
	{
		get
		{
			if (!this.characterModel)
			{
				return null;
			}
			return this.characterModel.characterMainControl;
		}
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x000335A4 File Offset: 0x000317A4
	private void Awake()
	{
		foreach (ParticleSystem particleSystem in this.runParticles)
		{
			particleSystem.emission.enabled = this.running;
		}
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x00033604 File Offset: 0x00031804
	private void Update()
	{
		if (!this.Character)
		{
			return;
		}
		if (this.Character.Running != this.running)
		{
			this.running = this.Character.Running;
			foreach (ParticleSystem particleSystem in this.runParticles)
			{
				particleSystem.emission.enabled = this.running;
			}
		}
	}

	// Token: 0x04000A51 RID: 2641
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x04000A52 RID: 2642
	public List<ParticleSystem> runParticles;

	// Token: 0x04000A53 RID: 2643
	private bool running;
}
