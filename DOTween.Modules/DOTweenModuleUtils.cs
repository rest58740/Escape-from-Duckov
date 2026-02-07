using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Scripting;

namespace DG.Tweening
{
	// Token: 0x0200000A RID: 10
	public static class DOTweenModuleUtils
	{
		// Token: 0x06000064 RID: 100 RVA: 0x000042BF File Offset: 0x000024BF
		[Preserve]
		public static void Init()
		{
			if (DOTweenModuleUtils._initialized)
			{
				return;
			}
			DOTweenModuleUtils._initialized = true;
			DOTweenExternalCommand.SetOrientationOnPath += DOTweenModuleUtils.Physics.SetOrientationOnPath;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000042E0 File Offset: 0x000024E0
		[Preserve]
		private static void Preserver()
		{
			AppDomain.CurrentDomain.GetAssemblies();
			typeof(MonoBehaviour).GetMethod("Stub");
		}

		// Token: 0x04000001 RID: 1
		private static bool _initialized;

		// Token: 0x0200005F RID: 95
		public static class Physics
		{
			// Token: 0x0600014D RID: 333 RVA: 0x000058CC File Offset: 0x00003ACC
			public static void SetOrientationOnPath(PathOptions options, Tween t, Quaternion newRot, Transform trans)
			{
				if (options.isRigidbody)
				{
					((Rigidbody)t.target).rotation = newRot;
					return;
				}
				trans.rotation = newRot;
			}

			// Token: 0x0600014E RID: 334 RVA: 0x000058EF File Offset: 0x00003AEF
			public static bool HasRigidbody2D(Component target)
			{
				return target.GetComponent<Rigidbody2D>() != null;
			}

			// Token: 0x0600014F RID: 335 RVA: 0x000058FD File Offset: 0x00003AFD
			[Preserve]
			public static bool HasRigidbody(Component target)
			{
				return target.GetComponent<Rigidbody>() != null;
			}

			// Token: 0x06000150 RID: 336 RVA: 0x0000590C File Offset: 0x00003B0C
			[Preserve]
			public static TweenerCore<Vector3, Path, PathOptions> CreateDOTweenPathTween(MonoBehaviour target, bool tweenRigidbody, bool isLocal, Path path, float duration, PathMode pathMode)
			{
				TweenerCore<Vector3, Path, PathOptions> result = null;
				bool flag = false;
				if (tweenRigidbody)
				{
					Rigidbody component = target.GetComponent<Rigidbody>();
					if (component != null)
					{
						flag = true;
						result = (isLocal ? component.DOLocalPath(path, duration, pathMode) : component.DOPath(path, duration, pathMode));
					}
				}
				if (!flag && tweenRigidbody)
				{
					Rigidbody2D component2 = target.GetComponent<Rigidbody2D>();
					if (component2 != null)
					{
						flag = true;
						result = (isLocal ? component2.DOLocalPath(path, duration, pathMode) : component2.DOPath(path, duration, pathMode));
					}
				}
				if (!flag)
				{
					result = (isLocal ? ShortcutExtensions.DOLocalPath(target.transform, path, duration, pathMode) : ShortcutExtensions.DOPath(target.transform, path, duration, pathMode));
				}
				return result;
			}
		}
	}
}
