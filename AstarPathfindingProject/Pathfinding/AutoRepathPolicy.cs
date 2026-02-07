using System;
using Pathfinding.Drawing;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x0200006B RID: 107
	[Serializable]
	public class AutoRepathPolicy
	{
		// Token: 0x060003A8 RID: 936 RVA: 0x00012A14 File Offset: 0x00010C14
		public virtual bool ShouldRecalculatePath(Vector3 position, float radius, Vector3 destination, float time)
		{
			if (this.mode == AutoRepathPolicy.Mode.Never || float.IsPositiveInfinity(destination.x))
			{
				return false;
			}
			float num = time - this.lastRepathTime;
			if (this.mode == AutoRepathPolicy.Mode.EveryNSeconds)
			{
				return num >= this.period;
			}
			float f = (destination - this.lastDestination).sqrMagnitude / Mathf.Max((position - this.lastDestination).sqrMagnitude, radius * radius) * (this.sensitivity * this.sensitivity);
			if (float.IsNaN(f))
			{
				f = 0f;
			}
			return num >= this.maximumPeriod * (1f - Mathf.Sqrt(f));
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00012ABF File Offset: 0x00010CBF
		public virtual void Reset()
		{
			this.lastRepathTime = float.NegativeInfinity;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00012ACC File Offset: 0x00010CCC
		public virtual void DidRecalculatePath(Vector3 destination, float time)
		{
			this.lastRepathTime = time;
			this.lastDestination = destination;
			this.lastRepathTime -= (UnityEngine.Random.value - 0.5f) * 0.3f * ((this.mode == AutoRepathPolicy.Mode.Dynamic) ? this.maximumPeriod : this.period);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00012B20 File Offset: 0x00010D20
		public void DrawGizmos(CommandBuilder draw, Vector3 position, float radius, NativeMovementPlane movementPlane)
		{
			if (this.visualizeSensitivity && !float.IsPositiveInfinity(this.lastDestination.x))
			{
				float radius2 = Mathf.Sqrt(Mathf.Max((position - this.lastDestination).sqrMagnitude, radius * radius) / (this.sensitivity * this.sensitivity));
				draw.Circle(this.lastDestination, movementPlane.ToWorld(float2.zero, 1f), radius2, Color.magenta);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00012BA0 File Offset: 0x00010DA0
		public AutoRepathPolicy Clone()
		{
			return base.MemberwiseClone() as AutoRepathPolicy;
		}

		// Token: 0x0400026B RID: 619
		public AutoRepathPolicy.Mode mode = AutoRepathPolicy.Mode.Dynamic;

		// Token: 0x0400026C RID: 620
		[FormerlySerializedAs("interval")]
		public float period = 0.5f;

		// Token: 0x0400026D RID: 621
		public float sensitivity = 10f;

		// Token: 0x0400026E RID: 622
		[FormerlySerializedAs("maximumInterval")]
		public float maximumPeriod = 2f;

		// Token: 0x0400026F RID: 623
		public bool visualizeSensitivity;

		// Token: 0x04000270 RID: 624
		private Vector3 lastDestination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x04000271 RID: 625
		private float lastRepathTime = float.NegativeInfinity;

		// Token: 0x0200006C RID: 108
		public enum Mode : byte
		{
			// Token: 0x04000273 RID: 627
			Never,
			// Token: 0x04000274 RID: 628
			EveryNSeconds,
			// Token: 0x04000275 RID: 629
			Dynamic
		}
	}
}
