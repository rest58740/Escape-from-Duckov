using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	// Token: 0x0200055F RID: 1375
	[ComVisible(true)]
	public interface IRemotingTypeInfo
	{
		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060035E5 RID: 13797
		// (set) Token: 0x060035E6 RID: 13798
		string TypeName { get; set; }

		// Token: 0x060035E7 RID: 13799
		bool CanCastTo(Type fromType, object o);
	}
}
