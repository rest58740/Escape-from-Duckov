using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class DoorTrigger : MonoBehaviour
{
	// Token: 0x060006EC RID: 1772 RVA: 0x0001F4D0 File Offset: 0x0001D6D0
	private void OnTriggerEnter(Collider collision)
	{
		if (this.parent.IsOpen)
		{
			return;
		}
		if (!this.parent.NoRequireItem)
		{
			return;
		}
		if (this.parent.Interact && !this.parent.Interact.gameObject.activeInHierarchy)
		{
			return;
		}
		if (collision.gameObject.layer != LayerMask.NameToLayer("Character"))
		{
			return;
		}
		CharacterMainControl component = collision.gameObject.GetComponent<CharacterMainControl>();
		if (!component || component.Team == Teams.player)
		{
			return;
		}
		this.parent.Open();
	}

	// Token: 0x040006BB RID: 1723
	public Door parent;
}
