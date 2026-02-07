using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000289 RID: 649
	[GenerateTestsForBurstCompatibility]
	public readonly struct SimpleMovementPlane : IMovementPlane
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0005F214 File Offset: 0x0005D414
		public bool isXY
		{
			get
			{
				return this.plane == 1;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0005F21F File Offset: 0x0005D41F
		public bool isXZ
		{
			get
			{
				return this.plane == 2;
			}
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0005F22C File Offset: 0x0005D42C
		public SimpleMovementPlane(Quaternion rotation)
		{
			this.rotation = rotation;
			this.inverseRotation = Quaternion.Inverse(rotation);
			if (rotation == SimpleMovementPlane.XYPlane.rotation)
			{
				this.plane = 1;
				return;
			}
			if (rotation == Quaternion.identity)
			{
				this.plane = 2;
				return;
			}
			this.plane = 0;
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0005F284 File Offset: 0x0005D484
		public Vector2 ToPlane(Vector3 point)
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

		// Token: 0x06000F76 RID: 3958 RVA: 0x0005F2D4 File Offset: 0x0005D4D4
		public float2 ToPlane(float3 point)
		{
			return (this.inverseRotation * point).xz;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0005F2FF File Offset: 0x0005D4FF
		public Vector2 ToPlane(Vector3 point, out float elevation)
		{
			if (!this.isXZ)
			{
				point = this.inverseRotation * point;
			}
			elevation = point.y;
			return new Vector2(point.x, point.z);
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0005F330 File Offset: 0x0005D530
		public float2 ToPlane(float3 point, out float elevation)
		{
			point = math.mul(this.inverseRotation, point);
			elevation = point.y;
			return point.xz;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0005F354 File Offset: 0x0005D554
		public Vector3 ToWorld(Vector2 point, float elevation = 0f)
		{
			return this.rotation * new Vector3(point.x, elevation, point.y);
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0005F373 File Offset: 0x0005D573
		public float3 ToWorld(float2 point, float elevation = 0f)
		{
			return this.rotation * new Vector3(point.x, elevation, point.y);
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0005F397 File Offset: 0x0005D597
		public SimpleMovementPlane ToSimpleMovementPlane()
		{
			return this;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0005F39F File Offset: 0x0005D59F
		public static bool operator ==(SimpleMovementPlane lhs, SimpleMovementPlane rhs)
		{
			return lhs.rotation == rhs.rotation;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0005F3B2 File Offset: 0x0005D5B2
		public static bool operator !=(SimpleMovementPlane lhs, SimpleMovementPlane rhs)
		{
			return lhs.rotation != rhs.rotation;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0005F3C5 File Offset: 0x0005D5C5
		public override bool Equals(object other)
		{
			return other is SimpleMovementPlane && this.rotation == ((SimpleMovementPlane)other).rotation;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0005F3E8 File Offset: 0x0005D5E8
		public override int GetHashCode()
		{
			return this.rotation.GetHashCode();
		}

		// Token: 0x04000B62 RID: 2914
		public readonly Quaternion rotation;

		// Token: 0x04000B63 RID: 2915
		public readonly Quaternion inverseRotation;

		// Token: 0x04000B64 RID: 2916
		private readonly byte plane;

		// Token: 0x04000B65 RID: 2917
		public static readonly SimpleMovementPlane XYPlane = new SimpleMovementPlane(Quaternion.Euler(-90f, 0f, 0f));

		// Token: 0x04000B66 RID: 2918
		public static readonly SimpleMovementPlane XZPlane = new SimpleMovementPlane(Quaternion.identity);
	}
}
