using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference.Utility;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace Eflatun.SceneReference
{
	// Token: 0x02000009 RID: 9
	[PublicAPI]
	public static class SceneGuidToPathMapProvider
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000223A File Offset: 0x0000043A
		public static IReadOnlyDictionary<string, string> SceneGuidToPathMap
		{
			get
			{
				return SceneGuidToPathMapProvider.GetSceneGuidToPathMap(true);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002242 File Offset: 0x00000442
		public static IReadOnlyDictionary<string, string> ScenePathToGuidMap
		{
			get
			{
				return SceneGuidToPathMapProvider.GetScenePathToGuidMap(true);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000224A File Offset: 0x0000044A
		[Preserve]
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RuntimeInit()
		{
			SceneGuidToPathMapProvider.LoadIfNotAlready(true);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002252 File Offset: 0x00000452
		internal static IReadOnlyDictionary<string, string> GetSceneGuidToPathMap(bool errorIfMissingDuringLoad)
		{
			SceneGuidToPathMapProvider.LoadIfNotAlready(errorIfMissingDuringLoad);
			return SceneGuidToPathMapProvider._sceneGuidToPathMap;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000225F File Offset: 0x0000045F
		internal static IReadOnlyDictionary<string, string> GetScenePathToGuidMap(bool errorIfMissingDuringLoad)
		{
			SceneGuidToPathMapProvider.LoadIfNotAlready(errorIfMissingDuringLoad);
			return SceneGuidToPathMapProvider._scenePathToGuidMap;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000226C File Offset: 0x0000046C
		internal static void DirectAssign(Dictionary<string, string> sceneGuidToPathMap)
		{
			SceneGuidToPathMapProvider.FillWith(sceneGuidToPathMap);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002274 File Offset: 0x00000474
		private static void LoadIfNotAlready(bool errorIfMissing)
		{
			if (SceneGuidToPathMapProvider._sceneGuidToPathMap == null)
			{
				SceneGuidToPathMapProvider.Load(errorIfMissing);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002284 File Offset: 0x00000484
		private static void Load(bool errorIfMissing)
		{
			TextAsset textAsset = Resources.Load<TextAsset>(Paths.RelativeToResources.SceneGuidToPathMapFile.UnixPath.WithoutExtension());
			if (textAsset == null)
			{
				if (errorIfMissing)
				{
					Logger.Error("Scene GUID to path map file not found!");
				}
				return;
			}
			SceneGuidToPathMapProvider.FillWith(JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text));
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022D0 File Offset: 0x000004D0
		private static void FillWith(Dictionary<string, string> sceneGuidToPathMap)
		{
			SceneGuidToPathMapProvider._sceneGuidToPathMap = sceneGuidToPathMap;
			SceneGuidToPathMapProvider._scenePathToGuidMap = sceneGuidToPathMap.ToDictionary((KeyValuePair<string, string> x) => x.Value, (KeyValuePair<string, string> x) => x.Key);
		}

		// Token: 0x04000012 RID: 18
		private static Dictionary<string, string> _sceneGuidToPathMap;

		// Token: 0x04000013 RID: 19
		private static Dictionary<string, string> _scenePathToGuidMap;
	}
}
