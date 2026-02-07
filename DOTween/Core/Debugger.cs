using System;
using DG.Tweening.Core.Enums;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x0200004E RID: 78
	public static class Debugger
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000F398 File Offset: 0x0000D598
		public static int logPriority
		{
			get
			{
				return Debugger._logPriority;
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000F3A0 File Offset: 0x0000D5A0
		public static void Log(object message)
		{
			string text = "<color=#0099bc><b>DOTWEEN ► </b></color>" + ((message != null) ? message.ToString() : null);
			if (DOTween.onWillLog != null && !DOTween.onWillLog(LogType.Log, text))
			{
				return;
			}
			Debug.Log(text);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000F3E0 File Offset: 0x0000D5E0
		public static void LogWarning(object message, Tween t = null)
		{
			string text;
			if (DOTween.debugMode)
			{
				text = "<color=#0099bc><b>DOTWEEN ► </b></color>" + Debugger.GetDebugDataMessage(t) + ((message != null) ? message.ToString() : null);
			}
			else
			{
				text = "<color=#0099bc><b>DOTWEEN ► </b></color>" + ((message != null) ? message.ToString() : null);
			}
			if (DOTween.onWillLog != null && !DOTween.onWillLog(LogType.Warning, text))
			{
				return;
			}
			Debug.LogWarning(text);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000F448 File Offset: 0x0000D648
		public static void LogError(object message, Tween t = null)
		{
			string text;
			if (DOTween.debugMode)
			{
				text = "<color=#0099bc><b>DOTWEEN ► </b></color>" + Debugger.GetDebugDataMessage(t) + ((message != null) ? message.ToString() : null);
			}
			else
			{
				text = "<color=#0099bc><b>DOTWEEN ► </b></color>" + ((message != null) ? message.ToString() : null);
			}
			if (DOTween.onWillLog != null && !DOTween.onWillLog(LogType.Error, text))
			{
				return;
			}
			Debug.LogError(text);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000F4B0 File Offset: 0x0000D6B0
		public static void LogSafeModeCapturedError(object message, Tween t = null)
		{
			string text;
			if (DOTween.debugMode)
			{
				text = "<color=#0099bc><b>DOTWEEN ► </b></color>" + Debugger.GetDebugDataMessage(t) + ((message != null) ? message.ToString() : null);
			}
			else
			{
				text = "<color=#0099bc><b>DOTWEEN ► </b></color>" + ((message != null) ? message.ToString() : null);
			}
			if (DOTween.onWillLog != null && !DOTween.onWillLog(LogType.Log, text))
			{
				return;
			}
			switch (DOTween.safeModeLogBehaviour)
			{
			case SafeModeLogBehaviour.Normal:
				Debug.Log(text);
				return;
			case SafeModeLogBehaviour.Warning:
				Debug.LogWarning(text);
				return;
			case SafeModeLogBehaviour.Error:
				Debug.LogError(text);
				return;
			default:
				return;
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000F540 File Offset: 0x0000D740
		public static void LogReport(object message)
		{
			string text = string.Format("<color=#00B500FF>{0} REPORT ►</color> {1}", "<color=#0099bc><b>DOTWEEN ► </b></color>", message);
			if (DOTween.onWillLog != null && !DOTween.onWillLog(LogType.Log, text))
			{
				return;
			}
			Debug.Log(text);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000F57C File Offset: 0x0000D77C
		public static void LogSafeModeReport(object message)
		{
			string text = string.Format("<color=#ff7337>{0} SAFE MODE ►</color> {1}", "<color=#0099bc><b>DOTWEEN ► </b></color>", message);
			if (DOTween.onWillLog != null && !DOTween.onWillLog(LogType.Log, text))
			{
				return;
			}
			Debug.LogWarning(text);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000F5B6 File Offset: 0x0000D7B6
		public static void LogInvalidTween(Tween t)
		{
			Debugger.LogWarning("This Tween has been killed and is now invalid", null);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000F5C3 File Offset: 0x0000D7C3
		public static void LogNestedTween(Tween t)
		{
			Debugger.LogWarning("This Tween was added to a Sequence and can't be controlled directly", t);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000F5D0 File Offset: 0x0000D7D0
		public static void LogNullTween(Tween t)
		{
			Debugger.LogWarning("Null Tween", null);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000F5DD File Offset: 0x0000D7DD
		public static void LogNonPathTween(Tween t)
		{
			Debugger.LogWarning("This Tween is not a path tween", t);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000F5EA File Offset: 0x0000D7EA
		public static void LogMissingMaterialProperty(string propertyName)
		{
			Debugger.LogWarning(string.Format("This material doesn't have a {0} property", propertyName), null);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000F5FD File Offset: 0x0000D7FD
		public static void LogMissingMaterialProperty(int propertyId)
		{
			Debugger.LogWarning(string.Format("This material doesn't have a {0} property ID", propertyId), null);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000F615 File Offset: 0x0000D815
		public static void LogRemoveActiveTweenError(string errorInfo, Tween t)
		{
			Debugger.LogWarning(string.Format("Error in RemoveActiveTween ({0}). It's been taken care of so no problems, but Daniele (DOTween's author) is trying to pinpoint it (it's very rare and he can't reproduce it) so it would be awesome if you could reproduce this log in a sample project and send it to him. Or even just write him the complete log that was generated by this message. Fixing this would make DOTween slightly faster. Thanks.", errorInfo), t);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000F628 File Offset: 0x0000D828
		public static void LogAddActiveTweenError(string errorInfo, Tween t)
		{
			Debugger.LogWarning(string.Format("Error in AddActiveTween ({0}). It's been taken care of so no problems, but Daniele (DOTween's author) is trying to pinpoint it (it's very rare and he can't reproduce it) so it would be awesome if you could reproduce this log in a sample project and send it to him. Or even just write him the complete log that was generated by this message. Fixing this would make DOTween slightly faster. Thanks.", errorInfo), t);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000F63B File Offset: 0x0000D83B
		public static void SetLogPriority(LogBehaviour logBehaviour)
		{
			if (logBehaviour == LogBehaviour.Default)
			{
				Debugger._logPriority = 1;
				return;
			}
			if (logBehaviour != LogBehaviour.Verbose)
			{
				Debugger._logPriority = 0;
				return;
			}
			Debugger._logPriority = 2;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000F65C File Offset: 0x0000D85C
		public static bool ShouldLogSafeModeCapturedError()
		{
			SafeModeLogBehaviour safeModeLogBehaviour = DOTween.safeModeLogBehaviour;
			return safeModeLogBehaviour != SafeModeLogBehaviour.None && (safeModeLogBehaviour - SafeModeLogBehaviour.Normal > 1 || Debugger._logPriority >= 1);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000F68C File Offset: 0x0000D88C
		private static string GetDebugDataMessage(Tween t)
		{
			string result = "";
			Debugger.AddDebugDataToMessage(ref result, t);
			return result;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000F6A8 File Offset: 0x0000D8A8
		private static void AddDebugDataToMessage(ref string message, Tween t)
		{
			if (t == null)
			{
				return;
			}
			bool flag = t.debugTargetId != null;
			bool flag2 = t.stringId != null;
			bool flag3 = t.intId != -999;
			if (flag || flag2 || flag3)
			{
				message += "DEBUG MODE INFO ► ";
				if (flag)
				{
					message += string.Format("[tween target: {0}]", t.debugTargetId);
				}
				if (flag2)
				{
					message += string.Format("[stringId: {0}]", t.stringId);
				}
				if (flag3)
				{
					message += string.Format("[intId: {0}]", t.intId);
				}
				message += "\n";
			}
		}

		// Token: 0x04000147 RID: 327
		private static int _logPriority;

		// Token: 0x04000148 RID: 328
		private const string _LogPrefix = "<color=#0099bc><b>DOTWEEN ► </b></color>";

		// Token: 0x020000B9 RID: 185
		internal static class Sequence
		{
			// Token: 0x0600043E RID: 1086 RVA: 0x000140B0 File Offset: 0x000122B0
			public static void LogAddToNullSequence()
			{
				Debugger.LogWarning("You can't add elements to a NULL Sequence", null);
			}

			// Token: 0x0600043F RID: 1087 RVA: 0x000140BD File Offset: 0x000122BD
			public static void LogAddToInactiveSequence()
			{
				Debugger.LogWarning("You can't add elements to an inactive/killed Sequence", null);
			}

			// Token: 0x06000440 RID: 1088 RVA: 0x000140CA File Offset: 0x000122CA
			public static void LogAddToLockedSequence()
			{
				Debugger.LogWarning("The Sequence has started and is now locked, you can only elements to a Sequence before it starts", null);
			}

			// Token: 0x06000441 RID: 1089 RVA: 0x000140D7 File Offset: 0x000122D7
			public static void LogAddNullTween()
			{
				Debugger.LogWarning("You can't add a NULL tween to a Sequence", null);
			}

			// Token: 0x06000442 RID: 1090 RVA: 0x000140E4 File Offset: 0x000122E4
			public static void LogAddInactiveTween(Tween t)
			{
				Debugger.LogWarning("You can't add an inactive/killed tween to a Sequence", t);
			}

			// Token: 0x06000443 RID: 1091 RVA: 0x000140F1 File Offset: 0x000122F1
			public static void LogAddAlreadySequencedTween(Tween t)
			{
				Debugger.LogWarning("You can't add a tween that is already nested into a Sequence to another Sequence", t);
			}
		}
	}
}
