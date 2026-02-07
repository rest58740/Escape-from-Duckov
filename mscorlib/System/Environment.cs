using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Mono;

namespace System
{
	// Token: 0x0200021C RID: 540
	[ComVisible(true)]
	public static class Environment
	{
		// Token: 0x0600181D RID: 6173 RVA: 0x0000270D File Offset: 0x0000090D
		internal static string GetResourceString(string key)
		{
			return key;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0000270D File Offset: 0x0000090D
		internal static string GetResourceString(string key, CultureInfo culture)
		{
			return key;
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0005CA33 File Offset: 0x0005AC33
		internal static string GetResourceString(string key, params object[] values)
		{
			return string.Format(CultureInfo.InvariantCulture, key, values);
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0000270D File Offset: 0x0000090D
		internal static string GetRuntimeResourceString(string key)
		{
			return key;
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0005CA33 File Offset: 0x0005AC33
		internal static string GetRuntimeResourceString(string key, params object[] values)
		{
			return string.Format(CultureInfo.InvariantCulture, key, values);
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0005CA44 File Offset: 0x0005AC44
		internal static string GetResourceStringEncodingName(int codePage)
		{
			if (codePage <= 12000)
			{
				if (codePage == 1200)
				{
					return Environment.GetResourceString("Unicode");
				}
				if (codePage == 1201)
				{
					return Environment.GetResourceString("Unicode (Big-Endian)");
				}
				if (codePage == 12000)
				{
					return Environment.GetResourceString("Unicode (UTF-32)");
				}
			}
			else if (codePage <= 20127)
			{
				if (codePage == 12001)
				{
					return Environment.GetResourceString("Unicode (UTF-32 Big-Endian)");
				}
				if (codePage == 20127)
				{
					return Environment.GetResourceString("US-ASCII");
				}
			}
			else
			{
				if (codePage == 65000)
				{
					return Environment.GetResourceString("Unicode (UTF-7)");
				}
				if (codePage == 65001)
				{
					return Environment.GetResourceString("Unicode (UTF-8)");
				}
			}
			return codePage.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001823 RID: 6179 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal static bool IsWindows8OrAbove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x0005CAF8 File Offset: 0x0005ACF8
		public static string CommandLine
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text in Environment.GetCommandLineArgs())
				{
					bool flag = false;
					string text2 = "";
					string text3 = text;
					for (int j = 0; j < text3.Length; j++)
					{
						if (text2.Length == 0 && char.IsWhiteSpace(text3[j]))
						{
							text2 = "\"";
						}
						else if (text3[j] == '"')
						{
							flag = true;
						}
					}
					if (flag && text2.Length != 0)
					{
						text3 = text3.Replace("\"", "\\\"");
					}
					stringBuilder.AppendFormat("{0}{1}{0} ", text2, text3);
				}
				if (stringBuilder.Length > 0)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					int i = stringBuilder2.Length;
					stringBuilder2.Length = i - 1;
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001825 RID: 6181 RVA: 0x0005CBC8 File Offset: 0x0005ADC8
		// (set) Token: 0x06001826 RID: 6182 RVA: 0x0005CBCF File Offset: 0x0005ADCF
		public static string CurrentDirectory
		{
			get
			{
				return Directory.InsecureGetCurrentDirectory();
			}
			set
			{
				Directory.InsecureSetCurrentDirectory(value);
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001827 RID: 6183 RVA: 0x0005CBD7 File Offset: 0x0005ADD7
		public static int CurrentManagedThreadId
		{
			get
			{
				return Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001828 RID: 6184
		// (set) Token: 0x06001829 RID: 6185
		public static extern int ExitCode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600182A RID: 6186
		public static extern bool HasShutdownStarted { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600182B RID: 6187
		public static extern string MachineName { [EnvironmentPermission(SecurityAction.Demand, Read = "COMPUTERNAME")] [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x0600182C RID: 6188
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetNewLine();

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600182D RID: 6189 RVA: 0x0005CBE3 File Offset: 0x0005ADE3
		public static string NewLine
		{
			get
			{
				if (Environment.nl != null)
				{
					return Environment.nl;
				}
				Environment.nl = Environment.GetNewLine();
				return Environment.nl;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600182E RID: 6190
		internal static extern PlatformID Platform { [CompilerGenerated] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x0600182F RID: 6191
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetOSVersionString();

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001830 RID: 6192 RVA: 0x0005CC04 File Offset: 0x0005AE04
		public static OperatingSystem OSVersion
		{
			get
			{
				if (Environment.os == null)
				{
					Version version = Environment.CreateVersionFromString(Environment.GetOSVersionString());
					PlatformID platformID = Environment.Platform;
					if (platformID == PlatformID.MacOSX)
					{
						platformID = PlatformID.Unix;
					}
					Environment.os = new OperatingSystem(platformID, version);
				}
				return Environment.os;
			}
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0005CC40 File Offset: 0x0005AE40
		internal static Version CreateVersionFromString(string info)
		{
			int major = 0;
			int minor = 0;
			int build = 0;
			int revision = 0;
			int num = 1;
			int num2 = -1;
			if (info == null)
			{
				return new Version(0, 0, 0, 0);
			}
			foreach (char c in info)
			{
				if (char.IsDigit(c))
				{
					if (num2 < 0)
					{
						num2 = (int)(c - '0');
					}
					else
					{
						num2 = num2 * 10 + (int)(c - '0');
					}
				}
				else if (num2 >= 0)
				{
					switch (num)
					{
					case 1:
						major = num2;
						break;
					case 2:
						minor = num2;
						break;
					case 3:
						build = num2;
						break;
					case 4:
						revision = num2;
						break;
					}
					num2 = -1;
					num++;
				}
				if (num == 5)
				{
					break;
				}
			}
			if (num2 >= 0)
			{
				switch (num)
				{
				case 1:
					major = num2;
					break;
				case 2:
					minor = num2;
					break;
				case 3:
					build = num2;
					break;
				case 4:
					revision = num2;
					break;
				}
			}
			return new Version(major, minor, build, revision);
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001832 RID: 6194 RVA: 0x0005CD28 File Offset: 0x0005AF28
		public static string StackTrace
		{
			[EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
			get
			{
				return new StackTrace(0, true).ToString();
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001833 RID: 6195 RVA: 0x0005CD36 File Offset: 0x0005AF36
		public static string SystemDirectory
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.System);
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001834 RID: 6196
		public static extern int TickCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001835 RID: 6197 RVA: 0x0005CD3F File Offset: 0x0005AF3F
		public static string UserDomainName
		{
			[EnvironmentPermission(SecurityAction.Demand, Read = "USERDOMAINNAME")]
			get
			{
				return Environment.MachineName;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001836 RID: 6198 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO("Currently always returns false, regardless of interactive state")]
		public static bool UserInteractive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001837 RID: 6199
		public static extern string UserName { [EnvironmentPermission(SecurityAction.Demand, Read = "USERNAME;USER")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001838 RID: 6200 RVA: 0x0005CD46 File Offset: 0x0005AF46
		public static Version Version
		{
			get
			{
				return new Version("4.0.30319.42000");
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x0005CD52 File Offset: 0x0005AF52
		[MonoTODO("Currently always returns zero")]
		public static long WorkingSet
		{
			[EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
			get
			{
				return 0L;
			}
		}

		// Token: 0x0600183A RID: 6202
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Exit(int exitCode);

		// Token: 0x0600183B RID: 6203 RVA: 0x0005CD56 File Offset: 0x0005AF56
		internal static void _Exit(int exitCode)
		{
			Environment.Exit(exitCode);
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0005CD60 File Offset: 0x0005AF60
		public static string ExpandEnvironmentVariables(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			int num = name.IndexOf('%');
			if (num == -1)
			{
				return name;
			}
			int length = name.Length;
			int num2;
			if (num == length - 1 || (num2 = name.IndexOf('%', num + 1)) == -1)
			{
				return name;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(name, 0, num);
			Hashtable hashtable = null;
			do
			{
				string text = name.Substring(num + 1, num2 - num - 1);
				string text2 = Environment.GetEnvironmentVariable(text);
				if (text2 == null && Environment.IsRunningOnWindows)
				{
					if (hashtable == null)
					{
						hashtable = Environment.GetEnvironmentVariablesNoCase();
					}
					text2 = (hashtable[text] as string);
				}
				int num3 = num2;
				if (text2 == null)
				{
					stringBuilder.Append('%');
					stringBuilder.Append(text);
					num2--;
				}
				else
				{
					stringBuilder.Append(text2);
				}
				int num4 = num2;
				num = name.IndexOf('%', num2 + 1);
				num2 = ((num == -1 || num2 > length - 1) ? -1 : name.IndexOf('%', num + 1));
				int count;
				if (num == -1 || num2 == -1)
				{
					count = length - num4 - 1;
				}
				else if (text2 != null)
				{
					count = num - num4 - 1;
				}
				else
				{
					count = num - num3;
				}
				if (num >= num4 || num == -1)
				{
					stringBuilder.Append(name, num4 + 1, count);
				}
			}
			while (num2 > -1 && num2 < length);
			return stringBuilder.ToString();
		}

		// Token: 0x0600183D RID: 6205
		[EnvironmentPermission(SecurityAction.Demand, Read = "PATH")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetCommandLineArgs();

		// Token: 0x0600183E RID: 6206
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string internalGetEnvironmentVariable_native(IntPtr variable);

		// Token: 0x0600183F RID: 6207 RVA: 0x0005CE9C File Offset: 0x0005B09C
		internal static string internalGetEnvironmentVariable(string variable)
		{
			if (variable == null)
			{
				return null;
			}
			string result;
			using (SafeStringMarshal safeStringMarshal = RuntimeMarshal.MarshalString(variable))
			{
				result = Environment.internalGetEnvironmentVariable_native(safeStringMarshal.Value);
			}
			return result;
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0005CEE4 File Offset: 0x0005B0E4
		public static string GetEnvironmentVariable(string variable)
		{
			return Environment.internalGetEnvironmentVariable(variable);
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0005CEEC File Offset: 0x0005B0EC
		private static Hashtable GetEnvironmentVariablesNoCase()
		{
			Hashtable hashtable = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
			foreach (string text in Environment.GetEnvironmentVariableNames())
			{
				hashtable[text] = Environment.internalGetEnvironmentVariable(text);
			}
			return hashtable;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0005CF30 File Offset: 0x0005B130
		public static IDictionary GetEnvironmentVariables()
		{
			StringBuilder stringBuilder = null;
			if (SecurityManager.SecurityEnabled)
			{
				stringBuilder = new StringBuilder();
			}
			Hashtable hashtable = new Hashtable();
			foreach (string text in Environment.GetEnvironmentVariableNames())
			{
				hashtable[text] = Environment.internalGetEnvironmentVariable(text);
				if (stringBuilder != null)
				{
					stringBuilder.Append(text);
					stringBuilder.Append(";");
				}
			}
			if (stringBuilder != null)
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, stringBuilder.ToString()).Demand();
			}
			return hashtable;
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0005CFA7 File Offset: 0x0005B1A7
		public static string GetFolderPath(Environment.SpecialFolder folder)
		{
			return Environment.GetFolderPath(folder, Environment.SpecialFolderOption.None);
		}

		// Token: 0x06001844 RID: 6212
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetWindowsFolderPath(int folder);

		// Token: 0x06001845 RID: 6213 RVA: 0x0005CFB0 File Offset: 0x0005B1B0
		public static string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
		{
			string result;
			if (Environment.IsRunningOnWindows)
			{
				result = Environment.GetWindowsFolderPath((int)folder);
			}
			else
			{
				result = Environment.UnixGetFolderPath(folder, option);
			}
			return result;
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x0005CFD8 File Offset: 0x0005B1D8
		private static string ReadXdgUserDir(string config_dir, string home_dir, string key, string fallback)
		{
			string text = Environment.internalGetEnvironmentVariable(key);
			if (text != null && text != string.Empty)
			{
				return text;
			}
			string path = Path.Combine(config_dir, "user-dirs.dirs");
			if (!File.Exists(path))
			{
				return Path.Combine(home_dir, fallback);
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(path))
				{
					string text2;
					while ((text2 = streamReader.ReadLine()) != null)
					{
						text2 = text2.Trim();
						int num = text2.IndexOf('=');
						if (num > 8 && text2.Substring(0, num) == key)
						{
							string text3 = text2.Substring(num + 1).Trim('"');
							bool flag = false;
							if (text3.StartsWithOrdinalUnchecked("$HOME/"))
							{
								flag = true;
								text3 = text3.Substring(6);
							}
							else if (!text3.StartsWithOrdinalUnchecked("/"))
							{
								flag = true;
							}
							return flag ? Path.Combine(home_dir, text3) : text3;
						}
					}
				}
			}
			catch
			{
			}
			return Path.Combine(home_dir, fallback);
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0005D0E8 File Offset: 0x0005B2E8
		internal static string UnixGetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
		{
			string text = Environment.internalGetHome();
			string text2 = Environment.internalGetEnvironmentVariable("XDG_DATA_HOME");
			if (text2 == null || text2 == string.Empty)
			{
				text2 = Path.Combine(text, ".local");
				text2 = Path.Combine(text2, "share");
			}
			string text3 = Environment.internalGetEnvironmentVariable("XDG_CONFIG_HOME");
			if (text3 == null || text3 == string.Empty)
			{
				text3 = Path.Combine(text, ".config");
			}
			switch (folder)
			{
			case Environment.SpecialFolder.Desktop:
			case Environment.SpecialFolder.DesktopDirectory:
				return Environment.ReadXdgUserDir(text3, text, "XDG_DESKTOP_DIR", "Desktop");
			case Environment.SpecialFolder.Programs:
			case Environment.SpecialFolder.Startup:
			case Environment.SpecialFolder.Recent:
			case Environment.SpecialFolder.SendTo:
			case Environment.SpecialFolder.StartMenu:
			case Environment.SpecialFolder.NetworkShortcuts:
			case Environment.SpecialFolder.CommonStartMenu:
			case Environment.SpecialFolder.CommonPrograms:
			case Environment.SpecialFolder.CommonStartup:
			case Environment.SpecialFolder.CommonDesktopDirectory:
			case Environment.SpecialFolder.PrinterShortcuts:
			case Environment.SpecialFolder.Cookies:
			case Environment.SpecialFolder.History:
			case Environment.SpecialFolder.Windows:
			case Environment.SpecialFolder.System:
			case Environment.SpecialFolder.SystemX86:
			case Environment.SpecialFolder.ProgramFilesX86:
			case Environment.SpecialFolder.CommonProgramFiles:
			case Environment.SpecialFolder.CommonProgramFilesX86:
			case Environment.SpecialFolder.CommonDocuments:
			case Environment.SpecialFolder.CommonAdminTools:
			case Environment.SpecialFolder.AdminTools:
			case Environment.SpecialFolder.CommonMusic:
			case Environment.SpecialFolder.CommonPictures:
			case Environment.SpecialFolder.CommonVideos:
			case Environment.SpecialFolder.Resources:
			case Environment.SpecialFolder.LocalizedResources:
			case Environment.SpecialFolder.CommonOemLinks:
			case Environment.SpecialFolder.CDBurning:
				return string.Empty;
			case Environment.SpecialFolder.MyDocuments:
				return text;
			case Environment.SpecialFolder.Favorites:
				if (Environment.Platform == PlatformID.MacOSX)
				{
					return Path.Combine(text, "Library", "Favorites");
				}
				return string.Empty;
			case Environment.SpecialFolder.MyMusic:
				if (Environment.Platform == PlatformID.MacOSX)
				{
					return Path.Combine(text, "Music");
				}
				return Environment.ReadXdgUserDir(text3, text, "XDG_MUSIC_DIR", "Music");
			case Environment.SpecialFolder.MyVideos:
				return Environment.ReadXdgUserDir(text3, text, "XDG_VIDEOS_DIR", "Videos");
			case Environment.SpecialFolder.MyComputer:
				return string.Empty;
			case Environment.SpecialFolder.Fonts:
				if (Environment.Platform == PlatformID.MacOSX)
				{
					return Path.Combine(text, "Library", "Fonts");
				}
				return Path.Combine(text, ".fonts");
			case Environment.SpecialFolder.Templates:
				return Environment.ReadXdgUserDir(text3, text, "XDG_TEMPLATES_DIR", "Templates");
			case Environment.SpecialFolder.ApplicationData:
				return text3;
			case Environment.SpecialFolder.LocalApplicationData:
				return text2;
			case Environment.SpecialFolder.InternetCache:
				if (Environment.Platform == PlatformID.MacOSX)
				{
					return Path.Combine(text, "Library", "Caches");
				}
				return string.Empty;
			case Environment.SpecialFolder.CommonApplicationData:
				return "/usr/share";
			case Environment.SpecialFolder.ProgramFiles:
				if (Environment.Platform == PlatformID.MacOSX)
				{
					return "/Applications";
				}
				return string.Empty;
			case Environment.SpecialFolder.MyPictures:
				if (Environment.Platform == PlatformID.MacOSX)
				{
					return Path.Combine(text, "Pictures");
				}
				return Environment.ReadXdgUserDir(text3, text, "XDG_PICTURES_DIR", "Pictures");
			case Environment.SpecialFolder.UserProfile:
				return text;
			case Environment.SpecialFolder.CommonTemplates:
				return "/usr/share/templates";
			}
			throw new ArgumentException("Invalid SpecialFolder");
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x0005D373 File Offset: 0x0005B573
		[EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
		public static string[] GetLogicalDrives()
		{
			return Environment.GetLogicalDrivesInternal();
		}

		// Token: 0x06001849 RID: 6217
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void internalBroadcastSettingChange();

		// Token: 0x0600184A RID: 6218 RVA: 0x0005D37C File Offset: 0x0005B57C
		public static string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
		{
			switch (target)
			{
			case EnvironmentVariableTarget.Process:
				return Environment.GetEnvironmentVariable(variable);
			case EnvironmentVariableTarget.User:
				break;
			case EnvironmentVariableTarget.Machine:
				new EnvironmentPermission(PermissionState.Unrestricted).Demand();
				if (!Environment.IsRunningOnWindows)
				{
					return null;
				}
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment"))
				{
					object value = registryKey.GetValue(variable);
					return (value == null) ? null : value.ToString();
				}
				break;
			default:
				goto IL_AC;
			}
			new EnvironmentPermission(PermissionState.Unrestricted).Demand();
			if (!Environment.IsRunningOnWindows)
			{
				return null;
			}
			using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Environment", false))
			{
				object value2 = registryKey2.GetValue(variable);
				return (value2 == null) ? null : value2.ToString();
			}
			IL_AC:
			throw new ArgumentException("target");
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0005D460 File Offset: 0x0005B660
		public static IDictionary GetEnvironmentVariables(EnvironmentVariableTarget target)
		{
			IDictionary dictionary = new Hashtable();
			switch (target)
			{
			case EnvironmentVariableTarget.Process:
				return Environment.GetEnvironmentVariables();
			case EnvironmentVariableTarget.User:
				break;
			case EnvironmentVariableTarget.Machine:
				new EnvironmentPermission(PermissionState.Unrestricted).Demand();
				if (!Environment.IsRunningOnWindows)
				{
					return dictionary;
				}
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment"))
				{
					foreach (string text in registryKey.GetValueNames())
					{
						dictionary.Add(text, registryKey.GetValue(text));
					}
					return dictionary;
				}
				break;
			default:
				goto IL_E0;
			}
			new EnvironmentPermission(PermissionState.Unrestricted).Demand();
			if (!Environment.IsRunningOnWindows)
			{
				return dictionary;
			}
			using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Environment"))
			{
				foreach (string text2 in registryKey2.GetValueNames())
				{
					dictionary.Add(text2, registryKey2.GetValue(text2));
				}
				return dictionary;
			}
			IL_E0:
			throw new ArgumentException("target");
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0005D578 File Offset: 0x0005B778
		[EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
		public static void SetEnvironmentVariable(string variable, string value)
		{
			Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.Process);
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0005D584 File Offset: 0x0005B784
		[EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
		public static void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			if (variable == string.Empty)
			{
				throw new ArgumentException("String cannot be of zero length.", "variable");
			}
			if (variable.IndexOf('=') != -1)
			{
				throw new ArgumentException("Environment variable name cannot contain an equal character.", "variable");
			}
			if (variable[0] == '\0')
			{
				throw new ArgumentException("The first char in the string is the null character.", "variable");
			}
			switch (target)
			{
			case EnvironmentVariableTarget.Process:
				Environment.InternalSetEnvironmentVariable(variable, value);
				return;
			case EnvironmentVariableTarget.User:
				break;
			case EnvironmentVariableTarget.Machine:
				if (!Environment.IsRunningOnWindows)
				{
					return;
				}
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment", true))
				{
					if (string.IsNullOrEmpty(value))
					{
						registryKey.DeleteValue(variable, false);
					}
					else
					{
						registryKey.SetValue(variable, value);
					}
					Environment.internalBroadcastSettingChange();
					return;
				}
				break;
			default:
				goto IL_106;
			}
			if (!Environment.IsRunningOnWindows)
			{
				return;
			}
			using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Environment", true))
			{
				if (string.IsNullOrEmpty(value))
				{
					registryKey2.DeleteValue(variable, false);
				}
				else
				{
					registryKey2.SetValue(variable, value);
				}
				Environment.internalBroadcastSettingChange();
				return;
			}
			IL_106:
			throw new ArgumentException("target");
		}

		// Token: 0x0600184E RID: 6222
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void InternalSetEnvironmentVariable(char* variable, int variable_length, char* value, int value_length);

		// Token: 0x0600184F RID: 6223 RVA: 0x0005D6C0 File Offset: 0x0005B8C0
		internal unsafe static void InternalSetEnvironmentVariable(string variable, string value)
		{
			fixed (string text = variable)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				fixed (string text2 = value)
				{
					char* ptr2 = text2;
					if (ptr2 != null)
					{
						ptr2 += RuntimeHelpers.OffsetToStringData / 2;
					}
					Environment.InternalSetEnvironmentVariable(ptr, (variable != null) ? variable.Length : 0, ptr2, (value != null) ? value.Length : 0);
				}
			}
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0005D710 File Offset: 0x0005B910
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void FailFast(string message)
		{
			Environment.FailFast(message, null, null);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0005D710 File Offset: 0x0005B910
		internal static void FailFast(string message, uint exitCode)
		{
			Environment.FailFast(message, null, null);
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0005D71A File Offset: 0x0005B91A
		[SecurityCritical]
		public static void FailFast(string message, Exception exception)
		{
			Environment.FailFast(message, exception, null);
		}

		// Token: 0x06001853 RID: 6227
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void FailFast(string message, Exception exception, string errorSource);

		// Token: 0x06001854 RID: 6228
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIs64BitOperatingSystem();

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001855 RID: 6229 RVA: 0x0005D724 File Offset: 0x0005B924
		public static bool Is64BitOperatingSystem
		{
			get
			{
				return Environment.GetIs64BitOperatingSystem();
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001856 RID: 6230 RVA: 0x0005D72B File Offset: 0x0005B92B
		public static int SystemPageSize
		{
			get
			{
				return Environment.GetPageSize();
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06001857 RID: 6231 RVA: 0x0005D732 File Offset: 0x0005B932
		public static bool Is64BitProcess
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06001858 RID: 6232
		public static extern int ProcessorCount { [EnvironmentPermission(SecurityAction.Demand, Read = "NUMBER_OF_PROCESSORS")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06001859 RID: 6233 RVA: 0x0005D73C File Offset: 0x0005B93C
		internal static bool IsRunningOnWindows
		{
			get
			{
				return Environment.Platform < PlatformID.Unix;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x0005D748 File Offset: 0x0005B948
		private static string GacPath
		{
			get
			{
				if (Environment.IsRunningOnWindows)
				{
					return Path.Combine(Path.Combine(new DirectoryInfo(Path.GetDirectoryName(typeof(int).Assembly.Location)).Parent.Parent.FullName, "mono"), "gac");
				}
				return Path.Combine(Path.Combine(Environment.internalGetGacPath(), "mono"), "gac");
			}
		}

		// Token: 0x0600185B RID: 6235
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string internalGetGacPath();

		// Token: 0x0600185C RID: 6236
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string[] GetLogicalDrivesInternal();

		// Token: 0x0600185D RID: 6237
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string[] GetEnvironmentVariableNames();

		// Token: 0x0600185E RID: 6238
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetMachineConfigPath();

		// Token: 0x0600185F RID: 6239
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string internalGetHome();

		// Token: 0x06001860 RID: 6240
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetPageSize();

		// Token: 0x06001861 RID: 6241
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string get_bundled_machine_config();

		// Token: 0x06001862 RID: 6242 RVA: 0x0005D7B7 File Offset: 0x0005B9B7
		internal static string GetBundledMachineConfig()
		{
			return Environment.get_bundled_machine_config();
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x0005D7C0 File Offset: 0x0005B9C0
		internal static bool IsUnix
		{
			get
			{
				int platform = (int)Environment.Platform;
				return platform == 4 || platform == 128 || platform == 6;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001864 RID: 6244 RVA: 0x0005D7E5 File Offset: 0x0005B9E5
		internal static bool IsMacOS
		{
			get
			{
				return Environment.Platform == PlatformID.MacOSX;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal static bool IsCLRHosted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal static void TriggerCodeContractFailure(ContractFailureKind failureKind, string message, string condition, string exceptionAsString)
		{
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x0005D7F0 File Offset: 0x0005B9F0
		internal static string GetStackTrace(Exception e, bool needFileInfo)
		{
			StackTrace stackTrace;
			if (e == null)
			{
				stackTrace = new StackTrace(needFileInfo);
			}
			else
			{
				stackTrace = new StackTrace(e, needFileInfo);
			}
			return stackTrace.ToString(System.Diagnostics.StackTrace.TraceFormat.Normal);
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001868 RID: 6248 RVA: 0x000040F7 File Offset: 0x000022F7
		internal static bool IsWinRTSupported
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001678 RID: 5752
		private const string mono_corlib_version = "1A5E0066-58DC-428A-B21C-0AD6CDAE2789";

		// Token: 0x04001679 RID: 5753
		private static string nl;

		// Token: 0x0400167A RID: 5754
		private static OperatingSystem os;

		// Token: 0x0200021D RID: 541
		[ComVisible(true)]
		public enum SpecialFolder
		{
			// Token: 0x0400167D RID: 5757
			MyDocuments = 5,
			// Token: 0x0400167E RID: 5758
			Desktop = 0,
			// Token: 0x0400167F RID: 5759
			MyComputer = 17,
			// Token: 0x04001680 RID: 5760
			Programs = 2,
			// Token: 0x04001681 RID: 5761
			Personal = 5,
			// Token: 0x04001682 RID: 5762
			Favorites,
			// Token: 0x04001683 RID: 5763
			Startup,
			// Token: 0x04001684 RID: 5764
			Recent,
			// Token: 0x04001685 RID: 5765
			SendTo,
			// Token: 0x04001686 RID: 5766
			StartMenu = 11,
			// Token: 0x04001687 RID: 5767
			MyMusic = 13,
			// Token: 0x04001688 RID: 5768
			DesktopDirectory = 16,
			// Token: 0x04001689 RID: 5769
			Templates = 21,
			// Token: 0x0400168A RID: 5770
			ApplicationData = 26,
			// Token: 0x0400168B RID: 5771
			LocalApplicationData = 28,
			// Token: 0x0400168C RID: 5772
			InternetCache = 32,
			// Token: 0x0400168D RID: 5773
			Cookies,
			// Token: 0x0400168E RID: 5774
			History,
			// Token: 0x0400168F RID: 5775
			CommonApplicationData,
			// Token: 0x04001690 RID: 5776
			System = 37,
			// Token: 0x04001691 RID: 5777
			ProgramFiles,
			// Token: 0x04001692 RID: 5778
			MyPictures,
			// Token: 0x04001693 RID: 5779
			CommonProgramFiles = 43,
			// Token: 0x04001694 RID: 5780
			MyVideos = 14,
			// Token: 0x04001695 RID: 5781
			NetworkShortcuts = 19,
			// Token: 0x04001696 RID: 5782
			Fonts,
			// Token: 0x04001697 RID: 5783
			CommonStartMenu = 22,
			// Token: 0x04001698 RID: 5784
			CommonPrograms,
			// Token: 0x04001699 RID: 5785
			CommonStartup,
			// Token: 0x0400169A RID: 5786
			CommonDesktopDirectory,
			// Token: 0x0400169B RID: 5787
			PrinterShortcuts = 27,
			// Token: 0x0400169C RID: 5788
			Windows = 36,
			// Token: 0x0400169D RID: 5789
			UserProfile = 40,
			// Token: 0x0400169E RID: 5790
			SystemX86,
			// Token: 0x0400169F RID: 5791
			ProgramFilesX86,
			// Token: 0x040016A0 RID: 5792
			CommonProgramFilesX86 = 44,
			// Token: 0x040016A1 RID: 5793
			CommonTemplates,
			// Token: 0x040016A2 RID: 5794
			CommonDocuments,
			// Token: 0x040016A3 RID: 5795
			CommonAdminTools,
			// Token: 0x040016A4 RID: 5796
			AdminTools,
			// Token: 0x040016A5 RID: 5797
			CommonMusic = 53,
			// Token: 0x040016A6 RID: 5798
			CommonPictures,
			// Token: 0x040016A7 RID: 5799
			CommonVideos,
			// Token: 0x040016A8 RID: 5800
			Resources,
			// Token: 0x040016A9 RID: 5801
			LocalizedResources,
			// Token: 0x040016AA RID: 5802
			CommonOemLinks,
			// Token: 0x040016AB RID: 5803
			CDBurning
		}

		// Token: 0x0200021E RID: 542
		public enum SpecialFolderOption
		{
			// Token: 0x040016AD RID: 5805
			None,
			// Token: 0x040016AE RID: 5806
			DoNotVerify = 16384,
			// Token: 0x040016AF RID: 5807
			Create = 32768
		}
	}
}
