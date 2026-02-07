using System;

namespace Animancer
{
	// Token: 0x02000005 RID: 5
	public static class HybridAnimancerComponentExtensions
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x00002F1F File Offset: 0x0000111F
		public static void Update(this HybridAnimancerComponent animancer, float deltaTime)
		{
			animancer.Evaluate(deltaTime);
		}
	}
}
