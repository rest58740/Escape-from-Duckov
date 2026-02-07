using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000711 RID: 1809
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Module, Inherited = false)]
	public sealed class DefaultCharSetAttribute : Attribute
	{
		// Token: 0x060040C3 RID: 16579 RVA: 0x000E1643 File Offset: 0x000DF843
		public DefaultCharSetAttribute(CharSet charSet)
		{
			this._CharSet = charSet;
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x000E1652 File Offset: 0x000DF852
		public CharSet CharSet
		{
			get
			{
				return this._CharSet;
			}
		}

		// Token: 0x04002AEF RID: 10991
		internal CharSet _CharSet;
	}
}
