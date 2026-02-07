using System;
using System.Collections.Generic;

namespace MiniExcelLibs.Utils
{
	// Token: 0x0200002E RID: 46
	internal static class Evaluator
	{
		// Token: 0x0600013C RID: 316 RVA: 0x000054E0 File Offset: 0x000036E0
		public static Section GetSection(List<Section> sections, object value)
		{
			if (!(value is string))
			{
				if (value is DateTime)
				{
					DateTime dateTime = (DateTime)value;
					return Evaluator.GetFirstSection(sections, SectionType.Date);
				}
				if (value is TimeSpan)
				{
					return Evaluator.GetNumericSection(sections, ((TimeSpan)value).TotalDays);
				}
				if (value is double)
				{
					double value2 = (double)value;
					return Evaluator.GetNumericSection(sections, value2);
				}
				if (value is int)
				{
					int num = (int)value;
					return Evaluator.GetNumericSection(sections, (double)num);
				}
				if (value is short)
				{
					short num2 = (short)value;
					return Evaluator.GetNumericSection(sections, (double)num2);
				}
				return null;
			}
			else
			{
				if (sections.Count >= 4)
				{
					return sections[3];
				}
				return null;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005590 File Offset: 0x00003790
		public static Section GetFirstSection(List<Section> sections, SectionType type)
		{
			foreach (Section section in sections)
			{
				if (section.Type == type)
				{
					return section;
				}
			}
			return null;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000055E8 File Offset: 0x000037E8
		private static Section GetNumericSection(List<Section> sections, double value)
		{
			if (sections.Count < 3)
			{
				return null;
			}
			return sections[2];
		}
	}
}
