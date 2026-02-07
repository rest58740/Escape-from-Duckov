using System;

// Token: 0x02000165 RID: 357
public interface ISingleSelectionMenu<EntryType> where EntryType : class
{
	// Token: 0x06000B1A RID: 2842
	EntryType GetSelection();

	// Token: 0x06000B1B RID: 2843
	bool SetSelection(EntryType selection);
}
