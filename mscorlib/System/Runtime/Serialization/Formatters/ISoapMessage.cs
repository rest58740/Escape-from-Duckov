using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x0200067C RID: 1660
	[ComVisible(true)]
	public interface ISoapMessage
	{
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06003DCA RID: 15818
		// (set) Token: 0x06003DCB RID: 15819
		string[] ParamNames { get; set; }

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06003DCC RID: 15820
		// (set) Token: 0x06003DCD RID: 15821
		object[] ParamValues { get; set; }

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06003DCE RID: 15822
		// (set) Token: 0x06003DCF RID: 15823
		Type[] ParamTypes { get; set; }

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06003DD0 RID: 15824
		// (set) Token: 0x06003DD1 RID: 15825
		string MethodName { get; set; }

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06003DD2 RID: 15826
		// (set) Token: 0x06003DD3 RID: 15827
		string XmlNameSpace { get; set; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06003DD4 RID: 15828
		// (set) Token: 0x06003DD5 RID: 15829
		Header[] Headers { get; set; }
	}
}
