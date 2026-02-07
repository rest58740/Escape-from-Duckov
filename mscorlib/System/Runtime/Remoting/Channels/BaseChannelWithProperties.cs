using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005A6 RID: 1446
	[ComVisible(true)]
	public abstract class BaseChannelWithProperties : BaseChannelObjectWithProperties
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06003828 RID: 14376 RVA: 0x000C94EA File Offset: 0x000C76EA
		public override IDictionary Properties
		{
			get
			{
				if (this.SinksWithProperties == null || this.SinksWithProperties.Properties == null)
				{
					return base.Properties;
				}
				return new AggregateDictionary(new IDictionary[]
				{
					base.Properties,
					this.SinksWithProperties.Properties
				});
			}
		}

		// Token: 0x040025CE RID: 9678
		protected IChannelSinkBase SinksWithProperties;
	}
}
