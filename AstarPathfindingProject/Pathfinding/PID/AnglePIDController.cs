using System;
using Unity.Mathematics;

namespace Pathfinding.PID
{
	// Token: 0x02000252 RID: 594
	public static class AnglePIDController
	{
		// Token: 0x06000DF3 RID: 3571 RVA: 0x000587AC File Offset: 0x000569AC
		public static float ApproximateTurningRadius(float followingStrength)
		{
			float num = 2f * math.sqrt(math.abs(followingStrength)) * 1f;
			return 1f / (num * 1.5707964f);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x000587E0 File Offset: 0x000569E0
		public static float RotationSpeedToFollowingStrength(float speed, float maxRotationSpeed)
		{
			float num = maxRotationSpeed / (6.2831855f * speed * 1f);
			return num * num;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x000587F3 File Offset: 0x000569F3
		public static float FollowingStrengthToRotationSpeed(float followingStrength)
		{
			return 1f / (AnglePIDController.ApproximateTurningRadius(followingStrength) * 0.5f);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00058808 File Offset: 0x00056A08
		public static AnglePIDControlOutput2D Control(ref PIDMovement settings, float followingStrength, float angle, float curveAngle, float curveCurvature, float curveDistanceSigned, float speed, float remainingDistance, float minRotationSpeed, bool isStationary, float dt)
		{
			float num = 2f * math.sqrt(math.abs(followingStrength)) * 1f;
			float num2 = 1f;
			float num3 = AstarMath.DeltaAngle(angle, curveAngle);
			float angle2 = curveAngle + math.sign(curveDistanceSigned) * 3.1415927f * 0.5f;
			float num4 = AstarMath.DeltaAngle(angle, angle2);
			float num5 = followingStrength * math.abs(curveDistanceSigned) * num4;
			float num6 = num5 * speed * dt;
			float num7 = num * num3;
			float num8 = num + followingStrength * math.abs(curveDistanceSigned);
			float num9 = (num8 > 1.1754944E-38f) ? ((num7 + num5) / num8) : 0f;
			float.IsFinite(num9);
			isStationary = (settings.allowRotatingOnSpot && (math.abs(num9) > 2.0941856f || (isStationary && math.abs(num9) > 0.1f)));
			if (!isStationary)
			{
				speed = math.min(settings.Speed(remainingDistance), settings.Accelerate(speed, settings.slowdownTime, dt));
				if (math.abs(num3) > 1.5707964f)
				{
					num6 = 0f;
				}
				if (math.abs(num7) > 0.0001f)
				{
					num7 = math.max(math.abs(num7), minRotationSpeed) * math.sign(num7);
				}
				float num10 = num7 * speed * dt;
				float x = math.abs(num6 / num4);
				float x2 = math.abs(num10 / num3);
				float y = 1f;
				float num11 = math.max(0f, math.cos(num3));
				float num12 = 1f;
				float num13 = speed * num12 * dt;
				float num14 = curveCurvature * num13;
				float num15 = num2 * num14 * num11;
				float num16 = math.max(1f, math.max(x, math.max(x2, y)));
				float num17 = (num15 + num10 + num6) / num16;
				float num18 = math.radians(settings.maxRotationSpeed);
				float num19 = math.max(0.1f, math.min(1f, num18 * dt / math.abs(num17)));
				return new AnglePIDControlOutput2D(angle, angle + num9, num17 * num19, num13 * num19);
			}
			float num20 = settings.Accelerate(speed, settings.slowdownTimeWhenTurningOnSpot, -dt);
			float num21 = math.radians(settings.maxOnSpotRotationSpeed);
			bool flag = num21 * dt > math.abs(num9);
			if (num20 > 0f && !flag)
			{
				return AnglePIDControlOutput2D.WithMovementAtEnd(angle, angle, 0f, num20 * dt);
			}
			return AnglePIDControlOutput2D.WithMovementAtEnd(angle, angle + num9, math.clamp(num9, -num21 * dt, num21 * dt), flag ? (speed * dt) : 0f);
		}

		// Token: 0x04000ACC RID: 2764
		private const float DampingRatio = 1f;
	}
}
