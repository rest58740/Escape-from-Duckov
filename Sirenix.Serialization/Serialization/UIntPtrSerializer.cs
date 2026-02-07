using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000090 RID: 144
	public sealed class UIntPtrSerializer : Serializer<UIntPtr>
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x0001EE70 File Offset: 0x0001D070
		public override UIntPtr ReadValue(IDataReader reader)
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
				return new UIntPtr(num);
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
			return (UIntPtr)0;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001EF33 File Offset: 0x0001D133
		public override void WriteValue(string name, UIntPtr value, IDataWriter writer)
		{
			writer.WriteUInt64(name, (ulong)value);
		}
	}
}
