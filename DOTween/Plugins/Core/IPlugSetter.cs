using System;
using DG.Tweening.Core;

namespace DG.Tweening.Plugins.Core
{
	// Token: 0x02000040 RID: 64
	public interface IPlugSetter<T1, out T2, TPlugin, out TPlugOptions>
	{
		// Token: 0x0600025E RID: 606
		DOGetter<T1> Getter();

		// Token: 0x0600025F RID: 607
		DOSetter<T1> Setter();

		// Token: 0x06000260 RID: 608
		T2 EndValue();

		// Token: 0x06000261 RID: 609
		TPlugOptions GetOptions();
	}
}
