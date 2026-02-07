using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItemStatsSystem
{
	// Token: 0x02000235 RID: 565
	public class TestitemGraphic : MonoBehaviour
	{
		// Token: 0x0600119E RID: 4510 RVA: 0x00044C23 File Offset: 0x00042E23
		private void Start()
		{
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00044C28 File Offset: 0x00042E28
		private void Update()
		{
			if (Keyboard.current.gKey.wasPressedThisFrame)
			{
				if (this.instance)
				{
					Object.Destroy(this.instance.gameObject);
				}
				DuckovItemAgent currentHoldItemAgent = CharacterMainControl.Main.CurrentHoldItemAgent;
				if (!currentHoldItemAgent)
				{
					return;
				}
				this.instance = ItemGraphicInfo.CreateAGraphic(currentHoldItemAgent.Item, base.transform);
			}
		}

		// Token: 0x04000DCD RID: 3533
		private ItemGraphicInfo instance;
	}
}
