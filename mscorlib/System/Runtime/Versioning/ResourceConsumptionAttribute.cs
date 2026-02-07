using System;
using System.Diagnostics;

namespace System.Runtime.Versioning
{
	// Token: 0x0200063F RID: 1599
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
	[Conditional("RESOURCE_ANNOTATION_WORK")]
	public sealed class ResourceConsumptionAttribute : Attribute
	{
		// Token: 0x06003C2B RID: 15403 RVA: 0x000D118A File Offset: 0x000CF38A
		public ResourceConsumptionAttribute(ResourceScope resourceScope)
		{
			this._resourceScope = resourceScope;
			this._consumptionScope = this._resourceScope;
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x000D11A5 File Offset: 0x000CF3A5
		public ResourceConsumptionAttribute(ResourceScope resourceScope, ResourceScope consumptionScope)
		{
			this._resourceScope = resourceScope;
			this._consumptionScope = consumptionScope;
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x000D11BB File Offset: 0x000CF3BB
		public ResourceScope ResourceScope
		{
			get
			{
				return this._resourceScope;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003C2E RID: 15406 RVA: 0x000D11C3 File Offset: 0x000CF3C3
		public ResourceScope ConsumptionScope
		{
			get
			{
				return this._consumptionScope;
			}
		}

		// Token: 0x040026F6 RID: 9974
		private ResourceScope _consumptionScope;

		// Token: 0x040026F7 RID: 9975
		private ResourceScope _resourceScope;
	}
}
