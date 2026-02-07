using System;
using Unity.Collections;
using UnityEngine;

namespace UI_Spline_Renderer
{
	// Token: 0x02000008 RID: 8
	internal readonly struct NativeCurve : IDisposable
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002833 File Offset: 0x00000A33
		public NativeCurve(AnimationCurve c, Allocator alloc)
		{
			this.Keys = new NativeArray<Keyframe>(c.keys, alloc);
			this.owner = 1;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000284E File Offset: 0x00000A4E
		public NativeCurve(int size, Allocator alloc)
		{
			this.Keys = new NativeArray<Keyframe>(size, alloc, 1);
			this.owner = 1;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002865 File Offset: 0x00000A65
		public NativeCurve(Keyframe[] keyframes, Allocator alloc)
		{
			this.Keys = new NativeArray<Keyframe>(keyframes, alloc);
			this.owner = 1;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000287B File Offset: 0x00000A7B
		public NativeCurve(NativeArray<Keyframe> keyframes, Allocator alloc)
		{
			this.Keys = new NativeArray<Keyframe>(keyframes, alloc);
			this.owner = 1;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002891 File Offset: 0x00000A91
		public NativeCurve(NativeArray<Keyframe> keyframes)
		{
			this.Keys = keyframes;
			this.owner = 1;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028A1 File Offset: 0x00000AA1
		public NativeCurve(NativeCurve other)
		{
			this.Keys = other.Keys;
			this.owner = 1;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000028B6 File Offset: 0x00000AB6
		public NativeCurve(NativeCurve other, Allocator alloc)
		{
			this.Keys = new NativeArray<Keyframe>(other.Keys, alloc);
			this.owner = 1;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000028D4 File Offset: 0x00000AD4
		public void Dispose()
		{
			if (this.owner != 0)
			{
				this.Keys.Dispose();
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000028F7 File Offset: 0x00000AF7
		public float Evaluate(float time)
		{
			return CurveSampling.ThreadSafe.Evaluate(this.Keys, time);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002908 File Offset: 0x00000B08
		public int Length
		{
			get
			{
				return this.Keys.Length;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002924 File Offset: 0x00000B24
		public float Duration
		{
			get
			{
				return this.Keys[this.Length - 1].time - this.Keys[0].time;
			}
		}

		// Token: 0x0400000A RID: 10
		private const int TRUE = 1;

		// Token: 0x0400000B RID: 11
		private const int FALSE = 1;

		// Token: 0x0400000C RID: 12
		public readonly NativeArray<Keyframe> Keys;

		// Token: 0x0400000D RID: 13
		private readonly int owner;
	}
}
