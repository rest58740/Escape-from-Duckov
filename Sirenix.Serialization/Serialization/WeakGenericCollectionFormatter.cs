using System;
using System.Collections;
using System.Reflection;

namespace Sirenix.Serialization
{
	// Token: 0x02000035 RID: 53
	public sealed class WeakGenericCollectionFormatter : WeakBaseFormatter
	{
		// Token: 0x06000282 RID: 642 RVA: 0x00012E04 File Offset: 0x00011004
		public WeakGenericCollectionFormatter(Type collectionType, Type elementType) : base(collectionType)
		{
			this.ElementType = elementType;
			this.CountProperty = collectionType.GetProperty("Count", 52);
			this.AddMethod = collectionType.GetMethod("Add", 52, null, new Type[]
			{
				elementType
			}, null);
			if (this.AddMethod == null)
			{
				throw new ArgumentException("Cannot treat the type " + collectionType.Name + " as a generic collection since it has no accessible Add method.");
			}
			if (this.CountProperty == null || this.CountProperty.PropertyType != typeof(int))
			{
				throw new ArgumentException("Cannot treat the type " + collectionType.Name + " as a generic collection since it has no accessible Count property.");
			}
			Type type;
			if (!GenericCollectionFormatter.CanFormat(collectionType, out type))
			{
				throw new ArgumentException("Cannot treat the type " + collectionType.Name + " as a generic collection.");
			}
			if (type != elementType)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Type ",
					elementType.Name,
					" is not the element type of the generic collection type ",
					collectionType.Name,
					"."
				}));
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00012F28 File Offset: 0x00011128
		protected override object GetUninitializedObject()
		{
			return Activator.CreateInstance(this.SerializedType);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00012F38 File Offset: 0x00011138
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
						object[] array = new object[1];
						try
						{
							array[0] = this.ValueReaderWriter.ReadValueWeak(reader);
							this.AddMethod.Invoke(value, array);
						}
						catch (Exception exception)
						{
							reader.Context.Config.DebugContext.LogException(exception);
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
				catch (Exception exception2)
				{
					reader.Context.Config.DebugContext.LogException(exception2);
					return;
				}
				finally
				{
					reader.ExitArray();
				}
			}
			reader.SkipEntry();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000130BC File Offset: 0x000112BC
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			try
			{
				writer.BeginArrayNode((long)((int)this.CountProperty.GetValue(value, null)));
				foreach (object value2 in ((IEnumerable)value))
				{
					this.ValueReaderWriter.WriteValueWeak(value2, writer);
				}
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x040000C5 RID: 197
		private readonly Serializer ValueReaderWriter;

		// Token: 0x040000C6 RID: 198
		private readonly Type ElementType;

		// Token: 0x040000C7 RID: 199
		private readonly PropertyInfo CountProperty;

		// Token: 0x040000C8 RID: 200
		private readonly MethodInfo AddMethod;
	}
}
