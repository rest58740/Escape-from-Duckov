using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class MultiInteraction : MonoBehaviour
{
	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0003D88E File Offset: 0x0003BA8E
	public ReadOnlyCollection<InteractableBase> Interactables
	{
		get
		{
			return this.interactables.AsReadOnly();
		}
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x0003D89B File Offset: 0x0003BA9B
	private void OnTriggerEnter(Collider other)
	{
		if (CharacterMainControl.Main.gameObject == other.gameObject)
		{
			MultiInteractionMenu instance = MultiInteractionMenu.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetupAndShow(this).Forget();
		}
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x0003D8C9 File Offset: 0x0003BAC9
	private void OnTriggerExit(Collider other)
	{
		if (CharacterMainControl.Main.gameObject == other.gameObject)
		{
			MultiInteractionMenu instance = MultiInteractionMenu.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Hide().Forget();
		}
	}

	// Token: 0x04000CB3 RID: 3251
	[SerializeField]
	private List<InteractableBase> interactables;
}
