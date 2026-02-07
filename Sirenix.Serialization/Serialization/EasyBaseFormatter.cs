using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200002F RID: 47
	public abstract class EasyBaseFormatter<T> : BaseFormatter<T>
	{
		// Token: 0x0600026B RID: 619 RVA: 0x00011AF0 File Offset: 0x0000FCF0
		protected sealed override void DeserializeImplementation(ref T value, IDataReader reader)
		{
			int num = 0;
			string entryName;
			EntryType entryType;
			while ((entryType = reader.PeekEntry(out entryName)) != EntryType.EndOfNode && entryType != EntryType.EndOfArray && entryType != EntryType.EndOfStream)
			{
				this.ReadDataEntry(ref value, entryName, entryType, reader);
				num++;
				if (num > 1000)
				{
					reader.Context.Config.DebugContext.LogError("Breaking out of infinite reading loop!");
					return;
				}
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00011B49 File Offset: 0x0000FD49
		protected sealed override void SerializeImplementation(ref T value, IDataWriter writer)
		{
			this.WriteDataEntries(ref value, writer);
		}

		// Token: 0x0600026D RID: 621
		protected abstract void ReadDataEntry(ref T value, string entryName, EntryType entryType, IDataReader reader);

		// Token: 0x0600026E RID: 622
		protected abstract void WriteDataEntries(ref T value, IDataWriter writer);
	}
}
