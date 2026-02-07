using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006CC RID: 1740
	public readonly struct OSPlatform : IEquatable<OSPlatform>
	{
		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06004002 RID: 16386 RVA: 0x000E04EE File Offset: 0x000DE6EE
		public static OSPlatform Linux { get; } = new OSPlatform("LINUX");

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06004003 RID: 16387 RVA: 0x000E04F5 File Offset: 0x000DE6F5
		public static OSPlatform OSX { get; } = new OSPlatform("OSX");

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06004004 RID: 16388 RVA: 0x000E04FC File Offset: 0x000DE6FC
		public static OSPlatform Windows { get; } = new OSPlatform("WINDOWS");

		// Token: 0x06004005 RID: 16389 RVA: 0x000E0503 File Offset: 0x000DE703
		private OSPlatform(string osPlatform)
		{
			if (osPlatform == null)
			{
				throw new ArgumentNullException("osPlatform");
			}
			if (osPlatform.Length == 0)
			{
				throw new ArgumentException("Value cannot be empty.", "osPlatform");
			}
			this._osPlatform = osPlatform;
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x000E0532 File Offset: 0x000DE732
		public static OSPlatform Create(string osPlatform)
		{
			return new OSPlatform(osPlatform);
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x000E053A File Offset: 0x000DE73A
		public bool Equals(OSPlatform other)
		{
			return this.Equals(other._osPlatform);
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x000E0548 File Offset: 0x000DE748
		internal bool Equals(string other)
		{
			return string.Equals(this._osPlatform, other, StringComparison.Ordinal);
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x000E0557 File Offset: 0x000DE757
		public override bool Equals(object obj)
		{
			return obj is OSPlatform && this.Equals((OSPlatform)obj);
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x000E056F File Offset: 0x000DE76F
		public override int GetHashCode()
		{
			if (this._osPlatform != null)
			{
				return this._osPlatform.GetHashCode();
			}
			return 0;
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x000E0586 File Offset: 0x000DE786
		public override string ToString()
		{
			return this._osPlatform ?? string.Empty;
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x000E0597 File Offset: 0x000DE797
		public static bool operator ==(OSPlatform left, OSPlatform right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x000E05A1 File Offset: 0x000DE7A1
		public static bool operator !=(OSPlatform left, OSPlatform right)
		{
			return !(left == right);
		}

		// Token: 0x04002A04 RID: 10756
		private readonly string _osPlatform;
	}
}
