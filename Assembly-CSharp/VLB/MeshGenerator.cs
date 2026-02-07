using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000031 RID: 49
	public static class MeshGenerator
	{
		// Token: 0x0600015A RID: 346 RVA: 0x0000630C File Offset: 0x0000450C
		private static float GetAngleOffset(int numSides)
		{
			if (numSides != 4)
			{
				return 0f;
			}
			return 0.7853982f;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000631D File Offset: 0x0000451D
		private static float GetRadiiScale(int numSides)
		{
			if (numSides != 4)
			{
				return 1f;
			}
			return Mathf.Sqrt(2f);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006334 File Offset: 0x00004534
		public static Mesh GenerateConeZ_RadiusAndAngle(float lengthZ, float radiusStart, float coneAngle, int numSides, int numSegments, bool cap, bool doubleSided)
		{
			float radiusEnd = lengthZ * Mathf.Tan(coneAngle * 0.017453292f * 0.5f);
			return MeshGenerator.GenerateConeZ_Radii(lengthZ, radiusStart, radiusEnd, numSides, numSegments, cap, doubleSided);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006365 File Offset: 0x00004565
		public static Mesh GenerateConeZ_Angle(float lengthZ, float coneAngle, int numSides, int numSegments, bool cap, bool doubleSided)
		{
			return MeshGenerator.GenerateConeZ_RadiusAndAngle(lengthZ, 0f, coneAngle, numSides, numSegments, cap, doubleSided);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000637C File Offset: 0x0000457C
		public static Mesh GenerateConeZ_Radii(float lengthZ, float radiusStart, float radiusEnd, int numSides, int numSegments, bool cap, bool doubleSided)
		{
			Mesh mesh = new Mesh();
			bool flag = cap && radiusStart > 0f;
			radiusStart = Mathf.Max(radiusStart, 0.001f);
			float radiiScale = MeshGenerator.GetRadiiScale(numSides);
			radiusStart *= radiiScale;
			radiusEnd *= radiiScale;
			int num = numSides * (numSegments + 2);
			int num2 = num;
			if (flag)
			{
				num2 += numSides + 1;
			}
			float angleOffset = MeshGenerator.GetAngleOffset(numSides);
			Vector3[] array = new Vector3[num2];
			for (int i = 0; i < numSides; i++)
			{
				float f = angleOffset + 6.2831855f * (float)i / (float)numSides;
				float num3 = Mathf.Cos(f);
				float num4 = Mathf.Sin(f);
				for (int j = 0; j < numSegments + 2; j++)
				{
					float num5 = (float)j / (float)(numSegments + 1);
					float num6 = Mathf.Lerp(radiusStart, radiusEnd, num5);
					array[i + j * numSides] = new Vector3(num6 * num3, num6 * num4, num5 * lengthZ);
				}
			}
			if (flag)
			{
				int num7 = num;
				array[num7] = Vector3.zero;
				num7++;
				for (int k = 0; k < numSides; k++)
				{
					float f2 = angleOffset + 6.2831855f * (float)k / (float)numSides;
					float num8 = Mathf.Cos(f2);
					float num9 = Mathf.Sin(f2);
					array[num7] = new Vector3(radiusStart * num8, radiusStart * num9, 0f);
					num7++;
				}
			}
			if (!doubleSided)
			{
				mesh.vertices = array;
			}
			else
			{
				Vector3[] array2 = new Vector3[array.Length * 2];
				array.CopyTo(array2, 0);
				array.CopyTo(array2, array.Length);
				mesh.vertices = array2;
			}
			Vector2[] array3 = new Vector2[num2];
			int num10 = 0;
			for (int l = 0; l < num; l++)
			{
				array3[num10++] = Vector2.zero;
			}
			if (flag)
			{
				for (int m = 0; m < numSides + 1; m++)
				{
					array3[num10++] = new Vector2(1f, 0f);
				}
			}
			if (!doubleSided)
			{
				mesh.uv = array3;
			}
			else
			{
				Vector2[] array4 = new Vector2[array3.Length * 2];
				array3.CopyTo(array4, 0);
				array3.CopyTo(array4, array3.Length);
				for (int n = 0; n < array3.Length; n++)
				{
					Vector2 vector = array4[n + array3.Length];
					array4[n + array3.Length] = new Vector2(vector.x, 1f);
				}
				mesh.uv = array4;
			}
			int num11 = numSides * 2 * Mathf.Max(numSegments + 1, 1) * 3;
			if (flag)
			{
				num11 += numSides * 3;
			}
			int[] array5 = new int[num11];
			int num12 = 0;
			for (int num13 = 0; num13 < numSides; num13++)
			{
				int num14 = num13 + 1;
				if (num14 == numSides)
				{
					num14 = 0;
				}
				for (int num15 = 0; num15 < numSegments + 1; num15++)
				{
					int num16 = num15 * numSides;
					array5[num12++] = num16 + num13;
					array5[num12++] = num16 + num14;
					array5[num12++] = num16 + num13 + numSides;
					array5[num12++] = num16 + num14 + numSides;
					array5[num12++] = num16 + num13 + numSides;
					array5[num12++] = num16 + num14;
				}
			}
			if (flag)
			{
				for (int num17 = 0; num17 < numSides - 1; num17++)
				{
					array5[num12++] = num;
					array5[num12++] = num + num17 + 2;
					array5[num12++] = num + num17 + 1;
				}
				array5[num12++] = num;
				array5[num12++] = num + 1;
				array5[num12++] = num + numSides;
			}
			if (!doubleSided)
			{
				mesh.triangles = array5;
			}
			else
			{
				int[] array6 = new int[array5.Length * 2];
				array5.CopyTo(array6, 0);
				for (int num18 = 0; num18 < array5.Length; num18 += 3)
				{
					array6[array5.Length + num18] = array5[num18] + num2;
					array6[array5.Length + num18 + 1] = array5[num18 + 2] + num2;
					array6[array5.Length + num18 + 2] = array5[num18 + 1] + num2;
				}
				mesh.triangles = array6;
			}
			mesh.bounds = MeshGenerator.ComputeBounds(lengthZ, radiusStart, radiusEnd);
			return mesh;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006790 File Offset: 0x00004990
		public static Mesh GenerateConeZ_Radii_DoubleCaps(float lengthZ, float radiusStart, float radiusEnd, int numSides, bool inverted)
		{
			MeshGenerator.<>c__DisplayClass6_0 CS$<>8__locals1 = new MeshGenerator.<>c__DisplayClass6_0();
			CS$<>8__locals1.numSides = numSides;
			Mesh mesh = new Mesh();
			radiusStart = Mathf.Max(radiusStart, 0.001f);
			CS$<>8__locals1.vertCountSides = CS$<>8__locals1.numSides * 2;
			int vertCountSides = CS$<>8__locals1.vertCountSides;
			CS$<>8__locals1.vertSidesStartFromSlide = ((int slideID) => CS$<>8__locals1.numSides * slideID);
			CS$<>8__locals1.vertCenterFromSlide = ((int slideID) => CS$<>8__locals1.vertCountSides + slideID);
			int num = vertCountSides + 2;
			float angleOffset = MeshGenerator.GetAngleOffset(CS$<>8__locals1.numSides);
			Vector3[] array = new Vector3[num];
			for (int i = 0; i < CS$<>8__locals1.numSides; i++)
			{
				float f = angleOffset + 6.2831855f * (float)i / (float)CS$<>8__locals1.numSides;
				float num2 = Mathf.Cos(f);
				float num3 = Mathf.Sin(f);
				for (int j = 0; j < 2; j++)
				{
					float num4 = (float)j;
					float num5 = Mathf.Lerp(radiusStart, radiusEnd, num4);
					array[i + CS$<>8__locals1.vertSidesStartFromSlide(j)] = new Vector3(num5 * num2, num5 * num3, num4 * lengthZ);
				}
			}
			array[CS$<>8__locals1.vertCenterFromSlide(0)] = Vector3.zero;
			array[CS$<>8__locals1.vertCenterFromSlide(1)] = new Vector3(0f, 0f, lengthZ);
			mesh.vertices = array;
			int num6 = CS$<>8__locals1.numSides * 2 * 3;
			num6 += CS$<>8__locals1.numSides * 3;
			num6 += CS$<>8__locals1.numSides * 3;
			int[] indices = new int[num6];
			int ind = 0;
			for (int k = 0; k < CS$<>8__locals1.numSides; k++)
			{
				int num7 = k + 1;
				if (num7 == CS$<>8__locals1.numSides)
				{
					num7 = 0;
				}
				for (int l = 0; l < 1; l++)
				{
					int num8 = l * CS$<>8__locals1.numSides;
					indices[ind] = num8 + k;
					indices[ind + (inverted ? 1 : 2)] = num8 + num7;
					indices[ind + (inverted ? 2 : 1)] = num8 + k + CS$<>8__locals1.numSides;
					indices[ind + 3] = num8 + num7 + CS$<>8__locals1.numSides;
					indices[ind + (inverted ? 4 : 5)] = num8 + k + CS$<>8__locals1.numSides;
					indices[ind + (inverted ? 5 : 4)] = num8 + num7;
					ind += 6;
				}
			}
			Action<int, bool> action = delegate(int slideID, bool invert)
			{
				int num9 = CS$<>8__locals1.vertSidesStartFromSlide(slideID);
				for (int m = 0; m < CS$<>8__locals1.numSides - 1; m++)
				{
					indices[ind] = CS$<>8__locals1.vertCenterFromSlide(slideID);
					indices[ind + (invert ? 1 : 2)] = num9 + m + 1;
					indices[ind + (invert ? 2 : 1)] = num9 + m;
					ind += 3;
				}
				indices[ind] = CS$<>8__locals1.vertCenterFromSlide(slideID);
				indices[ind + (invert ? 1 : 2)] = num9;
				indices[ind + (invert ? 2 : 1)] = num9 + CS$<>8__locals1.numSides - 1;
				ind += 3;
			};
			action(0, inverted);
			action(1, !inverted);
			mesh.triangles = indices;
			Bounds bounds = new Bounds(new Vector3(0f, 0f, lengthZ * 0.5f), new Vector3(Mathf.Max(radiusStart, radiusEnd) * 2f, Mathf.Max(radiusStart, radiusEnd) * 2f, lengthZ));
			mesh.bounds = bounds;
			return mesh;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006AD4 File Offset: 0x00004CD4
		public static Bounds ComputeBounds(float lengthZ, float radiusStart, float radiusEnd)
		{
			float num = Mathf.Max(radiusStart, radiusEnd) * 2f;
			return new Bounds(new Vector3(0f, 0f, lengthZ * 0.5f), new Vector3(num, num, lengthZ));
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006B12 File Offset: 0x00004D12
		private static int GetCapAdditionalVerticesCount(MeshGenerator.CapMode capMode, int numSides)
		{
			switch (capMode)
			{
			case MeshGenerator.CapMode.None:
				return 0;
			case MeshGenerator.CapMode.OneVertexPerCap_1Cap:
				return 1;
			case MeshGenerator.CapMode.OneVertexPerCap_2Caps:
				return 2;
			case MeshGenerator.CapMode.SpecificVerticesPerCap_1Cap:
				return numSides + 1;
			case MeshGenerator.CapMode.SpecificVerticesPerCap_2Caps:
				return 2 * (numSides + 1);
			default:
				return 0;
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006B41 File Offset: 0x00004D41
		private static int GetCapAdditionalIndicesCount(MeshGenerator.CapMode capMode, int numSides)
		{
			switch (capMode)
			{
			case MeshGenerator.CapMode.None:
				return 0;
			case MeshGenerator.CapMode.OneVertexPerCap_1Cap:
			case MeshGenerator.CapMode.SpecificVerticesPerCap_1Cap:
				return numSides * 3;
			case MeshGenerator.CapMode.OneVertexPerCap_2Caps:
			case MeshGenerator.CapMode.SpecificVerticesPerCap_2Caps:
				return 2 * (numSides * 3);
			default:
				return 0;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006B6C File Offset: 0x00004D6C
		public static int GetVertexCount(int numSides, int numSegments, MeshGenerator.CapMode capMode, bool doubleSided)
		{
			int num = numSides * (numSegments + 2);
			num += MeshGenerator.GetCapAdditionalVerticesCount(capMode, numSides);
			if (doubleSided)
			{
				num *= 2;
			}
			return num;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006B94 File Offset: 0x00004D94
		public static int GetIndicesCount(int numSides, int numSegments, MeshGenerator.CapMode capMode, bool doubleSided)
		{
			int num = numSides * (numSegments + 1) * 2 * 3;
			num += MeshGenerator.GetCapAdditionalIndicesCount(capMode, numSides);
			if (doubleSided)
			{
				num *= 2;
			}
			return num;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006BBD File Offset: 0x00004DBD
		public static int GetSharedMeshVertexCount()
		{
			return MeshGenerator.GetVertexCount(Config.Instance.sharedMeshSides, Config.Instance.sharedMeshSegments, MeshGenerator.CapMode.SpecificVerticesPerCap_1Cap, Config.Instance.SD_requiresDoubleSidedMesh);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006BE3 File Offset: 0x00004DE3
		public static int GetSharedMeshIndicesCount()
		{
			return MeshGenerator.GetIndicesCount(Config.Instance.sharedMeshSides, Config.Instance.sharedMeshSegments, MeshGenerator.CapMode.SpecificVerticesPerCap_1Cap, Config.Instance.SD_requiresDoubleSidedMesh);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006C09 File Offset: 0x00004E09
		public static int GetSharedMeshHDVertexCount()
		{
			return MeshGenerator.GetVertexCount(Config.Instance.sharedMeshSides, 0, MeshGenerator.CapMode.OneVertexPerCap_2Caps, false);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006C1D File Offset: 0x00004E1D
		public static int GetSharedMeshHDIndicesCount()
		{
			return MeshGenerator.GetIndicesCount(Config.Instance.sharedMeshSides, 0, MeshGenerator.CapMode.OneVertexPerCap_2Caps, false);
		}

		// Token: 0x04000114 RID: 276
		private const float kMinTruncatedRadius = 0.001f;

		// Token: 0x020000B4 RID: 180
		public enum CapMode
		{
			// Token: 0x040003B4 RID: 948
			None,
			// Token: 0x040003B5 RID: 949
			OneVertexPerCap_1Cap,
			// Token: 0x040003B6 RID: 950
			OneVertexPerCap_2Caps,
			// Token: 0x040003B7 RID: 951
			SpecificVerticesPerCap_1Cap,
			// Token: 0x040003B8 RID: 952
			SpecificVerticesPerCap_2Caps
		}
	}
}
