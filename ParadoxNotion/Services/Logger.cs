using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace ParadoxNotion.Services
{
	// Token: 0x02000082 RID: 130
	public static class Logger
	{
		// Token: 0x06000560 RID: 1376 RVA: 0x0000F976 File Offset: 0x0000DB76
		public static void AddListener(Logger.LogHandler callback)
		{
			Logger.subscribers.Add(callback);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0000F983 File Offset: 0x0000DB83
		public static void RemoveListener(Logger.LogHandler callback)
		{
			Logger.subscribers.Remove(callback);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0000F991 File Offset: 0x0000DB91
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		public static void Log(object message, string tag = null, object context = null)
		{
			Logger.Internal_Log(LogType.Log, message, tag, context);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0000F99C File Offset: 0x0000DB9C
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		public static void LogWarning(object message, string tag = null, object context = null)
		{
			Logger.Internal_Log(LogType.Warning, message, tag, context);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0000F9A7 File Offset: 0x0000DBA7
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		public static void LogError(object message, string tag = null, object context = null)
		{
			Logger.Internal_Log(LogType.Error, message, tag, context);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0000F9B2 File Offset: 0x0000DBB2
		public static void LogException(Exception exception, string tag = null, object context = null)
		{
			Logger.Internal_Log(LogType.Exception, exception, tag, context);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0000F9C0 File Offset: 0x0000DBC0
		private static void Internal_Log(LogType type, object message, string tag, object context)
		{
			if (!Logger.enabled)
			{
				return;
			}
			if (Logger.subscribers != null && Logger.subscribers.Count > 0)
			{
				Logger.Message message2 = default(Logger.Message);
				message2.type = type;
				if (message is Exception)
				{
					Exception ex = (Exception)message;
					message2.text = ex.Message + "\n" + ex.StackTrace.Split('\n', 0).FirstOrDefault<string>();
				}
				else
				{
					message2.text = ((message != null) ? message.ToString() : "NULL");
				}
				message2.tag = tag;
				message2.context = context;
				bool flag = false;
				using (List<Logger.LogHandler>.Enumerator enumerator = Logger.subscribers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current(message2))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag && type != LogType.Exception)
				{
					return;
				}
			}
			if (!string.IsNullOrEmpty(tag))
			{
				tag = string.Format("<b>({0} {1})</b>", tag, type.ToString());
			}
			else
			{
				tag = string.Format("<b>({0})</b>", type.ToString());
			}
			Logger.ForwardToUnity(type, message, tag, context);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0000FAFC File Offset: 0x0000DCFC
		private static void ForwardToUnity(LogType type, object message, string tag, object context)
		{
			if (message is Exception)
			{
				UnityEngine.Debug.unityLogger.LogException((Exception)message);
				return;
			}
			UnityEngine.Debug.unityLogger.Log(type, tag, message, context as Object);
		}

		// Token: 0x040001AF RID: 431
		private static List<Logger.LogHandler> subscribers = new List<Logger.LogHandler>();

		// Token: 0x040001B0 RID: 432
		public static bool enabled = true;

		// Token: 0x02000124 RID: 292
		public struct Message
		{
			// Token: 0x1700015B RID: 347
			// (get) Token: 0x06000835 RID: 2101 RVA: 0x0001839C File Offset: 0x0001659C
			// (set) Token: 0x06000836 RID: 2102 RVA: 0x000183C2 File Offset: 0x000165C2
			public object context
			{
				get
				{
					object result = null;
					if (this._contextRef != null)
					{
						this._contextRef.TryGetTarget(ref result);
					}
					return result;
				}
				set
				{
					this._contextRef = new WeakReference<object>(value);
				}
			}

			// Token: 0x06000837 RID: 2103 RVA: 0x000183D0 File Offset: 0x000165D0
			public bool IsValid()
			{
				return !string.IsNullOrEmpty(this.text);
			}

			// Token: 0x040002EE RID: 750
			private WeakReference<object> _contextRef;

			// Token: 0x040002EF RID: 751
			public LogType type;

			// Token: 0x040002F0 RID: 752
			public string text;

			// Token: 0x040002F1 RID: 753
			public string tag;
		}

		// Token: 0x02000125 RID: 293
		// (Invoke) Token: 0x06000839 RID: 2105
		public delegate bool LogHandler(Logger.Message message);
	}
}
