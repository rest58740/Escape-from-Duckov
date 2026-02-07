using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200007C RID: 124
	public sealed class BooleanSerializer : Serializer<bool>
	{
		// Token: 0x06000411 RID: 1041 RVA: 0x0001CA80 File Offset: 0x0001AC80
		public override bool ReadValue(IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Boolean)
			{
				bool result;
				if (!reader.ReadBoolean(out result))
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
				}
				return result;
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Expected entry of type ",
				EntryType.Boolean.ToString(),
				", but got entry '",
				text,
				"' of type ",
				entryType.ToString()
			}));
			reader.SkipEntry();
			return false;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001CB3D File Offset: 0x0001AD3D
		public override void WriteValue(string name, bool value, IDataWriter writer)
		{
			writer.WriteBoolean(name, value);
		}
	}
}
