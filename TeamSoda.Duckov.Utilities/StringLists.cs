using System;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000010 RID: 16
	[CreateAssetMenu(menuName = "String Lists/Collection")]
	public class StringLists : ScriptableObject
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003B5D File Offset: 0x00001D5D
		private static StringLists Default
		{
			get
			{
				if (StringLists.cachedDefault == null)
				{
					StringLists.cachedDefault = Resources.Load<StringLists>("DefaultStringLists");
				}
				return StringLists.cachedDefault;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003B80 File Offset: 0x00001D80
		public static StringList StatKeys
		{
			get
			{
				StringLists @default = StringLists.Default;
				if (@default == null)
				{
					return null;
				}
				return @default.statKeys;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003B92 File Offset: 0x00001D92
		public static StringList SlotNames
		{
			get
			{
				StringLists @default = StringLists.Default;
				if (@default == null)
				{
					return null;
				}
				return @default.slotNames;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public static StringList ItemAgentKeys
		{
			get
			{
				StringLists @default = StringLists.Default;
				if (@default == null)
				{
					return null;
				}
				return @default.itemAgentKeys;
			}
		}

		// Token: 0x0400002A RID: 42
		private static StringLists cachedDefault;

		// Token: 0x0400002B RID: 43
		[SerializeField]
		private StringList statKeys;

		// Token: 0x0400002C RID: 44
		[SerializeField]
		private StringList slotNames;

		// Token: 0x0400002D RID: 45
		[SerializeField]
		private StringList itemAgentKeys;
	}
}
