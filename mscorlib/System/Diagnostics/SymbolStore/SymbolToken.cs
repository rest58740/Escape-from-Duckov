using System;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020009D4 RID: 2516
	public readonly struct SymbolToken
	{
		// Token: 0x06005A3C RID: 23100 RVA: 0x00134279 File Offset: 0x00132479
		public SymbolToken(int val)
		{
			this._token = val;
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x00134282 File Offset: 0x00132482
		public int GetToken()
		{
			return this._token;
		}

		// Token: 0x06005A3E RID: 23102 RVA: 0x00134282 File Offset: 0x00132482
		public override int GetHashCode()
		{
			return this._token;
		}

		// Token: 0x06005A3F RID: 23103 RVA: 0x0013428A File Offset: 0x0013248A
		public override bool Equals(object obj)
		{
			return obj is SymbolToken && this.Equals((SymbolToken)obj);
		}

		// Token: 0x06005A40 RID: 23104 RVA: 0x001342A2 File Offset: 0x001324A2
		public bool Equals(SymbolToken obj)
		{
			return obj._token == this._token;
		}

		// Token: 0x06005A41 RID: 23105 RVA: 0x001342B2 File Offset: 0x001324B2
		public static bool operator ==(SymbolToken a, SymbolToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06005A42 RID: 23106 RVA: 0x001342BC File Offset: 0x001324BC
		public static bool operator !=(SymbolToken a, SymbolToken b)
		{
			return !(a == b);
		}

		// Token: 0x040037BD RID: 14269
		private readonly int _token;
	}
}
