using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000022 RID: 34
	public static class Validate
	{
		// Token: 0x06000322 RID: 802 RVA: 0x00008F4C File Offset: 0x0000714C
		[Conditional("UNITY_ASSERTIONS")]
		public static void Disable(this OptionalWarning type)
		{
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00008F4E File Offset: 0x0000714E
		[Conditional("UNITY_ASSERTIONS")]
		public static void Enable(this OptionalWarning type)
		{
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00008F50 File Offset: 0x00007150
		[Conditional("UNITY_ASSERTIONS")]
		public static void SetEnabled(this OptionalWarning type, bool enable)
		{
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00008F52 File Offset: 0x00007152
		[Conditional("UNITY_ASSERTIONS")]
		public static void Log(this OptionalWarning type, string message, object context = null)
		{
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00008F54 File Offset: 0x00007154
		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertNotLegacy(AnimationClip clip)
		{
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00008F56 File Offset: 0x00007156
		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertRoot(AnimancerNode node, AnimancerPlayable root)
		{
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00008F58 File Offset: 0x00007158
		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertPlayable(AnimancerNode node)
		{
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00008F5A File Offset: 0x0000715A
		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertCanRemoveChild(AnimancerState state, IList<AnimancerState> childStates, int childCount)
		{
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00008F5C File Offset: 0x0000715C
		public static void ValueRule(ref float value, Validate.Value rule)
		{
			switch (rule)
			{
			default:
				return;
			case Validate.Value.ZeroToOne:
				if (value < 0f)
				{
					value = 0f;
					return;
				}
				if (value > 1f)
				{
					value = 1f;
					return;
				}
				break;
			case Validate.Value.IsNotNegative:
				if (value < 0f)
				{
					value = 0f;
					return;
				}
				break;
			case Validate.Value.IsFinite:
				if (float.IsNaN(value))
				{
					value = 0f;
					return;
				}
				if (float.IsPositiveInfinity(value))
				{
					value = float.MaxValue;
					return;
				}
				if (float.IsNegativeInfinity(value))
				{
					value = float.MinValue;
					return;
				}
				break;
			case Validate.Value.IsFiniteOrNaN:
				if (float.IsPositiveInfinity(value))
				{
					value = float.MaxValue;
					return;
				}
				if (float.IsNegativeInfinity(value))
				{
					value = float.MinValue;
				}
				break;
			}
		}

		// Token: 0x0200008D RID: 141
		public enum Value
		{
			// Token: 0x0400013B RID: 315
			Any,
			// Token: 0x0400013C RID: 316
			ZeroToOne,
			// Token: 0x0400013D RID: 317
			IsNotNegative,
			// Token: 0x0400013E RID: 318
			IsFinite,
			// Token: 0x0400013F RID: 319
			IsFiniteOrNaN
		}
	}
}
