using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ES3Internal;
using ES3Types;

// Token: 0x0200000D RID: 13
public abstract class ES3Reader : IDisposable
{
	// Token: 0x060000FB RID: 251
	internal abstract int Read_int();

	// Token: 0x060000FC RID: 252
	internal abstract float Read_float();

	// Token: 0x060000FD RID: 253
	internal abstract bool Read_bool();

	// Token: 0x060000FE RID: 254
	internal abstract char Read_char();

	// Token: 0x060000FF RID: 255
	internal abstract decimal Read_decimal();

	// Token: 0x06000100 RID: 256
	internal abstract double Read_double();

	// Token: 0x06000101 RID: 257
	internal abstract long Read_long();

	// Token: 0x06000102 RID: 258
	internal abstract ulong Read_ulong();

	// Token: 0x06000103 RID: 259
	internal abstract byte Read_byte();

	// Token: 0x06000104 RID: 260
	internal abstract sbyte Read_sbyte();

	// Token: 0x06000105 RID: 261
	internal abstract short Read_short();

	// Token: 0x06000106 RID: 262
	internal abstract ushort Read_ushort();

	// Token: 0x06000107 RID: 263
	internal abstract uint Read_uint();

	// Token: 0x06000108 RID: 264
	internal abstract string Read_string();

	// Token: 0x06000109 RID: 265
	internal abstract byte[] Read_byteArray();

	// Token: 0x0600010A RID: 266
	internal abstract long Read_ref();

	// Token: 0x0600010B RID: 267
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract string ReadPropertyName();

	// Token: 0x0600010C RID: 268
	protected abstract Type ReadKeyPrefix(bool ignore = false);

	// Token: 0x0600010D RID: 269
	protected abstract void ReadKeySuffix();

	// Token: 0x0600010E RID: 270
	internal abstract byte[] ReadElement(bool skip = false);

	// Token: 0x0600010F RID: 271
	public abstract void Dispose();

