using System;

namespace System
{
	// Token: 0x020001CB RID: 459
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class LoaderOptimizationAttribute : Attribute
	{
		// Token: 0x060013B9 RID: 5049 RVA: 0x0004E811 File Offset: 0x0004CA11
		public LoaderOptimizationAttribute(byte value)
		{
			this._val = value;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0004E820 File Offset: 0x0004CA20
		public LoaderOptimizationAttribute(LoaderOptimization value)
		{
			this._val = (byte)value;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x0004E830 File Offset: 0x0004CA30
		public LoaderOptimization Value
		{
			get
			{
				return (LoaderOptimization)this._val;
			}
		}

		// Token: 0x04001454 RID: 5204
		private readonly byte _val;
	}
}
