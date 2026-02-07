using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000437 RID: 1079
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class FileDialogPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002BC0 RID: 11200 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public FileDialogPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002BC1 RID: 11201 RVA: 0x0009DFC0 File Offset: 0x0009C1C0
		// (set) Token: 0x06002BC2 RID: 11202 RVA: 0x0009DFC8 File Offset: 0x0009C1C8
		public bool Open
		{
			get
			{
				return this.canOpen;
			}
			set
			{
				this.canOpen = value;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002BC3 RID: 11203 RVA: 0x0009DFD1 File Offset: 0x0009C1D1
		// (set) Token: 0x06002BC4 RID: 11204 RVA: 0x0009DFD9 File Offset: 0x0009C1D9
		public bool Save
		{
			get
			{
				return this.canSave;
			}
			set
			{
				this.canSave = value;
			}
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x0009DFE4 File Offset: 0x0009C1E4
		public override IPermission CreatePermission()
		{
			FileDialogPermission result;
			if (base.Unrestricted)
			{
				result = new FileDialogPermission(PermissionState.Unrestricted);
			}
			else
			{
				FileDialogPermissionAccess fileDialogPermissionAccess = FileDialogPermissionAccess.None;
				if (this.canOpen)
				{
					fileDialogPermissionAccess |= FileDialogPermissionAccess.Open;
				}
				if (this.canSave)
				{
					fileDialogPermissionAccess |= FileDialogPermissionAccess.Save;
				}
				result = new FileDialogPermission(fileDialogPermissionAccess);
			}
			return result;
		}

		// Token: 0x04002011 RID: 8209
		private bool canOpen;

		// Token: 0x04002012 RID: 8210
		private bool canSave;
	}
}
