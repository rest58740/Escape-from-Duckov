using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Resources
{
	// Token: 0x0200086E RID: 2158
	[ComVisible(true)]
	[Serializable]
	public class ResourceSet : IDisposable, IEnumerable
	{
		// Token: 0x060047E4 RID: 18404 RVA: 0x000EC061 File Offset: 0x000EA261
		protected ResourceSet()
		{
			this.CommonInit();
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x0000259F File Offset: 0x0000079F
		internal ResourceSet(bool junk)
		{
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x000EC06F File Offset: 0x000EA26F
		public ResourceSet(string fileName)
		{
			this.Reader = new ResourceReader(fileName);
			this.CommonInit();
			this.ReadResources();
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x000EC08F File Offset: 0x000EA28F
		[SecurityCritical]
		public ResourceSet(Stream stream)
		{
			this.Reader = new ResourceReader(stream);
			this.CommonInit();
			this.ReadResources();
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x000EC0AF File Offset: 0x000EA2AF
		public ResourceSet(IResourceReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.Reader = reader;
			this.CommonInit();
			this.ReadResources();
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x000EC0D8 File Offset: 0x000EA2D8
		private void CommonInit()
		{
			this.Table = new Hashtable();
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x000EC0E5 File Offset: 0x000EA2E5
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x000EC0F0 File Offset: 0x000EA2F0
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				IResourceReader reader = this.Reader;
				this.Reader = null;
				if (reader != null)
				{
					reader.Close();
				}
			}
			this.Reader = null;
			this._caseInsensitiveTable = null;
			this.Table = null;
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x000EC0E5 File Offset: 0x000EA2E5
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x000EC12C File Offset: 0x000EA32C
		public virtual Type GetDefaultReader()
		{
			return typeof(ResourceReader);
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x000EC138 File Offset: 0x000EA338
		public virtual Type GetDefaultWriter()
		{
			return typeof(ResourceWriter);
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x000EC144 File Offset: 0x000EA344
		[ComVisible(false)]
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x000EC144 File Offset: 0x000EA344
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x000EC14C File Offset: 0x000EA34C
		private IDictionaryEnumerator GetEnumeratorHelper()
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a closed resource set."));
			}
			return table.GetEnumerator();
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x000EC170 File Offset: 0x000EA370
		public virtual string GetString(string name)
		{
			object objectInternal = this.GetObjectInternal(name);
			string result;
			try
			{
				result = (string)objectInternal;
			}
			catch (InvalidCastException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Resource '{0}' was not a String - call GetObject instead.", new object[]
				{
					name
				}));
			}
			return result;
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x000EC1BC File Offset: 0x000EA3BC
		public virtual string GetString(string name, bool ignoreCase)
		{
			object obj = this.GetObjectInternal(name);
			string text;
			try
			{
				text = (string)obj;
			}
			catch (InvalidCastException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Resource '{0}' was not a String - call GetObject instead.", new object[]
				{
					name
				}));
			}
			if (text != null || !ignoreCase)
			{
				return text;
			}
			obj = this.GetCaseInsensitiveObjectInternal(name);
			string result;
			try
			{
				result = (string)obj;
			}
			catch (InvalidCastException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Resource '{0}' was not a String - call GetObject instead.", new object[]
				{
					name
				}));
			}
			return result;
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x000EC248 File Offset: 0x000EA448
		public virtual object GetObject(string name)
		{
			return this.GetObjectInternal(name);
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x000EC254 File Offset: 0x000EA454
		public virtual object GetObject(string name, bool ignoreCase)
		{
			object objectInternal = this.GetObjectInternal(name);
			if (objectInternal != null || !ignoreCase)
			{
				return objectInternal;
			}
			return this.GetCaseInsensitiveObjectInternal(name);
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x000EC278 File Offset: 0x000EA478
		protected virtual void ReadResources()
		{
			IDictionaryEnumerator enumerator = this.Reader.GetEnumerator();
			while (enumerator.MoveNext())
			{
				object value = enumerator.Value;
				this.Table.Add(enumerator.Key, value);
			}
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x000EC2B4 File Offset: 0x000EA4B4
		private object GetObjectInternal(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a closed resource set."));
			}
			return table[name];
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x000EC2E4 File Offset: 0x000EA4E4
		private object GetCaseInsensitiveObjectInternal(string name)
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a closed resource set."));
			}
			Hashtable hashtable = this._caseInsensitiveTable;
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
				IDictionaryEnumerator enumerator = table.GetEnumerator();
				while (enumerator.MoveNext())
				{
					hashtable.Add(enumerator.Key, enumerator.Value);
				}
				this._caseInsensitiveTable = hashtable;
			}
			return hashtable[name];
		}

		// Token: 0x04002E03 RID: 11779
		[NonSerialized]
		protected IResourceReader Reader;

		// Token: 0x04002E04 RID: 11780
		protected Hashtable Table;

		// Token: 0x04002E05 RID: 11781
		private Hashtable _caseInsensitiveTable;
	}
}
