using System;
using System.Collections.Generic;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009EE RID: 2542
	public class EventCommandEventArgs : EventArgs
	{
		// Token: 0x06005AB4 RID: 23220 RVA: 0x0013444C File Offset: 0x0013264C
		private EventCommandEventArgs()
		{
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06005AB5 RID: 23221 RVA: 0x000479FC File Offset: 0x00045BFC
		public IDictionary<string, string> Arguments
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06005AB6 RID: 23222 RVA: 0x000479FC File Offset: 0x00045BFC
		public EventCommand Command
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06005AB7 RID: 23223 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool DisableEvent(int eventId)
		{
			return true;
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool EnableEvent(int eventId)
		{
			return true;
		}
	}
}
