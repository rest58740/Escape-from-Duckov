using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000053 RID: 83
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class PropertyOrderAttribute : Attribute
	{
		// Token: 0x0600011C RID: 284 RVA: 0x00002102 File Offset: 0x00000302
		public PropertyOrderAttribute()
		{
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000343A File Offset: 0x0000163A
		public PropertyOrderAttribute(float order)
		{
			this.Order = order;
		}

		// Token: 0x040000EC RID: 236
		public float Order;
	}
}
