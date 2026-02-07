using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000228 RID: 552
	[ComVisible(false)]
	[Serializable]
	public sealed class ApplicationIdentity : ISerializable
	{
		// Token: 0x060018C7 RID: 6343 RVA: 0x0005E007 File Offset: 0x0005C207
		public ApplicationIdentity(string applicationIdentityFullName)
		{
			if (applicationIdentityFullName == null)
			{
				throw new ArgumentNullException("applicationIdentityFullName");
			}
			if (applicationIdentityFullName.IndexOf(", Culture=") == -1)
			{
				this._fullName = applicationIdentityFullName + ", Culture=neutral";
				return;
			}
			this._fullName = applicationIdentityFullName;
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060018C8 RID: 6344 RVA: 0x0005E044 File Offset: 0x0005C244
		public string CodeBase
		{
			get
			{
				return this._codeBase;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x0005E04C File Offset: 0x0005C24C
		public string FullName
		{
			get
			{
				return this._fullName;
			}
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x0005E04C File Offset: 0x0005C24C
		public override string ToString()
		{
			return this._fullName;
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0005D90C File Offset: 0x0005BB0C
		[MonoTODO("Missing serialization")]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
		}

		// Token: 0x040016D5 RID: 5845
		private string _fullName;

		// Token: 0x040016D6 RID: 5846
		private string _codeBase;
	}
}
