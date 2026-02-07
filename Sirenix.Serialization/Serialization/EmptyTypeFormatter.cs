using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000031 RID: 49
	public class EmptyTypeFormatter<T> : EasyBaseFormatter<T>
	{
		// Token: 0x06000271 RID: 625 RVA: 0x00011B63 File Offset: 0x0000FD63
		protected override void ReadDataEntry(ref T value, string entryName, EntryType entryType, IDataReader reader)
		{
			reader.SkipEntry();
		}

		// Token: 0x06000272 RID: 626 RVA: 0x000021B8 File Offset: 0x000003B8
		protected override void WriteDataEntries(ref T value, IDataWriter writer)
		{
		}
	}
}
