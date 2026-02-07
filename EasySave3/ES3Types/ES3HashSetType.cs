using System;
using System.Collections;
using System.Collections.Generic;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000020 RID: 32
	[Preserve]
	public class ES3HashSetType : ES3CollectionType
	{
		// Token: 0x0600020C RID: 524 RVA: 0x00007BBE File Offset: 0x00005DBE
		public ES3HashSetType(Type type) : base(type)
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00007BC8 File Offset: 0x00005DC8
		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			if (obj == null)
			{
				writer.WriteNull();
				return;
			}
			IEnumerable enumerable = (IEnumerable)obj;
			if (this.elementType == null)
			{
				throw new ArgumentNullException("ES3Type argument cannot be null.");
			}
			int num = 0;
			foreach (object obj2 in enumerable)
			{
				num++;
			}
			int num2 = 0;
			foreach (object value in enumerable)
			{
				writer.StartWriteCollectionItem(num2);
				writer.Write(value, this.elementType, memberReferenceMode);
				writer.EndWriteCollectionItem(num2);
				num2++;
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00007C9C File Offset: 0x00005E9C
		public override object Read<T>(ES3Reader reader)
		{
			object obj = this.Read(reader);
			if (obj == null)
			{
				return default(T);
			}
			return (T)((object)obj);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00007CD0 File Offset: 0x00005ED0
		public override object Read(ES3Reader reader)
		{
			Type genericParam = ES3Reflection.GetGenericArguments(this.type)[0];
			IList list = (IList)ES3Reflection.CreateInstance(ES3Reflection.MakeGenericType(typeof(List<>), genericParam));
			if (!reader.StartReadCollection())
			{
				while (reader.StartReadCollectionItem())
				{
					list.Add(reader.Read<object>(this.elementType));
					if (reader.EndReadCollectionItem())
					{
						break;
					}
				}
				reader.EndReadCollection();
			}
			return ES3Reflection.CreateInstance(this.type, new object[]
			{
				list
			});
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00007D4C File Offset: 0x00005F4C
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			this.ReadInto(reader, obj);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00007D56 File Offset: 0x00005F56
		public override void ReadInto(ES3Reader reader, object obj)
		{
			throw new NotImplementedException("Cannot use LoadInto/ReadInto with HashSet because HashSets do not maintain the order of elements");
		}
	}
}
