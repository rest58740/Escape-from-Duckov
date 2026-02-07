using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000518 RID: 1304
	public sealed class CompoundAce : KnownAce
	{
		// Token: 0x060033CE RID: 13262 RVA: 0x000BE047 File Offset: 0x000BC247
		public CompoundAce(AceFlags flags, int accessMask, CompoundAceType compoundAceType, SecurityIdentifier sid) : base(AceType.AccessAllowedCompound, flags)
		{
			this.compound_ace_type = compoundAceType;
			base.AccessMask = accessMask;
			base.SecurityIdentifier = sid;
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060033CF RID: 13263 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public override int BinaryLength
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060033D0 RID: 13264 RVA: 0x000BE067 File Offset: 0x000BC267
		// (set) Token: 0x060033D1 RID: 13265 RVA: 0x000BE06F File Offset: 0x000BC26F
		public CompoundAceType CompoundAceType
		{
			get
			{
				return this.compound_ace_type;
			}
			set
			{
				this.compound_ace_type = value;
			}
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x000479FC File Offset: 0x00045BFC
		internal override string GetSddlForm()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400245D RID: 9309
		private CompoundAceType compound_ace_type;
	}
}
