using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	public struct MinMaxRangeFloat : IEquatable<MinMaxRangeFloat>
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006C31 File Offset: 0x00004E31
		public float minValue
		{
			get
			{
				return this.m_MinValue;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00006C39 File Offset: 0x00004E39
		public float maxValue
		{
			get
			{
				return this.m_MaxValue;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00006C41 File Offset: 0x00004E41
		public float randomValue
		{
			get
			{
				return UnityEngine.Random.Range(this.minValue, this.maxValue);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00006C54 File Offset: 0x00004E54
		public Vector2 asVector2
		{
			get
			{
				return new Vector2(this.minValue, this.maxValue);
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006C67 File Offset: 0x00004E67
		public float GetLerpedValue(float lerp01)
		{
			return Mathf.Lerp(this.minValue, this.maxValue, lerp01);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006C7B File Offset: 0x00004E7B
		public MinMaxRangeFloat(float min, float max)
		{
			this.m_MinValue = min;
			this.m_MaxValue = max;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006C8C File Offset: 0x00004E8C
		public override bool Equals(object obj)
		{
			if (obj is MinMaxRangeFloat)
			{
				MinMaxRangeFloat other = (MinMaxRangeFloat)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006CB1 File Offset: 0x00004EB1
		public bool Equals(MinMaxRangeFloat other)
		{
			return this.m_MinValue == other.m_MinValue && this.m_MaxValue == other.m_MaxValue;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006CD4 File Offset: 0x00004ED4
		public override int GetHashCode()
		{
			return new ValueTuple<float, float>(this.m_MinValue, this.m_MaxValue).GetHashCode();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006D00 File Offset: 0x00004F00
		public static bool operator ==(MinMaxRangeFloat lhs, MinMaxRangeFloat rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006D0A File Offset: 0x00004F0A
		public static bool operator !=(MinMaxRangeFloat lhs, MinMaxRangeFloat rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04000115 RID: 277
		[SerializeField]
		private float m_MinValue;

		// Token: 0x04000116 RID: 278
		[SerializeField]
		private float m_MaxValue;
	}
}
