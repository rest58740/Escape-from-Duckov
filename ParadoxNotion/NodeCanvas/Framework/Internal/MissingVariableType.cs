using System;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200004F RID: 79
	public class MissingVariableType : Variable<object>, IMissingRecoverable
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000A3C5 File Offset: 0x000085C5
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0000A3CD File Offset: 0x000085CD
		public string missingType
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

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000A3D6 File Offset: 0x000085D6
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x0000A3DE File Offset: 0x000085DE
		public string recoveryState
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

		// Token: 0x0400010E RID: 270
		[SerializeField]
		private string _missingType;

		// Token: 0x0400010F RID: 271
		[SerializeField]
		private string _recoveryState;
	}
}
