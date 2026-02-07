using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200004C RID: 76
	public abstract class ReflectionOrEmittedBaseFormatter<T> : ReflectionFormatter<T>
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x00015A70 File Offset: 0x00013C70
		protected override void DeserializeImplementation(ref T value, IDataReader reader)
		{
			FormatterEmitter.RuntimeEmittedFormatter<T> runtimeEmittedFormatter = FormatterEmitter.GetEmittedFormatter(typeof(T), reader.Context.Config.SerializationPolicy) as FormatterEmitter.RuntimeEmittedFormatter<T>;
			if (runtimeEmittedFormatter == null)
			{
				return;
			}
			int num = 0;
			string entryName;
			EntryType entryType;
			while ((entryType = reader.PeekEntry(out entryName)) != EntryType.EndOfNode && entryType != EntryType.EndOfArray && entryType != EntryType.EndOfStream)
			{
				runtimeEmittedFormatter.Read(ref value, entryName, entryType, reader);
				num++;
				if (num > 1000)
				{
					reader.Context.Config.DebugContext.LogError("Breaking out of infinite reading loop!");
					return;
				}
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00015AF8 File Offset: 0x00013CF8
		protected override void SerializeImplementation(ref T value, IDataWriter writer)
		{
			FormatterEmitter.RuntimeEmittedFormatter<T> runtimeEmittedFormatter = FormatterEmitter.GetEmittedFormatter(typeof(T), writer.Context.Config.SerializationPolicy) as FormatterEmitter.RuntimeEmittedFormatter<T>;
			if (runtimeEmittedFormatter == null)
			{
				return;
			}
			runtimeEmittedFormatter.Write(ref value, writer);
		}
	}
}
