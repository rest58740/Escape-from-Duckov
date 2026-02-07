using System;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000009 RID: 9
	public static class DOTweenCYInstruction
	{
		// Token: 0x02000059 RID: 89
		public class WaitForCompletion : CustomYieldInstruction
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x06000141 RID: 321 RVA: 0x00005776 File Offset: 0x00003976
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && !TweenExtensions.IsComplete(this.t);
				}
			}

			// Token: 0x06000142 RID: 322 RVA: 0x00005795 File Offset: 0x00003995
			public WaitForCompletion(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x04000087 RID: 135
			private readonly Tween t;
		}

		// Token: 0x0200005A RID: 90
		public class WaitForRewind : CustomYieldInstruction
		{
			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000143 RID: 323 RVA: 0x000057A4 File Offset: 0x000039A4
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && (!this.t.playedOnce || this.t.position * (float)(TweenExtensions.CompletedLoops(this.t) + 1) > 0f);
				}
			}

			// Token: 0x06000144 RID: 324 RVA: 0x000057F0 File Offset: 0x000039F0
			public WaitForRewind(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x04000088 RID: 136
			private readonly Tween t;
		}

		// Token: 0x0200005B RID: 91
		public class WaitForKill : CustomYieldInstruction
		{
			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000145 RID: 325 RVA: 0x000057FF File Offset: 0x000039FF
			public override bool keepWaiting
			{
				get
				{
					return this.t.active;
				}
			}

			// Token: 0x06000146 RID: 326 RVA: 0x0000580C File Offset: 0x00003A0C
			public WaitForKill(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x04000089 RID: 137
			private readonly Tween t;
		}

		// Token: 0x0200005C RID: 92
		public class WaitForElapsedLoops : CustomYieldInstruction
		{
			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000147 RID: 327 RVA: 0x0000581B File Offset: 0x00003A1B
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && TweenExtensions.CompletedLoops(this.t) < this.elapsedLoops;
				}
			}

			// Token: 0x06000148 RID: 328 RVA: 0x0000583F File Offset: 0x00003A3F
			public WaitForElapsedLoops(Tween tween, int elapsedLoops)
			{
				this.t = tween;
				this.elapsedLoops = elapsedLoops;
			}

			// Token: 0x0400008A RID: 138
			private readonly Tween t;

			// Token: 0x0400008B RID: 139
			private readonly int elapsedLoops;
		}

		// Token: 0x0200005D RID: 93
		public class WaitForPosition : CustomYieldInstruction
		{
			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000149 RID: 329 RVA: 0x00005855 File Offset: 0x00003A55
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && this.t.position * (float)(TweenExtensions.CompletedLoops(this.t) + 1) < this.position;
				}
			}

			// Token: 0x0600014A RID: 330 RVA: 0x00005888 File Offset: 0x00003A88
			public WaitForPosition(Tween tween, float position)
			{
				this.t = tween;
				this.position = position;
			}

			// Token: 0x0400008C RID: 140
			private readonly Tween t;

			// Token: 0x0400008D RID: 141
			private readonly float position;
		}

		// Token: 0x0200005E RID: 94
		public class WaitForStart : CustomYieldInstruction
		{
			// Token: 0x17000006 RID: 6
			// (get) Token: 0x0600014B RID: 331 RVA: 0x0000589E File Offset: 0x00003A9E
			public override bool keepWaiting
			{
				get
				{
					return this.t.active && !this.t.playedOnce;
				}
			}

			// Token: 0x0600014C RID: 332 RVA: 0x000058BD File Offset: 0x00003ABD
			public WaitForStart(Tween tween)
			{
				this.t = tween;
			}

			// Token: 0x0400008E RID: 142
			private readonly Tween t;
		}
	}
}
