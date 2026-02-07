using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Linq;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting
{
	// Token: 0x020000DE RID: 222
	public abstract class ExportProvider
	{
		// Token: 0x060005CC RID: 1484 RVA: 0x00011A3D File Offset: 0x0000FC3D
		public Lazy<T> GetExport<T>()
		{
			return this.GetExport<T>(null);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00011A46 File Offset: 0x0000FC46
		public Lazy<T> GetExport<T>(string contractName)
		{
			return this.GetExportCore<T>(contractName);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00011A4F File Offset: 0x0000FC4F
		public Lazy<T, TMetadataView> GetExport<T, TMetadataView>()
		{
			return this.GetExport<T, TMetadataView>(null);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00011A58 File Offset: 0x0000FC58
		public Lazy<T, TMetadataView> GetExport<T, TMetadataView>(string contractName)
		{
			return this.GetExportCore<T, TMetadataView>(contractName);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00011A64 File Offset: 0x0000FC64
		public IEnumerable<Lazy<object, object>> GetExports(Type type, Type metadataViewType, string contractName)
		{
			IEnumerable<Export> exportsCore = this.GetExportsCore(type, metadataViewType, contractName, ImportCardinality.ZeroOrMore);
			Collection<Lazy<object, object>> collection = new Collection<Lazy<object, object>>();
			Func<Export, Lazy<object, object>> func = ExportServices.CreateSemiStronglyTypedLazyFactory(type, metadataViewType);
			foreach (Export export in exportsCore)
			{
				collection.Add(func.Invoke(export));
			}
			return collection;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00011ACC File Offset: 0x0000FCCC
		public IEnumerable<Lazy<T>> GetExports<T>()
		{
			return this.GetExports<T>(null);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00011AD5 File Offset: 0x0000FCD5
		public IEnumerable<Lazy<T>> GetExports<T>(string contractName)
		{
			return this.GetExportsCore<T>(contractName);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00011ADE File Offset: 0x0000FCDE
		public IEnumerable<Lazy<T, TMetadataView>> GetExports<T, TMetadataView>()
		{
			return this.GetExports<T, TMetadataView>(null);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00011AE7 File Offset: 0x0000FCE7
		public IEnumerable<Lazy<T, TMetadataView>> GetExports<T, TMetadataView>(string contractName)
		{
			return this.GetExportsCore<T, TMetadataView>(contractName);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00011AF0 File Offset: 0x0000FCF0
		public T GetExportedValue<T>()
		{
			return this.GetExportedValue<T>(null);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00011AF9 File Offset: 0x0000FCF9
		public T GetExportedValue<T>(string contractName)
		{
			return this.GetExportedValueCore<T>(contractName, ImportCardinality.ExactlyOne);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00011B03 File Offset: 0x0000FD03
		public T GetExportedValueOrDefault<T>()
		{
			return this.GetExportedValueOrDefault<T>(null);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00011B0C File Offset: 0x0000FD0C
		public T GetExportedValueOrDefault<T>(string contractName)
		{
			return this.GetExportedValueCore<T>(contractName, ImportCardinality.ZeroOrOne);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00011B16 File Offset: 0x0000FD16
		public IEnumerable<T> GetExportedValues<T>()
		{
			return this.GetExportedValues<T>(null);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00011B1F File Offset: 0x0000FD1F
		public IEnumerable<T> GetExportedValues<T>(string contractName)
		{
			return this.GetExportedValuesCore<T>(contractName);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00011B28 File Offset: 0x0000FD28
		private IEnumerable<T> GetExportedValuesCore<T>(string contractName)
		{
			IEnumerable<Export> exportsCore = this.GetExportsCore(typeof(T), null, contractName, ImportCardinality.ZeroOrMore);
			Collection<T> collection = new Collection<T>();
			foreach (Export export in exportsCore)
			{
				collection.Add(ExportServices.GetCastedExportedValue<T>(export));
			}
			return collection;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00011B90 File Offset: 0x0000FD90
		private T GetExportedValueCore<T>(string contractName, ImportCardinality cardinality)
		{
			Assumes.IsTrue(cardinality.IsAtMostOne());
			Export export = this.GetExportsCore(typeof(T), null, contractName, cardinality).SingleOrDefault<Export>();
			if (export == null)
			{
				return default(T);
			}
			return ExportServices.GetCastedExportedValue<T>(export);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00011BD4 File Offset: 0x0000FDD4
		private IEnumerable<Lazy<T>> GetExportsCore<T>(string contractName)
		{
			IEnumerable<Export> exportsCore = this.GetExportsCore(typeof(T), null, contractName, ImportCardinality.ZeroOrMore);
			Collection<Lazy<T>> collection = new Collection<Lazy<T>>();
			foreach (Export export in exportsCore)
			{
				collection.Add(ExportServices.CreateStronglyTypedLazyOfT<T>(export));
			}
			return collection;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00011C3C File Offset: 0x0000FE3C
		private IEnumerable<Lazy<T, TMetadataView>> GetExportsCore<T, TMetadataView>(string contractName)
		{
			IEnumerable<Export> exportsCore = this.GetExportsCore(typeof(T), typeof(TMetadataView), contractName, ImportCardinality.ZeroOrMore);
			Collection<Lazy<T, TMetadataView>> collection = new Collection<Lazy<T, TMetadataView>>();
			foreach (Export export in exportsCore)
			{
				collection.Add(ExportServices.CreateStronglyTypedLazyOfTM<T, TMetadataView>(export));
			}
			return collection;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00011CAC File Offset: 0x0000FEAC
		private Lazy<T, TMetadataView> GetExportCore<T, TMetadataView>(string contractName)
		{
			Export export = this.GetExportsCore(typeof(T), typeof(TMetadataView), contractName, ImportCardinality.ExactlyOne).SingleOrDefault<Export>();
			if (export == null)
			{
				return null;
			}
			return ExportServices.CreateStronglyTypedLazyOfTM<T, TMetadataView>(export);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00011CE8 File Offset: 0x0000FEE8
		private Lazy<T> GetExportCore<T>(string contractName)
		{
			Export export = this.GetExportsCore(typeof(T), null, contractName, ImportCardinality.ExactlyOne).SingleOrDefault<Export>();
			if (export == null)
			{
				return null;
			}
			return ExportServices.CreateStronglyTypedLazyOfT<T>(export);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00011D1C File Offset: 0x0000FF1C
		private IEnumerable<Export> GetExportsCore(Type type, Type metadataViewType, string contractName, ImportCardinality cardinality)
		{
			Requires.NotNull<Type>(type, "type");
			if (string.IsNullOrEmpty(contractName))
			{
				contractName = AttributedModelServices.GetContractName(type);
			}
			if (metadataViewType == null)
			{
				metadataViewType = ExportServices.DefaultMetadataViewType;
			}
			if (!MetadataViewProvider.IsViewTypeValid(metadataViewType))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidMetadataView, metadataViewType.Name));
			}
			ImportDefinition definition = ExportProvider.BuildImportDefinition(type, metadataViewType, contractName, cardinality);
			return this.GetExports(definition, null);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00011D8C File Offset: 0x0000FF8C
		private static ImportDefinition BuildImportDefinition(Type type, Type metadataViewType, string contractName, ImportCardinality cardinality)
		{
			Assumes.NotNull<Type, Type, string>(type, metadataViewType, contractName);
			IEnumerable<KeyValuePair<string, Type>> requiredMetadata = CompositionServices.GetRequiredMetadata(metadataViewType);
			IDictionary<string, object> importMetadata = CompositionServices.GetImportMetadata(type, null);
			string requiredTypeIdentity = null;
			if (type != typeof(object))
			{
				requiredTypeIdentity = AttributedModelServices.GetTypeIdentity(type);
			}
			return new ContractBasedImportDefinition(contractName, requiredTypeIdentity, requiredMetadata, cardinality, false, true, CreationPolicy.Any, importMetadata);
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060005E4 RID: 1508 RVA: 0x00011DD8 File Offset: 0x0000FFD8
		// (remove) Token: 0x060005E5 RID: 1509 RVA: 0x00011E10 File Offset: 0x00010010
		public event EventHandler<ExportsChangeEventArgs> ExportsChanged;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060005E6 RID: 1510 RVA: 0x00011E48 File Offset: 0x00010048
		// (remove) Token: 0x060005E7 RID: 1511 RVA: 0x00011E80 File Offset: 0x00010080
		public event EventHandler<ExportsChangeEventArgs> ExportsChanging;

		// Token: 0x060005E8 RID: 1512 RVA: 0x00011EB5 File Offset: 0x000100B5
		public IEnumerable<Export> GetExports(ImportDefinition definition)
		{
			return this.GetExports(definition, null);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00011EC0 File Offset: 0x000100C0
		public IEnumerable<Export> GetExports(ImportDefinition definition, AtomicComposition atomicComposition)
		{
			Requires.NotNull<ImportDefinition>(definition, "definition");
			IEnumerable<Export> result;
			ExportCardinalityCheckResult exportCardinalityCheckResult = this.TryGetExportsCore(definition, atomicComposition, out result);
			if (exportCardinalityCheckResult == ExportCardinalityCheckResult.Match)
			{
				return result;
			}
			if (exportCardinalityCheckResult != ExportCardinalityCheckResult.NoExports)
			{
				Assumes.IsTrue(exportCardinalityCheckResult == ExportCardinalityCheckResult.TooManyExports);
				throw new ImportCardinalityMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.CardinalityMismatch_TooManyExports, definition.ToString()));
			}
			throw new ImportCardinalityMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.CardinalityMismatch_NoExports, definition.ToString()));
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00011F2C File Offset: 0x0001012C
		public bool TryGetExports(ImportDefinition definition, AtomicComposition atomicComposition, out IEnumerable<Export> exports)
		{
			Requires.NotNull<ImportDefinition>(definition, "definition");
			exports = null;
			return this.TryGetExportsCore(definition, atomicComposition, out exports) == ExportCardinalityCheckResult.Match;
		}

		// Token: 0x060005EB RID: 1515
		protected abstract IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition);

		// Token: 0x060005EC RID: 1516 RVA: 0x00011F48 File Offset: 0x00010148
		protected virtual void OnExportsChanged(ExportsChangeEventArgs e)
		{
			EventHandler<ExportsChangeEventArgs> exportsChanged = this.ExportsChanged;
			if (exportsChanged != null)
			{
				CompositionServices.TryFire<ExportsChangeEventArgs>(exportsChanged, this, e).ThrowOnErrors(e.AtomicComposition);
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00011F78 File Offset: 0x00010178
		protected virtual void OnExportsChanging(ExportsChangeEventArgs e)
		{
			EventHandler<ExportsChangeEventArgs> exportsChanging = this.ExportsChanging;
			if (exportsChanging != null)
			{
				CompositionServices.TryFire<ExportsChangeEventArgs>(exportsChanging, this, e).ThrowOnErrors(e.AtomicComposition);
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00011FA8 File Offset: 0x000101A8
		private ExportCardinalityCheckResult TryGetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition, out IEnumerable<Export> exports)
		{
			Assumes.NotNull<ImportDefinition>(definition);
			exports = this.GetExportsCore(definition, atomicComposition);
			ExportCardinalityCheckResult exportCardinalityCheckResult = ExportServices.CheckCardinality<Export>(definition, exports);
			if (exportCardinalityCheckResult == ExportCardinalityCheckResult.TooManyExports && definition.Cardinality == ImportCardinality.ZeroOrOne)
			{
				exportCardinalityCheckResult = ExportCardinalityCheckResult.Match;
				exports = null;
			}
			if (exports == null)
			{
				exports = ExportProvider.EmptyExports;
			}
			return exportCardinalityCheckResult;
		}

		// Token: 0x04000280 RID: 640
		private static readonly Export[] EmptyExports = new Export[0];
	}
}
