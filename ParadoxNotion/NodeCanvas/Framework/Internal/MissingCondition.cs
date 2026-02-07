using System;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000048 RID: 72
	[DoNotList]
	[Description("Please resolve the MissingCondition issue by either replacing the condition, importing the missing condition type, or refactoring the type in GraphRefactor.")]
	public class MissingCondition : ConditionTask, IMissingRecoverable
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00009F26 File Offset: 0x00008126
		// (set) Token: 0x0600037C RID: 892 RVA: 0x00009F2E File Offset: 0x0000812E
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00009F37 File Offset: 0x00008137
		// (set) Token: 0x0600037E RID: 894 RVA: 0x00009F3F File Offset: 0x0000813F
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

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00009F48 File Offset: 0x00008148
		protected override string info
		{
			get
			{
				return ReflectionTools.FriendlyTypeName(this._missingType).FormatError();
			}
		}

		// Token: 0x040000FE RID: 254
		[SerializeField]
		private string _missingType;

		// Token: 0x040000FF RID: 255
		[SerializeField]
		private string _recoveryState;
	}
}
