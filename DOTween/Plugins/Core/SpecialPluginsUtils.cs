using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
	// Token: 0x0200003F RID: 63
	internal static class SpecialPluginsUtils
	{
		// Token: 0x0600025A RID: 602 RVA: 0x0000D994 File Offset: 0x0000BB94
		internal static bool SetLookAt(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
		{
			Transform transform = t.target as Transform;
			Vector3 vector = t.endValue;
			vector -= transform.position;
			AxisConstraint axisConstraint = t.plugOptions.axisConstraint;
			if (axisConstraint != AxisConstraint.X)
			{
				if (axisConstraint != AxisConstraint.Y)
				{
					if (axisConstraint == AxisConstraint.Z)
					{
						vector.z = 0f;
					}
				}
				else
				{
					vector.y = 0f;
				}
			}
			else
			{
				vector.x = 0f;
			}
			Vector3 eulerAngles = Quaternion.LookRotation(vector, t.plugOptions.up).eulerAngles;
			t.endValue = eulerAngles;
			return true;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000DA28 File Offset: 0x0000BC28
		internal static bool SetPunch(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
		{
			Vector3 b;
			try
			{
				b = t.getter();
			}
			catch
			{
				return false;
			}
			t.isRelative = (t.isSpeedBased = false);
			t.easeType = Ease.OutQuad;
			t.customEase = null;
			int num = t.endValue.Length;
			for (int i = 0; i < num; i++)
			{
				t.endValue[i] = t.endValue[i] + b;
			}
			return true;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000DAB4 File Offset: 0x0000BCB4
		internal static bool SetShake(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
		{
			if (!SpecialPluginsUtils.SetPunch(t))
			{
				return false;
			}
			t.easeType = Ease.Linear;
			return true;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000DAC8 File Offset: 0x0000BCC8
		internal static bool SetCameraShakePosition(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
		{
			if (!SpecialPluginsUtils.SetShake(t))
			{
				return false;
			}
			Camera camera = t.target as Camera;
			if (camera == null)
			{
				return false;
			}
			Vector3 b = t.getter();
			Transform transform = camera.transform;
			int num = t.endValue.Length;
			for (int i = 0; i < num; i++)
			{
				Vector3 a = t.endValue[i];
				t.endValue[i] = transform.localRotation * (a - b) + b;
			}
			return true;
		}
	}
}
