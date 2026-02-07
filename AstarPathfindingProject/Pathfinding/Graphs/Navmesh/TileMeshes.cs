using System;
using System.IO;
using System.IO.Compression;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001C1 RID: 449
	public struct TileMeshes
	{
		// Token: 0x06000BEF RID: 3055 RVA: 0x00045D64 File Offset: 0x00043F64
		public void Rotate(int rotation)
		{
			rotation = -rotation;
			rotation = (rotation % 4 + 4) % 4;
			if (rotation == 0)
			{
				return;
			}
			int2x2 b = new int2x2(0, -1, 1, 0);
			int2x2 a = int2x2.identity;
			for (int i = 0; i < rotation; i++)
			{
				a = math.mul(a, b);
			}
			Int3 @int = (Int3)new Vector3(this.tileWorldSize.x, 0f, this.tileWorldSize.y);
			int2 rhs = -math.min(int2.zero, math.mul(a, new int2(@int.x, @int.z)));
			int2 int2 = new int2(this.tileRect.Width, this.tileRect.Height);
			int2 rhs2 = -math.min(int2.zero, math.mul(a, int2 - 1));
			TileMesh[] array = new TileMesh[this.tileMeshes.Length];
			int2 int3 = (rotation % 2 == 0) ? int2 : new int2(int2.y, int2.x);
			for (int j = 0; j < int2.y; j++)
			{
				for (int k = 0; k < int2.x; k++)
				{
					Int3[] verticesInTileSpace = this.tileMeshes[k + j * int2.x].verticesInTileSpace;
					for (int l = 0; l < verticesInTileSpace.Length; l++)
					{
						Int3 int4 = verticesInTileSpace[l];
						int2 int5 = math.mul(a, new int2(int4.x, int4.z)) + rhs;
						verticesInTileSpace[l] = new Int3(int5.x, int4.y, int5.y);
					}
					int2 int6 = math.mul(a, new int2(k, j)) + rhs2;
					array[int6.x + int6.y * int3.x] = this.tileMeshes[k + j * int2.x];
				}
			}
			this.tileMeshes = array;
			this.tileWorldSize = ((rotation % 2 == 0) ? this.tileWorldSize : new Vector2(this.tileWorldSize.y, this.tileWorldSize.x));
			this.tileRect = new IntRect(this.tileRect.xmin, this.tileRect.ymin, this.tileRect.xmin + int3.x - 1, this.tileRect.ymin + int3.y - 1);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00045FEC File Offset: 0x000441EC
		public byte[] Serialize()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(new DeflateStream(memoryStream, CompressionMode.Compress));
			binaryWriter.Write(0);
			binaryWriter.Write(this.tileRect.Width);
			binaryWriter.Write(this.tileRect.Height);
			binaryWriter.Write(this.tileWorldSize.x);
			binaryWriter.Write(this.tileWorldSize.y);
			for (int i = 0; i < this.tileRect.Height; i++)
			{
				for (int j = 0; j < this.tileRect.Width; j++)
				{
					TileMesh tileMesh = this.tileMeshes[i * this.tileRect.Width + j];
					binaryWriter.Write(tileMesh.triangles.Length);
					binaryWriter.Write(tileMesh.verticesInTileSpace.Length);
					for (int k = 0; k < tileMesh.verticesInTileSpace.Length; k++)
					{
						Int3 @int = tileMesh.verticesInTileSpace[k];
						binaryWriter.Write(@int.x);
						binaryWriter.Write(@int.y);
						binaryWriter.Write(@int.z);
					}
					for (int l = 0; l < tileMesh.triangles.Length; l++)
					{
						binaryWriter.Write(tileMesh.triangles[l]);
					}
					for (int m = 0; m < tileMesh.tags.Length; m++)
					{
						binaryWriter.Write(tileMesh.tags[m]);
					}
				}
			}
			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00046174 File Offset: 0x00044374
		public static TileMeshes Deserialize(byte[] bytes)
		{
			BinaryReader binaryReader = new BinaryReader(new DeflateStream(new MemoryStream(bytes), CompressionMode.Decompress));
			if (binaryReader.ReadInt32() != 0)
			{
				throw new Exception("Invalid data. Unexpected version number.");
			}
			int num = binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			Vector2 vector = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
			if (num < 0 || num2 < 0)
			{
				throw new Exception("Invalid bounds");
			}
			IntRect intRect = new IntRect(0, 0, num - 1, num2 - 1);
			TileMesh[] array = new TileMesh[num * num2];
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					int[] array2 = new int[binaryReader.ReadInt32()];
					Int3[] array3 = new Int3[binaryReader.ReadInt32()];
					uint[] array4 = new uint[array2.Length / 3];
					for (int k = 0; k < array3.Length; k++)
					{
						array3[k] = new Int3(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
					}
					for (int l = 0; l < array2.Length; l++)
					{
						array2[l] = binaryReader.ReadInt32();
					}
					for (int m = 0; m < array4.Length; m++)
					{
						array4[m] = binaryReader.ReadUInt32();
					}
					array[j + i * num] = new TileMesh
					{
						triangles = array2,
						verticesInTileSpace = array3,
						tags = array4
					};
				}
			}
			return new TileMeshes
			{
				tileMeshes = array,
				tileRect = intRect,
				tileWorldSize = vector
			};
		}

		// Token: 0x0400083A RID: 2106
		public TileMesh[] tileMeshes;

		// Token: 0x0400083B RID: 2107
		public IntRect tileRect;

		// Token: 0x0400083C RID: 2108
		public Vector2 tileWorldSize;
	}
}
