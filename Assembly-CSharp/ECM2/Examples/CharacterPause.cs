using System;
using UnityEngine;

namespace ECM2.Examples
{
	// Token: 0x0200007B RID: 123
	public class CharacterPause : MonoBehaviour
	{
		// Token: 0x060003CE RID: 974 RVA: 0x0001082B File Offset: 0x0000EA2B
		private void Awake()
		{
			this._character = base.GetComponent<Character>();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00010839 File Offset: 0x0000EA39
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				this._character.Pause(!this._character.isPaused, true);
			}
		}

		// Token: 0x04000288 RID: 648
		private Character _character;
	}
}
