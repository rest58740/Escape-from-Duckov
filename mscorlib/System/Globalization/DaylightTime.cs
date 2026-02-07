using System;

namespace System.Globalization
{
	// Token: 0x02000962 RID: 2402
	[Serializable]
	public class DaylightTime
	{
		// Token: 0x0600553A RID: 21818 RVA: 0x0011DE48 File Offset: 0x0011C048
		public DaylightTime(DateTime start, DateTime end, TimeSpan delta)
		{
			this._start = start;
			this._end = end;
			this._delta = delta;
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x0600553B RID: 21819 RVA: 0x0011DE65 File Offset: 0x0011C065
		public DateTime Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x0600553C RID: 21820 RVA: 0x0011DE6D File Offset: 0x0011C06D
		public DateTime End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x0600553D RID: 21821 RVA: 0x0011DE75 File Offset: 0x0011C075
		public TimeSpan Delta
		{
			get
			{
				return this._delta;
			}
		}

		// Token: 0x04003484 RID: 13444
		private readonly DateTime _start;

		// Token: 0x04003485 RID: 13445
		private readonly DateTime _end;

		// Token: 0x04003486 RID: 13446
		private readonly TimeSpan _delta;
	}
}
