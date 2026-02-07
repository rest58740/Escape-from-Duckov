using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000836 RID: 2102
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public sealed class InternalsVisibleToAttribute : Attribute
	{
		// Token: 0x060046B8 RID: 18104 RVA: 0x000E713A File Offset: 0x000E533A
		public InternalsVisibleToAttribute(string assemblyName)
		{
			this._assemblyName = assemblyName;
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x060046B9 RID: 18105 RVA: 0x000E7150 File Offset: 0x000E5350
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x060046BA RID: 18106 RVA: 0x000E7158 File Offset: 0x000E5358
		// (set) Token: 0x060046BB RID: 18107 RVA: 0x000E7160 File Offset: 0x000E5360
		public bool AllInternalsVisible
		{
			get
			{
				return this._allInternalsVisible;
			}
			set
			{
				this._allInternalsVisible = value;
			}
		}

		// Token: 0x04002D7C RID: 11644
		private string _assemblyName;

		// Token: 0x04002D7D RID: 11645
		private bool _allInternalsVisible = true;
	}
}
