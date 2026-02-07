using System;
using System.Collections.Generic;

namespace System.Collections.Concurrent
{
	// Token: 0x02000A66 RID: 2662
	public abstract class Partitioner<TSource>
	{
		// Token: 0x06005F90 RID: 24464
		public abstract IList<IEnumerator<TSource>> GetPartitions(int partitionCount);

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x06005F91 RID: 24465 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool SupportsDynamicPartitions
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005F92 RID: 24466 RVA: 0x00140FE9 File Offset: 0x0013F1E9
		public virtual IEnumerable<TSource> GetDynamicPartitions()
		{
			throw new NotSupportedException("Dynamic partitions are not supported by this partitioner.");
		}
	}
}
