using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x0200000F RID: 15
	[CreateAssetMenu(menuName = "String Lists/List")]
	public class StringList : ScriptableObject, IEnumerable<string>, IEnumerable
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003B0F File Offset: 0x00001D0F
		public ReadOnlyCollection<string> Strings
		{
			get
			{
				if (this.strings_ReadOnly == null)
				{
					this.strings_ReadOnly = new ReadOnlyCollection<string>(this.strings);
				}
				return this.strings_ReadOnly;
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003B30 File Offset: 0x00001D30
		public IEnumerator<string> GetEnumerator()
		{
			return this.strings.GetEnumerator();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003B42 File Offset: 0x00001D42
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000028 RID: 40
		[SerializeField]
		private List<string> strings = new List<string>();

		// Token: 0x04000029 RID: 41
		private ReadOnlyCollection<string> strings_ReadOnly;
	}
}
