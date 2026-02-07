using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000087 RID: 135
	public sealed class IntPtrSerializer : Serializer<IntPtr>
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x0001E390 File Offset: 0x0001C590
		public override IntPtr ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer)
			{
				long num;
				if (!reader.ReadInt64(out num))
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
				}
				return new IntPtr(num);
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
			return (IntPtr)0;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001E453 File Offset: 0x0001C653
		public override void WriteValue(string name, IntPtr value, IDataWriter writer)
		{
			writer.WriteInt64(name, (long)value);
		}
	}
}
