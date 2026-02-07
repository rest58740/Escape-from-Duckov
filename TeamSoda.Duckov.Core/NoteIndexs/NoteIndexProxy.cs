using System;
using Duckov.UI;
using UnityEngine;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000274 RID: 628
	public class NoteIndexProxy : MonoBehaviour
	{
		// Token: 0x060013CD RID: 5069 RVA: 0x0004A654 File Offset: 0x00048854
		public void UnlockNote(string key)
		{
			NoteIndex.SetNoteUnlocked(key);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0004A65C File Offset: 0x0004885C
		public void UnlockAndShowNote(string key)
		{
			NoteIndexView.ShowNote(key, true);
		}
	}
}
