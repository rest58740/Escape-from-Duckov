using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000499 RID: 1177
	[ComVisible(true)]
	public abstract class KeyedHashAlgorithm : HashAlgorithm
	{
		// Token: 0x06002F25 RID: 12069 RVA: 0x000A80FB File Offset: 0x000A62FB
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.KeyValue != null)
				{
					Array.Clear(this.KeyValue, 0, this.KeyValue.Length);
				}
				this.KeyValue = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002F26 RID: 12070 RVA: 0x000A79CD File Offset: 0x000A5BCD
		// (set) Token: 0x06002F27 RID: 12071 RVA: 0x000A812A File Offset: 0x000A632A
		public virtual byte[] Key
		{
			get
			{
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (this.State != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Hash key cannot be changed after the first write to the stream."));
				}
				this.KeyValue = (byte[])value.Clone();
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000A8155 File Offset: 0x000A6355
		public new static KeyedHashAlgorithm Create()
		{
			return KeyedHashAlgorithm.Create("System.Security.Cryptography.KeyedHashAlgorithm");
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000A8161 File Offset: 0x000A6361
		public new static KeyedHashAlgorithm Create(string algName)
		{
			return (KeyedHashAlgorithm)CryptoConfig.CreateFromName(algName);
		}

		// Token: 0x0400217D RID: 8573
		protected byte[] KeyValue;
	}
}
