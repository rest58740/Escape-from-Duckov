using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000082 RID: 130
	public sealed class EnumSerializer<T> : Serializer<T>
	{
		// Token: 0x06000425 RID: 1061 RVA: 0x0001DEE1 File Offset: 0x0001C0E1
		static EnumSerializer()
		{
			if (!typeof(T).IsEnum)
			{
				throw new Exception("Type " + typeof(T).Name + " is not an enum.");
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001DF18 File Offset: 0x0001C118
		public override T ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				ulong num;
				if (!reader.ReadUInt64(out num))
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
				}
				return (T)((object)Enum.ToObject(typeof(T), num));
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Expected entry of type ",
				EntryType.Integer.ToString(),
				", but got entry '",
				text,
				"' of type ",
				entryType.ToString()
			}));
			reader.SkipEntry();
			return default(T);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001DFF4 File Offset: 0x0001C1F4
		public override void WriteValue(string name, T value, IDataWriter writer)
		{
			ulong value2;
			try
			{
				value2 = Convert.ToUInt64(value as Enum);
			}
			catch (OverflowException)
			{
				value2 = (ulong)Convert.ToInt64(value as Enum);
			}
			writer.WriteUInt64(name, value2);
		}
	}
}
