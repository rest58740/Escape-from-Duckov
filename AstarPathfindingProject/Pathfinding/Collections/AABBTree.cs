using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pathfinding.Util;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding.Collections
{
	// Token: 0x02000267 RID: 615
	public class AABBTree<T>
	{
		// Token: 0x06000EB6 RID: 3766 RVA: 0x0005ABB0 File Offset: 0x00058DB0
		private static float ExpansionRequired(Bounds b, Bounds b2)
		{
			Bounds bounds = b;
			bounds.Encapsulate(b2);
			return bounds.size.x * bounds.size.y * bounds.size.z - b.size.x * b.size.y * b.size.z;
		}

		// Token: 0x17000206 RID: 518
		public T this[AABBTree<T>.Key key]
		{
			get
			{
				return this.nodes[key.node].value;
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0005AC30 File Offset: 0x00058E30
		public Bounds GetBounds(AABBTree<T>.Key key)
		{
			if (!key.isValid)
			{
				throw new ArgumentException("Key is not valid");
			}
			AABBTree<T>.Node node = this.nodes[key.node];
			if (!node.isAllocated)
			{
				throw new ArgumentException("Key does not point to an allocated node");
			}
			if (!node.isLeaf)
			{
				throw new ArgumentException("Key does not point to a leaf node");
			}
			return node.bounds;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0005AC94 File Offset: 0x00058E94
		private int AllocNode()
		{
			int result;
			if (!this.freeNodes.TryPop(out result))
			{
				int num = this.nodes.Length;
				Memory.Realloc<AABBTree<T>.Node>(ref this.nodes, Mathf.Max(8, this.nodes.Length * 2));
				for (int i = this.nodes.Length - 1; i >= num; i--)
				{
					this.FreeNode(i);
				}
				result = this.freeNodes.Pop();
			}
			return result;
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0005ACFD File Offset: 0x00058EFD
		private void FreeNode(int node)
		{
			this.nodes[node].isAllocated = false;
			this.nodes[node].value = default(T);
			this.freeNodes.Push(node);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0005AD34 File Offset: 0x00058F34
		public unsafe void Rebuild()
		{
			UnsafeSpan<int> unsafeSpan = new UnsafeSpan<int>(Allocator.Temp, this.nodes.Length);
			int num = 0;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i].isAllocated)
				{
					if (this.nodes[i].isLeaf)
					{
						*unsafeSpan[num++] = i;
					}
					else
					{
						this.FreeNode(i);
					}
				}
			}
			this.root = this.Rebuild(unsafeSpan.Slice(0, num), 536870911);
			this.rebuildCounter = Mathf.Max(64, num / 3);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0005ADD0 File Offset: 0x00058FD0
		public void Clear()
		{
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i].isAllocated)
				{
					this.FreeNode(i);
				}
			}
			this.root = -1;
			this.rebuildCounter = 64;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0005AE1C File Offset: 0x0005901C
		private static int ArgMax(Vector3 v)
		{
			float num = Mathf.Max(v.x, Mathf.Max(v.y, v.z));
			if (num == v.x)
			{
				return 0;
			}
			if (num != v.y)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0005AE60 File Offset: 0x00059060
		private unsafe int Rebuild(UnsafeSpan<int> leaves, int parent)
		{
			if (leaves.Length == 0)
			{
				return -1;
			}
			if (leaves.Length == 1)
			{
				this.nodes[*leaves[0]].parent = parent;
				return *leaves[0];
			}
			Bounds bounds = this.nodes[*leaves[0]].bounds;
			for (int i = 1; i < leaves.Length; i++)
			{
				bounds.Encapsulate(this.nodes[*leaves[i]].bounds);
			}
			leaves.Sort(new AABBTree<T>.AABBComparer
			{
				nodes = this.nodes,
				dim = AABBTree<T>.ArgMax(bounds.extents)
			});
			int num = this.AllocNode();
			this.nodes[num] = new AABBTree<T>.Node
			{
				bounds = bounds,
				left = this.Rebuild(leaves.Slice(0, leaves.Length / 2), num),
				right = this.Rebuild(leaves.Slice(leaves.Length / 2), num),
				parent = parent,
				isAllocated = true
			};
			return num;
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0005AF94 File Offset: 0x00059194
		public void Move(AABBTree<T>.Key key, Bounds bounds)
		{
			T value = this.nodes[key.node].value;
			this.Remove(key);
			this.Add(bounds, value);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0005AFCC File Offset: 0x000591CC
		[Conditional("VALIDATE_AABB_TREE")]
		private void Validate(int node)
		{
			if (node == -1)
			{
				return;
			}
			AABBTree<T>.Node node2 = this.nodes[node];
			int num = this.root;
			bool isLeaf = node2.isLeaf;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0005AFFC File Offset: 0x000591FC
		public Bounds Remove(AABBTree<T>.Key key)
		{
			if (!key.isValid)
			{
				throw new ArgumentException("Key is not valid");
			}
			AABBTree<T>.Node node = this.nodes[key.node];
			if (!node.isAllocated)
			{
				throw new ArgumentException("Key does not point to an allocated node");
			}
			if (!node.isLeaf)
			{
				throw new ArgumentException("Key does not point to a leaf node");
			}
			if (key.node == this.root)
			{
				this.root = -1;
				this.FreeNode(key.node);
				return node.bounds;
			}
			int parent = node.parent;
			AABBTree<T>.Node node2 = this.nodes[parent];
			int num = (node2.left == key.node) ? node2.right : node2.left;
			this.FreeNode(parent);
			this.FreeNode(key.node);
			this.nodes[num].parent = node2.parent;
			if (node2.parent == 536870911)
			{
				this.root = num;
			}
			else if (this.nodes[node2.parent].left == parent)
			{
				this.nodes[node2.parent].left = num;
			}
			else
			{
				this.nodes[node2.parent].right = num;
			}
			AABBTree<T>.Node ptr;
			for (int parent2 = this.nodes[num].parent; parent2 != 536870911; parent2 = ptr.parent)
			{
				ptr = ref this.nodes[parent2];
				Bounds bounds = this.nodes[ptr.left].bounds;
				bounds.Encapsulate(this.nodes[ptr.right].bounds);
				ptr.bounds = bounds;
				ptr.subtreePartiallyTagged = (this.nodes[ptr.left].subtreePartiallyTagged | this.nodes[ptr.right].subtreePartiallyTagged);
			}
			return node.bounds;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0005B1FC File Offset: 0x000593FC
		public AABBTree<T>.Key Add(Bounds bounds, T value)
		{
			int num = this.AllocNode();
			this.nodes[num] = new AABBTree<T>.Node
			{
				bounds = bounds,
				parent = 536870911,
				left = -1,
				right = -1,
				value = value,
				isAllocated = true
			};
			if (this.root == -1)
			{
				this.root = num;
				return new AABBTree<T>.Key(num);
			}
			int num2 = this.root;
			AABBTree<T>.Node node;
			for (;;)
			{
				node = this.nodes[num2];
				this.nodes[num2].wholeSubtreeTagged = false;
				if (node.isLeaf)
				{
					break;
				}
				this.nodes[num2].bounds.Encapsulate(bounds);
				float num3 = AABBTree<T>.ExpansionRequired(this.nodes[node.left].bounds, bounds);
				float num4 = AABBTree<T>.ExpansionRequired(this.nodes[node.right].bounds, bounds);
				num2 = ((num3 < num4) ? node.left : node.right);
			}
			int num5 = this.AllocNode();
			if (node.parent != 536870911)
			{
				if (this.nodes[node.parent].left == num2)
				{
					this.nodes[node.parent].left = num5;
				}
				else
				{
					this.nodes[node.parent].right = num5;
				}
			}
			bounds.Encapsulate(node.bounds);
			this.nodes[num5] = new AABBTree<T>.Node
			{
				bounds = bounds,
				left = num2,
				right = num,
				parent = node.parent,
				isAllocated = true
			};
			this.nodes[num].parent = (this.nodes[num2].parent = num5);
			if (this.root == num2)
			{
				this.root = num5;
			}
			int num6 = this.rebuildCounter;
			this.rebuildCounter = num6 - 1;
			if (num6 <= 0)
			{
				this.Rebuild();
			}
			return new AABBTree<T>.Key(num);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0005B41D File Offset: 0x0005961D
		public void Query(Bounds bounds, List<T> buffer)
		{
			this.QueryNode(this.root, bounds, buffer);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0005B430 File Offset: 0x00059630
		private void QueryNode(int node, Bounds bounds, List<T> buffer)
		{
			if (node == -1 || !bounds.Intersects(this.nodes[node].bounds))
			{
				return;
			}
			if (this.nodes[node].isLeaf)
			{
				buffer.Add(this.nodes[node].value);
				return;
			}
			this.QueryNode(this.nodes[node].left, bounds, buffer);
			this.QueryNode(this.nodes[node].right, bounds, buffer);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0005B4B9 File Offset: 0x000596B9
		public void QueryTagged(List<T> buffer, bool clearTags = false)
		{
			this.QueryTaggedNode(this.root, clearTags, buffer);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0005B4CC File Offset: 0x000596CC
		private void QueryTaggedNode(int node, bool clearTags, List<T> buffer)
		{
			if (node == -1 || !this.nodes[node].subtreePartiallyTagged)
			{
				return;
			}
			if (clearTags)
			{
				this.nodes[node].wholeSubtreeTagged = false;
				this.nodes[node].subtreePartiallyTagged = false;
			}
			if (this.nodes[node].isLeaf)
			{
				buffer.Add(this.nodes[node].value);
				return;
			}
			this.QueryTaggedNode(this.nodes[node].left, clearTags, buffer);
			this.QueryTaggedNode(this.nodes[node].right, clearTags, buffer);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0005B578 File Offset: 0x00059778
		public void Tag(AABBTree<T>.Key key)
		{
			if (!key.isValid)
			{
				throw new ArgumentException("Key is not valid");
			}
			if (key.node < 0 || key.node >= this.nodes.Length)
			{
				throw new ArgumentException("Key does not point to a valid node");
			}
			AABBTree<T>.Node[] array = this.nodes;
			int node = key.node;
			if (!array[node].isAllocated)
			{
				throw new ArgumentException("Key does not point to an allocated node");
			}
			if (!array[node].isLeaf)
			{
				throw new ArgumentException("Key does not point to a leaf node");
			}
			array[node].wholeSubtreeTagged = true;
			for (int num = key.node; num != 536870911; num = this.nodes[num].parent)
			{
				this.nodes[num].subtreePartiallyTagged = true;
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0005B633 File Offset: 0x00059833
		public void Tag(Bounds bounds)
		{
			this.TagNode(this.root, bounds);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0005B644 File Offset: 0x00059844
		private bool TagNode(int node, Bounds bounds)
		{
			if (node == -1 || this.nodes[node].wholeSubtreeTagged)
			{
				return true;
			}
			if (!bounds.Intersects(this.nodes[node].bounds))
			{
				return false;
			}
			this.nodes[node].subtreePartiallyTagged = true;
			if (this.nodes[node].isLeaf)
			{
				return this.nodes[node].wholeSubtreeTagged = true;
			}
			return this.nodes[node].wholeSubtreeTagged = (this.TagNode(this.nodes[node].left, bounds) & this.TagNode(this.nodes[node].right, bounds));
		}

		// Token: 0x04000B07 RID: 2823
		private AABBTree<T>.Node[] nodes = new AABBTree<T>.Node[0];

		// Token: 0x04000B08 RID: 2824
		private int root = -1;

		// Token: 0x04000B09 RID: 2825
		private readonly Stack<int> freeNodes = new Stack<int>();

		// Token: 0x04000B0A RID: 2826
		private int rebuildCounter = 64;

		// Token: 0x04000B0B RID: 2827
		private const int NoNode = -1;

		// Token: 0x02000268 RID: 616
		private struct Node
		{
			// Token: 0x17000207 RID: 519
			// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0005B734 File Offset: 0x00059934
			// (set) Token: 0x06000ECC RID: 3788 RVA: 0x0005B745 File Offset: 0x00059945
			public bool wholeSubtreeTagged
			{
				get
				{
					return (this.flags & 1073741824U) > 0U;
				}
				set
				{
					this.flags = ((this.flags & 3221225471U) | (value ? 1073741824U : 0U));
				}
			}

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0005B765 File Offset: 0x00059965
			// (set) Token: 0x06000ECE RID: 3790 RVA: 0x0005B776 File Offset: 0x00059976
			public bool subtreePartiallyTagged
			{
				get
				{
					return (this.flags & 2147483648U) > 0U;
				}
				set
				{
					this.flags = ((this.flags & 2147483647U) | (value ? 2147483648U : 0U));
				}
			}

			// Token: 0x17000209 RID: 521
			// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0005B796 File Offset: 0x00059996
			// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x0005B7A7 File Offset: 0x000599A7
			public bool isAllocated
			{
				get
				{
					return (this.flags & 536870912U) > 0U;
				}
				set
				{
					this.flags = ((this.flags & 3758096383U) | (value ? 536870912U : 0U));
				}
			}

			// Token: 0x1700020A RID: 522
			// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0005B7C7 File Offset: 0x000599C7
			public bool isLeaf
			{
				get
				{
					return this.left == -1;
				}
			}

			// Token: 0x1700020B RID: 523
			// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x0005B7D2 File Offset: 0x000599D2
			// (set) Token: 0x06000ED3 RID: 3795 RVA: 0x0005B7E0 File Offset: 0x000599E0
			public int parent
			{
				get
				{
					return (int)(this.flags & 536870911U);
				}
				set
				{
					this.flags = ((this.flags & 3758096384U) | (uint)value);
				}
			}

			// Token: 0x04000B0C RID: 2828
			public Bounds bounds;

			// Token: 0x04000B0D RID: 2829
			public uint flags;

			// Token: 0x04000B0E RID: 2830
			private const uint TagInsideBit = 1073741824U;

			// Token: 0x04000B0F RID: 2831
			private const uint TagPartiallyInsideBit = 2147483648U;

			// Token: 0x04000B10 RID: 2832
			private const uint AllocatedBit = 536870912U;

			// Token: 0x04000B11 RID: 2833
			private const uint ParentMask = 536870911U;

			// Token: 0x04000B12 RID: 2834
			public const int InvalidParent = 536870911;

			// Token: 0x04000B13 RID: 2835
			public int left;

			// Token: 0x04000B14 RID: 2836
			public int right;

			// Token: 0x04000B15 RID: 2837
			public T value;
		}

		// Token: 0x02000269 RID: 617
		public readonly struct Key
		{
			// Token: 0x1700020C RID: 524
			// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x0005B7F6 File Offset: 0x000599F6
			public int node
			{
				get
				{
					return this.value - 1;
				}
			}

			// Token: 0x1700020D RID: 525
			// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0005B800 File Offset: 0x00059A00
			public bool isValid
			{
				get
				{
					return this.value != 0;
				}
			}

			// Token: 0x06000ED6 RID: 3798 RVA: 0x0005B80B File Offset: 0x00059A0B
			internal Key(int node)
			{
				this.value = node + 1;
			}

			// Token: 0x04000B16 RID: 2838
			internal readonly int value;
		}

		// Token: 0x0200026A RID: 618
		private struct AABBComparer : IComparer<int>
		{
			// Token: 0x06000ED7 RID: 3799 RVA: 0x0005B818 File Offset: 0x00059A18
			public int Compare(int a, int b)
			{
				return this.nodes[a].bounds.center[this.dim].CompareTo(this.nodes[b].bounds.center[this.dim]);
			}

			// Token: 0x04000B17 RID: 2839
			public AABBTree<T>.Node[] nodes;

			// Token: 0x04000B18 RID: 2840
			public int dim;
		}
	}
}
