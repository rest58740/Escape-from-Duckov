using System;

// Token: 0x020000DD RID: 221
public class InteractableCarriable : InteractableBase
{
	// Token: 0x06000717 RID: 1815 RVA: 0x000202A8 File Offset: 0x0001E4A8
	protected override void Start()
	{
		base.Start();
		this.finishWhenTimeOut = true;
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x000202B7 File Offset: 0x0001E4B7
	protected override bool IsInteractable()
	{
		return true;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x000202BA File Offset: 0x0001E4BA
	protected override void OnInteractStart(CharacterMainControl character)
	{
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x000202BC File Offset: 0x0001E4BC
	protected override void OnInteractFinished()
	{
		if (!this.interactCharacter)
		{
			return;
		}
		CharacterMainControl interactCharacter = this.interactCharacter;
		base.StopInteract();
		interactCharacter.Carry(this.carryTarget);
	}

	// Token: 0x040006DE RID: 1758
	public Carriable carryTarget;
}
