using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000461 RID: 1121
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class UIPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002D93 RID: 11667 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public UIPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06002D94 RID: 11668 RVA: 0x000A359B File Offset: 0x000A179B
		// (set) Token: 0x06002D95 RID: 11669 RVA: 0x000A35A3 File Offset: 0x000A17A3
		public UIPermissionClipboard Clipboard
		{
			get
			{
				return this.clipboard;
			}
			set
			{
				this.clipboard = value;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06002D96 RID: 11670 RVA: 0x000A35AC File Offset: 0x000A17AC
		// (set) Token: 0x06002D97 RID: 11671 RVA: 0x000A35B4 File Offset: 0x000A17B4
		public UIPermissionWindow Window
		{
			get
			{
				return this.window;
			}
			set
			{
				this.window = value;
			}
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000A35C0 File Offset: 0x000A17C0
		public override IPermission CreatePermission()
		{
			UIPermission result;
			if (base.Unrestricted)
			{
				result = new UIPermission(PermissionState.Unrestricted);
			}
			else
			{
				result = new UIPermission(this.window, this.clipboard);
			}
			return result;
		}

		// Token: 0x040020BB RID: 8379
		private UIPermissionClipboard clipboard;

		// Token: 0x040020BC RID: 8380
		private UIPermissionWindow window;
	}
}
