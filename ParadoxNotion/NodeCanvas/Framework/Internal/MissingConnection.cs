using System;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000034 RID: 52
	[DoNotList]
	[Description("Please resolve the MissingConnection issue by either replacing the connection, importing the missing connection type, or refactoring the type in GraphRefactor.")]
	public sealed class MissingConnection : Connection, IMissingRecoverable
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000918C File Offset: 0x0000738C
		// (set) Token: 0x0600031B RID: 795 RVA: 0x00009194 File Offset: 0x00007394
		string IMissingRecoverable.missingType
		{
			get
			{
				return this._missingType;
			}
			set
			{
				this._missingType = value;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000919D File Offset: 0x0000739D
		// (set) Token: 0x0600031D RID: 797 RVA: 0x000091A5 File Offset: 0x000073A5
		string IMissingRecoverable.recoveryState
		{
			get
			{
				return this._recoveryState;
			}
			set
			{
				this._recoveryState = value;
			}
		}

		// Token: 0x040000B8 RID: 184
		[SerializeField]
		private string _missingType;

		// Token: 0x040000B9 RID: 185
		[SerializeField]
		private string _recoveryState;
	}
}
