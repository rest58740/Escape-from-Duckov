using System;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x02000788 RID: 1928
	public struct EventRegistrationToken
	{
		// Token: 0x06004488 RID: 17544 RVA: 0x000E3B49 File Offset: 0x000E1D49
		internal EventRegistrationToken(ulong value)
		{
			this.m_value = value;
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06004489 RID: 17545 RVA: 0x000E3B52 File Offset: 0x000E1D52
		internal ulong Value
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x000E3B5A File Offset: 0x000E1D5A
		public static bool operator ==(EventRegistrationToken left, EventRegistrationToken right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x000E3B6F File Offset: 0x000E1D6F
		public static bool operator !=(EventRegistrationToken left, EventRegistrationToken right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x000E3B88 File Offset: 0x000E1D88
		public override bool Equals(object obj)
		{
			return obj is EventRegistrationToken && ((EventRegistrationToken)obj).Value == this.Value;
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x000E3BB5 File Offset: 0x000E1DB5
		public override int GetHashCode()
		{
			return this.m_value.GetHashCode();
		}

		// Token: 0x04002C24 RID: 11300
		internal ulong m_value;
	}
}
