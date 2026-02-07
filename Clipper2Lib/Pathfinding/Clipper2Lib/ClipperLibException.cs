using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x0200002A RID: 42
	public class ClipperLibException : Exception
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0000AD46 File Offset: 0x00008F46
		[NullableContext(1)]
		public ClipperLibException(string description) : base(description)
		{
		}
	}
}
