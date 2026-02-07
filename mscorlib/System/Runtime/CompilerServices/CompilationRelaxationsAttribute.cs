using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000831 RID: 2097
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
	[Serializable]
	public class CompilationRelaxationsAttribute : Attribute
	{
		// Token: 0x060046B1 RID: 18097 RVA: 0x000E7113 File Offset: 0x000E5313
		public CompilationRelaxationsAttribute(int relaxations)
		{
			this.m_relaxations = relaxations;
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x000E7113 File Offset: 0x000E5313
		public CompilationRelaxationsAttribute(CompilationRelaxations relaxations)
		{
			this.m_relaxations = (int)relaxations;
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x060046B3 RID: 18099 RVA: 0x000E7122 File Offset: 0x000E5322
		public int CompilationRelaxations
		{
			get
			{
				return this.m_relaxations;
			}
		}

		// Token: 0x04002D7B RID: 11643
		private int m_relaxations;
	}
}
