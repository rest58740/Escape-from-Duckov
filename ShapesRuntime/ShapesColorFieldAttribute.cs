using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000071 RID: 113
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	public sealed class ShapesColorFieldAttribute : PropertyAttribute
	{
		// Token: 0x06000CB1 RID: 3249 RVA: 0x00019BDF File Offset: 0x00017DDF
		public ShapesColorFieldAttribute(bool showAlpha)
		{
			this.showAlpha = showAlpha;
		}

		// Token: 0x04000281 RID: 641
		public readonly bool showAlpha = true;
	}
}
