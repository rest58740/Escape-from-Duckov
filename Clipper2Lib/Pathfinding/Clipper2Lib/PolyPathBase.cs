using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000025 RID: 37
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class PolyPathBase : IEnumerable
	{
		// Token: 0x06000162 RID: 354 RVA: 0x0000A868 File Offset: 0x00008A68
		public IEnumerator GetEnumerator()
		{
			return new PolyPathBase.NodeEnumerator(this._childs);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000A875 File Offset: 0x00008A75
		public bool IsHole
		{
			get
			{
				return this.GetIsHole();
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000A87D File Offset: 0x00008A7D
		[NullableContext(2)]
		public PolyPathBase(PolyPathBase parent = null)
		{
			this._parent = parent;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000A898 File Offset: 0x00008A98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int GetLevel()
		{
			int num = 0;
			for (PolyPathBase parent = this._parent; parent != null; parent = parent._parent)
			{
				num++;
			}
			return num;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000A8BF File Offset: 0x00008ABF
		public int Level
		{
			get
			{
				return this.GetLevel();
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000A8C8 File Offset: 0x00008AC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool GetIsHole()
		{
			int level = this.GetLevel();
			return level != 0 && (level & 1) == 0;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000168 RID: 360 RVA: 0x0000A8E7 File Offset: 0x00008AE7
		public int Count
		{
			get
			{
				return this._childs.Count;
			}
		}

		// Token: 0x06000169 RID: 361
		public abstract PolyPathBase AddChild(List<Point64> p);

		// Token: 0x0600016A RID: 362 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			this._childs.Clear();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000A904 File Offset: 0x00008B04
		internal string ToStringInternal(int idx, int level)
		{
			string text = "";
			string text2 = "";
			string text3 = "s";
			if (this._childs.Count == 1)
			{
				text3 = "";
			}
			text2 = text2.PadLeft(level * 2);
			if ((level & 1) == 0)
			{
				text += string.Format("{0}+- hole ({1}) contains {2} nested polygon{3}.\n", new object[]
				{
					text2,
					idx,
					this._childs.Count,
					text3
				});
			}
			else
			{
				text += string.Format("{0}+- polygon ({1}) contains {2} hole{3}.\n", new object[]
				{
					text2,
					idx,
					this._childs.Count,
					text3
				});
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (this._childs[i].Count > 0)
				{
					text += this._childs[i].ToStringInternal(i, level + 1);
				}
			}
			return text;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000AA00 File Offset: 0x00008C00
		public override string ToString()
		{
			if (this.Level > 0)
			{
				return "";
			}
			string text = "s";
			if (this._childs.Count == 1)
			{
				text = "";
			}
			string str = string.Format("Polytree with {0} polygon{1}.\n", new object[]
			{
				this._childs.Count,
				text
			});
			for (int i = 0; i < this.Count; i++)
			{
				if (this._childs[i].Count > 0)
				{
					str += this._childs[i].ToStringInternal(i, 1);
				}
			}
			return str + "\n";
		}

		// Token: 0x04000089 RID: 137
		[Nullable(2)]
		internal PolyPathBase _parent;

		// Token: 0x0400008A RID: 138
		internal List<PolyPathBase> _childs = new List<PolyPathBase>();

		// Token: 0x02000037 RID: 55
		[Nullable(0)]
		private class NodeEnumerator : IEnumerator
		{
			// Token: 0x060001DF RID: 479 RVA: 0x0000E446 File Offset: 0x0000C646
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public NodeEnumerator(List<PolyPathBase> nodes)
			{
				this._nodes = new List<PolyPathBase>(nodes);
			}

			// Token: 0x060001E0 RID: 480 RVA: 0x0000E461 File Offset: 0x0000C661
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				this.position++;
				return this.position < this._nodes.Count;
			}

			// Token: 0x060001E1 RID: 481 RVA: 0x0000E484 File Offset: 0x0000C684
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Reset()
			{
				this.position = -1;
			}

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000E48D File Offset: 0x0000C68D
			public object Current
			{
				get
				{
					if (this.position < 0 || this.position >= this._nodes.Count)
					{
						throw new InvalidOperationException();
					}
					return this._nodes[this.position];
				}
			}

			// Token: 0x040000C0 RID: 192
			private int position = -1;

			// Token: 0x040000C1 RID: 193
			private readonly List<PolyPathBase> _nodes;
		}
	}
}
