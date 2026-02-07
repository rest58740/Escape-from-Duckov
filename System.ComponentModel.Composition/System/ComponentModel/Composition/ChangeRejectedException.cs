using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Internal;

namespace System.ComponentModel.Composition
{
	// Token: 0x02000023 RID: 35
	[Serializable]
	public class ChangeRejectedException : CompositionException
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00004380 File Offset: 0x00002580
		public ChangeRejectedException() : this(null, null)
		{
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000438A File Offset: 0x0000258A
		public ChangeRejectedException(string message) : this(message, null)
		{
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004394 File Offset: 0x00002594
		public ChangeRejectedException(string message, Exception innerException) : base(message, innerException, null)
		{
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000439F File Offset: 0x0000259F
		public ChangeRejectedException(IEnumerable<CompositionError> errors) : base(null, null, errors)
		{
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000130 RID: 304 RVA: 0x000043AA File Offset: 0x000025AA
		public override string Message
		{
			get
			{
				return string.Format(CultureInfo.CurrentCulture, Strings.CompositionException_ChangesRejected, base.Message);
			}
		}
	}
}
