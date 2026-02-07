using System;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000288 RID: 648
	public readonly struct NativeMovementPlane
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000F64 RID: 3940 RVA: 0x0005EF50 File Offset: 0x0005D150
		public float3 up
		{
			get
			{
				return 2f * new float3(this.rotation.value.x * this.rotation.value.y - this.rotation.value.w * this.rotation.value.z, 0.5f - this.rotation.value.x * this.rotation.value.x - this.rotation.value.z * this.rotation.value.z, this.rotation.value.w * this.rotation.value.x + this.rotation.value.y * this.rotation.value.z);
			}
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0005F03B File Offset: 0x0005D23B
		public NativeMovementPlane(quaternion rotation)
		{
			this.rotation = math.normalizesafe(rotation);
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0005F049 File Offset: 0x0005D249
		public NativeMovementPlane(SimpleMovementPlane plane)
		{
			this = new NativeMovementPlane(plane.rotation);
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0005F05C File Offset: 0x0005D25C
		public ToPlaneMatrix AsWorldToPlaneMatrix()
		{
			return new ToPlaneMatrix(this);
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0005F069 File Offset: 0x0005D269
		public ToWorldMatrix AsPlaneToWorldMatrix()
		{
			return new ToWorldMatrix(this);
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0005F078 File Offset: 0x0005D278
		public NativeMovementPlane MatchUpDirection(float3 up)
		{
			float3 @float = math.normalizesafe(math.mul(this.rotation, new float3(0f, 0f, 1f)), default(float3));
			up = math.normalizesafe(up, default(float3));
			return new NativeMovementPlane(new quaternion(new float3x3(math.cross(up, @float), up, @float)));
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0005F0DC File Offset: 0x0005D2DC
		public float ProjectedLength(float3 v)
		{
			return math.length(this.ToPlane(v));
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0005F0EC File Offset: 0x0005D2EC
		public float2 ToPlane(float3 p)
		{
			return math.mul(math.conjugate(this.rotation), p).xz;
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0005F112 File Offset: 0x0005D312
		public float2 ToPlane(float3 p, out float elevation)
		{
			p = math.mul(math.conjugate(this.rotation), p);
			elevation = p.y;
			return p.xz;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0005F136 File Offset: 0x0005D336
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return math.mul(this.rotation, new float3(p.x, elevation, p.y));
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0005F158 File Offset: 0x0005D358
		public float ToPlane(quaternion rotation)
		{
			quaternion quaternion = math.mul(math.conjugate(this.rotation), rotation);
			if (quaternion.value.y < 0f)
			{
				quaternion.value = -quaternion.value;
			}
			return -VectorMath.QuaternionAngle(math.normalizesafe(new quaternion(0f, quaternion.value.y, 0f, quaternion.value.w)));
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0005F1CB File Offset: 0x0005D3CB
		public quaternion ToWorldRotation(float angle)
		{
			return math.mul(this.rotation, quaternion.RotateY(-angle));
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0005F1DF File Offset: 0x0005D3DF
		public quaternion ToWorldRotationDelta(float deltaAngle)
		{
			return quaternion.AxisAngle(this.ToWorld(float2.zero, 1f), -deltaAngle);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0005F1F8 File Offset: 0x0005D3F8
		public Bounds ToWorld(Bounds bounds)
		{
			return this.AsPlaneToWorldMatrix().ToWorld(bounds);
		}

		// Token: 0x04000B61 RID: 2913
		public readonly quaternion rotation;
	}
}
