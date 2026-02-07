using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000025 RID: 37
	public class PathPlugin : ABSTweenPlugin<Vector3, Path, PathOptions>
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x00009F04 File Offset: 0x00008104
		public override void Reset(TweenerCore<Vector3, Path, PathOptions> t)
		{
			t.endValue.Destroy();
			t.startValue = (t.endValue = (t.changeValue = null));
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void SetFrom(TweenerCore<Vector3, Path, PathOptions> t, bool isRelative)
		{
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void SetFrom(TweenerCore<Vector3, Path, PathOptions> t, Path fromValue, bool setImmediately, bool isRelative)
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009F35 File Offset: 0x00008135
		public static ABSTweenPlugin<Vector3, Path, PathOptions> Get()
		{
			return PluginsManager.GetCustomPlugin<PathPlugin, Vector3, Path, PathOptions>();
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009F3C File Offset: 0x0000813C
		public override Path ConvertToStartValue(TweenerCore<Vector3, Path, PathOptions> t, Vector3 value)
		{
			return t.endValue;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009F44 File Offset: 0x00008144
		public override void SetRelativeEndValue(TweenerCore<Vector3, Path, PathOptions> t)
		{
			if (t.endValue.isFinalized)
			{
				return;
			}
			Vector3 b = t.getter();
			int num = t.endValue.wps.Length;
			for (int i = 0; i < num; i++)
			{
				t.endValue.wps[i] += b;
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009FA8 File Offset: 0x000081A8
		public override void SetChangeValue(TweenerCore<Vector3, Path, PathOptions> t)
		{
			Transform transform = ((Component)t.target).transform;
			if (t.plugOptions.orientType == OrientType.ToPath)
			{
				t.plugOptions.parent = transform.parent;
			}
			if (t.endValue.isFinalized)
			{
				t.changeValue = t.endValue;
				return;
			}
			Vector3 vector = t.getter();
			Path endValue = t.endValue;
			endValue.plugOptions = t.plugOptions;
			int num = endValue.wps.Length;
			int num2 = 0;
			bool flag = false;
			bool flag2 = false;
			if (num <= endValue.minInputWaypoints || !DOTweenUtils.Vector3AreApproximatelyEqual(endValue.wps[0], vector))
			{
				flag = true;
				num2++;
			}
			if (t.plugOptions.isClosedPath)
			{
				Vector3 lhs = endValue.wps[num - 1];
				if (endValue.type == PathType.CubicBezier)
				{
					if (num < 3)
					{
						Debug.LogError("CubicBezier paths must contain waypoints in multiple of 3 excluding the starting point added automatically by DOTween (1: waypoint, 2: IN control point, 3: OUT control point — the minimum amount of waypoints for a single curve is 3)");
					}
					else
					{
						lhs = endValue.wps[num - 3];
					}
				}
				if (lhs != vector)
				{
					flag2 = true;
					num2++;
				}
			}
			Vector3[] array = new Vector3[num + num2];
			int num3 = flag ? 1 : 0;
			if (flag)
			{
				array[0] = vector;
			}
			for (int i = 0; i < num; i++)
			{
				array[i + num3] = endValue.wps[i];
			}
			if (flag2)
			{
				array[array.Length - 1] = array[0];
			}
			endValue.wps = array;
			endValue.addedExtraStartWp = flag;
			endValue.addedExtraEndWp = flag2;
			endValue.FinalizePath(t.plugOptions.isClosedPath, t.plugOptions.lockPositionAxis, vector);
			t.plugOptions.startupRot = transform.rotation;
			t.plugOptions.startupZRot = transform.eulerAngles.z;
			t.changeValue = t.endValue;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000A17B File Offset: 0x0000837B
		public override float GetSpeedBasedDuration(PathOptions options, float unitsXSecond, Path changeValue)
		{
			return changeValue.length / unitsXSecond;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000A188 File Offset: 0x00008388
		public override void EvaluateAndApply(PathOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Path startValue, Path changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental && !options.isClosedPath)
			{
				int num = t.isComplete ? (t.completedLoops - 1) : t.completedLoops;
				if (num > 0)
				{
					changeValue = changeValue.CloneIncremental(num);
				}
			}
			float perc = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			float num2 = changeValue.ConvertToConstantPathPerc(perc);
			Vector3 point = changeValue.GetPoint(num2, false);
			changeValue.targetPosition = point;
			setter(point);
			if (options.mode != PathMode.Ignore && options.orientType != OrientType.None)
			{
				this.SetOrientation(options, t, changeValue, num2, point, updateNotice);
			}
			bool flag = !usingInversePosition;
			if (t.isBackwards)
			{
				flag = !flag;
			}
			int waypointIndexFromPerc = changeValue.GetWaypointIndexFromPerc(perc, flag);
			if (waypointIndexFromPerc != t.miscInt)
			{
				int miscInt = t.miscInt;
				t.miscInt = waypointIndexFromPerc;
				if (t.onWaypointChange != null)
				{
					bool flag2 = t.isBackwards;
					if (t.hasLoops && t.loopType == LoopType.Yoyo)
					{
						flag2 = ((!t.isBackwards && t.completedLoops % 2 != 0) || (t.isBackwards && t.completedLoops % 2 == 0));
					}
					if (flag2)
					{
						for (int i = miscInt - 1; i > waypointIndexFromPerc - 1; i--)
						{
							Tween.OnTweenCallback<int>(t.onWaypointChange, t, i);
						}
					}
					else
					{
						for (int j = miscInt + 1; j < waypointIndexFromPerc; j++)
						{
							Tween.OnTweenCallback<int>(t.onWaypointChange, t, j);
						}
					}
					Tween.OnTweenCallback<int>(t.onWaypointChange, t, waypointIndexFromPerc);
				}
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000A31C File Offset: 0x0000851C
		public void SetOrientation(PathOptions options, Tween t, Path path, float pathPerc, Vector3 tPos, UpdateNotice updateNotice)
		{
			Transform transform = ((Component)t.target).transform;
			Quaternion quaternion = Quaternion.identity;
			Vector3 position = transform.position;
			if (updateNotice == UpdateNotice.RewindStep)
			{
				transform.rotation = options.startupRot;
			}
			switch (options.orientType)
			{
			case OrientType.ToPath:
			{
				Vector3 vector;
				if (path.type == PathType.Linear && options.lookAhead <= 0.0001f)
				{
					vector = tPos + path.wps[path.linearWPIndex] - path.wps[path.linearWPIndex - 1];
				}
				else
				{
					float num = pathPerc + options.lookAhead;
					if (num > 1f)
					{
						num = (options.isClosedPath ? (num - 1f) : ((path.type == PathType.Linear) ? 1f : 1.00001f));
					}
					vector = path.GetPoint(num, false);
				}
				if (path.type == PathType.Linear)
				{
					Vector3 vector2 = path.wps[path.wps.Length - 1];
					if (vector == vector2)
					{
						vector = ((tPos == vector2) ? (vector2 + (vector2 - path.wps[path.wps.Length - 2])) : vector2);
					}
				}
				Vector3 upwards = transform.up;
				bool flag = options.parent != null;
				bool flag2 = options.useLocalPosition && flag;
				if (flag2)
				{
					vector = options.parent.TransformPoint(vector);
				}
				if (options.lockRotationAxis != AxisConstraint.None)
				{
					if ((options.lockRotationAxis & AxisConstraint.X) == AxisConstraint.X)
					{
						Vector3 position2 = transform.InverseTransformPoint(vector);
						position2.y = 0f;
						vector = transform.TransformPoint(position2);
						upwards = (flag2 ? options.parent.up : Vector3.up);
					}
					if ((options.lockRotationAxis & AxisConstraint.Y) == AxisConstraint.Y)
					{
						Vector3 vector3 = transform.InverseTransformPoint(vector);
						if (vector3.z < 0f)
						{
							vector3.z = -vector3.z;
						}
						vector3.x = 0f;
						vector = transform.TransformPoint(vector3);
					}
					if ((options.lockRotationAxis & AxisConstraint.Z) == AxisConstraint.Z)
					{
						if (flag2)
						{
							upwards = options.parent.TransformDirection(Vector3.up);
						}
						else
						{
							upwards = transform.TransformDirection(Vector3.up);
						}
						upwards.z = options.startupZRot;
					}
				}
				if (options.mode == PathMode.Full3D)
				{
					Vector3 vector4 = vector - position;
					if (vector4 == Vector3.zero)
					{
						vector4 = transform.forward;
					}
					if (flag)
					{
						vector4 = this.DivideVectorByVector(vector4, options.parent.localScale);
					}
					quaternion = Quaternion.LookRotation(vector4, upwards);
				}
				else
				{
					if (flag)
					{
						Vector3 b = this.DivideVectorByVector(vector - position, options.parent.localScale);
						vector = position + b;
					}
					float y = 0f;
					float num2 = DOTweenUtils.Angle2D(position, vector);
					if (num2 < 0f)
					{
						num2 = 360f + num2;
					}
					if (options.mode == PathMode.Sidescroller2D)
					{
						y = (float)((vector.x < position.x) ? 180 : 0);
						if (num2 > 90f && num2 < 270f)
						{
							num2 = 180f - num2;
						}
					}
					quaternion = Quaternion.Euler(0f, y, num2);
				}
				break;
			}
			case OrientType.LookAtTransform:
				if (options.lookAtTransform != null)
				{
					path.lookAtPosition = new Vector3?(options.lookAtTransform.position);
					quaternion = Quaternion.LookRotation(options.lookAtTransform.position - position, options.stableZRotation ? Vector3.up : transform.up);
				}
				break;
			case OrientType.LookAtPosition:
				path.lookAtPosition = new Vector3?(options.lookAtPosition);
				quaternion = Quaternion.LookRotation(options.lookAtPosition - position, options.stableZRotation ? Vector3.up : transform.up);
				break;
			}
			if (options.hasCustomForwardDirection)
			{
				quaternion *= options.forward;
			}
			DOTweenExternalCommand.Dispatch_SetOrientationOnPath(options, t, quaternion, transform);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000A6FF File Offset: 0x000088FF
		private Vector3 DivideVectorByVector(Vector3 vector, Vector3 byVector)
		{
			return new Vector3(vector.x / byVector.x, vector.y / byVector.y, vector.z / byVector.z);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000A72D File Offset: 0x0000892D
		private Vector3 MultiplyVectorByVector(Vector3 vector, Vector3 byVector)
		{
			return new Vector3(vector.x * byVector.x, vector.y * byVector.y, vector.z * byVector.z);
		}

		// Token: 0x040000DD RID: 221
		public const float MinLookAhead = 0.0001f;
	}
}
