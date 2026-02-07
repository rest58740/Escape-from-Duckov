using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.AttributedModel;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition
{
	// Token: 0x0200001F RID: 31
	public static class AttributedModelServices
	{
		// Token: 0x0600010E RID: 270 RVA: 0x00003F37 File Offset: 0x00002137
		public static TMetadataView GetMetadataView<TMetadataView>(IDictionary<string, object> metadata)
		{
			Requires.NotNull<IDictionary<string, object>>(metadata, "metadata");
			return MetadataViewProvider.GetMetadataView<TMetadataView>(metadata);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00003F4A File Offset: 0x0000214A
		public static ComposablePart CreatePart(object attributedPart)
		{
			Requires.NotNull<object>(attributedPart, "attributedPart");
			return AttributedModelDiscovery.CreatePart(attributedPart);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003F5D File Offset: 0x0000215D
		public static ComposablePart CreatePart(object attributedPart, ReflectionContext reflectionContext)
		{
			Requires.NotNull<object>(attributedPart, "attributedPart");
			Requires.NotNull<ReflectionContext>(reflectionContext, "reflectionContext");
			return AttributedModelDiscovery.CreatePart(attributedPart, reflectionContext);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00003F7C File Offset: 0x0000217C
		public static ComposablePart CreatePart(ComposablePartDefinition partDefinition, object attributedPart)
		{
			Requires.NotNull<ComposablePartDefinition>(partDefinition, "partDefinition");
			Requires.NotNull<object>(attributedPart, "attributedPart");
			ReflectionComposablePartDefinition reflectionComposablePartDefinition = partDefinition as ReflectionComposablePartDefinition;
			if (reflectionComposablePartDefinition == null)
			{
				throw ExceptionBuilder.CreateReflectionModelInvalidPartDefinition("partDefinition", partDefinition.GetType());
			}
			return AttributedModelDiscovery.CreatePart(reflectionComposablePartDefinition, attributedPart);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00003FB4 File Offset: 0x000021B4
		public static ComposablePartDefinition CreatePartDefinition(Type type, ICompositionElement origin)
		{
			Requires.NotNull<Type>(type, "type");
			return AttributedModelServices.CreatePartDefinition(type, origin, false);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00003FC9 File Offset: 0x000021C9
		public static ComposablePartDefinition CreatePartDefinition(Type type, ICompositionElement origin, bool ensureIsDiscoverable)
		{
			Requires.NotNull<Type>(type, "type");
			if (ensureIsDiscoverable)
			{
				return AttributedModelDiscovery.CreatePartDefinitionIfDiscoverable(type, origin);
			}
			return AttributedModelDiscovery.CreatePartDefinition(type, null, false, origin);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00003FEA File Offset: 0x000021EA
		public static string GetTypeIdentity(Type type)
		{
			Requires.NotNull<Type>(type, "type");
			return ContractNameServices.GetTypeIdentity(type);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00003FFD File Offset: 0x000021FD
		public static string GetTypeIdentity(MethodInfo method)
		{
			Requires.NotNull<MethodInfo>(method, "method");
			return ContractNameServices.GetTypeIdentityFromMethod(method);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004010 File Offset: 0x00002210
		public static string GetContractName(Type type)
		{
			Requires.NotNull<Type>(type, "type");
			return AttributedModelServices.GetTypeIdentity(type);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004024 File Offset: 0x00002224
		public static ComposablePart AddExportedValue<T>(this CompositionBatch batch, T exportedValue)
		{
			Requires.NotNull<CompositionBatch>(batch, "batch");
			string contractName = AttributedModelServices.GetContractName(typeof(T));
			return batch.AddExportedValue(contractName, exportedValue);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004054 File Offset: 0x00002254
		public static void ComposeExportedValue<T>(this CompositionContainer container, T exportedValue)
		{
			Requires.NotNull<CompositionContainer>(container, "container");
			CompositionBatch batch = new CompositionBatch();
			batch.AddExportedValue(exportedValue);
			container.Compose(batch);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004084 File Offset: 0x00002284
		public static ComposablePart AddExportedValue<T>(this CompositionBatch batch, string contractName, T exportedValue)
		{
			Requires.NotNull<CompositionBatch>(batch, "batch");
			string typeIdentity = AttributedModelServices.GetTypeIdentity(typeof(T));
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("ExportTypeIdentity", typeIdentity);
			return batch.AddExport(new Export(contractName, dictionary, () => exportedValue));
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000040E4 File Offset: 0x000022E4
		public static void ComposeExportedValue<T>(this CompositionContainer container, string contractName, T exportedValue)
		{
			Requires.NotNull<CompositionContainer>(container, "container");
			CompositionBatch batch = new CompositionBatch();
			batch.AddExportedValue(contractName, exportedValue);
			container.Compose(batch);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004114 File Offset: 0x00002314
		public static ComposablePart AddPart(this CompositionBatch batch, object attributedPart)
		{
			Requires.NotNull<CompositionBatch>(batch, "batch");
			Requires.NotNull<object>(attributedPart, "attributedPart");
			ComposablePart composablePart = AttributedModelServices.CreatePart(attributedPart);
			batch.AddPart(composablePart);
			return composablePart;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00004148 File Offset: 0x00002348
		public static void ComposeParts(this CompositionContainer container, params object[] attributedParts)
		{
			Requires.NotNull<CompositionContainer>(container, "container");
			Requires.NotNullOrNullElements<object>(attributedParts, "attributedParts");
			CompositionBatch batch = new CompositionBatch((from attributedPart in attributedParts
			select AttributedModelServices.CreatePart(attributedPart)).ToArray<ComposablePart>(), Enumerable.Empty<ComposablePart>());
			container.Compose(batch);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000041A8 File Offset: 0x000023A8
		public static ComposablePart SatisfyImportsOnce(this ICompositionService compositionService, object attributedPart)
		{
			Requires.NotNull<ICompositionService>(compositionService, "compositionService");
			Requires.NotNull<object>(attributedPart, "attributedPart");
			ComposablePart composablePart = AttributedModelServices.CreatePart(attributedPart);
			compositionService.SatisfyImportsOnce(composablePart);
			return composablePart;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000041DC File Offset: 0x000023DC
		public static ComposablePart SatisfyImportsOnce(this ICompositionService compositionService, object attributedPart, ReflectionContext reflectionContext)
		{
			Requires.NotNull<ICompositionService>(compositionService, "compositionService");
			Requires.NotNull<object>(attributedPart, "attributedPart");
			Requires.NotNull<ReflectionContext>(reflectionContext, "reflectionContext");
			ComposablePart composablePart = AttributedModelServices.CreatePart(attributedPart, reflectionContext);
			compositionService.SatisfyImportsOnce(composablePart);
			return composablePart;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000421A File Offset: 0x0000241A
		public static bool Exports(this ComposablePartDefinition part, Type contractType)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			Requires.NotNull<Type>(contractType, "contractType");
			return part.Exports(AttributedModelServices.GetContractName(contractType));
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000423E File Offset: 0x0000243E
		public static bool Exports<T>(this ComposablePartDefinition part)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			return part.Exports(typeof(T));
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000425B File Offset: 0x0000245B
		public static bool Imports(this ComposablePartDefinition part, Type contractType)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			Requires.NotNull<Type>(contractType, "contractType");
			return part.Imports(AttributedModelServices.GetContractName(contractType));
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000427F File Offset: 0x0000247F
		public static bool Imports<T>(this ComposablePartDefinition part)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			return part.Imports(typeof(T));
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000429C File Offset: 0x0000249C
		public static bool Imports(this ComposablePartDefinition part, Type contractType, ImportCardinality importCardinality)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			Requires.NotNull<Type>(contractType, "contractType");
			return part.Imports(AttributedModelServices.GetContractName(contractType), importCardinality);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000042C1 File Offset: 0x000024C1
		public static bool Imports<T>(this ComposablePartDefinition part, ImportCardinality importCardinality)
		{
			Requires.NotNull<ComposablePartDefinition>(part, "part");
			return part.Imports(typeof(T), importCardinality);
		}
	}
}
