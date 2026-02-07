using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Primitives
{
	// Token: 0x0200008A RID: 138
	[DebuggerTypeProxy(typeof(ComposablePartCatalogDebuggerProxy))]
	public abstract class ComposablePartCatalog : IEnumerable<ComposablePartDefinition>, IEnumerable, IDisposable
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000AB84 File Offset: 0x00008D84
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual IQueryable<ComposablePartDefinition> Parts
		{
			get
			{
				this.ThrowIfDisposed();
				if (this._queryableParts == null)
				{
					IQueryable<ComposablePartDefinition> queryable = this.AsQueryable<ComposablePartDefinition>();
					Interlocked.CompareExchange<IQueryable<ComposablePartDefinition>>(ref this._queryableParts, queryable, null);
					Assumes.NotNull<IQueryable<ComposablePartDefinition>>(this._queryableParts);
				}
				return this._queryableParts;
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000ABCC File Offset: 0x00008DCC
		public virtual IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
		{
			this.ThrowIfDisposed();
			Requires.NotNull<ImportDefinition>(definition, "definition");
			List<Tuple<ComposablePartDefinition, ExportDefinition>> list = null;
			IEnumerable<ComposablePartDefinition> candidateParts = this.GetCandidateParts(definition);
			if (candidateParts != null)
			{
				foreach (ComposablePartDefinition composablePartDefinition in candidateParts)
				{
					IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> exports = composablePartDefinition.GetExports(definition);
					if (exports != ComposablePartDefinition._EmptyExports)
					{
						list = list.FastAppendToListAllowNulls(exports);
					}
				}
			}
			return list ?? ComposablePartCatalog._EmptyExportsList;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000AC4C File Offset: 0x00008E4C
		internal virtual IEnumerable<ComposablePartDefinition> GetCandidateParts(ImportDefinition definition)
		{
			return this;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000AC4F File Offset: 0x00008E4F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000AC5E File Offset: 0x00008E5E
		protected virtual void Dispose(bool disposing)
		{
			this._isDisposed = true;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000AC67 File Offset: 0x00008E67
		[DebuggerStepThrough]
		private void ThrowIfDisposed()
		{
			if (this._isDisposed)
			{
				throw ExceptionBuilder.CreateObjectDisposed(this);
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000AC78 File Offset: 0x00008E78
		public virtual IEnumerator<ComposablePartDefinition> GetEnumerator()
		{
			IQueryable<ComposablePartDefinition> parts = this.Parts;
			if (parts == this._queryableParts)
			{
				return Enumerable.Empty<ComposablePartDefinition>().GetEnumerator();
			}
			return parts.GetEnumerator();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000ACA8 File Offset: 0x00008EA8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000172 RID: 370
		private bool _isDisposed;

		// Token: 0x04000173 RID: 371
		private volatile IQueryable<ComposablePartDefinition> _queryableParts;

		// Token: 0x04000174 RID: 372
		private static readonly List<Tuple<ComposablePartDefinition, ExportDefinition>> _EmptyExportsList = new List<Tuple<ComposablePartDefinition, ExportDefinition>>();
	}
}
