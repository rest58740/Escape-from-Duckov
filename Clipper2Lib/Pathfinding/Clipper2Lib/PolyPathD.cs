using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000027 RID: 39
	[NullableContext(2)]
	[Nullable(0)]
	public class PolyPathD : PolyPathBase
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000ABC4 File Offset: 0x00008DC4
		// (set) Token: 0x06000175 RID: 373 RVA: 0x0000ABCC File Offset: 0x00008DCC
		internal double Scale { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000ABD5 File Offset: 0x00008DD5
		// (set) Token: 0x06000177 RID: 375 RVA: 0x0000ABDD File Offset: 0x00008DDD
		public PathD Polygon { get; private set; }

		// Token: 0x06000178 RID: 376 RVA: 0x0000ABE6 File Offset: 0x00008DE6
		public PolyPathD(PolyPathBase parent = null) : base(parent)
		{
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000ABF0 File Offset: 0x00008DF0
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override PolyPathBase AddChild(List<Point64> p)
		{
			PolyPathBase polyPathBase = new PolyPathD(this);
			(polyPathBase as PolyPathD).Scale = this.Scale;
			(polyPathBase as PolyPathD).Polygon = Clipper.ScalePathD(p, 1.0 / this.Scale);
			this._childs.Add(polyPathBase);
			return polyPathBase;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000AC44 File Offset: 0x00008E44
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public PolyPathBase AddChild(PathD p)
		{
			PolyPathBase polyPathBase = new PolyPathD(this);
			(polyPathBase as PolyPathD).Scale = this.Scale;
			(polyPathBase as PolyPathD).Polygon = p;
			this._childs.Add(polyPathBase);
			return polyPathBase;
		}

		// Token: 0x17000010 RID: 16
		[Nullable(1)]
		[IndexerName("Child")]
		public PolyPathD this[int index]
		{
			[NullableContext(1)]
			get
			{
				if (index < 0 || index >= this._childs.Count)
				{
					throw new InvalidOperationException();
				}
				return (PolyPathD)this._childs[index];
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000ACB0 File Offset: 0x00008EB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double Area()
		{
			double num = (this.Polygon == null) ? 0.0 : Clipper.Area(this.Polygon);
			foreach (PolyPathBase polyPathBase in this._childs)
			{
				PolyPathD polyPathD = (PolyPathD)polyPathBase;
				num += polyPathD.Area();
			}
			return num;
		}
	}
}
