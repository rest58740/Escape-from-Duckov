using System;
using System.Collections.Generic;
using System.Linq;

namespace System.ComponentModel.Composition.Primitives
{
	// Token: 0x0200008C RID: 140
	public abstract class ComposablePartDefinition
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060003B4 RID: 948
		public abstract IEnumerable<ExportDefinition> ExportDefinitions { get; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060003B5 RID: 949
		public abstract IEnumerable<ImportDefinition> ImportDefinitions { get; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0000AB7D File Offset: 0x00008D7D
		public virtual IDictionary<string, object> Metadata
		{
			get
			{
				return MetadataServices.EmptyMetadata;
			}
		}

		// Token: 0x060003B7 RID: 951
		public abstract ComposablePart CreatePart();

		// Token: 0x060003B8 RID: 952 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		internal virtual IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
		{
			List<Tuple<ComposablePartDefinition, ExportDefinition>> list = null;
			foreach (ExportDefinition exportDefinition in this.ExportDefinitions)
			{
				if (definition.IsConstraintSatisfiedBy(exportDefinition))
				{
					if (list == null)
					{
						list = new List<Tuple<ComposablePartDefinition, ExportDefinition>>();
					}
					list.Add(new Tuple<ComposablePartDefinition, ExportDefinition>(this, exportDefinition));
				}
			}
			IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> enumerable = list;
			return enumerable ?? ComposablePartDefinition._EmptyExports;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000AD5C File Offset: 0x00008F5C
		internal virtual ComposablePartDefinition GetGenericPartDefinition()
		{
			return null;
		}

		// Token: 0x04000176 RID: 374
		internal static readonly IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> _EmptyExports = Enumerable.Empty<Tuple<ComposablePartDefinition, ExportDefinition>>();
	}
}
