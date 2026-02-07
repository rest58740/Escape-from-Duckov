using System;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000035 RID: 53
	[DoNotList]
	[Description("Please resolve the MissingNode issue by either replacing the node, importing the missing node type, or refactoring the type in GraphRefactor.")]
	public sealed class MissingNode : Node, IMissingRecoverable
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000091B6 File Offset: 0x000073B6
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000091BE File Offset: 0x000073BE
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000321 RID: 801 RVA: 0x000091C7 File Offset: 0x000073C7
		// (set) Token: 0x06000322 RID: 802 RVA: 0x000091CF File Offset: 0x000073CF
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

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000323 RID: 803 RVA: 0x000091D8 File Offset: 0x000073D8
		public override string name
		{
			get
			{
				return "Missing Node".FormatError();
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000324 RID: 804 RVA: 0x000091E4 File Offset: 0x000073E4
		public override Type outConnectionType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000325 RID: 805 RVA: 0x000091E7 File Offset: 0x000073E7
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000326 RID: 806 RVA: 0x000091EA File Offset: 0x000073EA
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000327 RID: 807 RVA: 0x000091ED File Offset: 0x000073ED
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000328 RID: 808 RVA: 0x000091F0 File Offset: 0x000073F0
		public override bool canSelfConnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000329 RID: 809 RVA: 0x000091F3 File Offset: 0x000073F3
		public override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Right;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600032A RID: 810 RVA: 0x000091F6 File Offset: 0x000073F6
		public override Alignment2x2 iconAlignment
		{
			get
			{
				return Alignment2x2.Default;
			}
		}

		// Token: 0x040000BA RID: 186
		[SerializeField]
		private string _missingType;

		// Token: 0x040000BB RID: 187
		[SerializeField]
		private string _recoveryState;
	}
}
