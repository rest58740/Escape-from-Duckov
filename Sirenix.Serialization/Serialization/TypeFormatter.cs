using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000054 RID: 84
	public sealed class TypeFormatter : MinimalBaseFormatter<Type>
	{
		// Token: 0x06000313 RID: 787 RVA: 0x00016968 File Offset: 0x00014B68
		protected override void Read(ref Type value, IDataReader reader)
		{
			string typeName;
			if (reader.PeekEntry(out typeName) == EntryType.String)
			{
				reader.ReadString(out typeName);
				value = reader.Context.Binder.BindToType(typeName, reader.Context.Config.DebugContext);
				if (value != null)
				{
					base.RegisterReferenceID(value, reader);
				}
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x000169BF File Offset: 0x00014BBF
		protected override void Write(ref Type value, IDataWriter writer)
		{
			writer.WriteString(null, writer.Context.Binder.BindToName(value, writer.Context.Config.DebugContext));
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override Type GetUninitializedObject()
		{
			return null;
		}
	}
}
