using System;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x0200000C RID: 12
	public class SaveProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002304 File Offset: 0x00000504
		internal SaveProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesSaved, ZipEntry entry) : base(archiveName, (!before) ? ZipProgressEventType.Saving_AfterWriteEntry : ZipProgressEventType.Saving_BeforeWriteEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			this._entriesSaved = entriesSaved;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002340 File Offset: 0x00000540
		internal SaveProgressEventArgs()
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002348 File Offset: 0x00000548
		internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002354 File Offset: 0x00000554
		internal static SaveProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002388 File Offset: 0x00000588
		internal static SaveProgressEventArgs Started(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000023A0 File Offset: 0x000005A0
		internal static SaveProgressEventArgs Completed(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000023B8 File Offset: 0x000005B8
		public int EntriesSaved
		{
			get
			{
				return this._entriesSaved;
			}
		}

		// Token: 0x04000025 RID: 37
		private int _entriesSaved;
	}
}
