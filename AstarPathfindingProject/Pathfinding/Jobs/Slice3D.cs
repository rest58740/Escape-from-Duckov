using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Jobs
{
	// Token: 0x0200016E RID: 366
	public readonly struct Slice3D
	{
		// Token: 0x06000AB2 RID: 2738 RVA: 0x0003CC48 File Offset: 0x0003AE48
		public Slice3D(IntBounds outer, IntBounds slice)
		{
			this = new Slice3D(outer.size, slice.Offset(-outer.min));
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0003CC69 File Offset: 0x0003AE69
		public Slice3D(int3 outerSize, IntBounds slice)
		{
			this.outerSize = outerSize;
			this.slice = slice;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000035CE File Offset: 0x000017CE
		public void AssertMatchesOuter<[IsUnmanaged] T>(UnsafeSpan<T> values) where T : struct, ValueType
		{
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000035CE File Offset: 0x000017CE
		public void AssertMatchesOuter<T>(NativeArray<T> values) where T : struct
		{
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000035CE File Offset: 0x000017CE
		public void AssertMatchesInner<T>(NativeArray<T> values) where T : struct
		{
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000035CE File Offset: 0x000017CE
		public void AssertSameSize(Slice3D other)
		{
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0003CC7C File Offset: 0x0003AE7C
		public int InnerCoordinateToOuterIndex(int x, int y, int z)
		{
			ValueTuple<int, int, int> outerStrides = this.outerStrides;
			int item = outerStrides.Item1;
			int item2 = outerStrides.Item2;
			int item3 = outerStrides.Item3;
			return (x + this.slice.min.x) * item + (y + this.slice.min.y) * item2 + (z + this.slice.min.z) * item3;
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0003CCE4 File Offset: 0x0003AEE4
		public int length
		{
			get
			{
				return this.slice.size.x * this.slice.size.y * this.slice.size.z;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x0003CD2C File Offset: 0x0003AF2C
		public ValueTuple<int, int, int> outerStrides
		{
			get
			{
				return new ValueTuple<int, int, int>(1, this.outerSize.x * this.outerSize.z, this.outerSize.x);
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0003CD58 File Offset: 0x0003AF58
		public ValueTuple<int, int, int> innerStrides
		{
			get
			{
				return new ValueTuple<int, int, int>(1, this.slice.size.x * this.slice.size.z, this.slice.size.x);
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0003CDA8 File Offset: 0x0003AFA8
		public int outerStartIndex
		{
			get
			{
				ValueTuple<int, int, int> outerStrides = this.outerStrides;
				int item = outerStrides.Item1;
				int item2 = outerStrides.Item2;
				int item3 = outerStrides.Item3;
				return this.slice.min.x * item + this.slice.min.y * item2 + this.slice.min.z * item3;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0003CE08 File Offset: 0x0003B008
		public bool coversEverything
		{
			get
			{
				return math.all(this.slice.size == this.outerSize);
			}
		}

		// Token: 0x04000730 RID: 1840
		public readonly int3 outerSize;

		// Token: 0x04000731 RID: 1841
		public readonly IntBounds slice;
	}
}
