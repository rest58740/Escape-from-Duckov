using System;

namespace Microsoft.Win32
{
	// Token: 0x020000A1 RID: 161
	public static class Registry
	{
		// Token: 0x06000404 RID: 1028 RVA: 0x00015948 File Offset: 0x00013B48
		public static object GetValue(string keyName, string valueName, object defaultValue)
		{
			string name;
			object result;
			using (RegistryKey registryKey = Registry.GetBaseKeyFromKeyName(keyName, out name).OpenSubKey(name))
			{
				result = ((registryKey != null) ? registryKey.GetValue(valueName, defaultValue) : null);
			}
			return result;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00015990 File Offset: 0x00013B90
		public static void SetValue(string keyName, string valueName, object value)
		{
			Registry.SetValue(keyName, valueName, value, RegistryValueKind.Unknown);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001599C File Offset: 0x00013B9C
		public static void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
		{
			string subkey;
			using (RegistryKey registryKey = Registry.GetBaseKeyFromKeyName(keyName, out subkey).CreateSubKey(subkey))
			{
				registryKey.SetValue(valueName, value, valueKind);
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000159E0 File Offset: 0x00013BE0
		private static RegistryKey GetBaseKeyFromKeyName(string keyName, out string subKeyName)
		{
			if (keyName == null)
			{
				throw new ArgumentNullException("keyName");
			}
			int num = keyName.IndexOf('\\');
			int num2 = (num != -1) ? num : keyName.Length;
			RegistryKey registryKey = null;
			if (num2 != 10)
			{
				switch (num2)
				{
				case 17:
					registryKey = ((char.ToUpperInvariant(keyName[6]) == 'L') ? Registry.ClassesRoot : Registry.CurrentUser);
					break;
				case 18:
					registryKey = Registry.LocalMachine;
					break;
				case 19:
					registryKey = Registry.CurrentConfig;
					break;
				case 21:
					registryKey = Registry.PerformanceData;
					break;
				}
			}
			else
			{
				registryKey = Registry.Users;
			}
			if (registryKey != null && keyName.StartsWith(registryKey.Name, StringComparison.OrdinalIgnoreCase))
			{
				subKeyName = ((num == -1 || num == keyName.Length) ? string.Empty : keyName.Substring(num + 1, keyName.Length - num - 1));
				return registryKey;
			}
			throw new ArgumentException(SR.Format("Registry key name must start with a valid base key name.", "keyName"), "keyName");
		}

		// Token: 0x04000F46 RID: 3910
		public static readonly RegistryKey CurrentUser = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);

		// Token: 0x04000F47 RID: 3911
		public static readonly RegistryKey LocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);

		// Token: 0x04000F48 RID: 3912
		public static readonly RegistryKey ClassesRoot = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default);

		// Token: 0x04000F49 RID: 3913
		public static readonly RegistryKey Users = RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Default);

		// Token: 0x04000F4A RID: 3914
		public static readonly RegistryKey PerformanceData = RegistryKey.OpenBaseKey(RegistryHive.PerformanceData, RegistryView.Default);

		// Token: 0x04000F4B RID: 3915
		public static readonly RegistryKey CurrentConfig = RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, RegistryView.Default);

		// Token: 0x04000F4C RID: 3916
		[Obsolete("Use PerformanceData instead")]
		public static readonly RegistryKey DynData = RegistryKey.OpenBaseKey(RegistryHive.DynData, RegistryView.Default);
	}
}
