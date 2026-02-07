using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020008EF RID: 2287
	[ComVisible(true)]
	[StructLayout(LayoutKind.Sequential)]
	public class LocalVariableInfo
	{
		// Token: 0x06004CAB RID: 19627 RVA: 0x0000259F File Offset: 0x0000079F
		protected LocalVariableInfo()
		{
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06004CAC RID: 19628 RVA: 0x000F30FD File Offset: 0x000F12FD
		public virtual bool IsPinned
		{
			get
			{
				return this.is_pinned;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06004CAD RID: 19629 RVA: 0x000F3105 File Offset: 0x000F1305
		public virtual int LocalIndex
		{
			get
			{
				return (int)this.position;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06004CAE RID: 19630 RVA: 0x000F310D File Offset: 0x000F130D
		public virtual Type LocalType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x000F3118 File Offset: 0x000F1318
		public override string ToString()
		{
			if (this.is_pinned)
			{
				return string.Format("{0} ({1}) (pinned)", this.type, this.position);
			}
			return string.Format("{0} ({1})", this.type, this.position);
		}

		// Token: 0x04003032 RID: 12338
		internal Type type;

		// Token: 0x04003033 RID: 12339
		internal bool is_pinned;

		// Token: 0x04003034 RID: 12340
		internal ushort position;
	}
}
