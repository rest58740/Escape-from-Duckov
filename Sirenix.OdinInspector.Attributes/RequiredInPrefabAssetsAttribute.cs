using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200005A RID: 90
	[Obsolete("Use [RequiredIn(PrefabKind.PrefabAsset)] instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class RequiredInPrefabAssetsAttribute : Attribute
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00003587 File Offset: 0x00001787
		public RequiredInPrefabAssetsAttribute()
		{
			this.MessageType = InfoMessageType.Error;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00003596 File Offset: 0x00001796
		public RequiredInPrefabAssetsAttribute(string errorMessage, InfoMessageType messageType)
		{
			this.ErrorMessage = errorMessage;
			this.MessageType = messageType;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000035AC File Offset: 0x000017AC
		public RequiredInPrefabAssetsAttribute(string errorMessage)
		{
			this.ErrorMessage = errorMessage;
			this.MessageType = InfoMessageType.Error;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000035C2 File Offset: 0x000017C2
		public RequiredInPrefabAssetsAttribute(InfoMessageType messageType)
		{
			this.MessageType = messageType;
		}

		// Token: 0x040000F8 RID: 248
		public string ErrorMessage;

		// Token: 0x040000F9 RID: 249
		public InfoMessageType MessageType;
	}
}
