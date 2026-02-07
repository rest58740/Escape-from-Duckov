using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting
{
	// Token: 0x020000C9 RID: 201
	public class CompositionBatch
	{
		// Token: 0x06000522 RID: 1314 RVA: 0x0000F5AE File Offset: 0x0000D7AE
		public CompositionBatch() : this(null, null)
		{
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000F5B8 File Offset: 0x0000D7B8
		public CompositionBatch(IEnumerable<ComposablePart> partsToAdd, IEnumerable<ComposablePart> partsToRemove)
		{
			this._partsToAdd = new List<ComposablePart>();
			if (partsToAdd != null)
			{
				foreach (ComposablePart composablePart in partsToAdd)
				{
					if (composablePart == null)
					{
						throw ExceptionBuilder.CreateContainsNullElement("partsToAdd");
					}
					this._partsToAdd.Add(composablePart);
				}
			}
			this._readOnlyPartsToAdd = this._partsToAdd.AsReadOnly();
			this._partsToRemove = new List<ComposablePart>();
			if (partsToRemove != null)
			{
				foreach (ComposablePart composablePart2 in partsToRemove)
				{
					if (composablePart2 == null)
					{
						throw ExceptionBuilder.CreateContainsNullElement("partsToRemove");
					}
					this._partsToRemove.Add(composablePart2);
				}
			}
			this._readOnlyPartsToRemove = this._partsToRemove.AsReadOnly();
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0000F6AC File Offset: 0x0000D8AC
		public ReadOnlyCollection<ComposablePart> PartsToAdd
		{
			get
			{
				object @lock = this._lock;
				ReadOnlyCollection<ComposablePart> readOnlyPartsToAdd;
				lock (@lock)
				{
					this._copyNeededForAdd = true;
					readOnlyPartsToAdd = this._readOnlyPartsToAdd;
				}
				return readOnlyPartsToAdd;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0000F6F8 File Offset: 0x0000D8F8
		public ReadOnlyCollection<ComposablePart> PartsToRemove
		{
			get
			{
				object @lock = this._lock;
				ReadOnlyCollection<ComposablePart> readOnlyPartsToRemove;
				lock (@lock)
				{
					this._copyNeededForRemove = true;
					readOnlyPartsToRemove = this._readOnlyPartsToRemove;
				}
				return readOnlyPartsToRemove;
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000F744 File Offset: 0x0000D944
		public void AddPart(ComposablePart part)
		{
			Requires.NotNull<ComposablePart>(part, "part");
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._copyNeededForAdd)
				{
					this._partsToAdd = new List<ComposablePart>(this._partsToAdd);
					this._readOnlyPartsToAdd = this._partsToAdd.AsReadOnly();
					this._copyNeededForAdd = false;
				}
				this._partsToAdd.Add(part);
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000F7C8 File Offset: 0x0000D9C8
		public void RemovePart(ComposablePart part)
		{
			Requires.NotNull<ComposablePart>(part, "part");
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._copyNeededForRemove)
				{
					this._partsToRemove = new List<ComposablePart>(this._partsToRemove);
					this._readOnlyPartsToRemove = this._partsToRemove.AsReadOnly();
					this._copyNeededForRemove = false;
				}
				this._partsToRemove.Add(part);
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0000F84C File Offset: 0x0000DA4C
		public ComposablePart AddExport(Export export)
		{
			Requires.NotNull<Export>(export, "export");
			ComposablePart composablePart = new CompositionBatch.SingleExportComposablePart(export);
			this.AddPart(composablePart);
			return composablePart;
		}

		// Token: 0x04000224 RID: 548
		private object _lock = new object();

		// Token: 0x04000225 RID: 549
		private bool _copyNeededForAdd;

		// Token: 0x04000226 RID: 550
		private bool _copyNeededForRemove;

		// Token: 0x04000227 RID: 551
		private List<ComposablePart> _partsToAdd;

		// Token: 0x04000228 RID: 552
		private ReadOnlyCollection<ComposablePart> _readOnlyPartsToAdd;

		// Token: 0x04000229 RID: 553
		private List<ComposablePart> _partsToRemove;

		// Token: 0x0400022A RID: 554
		private ReadOnlyCollection<ComposablePart> _readOnlyPartsToRemove;

		// Token: 0x020000CA RID: 202
		private class SingleExportComposablePart : ComposablePart
		{
			// Token: 0x06000529 RID: 1321 RVA: 0x0000F873 File Offset: 0x0000DA73
			public SingleExportComposablePart(Export export)
			{
				Assumes.NotNull<Export>(export);
				this._export = export;
			}

			// Token: 0x1700015C RID: 348
			// (get) Token: 0x0600052A RID: 1322 RVA: 0x0000AB7D File Offset: 0x00008D7D
			public override IDictionary<string, object> Metadata
			{
				get
				{
					return MetadataServices.EmptyMetadata;
				}
			}

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x0600052B RID: 1323 RVA: 0x0000F888 File Offset: 0x0000DA88
			public override IEnumerable<ExportDefinition> ExportDefinitions
			{
				get
				{
					return new ExportDefinition[]
					{
						this._export.Definition
					};
				}
			}

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x0600052C RID: 1324 RVA: 0x0000DBB3 File Offset: 0x0000BDB3
			public override IEnumerable<ImportDefinition> ImportDefinitions
			{
				get
				{
					return Enumerable.Empty<ImportDefinition>();
				}
			}

			// Token: 0x0600052D RID: 1325 RVA: 0x0000F89E File Offset: 0x0000DA9E
			public override object GetExportedValue(ExportDefinition definition)
			{
				Requires.NotNull<ExportDefinition>(definition, "definition");
				if (definition != this._export.Definition)
				{
					throw ExceptionBuilder.CreateExportDefinitionNotOnThisComposablePart("definition");
				}
				return this._export.Value;
			}

			// Token: 0x0600052E RID: 1326 RVA: 0x0000F8CF File Offset: 0x0000DACF
			public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports)
			{
				Requires.NotNull<ImportDefinition>(definition, "definition");
				Requires.NotNullOrNullElements<Export>(exports, "exports");
				throw ExceptionBuilder.CreateImportDefinitionNotOnThisComposablePart("definition");
			}

			// Token: 0x0400022B RID: 555
			private readonly Export _export;
		}
	}
}