	// Token: 0x06000110 RID: 272 RVA: 0x00005020 File Offset: 0x00003220
	internal virtual bool Goto(string key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("Key cannot be NULL when loading data.");
		}
		string text;
		while ((text = this.ReadPropertyName()) != key)
		{
			if (text == null)
			{
				return false;
			}
			this.Skip();
		}
		return true;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00005057 File Offset: 0x00003257
	internal virtual bool StartReadObject()
	{
		this.serializationDepth++;
		return false;
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00005068 File Offset: 0x00003268
	internal virtual void EndReadObject()
	{
		this.serializationDepth--;
	}

	// Token: 0x06000113 RID: 275
	internal abstract bool StartReadDictionary();

	// Token: 0x06000114 RID: 276
	internal abstract void EndReadDictionary();

	// Token: 0x06000115 RID: 277
	internal abstract bool StartReadDictionaryKey();

	// Token: 0x06000116 RID: 278
	internal abstract void EndReadDictionaryKey();

	// Token: 0x06000117 RID: 279
	internal abstract void StartReadDictionaryValue();

	// Token: 0x06000118 RID: 280
	internal abstract bool EndReadDictionaryValue();

	// Token: 0x06000119 RID: 281
	internal abstract bool StartReadCollection();

	// Token: 0x0600011A RID: 282
	internal abstract void EndReadCollection();

	// Token: 0x0600011B RID: 283
	internal abstract bool StartReadCollectionItem();

	// Token: 0x0600011C RID: 284
	internal abstract bool EndReadCollectionItem();

	// Token: 0x0600011D RID: 285 RVA: 0x00005078 File Offset: 0x00003278
	internal ES3Reader(ES3Settings settings, bool readHeaderAndFooter = true)
	{
		this.settings = settings;
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600011E RID: 286 RVA: 0x00005087 File Offset: 0x00003287
	public virtual ES3Reader.ES3ReaderPropertyEnumerator Properties
	{
		get
		{
			return new ES3Reader.ES3ReaderPropertyEnumerator(this);
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600011F RID: 287 RVA: 0x0000508F File Offset: 0x0000328F
	internal virtual ES3Reader.ES3ReaderRawEnumerator RawEnumerator
	{
		get
		{
			return new ES3Reader.ES3ReaderRawEnumerator(this);
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00005097 File Offset: 0x00003297
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void Skip()
	{
		this.ReadElement(true);
	}

	// Token: 0x06000121 RID: 289 RVA: 0x000050A1 File Offset: 0x000032A1
	public virtual T Read<T>()
	{
		return this.Read<T>(ES3TypeMgr.GetOrCreateES3Type(typeof(T), true));
	}

	// Token: 0x06000122 RID: 290 RVA: 0x000050B9 File Offset: 0x000032B9
	public virtual void ReadInto<T>(object obj)
	{
		this.ReadInto<T>(obj, ES3TypeMgr.GetOrCreateES3Type(typeof(T), true));
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000050D2 File Offset: 0x000032D2
	[EditorBrowsable(EditorBrowsableState.Never)]
	public T ReadProperty<T>()
	{
		return this.ReadProperty<T>(ES3TypeMgr.GetOrCreateES3Type(typeof(T), true));
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000050EA File Offset: 0x000032EA
	[EditorBrowsable(EditorBrowsableState.Never)]
	public T ReadProperty<T>(ES3Type type)
	{
		this.ReadPropertyName();
		return this.Read<T>(type);
	}

	// Token: 0x06000125 RID: 293 RVA: 0x000050FA File Offset: 0x000032FA
	[EditorBrowsable(EditorBrowsableState.Never)]
	public long ReadRefProperty()
	{
		this.ReadPropertyName();
		return this.Read_ref();
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00005109 File Offset: 0x00003309
	internal Type ReadType()
	{
		return ES3Reflection.GetType(this.Read<string>(ES3Type_string.Instance));
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0000511C File Offset: 0x0000331C
	public object SetPrivateProperty(string name, object value, object objectContainingProperty)
	{
		ES3Reflection.ES3ReflectedMember es3ReflectedProperty = ES3Reflection.GetES3ReflectedProperty(objectContainingProperty.GetType(), name);
		if (es3ReflectedProperty.IsNull)
		{
			string str = "A private property named ";
			string str2 = " does not exist in the type ";
			Type type = objectContainingProperty.GetType();
			throw new MissingMemberException(str + name + str2 + ((type != null) ? type.ToString() : null));
		}
		es3ReflectedProperty.SetValue(objectContainingProperty, value);
		return objectContainingProperty;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00005174 File Offset: 0x00003374
	public object SetPrivateField(string name, object value, object objectContainingField)
	{
		ES3Reflection.ES3ReflectedMember es3ReflectedMember = ES3Reflection.GetES3ReflectedMember(objectContainingField.GetType(), name);
		if (es3ReflectedMember.IsNull)
		{
			string str = "A private field named ";
			string str2 = " does not exist in the type ";
			Type type = objectContainingField.GetType();
			throw new MissingMemberException(str + name + str2 + ((type != null) ? type.ToString() : null));
		}
		es3ReflectedMember.SetValue(objectContainingField, value);
		return objectContainingField;
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000051CC File Offset: 0x000033CC
	public virtual T Read<T>(string key)
	{
		if (!this.Goto(key))
		{
			throw new KeyNotFoundException(string.Concat(new string[]
			{
				"Key \"",
				key,
				"\" was not found in file \"",
				this.settings.FullPath,
				"\". Use Load<T>(key, defaultValue) if you want to return a default value if the key does not exist."
			}));
		}
		Type type = this.ReadTypeFromHeader<T>();
		return this.Read<T>(ES3TypeMgr.GetOrCreateES3Type(type, true));
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00005234 File Offset: 0x00003434
	public virtual T Read<T>(string key, T defaultValue)
	{
		if (!this.Goto(key))
		{
			return defaultValue;
		}
		Type type = this.ReadTypeFromHeader<T>();
		return this.Read<T>(ES3TypeMgr.GetOrCreateES3Type(type, true));
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00005260 File Offset: 0x00003460
	public virtual void ReadInto<T>(string key, T obj) where T : class
	{
		if (!this.Goto(key))
		{
			throw new KeyNotFoundException(string.Concat(new string[]
			{
				"Key \"",
				key,
				"\" was not found in file \"",
				this.settings.FullPath,
				"\""
			}));
		}
		Type type = this.ReadTypeFromHeader<T>();
		this.ReadInto<T>(obj, ES3TypeMgr.GetOrCreateES3Type(type, true));
	}

	// Token: 0x0600012C RID: 300 RVA: 0x000052CB File Offset: 0x000034CB
	protected virtual void ReadObject<T>(object obj, ES3Type type)
	{
		if (this.StartReadObject())
		{
			return;
		}
		type.ReadInto<T>(this, obj);
		this.EndReadObject();
	}

	// Token: 0x0600012D RID: 301 RVA: 0x000052E4 File Offset: 0x000034E4
	protected virtual T ReadObject<T>(ES3Type type)
	{
		if (this.StartReadObject())
		{
			return default(T);
		}
		object obj = type.Read<T>(this);
		this.EndReadObject();
		return (T)((object)obj);
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00005318 File Offset: 0x00003518
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual T Read<T>(ES3Type type)
	{
		if (type == null || type.isUnsupported)
		{
			throw new NotSupportedException("Type of " + ((type != null) ? type.ToString() : null) + " is not currently supported, and could not be loaded using reflection.");
		}
		if (type.isPrimitive)
		{
			return (T)((object)type.Read<T>(this));
		}
		if (type.isCollection)
		{
			return (T)((object)((ES3CollectionType)type).Read(this));
		}
		if (type.isDictionary)
		{
			return (T)((object)((ES3DictionaryType)type).Read(this));
		}
		return this.ReadObject<T>(type);
	}

	// Token: 0x0600012F RID: 303 RVA: 0x000053A4 File Offset: 0x000035A4
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void ReadInto<T>(object obj, ES3Type type)
	{
		if (type == null || type.isUnsupported)
		{
			string str = "Type of ";
			Type type2 = obj.GetType();
			throw new NotSupportedException(str + ((type2 != null) ? type2.ToString() : null) + " is not currently supported, and could not be loaded using reflection.");
		}
		if (type.isCollection)
		{
			((ES3CollectionType)type).ReadInto(this, obj);
			return;
		}
		if (type.isDictionary)
		{
			((ES3DictionaryType)type).ReadInto(this, obj);
			return;
		}
		this.ReadObject<T>(obj, type);
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00005418 File Offset: 0x00003618
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal Type ReadTypeFromHeader<T>()
	{
		if (typeof(T) == typeof(object))
		{
			return this.ReadKeyPrefix(false);
		}
		if (!this.settings.typeChecking)
		{
			this.ReadKeyPrefix(true);
			return typeof(T);
		}
		Type type = this.ReadKeyPrefix(false);
		if (type == null)
		{
			string str = "Trying to load data of type ";
			Type typeFromHandle = typeof(T);
			throw new TypeLoadException(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + ", but the type of data contained in file no longer exists. This may be because the type has been removed from your project or renamed.");
		}
		if (type != typeof(T))
		{
			string[] array = new string[5];
			array[0] = "Trying to load data of type ";
			int num = 1;
			Type typeFromHandle2 = typeof(T);
			array[num] = ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null);
			array[2] = ", but data contained in file is type of ";
			int num2 = 3;
			Type type2 = type;
			array[num2] = ((type2 != null) ? type2.ToString() : null);
			array[4] = ".";
			throw new InvalidOperationException(string.Concat(array));
		}
		return type;
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000550C File Offset: 0x0000370C
	public static ES3Reader Create()
	{
		return ES3Reader.Create(new ES3Settings(null, null));
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000551A File Offset: 0x0000371A
	public static ES3Reader Create(string filePath)
	{
		return ES3Reader.Create(new ES3Settings(filePath, null));
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00005528 File Offset: 0x00003728
	public static ES3Reader Create(string filePath, ES3Settings settings)
	{
		return ES3Reader.Create(new ES3Settings(filePath, settings));
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00005538 File Offset: 0x00003738
	public static ES3Reader Create(ES3Settings settings)
	{
		Stream stream = ES3Stream.CreateStream(settings, ES3FileMode.Read);
		if (stream == null)
		{
			return null;
		}
		if (settings.format == ES3.Format.JSON)
		{
			return new ES3JSONReader(stream, settings, true);
		}
		return null;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00005564 File Offset: 0x00003764
	public static ES3Reader Create(byte[] bytes)
	{
		return ES3Reader.Create(bytes, new ES3Settings(null, null));
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00005574 File Offset: 0x00003774
	public static ES3Reader Create(byte[] bytes, ES3Settings settings)
	{
		Stream stream = ES3Stream.CreateStream(new MemoryStream(bytes), settings, ES3FileMode.Read);
		if (stream == null)
		{
			return null;
		}
		if (settings.format == ES3.Format.JSON)
		{
			return new ES3JSONReader(stream, settings, true);
		}
		return null;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x000055A6 File Offset: 0x000037A6
	internal static ES3Reader Create(Stream stream, ES3Settings settings)
	{
		stream = ES3Stream.CreateStream(stream, settings, ES3FileMode.Read);
		if (settings.format == ES3.Format.JSON)
		{
			return new ES3JSONReader(stream, settings, true);
		}
		return null;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000055C4 File Offset: 0x000037C4
	internal static ES3Reader Create(Stream stream, ES3Settings settings, bool readHeaderAndFooter)
	{
		if (settings.format == ES3.Format.JSON)
		{
			return new ES3JSONReader(stream, settings, readHeaderAndFooter);
		}
		return null;
	}

	// Token: 0x04000021 RID: 33
	public ES3Settings settings;

	// Token: 0x04000022 RID: 34
	protected int serializationDepth;

	// Token: 0x04000023 RID: 35
	internal string overridePropertiesName;

	// Token: 0x020000F6 RID: 246
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ES3ReaderPropertyEnumerator
	{
		// Token: 0x06000543 RID: 1347 RVA: 0x0001EDAB File Offset: 0x0001CFAB
		public ES3ReaderPropertyEnumerator(ES3Reader reader)
		{
			this.reader = reader;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001EDBA File Offset: 0x0001CFBA
		public IEnumerator GetEnumerator()
		{
			for (;;)
			{
				if (this.reader.overridePropertiesName != null)
				{
					string overridePropertiesName = this.reader.overridePropertiesName;
					this.reader.overridePropertiesName = null;
					yield return overridePropertiesName;
				}
				else
				{
					string text;
					if ((text = this.reader.ReadPropertyName()) == null)
					{
						break;
					}
					yield return text;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0400019E RID: 414
		public ES3Reader reader;
	}

	// Token: 0x020000F7 RID: 247
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ES3ReaderRawEnumerator
	{
		// Token: 0x06000545 RID: 1349 RVA: 0x0001EDC9 File Offset: 0x0001CFC9
		public ES3ReaderRawEnumerator(ES3Reader reader)
		{
			this.reader = reader;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001EDD8 File Offset: 0x0001CFD8
		public IEnumerator GetEnumerator()
		{
			for (;;)
			{
				string text = this.reader.ReadPropertyName();
				if (text == null)
				{
					break;
				}
				Type type = this.reader.ReadTypeFromHeader<object>();
				byte[] bytes = this.reader.ReadElement(false);
				this.reader.ReadKeySuffix();
				if (type != null)
				{
					yield return new KeyValuePair<string, ES3Data>(text, new ES3Data(type, bytes));
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0400019F RID: 415
		public ES3Reader reader;
	}
}
