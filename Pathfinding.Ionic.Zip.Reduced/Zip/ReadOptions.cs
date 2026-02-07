using System;
using System.IO;
using System.Text;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000032 RID: 50
	public class ReadOptions
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000EB98 File Offset: 0x0000CD98
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000EBA0 File Offset: 0x0000CDA0
		public EventHandler<ReadProgressEventArgs> ReadProgress { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000EBAC File Offset: 0x0000CDAC
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
		public TextWriter StatusMessageWriter { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000EBC0 File Offset: 0x0000CDC0
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000EBC8 File Offset: 0x0000CDC8
		public Encoding Encoding { get; set; }
	}
}
