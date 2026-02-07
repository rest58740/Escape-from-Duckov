using System;
using System.ComponentModel.Composition.Primitives;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting
{
	// Token: 0x020000F8 RID: 248
	public static class ScopingExtensions
	{
		// Token: 0x06000686 RID: 1670 RVA: 0x0001423C File Offset: 0x0001243C
		public static bool Exports(this ComposablePartDefinition part, string contractName)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			Requires.NotNull<string>(contractName, "contractName");
			foreach (ExportDefinition exportDefinition in part.ExportDefinitions)
			{
				if (StringComparers.ContractName.Equals(contractName, exportDefinition.ContractName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x000142B4 File Offset: 0x000124B4
		public static bool Imports(this ComposablePartDefinition part, string contractName)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			Requires.NotNull<string>(contractName, "contractName");
			foreach (ImportDefinition importDefinition in part.ImportDefinitions)
			{
				if (StringComparers.ContractName.Equals(contractName, importDefinition.ContractName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001432C File Offset: 0x0001252C
		public static bool Imports(this ComposablePartDefinition part, string contractName, ImportCardinality importCardinality)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			Requires.NotNull<string>(contractName, "contractName");
			foreach (ImportDefinition importDefinition in part.ImportDefinitions)
			{
				if (StringComparers.ContractName.Equals(contractName, importDefinition.ContractName) && importDefinition.Cardinality == importCardinality)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000143AC File Offset: 0x000125AC
		public static bool ContainsPartMetadataWithKey(this ComposablePartDefinition part, string key)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			Requires.NotNull<string>(key, "key");
			return part.Metadata.ContainsKey(key);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x000143D0 File Offset: 0x000125D0
		public static bool ContainsPartMetadata<T>(this ComposablePartDefinition part, string key, T value)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			Requires.NotNull<string>(key, "key");
			object obj = null;
			if (!part.Metadata.TryGetValue(key, ref obj))
			{
				return false;
			}
			if (value == null)
			{
				return obj == null;
			}
			return value.Equals(obj);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00014422 File Offset: 0x00012622
		public static FilteredCatalog Filter(this ComposablePartCatalog catalog, Func<ComposablePartDefinition, bool> filter)
		{
			Requires.NotNull<ComposablePartCatalog>(catalog, "catalog");
			Requires.NotNull<Func<ComposablePartDefinition, bool>>(filter, "filter");
			return new FilteredCatalog(catalog, filter);
		}
	}
}
