using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlexFramework.Excel
{
	// Token: 0x02000019 RID: 25
	public sealed class SharedStringCollection : ReadOnlyCollection<string>
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00003FDB File Offset: 0x000021DB
		public SharedStringCollection(IList<string> list) : base(list)
		{
		}
	}
}
