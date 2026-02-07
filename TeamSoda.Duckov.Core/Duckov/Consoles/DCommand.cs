using System;
using UnityEngine;

namespace Duckov.Consoles
{
	// Token: 0x02000319 RID: 793
	public abstract class DCommand : ScriptableObject, IDCommand
	{
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001A0B RID: 6667
		public abstract string CommandWord { get; }

		// Token: 0x06001A0C RID: 6668
		public abstract string Execute(DConsole console, string[] args);
	}
}
