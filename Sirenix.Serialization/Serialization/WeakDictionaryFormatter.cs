using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200002C RID: 44
	internal sealed class WeakDictionaryFormatter : WeakBaseFormatter
	{
		// Token: 0x0600025E RID: 606 RVA: 0x0001106C File Offset: 0x0000F26C
		public WeakDictionaryFormatter(Type serializedType) : base(serializedType)
		{
			Type[] argumentsOfInheritedOpenGenericClass = serializedType.GetArgumentsOfInheritedOpenGenericClass(typeof(Dictionary));
			this.KeyType = argumentsOfInheritedOpenGenericClass[0];
			this.ValueType = argumentsOfInheritedOpenGenericClass[1];
			this.KeyIsValueType = this.KeyType.IsValueType;
			this.KeyReaderWriter = Serializer.Get(this.KeyType);
			this.ValueReaderWriter = Serializer.Get(this.ValueType);
			this.CountProperty = serializedType.GetProperty("Count");
			if (this.CountProperty == null)
			{
				throw new SerializationAbortException("Can't serialize/deserialize the type " + serializedType.GetNiceFullName() + " because it has no accessible Count property.");
			}
			try
			{
				Type type = typeof(IEqualityComparer).MakeGenericType(new Type[]
				{
					this.KeyType
				});
				this.EqualityComparerSerializer = Serializer.Get(type);
				this.ComparerConstructor = serializedType.GetConstructor(new Type[]
				{
					type
				});
				this.ComparerProperty = serializedType.GetProperty("Comparer");
			}
			catch (Exception)
			{
				this.EqualityComparerSerializer = Serializer.Get<object>();
				this.ComparerConstructor = null;
				this.ComparerProperty = null;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00011194 File Offset: 0x0000F394
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			object obj = null;
			if (text == "comparer" || entryType == EntryType.StartOfNode)
			{
				obj = this.EqualityComparerSerializer.ReadValueWeak(reader);
				entryType = reader.PeekEntry(out text);
			}
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					if (obj != null && this.ComparerConstructor != null)
					{
						value = this.ComparerConstructor.Invoke(new object[]
						{
							obj
						});
					}
					else
					{
						value = Activator.CreateInstance(this.SerializedType);
					}
					IDictionary dictionary = (IDictionary)value;
					base.RegisterReferenceID(value, reader);
					int num2 = 0;
					while ((long)num2 < num)
					{
						if (reader.PeekEntry(out text) == EntryType.EndOfArray)
						{
							reader.Context.Config.DebugContext.LogError(string.Concat(new string[]
							{
								"Reached end of array after ",
								num2.ToString(),
								" elements, when ",
								num.ToString(),
								" elements were expected."
							}));
							break;
						}
						bool flag = true;
						try
						{
							Type type;
							reader.EnterNode(out type);
							object obj2 = this.KeyReaderWriter.ReadValueWeak(reader);
							object obj3 = this.ValueReaderWriter.ReadValueWeak(reader);
							if (!this.KeyIsValueType && obj2 == null)
							{
								reader.Context.Config.DebugContext.LogWarning("Dictionary key of type '" + this.KeyType.FullName + "' was null upon deserialization. A key has gone missing.");
								goto IL_1C1;
							}
							dictionary[obj2] = obj3;
						}
						catch (SerializationAbortException ex)
						{
							flag = false;
							throw ex;
						}
						catch (Exception exception)
						{
							reader.Context.Config.DebugContext.LogException(exception);
						}
						finally
						{
							if (flag)
							{
								reader.ExitNode();
							}
						}
						goto IL_192;
						IL_1C1:
						num2++;
						continue;
						IL_192:
						if (!reader.IsInArrayNode)
						{
							reader.Context.Config.DebugContext.LogError("Reading array went wrong. Data dump: " + reader.GetDataDump());
							break;
						}
						goto IL_1C1;
					}
					return;
				}
				finally
				{
					reader.ExitArray();
				}
			}
			reader.SkipEntry();
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000113E8 File Offset: 0x0000F5E8
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			try
			{
				IDictionary dictionary = (IDictionary)value;
				if (this.ComparerProperty != null)
				{
					object value2 = this.ComparerProperty.GetValue(value, null);
					if (value2 != null)
					{
						this.EqualityComparerSerializer.WriteValueWeak("comparer", value2, writer);
					}
				}
				writer.BeginArrayNode((long)((int)this.CountProperty.GetValue(value, null)));
				IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						bool flag = true;
						try
						{
							writer.BeginStructNode(null, null);
							this.KeyReaderWriter.WriteValueWeak("$k", enumerator.Key, writer);
							this.ValueReaderWriter.WriteValueWeak("$v", enumerator.Value, writer);
						}
						catch (SerializationAbortException ex)
						{
							flag = false;
							throw ex;
						}
						catch (Exception exception)
						{
							writer.Context.Config.DebugContext.LogException(exception);
						}
						finally
						{
							if (flag)
							{
								writer.EndNode(null);
							}
						}
					}
				}
				finally
				{
					enumerator.Reset();
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x040000B0 RID: 176
		private readonly bool KeyIsValueType;

		// Token: 0x040000B1 RID: 177
		private readonly Serializer EqualityComparerSerializer;

		// Token: 0x040000B2 RID: 178
		private readonly Serializer KeyReaderWriter;

		// Token: 0x040000B3 RID: 179
		private readonly Serializer ValueReaderWriter;

		// Token: 0x040000B4 RID: 180
		private readonly ConstructorInfo ComparerConstructor;

		// Token: 0x040000B5 RID: 181
		private readonly PropertyInfo ComparerProperty;

		// Token: 0x040000B6 RID: 182
		private readonly PropertyInfo CountProperty;

		// Token: 0x040000B7 RID: 183
		private readonly Type KeyType;

		// Token: 0x040000B8 RID: 184
		private readonly Type ValueType;
	}
}
