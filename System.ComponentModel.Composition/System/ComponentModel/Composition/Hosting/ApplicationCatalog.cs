using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting
{
	// Token: 0x020000A0 RID: 160
	public class ApplicationCatalog : ComposablePartCatalog, ICompositionElement
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x0000BD36 File Offset: 0x00009F36
		public ApplicationCatalog()
		{
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000BD49 File Offset: 0x00009F49
		public ApplicationCatalog(ICompositionElement definitionOrigin)
		{
			Requires.NotNull<ICompositionElement>(definitionOrigin, "definitionOrigin");
			this._definitionOrigin = definitionOrigin;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000BD6E File Offset: 0x00009F6E
		public ApplicationCatalog(ReflectionContext reflectionContext)
		{
			Requires.NotNull<ReflectionContext>(reflectionContext, "reflectionContext");
			this._reflectionContext = reflectionContext;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000BD93 File Offset: 0x00009F93
		public ApplicationCatalog(ReflectionContext reflectionContext, ICompositionElement definitionOrigin)
		{
			Requires.NotNull<ReflectionContext>(reflectionContext, "reflectionContext");
			Requires.NotNull<ICompositionElement>(definitionOrigin, "definitionOrigin");
			this._reflectionContext = reflectionContext;
			this._definitionOrigin = definitionOrigin;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000BDCC File Offset: 0x00009FCC
		internal ComposablePartCatalog CreateCatalog(string location, string pattern)
		{
			if (this._reflectionContext != null)
			{
				if (this._definitionOrigin == null)
				{
					return new DirectoryCatalog(location, pattern, this._reflectionContext);
				}
				return new DirectoryCatalog(location, pattern, this._reflectionContext, this._definitionOrigin);
			}
			else
			{
				if (this._definitionOrigin == null)
				{
					return new DirectoryCatalog(location, pattern);
				}
				return new DirectoryCatalog(location, pattern, this._definitionOrigin);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000BE28 File Offset: 0x0000A028
		private AggregateCatalog InnerCatalog
		{
			get
			{
				if (this._innerCatalog == null)
				{
					object thisLock = this._thisLock;
					lock (thisLock)
					{
						if (this._innerCatalog == null)
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							Assumes.NotNull<string>(baseDirectory);
							List<ComposablePartCatalog> list = new List<ComposablePartCatalog>();
							list.Add(this.CreateCatalog(baseDirectory, "*.exe"));
							list.Add(this.CreateCatalog(baseDirectory, "*.dll"));
							string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
							if (!string.IsNullOrEmpty(relativeSearchPath))
							{
								foreach (string text in relativeSearchPath.Split(new char[]
								{
									';'
								}, 1))
								{
									string text2 = Path.Combine(baseDirectory, text);
									if (Directory.Exists(text2))
									{
										list.Add(this.CreateCatalog(text2, "*.dll"));
									}
								}
							}
							AggregateCatalog innerCatalog = new AggregateCatalog(list);
							this._innerCatalog = innerCatalog;
						}
					}
				}
				return this._innerCatalog;
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000BF3C File Offset: 0x0000A13C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._isDisposed)
				{
					IDisposable disposable = null;
					object thisLock = this._thisLock;
					lock (thisLock)
					{
						disposable = this._innerCatalog;
						this._innerCatalog = null;
						this._isDisposed = true;
					}
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000BFB8 File Offset: 0x0000A1B8
		public override IEnumerator<ComposablePartDefinition> GetEnumerator()
		{
			this.ThrowIfDisposed();
			return this.InnerCatalog.GetEnumerator();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000BFCB File Offset: 0x0000A1CB
		public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
		{
			this.ThrowIfDisposed();
			Requires.NotNull<ImportDefinition>(definition, "definition");
			return this.InnerCatalog.GetExports(definition);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000BFEA File Offset: 0x0000A1EA
		[DebuggerStepThrough]
		private void ThrowIfDisposed()
		{
			if (this._isDisposed)
			{
				throw ExceptionBuilder.CreateObjectDisposed(this);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000BFFB File Offset: 0x0000A1FB
		private string GetDisplayName()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} (Path=\"{1}\") (PrivateProbingPath=\"{2}\")", base.GetType().Name, AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000C02B File Offset: 0x0000A22B
		public override string ToString()
		{
			return this.GetDisplayName();
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000C02B File Offset: 0x0000A22B
		string ICompositionElement.DisplayName
		{
			get
			{
				return this.GetDisplayName();
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000AD5C File Offset: 0x00008F5C
		ICompositionElement ICompositionElement.Origin
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040001A6 RID: 422
		private bool _isDisposed;

		// Token: 0x040001A7 RID: 423
		private volatile AggregateCatalog _innerCatalog;

		// Token: 0x040001A8 RID: 424
		private readonly object _thisLock = new object();

		// Token: 0x040001A9 RID: 425
		private ICompositionElement _definitionOrigin;

		// Token: 0x040001AA RID: 426
		private ReflectionContext _reflectionContext;
	}
}
