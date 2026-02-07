using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200018D RID: 397
	public class MMKVPMarshaller
	{
		// Token: 0x0600090C RID: 2316 RVA: 0x0000D700 File Offset: 0x0000B900
		public MMKVPMarshaller(MatchMakingKeyValuePair_t[] filters)
		{
			if (filters == null)
			{
				return;
			}
			int num = Marshal.SizeOf(typeof(MatchMakingKeyValuePair_t));
			this.m_pNativeArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * filters.Length);
			this.m_pArrayEntries = Marshal.AllocHGlobal(num * filters.Length);
			for (int i = 0; i < filters.Length; i++)
			{
				Marshal.StructureToPtr<MatchMakingKeyValuePair_t>(filters[i], new IntPtr(this.m_pArrayEntries.ToInt64() + (long)(i * num)), false);
			}
			Marshal.WriteIntPtr(this.m_pNativeArray, this.m_pArrayEntries);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0000D798 File Offset: 0x0000B998
		~MMKVPMarshaller()
		{
			if (this.m_pArrayEntries != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pArrayEntries);
			}
			if (this.m_pNativeArray != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pNativeArray);
			}
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0000D7F8 File Offset: 0x0000B9F8
		public static implicit operator IntPtr(MMKVPMarshaller that)
		{
			return that.m_pNativeArray;
		}

		// Token: 0x04000A72 RID: 2674
		private IntPtr m_pNativeArray;

		// Token: 0x04000A73 RID: 2675
		private IntPtr m_pArrayEntries;
	}
}
