using System;
using System.IO;
using Eflatun.SceneReference.Utility;
using UnityEngine;

namespace Eflatun.SceneReference
{
	// Token: 0x02000007 RID: 7
	internal static class Paths
	{
		// Token: 0x0400000F RID: 15
		private static readonly ConvertedPath AssetsFolder = new ConvertedPath(Application.dataPath);

		// Token: 0x04000010 RID: 16
		private static readonly ConvertedPath ResourcesFolder = new ConvertedPath(Path.Combine(Paths.AssetsFolder.GivenPath, "Resources"));

		// Token: 0x0200001C RID: 28
		public static class RelativeToResources
		{
			// Token: 0x04000038 RID: 56
			public static readonly ConvertedPath SceneDataMapsFolder = new ConvertedPath(Path.Combine("Eflatun", "SceneReference"));

			// Token: 0x04000039 RID: 57
			public static readonly ConvertedPath SceneGuidToPathMapFile = new ConvertedPath(Path.Combine(Paths.RelativeToResources.SceneDataMapsFolder.GivenPath, "SceneGuidToPathMap.generated.json"));

			// Token: 0x0400003A RID: 58
			public static readonly ConvertedPath SceneDataMapsDotKeepFile = new ConvertedPath(Path.Combine(Paths.RelativeToResources.SceneDataMapsFolder.GivenPath, ".keep"));

			// Token: 0x0400003B RID: 59
			public static readonly ConvertedPath SceneGuidToAddressMapFile = new ConvertedPath(Path.Combine(Paths.RelativeToResources.SceneDataMapsFolder.GivenPath, "SceneGuidToAddressMap.generated.json"));
		}

		// Token: 0x0200001D RID: 29
		public static class Absolute
		{
			// Token: 0x0400003C RID: 60
			public static readonly ConvertedPath SceneDataMapsFolder = new ConvertedPath(Path.Combine(Paths.ResourcesFolder.GivenPath, Paths.RelativeToResources.SceneDataMapsFolder.GivenPath));

			// Token: 0x0400003D RID: 61
			public static readonly ConvertedPath SceneGuidToPathMapFile = new ConvertedPath(Path.Combine(Paths.ResourcesFolder.GivenPath, Paths.RelativeToResources.SceneGuidToPathMapFile.GivenPath));

			// Token: 0x0400003E RID: 62
			public static readonly ConvertedPath SceneDataMapsDotKeepFile = new ConvertedPath(Path.Combine(Paths.ResourcesFolder.GivenPath, Paths.RelativeToResources.SceneDataMapsDotKeepFile.GivenPath));

			// Token: 0x0400003F RID: 63
			public static readonly ConvertedPath SceneGuidToAddressMapFile = new ConvertedPath(Path.Combine(Paths.ResourcesFolder.GivenPath, Paths.RelativeToResources.SceneGuidToAddressMapFile.GivenPath));
		}
	}
}
