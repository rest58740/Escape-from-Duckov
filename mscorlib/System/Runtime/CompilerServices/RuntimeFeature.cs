using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000802 RID: 2050
	public static class RuntimeFeature
	{
		// Token: 0x06004617 RID: 17943 RVA: 0x000E5868 File Offset: 0x000E3A68
		public static bool IsSupported(string feature)
		{
			if (feature == "PortablePdb" || feature == "DefaultImplementationsOfInterfaces")
			{
				return true;
			}
			if (!(feature == "IsDynamicCodeSupported"))
			{
				return feature == "IsDynamicCodeCompiled" && RuntimeFeature.IsDynamicCodeCompiled;
			}
			return RuntimeFeature.IsDynamicCodeSupported;
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06004618 RID: 17944 RVA: 0x000040F7 File Offset: 0x000022F7
		public static bool IsDynamicCodeSupported
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06004619 RID: 17945 RVA: 0x000040F7 File Offset: 0x000022F7
		public static bool IsDynamicCodeCompiled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04002D3E RID: 11582
		public const string PortablePdb = "PortablePdb";

		// Token: 0x04002D3F RID: 11583
		public const string DefaultImplementationsOfInterfaces = "DefaultImplementationsOfInterfaces";
	}
}
