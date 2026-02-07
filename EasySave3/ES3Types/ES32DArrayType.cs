using System;
using System.Collections.Generic;
using ES3Internal;

namespace ES3Types
{
	// Token: 0x0200001A RID: 26
	public class ES32DArrayType : ES3CollectionType
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x00006EEC File Offset: 0x000050EC
		public ES32DArrayType(Type type) : base(type)
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006EF8 File Offset: 0x000050F8
		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode unityObjectType)
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
					writer.Write(array.GetValue(i, j), this.elementType, unityObjectType);
					writer.EndWriteCollectionItem(j);
				}
				writer.EndWriteCollection();
				writer.EndWriteCollectionItem(i);
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00006F80 File Offset: 0x00005180
		public override object Read<T>(ES3Reader reader)
		{
			return this.Read(reader);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00006F8C File Offset: 0x0000518C
		public override object Read(ES3Reader reader)
		{
			if (reader.StartReadCollection())
			{
				return null;
			}
			List<object> list = new List<object>();
			int num = 0;
			while (reader.StartReadCollectionItem())
			{
				this.ReadICollection<object>(reader, list, this.elementType);
				num++;
				if (reader.EndReadCollectionItem())
				{
					break;
				}
			}
			int num2 = list.Count / num;
			Array array = ES3Reflection.ArrayCreateInstance(this.elementType.type, new int[]
			{
				num,
				num2
			});
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array.SetValue(list[i * num2 + j], i, j);
				}
			}
			return array;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000702E File Offset: 0x0000522E
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			this.ReadInto(reader, obj);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00007038 File Offset: 0x00005238
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
					if (!reader.StartReadCollectionItem())
					{
						throw new IndexOutOfRangeException("The collection we are loading is smaller than the collection provided as a parameter.");
					}
					reader.ReadInto<object>(array.GetValue(i, j), this.elementType);
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
