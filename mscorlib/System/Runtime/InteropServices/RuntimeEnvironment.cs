using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200071C RID: 1820
	[ComVisible(true)]
	public class RuntimeEnvironment
	{
		// Token: 0x060040E4 RID: 16612 RVA: 0x0000259F File Offset: 0x0000079F
		[Obsolete("Do not create instances of the RuntimeEnvironment class.  Call the static methods directly on this type instead", true)]
		public RuntimeEnvironment()
		{
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x000E1757 File Offset: 0x000DF957
		public static bool FromGlobalAccessCache(Assembly a)
		{
			return a.GlobalAssemblyCache;
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x000E175F File Offset: 0x000DF95F
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string GetSystemVersion()
		{
			return Assembly.GetExecutingAssembly().ImageRuntimeVersion;
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x000E176B File Offset: 0x000DF96B
		[SecuritySafeCritical]
		public static string GetRuntimeDirectory()
		{
			if (Environment.GetEnvironmentVariable("CSC_SDK_PATH_DISABLED") != null)
			{
				return null;
			}
			return RuntimeEnvironment.GetRuntimeDirectoryImpl();
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x000E1780 File Offset: 0x000DF980
		private static string GetRuntimeDirectoryImpl()
		{
			return Path.GetDirectoryName(typeof(object).Assembly.Location);
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x060040E9 RID: 16617 RVA: 0x000E179B File Offset: 0x000DF99B
		public static string SystemConfigurationFile
		{
			[SecuritySafeCritical]
			get
			{
				return Environment.GetMachineConfigPath();
			}
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x000472CC File Offset: 0x000454CC
		private static IntPtr GetRuntimeInterfaceImpl(Guid clsid, Guid riid)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x000E17A2 File Offset: 0x000DF9A2
		[ComVisible(false)]
		[SecurityCritical]
		public static IntPtr GetRuntimeInterfaceAsIntPtr(Guid clsid, Guid riid)
		{
			return RuntimeEnvironment.GetRuntimeInterfaceImpl(clsid, riid);
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x000E17AC File Offset: 0x000DF9AC
		[ComVisible(false)]
		[SecurityCritical]
		public static object GetRuntimeInterfaceAsObject(Guid clsid, Guid riid)
		{
			IntPtr intPtr = IntPtr.Zero;
			object objectForIUnknown;
			try
			{
				intPtr = RuntimeEnvironment.GetRuntimeInterfaceImpl(clsid, riid);
				objectForIUnknown = Marshal.GetObjectForIUnknown(intPtr);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
				}
			}
			return objectForIUnknown;
		}
	}
}
