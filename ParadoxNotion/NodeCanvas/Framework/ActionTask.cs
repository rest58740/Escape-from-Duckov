using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000020 RID: 32
	public abstract class ActionTask<T> : ActionTask where T : class
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000060CE File Offset: 0x000042CE
		public sealed override Type agentType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000060DA File Offset: 0x000042DA
		public new T agent
		{
			get
			{
				return base.agent as T;
			}
		}
	}
}
