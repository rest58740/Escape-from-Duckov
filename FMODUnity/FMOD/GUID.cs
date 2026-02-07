using System;

namespace FMOD
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public struct GUID : IEquatable<GUID>
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020D4 File Offset: 0x000002D4
		public GUID(Guid guid)
		{
			byte[] value = guid.ToByteArray();
			this.Data1 = BitConverter.ToInt32(value, 0);
			this.Data2 = BitConverter.ToInt32(value, 4);
			this.Data3 = BitConverter.ToInt32(value, 8);
			this.Data4 = BitConverter.ToInt32(value, 12);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000211E File Offset: 0x0000031E
		public static GUID Parse(string s)
		{
			return new GUID(new Guid(s));
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000212B File Offset: 0x0000032B
		public bool IsNull
		{
			get
			{
				return this.Data1 == 0 && this.Data2 == 0 && this.Data3 == 0 && this.Data4 == 0;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002150 File Offset: 0x00000350
		public override bool Equals(object other)
		{
			return other is GUID && this.Equals((GUID)other);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002168 File Offset: 0x00000368
		public bool Equals(GUID other)
		{
			return this.Data1 == other.Data1 && this.Data2 == other.Data2 && this.Data3 == other.Data3 && this.Data4 == other.Data4;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021A4 File Offset: 0x000003A4
		public static bool operator ==(GUID a, GUID b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021AE File Offset: 0x000003AE
		public static bool operator !=(GUID a, GUID b)
		{
			return !a.Equals(b);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021BB File Offset: 0x000003BB
		public override int GetHashCode()
		{
			return this.Data1 ^ this.Data2 ^ this.Data3 ^ this.Data4;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021D8 File Offset: 0x000003D8
		public static implicit operator Guid(GUID guid)
		{
			return new Guid(guid.Data1, (short)(guid.Data2 & 65535), (short)(guid.Data2 >> 16 & 65535), (byte)(guid.Data3 & 255), (byte)(guid.Data3 >> 8 & 255), (byte)(guid.Data3 >> 16 & 255), (byte)(guid.Data3 >> 24 & 255), (byte)(guid.Data4 & 255), (byte)(guid.Data4 >> 8 & 255), (byte)(guid.Data4 >> 16 & 255), (byte)(guid.Data4 >> 24 & 255));
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002288 File Offset: 0x00000488
		public override string ToString()
		{
			return this.ToString("B");
		}

		// Token: 0x04000066 RID: 102
		public int Data1;

		// Token: 0x04000067 RID: 103
		public int Data2;

		// Token: 0x04000068 RID: 104
		public int Data3;

		// Token: 0x04000069 RID: 105
		public int Data4;
	}
}
