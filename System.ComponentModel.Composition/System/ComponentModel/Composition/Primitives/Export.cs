using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives
{
	// Token: 0x02000093 RID: 147
	public class Export
	{
		// Token: 0x060003E3 RID: 995 RVA: 0x0000B284 File Offset: 0x00009484
		protected Export()
		{
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000B299 File Offset: 0x00009499
		public Export(string contractName, Func<object> exportedValueGetter) : this(new ExportDefinition(contractName, null), exportedValueGetter)
		{
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000B2A9 File Offset: 0x000094A9
		public Export(string contractName, IDictionary<string, object> metadata, Func<object> exportedValueGetter) : this(new ExportDefinition(contractName, metadata), exportedValueGetter)
		{
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000B2B9 File Offset: 0x000094B9
		public Export(ExportDefinition definition, Func<object> exportedValueGetter)
		{
			Requires.NotNull<ExportDefinition>(definition, "definition");
			Requires.NotNull<Func<object>>(exportedValueGetter, "exportedValueGetter");
			this._definition = definition;
			this._exportedValueGetter = exportedValueGetter;
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000B2F2 File Offset: 0x000094F2
		public virtual ExportDefinition Definition
		{
			get
			{
				if (this._definition != null)
				{
					return this._definition;
				}
				throw ExceptionBuilder.CreateNotOverriddenByDerived("Definition");
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000B30D File Offset: 0x0000950D
		public IDictionary<string, object> Metadata
		{
			get
			{
				return this.Definition.Metadata;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000B31C File Offset: 0x0000951C
		public object Value
		{
			get
			{
				if (this._exportedValue == Export._EmptyValue)
				{
					object exportedValueCore = this.GetExportedValueCore();
					Interlocked.CompareExchange(ref this._exportedValue, exportedValueCore, Export._EmptyValue);
				}
				return this._exportedValue;
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000B359 File Offset: 0x00009559
		protected virtual object GetExportedValueCore()
		{
			if (this._exportedValueGetter != null)
			{
				return this._exportedValueGetter.Invoke();
			}
			throw ExceptionBuilder.CreateNotOverriddenByDerived("GetExportedValueCore");
		}

		// Token: 0x04000181 RID: 385
		private readonly ExportDefinition _definition;

		// Token: 0x04000182 RID: 386
		private readonly Func<object> _exportedValueGetter;

		// Token: 0x04000183 RID: 387
		private static readonly object _EmptyValue = new object();

		// Token: 0x04000184 RID: 388
		private volatile object _exportedValue = Export._EmptyValue;
	}
}
