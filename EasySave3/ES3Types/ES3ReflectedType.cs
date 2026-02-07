using System;
using ES3Internal;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200005A RID: 90
	[Preserve]
	internal class ES3ReflectedType : ES3Type
	{
		// Token: 0x060002C4 RID: 708 RVA: 0x0000A3C2 File Offset: 0x000085C2
		public ES3ReflectedType(Type type) : base(type)
		{
			this.isReflectedType = true;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000A3D2 File Offset: 0x000085D2
		public ES3ReflectedType(Type type, string[] members) : this(type)
		{
			base.GetMembers(false, members);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000A3E4 File Offset: 0x000085E4
		public override void Write(object obj, ES3Writer writer)
		{
			if (obj == null)
			{
				writer.WriteNull();
				return;
			}
			UnityEngine.Object @object = obj as UnityEngine.Object;
			bool flag = @object != null;
			Type type = obj.GetType();
			if (type != this.type)
			{
				writer.WriteType(type);
				ES3TypeMgr.GetOrCreateES3Type(type, true).Write(obj, writer);
				return;
			}
			if (flag)
			{
				writer.WriteRef(@object);
			}
			if (this.members == null)
			{
				base.GetMembers(writer.settings.safeReflection);
			}
			for (int i = 0; i < this.members.Length; i++)
			{
				ES3Member es3Member = this.members[i];
				if (ES3Reflection.IsAssignableFrom(typeof(UnityEngine.Object), es3Member.type))
				{
					object value = es3Member.reflectedMember.GetValue(obj);
					UnityEngine.Object value2 = (value == null) ? null : ((UnityEngine.Object)value);
					writer.WritePropertyByRef(es3Member.name, value2);
				}
				else
				{
					writer.WriteProperty(es3Member.name, es3Member.reflectedMember.GetValue(obj), ES3TypeMgr.GetOrCreateES3Type(es3Member.type, true));
				}
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000A4E8 File Offset: 0x000086E8
		public override object Read<T>(ES3Reader reader)
		{
			if (this.members == null)
			{
				base.GetMembers(reader.settings.safeReflection);
			}
			string text = reader.ReadPropertyName();
			if (text == "__type")
			{
				return ES3TypeMgr.GetOrCreateES3Type(reader.ReadType(), true).Read<T>(reader);
			}
			object obj;
			if (text == "_ES3Ref")
			{
				long id = reader.Read_ref();
				obj = ES3ReferenceMgrBase.Current.Get(id, this.type, false);
				if (obj == null)
				{
					obj = ES3Reflection.CreateInstance(this.type);
					ES3ReferenceMgrBase.Current.Add((UnityEngine.Object)obj, id);
				}
			}
			else
			{
				reader.overridePropertiesName = text;
				obj = ES3Reflection.CreateInstance(this.type);
			}
			base.ReadProperties(reader, obj);
			return obj;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000A59C File Offset: 0x0000879C
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			if (this.members == null)
			{
				base.GetMembers(reader.settings.safeReflection);
			}
			string text = reader.ReadPropertyName();
			if (text == "__type")
			{
				ES3TypeMgr.GetOrCreateES3Type(reader.ReadType(), true).ReadInto<T>(reader, obj);
				return;
			}
			reader.overridePropertiesName = text;
			base.ReadProperties(reader, obj);
		}
	}
}
