using System;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x0200028C RID: 652
	public class GraphTransform : IMovementPlane, ITransform
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000F85 RID: 3973 RVA: 0x0005F44A File Offset: 0x0005D64A
		public bool identity
		{
			get
			{
				return this.isIdentity;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0005F452 File Offset: 0x0005D652
		public bool onlyTranslational
		{
			get
			{
				return this.isOnlyTranslational;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0005F45A File Offset: 0x0005D65A
		// (set) Token: 0x06000F88 RID: 3976 RVA: 0x0005F462 File Offset: 0x0005D662
		public Matrix4x4 matrix { get; private set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0005F46B File Offset: 0x0005D66B
		// (set) Token: 0x06000F8A RID: 3978 RVA: 0x0005F473 File Offset: 0x0005D673
		public Matrix4x4 inverseMatrix { get; private set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0005F47C File Offset: 0x0005D67C
		// (set) Token: 0x06000F8C RID: 3980 RVA: 0x0005F484 File Offset: 0x0005D684
		public Quaternion rotation { get; private set; }

		// Token: 0x06000F8D RID: 3981 RVA: 0x0005F48D File Offset: 0x0005D68D
		public GraphTransform(Matrix4x4 matrix)
		{
			this.Set(matrix);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0005F49C File Offset: 0x0005D69C
		protected void Set(Matrix4x4 matrix)
		{
			this.matrix = matrix;
			this.inverseMatrix = matrix.inverse;
			this.isIdentity = matrix.isIdentity;
			this.isOnlyTranslational = GraphTransform.MatrixIsTranslational(matrix);
			this.up = matrix.MultiplyVector(Vector3.up).normalized;
			this.translation = matrix.MultiplyPoint3x4(Vector3.zero);
			this.i3translation = (Int3)this.translation;
			this.rotation = Quaternion.LookRotation(this.TransformVector(Vector3.forward), this.TransformVector(Vector3.up));
			this.inverseRotation = Quaternion.Inverse(this.rotation);
			this.isXY = (this.rotation == Quaternion.Euler(-90f, 0f, 0f));
			this.isXZ = (this.rotation == Quaternion.Euler(0f, 0f, 0f));
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0005F58F File Offset: 0x0005D78F
		public Vector3 WorldUpAtGraphPosition(Vector3 point)
		{
			return this.up;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0005F598 File Offset: 0x0005D798
		private static bool MatrixIsTranslational(Matrix4x4 matrix)
		{
			return matrix.GetColumn(0) == new Vector4(1f, 0f, 0f, 0f) && matrix.GetColumn(1) == new Vector4(0f, 1f, 0f, 0f) && matrix.GetColumn(2) == new Vector4(0f, 0f, 1f, 0f) && matrix.m33 == 1f;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0005F62C File Offset: 0x0005D82C
		public Vector3 Transform(Vector3 point)
		{
			if (this.onlyTranslational)
			{
				return point + this.translation;
			}
			return this.matrix.MultiplyPoint3x4(point);
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0005F660 File Offset: 0x0005D860
		public Vector3 TransformVector(Vector3 dir)
		{
			if (this.onlyTranslational)
			{
				return dir;
			}
			return this.matrix.MultiplyVector(dir);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0005F688 File Offset: 0x0005D888
		public void Transform(Int3[] arr)
		{
			if (this.onlyTranslational)
			{
				for (int i = arr.Length - 1; i >= 0; i--)
				{
					arr[i] += this.i3translation;
				}
				return;
			}
			for (int j = arr.Length - 1; j >= 0; j--)
			{
				arr[j] = (Int3)this.matrix.MultiplyPoint3x4((Vector3)arr[j]);
			}
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0005F704 File Offset: 0x0005D904
		public unsafe void Transform(UnsafeSpan<Int3> arr)
		{
			if (this.onlyTranslational)
			{
				for (int i = arr.Length - 1; i >= 0; i--)
				{
					*arr[i] += this.i3translation;
				}
				return;
			}
			for (int j = arr.Length - 1; j >= 0; j--)
			{
				*arr[j] = (Int3)this.matrix.MultiplyPoint3x4((Vector3)(*arr[j]));
			}
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0005F794 File Offset: 0x0005D994
		public void Transform(Vector3[] arr)
		{
			if (this.onlyTranslational)
			{
				for (int i = arr.Length - 1; i >= 0; i--)
				{
					arr[i] += this.translation;
				}
				return;
			}
			for (int j = arr.Length - 1; j >= 0; j--)
			{
				arr[j] = this.matrix.MultiplyPoint3x4(arr[j]);
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0005F804 File Offset: 0x0005DA04
		public Vector3 InverseTransform(Vector3 point)
		{
			if (this.onlyTranslational)
			{
				return point - this.translation;
			}
			return this.inverseMatrix.MultiplyPoint3x4(point);
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0005F838 File Offset: 0x0005DA38
		public Vector3 InverseTransformVector(Vector3 dir)
		{
			if (this.onlyTranslational)
			{
				return dir;
			}
			return this.inverseMatrix.MultiplyVector(dir);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0005F860 File Offset: 0x0005DA60
		public Int3 InverseTransform(Int3 point)
		{
			if (this.onlyTranslational)
			{
				return point - this.i3translation;
			}
			return (Int3)this.inverseMatrix.MultiplyPoint3x4((Vector3)point);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0005F89C File Offset: 0x0005DA9C
		public void InverseTransform(Int3[] arr)
		{
			for (int i = arr.Length - 1; i >= 0; i--)
			{
				arr[i] = (Int3)this.inverseMatrix.MultiplyPoint3x4((Vector3)arr[i]);
			}
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0005F8E0 File Offset: 0x0005DAE0
		public unsafe void InverseTransform(UnsafeSpan<Int3> arr)
		{
			for (int i = arr.Length - 1; i >= 0; i--)
			{
				*arr[i] = (Int3)this.inverseMatrix.MultiplyPoint3x4((Vector3)(*arr[i]));
			}
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0005F933 File Offset: 0x0005DB33
		public static GraphTransform operator *(GraphTransform lhs, Matrix4x4 rhs)
		{
			return new GraphTransform(lhs.matrix * rhs);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0005F946 File Offset: 0x0005DB46
		public static GraphTransform operator *(Matrix4x4 lhs, GraphTransform rhs)
		{
			return new GraphTransform(lhs * rhs.matrix);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0005F95C File Offset: 0x0005DB5C
		public Bounds Transform(Bounds bounds)
		{
			if (this.onlyTranslational)
			{
				return new Bounds(bounds.center + this.translation, bounds.size);
			}
			Vector3[] array = ArrayPool<Vector3>.Claim(8);
			Vector3 extents = bounds.extents;
			array[0] = this.Transform(bounds.center + new Vector3(extents.x, extents.y, extents.z));
			array[1] = this.Transform(bounds.center + new Vector3(extents.x, extents.y, -extents.z));
			array[2] = this.Transform(bounds.center + new Vector3(extents.x, -extents.y, extents.z));
			array[3] = this.Transform(bounds.center + new Vector3(extents.x, -extents.y, -extents.z));
			array[4] = this.Transform(bounds.center + new Vector3(-extents.x, extents.y, extents.z));
			array[5] = this.Transform(bounds.center + new Vector3(-extents.x, extents.y, -extents.z));
			array[6] = this.Transform(bounds.center + new Vector3(-extents.x, -extents.y, extents.z));
			array[7] = this.Transform(bounds.center + new Vector3(-extents.x, -extents.y, -extents.z));
			Vector3 vector = array[0];
			Vector3 vector2 = array[0];
			for (int i = 1; i < 8; i++)
			{
				vector = Vector3.Min(vector, array[i]);
				vector2 = Vector3.Max(vector2, array[i]);
			}
			ArrayPool<Vector3>.Release(ref array, false);
			return new Bounds((vector + vector2) * 0.5f, vector2 - vector);
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0005FB90 File Offset: 0x0005DD90
		public Bounds InverseTransform(Bounds bounds)
		{
			if (this.onlyTranslational)
			{
				return new Bounds(bounds.center - this.translation, bounds.size);
			}
			Vector3[] array = ArrayPool<Vector3>.Claim(8);
			Vector3 extents = bounds.extents;
			array[0] = this.InverseTransform(bounds.center + new Vector3(extents.x, extents.y, extents.z));
			array[1] = this.InverseTransform(bounds.center + new Vector3(extents.x, extents.y, -extents.z));
			array[2] = this.InverseTransform(bounds.center + new Vector3(extents.x, -extents.y, extents.z));
			array[3] = this.InverseTransform(bounds.center + new Vector3(extents.x, -extents.y, -extents.z));
			array[4] = this.InverseTransform(bounds.center + new Vector3(-extents.x, extents.y, extents.z));
			array[5] = this.InverseTransform(bounds.center + new Vector3(-extents.x, extents.y, -extents.z));
			array[6] = this.InverseTransform(bounds.center + new Vector3(-extents.x, -extents.y, extents.z));
			array[7] = this.InverseTransform(bounds.center + new Vector3(-extents.x, -extents.y, -extents.z));
			Vector3 vector = array[0];
			Vector3 vector2 = array[0];
			for (int i = 1; i < 8; i++)
			{
				vector = Vector3.Min(vector, array[i]);
				vector2 = Vector3.Max(vector2, array[i]);
			}
			ArrayPool<Vector3>.Release(ref array, false);
			return new Bounds((vector + vector2) * 0.5f, vector2 - vector);
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0005FDC4 File Offset: 0x0005DFC4
		Vector2 IMovementPlane.ToPlane(Vector3 point)
		{
			if (this.isXY)
			{
				return new Vector2(point.x, point.y);
			}
			if (!this.isXZ)
			{
				point = this.inverseRotation * point;
			}
			return new Vector2(point.x, point.z);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0005FE12 File Offset: 0x0005E012
		Vector2 IMovementPlane.ToPlane(Vector3 point, out float elevation)
		{
			if (!this.isXZ)
			{
				point = this.inverseRotation * point;
			}
			elevation = point.y;
			return new Vector2(point.x, point.z);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0005FE43 File Offset: 0x0005E043
		Vector3 IMovementPlane.ToWorld(Vector2 point, float elevation)
		{
			return this.rotation * new Vector3(point.x, elevation, point.y);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0005FE62 File Offset: 0x0005E062
		public SimpleMovementPlane ToSimpleMovementPlane()
		{
			return new SimpleMovementPlane(this.rotation);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0005FE70 File Offset: 0x0005E070
		public void CopyTo(MutableGraphTransform graphTransform)
		{
			graphTransform.isXY = this.isXY;
			graphTransform.isXZ = this.isXZ;
			graphTransform.isOnlyTranslational = this.isOnlyTranslational;
			graphTransform.isIdentity = this.isIdentity;
			graphTransform.matrix = this.matrix;
			graphTransform.inverseMatrix = this.inverseMatrix;
			graphTransform.up = this.up;
			graphTransform.translation = this.translation;
			graphTransform.i3translation = this.i3translation;
			graphTransform.rotation = this.rotation;
			graphTransform.inverseRotation = this.inverseRotation;
		}

		// Token: 0x04000B67 RID: 2919
		private bool isXY;

		// Token: 0x04000B68 RID: 2920
		private bool isXZ;

		// Token: 0x04000B69 RID: 2921
		private bool isOnlyTranslational;

		// Token: 0x04000B6A RID: 2922
		private bool isIdentity;

		// Token: 0x04000B6D RID: 2925
		private Vector3 up;

		// Token: 0x04000B6E RID: 2926
		private Vector3 translation;

		// Token: 0x04000B6F RID: 2927
		private Int3 i3translation;

		// Token: 0x04000B71 RID: 2929
		private Quaternion inverseRotation;

		// Token: 0x04000B72 RID: 2930
		public static readonly GraphTransform identityTransform = new GraphTransform(Matrix4x4.identity);

		// Token: 0x04000B73 RID: 2931
		public static readonly GraphTransform xyPlane = new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90f, 0f, 0f), Vector3.one));

		// Token: 0x04000B74 RID: 2932
		public static readonly GraphTransform xzPlane = new GraphTransform(Matrix4x4.identity);
	}
}
