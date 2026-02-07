using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000026 RID: 38
	[NullableContext(2)]
	[Nullable(0)]
	public class PolyPath64 : PolyPathBase
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		public List<Point64> Polygon { get; private set; }

		// Token: 0x0600016F RID: 367 RVA: 0x0000AAB9 File Offset: 0x00008CB9
		public PolyPath64(PolyPathBase parent = null) : base(parent)
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override PolyPathBase AddChild(List<Point64> p)
		{
			PolyPathBase polyPathBase = new PolyPath64(this);
			(polyPathBase as PolyPath64).Polygon = p;
			this._childs.Add(polyPathBase);
			return polyPathBase;
		}

		// Token: 0x1700000D RID: 13
		[Nullable(1)]
		public PolyPath64 this[int index]
		{
			[NullableContext(1)]
			get
			{
				if (index < 0 || index >= this._childs.Count)
				{
					throw new InvalidOperationException();
				}
				return (PolyPath64)this._childs[index];
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000AB1C File Offset: 0x00008D1C
		[NullableContext(1)]
		public PolyPath64 Child(int index)
		{
			if (index < 0 || index >= this._childs.Count)
			{
				throw new InvalidOperationException();
			}
			return (PolyPath64)this._childs[index];
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000AB48 File Offset: 0x00008D48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double Area()
		{
			double num = (this.Polygon == null) ? 0.0 : Clipper.Area(this.Polygon);
			foreach (PolyPathBase polyPathBase in this._childs)
			{
				PolyPath64 polyPath = (PolyPath64)polyPathBase;
				num += polyPath.Area();
			}
			return num;
		}
	}
}
