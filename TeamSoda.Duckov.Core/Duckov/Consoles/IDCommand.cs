using System;

namespace Duckov.Consoles
{
	// Token: 0x0200031C RID: 796
	public interface IDCommand
	{
		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001A1F RID: 6687
		string CommandWord { get; }

		// Token: 0x06001A20 RID: 6688
		string Execute(DConsole console, string[] args);
	}
}
