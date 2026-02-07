using System;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x0200000B RID: 11
	public class AddProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x0600002F RID: 47 RVA: 0x0000229C File Offset: 0x0000049C
		internal AddProgressEventArgs()
		{
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000022A4 File Offset: 0x000004A4
		private AddProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000022B0 File Offset: 0x000004B0
		internal static AddProgressEventArgs AfterEntry(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_AfterAddEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000022D4 File Offset: 0x000004D4
		internal static AddProgressEventArgs Started(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Started);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000022EC File Offset: 0x000004EC
		internal static AddProgressEventArgs Completed(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Completed);
		}
	}
}
