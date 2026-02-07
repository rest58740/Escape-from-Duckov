using System;
using System.Threading;

namespace System
{
	// Token: 0x0200001D RID: 29
	[Serializable]
	public class Lazy<T, TMetadata> : Lazy<T>
	{
		// Token: 0x06000106 RID: 262 RVA: 0x00003EC6 File Offset: 0x000020C6
		public Lazy(Func<T> valueFactory, TMetadata metadata) : base(valueFactory)
		{
			this._metadata = metadata;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003ED6 File Offset: 0x000020D6
		public Lazy(TMetadata metadata)
		{
			this._metadata = metadata;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00003EE5 File Offset: 0x000020E5
		public Lazy(TMetadata metadata, bool isThreadSafe) : base(isThreadSafe)
		{
			this._metadata = metadata;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00003EF5 File Offset: 0x000020F5
		public Lazy(Func<T> valueFactory, TMetadata metadata, bool isThreadSafe) : base(valueFactory, isThreadSafe)
		{
			this._metadata = metadata;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00003F06 File Offset: 0x00002106
		public Lazy(TMetadata metadata, LazyThreadSafetyMode mode) : base(mode)
		{
			this._metadata = metadata;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00003F16 File Offset: 0x00002116
		public Lazy(Func<T> valueFactory, TMetadata metadata, LazyThreadSafetyMode mode) : base(valueFactory, mode)
		{
			this._metadata = metadata;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00003F27 File Offset: 0x00002127
		public TMetadata Metadata
		{
			get
			{
				return this._metadata;
			}
		}

		// Token: 0x04000067 RID: 103
		private TMetadata _metadata;
	}
}
