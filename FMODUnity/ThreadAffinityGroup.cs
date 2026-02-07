using System;
using System.Collections.Generic;

namespace FMODUnity
{
	// Token: 0x0200011E RID: 286
	[Serializable]
	public class ThreadAffinityGroup
	{
		// Token: 0x06000764 RID: 1892 RVA: 0x0000A539 File Offset: 0x00008739
		public ThreadAffinityGroup()
		{
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0000A54C File Offset: 0x0000874C
		public ThreadAffinityGroup(ThreadAffinityGroup other)
		{
			this.threads = new List<ThreadType>(other.threads);
			this.affinity = other.affinity;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0000A57C File Offset: 0x0000877C
		public ThreadAffinityGroup(ThreadAffinity affinity, params ThreadType[] threads)
		{
			this.threads = new List<ThreadType>(threads);
			this.affinity = affinity;
		}

		// Token: 0x04000603 RID: 1539
		public List<ThreadType> threads = new List<ThreadType>();

		// Token: 0x04000604 RID: 1540
		public ThreadAffinity affinity;
	}
}
