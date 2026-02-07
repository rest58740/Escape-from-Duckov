using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000014 RID: 20
	public abstract class ChannelWriter<T>
	{
		// Token: 0x06000060 RID: 96
		public abstract bool TryWrite(T item);

		// Token: 0x06000061 RID: 97
		public abstract bool TryComplete(Exception error = null);

		// Token: 0x06000062 RID: 98 RVA: 0x00002B57 File Offset: 0x00000D57
		public void Complete(Exception error = null)
		{
			if (!this.TryComplete(error))
			{
				throw new ChannelClosedException();
			}
		}
	}
}
