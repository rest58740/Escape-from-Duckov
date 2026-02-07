using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200004F RID: 79
	public class SectorGrid3D<T>
	{
		// Token: 0x06000199 RID: 409 RVA: 0x0000E93C File Offset: 0x0000CB3C
		public SectorGrid3D(Int3 sectorCount, Vector3 sectorSize, Vector3 sectorGridOffset)
		{
			this.sectors = new Sector3D<T>[sectorCount.x, sectorCount.y, sectorCount.z];
			this.sectorCount = sectorCount;
			this.sectorSize = sectorSize;
			this.sectorGridOffset = sectorGridOffset;
			this.invSectorSize = Mathw.Divide(1f, sectorSize);
			this.halfSectorSize = sectorSize / 2f;
			this.totalSize = Mathw.Scale(sectorSize, sectorCount);
			this.halfTotalSize = this.totalSize * 0.5f;
			this.rect = new Rect(sectorGridOffset - this.halfTotalSize, this.totalSize);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000E9F8 File Offset: 0x0000CBF8
		public void GetSectors(FastList<Sector3D<T>> list, Vector3 pos, float radius)
		{
			list.FastClear();
			Int3 sectorIndex = this.GetSectorIndex(new Vector3(pos.x - radius, pos.y - radius, pos.z - radius));
			Int3 sectorIndex2 = this.GetSectorIndex(new Vector3(pos.x + radius, pos.y + radius, pos.z + radius));
			for (int i = sectorIndex.z; i < sectorIndex2.z; i++)
			{
				for (int j = sectorIndex.y; j <= sectorIndex2.y; j++)
				{
					for (int k = sectorIndex.x; k <= sectorIndex2.x; k++)
					{
						if (this.sectors[k, j, i] != null)
						{
							list.Add(this.sectors[k, j, i]);
						}
					}
				}
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000EAC0 File Offset: 0x0000CCC0
		public void GetOrCreateSector(Vector3 pos, out Sector3D<T> sector)
		{
			Int3 sectorIndex = this.GetSectorIndex(pos);
			sector = this.sectors[sectorIndex.x, sectorIndex.y, sectorIndex.z];
			if (sector == null)
			{
				sector = this.CreateSector(ref sectorIndex);
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000EB04 File Offset: 0x0000CD04
		public Int3 GetSectorIndex(Vector3 pos)
		{
			pos += -this.sectorGridOffset + this.halfTotalSize + this.halfSectorSize;
			pos.x *= this.invSectorSize.x;
			pos.y *= this.invSectorSize.y;
			pos.z *= this.invSectorSize.z;
			return new Int3((int)pos.x, (int)pos.y, (int)pos.z);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000EB98 File Offset: 0x0000CD98
		public Sector3D<T> GetSector(Vector3 pos)
		{
			Int3 sectorIndex = this.GetSectorIndex(pos);
			return this.sectors[sectorIndex.x, sectorIndex.y, sectorIndex.z];
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000EBCC File Offset: 0x0000CDCC
		public Sector3D<T> CreateSector(ref Int3 s)
		{
			Sector3D<T> sector3D = new Sector3D<T>();
			sector3D.bounds = new Bounds(new Vector3((float)s.x * this.sectorSize.x, (float)s.y * this.sectorSize.y, (float)s.z * this.sectorSize.z) + (this.sectorGridOffset - this.halfTotalSize), this.sectorSize);
			this.sectors[s.x, s.y, s.z] = sector3D;
			this.sectorList.Add(sector3D);
			return sector3D;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000EC70 File Offset: 0x0000CE70
		public void RemoveSector(Vector3 pos)
		{
			Int3 sectorIndex = this.GetSectorIndex(pos);
			this.sectorList.Remove(this.sectors[sectorIndex.x, sectorIndex.y, sectorIndex.z]);
			this.sectors[sectorIndex.x, sectorIndex.y, sectorIndex.z] = null;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000ECCC File Offset: 0x0000CECC
		public void RemoveSector(Int3 sectorIndex)
		{
			this.sectorList.Remove(this.sectors[sectorIndex.x, sectorIndex.y, sectorIndex.z]);
			this.sectors[sectorIndex.x, sectorIndex.y, sectorIndex.z] = null;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000ED20 File Offset: 0x0000CF20
		public void Reset()
		{
			this.sectors = new Sector3D<T>[this.sectorCount.y, this.sectorCount.x, this.sectorCount.z];
			this.sectorList.Clear();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000ED59 File Offset: 0x0000CF59
		public void Draw()
		{
			this.DrawSectors(this.sectorList, Color.white);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000ED6C File Offset: 0x0000CF6C
		public void DrawSectors(FastList<Sector3D<T>> sectors, Color color)
		{
			Gizmos.color = color;
			for (int i = 0; i < sectors.Count; i++)
			{
				Bounds bounds = sectors.items[i].bounds;
				Gizmos.DrawWireCube(bounds.center, bounds.size);
			}
		}

		// Token: 0x0400020D RID: 525
		public FastIndexList<Sector3D<T>> sectorList = new FastIndexList<Sector3D<T>>();

		// Token: 0x0400020E RID: 526
		public Sector3D<T>[,,] sectors;

		// Token: 0x0400020F RID: 527
		public Rect rect;

		// Token: 0x04000210 RID: 528
		public Int3 sectorCount;

		// Token: 0x04000211 RID: 529
		public Vector3 sectorGridOffset;

		// Token: 0x04000212 RID: 530
		public Vector3 sectorSize;

		// Token: 0x04000213 RID: 531
		public Vector3 halfSectorSize;

		// Token: 0x04000214 RID: 532
		public Vector3 invSectorSize;

		// Token: 0x04000215 RID: 533
		public Vector3 totalSize;

		// Token: 0x04000216 RID: 534
		public Vector3 halfTotalSize;
	}
}
