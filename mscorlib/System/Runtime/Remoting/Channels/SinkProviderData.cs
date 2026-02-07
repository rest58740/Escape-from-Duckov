using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005C9 RID: 1481
	[ComVisible(true)]
	public class SinkProviderData
	{
		// Token: 0x060038B4 RID: 14516 RVA: 0x000CA860 File Offset: 0x000C8A60
		public SinkProviderData(string name)
		{
			this.sinkName = name;
			this.children = new ArrayList();
			this.properties = new Hashtable();
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060038B5 RID: 14517 RVA: 0x000CA885 File Offset: 0x000C8A85
		public IList Children
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060038B6 RID: 14518 RVA: 0x000CA88D File Offset: 0x000C8A8D
		public string Name
		{
			get
			{
				return this.sinkName;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060038B7 RID: 14519 RVA: 0x000CA895 File Offset: 0x000C8A95
		public IDictionary Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x040025EE RID: 9710
		private string sinkName;

		// Token: 0x040025EF RID: 9711
		private ArrayList children;

		// Token: 0x040025F0 RID: 9712
		private Hashtable properties;
	}
}
