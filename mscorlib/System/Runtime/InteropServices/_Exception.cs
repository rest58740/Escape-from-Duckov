using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200076D RID: 1901
	[CLSCompliant(false)]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("b36b5c63-42ef-38bc-a07e-0b34c98f164a")]
	public interface _Exception
	{
		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x0600432E RID: 17198
		// (set) Token: 0x0600432F RID: 17199
		string HelpLink { get; set; }

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06004330 RID: 17200
		Exception InnerException { get; }

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06004331 RID: 17201
		string Message { get; }

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06004332 RID: 17202
		// (set) Token: 0x06004333 RID: 17203
		string Source { get; set; }

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06004334 RID: 17204
		string StackTrace { get; }

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06004335 RID: 17205
		MethodBase TargetSite { get; }

		// Token: 0x06004336 RID: 17206
		bool Equals(object obj);

		// Token: 0x06004337 RID: 17207
		Exception GetBaseException();

		// Token: 0x06004338 RID: 17208
		int GetHashCode();

		// Token: 0x06004339 RID: 17209
		void GetObjectData(SerializationInfo info, StreamingContext context);

		// Token: 0x0600433A RID: 17210
		Type GetType();

		// Token: 0x0600433B RID: 17211
		string ToString();
	}
}
