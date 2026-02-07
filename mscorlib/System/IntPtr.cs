using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000239 RID: 569
	[ComVisible(true)]
	[Serializable]
	public readonly struct IntPtr : ISerializable, IEquatable<IntPtr>
	{
		// Token: 0x060019F8 RID: 6648 RVA: 0x0005FCF4 File Offset: 0x0005DEF4
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public IntPtr(int value)
		{
			this.m_value = value;
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0005FCFE File Offset: 0x0005DEFE
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public IntPtr(long value)
		{
			this.m_value = value;
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0005FD08 File Offset: 0x0005DF08
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		[CLSCompliant(false)]
		public unsafe IntPtr(void* value)
		{
			this.m_value = value;
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0005FD14 File Offset: 0x0005DF14
		private IntPtr(SerializationInfo info, StreamingContext context)
		{
			long @int = info.GetInt64("value");
			this.m_value = @int;
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x0005FD35 File Offset: 0x0005DF35
		public unsafe static int Size
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return sizeof(void*);
			}
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x0005FD3D File Offset: 0x0005DF3D
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("value", this.ToInt64());
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0005FD5E File Offset: 0x0005DF5E
		public override bool Equals(object obj)
		{
			return obj is IntPtr && ((IntPtr)obj).m_value == this.m_value;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0005FD7D File Offset: 0x0005DF7D
		public override int GetHashCode()
		{
			return this.m_value;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0005FD7D File Offset: 0x0005DF7D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int ToInt32()
		{
			return this.m_value;
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0005FD86 File Offset: 0x0005DF86
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public long ToInt64()
		{
			if (IntPtr.Size == 4)
			{
				return (long)this.m_value;
			}
			return this.m_value;
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0005FDA0 File Offset: 0x0005DFA0
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public unsafe void* ToPointer()
		{
			return this.m_value;
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x0005FDA8 File Offset: 0x0005DFA8
		public override string ToString()
		{
			return this.ToString(null);
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0005FDB4 File Offset: 0x0005DFB4
		public string ToString(string format)
		{
			if (IntPtr.Size == 4)
			{
				return this.m_value.ToString(format, null);
			}
			return this.m_value.ToString(format, null);
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0005FDEC File Offset: 0x0005DFEC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool operator ==(IntPtr value1, IntPtr value2)
		{
			return value1.m_value == value2.m_value;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0005FDFE File Offset: 0x0005DFFE
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool operator !=(IntPtr value1, IntPtr value2)
		{
			return value1.m_value != value2.m_value;
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0005FE13 File Offset: 0x0005E013
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static explicit operator IntPtr(int value)
		{
			return new IntPtr(value);
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0005FE1B File Offset: 0x0005E01B
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static explicit operator IntPtr(long value)
		{
			return new IntPtr(value);
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0005FE23 File Offset: 0x0005E023
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		[CLSCompliant(false)]
		public unsafe static explicit operator IntPtr(void* value)
		{
			return new IntPtr(value);
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x0005FE2B File Offset: 0x0005E02B
		public static explicit operator int(IntPtr value)
		{
			return value.m_value;
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x0005FE35 File Offset: 0x0005E035
		public static explicit operator long(IntPtr value)
		{
			return value.ToInt64();
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0005FE3E File Offset: 0x0005E03E
		[CLSCompliant(false)]
		public unsafe static explicit operator void*(IntPtr value)
		{
			return value.m_value;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0005FE47 File Offset: 0x0005E047
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe static IntPtr Add(IntPtr pointer, int offset)
		{
			return (IntPtr)((void*)((byte*)((void*)pointer) + offset));
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0005FE56 File Offset: 0x0005E056
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe static IntPtr Subtract(IntPtr pointer, int offset)
		{
			return (IntPtr)((void*)((byte*)((void*)pointer) - offset));
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0005FE47 File Offset: 0x0005E047
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe static IntPtr operator +(IntPtr pointer, int offset)
		{
			return (IntPtr)((void*)((byte*)((void*)pointer) + offset));
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0005FE56 File Offset: 0x0005E056
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe static IntPtr operator -(IntPtr pointer, int offset)
		{
			return (IntPtr)((void*)((byte*)((void*)pointer) - offset));
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0005FE65 File Offset: 0x0005E065
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal bool IsNull()
		{
			return this.m_value == null;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0005FE71 File Offset: 0x0005E071
		bool IEquatable<IntPtr>.Equals(IntPtr other)
		{
			return this.m_value == other.m_value;
		}

		// Token: 0x0400171C RID: 5916
		private unsafe readonly void* m_value;

		// Token: 0x0400171D RID: 5917
		public static readonly IntPtr Zero;
	}
}
