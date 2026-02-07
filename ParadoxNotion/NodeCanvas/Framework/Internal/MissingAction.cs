using System;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000047 RID: 71
	[DoNotList]
	[Description("Please resolve the MissingAction issue by either replacing the action, importing the missing action type, or refactoring the type in GraphRefactor.")]
	public class MissingAction : ActionTask, IMissingRecoverable
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00009EEA File Offset: 0x000080EA
		// (set) Token: 0x06000376 RID: 886 RVA: 0x00009EF2 File Offset: 0x000080F2
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00009EFB File Offset: 0x000080FB
		// (set) Token: 0x06000378 RID: 888 RVA: 0x00009F03 File Offset: 0x00008103
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

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00009F0C File Offset: 0x0000810C
		protected override string info
		{
			get
			{
				return ReflectionTools.FriendlyTypeName(this._missingType).FormatError();
			}
		}

		// Token: 0x040000FC RID: 252
		[SerializeField]
		private string _missingType;

		// Token: 0x040000FD RID: 253
		[SerializeField]
		private string _recoveryState;
	}
}
