using System;
using System.Collections;
using System.Collections.Generic;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200001E RID: 30
	[Preserve]
	public class ES3ConcurrentDictionaryType : ES3Type
	{
		// Token: 0x060001FC RID: 508 RVA: 0x00007720 File Offset: 0x00005920
		public ES3ConcurrentDictionaryType(Type type) : base(type)
		{
			Type[] elementTypes = ES3Reflection.GetElementTypes(type);
			this.keyType = ES3TypeMgr.GetOrCreateES3Type(elementTypes[0], false);
			this.valueType = ES3TypeMgr.GetOrCreateES3Type(elementTypes[1], false);
			if (this.keyType == null || this.valueType == null)
			{
				this.isUnsupported = true;
			}
			this.isDictionary = true;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00007777 File Offset: 0x00005977
		public ES3ConcurrentDictionaryType(Type type, ES3Type keyType, ES3Type valueType) : base(type)
		{
			this.keyType = keyType;
			this.valueType = valueType;
			if (keyType == null || valueType == null)
			{
				this.isUnsupported = true;
			}
			this.isDictionary = true;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000077A2 File Offset: 0x000059A2
		public override void Write(object obj, ES3Writer writer)
		{
			this.Write(obj, writer, writer.settings.memberReferenceMode);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000077B8 File Offset: 0x000059B8
		public void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode)
		{
			IDictionary dictionary = (IDictionary)obj;
			int num = 0;
			foreach (object obj2 in dictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				writer.StartWriteDictionaryKey(num);
				writer.Write(dictionaryEntry.Key, this.keyType, memberReferenceMode);
				writer.EndWriteDictionaryKey(num);
				writer.StartWriteDictionaryValue(num);
				writer.Write(dictionaryEntry.Value, this.valueType, memberReferenceMode);
				writer.EndWriteDictionaryValue(num);
				num++;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00007854 File Offset: 0x00005A54
		public override object Read<T>(ES3Reader reader)
		{
			return this.Read(reader);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000785D File Offset: 0x00005A5D
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			this.ReadInto(reader, obj);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00007868 File Offset: 0x00005A68
		public object Read(ES3Reader reader)
		{
			if (reader.StartReadDictionary())
			{
				return null;
			}
			IDictionary dictionary = (IDictionary)ES3Reflection.CreateInstance(this.type);
			while (reader.StartReadDictionaryKey())
			{
				object key = reader.Read<object>(this.keyType);
				reader.EndReadDictionaryKey();
				reader.StartReadDictionaryValue();
				object value = reader.Read<object>(this.valueType);
				dictionary.Add(key, value);
				if (reader.EndReadDictionaryValue())
				{
					reader.EndReadDictionary();
					return dictionary;
				}
			}
			return dictionary;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000078D8 File Offset: 0x00005AD8
		public void ReadInto(ES3Reader reader, object obj)
		{
			if (reader.StartReadDictionary())
			{
				throw new NullReferenceException("The Dictionary we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			}
			IDictionary dictionary = (IDictionary)obj;
			while (reader.StartReadDictionaryKey())
			{
				object obj2 = reader.Read<object>(this.keyType);
				if (!dictionary.Contains(obj2))
				{
					throw new KeyNotFoundException("The key \"" + ((obj2 != null) ? obj2.ToString() : null) + "\" in the Dictionary we are loading does not exist in the Dictionary we are loading into");
				}
				object obj3 = dictionary[obj2];
				reader.EndReadDictionaryKey();
				reader.StartReadDictionaryValue();
				reader.ReadInto<object>(obj3, this.valueType);
				if (reader.EndReadDictionaryValue())
				{
					reader.EndReadDictionary();
					return;
				}
			}
		}

		// Token: 0x04000055 RID: 85
		public ES3Type keyType;

		// Token: 0x04000056 RID: 86
		public ES3Type valueType;

		// Token: 0x04000057 RID: 87
		protected ES3Reflection.ES3ReflectedMethod readMethod;

		// Token: 0x04000058 RID: 88
		protected ES3Reflection.ES3ReflectedMethod readIntoMethod;
	}
}
