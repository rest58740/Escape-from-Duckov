using System;
using System.Diagnostics;
using System.Linq;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000003 RID: 3
	[Conditional("UNITY_EDITOR")]
	public class AssetSelectorAttribute : Attribute
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C9 File Offset: 0x000002C9
		// (set) Token: 0x06000002 RID: 2 RVA: 0x0000207C File Offset: 0x0000027C
		public string Paths
		{
			get
			{
				if (this.SearchInFolders != null)
				{
					return string.Join(",", this.SearchInFolders);
				}
				return null;
			}
			set
			{
				this.SearchInFolders = (from x in value.Split(new char[]
				{
					'|'
				})
				select x.Trim().Trim(new char[]
				{
					'/',
					'\\'
				})).ToArray<string>();
			}
		}

		// Token: 0x04000007 RID: 7
		public bool IsUniqueList = true;

		// Token: 0x04000008 RID: 8
		public bool DrawDropdownForListElements = true;

		// Token: 0x04000009 RID: 9
		public bool DisableListAddButtonBehaviour;

		// Token: 0x0400000A RID: 10
		public bool ExcludeExistingValuesInList;

		// Token: 0x0400000B RID: 11
		public bool ExpandAllMenuItems = true;

		// Token: 0x0400000C RID: 12
		public bool FlattenTreeView;

		// Token: 0x0400000D RID: 13
		public int DropdownWidth;

		// Token: 0x0400000E RID: 14
		public int DropdownHeight;

		// Token: 0x0400000F RID: 15
		public string DropdownTitle;

		// Token: 0x04000010 RID: 16
		public string[] SearchInFolders;

		// Token: 0x04000011 RID: 17
		public string Filter;
	}
}
