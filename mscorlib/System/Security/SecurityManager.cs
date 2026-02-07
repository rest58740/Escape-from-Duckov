using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;

namespace System.Security
{
	// Token: 0x020003EB RID: 1003
	[ComVisible(true)]
	public static class SecurityManager
	{
		// Token: 0x0600295B RID: 10587 RVA: 0x00095FA4 File Offset: 0x000941A4
		static SecurityManager()
		{
			SecurityManager._lockObject = new object();
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x0600295C RID: 10588 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		// (set) Token: 0x0600295D RID: 10589 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Obsolete]
		public static bool CheckExecutionRights
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x0600295E RID: 10590
		// (set) Token: 0x0600295F RID: 10591
		[Obsolete("The security manager cannot be turned off on MS runtime")]
		public static extern bool SecurityEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; [SecurityPermission(SecurityAction.Demand, ControlPolicy = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		// Token: 0x06002960 RID: 10592 RVA: 0x000040F7 File Offset: 0x000022F7
		internal static bool CheckElevatedPermissions()
		{
			return true;
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Conditional("ENABLE_SANDBOX")]
		internal static void EnsureElevatedPermissions()
		{
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x00095FBB File Offset: 0x000941BB
		[MonoTODO("CAS support is experimental (and unsupported). This method only works in FullTrust.")]
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000")]
		public static void GetZoneAndOrigin(out ArrayList zone, out ArrayList origin)
		{
			zone = new ArrayList();
			origin = new ArrayList();
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x00095FCB File Offset: 0x000941CB
		[Obsolete]
		public static bool IsGranted(IPermission perm)
		{
			return perm == null || !SecurityManager.SecurityEnabled || SecurityManager.IsGranted(Assembly.GetCallingAssembly(), perm);
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x00095FE8 File Offset: 0x000941E8
		internal static bool IsGranted(Assembly a, IPermission perm)
		{
			PermissionSet grantedPermissionSet = a.GrantedPermissionSet;
			if (grantedPermissionSet != null && !grantedPermissionSet.IsUnrestricted())
			{
				CodeAccessPermission target = (CodeAccessPermission)grantedPermissionSet.GetPermission(perm.GetType());
				if (!perm.IsSubsetOf(target))
				{
					return false;
				}
			}
			PermissionSet deniedPermissionSet = a.DeniedPermissionSet;
			if (deniedPermissionSet != null && !deniedPermissionSet.IsEmpty())
			{
				if (deniedPermissionSet.IsUnrestricted())
				{
					return false;
				}
				CodeAccessPermission codeAccessPermission = (CodeAccessPermission)a.DeniedPermissionSet.GetPermission(perm.GetType());
				if (codeAccessPermission != null && perm.IsSubsetOf(codeAccessPermission))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x00096068 File Offset: 0x00094268
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static PolicyLevel LoadPolicyLevelFromFile(string path, PolicyLevelType type)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			PolicyLevel policyLevel = null;
			try
			{
				policyLevel = new PolicyLevel(type.ToString(), type);
				policyLevel.LoadFromFile(path);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(Locale.GetText("Invalid policy XML"), innerException);
			}
			return policyLevel;
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000960C8 File Offset: 0x000942C8
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static PolicyLevel LoadPolicyLevelFromString(string str, PolicyLevelType type)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			PolicyLevel policyLevel = null;
			try
			{
				policyLevel = new PolicyLevel(type.ToString(), type);
				policyLevel.LoadFromString(str);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(Locale.GetText("Invalid policy XML"), innerException);
			}
			return policyLevel;
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x00096128 File Offset: 0x00094328
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static IEnumerator PolicyHierarchy()
		{
			return SecurityManager.Hierarchy;
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x00096130 File Offset: 0x00094330
		[Obsolete]
		public static PermissionSet ResolvePolicy(Evidence evidence)
		{
			if (evidence == null)
			{
				return new PermissionSet(PermissionState.None);
			}
			PermissionSet permissionSet = null;
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				PolicyLevel pl = (PolicyLevel)obj;
				if (SecurityManager.ResolvePolicyLevel(ref permissionSet, pl, evidence))
				{
					break;
				}
			}
			SecurityManager.ResolveIdentityPermissions(permissionSet, evidence);
			return permissionSet;
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x00096178 File Offset: 0x00094378
		[MonoTODO("(2.0) more tests are needed")]
		[Obsolete]
		public static PermissionSet ResolvePolicy(Evidence[] evidences)
		{
			if (evidences == null || evidences.Length == 0 || (evidences.Length == 1 && evidences[0].Count == 0))
			{
				return new PermissionSet(PermissionState.None);
			}
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidences[0]);
			for (int i = 1; i < evidences.Length; i++)
			{
				permissionSet = permissionSet.Intersect(SecurityManager.ResolvePolicy(evidences[i]));
			}
			return permissionSet;
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000961CC File Offset: 0x000943CC
		[Obsolete]
		public static PermissionSet ResolveSystemPolicy(Evidence evidence)
		{
			if (evidence == null)
			{
				return new PermissionSet(PermissionState.None);
			}
			PermissionSet permissionSet = null;
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				PolicyLevel policyLevel = (PolicyLevel)obj;
				if (policyLevel.Type == PolicyLevelType.AppDomain || SecurityManager.ResolvePolicyLevel(ref permissionSet, policyLevel, evidence))
				{
					break;
				}
			}
			SecurityManager.ResolveIdentityPermissions(permissionSet, evidence);
			return permissionSet;
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x00096220 File Offset: 0x00094420
		[Obsolete]
		public static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied)
		{
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidence);
			if (reqdPset != null && !reqdPset.IsSubsetOf(permissionSet))
			{
				throw new PolicyException(Locale.GetText("Policy doesn't grant the minimal permissions required to execute the assembly."));
			}
			if (SecurityManager.CheckExecutionRights)
			{
				bool flag = false;
				if (permissionSet != null)
				{
					if (permissionSet.IsUnrestricted())
					{
						flag = true;
					}
					else
					{
						IPermission permission = permissionSet.GetPermission(typeof(SecurityPermission));
						flag = SecurityManager._execution.IsSubsetOf(permission);
					}
				}
				if (!flag)
				{
					throw new PolicyException(Locale.GetText("Policy doesn't grant the right to execute the assembly."));
				}
			}
			denied = denyPset;
			return permissionSet;
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000962A0 File Offset: 0x000944A0
		[Obsolete]
		public static IEnumerator ResolvePolicyGroups(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			ArrayList arrayList = new ArrayList();
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				CodeGroup value = ((PolicyLevel)obj).ResolveMatchingCodeGroups(evidence);
				arrayList.Add(value);
			}
			return arrayList.GetEnumerator();
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x000962F4 File Offset: 0x000944F4
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static void SavePolicy()
		{
			IEnumerator hierarchy = SecurityManager.Hierarchy;
			while (hierarchy.MoveNext())
			{
				object obj = hierarchy.Current;
				(obj as PolicyLevel).Save();
			}
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x00096321 File Offset: 0x00094521
		[Obsolete]
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		public static void SavePolicyLevel(PolicyLevel level)
		{
			level.Save();
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x0600296F RID: 10607 RVA: 0x0009632C File Offset: 0x0009452C
		private static IEnumerator Hierarchy
		{
			get
			{
				object lockObject = SecurityManager._lockObject;
				lock (lockObject)
				{
					if (SecurityManager._hierarchy == null)
					{
						SecurityManager.InitializePolicyHierarchy();
					}
				}
				return SecurityManager._hierarchy.GetEnumerator();
			}
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x0009637C File Offset: 0x0009457C
		private static void InitializePolicyHierarchy()
		{
			string directoryName = Path.GetDirectoryName(Environment.GetMachineConfigPath());
			string path = Path.Combine(Environment.UnixGetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "mono");
			PolicyLevel policyLevel = new PolicyLevel("Enterprise", PolicyLevelType.Enterprise);
			SecurityManager._level = policyLevel;
			policyLevel.LoadFromFile(Path.Combine(directoryName, "enterprisesec.config"));
			PolicyLevel policyLevel2 = new PolicyLevel("Machine", PolicyLevelType.Machine);
			SecurityManager._level = policyLevel2;
			policyLevel2.LoadFromFile(Path.Combine(directoryName, "security.config"));
			PolicyLevel policyLevel3 = new PolicyLevel("User", PolicyLevelType.User);
			SecurityManager._level = policyLevel3;
			policyLevel3.LoadFromFile(Path.Combine(path, "security.config"));
			SecurityManager._hierarchy = ArrayList.Synchronized(new ArrayList
			{
				policyLevel,
				policyLevel2,
				policyLevel3
			});
			SecurityManager._level = null;
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x00096448 File Offset: 0x00094648
		internal static bool ResolvePolicyLevel(ref PermissionSet ps, PolicyLevel pl, Evidence evidence)
		{
			PolicyStatement policyStatement = pl.Resolve(evidence);
			if (policyStatement != null)
			{
				if (ps == null)
				{
					ps = policyStatement.PermissionSet;
				}
				else
				{
					ps = ps.Intersect(policyStatement.PermissionSet);
					if (ps == null)
					{
						ps = new PermissionSet(PermissionState.None);
					}
				}
				if ((policyStatement.Attributes & PolicyStatementAttribute.LevelFinal) == PolicyStatementAttribute.LevelFinal)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x00096498 File Offset: 0x00094698
		internal static void ResolveIdentityPermissions(PermissionSet ps, Evidence evidence)
		{
			if (ps.IsUnrestricted())
			{
				return;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				IIdentityPermissionFactory identityPermissionFactory = obj as IIdentityPermissionFactory;
				if (identityPermissionFactory != null)
				{
					IPermission perm = identityPermissionFactory.CreateIdentityPermission(evidence);
					ps.AddPermission(perm);
				}
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06002973 RID: 10611 RVA: 0x000964DE File Offset: 0x000946DE
		// (set) Token: 0x06002974 RID: 10612 RVA: 0x000964E5 File Offset: 0x000946E5
		internal static PolicyLevel ResolvingPolicyLevel
		{
			get
			{
				return SecurityManager._level;
			}
			set
			{
				SecurityManager._level = value;
			}
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000964F0 File Offset: 0x000946F0
		internal static PermissionSet Decode(IntPtr permissions, int length)
		{
			PermissionSet permissionSet = null;
			object lockObject = SecurityManager._lockObject;
			lock (lockObject)
			{
				if (SecurityManager._declsecCache == null)
				{
					SecurityManager._declsecCache = new Hashtable();
				}
				object key = (int)permissions;
				permissionSet = (PermissionSet)SecurityManager._declsecCache[key];
				if (permissionSet == null)
				{
					byte[] array = new byte[length];
					Marshal.Copy(permissions, array, 0, length);
					permissionSet = SecurityManager.Decode(array);
					permissionSet.DeclarativeSecurity = true;
					SecurityManager._declsecCache.Add(key, permissionSet);
				}
			}
			return permissionSet;
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0009658C File Offset: 0x0009478C
		internal static PermissionSet Decode(byte[] encodedPermissions)
		{
			if (encodedPermissions == null || encodedPermissions.Length < 1)
			{
				throw new SecurityException("Invalid metadata format.");
			}
			byte b = encodedPermissions[0];
			if (b == 46)
			{
				return PermissionSet.CreateFromBinaryFormat(encodedPermissions);
			}
			if (b == 60)
			{
				return new PermissionSet(Encoding.Unicode.GetString(encodedPermissions));
			}
			throw new SecurityException(Locale.GetText("Unknown metadata format."));
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06002977 RID: 10615 RVA: 0x000965E4 File Offset: 0x000947E4
		private static IPermission UnmanagedCode
		{
			get
			{
				object lockObject = SecurityManager._lockObject;
				lock (lockObject)
				{
					if (SecurityManager._unmanagedCode == null)
					{
						SecurityManager._unmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
					}
				}
				return SecurityManager._unmanagedCode;
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x00096634 File Offset: 0x00094834
		private static void ThrowException(Exception ex)
		{
			throw ex;
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x00096637 File Offset: 0x00094837
		public static PermissionSet GetStandardSandbox(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x000479FC File Offset: 0x00045BFC
		public static bool CurrentThreadRequiresSecurityContextCapture()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04001EF5 RID: 7925
		private static object _lockObject;

		// Token: 0x04001EF6 RID: 7926
		private static ArrayList _hierarchy;

		// Token: 0x04001EF7 RID: 7927
		private static IPermission _unmanagedCode;

		// Token: 0x04001EF8 RID: 7928
		private static Hashtable _declsecCache;

		// Token: 0x04001EF9 RID: 7929
		private static PolicyLevel _level;

		// Token: 0x04001EFA RID: 7930
		private static SecurityPermission _execution = new SecurityPermission(SecurityPermissionFlag.Execution);
	}
}
