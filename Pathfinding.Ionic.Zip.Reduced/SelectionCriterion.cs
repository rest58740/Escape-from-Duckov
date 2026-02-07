using System;
using System.Diagnostics;
using Pathfinding.Ionic.Zip;

namespace Pathfinding.Ionic
{
	// Token: 0x02000019 RID: 25
	internal abstract class SelectionCriterion
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000263C File Offset: 0x0000083C
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002644 File Offset: 0x00000844
		internal virtual bool Verbose { get; set; }

		// Token: 0x0600005D RID: 93
		internal abstract bool Evaluate(string filename);

		// Token: 0x0600005E RID: 94 RVA: 0x00002650 File Offset: 0x00000850
		[Conditional("SelectorTrace")]
		protected static void CriterionTrace(string format, params object[] args)
		{
		}

		// Token: 0x0600005F RID: 95
		internal abstract bool Evaluate(ZipEntry entry);
	}
}
