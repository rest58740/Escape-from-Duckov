using System;

namespace System.Threading
{
	// Token: 0x02000283 RID: 643
	public readonly struct AsyncLocalValueChangedArgs<T>
	{
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001D60 RID: 7520 RVA: 0x0006DB3E File Offset: 0x0006BD3E
		public T PreviousValue { get; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001D61 RID: 7521 RVA: 0x0006DB46 File Offset: 0x0006BD46
		public T CurrentValue { get; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001D62 RID: 7522 RVA: 0x0006DB4E File Offset: 0x0006BD4E
		public bool ThreadContextChanged { get; }

		// Token: 0x06001D63 RID: 7523 RVA: 0x0006DB56 File Offset: 0x0006BD56
		internal AsyncLocalValueChangedArgs(T previousValue, T currentValue, bool contextChanged)
		{
			this = default(AsyncLocalValueChangedArgs<T>);
			this.PreviousValue = previousValue;
			this.CurrentValue = currentValue;
			this.ThreadContextChanged = contextChanged;
		}
	}
}
