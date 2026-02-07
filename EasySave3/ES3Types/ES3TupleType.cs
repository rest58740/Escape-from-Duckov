using System;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000025 RID: 37
	[Preserve]
	public class ES3TupleType : ES3Type
	{
		// Token: 0x0600022D RID: 557 RVA: 0x00008658 File Offset: 0x00006858
		public ES3TupleType(Type type) : base(type)
		{
			this.types = ES3Reflection.GetElementTypes(type);
			this.es3Types = new ES3Type[this.types.Length];
			for (int i = 0; i < this.types.Length; i++)
			{
				this.es3Types[i] = ES3TypeMgr.GetOrCreateES3Type(this.types[i], false);
				if (this.es3Types[i] == null)
				{
					this.isUnsupported = true;
				}
			}
			this.isTuple = true;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000086CC File Offset: 0x000068CC
		public override void Write(object obj, ES3Writer writer)
		{
			this.Write(obj, writer, writer.settings.memberReferenceMode);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000086E4 File Offset: 0x000068E4
		public void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			if (obj == null)
			{
				writer.WriteNull();
				return;
			}
			writer.StartWriteCollection();
			for (int i = 0; i < this.es3Types.Length; i++)
			{
				object value = ES3Reflection.GetProperty(this.type, "Item" + (i + 1).ToString()).GetValue(obj);
				writer.StartWriteCollectionItem(i);
				writer.Write(value, this.es3Types[i], memberReferenceMode);
				writer.EndWriteCollectionItem(i);
			}
			writer.EndWriteCollection();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00008760 File Offset: 0x00006960
		public override object Read<T>(ES3Reader reader)
		{
			object[] array = new object[this.types.Length];
			if (reader.StartReadCollection())
			{
				return null;
			}
			for (int i = 0; i < this.types.Length; i++)
			{
				reader.StartReadCollectionItem();
				array[i] = reader.Read<object>(this.es3Types[i]);
				reader.EndReadCollectionItem();
			}
			reader.EndReadCollection();
			return this.type.GetConstructor(this.types).Invoke(array);
		}

		// Token: 0x0400005D RID: 93
		public ES3Type[] es3Types;

		// Token: 0x0400005E RID: 94
		public Type[] types;

		// Token: 0x0400005F RID: 95
		protected ES3Reflection.ES3ReflectedMethod readMethod;

		// Token: 0x04000060 RID: 96
		protected ES3Reflection.ES3ReflectedMethod readIntoMethod;
	}
}
