using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000046 RID: 70
	public class BaseOctree
	{
		// Token: 0x02000070 RID: 112
		public class Cell
		{
			// Token: 0x060001E8 RID: 488 RVA: 0x0001173F File Offset: 0x0000F93F
			public Cell()
			{
			}

			// Token: 0x060001E9 RID: 489 RVA: 0x00011747 File Offset: 0x0000F947
			public Cell(Vector3 position, Vector3 size, int maxLevels)
			{
				this.bounds = new Bounds(position, size);
				this.maxLevels = maxLevels;
			}

			// Token: 0x060001EA RID: 490 RVA: 0x00011764 File Offset: 0x0000F964
			public Cell(BaseOctree.Cell parent, int cellIndex, Bounds bounds)
			{
				if (parent != null)
				{
					this.maxLevels = parent.maxLevels;
					this.mainParent = parent.mainParent;
					this.level = parent.level + 1;
				}
				this.parent = parent;
				this.cellIndex = cellIndex;
				this.bounds = bounds;
			}

			// Token: 0x060001EB RID: 491 RVA: 0x000117B5 File Offset: 0x0000F9B5
			public void SetCell(BaseOctree.Cell parent, int cellIndex, Bounds bounds)
			{
				if (parent != null)
				{
					this.maxLevels = parent.maxLevels;
					this.mainParent = parent.mainParent;
					this.level = parent.level + 1;
				}
				this.parent = parent;
				this.cellIndex = cellIndex;
				this.bounds = bounds;
			}

			// Token: 0x060001EC RID: 492 RVA: 0x000117F8 File Offset: 0x0000F9F8
			protected int AddCell<T, U>(ref T[] cells, Vector3 position, out bool maxCellCreated) where T : BaseOctree.Cell, new() where U : BaseOctree.Cell, new()
			{
				Vector3 vector = position - this.bounds.min;
				int num = (int)(vector.x / this.bounds.extents.x);
				int num2 = (int)(vector.y / this.bounds.extents.y);
				int num3 = (int)(vector.z / this.bounds.extents.z);
				int num4 = num + num2 * 4 + num3 * 2;
				this.AddCell<T, U>(ref cells, num4, num, num2, num3, out maxCellCreated);
				return num4;
			}

			// Token: 0x060001ED RID: 493 RVA: 0x00011878 File Offset: 0x0000FA78
			protected T GetCell<T>(T[] cells, Vector3 position)
			{
				if (cells == null)
				{
					return default(T);
				}
				Vector3 vector = position - this.bounds.min;
				int num = (int)(vector.x / this.bounds.extents.x);
				int num2 = (int)(vector.y / this.bounds.extents.y);
				int num3 = (int)(vector.z / this.bounds.extents.z);
				int num4 = num + num2 * 4 + num3 * 2;
				return cells[num4];
			}

			// Token: 0x060001EE RID: 494 RVA: 0x00011900 File Offset: 0x0000FB00
			protected void AddCell<T, U>(ref T[] cells, int index, int x, int y, int z, out bool maxCellCreated) where T : BaseOctree.Cell, new() where U : BaseOctree.Cell, new()
			{
				if (cells == null)
				{
					cells = new T[8];
				}
				if (this.cellsUsed == null)
				{
					this.cellsUsed = new bool[8];
				}
				if (!this.cellsUsed[index])
				{
					Bounds bounds = new Bounds(new Vector3(this.bounds.min.x + this.bounds.extents.x * ((float)x + 0.5f), this.bounds.min.y + this.bounds.extents.y * ((float)y + 0.5f), this.bounds.min.z + this.bounds.extents.z * ((float)z + 0.5f)), this.bounds.extents);
					if (this.level == this.maxLevels - 1)
					{
						cells[index] = (Activator.CreateInstance<U>() as T);
						cells[index].SetCell(this, index, bounds);
						maxCellCreated = true;
					}
					else
					{
						maxCellCreated = false;
						cells[index] = Activator.CreateInstance<T>();
						cells[index].SetCell(this, index, bounds);
					}
					this.cellsUsed[index] = true;
					this.cellCount++;
					return;
				}
				maxCellCreated = false;
			}

			// Token: 0x060001EF RID: 495 RVA: 0x00011A5C File Offset: 0x0000FC5C
			public bool InsideBounds(Vector3 position)
			{
				position -= this.bounds.min;
				return position.x < this.bounds.size.x && position.y < this.bounds.size.y && position.z < this.bounds.size.z && position.x > 0f && position.y > 0f && position.z > 0f;
			}

			// Token: 0x060001F0 RID: 496 RVA: 0x00011AEE File Offset: 0x0000FCEE
			public void Reset(ref BaseOctree.Cell[] cells)
			{
				cells = null;
				this.cellsUsed = null;
			}

			// Token: 0x040002B8 RID: 696
			public BaseOctree.Cell mainParent;

			// Token: 0x040002B9 RID: 697
			public BaseOctree.Cell parent;

			// Token: 0x040002BA RID: 698
			public bool[] cellsUsed;

			// Token: 0x040002BB RID: 699
			public Bounds bounds;

			// Token: 0x040002BC RID: 700
			public int cellIndex;

			// Token: 0x040002BD RID: 701
			public int cellCount;

			// Token: 0x040002BE RID: 702
			public int level;

			// Token: 0x040002BF RID: 703
			public int maxLevels;
		}
	}
}
