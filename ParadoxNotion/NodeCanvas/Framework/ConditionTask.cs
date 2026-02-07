using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000023 RID: 35
	public abstract class ConditionTask<T> : ConditionTask where T : class
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000663E File Offset: 0x0000483E
		public sealed override Type agentType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000664A File Offset: 0x0000484A
		public new T agent
		{
			get
			{
				return base.agent as T;
			}
		}
	}
}
