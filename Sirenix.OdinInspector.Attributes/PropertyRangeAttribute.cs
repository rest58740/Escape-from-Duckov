using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000054 RID: 84
	[AttributeUsage(32767, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public sealed class PropertyRangeAttribute : Attribute
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00003449 File Offset: 0x00001649
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00003451 File Offset: 0x00001651
		[Obsolete("Use the MinGetter member instead.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string MinMember
		{
			get
			{
				return this.MinGetter;
			}
			set
			{
				this.MinGetter = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000345A File Offset: 0x0000165A
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00003462 File Offset: 0x00001662
		[Obsolete("Use the MaxGetter member instead.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string MaxMember
		{
			get
			{
				return this.MaxGetter;
			}
			set
			{
				this.MaxGetter = value;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000346B File Offset: 0x0000166B
		public PropertyRangeAttribute(double min, double max)
		{
			this.Min = ((min < max) ? min : max);
			this.Max = ((max > min) ? max : min);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000348F File Offset: 0x0000168F
		public PropertyRangeAttribute(string minGetter, double max)
		{
			this.MinGetter = minGetter;
			this.Max = max;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000034A5 File Offset: 0x000016A5
		public PropertyRangeAttribute(double min, string maxGetter)
		{
			this.Min = min;
			this.MaxGetter = maxGetter;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000034BB File Offset: 0x000016BB
		public PropertyRangeAttribute(string minGetter, string maxGetter)
		{
			this.MinGetter = minGetter;
			this.MaxGetter = maxGetter;
		}

		// Token: 0x040000ED RID: 237
		public double Min;

		// Token: 0x040000EE RID: 238
		public double Max;

		// Token: 0x040000EF RID: 239
		public string MinGetter;

		// Token: 0x040000F0 RID: 240
		public string MaxGetter;
	}
}
