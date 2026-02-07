using System;
using System.Collections;
using System.Collections.Generic;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200001C RID: 28
	[Preserve]
	public class ES3ArrayType : ES3CollectionType
	{
		// Token: 0x060001EC RID: 492 RVA: 0x000073FD File Offset: 0x000055FD
		public ES3ArrayType(Type type) : base(type)
		{
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007406 File Offset: 0x00005606
		public ES3ArrayType(Type type, ES3Type elementType) : base(type, elementType)
		{
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007410 File Offset: 0x00005610
		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			Array array = (Array)obj;
			if (this.elementType == null)
			{
				throw new ArgumentNullException("ES3Type argument cannot be null.");
			}
			for (int i = 0; i < array.Length; i++)
			{
				writer.StartWriteCollectionItem(i);
				writer.Write(array.GetValue(i), this.elementType, memberReferenceMode);
				writer.EndWriteCollectionItem(i);
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000746C File Offset: 0x0000566C
		public override object Read(ES3Reader reader)
		{
			List<object> list = new List<object>();
			if (!this.ReadICollection<object>(reader, list, this.elementType))
			{
				return null;
			}
			Array array = ES3Reflection.ArrayCreateInstance(this.elementType.type, list.Count);
			int num = 0;
			foreach (object value in list)
			{
				array.SetValue(value, num);
				num++;
			}
			return array;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000074F4 File Offset: 0x000056F4
		public override object Read<T>(ES3Reader reader)
		{
			return this.Read(reader);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000074FD File Offset: 0x000056FD
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			this.ReadICollectionInto(reader, (ICollection)obj, this.elementType);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007514 File Offset: 0x00005714
		public override void ReadInto(ES3Reader reader, object obj)
		{
			IList list = (IList)obj;
			if (list.Count == 0)
			{
				ES3Debug.LogWarning("LoadInto/ReadInto expects a collection containing instances to load data in to, but the collection is empty.", null, 0);
			}
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
