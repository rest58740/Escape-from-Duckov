using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000B2 RID: 178
	[Serializable]
	public struct SerializationData
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x000213A8 File Offset: 0x0001F5A8
		[Obsolete("Use ContainsData instead")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool HasEditorData
		{
			get
			{
				DataFormat serializedFormat = this.SerializedFormat;
				if (serializedFormat <= 1)
				{
					return !this.SerializedBytesString.IsNullOrWhitespace() || (this.SerializedBytes != null && this.SerializedBytes.Length != 0);
				}
				if (serializedFormat != 2)
				{
					throw new NotImplementedException(this.SerializedFormat.ToString());
				}
				return this.SerializationNodes != null && this.SerializationNodes.Count != 0;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00021419 File Offset: 0x0001F619
		public bool ContainsData
		{
			get
			{
				return this.SerializedBytes != null && this.SerializationNodes != null && this.PrefabModifications != null && this.ReferencedUnityObjects != null;
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00021440 File Offset: 0x0001F640
		public void Reset()
		{
			this.SerializedFormat = 0;
			if (this.SerializedBytes != null && this.SerializedBytes.Length != 0)
			{
				this.SerializedBytes = new byte[0];
			}
			if (this.ReferencedUnityObjects != null && this.ReferencedUnityObjects.Count > 0)
			{
				this.ReferencedUnityObjects.Clear();
			}
			this.Prefab = null;
			if (this.SerializationNodes != null && this.SerializationNodes.Count > 0)
			{
				this.SerializationNodes.Clear();
			}
			if (this.SerializedBytesString != null && this.SerializedBytesString.Length > 0)
			{
				this.SerializedBytesString = string.Empty;
			}
			if (this.PrefabModificationsReferencedUnityObjects != null && this.PrefabModificationsReferencedUnityObjects.Count > 0)
			{
				this.PrefabModificationsReferencedUnityObjects.Clear();
			}
			if (this.PrefabModifications != null && this.PrefabModifications.Count > 0)
			{
				this.PrefabModifications.Clear();
			}
		}

		// Token: 0x040001B8 RID: 440
		public const string PrefabModificationsReferencedUnityObjectsFieldName = "PrefabModificationsReferencedUnityObjects";

		// Token: 0x040001B9 RID: 441
		public const string PrefabModificationsFieldName = "PrefabModifications";

		// Token: 0x040001BA RID: 442
		public const string PrefabFieldName = "Prefab";

		// Token: 0x040001BB RID: 443
		[SerializeField]
		public DataFormat SerializedFormat;

		// Token: 0x040001BC RID: 444
		[SerializeField]
		public byte[] SerializedBytes;

		// Token: 0x040001BD RID: 445
		[SerializeField]
		public List<Object> ReferencedUnityObjects;

		// Token: 0x040001BE RID: 446
		[SerializeField]
		public string SerializedBytesString;

		// Token: 0x040001BF RID: 447
		[SerializeField]
		public Object Prefab;

		// Token: 0x040001C0 RID: 448
		[SerializeField]
		public List<Object> PrefabModificationsReferencedUnityObjects;

		// Token: 0x040001C1 RID: 449
		[SerializeField]
		public List<string> PrefabModifications;

		// Token: 0x040001C2 RID: 450
		[SerializeField]
		public List<SerializationNode> SerializationNodes;
	}
}
