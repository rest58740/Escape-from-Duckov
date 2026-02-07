using System;

namespace System.Security.AccessControl
{
	// Token: 0x0200051F RID: 1311
	public sealed class CustomAce : GenericAce
	{
		// Token: 0x060033EC RID: 13292 RVA: 0x000BE102 File Offset: 0x000BC302
		public CustomAce(AceType type, AceFlags flags, byte[] opaque) : base(type, flags)
		{
			this.SetOpaque(opaque);
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public override int BinaryLength
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060033EE RID: 13294 RVA: 0x000BE113 File Offset: 0x000BC313
		public int OpaqueLength
		{
			get
			{
				return this.opaque.Length;
			}
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x000BE11D File Offset: 0x000BC31D
		public byte[] GetOpaque()
		{
			return (byte[])this.opaque.Clone();
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x000BE12F File Offset: 0x000BC32F
		public void SetOpaque(byte[] opaque)
		{
			if (opaque == null)
			{
				this.opaque = null;
				return;
			}
			this.opaque = (byte[])opaque.Clone();
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x000472CC File Offset: 0x000454CC
		internal override string GetSddlForm()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04002483 RID: 9347
		private byte[] opaque;

		// Token: 0x04002484 RID: 9348
		[MonoTODO]
		public static readonly int MaxOpaqueLength;
	}
}
