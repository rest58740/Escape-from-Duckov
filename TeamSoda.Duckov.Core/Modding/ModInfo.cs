using System;
using System.IO;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x02000279 RID: 633
	[Serializable]
	public struct ModInfo
	{
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x0600140A RID: 5130 RVA: 0x0004B55F File Offset: 0x0004975F
		public string dllPath
		{
			get
			{
				return Path.Combine(this.path, this.name + ".dll");
			}
		}

		// Token: 0x04000EDF RID: 3807
		public string path;

		// Token: 0x04000EE0 RID: 3808
		public string name;

		// Token: 0x04000EE1 RID: 3809
		public string displayName;

		// Token: 0x04000EE2 RID: 3810
		public string description;

		// Token: 0x04000EE3 RID: 3811
		public string version;

		// Token: 0x04000EE4 RID: 3812
		public string tags;

		// Token: 0x04000EE5 RID: 3813
		public Texture2D preview;

		// Token: 0x04000EE6 RID: 3814
		public bool dllFound;

		// Token: 0x04000EE7 RID: 3815
		public bool isSteamItem;

		// Token: 0x04000EE8 RID: 3816
		public ulong publishedFileId;
	}
}
