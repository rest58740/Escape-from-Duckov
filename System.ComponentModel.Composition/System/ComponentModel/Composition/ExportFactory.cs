using System;
using System.ComponentModel.Composition.Primitives;

namespace System.ComponentModel.Composition
{
	// Token: 0x02000037 RID: 55
	public class ExportFactory<T>
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x000058B5 File Offset: 0x00003AB5
		public ExportFactory(Func<Tuple<T, Action>> exportLifetimeContextCreator)
		{
			if (exportLifetimeContextCreator == null)
			{
				throw new ArgumentNullException("exportLifetimeContextCreator");
			}
			this._exportLifetimeContextCreator = exportLifetimeContextCreator;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000058D4 File Offset: 0x00003AD4
		public ExportLifetimeContext<T> CreateExport()
		{
			Tuple<T, Action> tuple = this._exportLifetimeContextCreator.Invoke();
			return new ExportLifetimeContext<T>(tuple.Item1, tuple.Item2);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000058FE File Offset: 0x00003AFE
		internal bool IncludeInScopedCatalog(ComposablePartDefinition composablePartDefinition)
		{
			return this.OnFilterScopedCatalog(composablePartDefinition);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00005907 File Offset: 0x00003B07
		protected virtual bool OnFilterScopedCatalog(ComposablePartDefinition composablePartDefinition)
		{
			return true;
		}

		// Token: 0x040000B4 RID: 180
		private Func<Tuple<T, Action>> _exportLifetimeContextCreator;
	}
}
