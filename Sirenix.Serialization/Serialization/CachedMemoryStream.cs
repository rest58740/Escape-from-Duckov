using System;
using System.IO;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200005A RID: 90
	internal sealed class CachedMemoryStream : ICacheNotificationReceiver
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00016E1B File Offset: 0x0001501B
		public MemoryStream MemoryStream
		{
			get
			{
				if (!this.memoryStream.CanRead)
				{
					this.memoryStream = new MemoryStream(CachedMemoryStream.InitialCapacity);
				}
				return this.memoryStream;
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00016E40 File Offset: 0x00015040
		public CachedMemoryStream()
		{
			this.memoryStream = new MemoryStream(CachedMemoryStream.InitialCapacity);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00016E58 File Offset: 0x00015058
		public void OnFreed()
		{
			this.memoryStream.SetLength(0L);
			this.memoryStream.Position = 0L;
			if (this.memoryStream.Capacity > CachedMemoryStream.MaxCapacity)
			{
				this.memoryStream.Capacity = CachedMemoryStream.MaxCapacity;
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00016E96 File Offset: 0x00015096
		public void OnClaimed()
		{
			this.memoryStream.SetLength(0L);
			this.memoryStream.Position = 0L;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00016EB4 File Offset: 0x000150B4
		public static Cache<CachedMemoryStream> Claim(int minCapacity)
		{
			Cache<CachedMemoryStream> cache = Cache<CachedMemoryStream>.Claim();
			if (cache.Value.MemoryStream.Capacity < minCapacity)
			{
				cache.Value.MemoryStream.Capacity = minCapacity;
			}
			return cache;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00016EEC File Offset: 0x000150EC
		public static Cache<CachedMemoryStream> Claim(byte[] bytes = null)
		{
			Cache<CachedMemoryStream> cache = Cache<CachedMemoryStream>.Claim();
			if (bytes != null)
			{
				cache.Value.MemoryStream.Write(bytes, 0, bytes.Length);
				cache.Value.MemoryStream.Position = 0L;
			}
			return cache;
		}

		// Token: 0x040000FF RID: 255
		public static int InitialCapacity = 1024;

		// Token: 0x04000100 RID: 256
		public static int MaxCapacity = 32768;

		// Token: 0x04000101 RID: 257
		private MemoryStream memoryStream;
	}
}
