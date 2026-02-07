using System;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x0200000E RID: 14
	public class ZipErrorEventArgs : ZipProgressEventArgs
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00002538 File Offset: 0x00000738
		private ZipErrorEventArgs()
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002540 File Offset: 0x00000740
		internal static ZipErrorEventArgs Saving(string archiveName, ZipEntry entry, Exception exception)
		{
			return new ZipErrorEventArgs
			{
				EventType = ZipProgressEventType.Error_Saving,
				ArchiveName = archiveName,
				CurrentEntry = entry,
				_exc = exception
			};
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002574 File Offset: 0x00000774
		public Exception Exception
		{
			get
			{
				return this._exc;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000257C File Offset: 0x0000077C
		public string FileName
		{
			get
			{
				return base.CurrentEntry.LocalFileName;
			}
		}

		// Token: 0x04000028 RID: 40
		private Exception _exc;
	}
}
