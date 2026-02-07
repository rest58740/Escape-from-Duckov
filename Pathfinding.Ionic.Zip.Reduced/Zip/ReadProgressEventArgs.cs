using System;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x0200000A RID: 10
	public class ReadProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000028 RID: 40 RVA: 0x000021E8 File Offset: 0x000003E8
		internal ReadProgressEventArgs()
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000021F0 File Offset: 0x000003F0
		private ReadProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000021FC File Offset: 0x000003FC
		internal static ReadProgressEventArgs Before(string archiveName, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_BeforeReadEntry)
			{
				EntriesTotal = entriesTotal
			};
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000221C File Offset: 0x0000041C
		internal static ReadProgressEventArgs After(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_AfterReadEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002240 File Offset: 0x00000440
		internal static ReadProgressEventArgs Started(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Started);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002258 File Offset: 0x00000458
		internal static ReadProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_ArchiveBytesRead)
			{
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002284 File Offset: 0x00000484
		internal static ReadProgressEventArgs Completed(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Completed);
		}
	}
}
