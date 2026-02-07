using System;
using UnityEngine;

namespace Duckov.MiniGames.Debugging
{
	// Token: 0x020002DC RID: 732
	public class HideAndLockCursor : MonoBehaviour
	{
		// Token: 0x0600176B RID: 5995 RVA: 0x000565BB File Offset: 0x000547BB
		private void OnEnable()
		{
			Cursor.visible = false;
			Cursor.lockState = 1;
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x000565C9 File Offset: 0x000547C9
		private void OnDisable()
		{
			Cursor.visible = true;
			Cursor.lockState = 0;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x000565D7 File Offset: 0x000547D7
		private void Update()
		{
			if (Input.GetKeyDown(27))
			{
				base.enabled = false;
			}
		}
	}
}
