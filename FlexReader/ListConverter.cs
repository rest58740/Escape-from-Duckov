using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexFramework.Excel
{
	// Token: 0x02000008 RID: 8
	public sealed class ListConverter<T> : CustomConverter<List<T>>
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002588 File Offset: 0x00000788
		public override List<T> Convert(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return null;
			}
			return (from p in base.Split(input, new char[]
			{
				'#'
			})
			select ValueConverter.Convert<T>(p)).ToList<T>();
		}
	}
}
