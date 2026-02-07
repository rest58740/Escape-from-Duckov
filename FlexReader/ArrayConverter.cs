using System;
using System.Linq;

namespace FlexFramework.Excel
{
	// Token: 0x02000004 RID: 4
	public sealed class ArrayConverter<T> : CustomConverter<T[]>
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000021E0 File Offset: 0x000003E0
		public override T[] Convert(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return null;
			}
			return (from p in base.Split(input, new char[]
			{
				'#'
			})
			select ValueConverter.Convert<T>(p)).ToArray<T>();
		}
	}
}
