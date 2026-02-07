using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A6 RID: 166
public class FishingPoint : MonoBehaviour
{
	// Token: 0x060005C8 RID: 1480 RVA: 0x0001A18B File Offset: 0x0001838B
	private void Awake()
	{
		this.OnPlayerTakeFishingRod(null);
		this.Interactable.OnInteractFinishedEvent.AddListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractFinished));
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0001A1B0 File Offset: 0x000183B0
	private void OnDestroy()
	{
		if (this.Interactable)
		{
			this.Interactable.OnInteractFinishedEvent.RemoveListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractFinished));
		}
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0001A1DB File Offset: 0x000183DB
	private void OnPlayerTakeFishingRod(FishingRod rod)
	{
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0001A1E0 File Offset: 0x000183E0
	private void OnInteractFinished(CharacterMainControl character, InteractableBase interact)
	{
		if (!character)
		{
			return;
		}
		character.SetPosition(this.playerPoint.position);
		character.SetAimPoint(this.playerPoint.position + this.playerPoint.forward * 10f);
		character.movementControl.SetAimDirection(this.playerPoint.forward);
		character.StartAction(this.action);
	}

	// Token: 0x0400054D RID: 1357
	public InteractableBase Interactable;

	// Token: 0x0400054E RID: 1358
	public Action_Fishing action;

	// Token: 0x0400054F RID: 1359
	public Transform playerPoint;
}
