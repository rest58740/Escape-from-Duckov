using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ES3Internal;
using Unity.Collections;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000022 RID: 34
	[Preserve]
	public class ES3NativeArrayType : ES3CollectionType
	{
		// Token: 0x06000219 RID: 537 RVA: 0x00007F34 File Offset: 0x00006134
		public ES3NativeArrayType(Type type) : base(type)
		{
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00007F3D File Offset: 0x0000613D
		public ES3NativeArrayType(Type type, ES3Type elementType) : base(type, elementType)
		{
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00007F48 File Offset: 0x00006148
		public override void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			if (this.elementType == null)
			{
				throw new ArgumentNullException("ES3Type argument cannot be null.");
			}
			IEnumerable enumerable = (IEnumerable)obj;
			int num = 0;
			foreach (object value in enumerable)
			{
				writer.StartWriteCollectionItem(num);
				writer.Write(value, this.elementType, memberReferenceMode);
				writer.EndWriteCollectionItem(num);
				num++;
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00007FCC File Offset: 0x000061CC
		public override object Read(ES3Reader reader)
		{
			Array array = this.ReadAsArray(reader);
			return ES3Reflection.CreateInstance(this.type, new object[]
			{
				array,
				Allocator.Persistent
			});
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00007FFF File Offset: 0x000061FF
		public override object Read<T>(ES3Reader reader)
		{
			return this.Read(reader);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00008008 File Offset: 0x00006208
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			this.ReadInto(reader, obj);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00008014 File Offset: 0x00006214
		public override void ReadInto(ES3Reader reader, object obj)
		{
			Array array = this.ReadAsArray(reader);
			ES3Reflection.GetMethods(this.type, "CopyFrom").First((MethodInfo m) => ES3Reflection.TypeIsArray(m.GetParameters()[0].GetType())).Invoke(obj, new object[]
			{
				array
			});
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00008070 File Offset: 0x00006270
		private Array ReadAsArray(ES3Reader reader)
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
	}
}
