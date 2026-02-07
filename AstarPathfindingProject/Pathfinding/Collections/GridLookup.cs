using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding.Collections
{
	// Token: 0x02000278 RID: 632
	public class GridLookup<T> where T : class
	{
		// Token: 0x06000F12 RID: 3858 RVA: 0x0005CAE0 File Offset: 0x0005ACE0
		public GridLookup(Vector2Int size)
		{
			this.size = size;
			this.cells = new GridLookup<T>.Item[size.x * size.y];
			for (int i = 0; i < this.cells.Length; i++)
			{
				this.cells[i] = new GridLookup<T>.Item();
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0005CB55 File Offset: 0x0005AD55
		public GridLookup<T>.Root AllItems
		{
			get
			{
				return this.all.next;
			}
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0005CB64 File Offset: 0x0005AD64
		public void Clear()
		{
			this.rootLookup.Clear();
			this.all.next = null;
			GridLookup<T>.Item[] array = this.cells;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].next = null;
			}
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0005CBA8 File Offset: 0x0005ADA8
		public GridLookup<T>.Root GetRoot(T item)
		{
			GridLookup<T>.Root result;
			this.rootLookup.TryGetValue(item, out result);
			return result;
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0005CBC8 File Offset: 0x0005ADC8
		public GridLookup<T>.Root Add(T item, IntRect bounds)
		{
			GridLookup<T>.Root root = new GridLookup<T>.Root
			{
				obj = item,
				prev = this.all,
				next = this.all.next
			};
			this.all.next = root;
			if (root.next != null)
			{
				root.next.prev = root;
			}
			this.rootLookup.Add(item, root);
			this.Move(item, bounds);
			return root;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0005CC38 File Offset: 0x0005AE38
		public void Remove(T item)
		{
			GridLookup<T>.Root root;
			if (!this.rootLookup.TryGetValue(item, out root))
			{
				return;
			}
			this.Move(item, new IntRect(0, 0, -1, -1));
			this.rootLookup.Remove(item);
			root.prev.next = root.next;
			if (root.next != null)
			{
				root.next.prev = root.prev;
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0005CCA0 File Offset: 0x0005AEA0
		public void Dirty(T item)
		{
			GridLookup<T>.Root root;
			if (!this.rootLookup.TryGetValue(item, out root))
			{
				return;
			}
			root.previousPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0005CCD8 File Offset: 0x0005AED8
		public void Move(T item, IntRect bounds)
		{
			GridLookup<T>.Root root;
			if (!this.rootLookup.TryGetValue(item, out root))
			{
				throw new ArgumentException("The item has not been added to this object");
			}
			if (root.previousBounds == bounds)
			{
				return;
			}
			for (int i = 0; i < root.items.Count; i++)
			{
				GridLookup<T>.Item item2 = root.items[i];
				item2.prev.next = item2.next;
				if (item2.next != null)
				{
					item2.next.prev = item2.prev;
				}
			}
			root.previousBounds = bounds;
			int num = 0;
			for (int j = bounds.ymin; j <= bounds.ymax; j++)
			{
				for (int k = bounds.xmin; k <= bounds.xmax; k++)
				{
					GridLookup<T>.Item item3;
					if (num < root.items.Count)
					{
						item3 = root.items[num];
					}
					else
					{
						item3 = ((this.itemPool.Count > 0) ? this.itemPool.Pop() : new GridLookup<T>.Item());
						item3.root = root;
						root.items.Add(item3);
					}
					num++;
					item3.prev = this.cells[k + j * this.size.x];
					item3.next = item3.prev.next;
					item3.prev.next = item3;
					if (item3.next != null)
					{
						item3.next.prev = item3;
					}
				}
			}
			for (int l = root.items.Count - 1; l >= num; l--)
			{
				GridLookup<T>.Item item4 = root.items[l];
				item4.root = null;
				item4.next = null;
				item4.prev = null;
				root.items.RemoveAt(l);
				this.itemPool.Push(item4);
			}
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0005CEB4 File Offset: 0x0005B0B4
		public List<U> QueryRect<U>(IntRect r) where U : class, T
		{
			List<U> list = ListPool<U>.Claim();
			for (int i = r.ymin; i <= r.ymax; i++)
			{
				int num = i * this.size.x;
				for (int j = r.xmin; j <= r.xmax; j++)
				{
					GridLookup<T>.Item item = this.cells[j + num];
					while (item.next != null)
					{
						item = item.next;
						U u = item.root.obj as U;
						if (!item.root.flag && u != null)
						{
							item.root.flag = true;
							list.Add(u);
						}
					}
				}
			}
			for (int k = r.ymin; k <= r.ymax; k++)
			{
				int num2 = k * this.size.x;
				for (int l = r.xmin; l <= r.xmax; l++)
				{
					GridLookup<T>.Item item2 = this.cells[l + num2];
					while (item2.next != null)
					{
						item2 = item2.next;
						item2.root.flag = false;
					}
				}
			}
			return list;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0005CFE4 File Offset: 0x0005B1E4
		public void Resize(IntRect newBounds)
		{
			GridLookup<T>.Item[] array = new GridLookup<T>.Item[newBounds.Width * newBounds.Height];
			for (int i = 0; i < this.size.y; i++)
			{
				for (int j = 0; j < this.size.x; j++)
				{
					if (newBounds.Contains(j, i))
					{
						array[j - newBounds.xmin + (i - newBounds.ymin) * newBounds.Width] = this.cells[j + i * this.size.x];
					}
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] == null)
				{
					array[k] = new GridLookup<T>.Item();
				}
			}
			this.size = new Vector2Int(newBounds.Width, newBounds.Height);
			this.cells = array;
			GridLookup<T>.Root root = this.AllItems;
			Vector2Int offset = new Vector2Int(-newBounds.xmin, -newBounds.ymin);
			IntRect b = new IntRect(0, 0, newBounds.Width - 1, newBounds.Height - 1);
			while (root != null)
			{
				root.previousBounds = IntRect.Intersection(root.previousBounds.Offset(offset), b);
				root = root.next;
			}
		}

		// Token: 0x04000B42 RID: 2882
		private Vector2Int size;

		// Token: 0x04000B43 RID: 2883
		private GridLookup<T>.Item[] cells;

		// Token: 0x04000B44 RID: 2884
		private GridLookup<T>.Root all = new GridLookup<T>.Root();

		// Token: 0x04000B45 RID: 2885
		private Dictionary<T, GridLookup<T>.Root> rootLookup = new Dictionary<T, GridLookup<T>.Root>();

		// Token: 0x04000B46 RID: 2886
		private Stack<GridLookup<T>.Item> itemPool = new Stack<GridLookup<T>.Item>();

		// Token: 0x02000279 RID: 633
		internal class Item
		{
			// Token: 0x04000B47 RID: 2887
			public GridLookup<T>.Root root;

			// Token: 0x04000B48 RID: 2888
			public GridLookup<T>.Item prev;

			// Token: 0x04000B49 RID: 2889
			public GridLookup<T>.Item next;
		}

		// Token: 0x0200027A RID: 634
		public class Root
		{
			// Token: 0x04000B4A RID: 2890
			public T obj;

			// Token: 0x04000B4B RID: 2891
			public GridLookup<T>.Root next;

			// Token: 0x04000B4C RID: 2892
			internal GridLookup<T>.Root prev;

			// Token: 0x04000B4D RID: 2893
			internal IntRect previousBounds = new IntRect(0, 0, -1, -1);

			// Token: 0x04000B4E RID: 2894
			internal List<GridLookup<T>.Item> items = new List<GridLookup<T>.Item>();

			// Token: 0x04000B4F RID: 2895
			internal bool flag;

			// Token: 0x04000B50 RID: 2896
			public Vector3 previousPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

			// Token: 0x04000B51 RID: 2897
			public Quaternion previousRotation;
		}
	}
}
