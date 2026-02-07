using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using Mono.Security.Cryptography;
using Unity;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000B73 RID: 2931
	[ComVisible(true)]
	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	public sealed class IsolatedStorageFile : IsolatedStorage, IDisposable
	{
		// Token: 0x06006A8D RID: 27277 RVA: 0x0016C99E File Offset: 0x0016AB9E
		public static IEnumerator GetEnumerator(IsolatedStorageScope scope)
		{
			IsolatedStorageFile.Demand(scope);
			if (scope != IsolatedStorageScope.User && scope != (IsolatedStorageScope.User | IsolatedStorageScope.Roaming) && scope != IsolatedStorageScope.Machine)
			{
				throw new ArgumentException(Locale.GetText("Invalid scope, only User, User|Roaming and Machine are valid"));
			}
			return new IsolatedStorageFileEnumerator(scope, IsolatedStorageFile.GetIsolatedStorageRoot(scope));
		}

		// Token: 0x06006A8E RID: 27278 RVA: 0x0016C9D0 File Offset: 0x0016ABD0
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Evidence domainEvidence, Type domainEvidenceType, Evidence assemblyEvidence, Type assemblyEvidenceType)
		{
			IsolatedStorageFile.Demand(scope);
			bool flag = (scope & IsolatedStorageScope.Domain) > IsolatedStorageScope.None;
			if (flag && domainEvidence == null)
			{
				throw new ArgumentNullException("domainEvidence");
			}
			bool flag2 = (scope & IsolatedStorageScope.Assembly) > IsolatedStorageScope.None;
			if (flag2 && assemblyEvidence == null)
			{
				throw new ArgumentNullException("assemblyEvidence");
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			if (flag)
			{
				if (domainEvidenceType == null)
				{
					isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetDomainIdentityFromEvidence(domainEvidence);
				}
				else
				{
					isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetTypeFromEvidence(domainEvidence, domainEvidenceType);
				}
				if (isolatedStorageFile._domainIdentity == null)
				{
					throw new IsolatedStorageException(Locale.GetText("Couldn't find domain identity."));
				}
			}
			if (flag2)
			{
				if (assemblyEvidenceType == null)
				{
					isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(assemblyEvidence);
				}
				else
				{
					isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetTypeFromEvidence(assemblyEvidence, assemblyEvidenceType);
				}
				if (isolatedStorageFile._assemblyIdentity == null)
				{
					throw new IsolatedStorageException(Locale.GetText("Couldn't find assembly identity."));
				}
			}
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A8F RID: 27279 RVA: 0x0016CAA0 File Offset: 0x0016ACA0
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object domainIdentity, object assemblyIdentity)
		{
			IsolatedStorageFile.Demand(scope);
			if ((scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None && domainIdentity == null)
			{
				throw new ArgumentNullException("domainIdentity");
			}
			bool flag = (scope & IsolatedStorageScope.Assembly) > IsolatedStorageScope.None;
			if (flag && assemblyIdentity == null)
			{
				throw new ArgumentNullException("assemblyIdentity");
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			if (flag)
			{
				isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			}
			isolatedStorageFile._domainIdentity = domainIdentity;
			isolatedStorageFile._assemblyIdentity = assemblyIdentity;
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A90 RID: 27280 RVA: 0x0016CB0C File Offset: 0x0016AD0C
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type domainEvidenceType, Type assemblyEvidenceType)
		{
			IsolatedStorageFile.Demand(scope);
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			if ((scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None)
			{
				if (domainEvidenceType == null)
				{
					domainEvidenceType = typeof(Url);
				}
				isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetTypeFromEvidence(AppDomain.CurrentDomain.Evidence, domainEvidenceType);
			}
			if ((scope & IsolatedStorageScope.Assembly) != IsolatedStorageScope.None)
			{
				Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
				isolatedStorageFile._fullEvidences = evidence;
				if ((scope & IsolatedStorageScope.Domain) != IsolatedStorageScope.None)
				{
					if (assemblyEvidenceType == null)
					{
						assemblyEvidenceType = typeof(Url);
					}
					isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetTypeFromEvidence(evidence, assemblyEvidenceType);
				}
				else
				{
					isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
				}
			}
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A91 RID: 27281 RVA: 0x0016CBA9 File Offset: 0x0016ADA9
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object applicationIdentity)
		{
			IsolatedStorageFile.Demand(scope);
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile._applicationIdentity = applicationIdentity;
			isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A92 RID: 27282 RVA: 0x0016CBE2 File Offset: 0x0016ADE2
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type applicationEvidenceType)
		{
			IsolatedStorageFile.Demand(scope);
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile.InitStore(scope, applicationEvidenceType);
			isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A93 RID: 27283 RVA: 0x0016CC10 File Offset: 0x0016AE10
		[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.ApplicationIsolationByMachine)]
		public static IsolatedStorageFile GetMachineStoreForApplication()
		{
			IsolatedStorageScope scope = IsolatedStorageScope.Machine | IsolatedStorageScope.Application;
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile.InitStore(scope, null);
			isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A94 RID: 27284 RVA: 0x0016CC44 File Offset: 0x0016AE44
		[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.AssemblyIsolationByMachine)]
		public static IsolatedStorageFile GetMachineStoreForAssembly()
		{
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine);
			Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile._fullEvidences = evidence;
			isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A95 RID: 27285 RVA: 0x0016CC7C File Offset: 0x0016AE7C
		[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.DomainIsolationByMachine)]
		public static IsolatedStorageFile GetMachineStoreForDomain()
		{
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine);
			isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetDomainIdentityFromEvidence(AppDomain.CurrentDomain.Evidence);
			Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile._fullEvidences = evidence;
			isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A96 RID: 27286 RVA: 0x0016CCCC File Offset: 0x0016AECC
		[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.ApplicationIsolationByUser)]
		public static IsolatedStorageFile GetUserStoreForApplication()
		{
			IsolatedStorageScope scope = IsolatedStorageScope.User | IsolatedStorageScope.Application;
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(scope);
			isolatedStorageFile.InitStore(scope, null);
			isolatedStorageFile._fullEvidences = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A97 RID: 27287 RVA: 0x0016CD00 File Offset: 0x0016AF00
		[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.AssemblyIsolationByUser)]
		public static IsolatedStorageFile GetUserStoreForAssembly()
		{
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(IsolatedStorageScope.User | IsolatedStorageScope.Assembly);
			Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile._fullEvidences = evidence;
			isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A98 RID: 27288 RVA: 0x0016CD38 File Offset: 0x0016AF38
		[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.DomainIsolationByUser)]
		public static IsolatedStorageFile GetUserStoreForDomain()
		{
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly);
			isolatedStorageFile._domainIdentity = IsolatedStorageFile.GetDomainIdentityFromEvidence(AppDomain.CurrentDomain.Evidence);
			Evidence evidence = Assembly.GetCallingAssembly().UnprotectedGetEvidence();
			isolatedStorageFile._fullEvidences = evidence;
			isolatedStorageFile._assemblyIdentity = IsolatedStorageFile.GetAssemblyIdentityFromEvidence(evidence);
			isolatedStorageFile.PostInit();
			return isolatedStorageFile;
		}

		// Token: 0x06006A99 RID: 27289 RVA: 0x000472CC File Offset: 0x000454CC
		[ComVisible(false)]
		public static IsolatedStorageFile GetUserStoreForSite()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06006A9A RID: 27290 RVA: 0x0016CD84 File Offset: 0x0016AF84
		public static void Remove(IsolatedStorageScope scope)
		{
			string isolatedStorageRoot = IsolatedStorageFile.GetIsolatedStorageRoot(scope);
			if (!Directory.Exists(isolatedStorageRoot))
			{
				return;
			}
			try
			{
				Directory.Delete(isolatedStorageRoot, true);
			}
			catch (IOException)
			{
				throw new IsolatedStorageException("Could not remove storage.");
			}
		}

		// Token: 0x06006A9B RID: 27291 RVA: 0x0016CDC8 File Offset: 0x0016AFC8
		internal static string GetIsolatedStorageRoot(IsolatedStorageScope scope)
		{
			string text = null;
			if ((scope & IsolatedStorageScope.User) != IsolatedStorageScope.None)
			{
				if ((scope & IsolatedStorageScope.Roaming) != IsolatedStorageScope.None)
				{
					text = Environment.UnixGetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create);
				}
				else
				{
					text = Environment.UnixGetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
				}
			}
			else if ((scope & IsolatedStorageScope.Machine) != IsolatedStorageScope.None)
			{
				text = Environment.UnixGetFolderPath(Environment.SpecialFolder.CommonApplicationData, Environment.SpecialFolderOption.Create);
			}
			if (text == null)
			{
				throw new IsolatedStorageException(string.Format(Locale.GetText("Couldn't access storage location for '{0}'."), scope));
			}
			return Path.Combine(text, ".isolated-storage");
		}

		// Token: 0x06006A9C RID: 27292 RVA: 0x0016CE3B File Offset: 0x0016B03B
		private static void Demand(IsolatedStorageScope scope)
		{
			if (SecurityManager.SecurityEnabled)
			{
				new IsolatedStorageFilePermission(PermissionState.None)
				{
					UsageAllowed = IsolatedStorageFile.ScopeToContainment(scope)
				}.Demand();
			}
		}

		// Token: 0x06006A9D RID: 27293 RVA: 0x0016CE5C File Offset: 0x0016B05C
		private static IsolatedStorageContainment ScopeToContainment(IsolatedStorageScope scope)
		{
			if (scope <= (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming))
			{
				if (scope <= (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly))
				{
					if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Assembly))
					{
						return IsolatedStorageContainment.AssemblyIsolationByUser;
					}
					if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly))
					{
						return IsolatedStorageContainment.DomainIsolationByUser;
					}
				}
				else
				{
					if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming))
					{
						return IsolatedStorageContainment.AssemblyIsolationByRoamingUser;
					}
					if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming))
					{
						return IsolatedStorageContainment.DomainIsolationByRoamingUser;
					}
				}
			}
			else if (scope <= (IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine))
			{
				if (scope == (IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine))
				{
					return IsolatedStorageContainment.AssemblyIsolationByMachine;
				}
				if (scope == (IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine))
				{
					return IsolatedStorageContainment.DomainIsolationByMachine;
				}
			}
			else
			{
				if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Application))
				{
					return IsolatedStorageContainment.ApplicationIsolationByUser;
				}
				if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application))
				{
					return IsolatedStorageContainment.ApplicationIsolationByRoamingUser;
				}
				if (scope == (IsolatedStorageScope.Machine | IsolatedStorageScope.Application))
				{
					return IsolatedStorageContainment.ApplicationIsolationByMachine;
				}
			}
			return IsolatedStorageContainment.UnrestrictedIsolatedStorage;
		}

		// Token: 0x06006A9E RID: 27294 RVA: 0x0016CECC File Offset: 0x0016B0CC
		internal static ulong GetDirectorySize(DirectoryInfo di)
		{
			ulong num = 0UL;
			foreach (FileInfo fileInfo in di.GetFiles())
			{
				num += (ulong)fileInfo.Length;
			}
			foreach (DirectoryInfo di2 in di.GetDirectories())
			{
				num += IsolatedStorageFile.GetDirectorySize(di2);
			}
			return num;
		}

		// Token: 0x06006A9F RID: 27295 RVA: 0x0016CF26 File Offset: 0x0016B126
		private IsolatedStorageFile(IsolatedStorageScope scope)
		{
			this.storage_scope = scope;
		}

		// Token: 0x06006AA0 RID: 27296 RVA: 0x0016CF35 File Offset: 0x0016B135
		internal IsolatedStorageFile(IsolatedStorageScope scope, string location)
		{
			this.storage_scope = scope;
			this.directory = new DirectoryInfo(location);
			if (!this.directory.Exists)
			{
				throw new IsolatedStorageException(Locale.GetText("Invalid storage."));
			}
		}

		// Token: 0x06006AA1 RID: 27297 RVA: 0x0016CF70 File Offset: 0x0016B170
		~IsolatedStorageFile()
		{
		}

		// Token: 0x06006AA2 RID: 27298 RVA: 0x0016CF98 File Offset: 0x0016B198
		private void PostInit()
		{
			string text = IsolatedStorageFile.GetIsolatedStorageRoot(base.Scope);
			string path;
			if (this._applicationIdentity != null)
			{
				path = string.Format("a{0}{1}", this.SeparatorInternal, this.GetNameFromIdentity(this._applicationIdentity));
			}
			else if (this._domainIdentity != null)
			{
				path = string.Format("d{0}{1}{0}{2}", this.SeparatorInternal, this.GetNameFromIdentity(this._domainIdentity), this.GetNameFromIdentity(this._assemblyIdentity));
			}
			else
			{
				if (this._assemblyIdentity == null)
				{
					throw new IsolatedStorageException(Locale.GetText("No code identity available."));
				}
				path = string.Format("d{0}none{0}{1}", this.SeparatorInternal, this.GetNameFromIdentity(this._assemblyIdentity));
			}
			text = Path.Combine(text, path);
			this.directory = new DirectoryInfo(text);
			if (!this.directory.Exists)
			{
				try
				{
					this.directory.Create();
					this.SaveIdentities(text);
				}
				catch (IOException)
				{
				}
			}
		}

		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x0016D09C File Offset: 0x0016B29C
		[CLSCompliant(false)]
		[Obsolete]
		public override ulong CurrentSize
		{
			get
			{
				return IsolatedStorageFile.GetDirectorySize(this.directory);
			}
		}

		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06006AA4 RID: 27300 RVA: 0x0016D0AC File Offset: 0x0016B2AC
		[CLSCompliant(false)]
		[Obsolete]
		public override ulong MaximumSize
		{
			get
			{
				if (!SecurityManager.SecurityEnabled)
				{
					return 9223372036854775807UL;
				}
				if (this._resolved)
				{
					return this._maxSize;
				}
				Evidence evidence;
				if (this._fullEvidences != null)
				{
					evidence = this._fullEvidences;
				}
				else
				{
					evidence = new Evidence();
					if (this._assemblyIdentity != null)
					{
						evidence.AddHost(this._assemblyIdentity);
					}
				}
				if (evidence.Count < 1)
				{
					throw new InvalidOperationException(Locale.GetText("Couldn't get the quota from the available evidences."));
				}
				PermissionSet permissionSet = null;
				PermissionSet permissionSet2 = SecurityManager.ResolvePolicy(evidence, null, null, null, out permissionSet);
				IsolatedStoragePermission permission = this.GetPermission(permissionSet2);
				if (permission == null)
				{
					if (!permissionSet2.IsUnrestricted())
					{
						throw new InvalidOperationException(Locale.GetText("No quota from the available evidences."));
					}
					this._maxSize = 9223372036854775807UL;
				}
				else
				{
					this._maxSize = (ulong)permission.UserQuota;
				}
				this._resolved = true;
				return this._maxSize;
			}
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06006AA5 RID: 27301 RVA: 0x0016D17A File Offset: 0x0016B37A
		internal string Root
		{
			get
			{
				return this.directory.FullName;
			}
		}

		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06006AA6 RID: 27302 RVA: 0x0016D187 File Offset: 0x0016B387
		[ComVisible(false)]
		public override long AvailableFreeSpace
		{
			get
			{
				this.CheckOpen();
				return long.MaxValue;
			}
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06006AA7 RID: 27303 RVA: 0x0016D198 File Offset: 0x0016B398
		[ComVisible(false)]
		public override long Quota
		{
			get
			{
				this.CheckOpen();
				return (long)this.MaximumSize;
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06006AA8 RID: 27304 RVA: 0x0016D1A6 File Offset: 0x0016B3A6
		[ComVisible(false)]
		public override long UsedSize
		{
			get
			{
				this.CheckOpen();
				return (long)IsolatedStorageFile.GetDirectorySize(this.directory);
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06006AA9 RID: 27305 RVA: 0x000040F7 File Offset: 0x000022F7
		[ComVisible(false)]
		public static bool IsEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06006AAA RID: 27306 RVA: 0x0016D1B9 File Offset: 0x0016B3B9
		internal bool IsClosed
		{
			get
			{
				return this.closed;
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06006AAB RID: 27307 RVA: 0x0016D1C1 File Offset: 0x0016B3C1
		internal bool IsDisposed
		{
			get
			{
				return this.disposed;
			}
		}

		// Token: 0x06006AAC RID: 27308 RVA: 0x0016D1C9 File Offset: 0x0016B3C9
		public void Close()
		{
			this.closed = true;
		}

		// Token: 0x06006AAD RID: 27309 RVA: 0x0016D1D4 File Offset: 0x0016B3D4
		public void CreateDirectory(string dir)
		{
			if (dir == null)
			{
				throw new ArgumentNullException("dir");
			}
			if (dir.IndexOfAny(Path.PathSeparatorChars) >= 0)
			{
				string[] array = dir.Split(Path.PathSeparatorChars, StringSplitOptions.RemoveEmptyEntries);
				DirectoryInfo directoryInfo = this.directory;
				for (int i = 0; i < array.Length; i++)
				{
					if (directoryInfo.GetFiles(array[i]).Length != 0)
					{
						throw new IsolatedStorageException("Unable to create directory.");
					}
					directoryInfo = directoryInfo.CreateSubdirectory(array[i]);
				}
				return;
			}
			if (this.directory.GetFiles(dir).Length != 0)
			{
				throw new IsolatedStorageException("Unable to create directory.");
			}
			this.directory.CreateSubdirectory(dir);
		}

		// Token: 0x06006AAE RID: 27310 RVA: 0x0016D268 File Offset: 0x0016B468
		[ComVisible(false)]
		public void CopyFile(string sourceFileName, string destinationFileName)
		{
			this.CopyFile(sourceFileName, destinationFileName, false);
		}

		// Token: 0x06006AAF RID: 27311 RVA: 0x0016D274 File Offset: 0x0016B474
		[ComVisible(false)]
		public void CopyFile(string sourceFileName, string destinationFileName, bool overwrite)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName");
			}
			if (destinationFileName == null)
			{
				throw new ArgumentNullException("destinationFileName");
			}
			if (sourceFileName.Trim().Length == 0)
			{
				throw new ArgumentException("An empty file name is not valid.", "sourceFileName");
			}
			if (destinationFileName.Trim().Length == 0)
			{
				throw new ArgumentException("An empty file name is not valid.", "destinationFileName");
			}
			this.CheckOpen();
			string text = Path.Combine(this.directory.FullName, sourceFileName);
			string text2 = Path.Combine(this.directory.FullName, destinationFileName);
			if (!this.IsPathInStorage(text) || !this.IsPathInStorage(text2))
			{
				throw new IsolatedStorageException("Operation not allowed.");
			}
			if (!Directory.Exists(Path.GetDirectoryName(text)))
			{
				throw new DirectoryNotFoundException("Could not find a part of path '" + sourceFileName + "'.");
			}
			if (!File.Exists(text))
			{
				throw new FileNotFoundException("Could not find a part of path '" + sourceFileName + "'.");
			}
			if (File.Exists(text2) && !overwrite)
			{
				throw new IsolatedStorageException("Operation not allowed.");
			}
			try
			{
				File.Copy(text, text2, overwrite);
			}
			catch (IOException inner)
			{
				throw new IsolatedStorageException("Operation not allowed.", inner);
			}
			catch (UnauthorizedAccessException inner2)
			{
				throw new IsolatedStorageException("Operation not allowed.", inner2);
			}
		}

		// Token: 0x06006AB0 RID: 27312 RVA: 0x0016D3B8 File Offset: 0x0016B5B8
		[ComVisible(false)]
		public IsolatedStorageFileStream CreateFile(string path)
		{
			return new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, this);
		}

		// Token: 0x06006AB1 RID: 27313 RVA: 0x0016D3C4 File Offset: 0x0016B5C4
		public void DeleteDirectory(string dir)
		{
			try
			{
				if (Path.IsPathRooted(dir))
				{
					dir = dir.Substring(1);
				}
				this.directory.CreateSubdirectory(dir).Delete();
			}
			catch
			{
				throw new IsolatedStorageException(Locale.GetText("Could not delete directory '{0}'", new object[]
				{
					dir
				}));
			}
		}

		// Token: 0x06006AB2 RID: 27314 RVA: 0x0016D420 File Offset: 0x0016B620
		public void DeleteFile(string file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			if (!File.Exists(Path.Combine(this.directory.FullName, file)))
			{
				throw new IsolatedStorageException(Locale.GetText("Could not delete file '{0}'", new object[]
				{
					file
				}));
			}
			try
			{
				File.Delete(Path.Combine(this.directory.FullName, file));
			}
			catch
			{
				throw new IsolatedStorageException(Locale.GetText("Could not delete file '{0}'", new object[]
				{
					file
				}));
			}
		}

		// Token: 0x06006AB3 RID: 27315 RVA: 0x0016D4B4 File Offset: 0x0016B6B4
		public void Dispose()
		{
			this.disposed = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06006AB4 RID: 27316 RVA: 0x0016D4C4 File Offset: 0x0016B6C4
		[ComVisible(false)]
		public bool DirectoryExists(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			this.CheckOpen();
			string path2 = Path.Combine(this.directory.FullName, path);
			return this.IsPathInStorage(path2) && Directory.Exists(path2);
		}

		// Token: 0x06006AB5 RID: 27317 RVA: 0x0016D508 File Offset: 0x0016B708
		[ComVisible(false)]
		public bool FileExists(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			this.CheckOpen();
			string path2 = Path.Combine(this.directory.FullName, path);
			return this.IsPathInStorage(path2) && File.Exists(path2);
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x0016D54C File Offset: 0x0016B74C
		[ComVisible(false)]
		public DateTimeOffset GetCreationTime(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("An empty path is not valid.");
			}
			this.CheckOpen();
			string path2 = Path.Combine(this.directory.FullName, path);
			if (File.Exists(path2))
			{
				return File.GetCreationTime(path2);
			}
			return Directory.GetCreationTime(path2);
		}

		// Token: 0x06006AB7 RID: 27319 RVA: 0x0016D5B8 File Offset: 0x0016B7B8
		[ComVisible(false)]
		public DateTimeOffset GetLastAccessTime(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("An empty path is not valid.");
			}
			this.CheckOpen();
			string path2 = Path.Combine(this.directory.FullName, path);
			if (File.Exists(path2))
			{
				return File.GetLastAccessTime(path2);
			}
			return Directory.GetLastAccessTime(path2);
		}

		// Token: 0x06006AB8 RID: 27320 RVA: 0x0016D624 File Offset: 0x0016B824
		[ComVisible(false)]
		public DateTimeOffset GetLastWriteTime(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Trim().Length == 0)
			{
				throw new ArgumentException("An empty path is not valid.");
			}
			this.CheckOpen();
			string path2 = Path.Combine(this.directory.FullName, path);
			if (File.Exists(path2))
			{
				return File.GetLastWriteTime(path2);
			}
			return Directory.GetLastWriteTime(path2);
		}

		// Token: 0x06006AB9 RID: 27321 RVA: 0x0016D690 File Offset: 0x0016B890
		public string[] GetDirectoryNames(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			if (searchPattern.Contains(".."))
			{
				throw new ArgumentException("Search pattern cannot contain '..' to move up directories.", "searchPattern");
			}
			string directoryName = Path.GetDirectoryName(searchPattern);
			string fileName = Path.GetFileName(searchPattern);
			DirectoryInfo[] array = null;
			if (directoryName == null || directoryName.Length == 0)
			{
				array = this.directory.GetDirectories(searchPattern);
			}
			else
			{
				DirectoryInfo directoryInfo = this.directory.GetDirectories(directoryName)[0];
				if (directoryInfo.FullName.IndexOf(this.directory.FullName) >= 0)
				{
					array = directoryInfo.GetDirectories(fileName);
					string[] array2 = directoryName.Split(new char[]
					{
						Path.DirectorySeparatorChar
					}, StringSplitOptions.RemoveEmptyEntries);
					for (int i = array2.Length - 1; i >= 0; i--)
					{
						if (directoryInfo.Name != array2[i])
						{
							array = null;
							break;
						}
						directoryInfo = directoryInfo.Parent;
					}
				}
			}
			if (array == null)
			{
				throw new SecurityException();
			}
			FileSystemInfo[] afsi = array;
			return this.GetNames(afsi);
		}

		// Token: 0x06006ABA RID: 27322 RVA: 0x0016D77F File Offset: 0x0016B97F
		[ComVisible(false)]
		public string[] GetDirectoryNames()
		{
			return this.GetDirectoryNames("*");
		}

		// Token: 0x06006ABB RID: 27323 RVA: 0x0016D78C File Offset: 0x0016B98C
		private string[] GetNames(FileSystemInfo[] afsi)
		{
			string[] array = new string[afsi.Length];
			for (int num = 0; num != afsi.Length; num++)
			{
				array[num] = afsi[num].Name;
			}
			return array;
		}

		// Token: 0x06006ABC RID: 27324 RVA: 0x0016D7BC File Offset: 0x0016B9BC
		public string[] GetFileNames(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			if (searchPattern.Contains(".."))
			{
				throw new ArgumentException("Search pattern cannot contain '..' to move up directories.", "searchPattern");
			}
			string directoryName = Path.GetDirectoryName(searchPattern);
			string fileName = Path.GetFileName(searchPattern);
			FileInfo[] files;
			if (directoryName == null || directoryName.Length == 0)
			{
				files = this.directory.GetFiles(searchPattern);
			}
			else
			{
				DirectoryInfo[] directories = this.directory.GetDirectories(directoryName);
				if (directories.Length != 1)
				{
					throw new SecurityException();
				}
				if (!directories[0].FullName.StartsWith(this.directory.FullName))
				{
					throw new SecurityException();
				}
				if (directories[0].FullName.Substring(this.directory.FullName.Length + 1) != directoryName)
				{
					throw new SecurityException();
				}
				files = directories[0].GetFiles(fileName);
			}
			FileSystemInfo[] afsi = files;
			return this.GetNames(afsi);
		}

		// Token: 0x06006ABD RID: 27325 RVA: 0x0016D895 File Offset: 0x0016BA95
		[ComVisible(false)]
		public string[] GetFileNames()
		{
			return this.GetFileNames("*");
		}

		// Token: 0x06006ABE RID: 27326 RVA: 0x0016D8A2 File Offset: 0x0016BAA2
		[ComVisible(false)]
		public override bool IncreaseQuotaTo(long newQuotaSize)
		{
			if (newQuotaSize < this.Quota)
			{
				throw new ArgumentException();
			}
			this.CheckOpen();
			return false;
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x0016D8BC File Offset: 0x0016BABC
		[ComVisible(false)]
		public void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			if (sourceDirectoryName == null)
			{
				throw new ArgumentNullException("sourceDirectoryName");
			}
			if (destinationDirectoryName == null)
			{
				throw new ArgumentNullException("sourceDirectoryName");
			}
			if (sourceDirectoryName.Trim().Length == 0)
			{
				throw new ArgumentException("An empty directory name is not valid.", "sourceDirectoryName");
			}
			if (destinationDirectoryName.Trim().Length == 0)
			{
				throw new ArgumentException("An empty directory name is not valid.", "destinationDirectoryName");
			}
			this.CheckOpen();
			string text = Path.Combine(this.directory.FullName, sourceDirectoryName);
			string text2 = Path.Combine(this.directory.FullName, destinationDirectoryName);
			if (!this.IsPathInStorage(text) || !this.IsPathInStorage(text2))
			{
				throw new IsolatedStorageException("Operation not allowed.");
			}
			if (!Directory.Exists(text))
			{
				throw new DirectoryNotFoundException("Could not find a part of path '" + sourceDirectoryName + "'.");
			}
			if (!Directory.Exists(Path.GetDirectoryName(text2)))
			{
				throw new DirectoryNotFoundException("Could not find a part of path '" + destinationDirectoryName + "'.");
			}
			try
			{
				Directory.Move(text, text2);
			}
			catch (IOException inner)
			{
				throw new IsolatedStorageException("Operation not allowed.", inner);
			}
			catch (UnauthorizedAccessException inner2)
			{
				throw new IsolatedStorageException("Operation not allowed.", inner2);
			}
		}

		// Token: 0x06006AC0 RID: 27328 RVA: 0x0016D9E8 File Offset: 0x0016BBE8
		[ComVisible(false)]
		public void MoveFile(string sourceFileName, string destinationFileName)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName");
			}
			if (destinationFileName == null)
			{
				throw new ArgumentNullException("sourceFileName");
			}
			if (sourceFileName.Trim().Length == 0)
			{
				throw new ArgumentException("An empty file name is not valid.", "sourceFileName");
			}
			if (destinationFileName.Trim().Length == 0)
			{
				throw new ArgumentException("An empty file name is not valid.", "destinationFileName");
			}
			this.CheckOpen();
			string text = Path.Combine(this.directory.FullName, sourceFileName);
			string text2 = Path.Combine(this.directory.FullName, destinationFileName);
			if (!this.IsPathInStorage(text) || !this.IsPathInStorage(text2))
			{
				throw new IsolatedStorageException("Operation not allowed.");
			}
			if (!File.Exists(text))
			{
				throw new FileNotFoundException("Could not find a part of path '" + sourceFileName + "'.");
			}
			if (!Directory.Exists(Path.GetDirectoryName(text2)))
			{
				throw new IsolatedStorageException("Operation not allowed.");
			}
			try
			{
				File.Move(text, text2);
			}
			catch (UnauthorizedAccessException inner)
			{
				throw new IsolatedStorageException("Operation not allowed.", inner);
			}
		}

		// Token: 0x06006AC1 RID: 27329 RVA: 0x0016DAF0 File Offset: 0x0016BCF0
		[ComVisible(false)]
		public IsolatedStorageFileStream OpenFile(string path, FileMode mode)
		{
			return new IsolatedStorageFileStream(path, mode, this);
		}

		// Token: 0x06006AC2 RID: 27330 RVA: 0x0016DAFA File Offset: 0x0016BCFA
		[ComVisible(false)]
		public IsolatedStorageFileStream OpenFile(string path, FileMode mode, FileAccess access)
		{
			return new IsolatedStorageFileStream(path, mode, access, this);
		}

		// Token: 0x06006AC3 RID: 27331 RVA: 0x0016DB05 File Offset: 0x0016BD05
		[ComVisible(false)]
		public IsolatedStorageFileStream OpenFile(string path, FileMode mode, FileAccess access, FileShare share)
		{
			return new IsolatedStorageFileStream(path, mode, access, share, this);
		}

		// Token: 0x06006AC4 RID: 27332 RVA: 0x0016DB14 File Offset: 0x0016BD14
		public override void Remove()
		{
			this.CheckOpen(false);
			try
			{
				this.directory.Delete(true);
			}
			catch
			{
				throw new IsolatedStorageException("Could not remove storage.");
			}
			this.Close();
		}

		// Token: 0x06006AC5 RID: 27333 RVA: 0x0016DB58 File Offset: 0x0016BD58
		protected override IsolatedStoragePermission GetPermission(PermissionSet ps)
		{
			if (ps == null)
			{
				return null;
			}
			return (IsolatedStoragePermission)ps.GetPermission(typeof(IsolatedStorageFilePermission));
		}

		// Token: 0x06006AC6 RID: 27334 RVA: 0x0016DB74 File Offset: 0x0016BD74
		private void CheckOpen()
		{
			this.CheckOpen(true);
		}

		// Token: 0x06006AC7 RID: 27335 RVA: 0x0016DB80 File Offset: 0x0016BD80
		private void CheckOpen(bool checkDirExists)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("IsolatedStorageFile");
			}
			if (this.closed)
			{
				throw new InvalidOperationException("Storage needs to be open for this operation.");
			}
			if (checkDirExists && !Directory.Exists(this.directory.FullName))
			{
				throw new IsolatedStorageException("Isolated storage has been removed or disabled.");
			}
		}

		// Token: 0x06006AC8 RID: 27336 RVA: 0x0016DBD3 File Offset: 0x0016BDD3
		private bool IsPathInStorage(string path)
		{
			return Path.GetFullPath(path).StartsWith(this.directory.FullName);
		}

		// Token: 0x06006AC9 RID: 27337 RVA: 0x0016DBEC File Offset: 0x0016BDEC
		private string GetNameFromIdentity(object identity)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(identity.ToString());
			Array src = SHA1.Create().ComputeHash(bytes, 0, bytes.Length);
			byte[] array = new byte[10];
			Buffer.BlockCopy(src, 0, array, 0, array.Length);
			return CryptoConvert.ToHex(array);
		}

		// Token: 0x06006ACA RID: 27338 RVA: 0x0016DC34 File Offset: 0x0016BE34
		private static object GetTypeFromEvidence(Evidence e, Type t)
		{
			foreach (object obj in e)
			{
				if (obj.GetType() == t)
				{
					return obj;
				}
			}
			return null;
		}

		// Token: 0x06006ACB RID: 27339 RVA: 0x0016DC94 File Offset: 0x0016BE94
		internal static object GetAssemblyIdentityFromEvidence(Evidence e)
		{
			object typeFromEvidence = IsolatedStorageFile.GetTypeFromEvidence(e, typeof(Publisher));
			if (typeFromEvidence != null)
			{
				return typeFromEvidence;
			}
			typeFromEvidence = IsolatedStorageFile.GetTypeFromEvidence(e, typeof(StrongName));
			if (typeFromEvidence != null)
			{
				return typeFromEvidence;
			}
			return IsolatedStorageFile.GetTypeFromEvidence(e, typeof(Url));
		}

		// Token: 0x06006ACC RID: 27340 RVA: 0x0016DCE0 File Offset: 0x0016BEE0
		internal static object GetDomainIdentityFromEvidence(Evidence e)
		{
			object typeFromEvidence = IsolatedStorageFile.GetTypeFromEvidence(e, typeof(ApplicationDirectory));
			if (typeFromEvidence != null)
			{
				return typeFromEvidence;
			}
			return IsolatedStorageFile.GetTypeFromEvidence(e, typeof(Url));
		}

		// Token: 0x06006ACD RID: 27341 RVA: 0x0016DD14 File Offset: 0x0016BF14
		[SecurityPermission(SecurityAction.Assert, SerializationFormatter = true)]
		private void SaveIdentities(string root)
		{
			IsolatedStorageFile.Identities identities = new IsolatedStorageFile.Identities(this._applicationIdentity, this._assemblyIdentity, this._domainIdentity);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			IsolatedStorageFile.mutex.WaitOne();
			try
			{
				using (FileStream fileStream = File.Create(root + ".storage"))
				{
					binaryFormatter.Serialize(fileStream, identities);
				}
			}
			finally
			{
				IsolatedStorageFile.mutex.ReleaseMutex();
			}
		}

		// Token: 0x06006ACF RID: 27343 RVA: 0x000173AD File Offset: 0x000155AD
		internal IsolatedStorageFile()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04003D99 RID: 15769
		private bool _resolved;

		// Token: 0x04003D9A RID: 15770
		private ulong _maxSize;

		// Token: 0x04003D9B RID: 15771
		private Evidence _fullEvidences;

		// Token: 0x04003D9C RID: 15772
		private static readonly Mutex mutex = new Mutex();

		// Token: 0x04003D9D RID: 15773
		private bool closed;

		// Token: 0x04003D9E RID: 15774
		private bool disposed;

		// Token: 0x04003D9F RID: 15775
		private DirectoryInfo directory;

		// Token: 0x02000B74 RID: 2932
		[Serializable]
		private struct Identities
		{
			// Token: 0x06006AD0 RID: 27344 RVA: 0x0016DDA8 File Offset: 0x0016BFA8
			public Identities(object application, object assembly, object domain)
			{
				this.Application = application;
				this.Assembly = assembly;
				this.Domain = domain;
			}

			// Token: 0x04003DA0 RID: 15776
			public object Application;

			// Token: 0x04003DA1 RID: 15777
			public object Assembly;

			// Token: 0x04003DA2 RID: 15778
			public object Domain;
		}
	}
}
