using System;
using System.Collections.Generic;
using ES3Internal;

namespace ES3Types
{
	// Token: 0x0200001B RID: 27
	public class ES33DArrayType : ES3CollectionType
	{
		// Token: 0x060001E6 RID: 486 RVA: 0x00007109 File Offset: 0x00005309
		public ES33DArrayType(Type type) : base(type)
		{
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007114 File Offset: 0x00005314
		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			Array array = (Array)obj;
			if (this.elementType == null)
			{
				throw new ArgumentNullException("ES3Type argument cannot be null.");
			}
			for (int i = 0; i < array.GetLength(0); i++)
			{
				writer.StartWriteCollectionItem(i);
				writer.StartWriteCollection();
				for (int j = 0; j < array.GetLength(1); j++)
				{
					writer.StartWriteCollectionItem(j);
					writer.StartWriteCollection();
					for (int k = 0; k < array.GetLength(2); k++)
					{
						writer.StartWriteCollectionItem(k);
						writer.Write(array.GetValue(i, j, k), this.elementType, memberReferenceMode);
						writer.EndWriteCollectionItem(k);
					}
					writer.EndWriteCollection();
					writer.EndWriteCollectionItem(j);
				}
				writer.EndWriteCollection();
				writer.EndWriteCollectionItem(i);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000071CF File Offset: 0x000053CF
		public override object Read<T>(ES3Reader reader)
		{
			return this.Read(reader);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000071D8 File Offset: 0x000053D8
		public override object Read(ES3Reader reader)
		{
			if (reader.StartReadCollection())
			{
				return null;
			}
			List<object> list = new List<object>();
			int num = 0;
			int num2 = 0;
			while (reader.StartReadCollectionItem())
			{
				reader.StartReadCollection();
				num++;
				while (reader.StartReadCollectionItem())
				{
					this.ReadICollection<object>(reader, list, this.elementType);
					num2++;
					if (reader.EndReadCollectionItem())
					{
						break;
					}
				}
				reader.EndReadCollection();
				if (reader.EndReadCollectionItem())
				{
					break;
				}
			}
			reader.EndReadCollection();
			num2 /= num;
			int num3 = list.Count / num2 / num;
			Array array = ES3Reflection.ArrayCreateInstance(this.elementType.type, new int[]
			{
				num,
				num2,
				num3
			});
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					for (int k = 0; k < num3; k++)
					{
						array.SetValue(list[i * (num2 * num3) + j * num3 + k], i, j, k);
					}
				}
			}
			return array;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000072C9 File Offset: 0x000054C9
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			this.ReadInto(reader, obj);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000072D4 File Offset: 0x000054D4
		public override void ReadInto(ES3Reader reader, object obj)
		{
			Array array = (Array)obj;
			if (reader.StartReadCollection())
			{
				throw new NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			}
			bool flag = false;
			for (int i = 0; i < array.GetLength(0); i++)
			{
				bool flag2 = false;
				if (!reader.StartReadCollectionItem())
				{
					throw new IndexOutOfRangeException("The collection we are loading is smaller than the collection provided as a parameter.");
				}
				reader.StartReadCollection();
				for (int j = 0; j < array.GetLength(1); j++)
				{
					bool flag3 = false;
					if (!reader.StartReadCollectionItem())
					{
						throw new IndexOutOfRangeException("The collection we are loading is smaller than the collection provided as a parameter.");
					}
					reader.StartReadCollection();
					for (int k = 0; k < array.GetLength(2); k++)
					{
						if (!reader.StartReadCollectionItem())
						{
							throw new IndexOutOfRangeException("The collection we are loading is smaller than the collection provided as a parameter.");
						}
						reader.ReadInto<object>(array.GetValue(i, j, k), this.elementType);
						flag3 = reader.EndReadCollectionItem();
					}
					if (!flag3)
					{
						throw new IndexOutOfRangeException("The collection we are loading is larger than the collection provided as a parameter.");
					}
					reader.EndReadCollection();
					flag2 = reader.EndReadCollectionItem();
				}
				if (!flag2)
				{
					throw new IndexOutOfRangeException("The collection we are loading is larger than the collection provided as a parameter.");
				}
				reader.EndReadCollection();
				flag = reader.EndReadCollectionItem();
			}
			if (!flag)
			{
				throw new IndexOutOfRangeException("The collection we are loading is larger than the collection provided as a parameter.");
			}
			reader.EndReadCollection();
		}
	}
}
