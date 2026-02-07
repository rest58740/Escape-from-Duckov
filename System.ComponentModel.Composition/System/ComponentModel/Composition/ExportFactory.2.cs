using System;

namespace System.ComponentModel.Composition
{
	// Token: 0x02000038 RID: 56
	public class ExportFactory<T, TMetadata> : ExportFactory<T>
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x0000590A File Offset: 0x00003B0A
		public ExportFactory(Func<Tuple<T, Action>> exportLifetimeContextCreator, TMetadata metadata) : base(exportLifetimeContextCreator)
		{
			this._metadata = metadata;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000591A File Offset: 0x00003B1A
		public TMetadata Metadata
		{
			get
			{
				return this._metadata;
			}
		}

		// Token: 0x040000B5 RID: 181
		private readonly TMetadata _metadata;
	}
}
