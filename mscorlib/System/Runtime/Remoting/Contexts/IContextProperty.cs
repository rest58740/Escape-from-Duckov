using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000595 RID: 1429
	[ComVisible(true)]
	public interface IContextProperty
	{
		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060037D1 RID: 14289
		string Name { get; }

		// Token: 0x060037D2 RID: 14290
		void Freeze(Context newContext);

		// Token: 0x060037D3 RID: 14291
		bool IsNewContextOK(Context newCtx);
	}
}
