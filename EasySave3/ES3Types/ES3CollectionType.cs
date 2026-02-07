using System;
using System.Collections;
using System.Collections.Generic;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200001D RID: 29
	[Preserve]
	public abstract class ES3CollectionType : ES3Type
	{
		// Token: 0x060001F3 RID: 499
		public abstract object Read(ES3Reader reader);

		// Token: 0x060001F4 RID: 500
		public abstract void ReadInto(ES3Reader reader, object obj);

		// Token: 0x060001F5 RID: 501
		public abstract void Write(object obj, ES3Writer writer, ES3.ReferenceMode memberReferenceMode);

		// Token: 0x060001F6 RID: 502 RVA: 0x000075E4 File Offset: 0x000057E4
		public ES3CollectionType(Type type) : base(type)
		{
			this.elementType = ES3TypeMgr.GetOrCreateES3Type(ES3Reflection.GetElementTypes(type)[0], false);
			this.isCollection = true;
			if (this.elementType == null)
			{
				this.isUnsupported = true;
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007617 File Offset: 0x00005817
		public ES3CollectionType(Type type, ES3Type elementType) : base(type)
		{
			this.elementType = elementType;
			this.isCollection = true;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000762E File Offset: 0x0000582E
		[Preserve]
		public override void Write(object obj, ES3Writer writer)
		{
			this.Write(obj, writer, ES3.ReferenceMode.ByRefAndValue);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00007639 File Offset: 0x00005839
		protected virtual bool ReadICollection<T>(ES3Reader reader, ICollection<T> collection, ES3Type elementType)
		{
			if (reader.StartReadCollection())
			{
				return false;
			}
			while (reader.StartReadCollectionItem())
			{
				collection.Add(reader.Read<T>(elementType));
				if (reader.EndReadCollectionItem())
				{
					break;
				}
			}
			reader.EndReadCollection();
			return true;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00007669 File Offset: 0x00005869
		protected virtual void ReadICollectionInto<T>(ES3Reader reader, ICollection<T> collection, ES3Type elementType)
		{
			this.ReadICollectionInto<T>(reader, collection, elementType);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007674 File Offset: 0x00005874
		[Preserve]
		protected virtual void ReadICollectionInto(ES3Reader reader, ICollection collection, ES3Type elementType)
		{
			if (reader.StartReadCollection())
			{
				throw new NullReferenceException("The Collection we are trying to load is stored as null, which is not allowed when using ReadInto methods.");
			}
			int num = 0;
			foreach (object obj in collection)
			{
				num++;
				if (!reader.StartReadCollectionItem())
				{
					break;
				}
				reader.ReadInto<object>(obj, elementType);
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

		// Token: 0x04000054 RID: 84
		public ES3Type elementType;
	}
}
