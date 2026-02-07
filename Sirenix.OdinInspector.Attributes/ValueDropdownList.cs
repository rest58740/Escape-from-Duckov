using System;
using System.Collections.Generic;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200007E RID: 126
	public class ValueDropdownList<T> : List<ValueDropdownItem<T>>
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x00004076 File Offset: 0x00002276
		public void Add(string text, T value)
		{
			base.Add(new ValueDropdownItem<T>(text, value));
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00004085 File Offset: 0x00002285
		public void Add(T value)
		{
			base.Add(new ValueDropdownItem<T>(value.ToString(), value));
		}
	}
}
