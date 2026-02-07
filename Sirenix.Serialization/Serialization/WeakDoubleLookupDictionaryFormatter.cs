using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200002E RID: 46
	internal sealed class WeakDoubleLookupDictionaryFormatter : WeakBaseFormatter
	{
		// Token: 0x06000267 RID: 615 RVA: 0x000117EC File Offset: 0x0000F9EC
		public WeakDoubleLookupDictionaryFormatter(Type serializedType) : base(serializedType)
		{
			Type[] argumentsOfInheritedOpenGenericClass = serializedType.GetArgumentsOfInheritedOpenGenericClass(typeof(Dictionary));
			this.PrimaryReaderWriter = Serializer.Get(argumentsOfInheritedOpenGenericClass[0]);
			this.InnerReaderWriter = Serializer.Get(argumentsOfInheritedOpenGenericClass[1]);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00011830 File Offset: 0x0000FA30
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			try
			{
				IDictionary dictionary = (IDictionary)value;
				writer.BeginArrayNode((long)dictionary.Count);
				bool flag = true;
				IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						try
						{
							writer.BeginStructNode(null, null);
							this.PrimaryReaderWriter.WriteValueWeak("$k", enumerator.Key, writer);
							this.InnerReaderWriter.WriteValueWeak("$v", enumerator.Value, writer);
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

		// Token: 0x0600026A RID: 618 RVA: 0x0001192C File Offset: 0x0000FB2C
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				try
				{
					long num;
					reader.EnterArray(out num);
					value = Activator.CreateInstance(this.SerializedType);
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
							object obj = this.PrimaryReaderWriter.ReadValueWeak(reader);
							object obj2 = this.InnerReaderWriter.ReadValueWeak(reader);
							dictionary.Add(obj, obj2);
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
						if (!reader.IsInArrayNode)
						{
							reader.Context.Config.DebugContext.LogError("Reading array went wrong. Data dump: " + reader.GetDataDump());
							break;
						}
						num2++;
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

		// Token: 0x040000BB RID: 187
		private readonly Serializer PrimaryReaderWriter;

		// Token: 0x040000BC RID: 188
		private readonly Serializer InnerReaderWriter;
	}
}
