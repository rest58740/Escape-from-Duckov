using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000CF RID: 207
	[AttributeUsage(256)]
	public class ListInspectorOptionAttribute : Attribute
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x000170D9 File Offset: 0x000152D9
		public ListInspectorOptionAttribute(bool allowAdd, bool allowRemove, bool alwaysExpanded)
		{
			this.allowAdd = allowAdd;
			this.allowRemove = allowRemove;
			this.showFoldout = alwaysExpanded;
		}

		// Token: 0x0400023A RID: 570
		public readonly bool allowAdd;

		// Token: 0x0400023B RID: 571
		public readonly bool allowRemove;

		// Token: 0x0400023C RID: 572
		public readonly bool showFoldout;
	}
}
