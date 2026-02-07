using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000011 RID: 17
	public abstract class Channel<TWrite, TRead>
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002A97 File Offset: 0x00000C97
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002A9F File Offset: 0x00000C9F
		public ChannelReader<TRead> Reader { get; protected set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002AA8 File Offset: 0x00000CA8
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002AB0 File Offset: 0x00000CB0
		public ChannelWriter<TWrite> Writer { get; protected set; }

		// Token: 0x06000055 RID: 85 RVA: 0x00002AB9 File Offset: 0x00000CB9
		public static implicit operator ChannelReader<TRead>(Channel<TWrite, TRead> channel)
		{
			return channel.Reader;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002AC1 File Offset: 0x00000CC1
		public static implicit operator ChannelWriter<TWrite>(Channel<TWrite, TRead> channel)
		{
			return channel.Writer;
		}
	}
}
