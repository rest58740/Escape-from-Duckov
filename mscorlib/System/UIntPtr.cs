using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000265 RID: 613
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Serializable]
	public readonly struct UIntPtr : ISerializable, IEquatable<UIntPtr>
	{
		// Token: 0x06001BF4 RID: 7156 RVA: 0x0006895A File Offset: 0x00066B5A
		public UIntPtr(ulong value)
		{
			if (value > (ulong)-1 && UIntPtr.Size < 8)
			{
				throw new OverflowException(Locale.GetText("This isn't a 64bits machine."));
			}
			this._pointer = value;
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x00068981 File Offset: 0x00066B81
		public UIntPtr(uint value)
		{
			this._pointer = value;
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x0006898B File Offset: 0x00066B8B
		[CLSCompliant(false)]
		public unsafe UIntPtr(void* value)
		{
			this._pointer = value;
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x00068994 File Offset: 0x00066B94
		public override bool Equals(object obj)
		{
			if (obj is UIntPtr)
			{
				UIntPtr uintPtr = (UIntPtr)obj;
				return this._pointer == uintPtr._pointer;
			}
			return false;
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000689C1 File Offset: 0x00066BC1
		public override int GetHashCode()
		{
			return this._pointer;
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000689CA File Offset: 0x00066BCA
		public uint ToUInt32()
		{
			return this._pointer;
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x000689D3 File Offset: 0x00066BD3
		public ulong ToUInt64()
		{
			return this._pointer;
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000689DC File Offset: 0x00066BDC
		[CLSCompliant(false)]
		public unsafe void* ToPointer()
		{
			return this._pointer;
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000689E4 File Offset: 0x00066BE4
		public override string ToString()
		{
			if (UIntPtr.Size >= 8)
			{
				return this._pointer.ToString();
			}
			return this._pointer.ToString();
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x00068A18 File Offset: 0x00066C18
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("pointer", this._pointer);
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x00068A3A File Offset: 0x00066C3A
		public static bool operator ==(UIntPtr value1, UIntPtr value2)
		{
			return value1._pointer == value2._pointer;
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x00068A4C File Offset: 0x00066C4C
		public static bool operator !=(UIntPtr value1, UIntPtr value2)
		{
			return value1._pointer != value2._pointer;
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x00068A61 File Offset: 0x00066C61
		public static explicit operator ulong(UIntPtr value)
		{
			return value._pointer;
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00068A6B File Offset: 0x00066C6B
		public static explicit operator uint(UIntPtr value)
		{
			return value._pointer;
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x00068A75 File Offset: 0x00066C75
		public static explicit operator UIntPtr(ulong value)
		{
			return new UIntPtr(value);
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x00068A7D File Offset: 0x00066C7D
		[CLSCompliant(false)]
		public unsafe static explicit operator UIntPtr(void* value)
		{
			return new UIntPtr(value);
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x00068A85 File Offset: 0x00066C85
		[CLSCompliant(false)]
		public unsafe static explicit operator void*(UIntPtr value)
		{
			return value.ToPointer();
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x00068A8E File Offset: 0x00066C8E
		public static explicit operator UIntPtr(uint value)
		{
			return new UIntPtr(value);
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001C06 RID: 7174 RVA: 0x0005FD35 File Offset: 0x0005DF35
		public unsafe static int Size
		{
			get
			{
				return sizeof(void*);
			}
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x00068A96 File Offset: 0x00066C96
		public unsafe static UIntPtr Add(UIntPtr pointer, int offset)
		{
			return (UIntPtr)((void*)((byte*)((void*)pointer) + offset));
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x00068AA5 File Offset: 0x00066CA5
		public unsafe static UIntPtr Subtract(UIntPtr pointer, int offset)
		{
			return (UIntPtr)((void*)((byte*)((void*)pointer) - offset));
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x00068A96 File Offset: 0x00066C96
		public unsafe static UIntPtr operator +(UIntPtr pointer, int offset)
		{
			return (UIntPtr)((void*)((byte*)((void*)pointer) + offset));
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x00068AA5 File Offset: 0x00066CA5
		public unsafe static UIntPtr operator -(UIntPtr pointer, int offset)
		{
			return (UIntPtr)((void*)((byte*)((void*)pointer) - offset));
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x00068AB4 File Offset: 0x00066CB4
		bool IEquatable<UIntPtr>.Equals(UIntPtr other)
		{
			return this._pointer == other._pointer;
		}

		// Token: 0x040019A3 RID: 6563
		public static readonly UIntPtr Zero = new UIntPtr(0U);

		// Token: 0x040019A4 RID: 6564
		private unsafe readonly void* _pointer;
	}
}
