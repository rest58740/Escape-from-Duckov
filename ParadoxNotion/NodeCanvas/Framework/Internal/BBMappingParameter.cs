using System;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200004A RID: 74
	[Serializable]
	public class BBMappingParameter : BBObjectParameter
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00009FF2 File Offset: 0x000081F2
		public string targetSubGraphVariableID
		{
			get
			{
				return this._targetSubGraphVariableID;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00009FFA File Offset: 0x000081FA
		// (set) Token: 0x0600038A RID: 906 RVA: 0x0000A002 File Offset: 0x00008202
		public bool canRead
		{
			get
			{
				return this._canRead;
			}
			set
			{
				this._canRead = value;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000A00B File Offset: 0x0000820B
		// (set) Token: 0x0600038C RID: 908 RVA: 0x0000A013 File Offset: 0x00008213
		public bool canWrite
		{
			get
			{
				return this._canWrite;
			}
			set
			{
				this._canWrite = value;
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000A01C File Offset: 0x0000821C
		public BBMappingParameter()
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000A024 File Offset: 0x00008224
		public BBMappingParameter(Variable subVariable)
		{
			this._targetSubGraphVariableID = subVariable.ID;
			base.SetType(subVariable.varType);
		}

		// Token: 0x04000101 RID: 257
		[SerializeField]
		private string _targetSubGraphVariableID;

		// Token: 0x04000102 RID: 258
		[SerializeField]
		private bool _canRead;

		// Token: 0x04000103 RID: 259
		[SerializeField]
		private bool _canWrite;
	}
}
