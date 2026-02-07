using System;

namespace System.Diagnostics.Contracts
{
	// Token: 0x020009CE RID: 2510
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
	[Conditional("CONTRACTS_FULL")]
	public sealed class ContractOptionAttribute : Attribute
	{
		// Token: 0x06005A09 RID: 23049 RVA: 0x00133D73 File Offset: 0x00131F73
		public ContractOptionAttribute(string category, string setting, bool enabled)
		{
			this._category = category;
			this._setting = setting;
			this._enabled = enabled;
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x00133D90 File Offset: 0x00131F90
		public ContractOptionAttribute(string category, string setting, string value)
		{
			this._category = category;
			this._setting = setting;
			this._value = value;
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06005A0B RID: 23051 RVA: 0x00133DAD File Offset: 0x00131FAD
		public string Category
		{
			get
			{
				return this._category;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06005A0C RID: 23052 RVA: 0x00133DB5 File Offset: 0x00131FB5
		public string Setting
		{
			get
			{
				return this._setting;
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06005A0D RID: 23053 RVA: 0x00133DBD File Offset: 0x00131FBD
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06005A0E RID: 23054 RVA: 0x00133DC5 File Offset: 0x00131FC5
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x040037A7 RID: 14247
		private string _category;

		// Token: 0x040037A8 RID: 14248
		private string _setting;

		// Token: 0x040037A9 RID: 14249
		private bool _enabled;

		// Token: 0x040037AA RID: 14250
		private string _value;
	}
}
