using System;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000029 RID: 41
	[Preserve]
	public abstract class ES3ObjectType : ES3Type
	{
		// Token: 0x0600023F RID: 575 RVA: 0x00008B91 File Offset: 0x00006D91
		public ES3ObjectType(Type type) : base(type)
		{
		}

		// Token: 0x06000240 RID: 576
		protected abstract void WriteObject(object obj, ES3Writer writer);

		// Token: 0x06000241 RID: 577
		protected abstract object ReadObject<T>(ES3Reader reader);

		// Token: 0x06000242 RID: 578 RVA: 0x00008B9A File Offset: 0x00006D9A
		protected virtual void ReadObject<T>(ES3Reader reader, object obj)
		{
			string str = "ReadInto is not supported for type ";
			Type type = this.type;
			throw new NotSupportedException(str + ((type != null) ? type.ToString() : null));
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00008BC0 File Offset: 0x00006DC0
		public override void Write(object obj, ES3Writer writer)
		{
			if (!base.WriteUsingDerivedType(obj, writer))
			{
				Type type = ES3Reflection.BaseType(obj.GetType());
				if (type != typeof(object))
				{
					ES3Type orCreateES3Type = ES3TypeMgr.GetOrCreateES3Type(type, false);
					if (orCreateES3Type != null && (orCreateES3Type.isDictionary || orCreateES3Type.isCollection))
					{
						writer.WriteProperty("_Values", obj, orCreateES3Type);
					}
				}
				this.WriteObject(obj, writer);
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00008C28 File Offset: 0x00006E28
		public override object Read<T>(ES3Reader reader)
		{
			string text = base.ReadPropertyName(reader);
			if (text == "__type")
			{
				return ES3TypeMgr.GetOrCreateES3Type(reader.ReadType(), true).Read<T>(reader);
			}
			reader.overridePropertiesName = text;
			return this.ReadObject<T>(reader);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00008C6C File Offset: 0x00006E6C
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			for (;;)
			{
				string text = base.ReadPropertyName(reader);
				if (text == "__type")
				{
					break;
				}
				if (text == null)
				{
					return;
				}
				reader.overridePropertiesName = text;
				this.ReadObject<T>(reader, obj);
			}
			ES3TypeMgr.GetOrCreateES3Type(reader.ReadType(), true).ReadInto<T>(reader, obj);
		}
	}
}
