using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000024 RID: 36
	[Preserve]
	public class ES3StackType : ES3CollectionType
	{
		// Token: 0x06000227 RID: 551 RVA: 0x00008388 File Offset: 0x00006588
		public ES3StackType(Type type) : base(type)
		{
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008394 File Offset: 0x00006594
		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			IEnumerable enumerable = (ICollection)obj;
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

		// Token: 0x06000229 RID: 553 RVA: 0x00008418 File Offset: 0x00006618
		public override object Read<T>(ES3Reader reader)
		{
			return this.Read(reader);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00008424 File Offset: 0x00006624
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			if (reader.StartReadCollection())
			{
				throw new NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			}
			int num = 0;
			Stack<T> stack = (Stack<T>)obj;
			foreach (T t in stack)
			{
				num++;
				if (!reader.StartReadCollectionItem())
				{
					break;
				}
				reader.ReadInto<T>(t, this.elementType);
				if (reader.EndReadCollectionItem())
				{
					break;
				}
				if (num == stack.Count)
				{
					throw new IndexOutOfRangeException("The collection we are loading is longer than the collection provided as a parameter.");
				}
			}
			if (num != stack.Count)
			{
				throw new IndexOutOfRangeException("The collection we are loading is shorter than the collection provided as a parameter.");
			}
			reader.EndReadCollection();
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000084E0 File Offset: 0x000066E0
		public override object Read(ES3Reader reader)
		{
			IList list = (IList)ES3Reflection.CreateInstance(ES3Reflection.MakeGenericType(typeof(List<>), this.elementType.type));
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
			ES3Reflection.GetMethods(list.GetType(), "Reverse").FirstOrDefault((MethodInfo t) => !t.IsStatic).Invoke(list, new object[0]);
			return ES3Reflection.CreateInstance(this.type, new object[]
			{
				list
			});
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000859C File Offset: 0x0000679C
		public override void ReadInto(ES3Reader reader, object obj)
		{
			if (reader.StartReadCollection())
			{
				throw new NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			}
			int num = 0;
			ICollection collection = (ICollection)obj;
			foreach (object obj2 in collection)
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
				if (num == collection.Count)
				{
					throw new IndexOutOfRangeException("The collection we are loading is longer than the collection provided as a parameter.");
				}
			}
			if (num != collection.Count)
			{
				throw new IndexOutOfRangeException("The collection we are loading is shorter than the collection provided as a parameter.");
			}
			reader.EndReadCollection();
		}
	}
}
