using System;
using System.Collections.Generic;

namespace System.ComponentModel.Composition.Primitives
{
	// Token: 0x02000089 RID: 137
	public abstract class ComposablePart
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060003A1 RID: 929
		public abstract IEnumerable<ExportDefinition> ExportDefinitions { get; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060003A2 RID: 930
		public abstract IEnumerable<ImportDefinition> ImportDefinitions { get; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000AB7D File Offset: 0x00008D7D
		public virtual IDictionary<string, object> Metadata
		{
			get
			{
				return MetadataServices.EmptyMetadata;
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000028FF File Offset: 0x00000AFF
		public virtual void Activate()
		{
		}

		// Token: 0x060003A5 RID: 933
		public abstract object GetExportedValue(ExportDefinition definition);

		// Token: 0x060003A6 RID: 934
		public abstract void SetImport(ImportDefinition definition, IEnumerable<Export> exports);
	}
}
