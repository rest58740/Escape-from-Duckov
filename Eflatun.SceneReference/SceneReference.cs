using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Eflatun.SceneReference.Exceptions;
using Eflatun.SceneReference.Utility;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Eflatun.SceneReference
{
	// Token: 0x0200000A RID: 10
	[PublicAPI]
	[XmlRoot("Eflatun.SceneReference.SceneReference")]
	[Serializable]
	public class SceneReference : ISerializationCallbackReceiver, ISerializable, IXmlSerializable
	{
		// Token: 0x06000017 RID: 23 RVA: 0x0000232C File Offset: 0x0000052C
		public SceneReference()
		{
			this.guid = "00000000000000000000000000000000";
			this.asset = null;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002348 File Offset: 0x00000548
		public SceneReference(string guid)
		{
			if (string.IsNullOrWhiteSpace(guid))
			{
				throw new SceneReferenceCreationException("Given GUID is null or whitespace. GUID: '" + guid + "'.\nTo fix this, make sure you provide the GUID of a valid scene.");
			}
			string text;
			if (!SceneGuidToPathMapProvider.SceneGuidToPathMap.TryGetValue(guid, out text))
			{
				throw new SceneReferenceCreationException("Given GUID is not found in the scene GUID to path map. GUID: '" + guid + "'\nThis can happen for these reasons:\n1. The asset with the given GUID either doesn't exist or is not a scene. To fix this, make sure you provide the GUID of a valid scene.\n2. The scene GUID to path map is outdated. To fix this, you can either manually run the generator, or enable generation triggers. It is highly recommended to keep all the generation triggers enabled.");
			}
			this.guid = guid;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023A8 File Offset: 0x000005A8
		protected SceneReference(SerializationInfo info, StreamingContext context)
		{
			string @string = info.GetString("sceneAssetGuidHex");
			this.FillWithDeserializedGuid(@string);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000023D0 File Offset: 0x000005D0
		public static SceneReference FromScenePath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				throw new SceneReferenceCreationException("Given path is null or whitespace. Path: '" + path + "'\nTo fix this, make sure you provide the path of a valid scene.");
			}
			string text;
			if (!SceneGuidToPathMapProvider.ScenePathToGuidMap.TryGetValue(path, out text))
			{
				throw new SceneReferenceCreationException("Given path is not found in the scene GUID to path map. Path: '" + path + "'\nThis can happen for these reasons:\n1. The asset at the given path either doesn't exist or is not a scene. To fix this, make sure you provide the path of a valid scene.\n2. The scene GUID to path map is outdated. To fix this, you can either manually run the generator, or enable generation triggers. It is highly recommended to keep all the generation triggers enabled.");
			}
			return new SceneReference(text);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002428 File Offset: 0x00000628
		public static SceneReference FromAddress(string address)
		{
			if (string.IsNullOrWhiteSpace(address))
			{
				throw new SceneReferenceCreationException("Given address is null or whitespace. Path: '" + address + "'\nTo fix this, make sure you provide the address of a valid addressable scene.");
			}
			SceneReference result;
			try
			{
				result = new SceneReference(SceneGuidToAddressMapProvider.GetGuidFromAddress(address));
			}
			catch (AddressNotFoundException inner)
			{
				throw new SceneReferenceCreationException("Given address is not found in the Scene GUID to Address Map. Address: " + address + ".\nThis can happen for these reasons:\n1. The asset with the given address either doesn't exist or is not a scene. To fix this, make sure you provide the address of a valid addressable scene.\n2. The Scene GUID to Address Map is outdated. To fix this, you can either manually run the generator, or enable generation triggers. It is highly recommended to keep all the generation triggers enabled.", inner);
			}
			catch (AddressNotUniqueException inner2)
			{
				throw new SceneReferenceCreationException("Given address matches multiple scenes in the Scene GUID to Address Map. Address: " + address + ".\nThrown if a given address matches multiple entries in the Scene GUID to Address Map. This can happen for these reasons:\n1. There are multiple addressable scenes with the same given address. To fix this, make sure there is only one addressable scene with the given address.\n2. The Scene GUID to Address Map is outdated. To fix this, you can either manually run the generator, or enable generation triggers. It is highly recommended to keep all the generation triggers enabled.", inner2);
			}
			catch (AddressablesSupportDisabledException exception)
			{
				throw SceneReferenceInternalException.ExceptionImpossible<AddressablesSupportDisabledException>("48302749", exception);
			}
			return result;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000024C8 File Offset: 0x000006C8
		private bool HasValue
		{
			get
			{
				if (!this.Guid.IsValidGuid())
				{
					throw SceneReferenceInternalException.InvalidGuid("54783205", this.Guid);
				}
				return this.Guid != "00000000000000000000000000000000";
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000024F8 File Offset: 0x000006F8
		public string Guid
		{
			get
			{
				return this.guid.GuardGuidAgainstNullOrWhitespace();
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002508 File Offset: 0x00000708
		public string Path
		{
			get
			{
				if (!this.HasValue)
				{
					throw new EmptySceneReferenceException();
				}
				string result;
				if (!SceneGuidToPathMapProvider.SceneGuidToPathMap.TryGetValue(this.Guid, out result))
				{
					throw new InvalidSceneReferenceException();
				}
				return result;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000253E File Offset: 0x0000073E
		public int BuildIndex
		{
			get
			{
				return SceneUtility.GetBuildIndexByScenePath(this.Path);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000254B File Offset: 0x0000074B
		public string Name
		{
			get
			{
				return System.IO.Path.GetFileNameWithoutExtension(this.Path);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002558 File Offset: 0x00000758
		public Scene LoadedScene
		{
			get
			{
				return SceneManager.GetSceneByPath(this.Path);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002568 File Offset: 0x00000768
		public string Address
		{
			get
			{
				if (!this.HasValue)
				{
					throw new EmptySceneReferenceException();
				}
				if (!SceneGuidToPathMapProvider.SceneGuidToPathMap.ContainsKey(this.Guid))
				{
					throw new InvalidSceneReferenceException();
				}
				string result;
				if (!SceneGuidToAddressMapProvider.SceneGuidToAddressMap.TryGetValue(this.Guid, out result))
				{
					throw new SceneNotAddressableException();
				}
				return result;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000025B8 File Offset: 0x000007B8
		public SceneReferenceState State
		{
			get
			{
				if (this.HasValue)
				{
					string scenePath;
					if (SceneGuidToPathMapProvider.SceneGuidToPathMap.TryGetValue(this.Guid, out scenePath) && SceneUtility.GetBuildIndexByScenePath(scenePath) != -1)
					{
						return SceneReferenceState.Regular;
					}
					if (SceneGuidToAddressMapProvider.SceneGuidToAddressMap.ContainsKey(this.Guid))
					{
						return SceneReferenceState.Addressable;
					}
				}
				return SceneReferenceState.Unsafe;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002604 File Offset: 0x00000804
		public SceneReferenceUnsafeReason UnsafeReason
		{
			get
			{
				if (!this.HasValue)
				{
					return SceneReferenceUnsafeReason.Empty;
				}
				string text;
				if (SceneGuidToAddressMapProvider.SceneGuidToAddressMap.TryGetValue(this.Guid, out text))
				{
					return SceneReferenceUnsafeReason.None;
				}
				string scenePath;
				if (!SceneGuidToPathMapProvider.SceneGuidToPathMap.TryGetValue(this.Guid, out scenePath))
				{
					return SceneReferenceUnsafeReason.NotInMaps;
				}
				if (SceneUtility.GetBuildIndexByScenePath(scenePath) == -1)
				{
					return SceneReferenceUnsafeReason.NotInBuild;
				}
				return SceneReferenceUnsafeReason.None;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002653 File Offset: 0x00000853
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			this.GetObjectData(info, context);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002660 File Offset: 0x00000860
		protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			string guidToSerialize = this.GetGuidToSerialize();
			info.AddValue("sceneAssetGuidHex", guidToSerialize);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002680 File Offset: 0x00000880
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.OnBeforeSerialize();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual void OnBeforeSerialize()
		{
			this.guid = this.guid.GuardGuidAgainstNullOrWhitespace();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000269B File Offset: 0x0000089B
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.OnAfterDeserialize();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000026A3 File Offset: 0x000008A3
		protected virtual void OnAfterDeserialize()
		{
			this.guid = this.guid.GuardGuidAgainstNullOrWhitespace();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000026B6 File Offset: 0x000008B6
		XmlSchema IXmlSerializable.GetSchema()
		{
			return this.GetSchema();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000026BE File Offset: 0x000008BE
		protected virtual XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000026C1 File Offset: 0x000008C1
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			this.ReadXml(reader);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000026CC File Offset: 0x000008CC
		protected virtual void ReadXml(XmlReader reader)
		{
			string deserializedGuid = reader.ReadString();
			this.FillWithDeserializedGuid(deserializedGuid);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000026E7 File Offset: 0x000008E7
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			this.WriteXml(writer);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000026F0 File Offset: 0x000008F0
		protected virtual void WriteXml(XmlWriter writer)
		{
			string guidToSerialize = this.GetGuidToSerialize();
			writer.WriteString(guidToSerialize);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000270B File Offset: 0x0000090B
		private string GetGuidToSerialize()
		{
			return this.guid.GuardGuidAgainstNullOrWhitespace();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002718 File Offset: 0x00000918
		private void FillWithDeserializedGuid(string deserializedGuid)
		{
			deserializedGuid = deserializedGuid.GuardGuidAgainstNullOrWhitespace();
			this.guid = deserializedGuid;
		}

		// Token: 0x04000014 RID: 20
		internal const string XmlRootElementName = "Eflatun.SceneReference.SceneReference";

		// Token: 0x04000015 RID: 21
		internal const string CustomSerializationGuidKey = "sceneAssetGuidHex";

		// Token: 0x04000016 RID: 22
		[FormerlySerializedAs("sceneAsset")]
		[SerializeField]
		internal UnityEngine.Object asset;

		// Token: 0x04000017 RID: 23
		[FormerlySerializedAs("sceneAssetGuidHex")]
		[SerializeField]
		internal string guid;
	}
}
