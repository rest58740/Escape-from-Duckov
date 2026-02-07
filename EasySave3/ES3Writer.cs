using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ES3Internal;
using ES3Types;
using UnityEngine;

// Token: 0x02000015 RID: 21
public abstract class ES3Writer : IDisposable
{
	// Token: 0x06000195 RID: 405
	internal abstract void WriteNull();

	// Token: 0x06000196 RID: 406 RVA: 0x00006368 File Offset: 0x00004568
	internal virtual void StartWriteFile()
	{
		this.serializationDepth++;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00006378 File Offset: 0x00004578
	internal virtual void EndWriteFile()
	{
		this.serializationDepth--;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00006388 File Offset: 0x00004588
	internal virtual void StartWriteObject(string name)
	{
		this.serializationDepth++;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00006398 File Offset: 0x00004598
	internal virtual void EndWriteObject(string name)
	{
		this.serializationDepth--;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x000063A8 File Offset: 0x000045A8
	internal virtual void StartWriteProperty(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("Key or field name cannot be NULL when saving data.");
		}
		ES3Debug.Log("<b>" + name + "</b> (writing property)", null, this.serializationDepth);
	}

	// Token: 0x0600019B RID: 411 RVA: 0x000063D4 File Offset: 0x000045D4
	internal virtual void EndWriteProperty(string name)
	{
	}

	// Token: 0x0600019C RID: 412 RVA: 0x000063D6 File Offset: 0x000045D6
	internal virtual void StartWriteCollection()
	{
		this.serializationDepth++;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x000063E6 File Offset: 0x000045E6
	internal virtual void EndWriteCollection()
	{
		this.serializationDepth--;
	}

	// Token: 0x0600019E RID: 414
	internal abstract void StartWriteCollectionItem(int index);

	// Token: 0x0600019F RID: 415
	internal abstract void EndWriteCollectionItem(int index);

	// Token: 0x060001A0 RID: 416
	internal abstract void StartWriteDictionary();

	// Token: 0x060001A1 RID: 417
	internal abstract void EndWriteDictionary();

	// Token: 0x060001A2 RID: 418
	internal abstract void StartWriteDictionaryKey(int index);

	// Token: 0x060001A3 RID: 419
	internal abstract void EndWriteDictionaryKey(int index);

	// Token: 0x060001A4 RID: 420
	internal abstract void StartWriteDictionaryValue(int index);

	// Token: 0x060001A5 RID: 421
	internal abstract void EndWriteDictionaryValue(int index);

	// Token: 0x060001A6 RID: 422
	public abstract void Dispose();

	// Token: 0x060001A7 RID: 423
	internal abstract void WriteRawProperty(string name, byte[] bytes);

	// Token: 0x060001A8 RID: 424
	internal abstract void WritePrimitive(int value);

	// Token: 0x060001A9 RID: 425
	internal abstract void WritePrimitive(float value);

	// Token: 0x060001AA RID: 426
	internal abstract void WritePrimitive(bool value);

	// Token: 0x060001AB RID: 427
	internal abstract void WritePrimitive(decimal value);

	// Token: 0x060001AC RID: 428
	internal abstract void WritePrimitive(double value);

	// Token: 0x060001AD RID: 429
	internal abstract void WritePrimitive(long value);

	// Token: 0x060001AE RID: 430
	internal abstract void WritePrimitive(ulong value);

	// Token: 0x060001AF RID: 431
	internal abstract void WritePrimitive(uint value);

	// Token: 0x060001B0 RID: 432
	internal abstract void WritePrimitive(byte value);

	// Token: 0x060001B1 RID: 433
	internal abstract void WritePrimitive(sbyte value);

	// Token: 0x060001B2 RID: 434
	internal abstract void WritePrimitive(short value);

	// Token: 0x060001B3 RID: 435
	internal abstract void WritePrimitive(ushort value);

	// Token: 0x060001B4 RID: 436
	internal abstract void WritePrimitive(char value);

	// Token: 0x060001B5 RID: 437
	internal abstract void WritePrimitive(string value);

	// Token: 0x060001B6 RID: 438
	internal abstract void WritePrimitive(byte[] value);

	// Token: 0x060001B7 RID: 439 RVA: 0x000063F6 File Offset: 0x000045F6
	protected ES3Writer(ES3Settings settings, bool writeHeaderAndFooter, bool overwriteKeys)
	{
		this.settings = settings;
		this.writeHeaderAndFooter = writeHeaderAndFooter;
		this.overwriteKeys = overwriteKeys;
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000642C File Offset: 0x0000462C
	internal virtual void Write(string key, Type type, byte[] value)
	{
		this.StartWriteProperty(key);
		this.StartWriteObject(key);
		this.WriteType(type);
		this.WriteRawProperty("value", value);
		this.EndWriteObject(key);
		this.EndWriteProperty(key);
		this.MarkKeyForDeletion(key);
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00006464 File Offset: 0x00004664
	public virtual void Write<T>(string key, object value)
	{
		if (!(typeof(T) == typeof(object)))
		{
			this.Write(typeof(T), key, value);
			return;
		}
		if (value == null)
		{
			this.Write(typeof(object), key, null);
			return;
		}
		this.Write(value.GetType(), key, value);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x000064C4 File Offset: 0x000046C4
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void Write(Type type, string key, object value)
	{
		this.StartWriteProperty(key);
		this.StartWriteObject(key);
		this.WriteType(type);
		this.WriteProperty("value", value, ES3TypeMgr.GetOrCreateES3Type(type, true), this.settings.referenceMode);
		this.EndWriteObject(key);
		this.EndWriteProperty(key);
		this.MarkKeyForDeletion(key);
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000651C File Offset: 0x0000471C
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void Write(object value, ES3.ReferenceMode memberReferenceMode = ES3.ReferenceMode.ByRef)
	{
		if (value == null)
		{
			this.WriteNull();
			return;
		}
		ES3Type orCreateES3Type = ES3TypeMgr.GetOrCreateES3Type(value.GetType(), true);
		this.Write(value, orCreateES3Type, memberReferenceMode);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000654C File Offset: 0x0000474C
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void Write(object value, ES3Type type, ES3.ReferenceMode memberReferenceMode = ES3.ReferenceMode.ByRef)
	{
		if (value == null || (ES3Reflection.IsAssignableFrom(typeof(UnityEngine.Object), value.GetType()) && value as UnityEngine.Object == null))
		{
			this.WriteNull();
			return;
		}
		if (type == null || type.type == typeof(object))
		{
			Type type2 = value.GetType();
			type = ES3TypeMgr.GetOrCreateES3Type(type2, true);
			if (type == null)
			{
				string str = "Types of ";
				Type type3 = type2;
				throw new NotSupportedException(str + ((type3 != null) ? type3.ToString() : null) + " are not supported. Please see the Supported Types guide for more information: https://docs.moodkie.com/easy-save-3/es3-supported-types/");
			}
			if (!type.isCollection && !type.isDictionary)
			{
				this.StartWriteObject(null);
				this.WriteType(type2);
				type.Write(value, this);
				this.EndWriteObject(null);
				return;
			}
		}
		if (type == null)
		{
			throw new ArgumentNullException("ES3Type argument cannot be null.");
		}
		if (type.isUnsupported)
		{
			if (type.isCollection || type.isDictionary)
			{
				Type type4 = type.type;
				throw new NotSupportedException(((type4 != null) ? type4.ToString() : null) + " is not supported because it's element type is not supported. Please see the Supported Types guide for more information: https://docs.moodkie.com/easy-save-3/es3-supported-types/");
			}
			string str2 = "Types of ";
			Type type5 = type.type;
			throw new NotSupportedException(str2 + ((type5 != null) ? type5.ToString() : null) + " are not supported. Please see the Supported Types guide for more information: https://docs.moodkie.com/easy-save-3/es3-supported-types/");
		}
		else
		{
			if (type.isPrimitive || type.isEnum)
			{
				type.Write(value, this);
				return;
			}
			if (type.isCollection)
			{
				this.StartWriteCollection();
				((ES3CollectionType)type).Write(value, this, memberReferenceMode);
				this.EndWriteCollection();
				return;
			}
			if (type.isDictionary)
			{
				this.StartWriteDictionary();
				((ES3DictionaryType)type).Write(value, this, memberReferenceMode);
				this.EndWriteDictionary();
				return;
			}
			if (type.type == typeof(GameObject))
			{
				((ES3Type_GameObject)type).saveChildren = this.settings.saveChildren;
			}
			this.StartWriteObject(null);
			if (type.isES3TypeUnityObject)
			{
				((ES3UnityObjectType)type).WriteObject(value, this, memberReferenceMode);
			}
			else
			{
				type.Write(value, this);
			}
			this.EndWriteObject(null);
			return;
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00006730 File Offset: 0x00004930
	internal virtual void WriteRef(UnityEngine.Object obj)
	{
		ES3ReferenceMgrBase es3ReferenceMgrBase = ES3ReferenceMgrBase.Current;
		if (es3ReferenceMgrBase == null)
		{
			throw new InvalidOperationException(string.Format("An Easy Save 3 Manager is required to save references. To add one to your scene, exit playmode and go to Tools > Easy Save 3 > Add Manager to Scene. Object being saved by reference is {0} with name {1}.", obj.GetType(), obj.name));
		}
		long num = es3ReferenceMgrBase.Get(obj);
		if (num == -1L)
		{
			num = es3ReferenceMgrBase.Add(obj);
		}
		this.WriteProperty("_ES3Ref", num.ToString());
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000678F File Offset: 0x0000498F
	public virtual void WriteProperty(string name, object value)
	{
		this.WriteProperty(name, value, this.settings.memberReferenceMode);
	}

	// Token: 0x060001BF RID: 447 RVA: 0x000067A4 File Offset: 0x000049A4
	public virtual void WriteProperty(string name, object value, ES3.ReferenceMode memberReferenceMode)
	{
		if (this.SerializationDepthLimitExceeded())
		{
			return;
		}
		this.StartWriteProperty(name);
		this.Write(value, memberReferenceMode);
		this.EndWriteProperty(name);
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x000067C5 File Offset: 0x000049C5
	public virtual void WriteProperty<T>(string name, object value)
	{
		this.WriteProperty(name, value, ES3TypeMgr.GetOrCreateES3Type(typeof(T), true), this.settings.memberReferenceMode);
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x000067EA File Offset: 0x000049EA
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void WriteProperty(string name, object value, ES3Type type)
	{
		this.WriteProperty(name, value, type, this.settings.memberReferenceMode);
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00006800 File Offset: 0x00004A00
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void WriteProperty(string name, object value, ES3Type type, ES3.ReferenceMode memberReferenceMode)
	{
		if (this.SerializationDepthLimitExceeded())
		{
			return;
		}
		this.StartWriteProperty(name);
		this.Write(value, type, memberReferenceMode);
		this.EndWriteProperty(name);
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00006823 File Offset: 0x00004A23
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void WritePropertyByRef(string name, UnityEngine.Object value)
	{
		if (this.SerializationDepthLimitExceeded())
		{
			return;
		}
		this.StartWriteProperty(name);
		if (value == null)
		{
			this.WriteNull();
			return;
		}
		this.StartWriteObject(name);
		this.WriteRef(value);
		this.EndWriteObject(name);
		this.EndWriteProperty(name);
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00006864 File Offset: 0x00004A64
	public void WritePrivateProperty(string name, object objectContainingProperty)
	{
		ES3Reflection.ES3ReflectedMember es3ReflectedProperty = ES3Reflection.GetES3ReflectedProperty(objectContainingProperty.GetType(), name);
		if (es3ReflectedProperty.IsNull)
		{
			string str = "A private property named ";
			string str2 = " does not exist in the type ";
			Type type = objectContainingProperty.GetType();
			throw new MissingMemberException(str + name + str2 + ((type != null) ? type.ToString() : null));
		}
		this.WriteProperty(name, es3ReflectedProperty.GetValue(objectContainingProperty), ES3TypeMgr.GetOrCreateES3Type(es3ReflectedProperty.MemberType, true));
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x000068CC File Offset: 0x00004ACC
	public void WritePrivateField(string name, object objectContainingField)
	{
		ES3Reflection.ES3ReflectedMember es3ReflectedMember = ES3Reflection.GetES3ReflectedMember(objectContainingField.GetType(), name);
		if (es3ReflectedMember.IsNull)
		{
			string str = "A private field named ";
			string str2 = " does not exist in the type ";
			Type type = objectContainingField.GetType();
			throw new MissingMemberException(str + name + str2 + ((type != null) ? type.ToString() : null));
		}
		this.WriteProperty(name, es3ReflectedMember.GetValue(objectContainingField), ES3TypeMgr.GetOrCreateES3Type(es3ReflectedMember.MemberType, true));
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00006934 File Offset: 0x00004B34
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WritePrivateProperty(string name, object objectContainingProperty, ES3Type type)
	{
		ES3Reflection.ES3ReflectedMember es3ReflectedProperty = ES3Reflection.GetES3ReflectedProperty(objectContainingProperty.GetType(), name);
		if (es3ReflectedProperty.IsNull)
		{
			string str = "A private property named ";
			string str2 = " does not exist in the type ";
			Type type2 = objectContainingProperty.GetType();
			throw new MissingMemberException(str + name + str2 + ((type2 != null) ? type2.ToString() : null));
		}
		this.WriteProperty(name, es3ReflectedProperty.GetValue(objectContainingProperty), type);
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00006990 File Offset: 0x00004B90
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WritePrivateField(string name, object objectContainingField, ES3Type type)
	{
		ES3Reflection.ES3ReflectedMember es3ReflectedMember = ES3Reflection.GetES3ReflectedMember(objectContainingField.GetType(), name);
		if (es3ReflectedMember.IsNull)
		{
			string str = "A private field named ";
			string str2 = " does not exist in the type ";
			Type type2 = objectContainingField.GetType();
			throw new MissingMemberException(str + name + str2 + ((type2 != null) ? type2.ToString() : null));
		}
		this.WriteProperty(name, es3ReflectedMember.GetValue(objectContainingField), type);
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x000069EC File Offset: 0x00004BEC
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WritePrivatePropertyByRef(string name, object objectContainingProperty)
	{
		ES3Reflection.ES3ReflectedMember es3ReflectedProperty = ES3Reflection.GetES3ReflectedProperty(objectContainingProperty.GetType(), name);
		if (es3ReflectedProperty.IsNull)
		{
			string str = "A private property named ";
			string str2 = " does not exist in the type ";
			Type type = objectContainingProperty.GetType();
			throw new MissingMemberException(str + name + str2 + ((type != null) ? type.ToString() : null));
		}
		this.WritePropertyByRef(name, (UnityEngine.Object)es3ReflectedProperty.GetValue(objectContainingProperty));
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x00006A4C File Offset: 0x00004C4C
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WritePrivateFieldByRef(string name, object objectContainingField)
	{
		ES3Reflection.ES3ReflectedMember es3ReflectedMember = ES3Reflection.GetES3ReflectedMember(objectContainingField.GetType(), name);
		if (es3ReflectedMember.IsNull)
		{
			string str = "A private field named ";
			string str2 = " does not exist in the type ";
			Type type = objectContainingField.GetType();
			throw new MissingMemberException(str + name + str2 + ((type != null) ? type.ToString() : null));
		}
		this.WritePropertyByRef(name, (UnityEngine.Object)es3ReflectedMember.GetValue(objectContainingField));
	}

	// Token: 0x060001CA RID: 458 RVA: 0x00006AAB File Offset: 0x00004CAB
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void WriteType(Type type)
	{
		this.WriteProperty("__type", ES3Reflection.GetTypeString(type));
	}

	// Token: 0x060001CB RID: 459 RVA: 0x00006ABE File Offset: 0x00004CBE
	public static ES3Writer Create(string filePath, ES3Settings settings)
	{
		return ES3Writer.Create(new ES3Settings(filePath, settings));
	}

	// Token: 0x060001CC RID: 460 RVA: 0x00006ACC File Offset: 0x00004CCC
	public static ES3Writer Create(ES3Settings settings)
	{
		return ES3Writer.Create(settings, true, true, false);
	}

	// Token: 0x060001CD RID: 461 RVA: 0x00006AD8 File Offset: 0x00004CD8
	internal static ES3Writer Create(ES3Settings settings, bool writeHeaderAndFooter, bool overwriteKeys, bool append)
	{
		Stream stream = ES3Stream.CreateStream(settings, append ? ES3FileMode.Append : ES3FileMode.Write);
		if (stream == null)
		{
			return null;
		}
		return ES3Writer.Create(stream, settings, writeHeaderAndFooter, overwriteKeys);
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00006B01 File Offset: 0x00004D01
	internal static ES3Writer Create(Stream stream, ES3Settings settings, bool writeHeaderAndFooter, bool overwriteKeys)
	{
		if (stream.GetType() == typeof(MemoryStream))
		{
			settings = (ES3Settings)settings.Clone();
			settings.location = ES3.Location.InternalMS;
		}
		if (settings.format == ES3.Format.JSON)
		{
			return new ES3JSONWriter(stream, settings, writeHeaderAndFooter, overwriteKeys);
		}
		return null;
	}

	// Token: 0x060001CF RID: 463 RVA: 0x00006B41 File Offset: 0x00004D41
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected bool SerializationDepthLimitExceeded()
	{
		if (this.serializationDepth > this.settings.serializationDepthLimit)
		{
			ES3Debug.LogWarning("Serialization depth limit of " + this.settings.serializationDepthLimit.ToString() + " has been exceeded, indicating that there may be a circular reference.\nIf this is not a circular reference, you can increase the depth by going to Window > Easy Save 3 > Settings > Advanced Settings > Serialization Depth Limit", null, 0);
			return true;
		}
		return false;
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x00006B7F File Offset: 0x00004D7F
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void MarkKeyForDeletion(string key)
	{
		this.keysToDelete.Add(key);
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00006B90 File Offset: 0x00004D90
	protected void Merge()
	{
		using (ES3Reader es3Reader = ES3Reader.Create(this.settings))
		{
			if (es3Reader != null)
			{
				this.Merge(es3Reader);
			}
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00006BD4 File Offset: 0x00004DD4
	protected void Merge(ES3Reader reader)
	{
		foreach (object obj in reader.RawEnumerator)
		{
			KeyValuePair<string, ES3Data> keyValuePair = (KeyValuePair<string, ES3Data>)obj;
			if (!this.keysToDelete.Contains(keyValuePair.Key) || keyValuePair.Value.type == null)
			{
				this.Write(keyValuePair.Key, keyValuePair.Value.type.type, keyValuePair.Value.bytes);
			}
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00006C74 File Offset: 0x00004E74
	public virtual void Save()
	{
		this.Save(this.overwriteKeys);
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00006C82 File Offset: 0x00004E82
	public virtual void Save(bool overwriteKeys)
	{
		if (overwriteKeys)
		{
			this.Merge();
		}
		this.EndWriteFile();
		this.Dispose();
		if (this.settings.location == ES3.Location.File || this.settings.location == ES3.Location.PlayerPrefs)
		{
			ES3IO.CommitBackup(this.settings);
		}
	}

	// Token: 0x0400004D RID: 77
	public ES3Settings settings;

	// Token: 0x0400004E RID: 78
	protected HashSet<string> keysToDelete = new HashSet<string>();

	// Token: 0x0400004F RID: 79
	internal bool writeHeaderAndFooter = true;

	// Token: 0x04000050 RID: 80
	internal bool overwriteKeys = true;

	// Token: 0x04000051 RID: 81
	protected int serializationDepth;
}
