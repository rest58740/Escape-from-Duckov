using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200093A RID: 2362
	[ComVisible(true)]
	public readonly struct OpCode : IEquatable<OpCode>
	{
		// Token: 0x060051EB RID: 20971 RVA: 0x001004F8 File Offset: 0x000FE6F8
		internal OpCode(int p, int q)
		{
			this.op1 = (byte)(p & 255);
			this.op2 = (byte)(p >> 8 & 255);
			this.push = (byte)(p >> 16 & 255);
			this.pop = (byte)(p >> 24 & 255);
			this.size = (byte)(q & 255);
			this.type = (byte)(q >> 8 & 255);
			this.args = (byte)(q >> 16 & 255);
			this.flow = (byte)(q >> 24 & 255);
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x00100585 File Offset: 0x000FE785
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x00100594 File Offset: 0x000FE794
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is OpCode))
			{
				return false;
			}
			OpCode opCode = (OpCode)obj;
			return opCode.op1 == this.op1 && opCode.op2 == this.op2;
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x001005D3 File Offset: 0x000FE7D3
		public bool Equals(OpCode obj)
		{
			return obj.op1 == this.op1 && obj.op2 == this.op2;
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x001005F3 File Offset: 0x000FE7F3
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x060051F0 RID: 20976 RVA: 0x001005FB File Offset: 0x000FE7FB
		public string Name
		{
			get
			{
				if (this.op1 == 255)
				{
					return OpCodeNames.names[(int)this.op2];
				}
				return OpCodeNames.names[256 + (int)this.op2];
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x060051F1 RID: 20977 RVA: 0x00100629 File Offset: 0x000FE829
		public int Size
		{
			get
			{
				return (int)this.size;
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x060051F2 RID: 20978 RVA: 0x00100631 File Offset: 0x000FE831
		public OpCodeType OpCodeType
		{
			get
			{
				return (OpCodeType)this.type;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x060051F3 RID: 20979 RVA: 0x00100639 File Offset: 0x000FE839
		public OperandType OperandType
		{
			get
			{
				return (OperandType)this.args;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x060051F4 RID: 20980 RVA: 0x00100641 File Offset: 0x000FE841
		public FlowControl FlowControl
		{
			get
			{
				return (FlowControl)this.flow;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x060051F5 RID: 20981 RVA: 0x00100649 File Offset: 0x000FE849
		public StackBehaviour StackBehaviourPop
		{
			get
			{
				return (StackBehaviour)this.pop;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x060051F6 RID: 20982 RVA: 0x00100651 File Offset: 0x000FE851
		public StackBehaviour StackBehaviourPush
		{
			get
			{
				return (StackBehaviour)this.push;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x060051F7 RID: 20983 RVA: 0x00100659 File Offset: 0x000FE859
		public short Value
		{
			get
			{
				if (this.size == 1)
				{
					return (short)this.op2;
				}
				return (short)((int)this.op1 << 8 | (int)this.op2);
			}
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x0010067B File Offset: 0x000FE87B
		public static bool operator ==(OpCode a, OpCode b)
		{
			return a.op1 == b.op1 && a.op2 == b.op2;
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x0010069B File Offset: 0x000FE89B
		public static bool operator !=(OpCode a, OpCode b)
		{
			return a.op1 != b.op1 || a.op2 != b.op2;
		}

		// Token: 0x040031FF RID: 12799
		internal readonly byte op1;

		// Token: 0x04003200 RID: 12800
		internal readonly byte op2;

		// Token: 0x04003201 RID: 12801
		private readonly byte push;

		// Token: 0x04003202 RID: 12802
		private readonly byte pop;

		// Token: 0x04003203 RID: 12803
		private readonly byte size;

		// Token: 0x04003204 RID: 12804
		private readonly byte type;

		// Token: 0x04003205 RID: 12805
		private readonly byte args;

		// Token: 0x04003206 RID: 12806
		private readonly byte flow;
	}
}
