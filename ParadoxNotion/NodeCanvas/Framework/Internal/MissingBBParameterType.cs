using System;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200004E RID: 78
	public class MissingBBParameterType : BBParameter<object>, IMissingRecoverable
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000A39B File Offset: 0x0000859B
		// (set) Token: 0x060003AD RID: 941 RVA: 0x0000A3A3 File Offset: 0x000085A3
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

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000A3AC File Offset: 0x000085AC
		// (set) Token: 0x060003AF RID: 943 RVA: 0x0000A3B4 File Offset: 0x000085B4
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

		// Token: 0x0400010C RID: 268
		[SerializeField]
		private string _missingType;

		// Token: 0x0400010D RID: 269
		[SerializeField]
		private string _recoveryState;
	}
}
