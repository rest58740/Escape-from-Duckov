using System;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public class InteractablePMC : InteractableBase
{
	// Token: 0x06000741 RID: 1857 RVA: 0x00020EDF File Offset: 0x0001F0DF
	protected override bool IsInteractable()
	{
		return true;
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00020EE2 File Offset: 0x0001F0E2
	protected override void Awake()
	{
		this.finishWhenTimeOut = true;
		this.disableOnFinish = true;
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00020EF2 File Offset: 0x0001F0F2
	protected override void OnInteractStart(CharacterMainControl character)
	{
		this.targetCharacter = character;
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00020EFC File Offset: 0x0001F0FC
	protected override void OnInteractFinished()
	{
		if (this.specialAttachment == null || this.specialAttachment.aiCharacterController == null || this.targetCharacter == null)
		{
			return;
		}
		this.specialAttachment.aiCharacterController.leader = this.targetCharacter;
		this.specialAttachment.aiCharacterController.TakeOutWeapon();
	}

	// Token: 0x040006F3 RID: 1779
	[SerializeField]
	private AISpecialAttachmentBase specialAttachment;

	// Token: 0x040006F4 RID: 1780
	private CharacterMainControl targetCharacter;
}
