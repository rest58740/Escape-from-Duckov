using System;
using System.Collections.Generic;
using System.IO;

namespace Sirenix.Utilities
{
	// Token: 0x02000031 RID: 49
	public static class SirenixAssetPaths
	{
		// Token: 0x06000230 RID: 560 RVA: 0x0000CFC4 File Offset: 0x0000B1C4
		static SirenixAssetPaths()
		{
			SirenixAssetPaths.SirenixPluginPath = "Assets/Plugins/Sirenix/";
			SirenixAssetPaths.OdinPath = SirenixAssetPaths.SirenixPluginPath + "Odin Inspector/";
			SirenixAssetPaths.SirenixAssetsPath = SirenixAssetPaths.SirenixPluginPath + "Assets/";
			SirenixAssetPaths.SirenixAssembliesPath = SirenixAssetPaths.SirenixPluginPath + "Assemblies/";
			SirenixAssetPaths.OdinResourcesPath = SirenixAssetPaths.OdinPath + "Config/Resources/Sirenix/";
			SirenixAssetPaths.OdinEditorConfigsPath = SirenixAssetPaths.OdinPath + "Config/Editor/";
			SirenixAssetPaths.OdinResourcesConfigsPath = SirenixAssetPaths.OdinResourcesPath;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000D064 File Offset: 0x0000B264
		private static string ReplaceIllegalCharacters(string source, char replacement)
		{
			char[] array = source.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (SirenixAssetPaths.IllegalChars.Contains(array[i]))
				{
					array[i] = replacement;
				}
			}
			return new string(array);
		}

		// Token: 0x04000069 RID: 105
		public const string DefaultSirenixPluginPath = "Assets/Plugins/Sirenix/";

		// Token: 0x0400006A RID: 106
		public const string SirenixAssetPathsSOGuid = "08379ccefc05200459f90a1c0711a340";

		// Token: 0x0400006B RID: 107
		public const string LookupAssetName = "OdinPathLookup.asset";

		// Token: 0x0400006C RID: 108
		public static readonly string OdinPath;

		// Token: 0x0400006D RID: 109
		public static readonly string SirenixAssetsPath;

		// Token: 0x0400006E RID: 110
		public static readonly string SirenixPluginPath;

		// Token: 0x0400006F RID: 111
		public static readonly string SirenixAssembliesPath;

		// Token: 0x04000070 RID: 112
		public static readonly string OdinResourcesPath;

		// Token: 0x04000071 RID: 113
		public static readonly string OdinEditorConfigsPath;

		// Token: 0x04000072 RID: 114
		public static readonly string OdinResourcesConfigsPath;

		// Token: 0x04000073 RID: 115
		public static readonly string OdinTempPath;

		// Token: 0x04000074 RID: 116
		private static readonly HashSet<char> IllegalChars = new HashSet<char>(Path.GetInvalidFileNameChars())
		{
			'.'
		};
	}
}
