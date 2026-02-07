using System;
using System.Collections;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000021 RID: 33
	[Preserve]
	public class ES3ListType : ES3CollectionType
	{
		// Token: 0x06000212 RID: 530 RVA: 0x00007D62 File Offset: 0x00005F62
		public ES3ListType(Type type) : base(type)
		{
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00007D6B File Offset: 0x00005F6B
		public ES3ListType(Type type, ES3Type elementType) : base(type, elementType)
		{
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00007D78 File Offset: 0x00005F78
		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			if (obj == null)
			{
				writer.WriteNull();
				return;
			}
			IEnumerable enumerable = (IList)obj;
			if (this.elementType == null)
			{
				throw new ArgumentNullException("ES3Type argument cannot be null.");
			}
			int num = 0;
			foreach (object value in enumerable)
			{
				writer.StartWriteCollectionItem(num);
				writer.Write(value, this.elementType, memberReferenceMode);
				writer.EndWriteCollectionItem(num);
				num++;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00007E04 File Offset: 0x00006004
		public override object Read<T>(ES3Reader reader)
		{
			return this.Read(reader);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00007E0D File Offset: 0x0000600D
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			this.ReadICollectionInto(reader, (ICollection)obj, this.elementType);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00007E24 File Offset: 0x00006024
		public override object Read(ES3Reader reader)
		{
			IList list = (IList)ES3Reflection.CreateInstance(this.type);
			if (reader.StartReadCollection())
			{
				return null;
			}
			while (reader.StartReadCollectionItem())
			{
				list.Add(reader.Read<object>(this.elementType));
				if (reader.EndReadCollectionItem())
				{
					break;
				}
			}
			reader.EndReadCollection();
			return list;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00007E78 File Offset: 0x00006078
		public override void ReadInto(ES3Reader reader, object obj)
		{
			IList list = (IList)obj;
			if (reader.StartReadCollection())
			{
				throw new NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			}
			int num = 0;
			foreach (object obj2 in list)
			{
				num++;
				if (!reader.StartReadCollectionItem())
				{
					break;
				}
				reader.ReadInto<object>(obj2, this.elementType);
				if (reader.EndReadCollectionItem())
				{
					break;
				}
				if (num == list.Count)
				{
					throw new IndexOutOfRangeException("The collection we are loading is longer than the collection provided as a parameter.");
				}
			}
			if (num != list.Count)
			{
				throw new IndexOutOfRangeException("The collection we are loading is shorter than the collection provided as a parameter.");
			}
			reader.EndReadCollection();
		}
	}
}
