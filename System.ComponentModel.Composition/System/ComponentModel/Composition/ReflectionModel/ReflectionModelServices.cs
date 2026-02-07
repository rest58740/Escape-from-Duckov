using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel
{
	// Token: 0x0200007E RID: 126
	public static class ReflectionModelServices
	{
		// Token: 0x0600034E RID: 846 RVA: 0x0000A0D2 File Offset: 0x000082D2
		public static Lazy<Type> GetPartType(ComposablePartDefinition partDefinition)
		{
			Requires.NotNull<ComposablePartDefinition>(partDefinition, "partDefinition");
			ReflectionComposablePartDefinition reflectionComposablePartDefinition = partDefinition as ReflectionComposablePartDefinition;
			if (reflectionComposablePartDefinition == null)
			{
				throw ExceptionBuilder.CreateReflectionModelInvalidPartDefinition("partDefinition", partDefinition.GetType());
			}
			return reflectionComposablePartDefinition.GetLazyPartType();
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000A0FE File Offset: 0x000082FE
		public static bool IsDisposalRequired(ComposablePartDefinition partDefinition)
		{
			Requires.NotNull<ComposablePartDefinition>(partDefinition, "partDefinition");
			ReflectionComposablePartDefinition reflectionComposablePartDefinition = partDefinition as ReflectionComposablePartDefinition;
			if (reflectionComposablePartDefinition == null)
			{
				throw ExceptionBuilder.CreateReflectionModelInvalidPartDefinition("partDefinition", partDefinition.GetType());
			}
			return reflectionComposablePartDefinition.IsDisposalRequired;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000A12A File Offset: 0x0000832A
		public static LazyMemberInfo GetExportingMember(ExportDefinition exportDefinition)
		{
			Requires.NotNull<ExportDefinition>(exportDefinition, "exportDefinition");
			ReflectionMemberExportDefinition reflectionMemberExportDefinition = exportDefinition as ReflectionMemberExportDefinition;
			if (reflectionMemberExportDefinition == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidExportDefinition, exportDefinition.GetType()), "exportDefinition");
			}
			return reflectionMemberExportDefinition.ExportingLazyMember;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000A165 File Offset: 0x00008365
		public static LazyMemberInfo GetImportingMember(ImportDefinition importDefinition)
		{
			Requires.NotNull<ImportDefinition>(importDefinition, "importDefinition");
			ReflectionMemberImportDefinition reflectionMemberImportDefinition = importDefinition as ReflectionMemberImportDefinition;
			if (reflectionMemberImportDefinition == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidMemberImportDefinition, importDefinition.GetType()), "importDefinition");
			}
			return reflectionMemberImportDefinition.ImportingLazyMember;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000A1A0 File Offset: 0x000083A0
		public static Lazy<ParameterInfo> GetImportingParameter(ImportDefinition importDefinition)
		{
			Requires.NotNull<ImportDefinition>(importDefinition, "importDefinition");
			ReflectionParameterImportDefinition reflectionParameterImportDefinition = importDefinition as ReflectionParameterImportDefinition;
			if (reflectionParameterImportDefinition == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidParameterImportDefinition, importDefinition.GetType()), "importDefinition");
			}
			return reflectionParameterImportDefinition.ImportingLazyParameter;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000A1DB File Offset: 0x000083DB
		public static bool IsImportingParameter(ImportDefinition importDefinition)
		{
			Requires.NotNull<ImportDefinition>(importDefinition, "importDefinition");
			if (!(importDefinition is ReflectionImportDefinition))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidImportDefinition, importDefinition.GetType()), "importDefinition");
			}
			return importDefinition is ReflectionParameterImportDefinition;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000A219 File Offset: 0x00008419
		public static bool IsExportFactoryImportDefinition(ImportDefinition importDefinition)
		{
			Requires.NotNull<ImportDefinition>(importDefinition, "importDefinition");
			return importDefinition is IPartCreatorImportDefinition;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000A22F File Offset: 0x0000842F
		public static ContractBasedImportDefinition GetExportFactoryProductImportDefinition(ImportDefinition importDefinition)
		{
			Requires.NotNull<ImportDefinition>(importDefinition, "importDefinition");
			IPartCreatorImportDefinition partCreatorImportDefinition = importDefinition as IPartCreatorImportDefinition;
			if (partCreatorImportDefinition == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidImportDefinition, importDefinition.GetType()), "importDefinition");
			}
			return partCreatorImportDefinition.ProductImportDefinition;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000A26A File Offset: 0x0000846A
		public static ComposablePartDefinition CreatePartDefinition(Lazy<Type> partType, bool isDisposalRequired, Lazy<IEnumerable<ImportDefinition>> imports, Lazy<IEnumerable<ExportDefinition>> exports, Lazy<IDictionary<string, object>> metadata, ICompositionElement origin)
		{
			Requires.NotNull<Lazy<Type>>(partType, "partType");
			return new ReflectionComposablePartDefinition(new ReflectionPartCreationInfo(partType, isDisposalRequired, imports, exports, metadata, origin));
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000A289 File Offset: 0x00008489
		public static ExportDefinition CreateExportDefinition(LazyMemberInfo exportingMember, string contractName, Lazy<IDictionary<string, object>> metadata, ICompositionElement origin)
		{
			Requires.NotNullOrEmpty(contractName, "contractName");
			Requires.IsInMembertypeSet(exportingMember.MemberType, "exportingMember", 188);
			return new ReflectionMemberExportDefinition(exportingMember, new LazyExportDefinition(contractName, metadata), origin);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000A2BC File Offset: 0x000084BC
		public static ContractBasedImportDefinition CreateImportDefinition(LazyMemberInfo importingMember, string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, bool isRecomposable, CreationPolicy requiredCreationPolicy, ICompositionElement origin)
		{
			return ReflectionModelServices.CreateImportDefinition(importingMember, contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable, requiredCreationPolicy, MetadataServices.EmptyMetadata, false, origin);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000A2E0 File Offset: 0x000084E0
		public static ContractBasedImportDefinition CreateImportDefinition(LazyMemberInfo importingMember, string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, bool isRecomposable, CreationPolicy requiredCreationPolicy, IDictionary<string, object> metadata, bool isExportFactory, ICompositionElement origin)
		{
			return ReflectionModelServices.CreateImportDefinition(importingMember, contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable, false, requiredCreationPolicy, metadata, isExportFactory, origin);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000A304 File Offset: 0x00008504
		public static ContractBasedImportDefinition CreateImportDefinition(LazyMemberInfo importingMember, string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPreRequisite, CreationPolicy requiredCreationPolicy, IDictionary<string, object> metadata, bool isExportFactory, ICompositionElement origin)
		{
			Requires.NotNullOrEmpty(contractName, "contractName");
			Requires.IsInMembertypeSet(importingMember.MemberType, "importingMember", 20);
			if (isExportFactory)
			{
				return new PartCreatorMemberImportDefinition(importingMember, origin, new ContractBasedImportDefinition(contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable, isPreRequisite, CreationPolicy.NonShared, metadata));
			}
			return new ReflectionMemberImportDefinition(importingMember, contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable, isPreRequisite, requiredCreationPolicy, metadata, origin);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000A364 File Offset: 0x00008564
		public static ContractBasedImportDefinition CreateImportDefinition(Lazy<ParameterInfo> parameter, string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, CreationPolicy requiredCreationPolicy, ICompositionElement origin)
		{
			return ReflectionModelServices.CreateImportDefinition(parameter, contractName, requiredTypeIdentity, requiredMetadata, cardinality, requiredCreationPolicy, MetadataServices.EmptyMetadata, false, origin);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000A388 File Offset: 0x00008588
		public static ContractBasedImportDefinition CreateImportDefinition(Lazy<ParameterInfo> parameter, string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, CreationPolicy requiredCreationPolicy, IDictionary<string, object> metadata, bool isExportFactory, ICompositionElement origin)
		{
			Requires.NotNull<Lazy<ParameterInfo>>(parameter, "parameter");
			Requires.NotNullOrEmpty(contractName, "contractName");
			if (isExportFactory)
			{
				return new PartCreatorParameterImportDefinition(parameter, origin, new ContractBasedImportDefinition(contractName, requiredTypeIdentity, requiredMetadata, cardinality, false, true, CreationPolicy.NonShared, metadata));
			}
			return new ReflectionParameterImportDefinition(parameter, contractName, requiredTypeIdentity, requiredMetadata, cardinality, requiredCreationPolicy, metadata, origin);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000A3D8 File Offset: 0x000085D8
		public static bool TryMakeGenericPartDefinition(ComposablePartDefinition partDefinition, IEnumerable<Type> genericParameters, out ComposablePartDefinition specialization)
		{
			Requires.NotNull<ComposablePartDefinition>(partDefinition, "partDefinition");
			specialization = null;
			ReflectionComposablePartDefinition reflectionComposablePartDefinition = partDefinition as ReflectionComposablePartDefinition;
			if (reflectionComposablePartDefinition == null)
			{
				throw ExceptionBuilder.CreateReflectionModelInvalidPartDefinition("partDefinition", partDefinition.GetType());
			}
			return reflectionComposablePartDefinition.TryMakeGenericPartDefinition(genericParameters.ToArray<Type>(), out specialization);
		}
	}
}
