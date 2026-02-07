using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class CharacterItemControl : MonoBehaviour
{
	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000299 RID: 665 RVA: 0x0000BC9B File Offset: 0x00009E9B
	private Inventory inventory
	{
		get
		{
			return this.characterMainControl.CharacterItem.Inventory;
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000BCB0 File Offset: 0x00009EB0
	public bool PickupItem(Item item)
	{
		if (item == null)
		{
			return false;
		}
		if (this.inventory != null)
		{
			item.AgentUtilities.ReleaseActiveAgent();
			item.Detach();
			bool? flag = new bool?(this.characterMainControl.CharacterItem.TryPlug(item, true, null, 0));
			bool flag2;
			if (flag == null || !flag.Value)
			{
				if (this.characterMainControl.IsMainCharacter)
				{
					flag2 = ItemUtilities.SendToPlayerCharacterInventory(item, false);
				}
				else
				{
					flag2 = this.characterMainControl.CharacterItem.Inventory.AddAndMerge(item, 0);
				}
			}
			else
			{
				flag2 = true;
			}
			if (flag2)
			{
				return true;
			}
		}
		item.Drop(base.transform.position, true, Vector3.forward, 360f);
		return false;
	}

	// Token: 0x04000217 RID: 535
	public CharacterMainControl characterMainControl;
}
