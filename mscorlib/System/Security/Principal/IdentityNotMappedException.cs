using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Principal
{
	// Token: 0x020004E3 RID: 1251
	[ComVisible(false)]
	[Serializable]
	public sealed class IdentityNotMappedException : SystemException
	{
		// Token: 0x060031ED RID: 12781 RVA: 0x000B7B8F File Offset: 0x000B5D8F
		public IdentityNotMappedException() : base(Locale.GetText("Couldn't translate some identities."))
		{
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x0006E6B1 File Offset: 0x0006C8B1
		public IdentityNotMappedException(string message) : base(message)
		{
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x0006E6BA File Offset: 0x0006C8BA
		public IdentityNotMappedException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060031F0 RID: 12784 RVA: 0x000B7BA1 File Offset: 0x000B5DA1
		public IdentityReferenceCollection UnmappedIdentities
		{
			get
			{
				if (this._coll == null)
				{
					this._coll = new IdentityReferenceCollection();
				}
				return this._coll;
			}
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[SecurityCritical]
		[MonoTODO("not implemented")]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x040022BF RID: 8895
		private IdentityReferenceCollection _coll;
	}
}
