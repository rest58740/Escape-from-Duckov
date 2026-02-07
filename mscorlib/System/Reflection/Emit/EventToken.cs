using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000923 RID: 2339
	[ComVisible(true)]
	[Serializable]
	public readonly struct EventToken : IEquatable<EventToken>
	{
		// Token: 0x06005007 RID: 20487 RVA: 0x000FAA59 File Offset: 0x000F8C59
		internal EventToken(int val)
		{
			this.tokValue = val;
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x000FAA64 File Offset: 0x000F8C64
		public override bool Equals(object obj)
		{
			bool flag = obj is EventToken;
			if (flag)
			{
				EventToken eventToken = (EventToken)obj;
				flag = (this.tokValue == eventToken.tokValue);
			}
			return flag;
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x000FAA95 File Offset: 0x000F8C95
		public bool Equals(EventToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x000FAAA5 File Offset: 0x000F8CA5
		public static bool operator ==(EventToken a, EventToken b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x000FAAB8 File Offset: 0x000F8CB8
		public static bool operator !=(EventToken a, EventToken b)
		{
			return !object.Equals(a, b);
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x000FAACE File Offset: 0x000F8CCE
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x0600500D RID: 20493 RVA: 0x000FAACE File Offset: 0x000F8CCE
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x0400315B RID: 12635
		internal readonly int tokValue;

		// Token: 0x0400315C RID: 12636
		public static readonly EventToken Empty;
	}
}
