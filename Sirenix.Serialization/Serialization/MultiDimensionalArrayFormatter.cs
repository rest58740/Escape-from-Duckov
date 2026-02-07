using System;
using System.Globalization;
using System.Text;

namespace Sirenix.Serialization
{
	// Token: 0x02000042 RID: 66
	public sealed class MultiDimensionalArrayFormatter<TArray, TElement> : BaseFormatter<TArray> where TArray : class
	{
		// Token: 0x060002BD RID: 701 RVA: 0x00014300 File Offset: 0x00012500
		static MultiDimensionalArrayFormatter()
		{
			if (!typeof(TArray).IsArray)
			{
				throw new ArgumentException("Type " + typeof(TArray).Name + " is not an array.");
			}
			if (typeof(TArray).GetElementType() != typeof(TElement))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Array of type ",
					typeof(TArray).Name,
					" does not have the required element type of ",
					typeof(TElement).Name,
					"."
				}));
			}
			MultiDimensionalArrayFormatter<TArray, TElement>.ArrayRank = typeof(TArray).GetArrayRank();
			if (MultiDimensionalArrayFormatter<TArray, TElement>.ArrayRank <= 1)
			{
				throw new ArgumentException("Array of type " + typeof(TArray).Name + " only has one rank.");
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000143FC File Offset: 0x000125FC
		protected override TArray GetUninitializedObject()
		{
			return default(TArray);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00014414 File Offset: 0x00012614
		protected override void DeserializeImplementation(ref TArray value, IDataReader reader)
		{
			string name;
			EntryType entryType = reader.PeekEntry(out name);
			if (entryType != EntryType.StartOfArray)
			{
				value = default(TArray);
				reader.SkipEntry();
				return;
			}
			long length;
			reader.EnterArray(out length);
			entryType = reader.PeekEntry(out name);
			if (entryType != EntryType.String || name != "ranks")
			{
				value = default(TArray);
				reader.SkipEntry();
				return;
			}
			string text;
			reader.ReadString(out text);
			string[] array = text.Split(new char[]
			{
				'|'
			});
			if (array.Length != MultiDimensionalArrayFormatter<TArray, TElement>.ArrayRank)
			{
				value = default(TArray);
				reader.SkipEntry();
				return;
			}
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				int num;
				if (!int.TryParse(array[i], ref num))
				{
					value = default(TArray);
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
				value = default(TArray);
				reader.SkipEntry();
				return;
			}
			value = (TArray)((object)Array.CreateInstance(typeof(TElement), array2));
			base.RegisterReferenceID(value, reader);
			int elements = 0;
			try
			{
				this.IterateArrayWrite((Array)((object)value), delegate()
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
					TElement result = MultiDimensionalArrayFormatter<TArray, TElement>.ValueReaderWriter.ReadValue(reader);
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

		// Token: 0x060002C0 RID: 704 RVA: 0x00014610 File Offset: 0x00012810
		protected override void SerializeImplementation(ref TArray value, IDataWriter writer)
		{
			Array array = value as Array;
			try
			{
				writer.BeginArrayNode(array.LongLength);
				int[] array2 = new int[MultiDimensionalArrayFormatter<TArray, TElement>.ArrayRank];
				for (int i = 0; i < MultiDimensionalArrayFormatter<TArray, TElement>.ArrayRank; i++)
				{
					array2[i] = array.GetLength(i);
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < MultiDimensionalArrayFormatter<TArray, TElement>.ArrayRank; j++)
				{
					if (j > 0)
					{
						stringBuilder.Append('|');
					}
					stringBuilder.Append(array2[j].ToString(CultureInfo.InvariantCulture));
				}
				string value2 = stringBuilder.ToString();
				writer.WriteString("ranks", value2);
				this.IterateArrayRead((Array)((object)value), delegate(TElement v)
				{
					MultiDimensionalArrayFormatter<TArray, TElement>.ValueReaderWriter.WriteValue(v, writer);
				});
			}
			finally
			{
				writer.EndArrayNode();
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00014714 File Offset: 0x00012914
		private void IterateArrayWrite(Array a, Func<TElement> write)
		{
			int[] indices = new int[MultiDimensionalArrayFormatter<TArray, TElement>.ArrayRank];
			this.IterateArrayWrite(a, 0, indices, write);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00014738 File Offset: 0x00012938
		private void IterateArrayWrite(Array a, int rank, int[] indices, Func<TElement> write)
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

		// Token: 0x060002C3 RID: 707 RVA: 0x00014788 File Offset: 0x00012988
		private void IterateArrayRead(Array a, Action<TElement> read)
		{
			int[] indices = new int[MultiDimensionalArrayFormatter<TArray, TElement>.ArrayRank];
			this.IterateArrayRead(a, 0, indices, read);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000147AC File Offset: 0x000129AC
		private void IterateArrayRead(Array a, int rank, int[] indices, Action<TElement> read)
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
					read.Invoke((TElement)((object)a.GetValue(indices)));
				}
			}
		}

		// Token: 0x040000DB RID: 219
		private const string RANKS_NAME = "ranks";

		// Token: 0x040000DC RID: 220
		private const char RANKS_SEPARATOR = '|';

		// Token: 0x040000DD RID: 221
		private static readonly int ArrayRank;

		// Token: 0x040000DE RID: 222
		private static readonly Serializer<TElement> ValueReaderWriter = Serializer.Get<TElement>();
	}
}
