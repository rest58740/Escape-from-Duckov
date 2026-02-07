using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Security;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020005D5 RID: 1493
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlAttribute : ContextAttribute
	{
		// Token: 0x060038EB RID: 14571 RVA: 0x000CAF53 File Offset: 0x000C9153
		public UrlAttribute(string callsiteURL) : base(callsiteURL)
		{
			this.url = callsiteURL;
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x060038EC RID: 14572 RVA: 0x000CAF63 File Offset: 0x000C9163
		public string UrlValue
		{
			get
			{
				return this.url;
			}
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x000CAF6B File Offset: 0x000C916B
		public override bool Equals(object o)
		{
			return o is UrlAttribute && ((UrlAttribute)o).UrlValue == this.url;
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x000CAF8D File Offset: 0x000C918D
		public override int GetHashCode()
		{
			return this.url.GetHashCode();
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[ComVisible(true)]
		[SecurityCritical]
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x000040F7 File Offset: 0x000022F7
		[ComVisible(true)]
		[SecurityCritical]
		public override bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			return true;
		}

		// Token: 0x040025FD RID: 9725
		private string url;
	}
}
