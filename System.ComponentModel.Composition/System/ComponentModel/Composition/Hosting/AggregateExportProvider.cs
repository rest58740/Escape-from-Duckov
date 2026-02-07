using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting
{
	// Token: 0x0200009F RID: 159
	public class AggregateExportProvider : ExportProvider, IDisposable
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x0000BB28 File Offset: 0x00009D28
		public AggregateExportProvider(params ExportProvider[] providers)
		{
			ExportProvider[] array;
			if (providers != null)
			{
				array = new ExportProvider[providers.Length];
				for (int i = 0; i < providers.Length; i++)
				{
					ExportProvider exportProvider = providers[i];
					if (exportProvider == null)
					{
						throw ExceptionBuilder.CreateContainsNullElement("providers");
					}
					array[i] = exportProvider;
					exportProvider.ExportsChanged += new EventHandler<ExportsChangeEventArgs>(this.OnExportChangedInternal);
					exportProvider.ExportsChanging += new EventHandler<ExportsChangeEventArgs>(this.OnExportChangingInternal);
				}
			}
			else
			{
				array = new ExportProvider[0];
			}
			this._providers = array;
			this._readOnlyProviders = new ReadOnlyCollection<ExportProvider>(this._providers);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000BBB2 File Offset: 0x00009DB2
		public AggregateExportProvider(IEnumerable<ExportProvider> providers) : this((providers != null) ? providers.AsArray<ExportProvider>() : null)
		{
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000BBC6 File Offset: 0x00009DC6
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000BBD8 File Offset: 0x00009DD8
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && Interlocked.CompareExchange(ref this._isDisposed, 1, 0) == 0)
			{
				foreach (ExportProvider exportProvider in this._providers)
				{
					exportProvider.ExportsChanged -= new EventHandler<ExportsChangeEventArgs>(this.OnExportChangedInternal);
					exportProvider.ExportsChanging -= new EventHandler<ExportsChangeEventArgs>(this.OnExportChangingInternal);
				}
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0000BC32 File Offset: 0x00009E32
		public ReadOnlyCollection<ExportProvider> Providers
		{
			get
			{
				this.ThrowIfDisposed();
				return this._readOnlyProviders;
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000BC40 File Offset: 0x00009E40
		protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
		{
			this.ThrowIfDisposed();
			ExportProvider[] providers;
			if (definition.Cardinality == ImportCardinality.ZeroOrMore)
			{
				List<Export> list = new List<Export>();
				providers = this._providers;
				for (int i = 0; i < providers.Length; i++)
				{
					foreach (Export export in providers[i].GetExports(definition, atomicComposition))
					{
						list.Add(export);
					}
				}
				return list;
			}
			IEnumerable<Export> enumerable = null;
			providers = this._providers;
			for (int i = 0; i < providers.Length; i++)
			{
				IEnumerable<Export> enumerable2;
				bool flag = providers[i].TryGetExports(definition, atomicComposition, out enumerable2);
				bool flag2 = enumerable2.FastAny<Export>();
				if (flag && flag2)
				{
					return enumerable2;
				}
				if (flag2)
				{
					enumerable = ((enumerable != null) ? enumerable.Concat(enumerable2) : enumerable2);
				}
			}
			return enumerable;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000BD10 File Offset: 0x00009F10
		private void OnExportChangedInternal(object sender, ExportsChangeEventArgs e)
		{
			this.OnExportsChanged(e);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000BD19 File Offset: 0x00009F19
		private void OnExportChangingInternal(object sender, ExportsChangeEventArgs e)
		{
			this.OnExportsChanging(e);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000BD22 File Offset: 0x00009F22
		[DebuggerStepThrough]
		private void ThrowIfDisposed()
		{
			if (this._isDisposed == 1)
			{
				throw ExceptionBuilder.CreateObjectDisposed(this);
			}
		}

		// Token: 0x040001A3 RID: 419
		private readonly ReadOnlyCollection<ExportProvider> _readOnlyProviders;

		// Token: 0x040001A4 RID: 420
		private readonly ExportProvider[] _providers;

		// Token: 0x040001A5 RID: 421
		private volatile int _isDisposed;
	}
}
