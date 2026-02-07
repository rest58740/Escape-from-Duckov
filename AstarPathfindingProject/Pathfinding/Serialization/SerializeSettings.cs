using System;

namespace Pathfinding.Serialization
{
	// Token: 0x0200023B RID: 571
	public class SerializeSettings
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x000553FB File Offset: 0x000535FB
		public static SerializeSettings Settings
		{
			get
			{
				return new SerializeSettings
				{
					nodes = false
				};
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00055409 File Offset: 0x00053609
		public static SerializeSettings NodesAndSettings
		{
			get
			{
				return new SerializeSettings();
			}
		}

		// Token: 0x04000A7B RID: 2683
		public bool nodes = true;
	}
}
