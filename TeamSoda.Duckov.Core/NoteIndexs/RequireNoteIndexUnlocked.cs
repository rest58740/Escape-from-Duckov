using System;
using Duckov.Quests;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000276 RID: 630
	public class RequireNoteIndexUnlocked : Condition
	{
		// Token: 0x060013D5 RID: 5077 RVA: 0x0004A717 File Offset: 0x00048917
		public override bool Evaluate()
		{
			return NoteIndex.GetNoteUnlocked(this.key);
		}

		// Token: 0x04000ED1 RID: 3793
		public string key;
	}
}
