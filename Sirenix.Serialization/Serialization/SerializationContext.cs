using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x02000077 RID: 119
	public sealed class SerializationContext : ICacheNotificationReceiver
	{
		// Token: 0x060003CC RID: 972 RVA: 0x0001A930 File Offset: 0x00018B30
		public SerializationContext() : this(default(StreamingContext), new FormatterConverter())
		{
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001A951 File Offset: 0x00018B51
		public SerializationContext(StreamingContext context) : this(context, new FormatterConverter())
		{
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001A960 File Offset: 0x00018B60
		public SerializationContext(FormatterConverter formatterConverter) : this(default(StreamingContext), formatterConverter)
		{
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001A97D File Offset: 0x00018B7D
		public SerializationContext(StreamingContext context, FormatterConverter formatterConverter)
		{
			if (formatterConverter == null)
			{
				throw new ArgumentNullException("formatterConverter");
			}
			this.streamingContext = context;
			this.formatterConverter = formatterConverter;
			this.ResetToDefault();
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0001A9BC File Offset: 0x00018BBC
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x0001A9D7 File Offset: 0x00018BD7
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0001A9E0 File Offset: 0x00018BE0
		public StreamingContext StreamingContext
		{
			get
			{
				return this.streamingContext;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0001A9E8 File Offset: 0x00018BE8
		public IFormatterConverter FormatterConverter
		{
			get
			{
				return this.formatterConverter;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0001A9F0 File Offset: 0x00018BF0
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x0001A9F8 File Offset: 0x00018BF8
		public IExternalIndexReferenceResolver IndexReferenceResolver { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x0001AA01 File Offset: 0x00018C01
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x0001AA09 File Offset: 0x00018C09
		public IExternalStringReferenceResolver StringReferenceResolver { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0001AA12 File Offset: 0x00018C12
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0001AA1A File Offset: 0x00018C1A
		public IExternalGuidReferenceResolver GuidReferenceResolver { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0001AA23 File Offset: 0x00018C23
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0001AA3E File Offset: 0x00018C3E
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

		// Token: 0x060003DC RID: 988 RVA: 0x0001AA47 File Offset: 0x00018C47
		public bool TryGetInternalReferenceId(object reference, out int id)
		{
			return this.internalReferenceIdMap.TryGetValue(reference, ref id);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001AA56 File Offset: 0x00018C56
		public bool TryRegisterInternalReference(object reference, out int id)
		{
			if (!this.internalReferenceIdMap.TryGetValue(reference, ref id))
			{
				id = this.internalReferenceIdMap.Count;
				this.internalReferenceIdMap.Add(reference, id);
				return true;
			}
			return false;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001AA85 File Offset: 0x00018C85
		public bool TryRegisterExternalReference(object obj, out int index)
		{
			if (this.IndexReferenceResolver == null)
			{
				index = -1;
				return false;
			}
			if (this.IndexReferenceResolver.CanReference(obj, out index))
			{
				return true;
			}
			index = -1;
			return false;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001AAAC File Offset: 0x00018CAC
		public bool TryRegisterExternalReference(object obj, out Guid guid)
		{
			if (this.GuidReferenceResolver == null)
			{
				guid = Guid.Empty;
				return false;
			}
			for (IExternalGuidReferenceResolver externalGuidReferenceResolver = this.GuidReferenceResolver; externalGuidReferenceResolver != null; externalGuidReferenceResolver = externalGuidReferenceResolver.NextResolver)
			{
				if (externalGuidReferenceResolver.CanReference(obj, out guid))
				{
					return true;
				}
			}
			guid = Guid.Empty;
			return false;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0001AAFC File Offset: 0x00018CFC
		public bool TryRegisterExternalReference(object obj, out string id)
		{
			if (this.StringReferenceResolver == null)
			{
				id = null;
				return false;
			}
			for (IExternalStringReferenceResolver externalStringReferenceResolver = this.StringReferenceResolver; externalStringReferenceResolver != null; externalStringReferenceResolver = externalStringReferenceResolver.NextResolver)
			{
				if (externalStringReferenceResolver.CanReference(obj, out id))
				{
					return true;
				}
			}
			id = null;
			return false;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001AB39 File Offset: 0x00018D39
		public void ResetInternalReferences()
		{
			this.internalReferenceIdMap.Clear();
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001AB46 File Offset: 0x00018D46
		public void ResetToDefault()
		{
			if (this.config != null)
			{
				this.config.ResetToDefault();
			}
			this.internalReferenceIdMap.Clear();
			this.IndexReferenceResolver = null;
			this.GuidReferenceResolver = null;
			this.StringReferenceResolver = null;
			this.binder = null;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001AB82 File Offset: 0x00018D82
		void ICacheNotificationReceiver.OnFreed()
		{
			this.ResetToDefault();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000021B8 File Offset: 0x000003B8
		void ICacheNotificationReceiver.OnClaimed()
		{
		}

		// Token: 0x04000158 RID: 344
		private SerializationConfig config;

		// Token: 0x04000159 RID: 345
		private Dictionary<object, int> internalReferenceIdMap = new Dictionary<object, int>(128, ReferenceEqualityComparer<object>.Default);

		// Token: 0x0400015A RID: 346
		private StreamingContext streamingContext;

		// Token: 0x0400015B RID: 347
		private IFormatterConverter formatterConverter;

		// Token: 0x0400015C RID: 348
		private TwoWaySerializationBinder binder;
	}
}
