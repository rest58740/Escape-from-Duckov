using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200004A RID: 74
	public class ReflectionFormatter<T> : BaseFormatter<T>
	{
		// Token: 0x060002E8 RID: 744 RVA: 0x000155C4 File Offset: 0x000137C4
		public ReflectionFormatter()
		{
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000155CC File Offset: 0x000137CC
		public ReflectionFormatter(ISerializationPolicy overridePolicy)
		{
			this.OverridePolicy = overridePolicy;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060002EA RID: 746 RVA: 0x000155DB File Offset: 0x000137DB
		// (set) Token: 0x060002EB RID: 747 RVA: 0x000155E3 File Offset: 0x000137E3
		public ISerializationPolicy OverridePolicy { get; private set; }

		// Token: 0x060002EC RID: 748 RVA: 0x000155EC File Offset: 0x000137EC
		protected override void DeserializeImplementation(ref T value, IDataReader reader)
		{
			object obj = value;
			Dictionary<string, MemberInfo> serializableMembersMap = FormatterUtilities.GetSerializableMembersMap(typeof(T), this.OverridePolicy ?? reader.Context.Config.SerializationPolicy);
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
						typeof(T).GetNiceFullName(),
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
						FormatterUtilities.SetMemberValue(member, obj, value2);
					}
					catch (Exception exception)
					{
						reader.Context.Config.DebugContext.LogException(exception);
					}
				}
			}
			value = (T)((object)obj);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000157A0 File Offset: 0x000139A0
		protected override void SerializeImplementation(ref T value, IDataWriter writer)
		{
			foreach (MemberInfo memberInfo in FormatterUtilities.GetSerializableMembers(typeof(T), this.OverridePolicy ?? writer.Context.Config.SerializationPolicy))
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
