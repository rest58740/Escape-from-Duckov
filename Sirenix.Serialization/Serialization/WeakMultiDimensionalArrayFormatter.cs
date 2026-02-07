using System;
using System.Globalization;
using System.Text;

namespace Sirenix.Serialization
{
	// Token: 0x02000043 RID: 67
	public sealed class WeakMultiDimensionalArrayFormatter : WeakBaseFormatter
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x00014804 File Offset: 0x00012A04
		public WeakMultiDimensionalArrayFormatter(Type arrayType, Type elementType) : base(arrayType)
		{
			this.ArrayRank = arrayType.GetArrayRank();
			this.ElementType = elementType;
			this.ValueReaderWriter = Serializer.Get(elementType);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0001482C File Offset: 0x00012A2C
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			string name;
			EntryType entryType = reader.PeekEntry(out name);
			if (entryType != EntryType.StartOfArray)
			{
				value = null;
				reader.SkipEntry();
				return;
			}
			long length;
			reader.EnterArray(out length);
			entryType = reader.PeekEntry(out name);
			if (entryType != EntryType.String || name != "ranks")
			{
				value = null;
				reader.SkipEntry();
				return;
			}
			string text;
			reader.ReadString(out text);
			string[] array = text.Split(new char[]
			{
				'|'
			});
			if (array.Length != this.ArrayRank)
			{
				value = null;
				reader.SkipEntry();
				return;
			}
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				int num;
				if (!int.TryParse(array[i], ref num))
				{
					value = null;
					reader.SkipEntry();
					return;
				}
				array2[i] = num;
			}
			long num2 = (long)array2[0];
			for (int j = 1; j < array2.Length; j++)
			{
				num2 *= (long)array2[j];
			}
			if (num2 != length)
			{
				value = null;
				reader.SkipEntry();
				return;
			}
			value = Array.CreateInstance(this.ElementType, array2);
			base.RegisterReferenceID(value, reader);
			int elements = 0;
			try
			{
				this.IterateArrayWrite((Array)value, delegate()
				{
					int elements;
					if (reader.PeekEntry(out name) == EntryType.EndOfArray)
					{
						reader.Context.Config.DebugContext.LogError(string.Concat(new string[]
						{
							"Reached end of array after ",
							elements.ToString(),
							" elements, when ",
							length.ToString(),
							" elements were expected."
						}));
						throw new InvalidOperationException();
					}
					object result = this.ValueReaderWriter.ReadValueWeak(reader);
					if (!reader.IsInArrayNode)
					{
						reader.Context.Config.DebugContext.LogError("Reading array went wrong. Data dump: " + reader.GetDataDump());
						throw new InvalidOperationException();
					}
					elements = elements;
					elements++;
					return result;
				});
			}
			catch (InvalidOperationException)
			{
			}
			catch (Exception exception)
			{
				reader.Context.Config.DebugContext.LogException(exception);
			}
			reader.ExitArray();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00014A04 File Offset: 0x00012C04
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			Array array = value as Array;
			try
			{
				writer.BeginArrayNode(array.LongLength);
				int[] array2 = new int[this.ArrayRank];
				for (int i = 0; i < this.ArrayRank; i++)
				{
					array2[i] = array.GetLength(i);
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.ArrayRank; j++)
				{
					if (j > 0)
					{
						stringBuilder.Append('|');
					}
					stringBuilder.Append(array2[j].ToString(CultureInfo.InvariantCulture));
				}
				string value2 = stringBuilder.ToString();
				writer.WriteString("ranks", value2);
				this.IterateArrayRead((Array)value, delegate(object v)
				{
					this.ValueReaderWriter.WriteValueWeak(v, writer);
				});
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00014B00 File Offset: 0x00012D00
		private void IterateArrayWrite(Array a, Func<object> write)
		{
			int[] indices = new int[this.ArrayRank];
			this.IterateArrayWrite(a, 0, indices, write);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00014B24 File Offset: 0x00012D24
		private void IterateArrayWrite(Array a, int rank, int[] indices, Func<object> write)
		{
			for (int i = 0; i < a.GetLength(rank); i++)
			{
				indices[rank] = i;
				if (rank + 1 < a.Rank)
				{
					this.IterateArrayWrite(a, rank + 1, indices, write);
				}
				else
				{
					a.SetValue(write.Invoke(), indices);
				}
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00014B70 File Offset: 0x00012D70
		private void IterateArrayRead(Array a, Action<object> read)
		{
			int[] indices = new int[this.ArrayRank];
			this.IterateArrayRead(a, 0, indices, read);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00014B94 File Offset: 0x00012D94
		private void IterateArrayRead(Array a, int rank, int[] indices, Action<object> read)
		{
			for (int i = 0; i < a.GetLength(rank); i++)
			{
				indices[rank] = i;
				if (rank + 1 < a.Rank)
				{
					this.IterateArrayRead(a, rank + 1, indices, read);
				}
				else
				{
					read.Invoke(a.GetValue(indices));
				}
			}
		}

		// Token: 0x040000DF RID: 223
		private const string RANKS_NAME = "ranks";

		// Token: 0x040000E0 RID: 224
		private const char RANKS_SEPARATOR = '|';

		// Token: 0x040000E1 RID: 225
		private readonly int ArrayRank;

		// Token: 0x040000E2 RID: 226
		private readonly Type ElementType;

		// Token: 0x040000E3 RID: 227
		private readonly Serializer ValueReaderWriter;
	}
}
