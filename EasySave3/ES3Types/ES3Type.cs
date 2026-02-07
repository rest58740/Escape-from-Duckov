using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200002B RID: 43
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Preserve]
	public abstract class ES3Type
	{
		// Token: 0x0600024D RID: 589 RVA: 0x00008DF0 File Offset: 0x00006FF0
		protected ES3Type(Type type)
		{
			ES3TypeMgr.Add(type, this);
			this.type = type;
			this.isValueType = ES3Reflection.IsValueType(type);
		}

		// Token: 0x0600024E RID: 590
		public abstract void Write(object obj, ES3Writer writer);

		// Token: 0x0600024F RID: 591
		public abstract object Read<T>(ES3Reader reader);

		// Token: 0x06000250 RID: 592 RVA: 0x00008E12 File Offset: 0x00007012
		public virtual void ReadInto<T>(ES3Reader reader, object obj)
		{
			throw new NotImplementedException("Self-assigning Read is not implemented or supported on this type.");
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00008E20 File Offset: 0x00007020
		protected bool WriteUsingDerivedType(object obj, ES3Writer writer)
		{
			Type left = obj.GetType();
			if (left != this.type)
			{
				writer.WriteType(left);
				ES3TypeMgr.GetOrCreateES3Type(left, true).Write(obj, writer);
				return true;
			}
			return false;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00008E5A File Offset: 0x0000705A
		protected void ReadUsingDerivedType<T>(ES3Reader reader, object obj)
		{
			ES3TypeMgr.GetOrCreateES3Type(reader.ReadType(), true).ReadInto<T>(reader, obj);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00008E6F File Offset: 0x0000706F
		internal string ReadPropertyName(ES3Reader reader)
		{
			if (reader.overridePropertiesName != null)
			{
				string overridePropertiesName = reader.overridePropertiesName;
				reader.overridePropertiesName = null;
				return overridePropertiesName;
			}
			return reader.ReadPropertyName();
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00008E90 File Offset: 0x00007090
		protected void WriteProperties(object obj, ES3Writer writer)
		{
			if (this.members == null)
			{
				this.GetMembers(writer.settings.safeReflection);
			}
			for (int i = 0; i < this.members.Length; i++)
			{
				ES3Member es3Member = this.members[i];
				writer.WriteProperty(es3Member.name, es3Member.reflectedMember.GetValue(obj), ES3TypeMgr.GetOrCreateES3Type(es3Member.type, true), writer.settings.memberReferenceMode);
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00008F04 File Offset: 0x00007104
		protected object ReadProperties(ES3Reader reader, object obj)
		{
			foreach (object obj2 in reader.Properties)
			{
				string text = (string)obj2;
				ES3Member es3Member = null;
				for (int i = 0; i < this.members.Length; i++)
				{
					if (this.members[i].name == text)
					{
						es3Member = this.members[i];
						break;
					}
				}
				if (text == "_Values")
				{
					ES3Type orCreateES3Type = ES3TypeMgr.GetOrCreateES3Type(ES3Reflection.BaseType(obj.GetType()), true);
					if (orCreateES3Type.isDictionary)
					{
						IDictionary dictionary = (IDictionary)obj;
						using (IDictionaryEnumerator enumerator2 = ((IDictionary)orCreateES3Type.Read<IDictionary>(reader)).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj3 = enumerator2.Current;
								DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
								dictionary[dictionaryEntry.Key] = dictionaryEntry.Value;
							}
							goto IL_2AE;
						}
					}
					if (orCreateES3Type.isCollection)
					{
						IEnumerable enumerable = (IEnumerable)orCreateES3Type.Read<IEnumerable>(reader);
						Type left = orCreateES3Type.GetType();
						if (left == typeof(ES3ListType))
						{
							using (IEnumerator enumerator3 = enumerable.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									object value = enumerator3.Current;
									((IList)obj).Add(value);
								}
								goto IL_2AE;
							}
						}
						if (left == typeof(ES3QueueType))
						{
							MethodInfo method = orCreateES3Type.type.GetMethod("Enqueue");
							using (IEnumerator enumerator3 = enumerable.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									object obj4 = enumerator3.Current;
									method.Invoke(obj, new object[]
									{
										obj4
									});
								}
								goto IL_2AE;
							}
						}
						if (left == typeof(ES3StackType))
						{
							MethodInfo method2 = orCreateES3Type.type.GetMethod("Push");
							using (IEnumerator enumerator3 = enumerable.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									object obj5 = enumerator3.Current;
									method2.Invoke(obj, new object[]
									{
										obj5
									});
								}
								goto IL_2AE;
							}
						}
						if (left == typeof(ES3HashSetType))
						{
							MethodInfo method3 = orCreateES3Type.type.GetMethod("Add");
							foreach (object obj6 in enumerable)
							{
								method3.Invoke(obj, new object[]
								{
									obj6
								});
							}
						}
					}
				}
				IL_2AE:
				if (es3Member == null)
				{
					reader.Skip();
				}
				else
				{
					ES3Type orCreateES3Type2 = ES3TypeMgr.GetOrCreateES3Type(es3Member.type, true);
					if (ES3Reflection.IsAssignableFrom(typeof(ES3DictionaryType), orCreateES3Type2.GetType()))
					{
						es3Member.reflectedMember.SetValue(obj, ((ES3DictionaryType)orCreateES3Type2).Read(reader));
					}
					else if (ES3Reflection.IsAssignableFrom(typeof(ES3CollectionType), orCreateES3Type2.GetType()))
					{
						es3Member.reflectedMember.SetValue(obj, ((ES3CollectionType)orCreateES3Type2).Read(reader));
					}
					else
					{
						object value2 = reader.Read<object>(orCreateES3Type2);
						es3Member.reflectedMember.SetValue(obj, value2);
					}
				}
			}
			return obj;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00009310 File Offset: 0x00007510
		protected void GetMembers(bool safe)
		{
			this.GetMembers(safe, null);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000931C File Offset: 0x0000751C
		protected void GetMembers(bool safe, string[] memberNames)
		{
			ES3Reflection.ES3ReflectedMember[] serializableMembers = ES3Reflection.GetSerializableMembers(this.type, safe, memberNames);
			this.members = new ES3Member[serializableMembers.Length];
			for (int i = 0; i < serializableMembers.Length; i++)
			{
				this.members[i] = new ES3Member(serializableMembers[i]);
			}
		}

		// Token: 0x04000064 RID: 100
		public const string typeFieldName = "__type";

		// Token: 0x04000065 RID: 101
		public ES3Member[] members;

		// Token: 0x04000066 RID: 102
		public Type type;

		// Token: 0x04000067 RID: 103
		public bool isPrimitive;

		// Token: 0x04000068 RID: 104
		public bool isValueType;

		// Token: 0x04000069 RID: 105
		public bool isCollection;

		// Token: 0x0400006A RID: 106
		public bool isDictionary;

		// Token: 0x0400006B RID: 107
		public bool isTuple;

		// Token: 0x0400006C RID: 108
		public bool isEnum;

		// Token: 0x0400006D RID: 109
		public bool isES3TypeUnityObject;

		// Token: 0x0400006E RID: 110
		public bool isReflectedType;

		// Token: 0x0400006F RID: 111
		public bool isUnsupported;

		// Token: 0x04000070 RID: 112
		public int priority;
	}
}
