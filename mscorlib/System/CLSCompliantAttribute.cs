using System;

namespace System
{
	// Token: 0x02000104 RID: 260
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	[Serializable]
	public sealed class CLSCompliantAttribute : Attribute
	{
		// Token: 0x060007DB RID: 2011 RVA: 0x00022541 File Offset: 0x00020741
		public CLSCompliantAttribute(bool isCompliant)
		{
			this._compliant = isCompliant;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00022550 File Offset: 0x00020750
		public bool IsCompliant
		{
			get
			{
				return this._compliant;
			}
		}

		// Token: 0x04001077 RID: 4215
		private bool _compliant;
	}
}
