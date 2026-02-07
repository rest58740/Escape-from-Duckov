using System;

namespace System
{
	// Token: 0x020000FE RID: 254
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	[Serializable]
	public sealed class AttributeUsageAttribute : Attribute
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x0002172F File Offset: 0x0001F92F
		public AttributeUsageAttribute(AttributeTargets validOn)
		{
			this._attributeTarget = validOn;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00021750 File Offset: 0x0001F950
		internal AttributeUsageAttribute(AttributeTargets validOn, bool allowMultiple, bool inherited)
		{
			this._attributeTarget = validOn;
			this._allowMultiple = allowMultiple;
			this._inherited = inherited;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0002177F File Offset: 0x0001F97F
		public AttributeTargets ValidOn
		{
			get
			{
				return this._attributeTarget;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x00021787 File Offset: 0x0001F987
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x0002178F File Offset: 0x0001F98F
		public bool AllowMultiple
		{
			get
			{
				return this._allowMultiple;
			}
			set
			{
				this._allowMultiple = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00021798 File Offset: 0x0001F998
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x000217A0 File Offset: 0x0001F9A0
		public bool Inherited
		{
			get
			{
				return this._inherited;
			}
			set
			{
				this._inherited = value;
			}
		}

		// Token: 0x04001064 RID: 4196
		private AttributeTargets _attributeTarget = AttributeTargets.All;

		// Token: 0x04001065 RID: 4197
		private bool _allowMultiple;

		// Token: 0x04001066 RID: 4198
		private bool _inherited = true;

		// Token: 0x04001067 RID: 4199
		internal static AttributeUsageAttribute Default = new AttributeUsageAttribute(AttributeTargets.All);
	}
}
