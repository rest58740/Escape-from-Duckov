using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000B71 RID: 2929
	[ComVisible(true)]
	public abstract class IsolatedStorage : MarshalByRefObject
	{
		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06006A79 RID: 27257 RVA: 0x0016C82C File Offset: 0x0016AA2C
		[ComVisible(false)]
		[MonoTODO("Does not currently use the manifest support")]
		public object ApplicationIdentity
		{
			[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
			get
			{
				if ((this.storage_scope & IsolatedStorageScope.Application) == IsolatedStorageScope.None)
				{
					throw new InvalidOperationException(Locale.GetText("Invalid Isolation Scope."));
				}
				if (this._applicationIdentity == null)
				{
					throw new InvalidOperationException(Locale.GetText("Identity unavailable."));
				}
				throw new NotImplementedException(Locale.GetText("CAS related"));
			}
		}

		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06006A7A RID: 27258 RVA: 0x0016C87B File Offset: 0x0016AA7B
		public object AssemblyIdentity
		{
			[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
			get
			{
				if ((this.storage_scope & IsolatedStorageScope.Assembly) == IsolatedStorageScope.None)
				{
					throw new InvalidOperationException(Locale.GetText("Invalid Isolation Scope."));
				}
				if (this._assemblyIdentity == null)
				{
					throw new InvalidOperationException(Locale.GetText("Identity unavailable."));
				}
				return this._assemblyIdentity;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06006A7B RID: 27259 RVA: 0x0016C8B5 File Offset: 0x0016AAB5
		[Obsolete]
		[CLSCompliant(false)]
		public virtual ulong CurrentSize
		{
			get
			{
				throw new InvalidOperationException(Locale.GetText("IsolatedStorage does not have a preset CurrentSize."));
			}
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06006A7C RID: 27260 RVA: 0x0016C8C6 File Offset: 0x0016AAC6
		public object DomainIdentity
		{
			[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
			get
			{
				if ((this.storage_scope & IsolatedStorageScope.Domain) == IsolatedStorageScope.None)
				{
					throw new InvalidOperationException(Locale.GetText("Invalid Isolation Scope."));
				}
				if (this._domainIdentity == null)
				{
					throw new InvalidOperationException(Locale.GetText("Identity unavailable."));
				}
				return this._domainIdentity;
			}
		}

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06006A7D RID: 27261 RVA: 0x0016C900 File Offset: 0x0016AB00
		[Obsolete]
		[CLSCompliant(false)]
		public virtual ulong MaximumSize
		{
			get
			{
				throw new InvalidOperationException(Locale.GetText("IsolatedStorage does not have a preset MaximumSize."));
			}
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06006A7E RID: 27262 RVA: 0x0016C911 File Offset: 0x0016AB11
		public IsolatedStorageScope Scope
		{
			get
			{
				return this.storage_scope;
			}
		}

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06006A7F RID: 27263 RVA: 0x0016C919 File Offset: 0x0016AB19
		[ComVisible(false)]
		public virtual long AvailableFreeSpace
		{
			get
			{
				throw new InvalidOperationException("This property is not defined for this store.");
			}
		}

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06006A80 RID: 27264 RVA: 0x0016C919 File Offset: 0x0016AB19
		[ComVisible(false)]
		public virtual long Quota
		{
			get
			{
				throw new InvalidOperationException("This property is not defined for this store.");
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06006A81 RID: 27265 RVA: 0x0016C919 File Offset: 0x0016AB19
		[ComVisible(false)]
		public virtual long UsedSize
		{
			get
			{
				throw new InvalidOperationException("This property is not defined for this store.");
			}
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06006A82 RID: 27266 RVA: 0x0016C925 File Offset: 0x0016AB25
		protected virtual char SeparatorExternal
		{
			get
			{
				return Path.DirectorySeparatorChar;
			}
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06006A83 RID: 27267 RVA: 0x0016C92C File Offset: 0x0016AB2C
		protected virtual char SeparatorInternal
		{
			get
			{
				return '.';
			}
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x0000AF5E File Offset: 0x0000915E
		protected virtual IsolatedStoragePermission GetPermission(PermissionSet ps)
		{
			return null;
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x0016C930 File Offset: 0x0016AB30
		protected void InitStore(IsolatedStorageScope scope, Type domainEvidenceType, Type assemblyEvidenceType)
		{
			if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Assembly) || scope == (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly))
			{
				throw new NotImplementedException(scope.ToString());
			}
			throw new ArgumentException(scope.ToString());
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x0016C95F File Offset: 0x0016AB5F
		[MonoTODO("requires manifest support")]
		protected void InitStore(IsolatedStorageScope scope, Type appEvidenceType)
		{
			if (AppDomain.CurrentDomain.ApplicationIdentity == null)
			{
				throw new IsolatedStorageException(Locale.GetText("No ApplicationIdentity available for AppDomain."));
			}
			appEvidenceType == null;
			this.storage_scope = scope;
		}

		// Token: 0x06006A87 RID: 27271
		public abstract void Remove();

		// Token: 0x06006A88 RID: 27272 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[ComVisible(false)]
		public virtual bool IncreaseQuotaTo(long newQuotaSize)
		{
			return false;
		}

		// Token: 0x04003D95 RID: 15765
		internal IsolatedStorageScope storage_scope;

		// Token: 0x04003D96 RID: 15766
		internal object _assemblyIdentity;

		// Token: 0x04003D97 RID: 15767
		internal object _domainIdentity;

		// Token: 0x04003D98 RID: 15768
		internal object _applicationIdentity;
	}
}
