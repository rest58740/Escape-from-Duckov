using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005CA RID: 1482
	[MonoTODO("Serialization format not compatible with .NET")]
	[ComVisible(true)]
	[Serializable]
	public class TransportHeaders : ITransportHeaders
	{
		// Token: 0x060038B8 RID: 14520 RVA: 0x000CA89D File Offset: 0x000C8A9D
		public TransportHeaders()
		{
			this.hash_table = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
		}

		// Token: 0x17000810 RID: 2064
		public object this[object key]
		{
			[SecurityCritical]
			get
			{
				return this.hash_table[key];
			}
			[SecurityCritical]
			set
			{
				this.hash_table[key] = value;
			}
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x000CA8D7 File Offset: 0x000C8AD7
		[SecurityCritical]
		public IEnumerator GetEnumerator()
		{
			return this.hash_table.GetEnumerator();
		}

		// Token: 0x040025F1 RID: 9713
		private Hashtable hash_table;
	}
}
