using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting
{
	// Token: 0x0200009D RID: 157
	public class AggregateCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x0000B942 File Offset: 0x00009B42
		public AggregateCatalog() : this(null)
		{
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000B94B File Offset: 0x00009B4B
		public AggregateCatalog(params ComposablePartCatalog[] catalogs) : this(catalogs)
		{
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000B954 File Offset: 0x00009B54
		public AggregateCatalog(IEnumerable<ComposablePartCatalog> catalogs)
		{
			Requires.NullOrNotNullElements<ComposablePartCatalog>(catalogs, "catalogs");
			this._catalogs = new ComposablePartCatalogCollection(catalogs, new Action<ComposablePartCatalogChangeEventArgs>(this.OnChanged), new Action<ComposablePartCatalogChangeEventArgs>(this.OnChanging));
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600041A RID: 1050 RVA: 0x0000B98D File Offset: 0x00009B8D
		// (remove) Token: 0x0600041B RID: 1051 RVA: 0x0000B99B File Offset: 0x00009B9B
		public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed
		{
			add
			{
				this._catalogs.Changed += value;
			}
			remove
			{
				this._catalogs.Changed -= value;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600041C RID: 1052 RVA: 0x0000B9A9 File Offset: 0x00009BA9
		// (remove) Token: 0x0600041D RID: 1053 RVA: 0x0000B9B7 File Offset: 0x00009BB7
		public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing
		{
			add
			{
				this._catalogs.Changing += value;
			}
			remove
			{
				this._catalogs.Changing -= value;
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
		{
			this.ThrowIfDisposed();
			Requires.NotNull<ImportDefinition>(definition, "definition");
			List<Tuple<ComposablePartDefinition, ExportDefinition>> list = new List<Tuple<ComposablePartDefinition, ExportDefinition>>();
			foreach (ComposablePartCatalog composablePartCatalog in this._catalogs)
			{
				foreach (Tuple<ComposablePartDefinition, ExportDefinition> tuple in composablePartCatalog.GetExports(definition))
				{
					list.Add(tuple);
				}
			}
			return list;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000BA64 File Offset: 0x00009C64
		public ICollection<ComposablePartCatalog> Catalogs
		{
			get
			{
				this.ThrowIfDisposed();
				return this._catalogs;
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000BA74 File Offset: 0x00009C74
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && Interlocked.CompareExchange(ref this._isDisposed, 1, 0) == 0)
				{
					this._catalogs.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000BAB8 File Offset: 0x00009CB8
		public override IEnumerator<ComposablePartDefinition> GetEnumerator()
		{
			return this._catalogs.SelectMany((ComposablePartCatalog catalog) => catalog).GetEnumerator();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000BAE9 File Offset: 0x00009CE9
		protected virtual void OnChanged(ComposablePartCatalogChangeEventArgs e)
		{
			this._catalogs.OnChanged(this, e);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000BAF8 File Offset: 0x00009CF8
		protected virtual void OnChanging(ComposablePartCatalogChangeEventArgs e)
		{
			this._catalogs.OnChanging(this, e);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000BB07 File Offset: 0x00009D07
		[DebuggerStepThrough]
		private void ThrowIfDisposed()
		{
			if (this._isDisposed == 1)
			{
				throw ExceptionBuilder.CreateObjectDisposed(this);
			}
		}

		// Token: 0x0400019F RID: 415
		private ComposablePartCatalogCollection _catalogs;

		// Token: 0x040001A0 RID: 416
		private volatile int _isDisposed;
	}
}
