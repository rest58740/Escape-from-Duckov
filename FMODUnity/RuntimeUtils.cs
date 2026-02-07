using System;
using FMOD;
using FMOD.Studio;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x02000120 RID: 288
	public static class RuntimeUtils
	{
		// Token: 0x06000769 RID: 1897 RVA: 0x0000A5CA File Offset: 0x000087CA
		public static string GetCommonPlatformPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return path;
			}
			return path.Replace('\\', '/');
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0000A5E0 File Offset: 0x000087E0
		public static VECTOR ToFMODVector(this Vector3 vec)
		{
			VECTOR result;
			result.x = vec.x;
			result.y = vec.y;
			result.z = vec.z;
			return result;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0000A618 File Offset: 0x00008818
		public static ATTRIBUTES_3D To3DAttributes(this Vector3 pos)
		{
			return new ATTRIBUTES_3D
			{
				forward = Vector3.forward.ToFMODVector(),
				up = Vector3.up.ToFMODVector(),
				position = pos.ToFMODVector()
			};
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0000A660 File Offset: 0x00008860
		public static ATTRIBUTES_3D To3DAttributes(this Transform transform)
		{
			return new ATTRIBUTES_3D
			{
				forward = transform.forward.ToFMODVector(),
				up = transform.up.ToFMODVector(),
				position = transform.position.ToFMODVector()
			};
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0000A6AC File Offset: 0x000088AC
		public static ATTRIBUTES_3D To3DAttributes(this Transform transform, Vector3 velocity)
		{
			return new ATTRIBUTES_3D
			{
				forward = transform.forward.ToFMODVector(),
				up = transform.up.ToFMODVector(),
				position = transform.position.ToFMODVector(),
				velocity = velocity.ToFMODVector()
			};
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0000A705 File Offset: 0x00008905
		public static ATTRIBUTES_3D To3DAttributes(this GameObject go)
		{
			return go.transform.To3DAttributes();
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0000A714 File Offset: 0x00008914
		public static ATTRIBUTES_3D To3DAttributes(Transform transform, Rigidbody rigidbody = null)
		{
			ATTRIBUTES_3D result = transform.To3DAttributes();
			if (rigidbody)
			{
				result.velocity = rigidbody.velocity.ToFMODVector();
			}
			return result;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0000A744 File Offset: 0x00008944
		public static ATTRIBUTES_3D To3DAttributes(GameObject go, Rigidbody rigidbody)
		{
			ATTRIBUTES_3D result = go.transform.To3DAttributes();
			if (rigidbody)
			{
				result.velocity = rigidbody.velocity.ToFMODVector();
			}
			return result;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0000A778 File Offset: 0x00008978
		public static ATTRIBUTES_3D To3DAttributes(Transform transform, Rigidbody2D rigidbody)
		{
			ATTRIBUTES_3D result = transform.To3DAttributes();
			if (rigidbody)
			{
				VECTOR velocity;
				velocity.x = rigidbody.velocity.x;
				velocity.y = rigidbody.velocity.y;
				velocity.z = 0f;
				result.velocity = velocity;
			}
			return result;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0000A7D0 File Offset: 0x000089D0
		public static ATTRIBUTES_3D To3DAttributes(GameObject go, Rigidbody2D rigidbody)
		{
			ATTRIBUTES_3D result = go.transform.To3DAttributes();
			if (rigidbody)
			{
				VECTOR velocity;
				velocity.x = rigidbody.velocity.x;
				velocity.y = rigidbody.velocity.y;
				velocity.z = 0f;
				result.velocity = velocity;
			}
			return result;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0000A82C File Offset: 0x00008A2C
		public static THREAD_TYPE ToFMODThreadType(ThreadType threadType)
		{
			switch (threadType)
			{
			case ThreadType.Mixer:
				return THREAD_TYPE.MIXER;
			case ThreadType.Feeder:
				return THREAD_TYPE.FEEDER;
			case ThreadType.Stream:
				return THREAD_TYPE.STREAM;
			case ThreadType.File:
				return THREAD_TYPE.FILE;
			case ThreadType.Nonblocking:
				return THREAD_TYPE.NONBLOCKING;
			case ThreadType.Record:
				return THREAD_TYPE.RECORD;
			case ThreadType.Geometry:
				return THREAD_TYPE.GEOMETRY;
			case ThreadType.Profiler:
				return THREAD_TYPE.PROFILER;
			case ThreadType.Studio_Update:
				return THREAD_TYPE.STUDIO_UPDATE;
			case ThreadType.Studio_Load_Bank:
				return THREAD_TYPE.STUDIO_LOAD_BANK;
			case ThreadType.Studio_Load_Sample:
				return THREAD_TYPE.STUDIO_LOAD_SAMPLE;
			case ThreadType.Convolution_1:
				return THREAD_TYPE.CONVOLUTION1;
			case ThreadType.Convolution_2:
				return THREAD_TYPE.CONVOLUTION2;
			default:
				throw new ArgumentException("Unrecognised thread type '" + threadType.ToString() + "'");
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0000A8B4 File Offset: 0x00008AB4
		public static string DisplayName(this ThreadType thread)
		{
			return thread.ToString().Replace('_', ' ');
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0000A8CC File Offset: 0x00008ACC
		public static THREAD_AFFINITY ToFMODThreadAffinity(ThreadAffinity affinity)
		{
			THREAD_AFFINITY result = THREAD_AFFINITY.CORE_ALL;
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core0, THREAD_AFFINITY.CORE_0, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core1, THREAD_AFFINITY.CORE_1, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core2, THREAD_AFFINITY.CORE_2, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core3, THREAD_AFFINITY.CORE_3, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core4, THREAD_AFFINITY.CORE_4, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core5, THREAD_AFFINITY.CORE_5, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core6, THREAD_AFFINITY.CORE_6, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core7, THREAD_AFFINITY.CORE_7, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core8, THREAD_AFFINITY.CORE_8, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core9, THREAD_AFFINITY.CORE_9, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core10, THREAD_AFFINITY.CORE_10, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core11, THREAD_AFFINITY.CORE_11, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core12, THREAD_AFFINITY.CORE_12, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core13, THREAD_AFFINITY.CORE_13, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core14, THREAD_AFFINITY.CORE_14, ref result);
			RuntimeUtils.SetFMODAffinityBit(affinity, ThreadAffinity.Core15, THREAD_AFFINITY.CORE_15, ref result);
			return result;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0000A9DB File Offset: 0x00008BDB
		private static void SetFMODAffinityBit(ThreadAffinity affinity, ThreadAffinity mask, THREAD_AFFINITY fmodMask, ref THREAD_AFFINITY fmodAffinity)
		{
			if ((affinity & mask) != ThreadAffinity.Any)
			{
				fmodAffinity |= fmodMask;
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0000A9E8 File Offset: 0x00008BE8
		public static void EnforceLibraryOrder()
		{
			int num;
			int num2;
			Memory.GetStats(out num, out num2, true);
			GUID guid;
			Util.parseID("", out guid);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0000AA0D File Offset: 0x00008C0D
		public static void DebugLog(string message)
		{
			if (!Settings.IsInitialized() || Settings.Instance.LoggingLevel == DEBUG_FLAGS.LOG)
			{
				UnityEngine.Debug.Log(message);
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0000AA29 File Offset: 0x00008C29
		public static void DebugLogFormat(string format, params object[] args)
		{
			if (!Settings.IsInitialized() || Settings.Instance.LoggingLevel == DEBUG_FLAGS.LOG)
			{
				UnityEngine.Debug.LogFormat(format, args);
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0000AA46 File Offset: 0x00008C46
		public static void DebugLogWarning(string message)
		{
			if (!Settings.IsInitialized() || Settings.Instance.LoggingLevel >= DEBUG_FLAGS.WARNING)
			{
				UnityEngine.Debug.LogWarning(message);
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0000AA62 File Offset: 0x00008C62
		public static void DebugLogWarningFormat(string format, params object[] args)
		{
			if (!Settings.IsInitialized() || Settings.Instance.LoggingLevel >= DEBUG_FLAGS.WARNING)
			{
				UnityEngine.Debug.LogWarningFormat(format, args);
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0000AA7F File Offset: 0x00008C7F
		public static void DebugLogError(string message)
		{
			if (!Settings.IsInitialized() || Settings.Instance.LoggingLevel >= DEBUG_FLAGS.ERROR)
			{
				UnityEngine.Debug.LogError(message);
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0000AA9B File Offset: 0x00008C9B
		public static void DebugLogErrorFormat(string format, params object[] args)
		{
			if (!Settings.IsInitialized() || Settings.Instance.LoggingLevel >= DEBUG_FLAGS.ERROR)
			{
				UnityEngine.Debug.LogErrorFormat(format, args);
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0000AAB8 File Offset: 0x00008CB8
		public static void DebugLogException(Exception e)
		{
			if (!Settings.IsInitialized() || Settings.Instance.LoggingLevel >= DEBUG_FLAGS.ERROR)
			{
				UnityEngine.Debug.LogException(e);
			}
		}
	}
}
