using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference.Exceptions;
using Eflatun.SceneReference.Utility;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace Eflatun.SceneReference
{
	// Token: 0x02000008 RID: 8
	[PublicAPI]
	public static class SceneGuidToAddressMapProvider
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002125 File Offset: 0x00000325
		public static IReadOnlyDictionary<string, string> SceneGuidToAddressMap
		{
			get
			{
				SceneGuidToAddressMapProvider.LoadIfNotAlready();
				return SceneGuidToAddressMapProvider._sceneGuidToAddressMap;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002134 File Offset: 0x00000334
		public static string GetGuidFromAddress(string address)
		{
			SceneGuidToAddressMapProvider.LoadIfNotAlready();
			KeyValuePair<string, string>[] array = (from x in SceneGuidToAddressMapProvider._sceneGuidToAddressMap
			where x.Value == address
			select x).ToArray<KeyValuePair<string, string>>();
			if (array.Length < 1)
			{
				throw new AddressNotFoundException(address);
			}
			if (array.Length > 1)
			{
				throw new AddressNotUniqueException(address);
			}
			return array.First<KeyValuePair<string, string>>().Key;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021A0 File Offset: 0x000003A0
		[ContractAnnotation("=> true, guid:notnull; => false, guid:null")]
		public static bool TryGetGuidFromAddress(string address, out string guid)
		{
			bool result;
			try
			{
				guid = SceneGuidToAddressMapProvider.GetGuidFromAddress(address);
				result = true;
			}
			catch
			{
				guid = null;
				result = false;
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021D4 File Offset: 0x000003D4
		internal static void DirectAssign(Dictionary<string, string> sceneGuidToAddressMap)
		{
			SceneGuidToAddressMapProvider.FillWith(sceneGuidToAddressMap);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021DC File Offset: 0x000003DC
		[Preserve]
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void LoadIfNotAlready()
		{
			if (SceneGuidToAddressMapProvider._sceneGuidToAddressMap == null)
			{
				SceneGuidToAddressMapProvider.Load();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021EC File Offset: 0x000003EC
		private static void Load()
		{
			TextAsset textAsset = Resources.Load<TextAsset>(Paths.RelativeToResources.SceneGuidToAddressMapFile.UnixPath.WithoutExtension());
			if (textAsset == null)
			{
				Logger.Error("Scene GUID to address map file not found!");
				return;
			}
			SceneGuidToAddressMapProvider.FillWith(JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002232 File Offset: 0x00000432
		private static void FillWith(Dictionary<string, string> sceneGuidToAddressMap)
		{
			SceneGuidToAddressMapProvider._sceneGuidToAddressMap = sceneGuidToAddressMap;
		}

		// Token: 0x04000011 RID: 17
		private static Dictionary<string, string> _sceneGuidToAddressMap;
	}
}
