using System;

namespace System
{
	// Token: 0x02000169 RID: 361
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	[Serializable]
	public sealed class ObsoleteAttribute : Attribute
	{
		// Token: 0x06000E60 RID: 3680 RVA: 0x0003AD3D File Offset: 0x00038F3D
		public ObsoleteAttribute()
		{
			this._message = null;
			this._error = false;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0003AD53 File Offset: 0x00038F53
		public ObsoleteAttribute(string message)
		{
			this._message = message;
			this._error = false;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0003AD69 File Offset: 0x00038F69
		public ObsoleteAttribute(string message, bool error)
		{
			this._message = message;
			this._error = error;
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0003AD7F File Offset: 0x00038F7F
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0003AD87 File Offset: 0x00038F87
		public bool IsError
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x040012A5 RID: 4773
		private string _message;

		// Token: 0x040012A6 RID: 4774
		private bool _error;
	}
}
