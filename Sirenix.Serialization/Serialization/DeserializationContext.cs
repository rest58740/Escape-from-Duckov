using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x02000060 RID: 96
	public sealed class DeserializationContext : ICacheNotificationReceiver
	{
		// Token: 0x06000347 RID: 839 RVA: 0x00017BDC File Offset: 0x00015DDC
		public DeserializationContext() : this(default(StreamingContext), new FormatterConverter())
		{
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00017BFD File Offset: 0x00015DFD
		public DeserializationContext(StreamingContext context) : this(context, new FormatterConverter())
		{
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00017C0C File Offset: 0x00015E0C
		public DeserializationContext(FormatterConverter formatterConverter) : this(default(StreamingContext), formatterConverter)
		{
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00017C29 File Offset: 0x00015E29
		public DeserializationContext(StreamingContext context, FormatterConverter formatterConverter)
		{
			if (formatterConverter == null)
			{
				throw new ArgumentNullException("formatterConverter");
			}
			this.streamingContext = context;
			this.formatterConverter = formatterConverter;
			this.Reset();
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00017C63 File Offset: 0x00015E63
		// (set) Token: 0x0600034C RID: 844 RVA: 0x00017C7E File Offset: 0x00015E7E
		public TwoWaySerializationBinder Binder
		{
			get
			{
				if (this.binder == null)
				{
					this.binder = TwoWaySerializationBinder.Default;
				}
				return this.binder;
			}
			set
			{
				this.binder = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00017C87 File Offset: 0x00015E87
		// (set) Token: 0x0600034E RID: 846 RVA: 0x00017C8F File Offset: 0x00015E8F
		public IExternalStringReferenceResolver StringReferenceResolver { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00017C98 File Offset: 0x00015E98
		// (set) Token: 0x06000350 RID: 848 RVA: 0x00017CA0 File Offset: 0x00015EA0
		public IExternalGuidReferenceResolver GuidReferenceResolver { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000351 RID: 849 RVA: 0x00017CA9 File Offset: 0x00015EA9
		// (set) Token: 0x06000352 RID: 850 RVA: 0x00017CB1 File Offset: 0x00015EB1
		public IExternalIndexReferenceResolver IndexReferenceResolver { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00017CBA File Offset: 0x00015EBA
		public StreamingContext StreamingContext
		{
			get
			{
				return this.streamingContext;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00017CC2 File Offset: 0x00015EC2
		public IFormatterConverter FormatterConverter
		{
			get
			{
				return this.formatterConverter;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00017CCA File Offset: 0x00015ECA
		// (set) Token: 0x06000356 RID: 854 RVA: 0x00017CE5 File Offset: 0x00015EE5
		public SerializationConfig Config
		{
			get
			{
				if (this.config == null)
				{
					this.config = new SerializationConfig();
				}
				return this.config;
			}
			set
			{
				this.config = value;
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00017CEE File Offset: 0x00015EEE
		public void RegisterInternalReference(int id, object reference)
		{
			this.internalIdReferenceMap[id] = reference;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00017D00 File Offset: 0x00015F00
		public object GetInternalReference(int id)
		{
			object result;
			this.internalIdReferenceMap.TryGetValue(id, ref result);
			return result;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00017D20 File Offset: 0x00015F20
		public object GetExternalObject(int index)
		{
			if (this.IndexReferenceResolver == null)
			{
				this.Config.DebugContext.LogWarning("Tried to resolve external reference by index (" + index.ToString() + "), but no index reference resolver is assigned to the deserialization context. External reference has been lost.");
				return null;
			}
			object result;
			if (this.IndexReferenceResolver.TryResolveReference(index, out result))
			{
				return result;
			}
			this.Config.DebugContext.LogWarning("Failed to resolve external reference by index (" + index.ToString() + "); the index resolver could not resolve the index. Reference lost.");
			return null;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00017D98 File Offset: 0x00015F98
		public object GetExternalObject(Guid guid)
		{
			Guid guid2;
			if (this.GuidReferenceResolver == null)
			{
				DebugContext debugContext = this.Config.DebugContext;
				string text = "Tried to resolve external reference by guid (";
				guid2 = guid;
				debugContext.LogWarning(text + guid2.ToString() + "), but no guid reference resolver is assigned to the deserialization context. External reference has been lost.");
				return null;
			}
			for (IExternalGuidReferenceResolver externalGuidReferenceResolver = this.GuidReferenceResolver; externalGuidReferenceResolver != null; externalGuidReferenceResolver = externalGuidReferenceResolver.NextResolver)
			{
				object result;
				if (externalGuidReferenceResolver.TryResolveReference(guid, out result))
				{
					return result;
				}
			}
			DebugContext debugContext2 = this.Config.DebugContext;
			string text2 = "Failed to resolve external reference by guid (";
			guid2 = guid;
			debugContext2.LogWarning(text2 + guid2.ToString() + "); no guid resolver could resolve the guid. Reference lost.");
			return null;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00017E2C File Offset: 0x0001602C
		public object GetExternalObject(string id)
		{
			if (this.StringReferenceResolver == null)
			{
				this.Config.DebugContext.LogWarning("Tried to resolve external reference by string (" + id + "), but no string reference resolver is assigned to the deserialization context. External reference has been lost.");
				return null;
			}
			for (IExternalStringReferenceResolver externalStringReferenceResolver = this.StringReferenceResolver; externalStringReferenceResolver != null; externalStringReferenceResolver = externalStringReferenceResolver.NextResolver)
			{
				object result;
				if (externalStringReferenceResolver.TryResolveReference(id, out result))
				{
					return result;
				}
			}
			this.Config.DebugContext.LogWarning("Failed to resolve external reference by string (" + id + "); no string resolver could resolve the string. Reference lost.");
			return null;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00017EA4 File Offset: 0x000160A4
		public void Reset()
		{
			if (this.config != null)
			{
				this.config.ResetToDefault();
			}
			this.internalIdReferenceMap.Clear();
			this.IndexReferenceResolver = null;
			this.GuidReferenceResolver = null;
			this.StringReferenceResolver = null;
			this.binder = null;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00017EE0 File Offset: 0x000160E0
		void ICacheNotificationReceiver.OnFreed()
		{
			this.Reset();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000021B8 File Offset: 0x000003B8
		void ICacheNotificationReceiver.OnClaimed()
		{
		}

		// Token: 0x04000113 RID: 275
		private SerializationConfig config;

		// Token: 0x04000114 RID: 276
		private Dictionary<int, object> internalIdReferenceMap = new Dictionary<int, object>(128);

		// Token: 0x04000115 RID: 277
		private StreamingContext streamingContext;

		// Token: 0x04000116 RID: 278
		private IFormatterConverter formatterConverter;

		// Token: 0x04000117 RID: 279
		private TwoWaySerializationBinder binder;
	}
}
