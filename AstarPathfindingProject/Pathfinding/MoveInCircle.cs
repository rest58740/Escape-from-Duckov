using System;
using Pathfinding.Drawing;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000013 RID: 19
	[UniqueComponent(tag = "ai.destination")]
	[AddComponentMenu("Pathfinding/AI/Behaviors/MoveInCircle")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/moveincircle.html")]
	public class MoveInCircle : VersionedMonoBehaviour
	{
		// Token: 0x06000084 RID: 132 RVA: 0x000045FB File Offset: 0x000027FB
		private void OnEnable()
		{
			this.ai = base.GetComponent<IAstarAI>();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000460C File Offset: 0x0000280C
		private void Update()
		{
			Vector3 normalized = (this.ai.position - this.target.position).normalized;
			Vector3 a = Vector3.Cross(normalized, this.target.up);
			this.ai.destination = this.target.position + normalized * this.radius + a * this.offset;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004687 File Offset: 0x00002887
		public override void DrawGizmos()
		{
			if (this.target)
			{
				Draw.Circle(this.target.position, this.target.up, this.radius, Color.white);
			}
		}

		// Token: 0x04000072 RID: 114
		public Transform target;

		// Token: 0x04000073 RID: 115
		public float radius = 5f;

		// Token: 0x04000074 RID: 116
		public float offset = 2f;

		// Token: 0x04000075 RID: 117
		private IAstarAI ai;
	}
}
