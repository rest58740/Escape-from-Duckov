using System;
using System.Diagnostics;

namespace Animancer.Units
{
	// Token: 0x02000076 RID: 118
	[Conditional("UNITY_EDITOR")]
	public class UnitsAttribute : SelfDrawerAttribute
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x0000EE9E File Offset: 0x0000D09E
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x0000EEA6 File Offset: 0x0000D0A6
		public Validate.Value Rule { get; set; }

		// Token: 0x0600059A RID: 1434 RVA: 0x0000EEAF File Offset: 0x0000D0AF
		protected UnitsAttribute()
		{
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0000EEB7 File Offset: 0x0000D0B7
		public UnitsAttribute(string suffix)
		{
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0000EEBF File Offset: 0x0000D0BF
		public UnitsAttribute(float[] multipliers, string[] suffixes, int unitIndex = 0)
		{
		}
	}
}
