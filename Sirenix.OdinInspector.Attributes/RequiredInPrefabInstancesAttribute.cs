using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200005B RID: 91
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use [RequiredIn(PrefabKind.PrefabInstance)] instead.", true)]
	public sealed class RequiredInPrefabInstancesAttribute : Attribute
	{
		// Token: 0x06000134 RID: 308 RVA: 0x000035D1 File Offset: 0x000017D1
		public RequiredInPrefabInstancesAttribute()
		{
			this.MessageType = InfoMessageType.Error;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000035E0 File Offset: 0x000017E0
		public RequiredInPrefabInstancesAttribute(string errorMessage, InfoMessageType messageType)
		{
			this.ErrorMessage = errorMessage;
			this.MessageType = messageType;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000035F6 File Offset: 0x000017F6
		public RequiredInPrefabInstancesAttribute(string errorMessage)
		{
			this.ErrorMessage = errorMessage;
			this.MessageType = InfoMessageType.Error;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000360C File Offset: 0x0000180C
		public RequiredInPrefabInstancesAttribute(InfoMessageType messageType)
		{
			this.MessageType = messageType;
		}

		// Token: 0x040000FA RID: 250
		public string ErrorMessage;

		// Token: 0x040000FB RID: 251
		public InfoMessageType MessageType;
	}
}
