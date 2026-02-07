using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200000B RID: 11
	public readonly struct AsyncUnit : IEquatable<AsyncUnit>
	{
		// Token: 0x06000033 RID: 51 RVA: 0x000027B9 File Offset: 0x000009B9
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000027BC File Offset: 0x000009BC
		public bool Equals(AsyncUnit other)
		{
			return true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000027BF File Offset: 0x000009BF
		public override string ToString()
		{
			return "()";
		}

		// Token: 0x04000015 RID: 21
		public static readonly AsyncUnit Default;
	}
}
