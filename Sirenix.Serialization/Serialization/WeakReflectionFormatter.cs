using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200004B RID: 75
	public class WeakReflectionFormatter : WeakBaseFormatter
	{
		// Token: 0x060002EE RID: 750 RVA: 0x00015848 File Offset: 0x00013A48
		public WeakReflectionFormatter(Type serializedType) : base(serializedType)
		{
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00015854 File Offset: 0x00013A54
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			Dictionary<string, MemberInfo> serializableMembersMap = FormatterUtilities.GetSerializableMembersMap(this.SerializedType, reader.Context.Config.SerializationPolicy);
			string text;
			EntryType entryType;
			while ((entryType = reader.PeekEntry(out text)) != EntryType.EndOfNode && entryType != EntryType.EndOfArray && entryType != EntryType.EndOfStream)
			{
				MemberInfo member;
				if (string.IsNullOrEmpty(text))
				{
					reader.Context.Config.DebugContext.LogError(string.Concat(new string[]
					{
						"Entry of type \"",
						entryType.ToString(),
						"\" in node \"",
						reader.CurrentNodeName,
						"\" is missing a name."
					}));
					reader.SkipEntry();
				}
				else if (!serializableMembersMap.TryGetValue(text, ref member))
				{
					reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
					{
						"Lost serialization data for entry \"",
						text,
						"\" of type \"",
						entryType.ToString(),
						"\" in node \"",
						reader.CurrentNodeName,
						"\" because a serialized member of that name could not be found in type ",
						this.SerializedType.GetNiceFullName(),
						"."
					}));
					reader.SkipEntry();
				}
				else
				{
					Type containedType = FormatterUtilities.GetContainedType(member);
					try
					{
						Serializer serializer = Serializer.Get(containedType);
						object value2 = serializer.ReadValueWeak(reader);
						FormatterUtilities.SetMemberValue(member, value, value2);
					}
					catch (Exception exception)
					{
						reader.Context.Config.DebugContext.LogException(exception);
					}
				}
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000159DC File Offset: 0x00013BDC
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			foreach (MemberInfo memberInfo in FormatterUtilities.GetSerializableMembers(this.SerializedType, writer.Context.Config.SerializationPolicy))
			{
				object memberValue = FormatterUtilities.GetMemberValue(memberInfo, value);
				Type containedType = FormatterUtilities.GetContainedType(memberInfo);
				Serializer serializer = Serializer.Get(containedType);
				try
				{
					serializer.WriteValueWeak(memberInfo.Name, memberValue, writer);
				}
				catch (Exception exception)
				{
					writer.Context.Config.DebugContext.LogException(exception);
				}
			}
		}
	}
}
