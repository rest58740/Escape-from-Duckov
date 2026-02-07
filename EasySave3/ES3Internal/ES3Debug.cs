using System;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000CD RID: 205
	internal static class ES3Debug
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x0001A72C File Offset: 0x0001892C
		public static void Log(string msg, UnityEngine.Object context = null, int indent = 0)
		{
			if (!ES3Settings.defaultSettingsScriptableObject.logDebugInfo)
			{
				return;
			}
			if (context != null)
			{
				Debug.LogFormat(context, ES3Debug.Indent(indent) + msg + "\n<i>To disable these messages from Easy Save, go to Window > Easy Save 3 > Settings, and uncheck 'Log Info'</i>", Array.Empty<object>());
				return;
			}
			Debug.LogFormat(context, ES3Debug.Indent(indent) + msg, Array.Empty<object>());
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001A784 File Offset: 0x00018984
		public static void LogWarning(string msg, UnityEngine.Object context = null, int indent = 0)
		{
			if (!ES3Settings.defaultSettingsScriptableObject.logWarnings)
			{
				return;
			}
			if (context != null)
			{
				Debug.LogWarningFormat(context, ES3Debug.Indent(indent) + msg + "\n<i>To disable warnings from Easy Save, go to Window > Easy Save 3 > Settings, and uncheck 'Log Warnings'</i>", Array.Empty<object>());
				return;
			}
			Debug.LogWarningFormat(context, ES3Debug.Indent(indent) + msg + "\n<i>To disable warnings from Easy Save, go to Window > Easy Save 3 > Settings, and uncheck 'Log Warnings'</i>", Array.Empty<object>());
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001A7E0 File Offset: 0x000189E0
		public static void LogError(string msg, UnityEngine.Object context = null, int indent = 0)
		{
			if (!ES3Settings.defaultSettingsScriptableObject.logErrors)
			{
				return;
			}
			if (context != null)
			{
				Debug.LogErrorFormat(context, ES3Debug.Indent(indent) + msg + "\n<i>To disable these error messages from Easy Save, go to Window > Easy Save 3 > Settings, and uncheck 'Log Errors'</i>", Array.Empty<object>());
				return;
			}
			Debug.LogErrorFormat(context, ES3Debug.Indent(indent) + msg + "\n<i>To disable these error messages from Easy Save, go to Window > Easy Save 3 > Settings, and uncheck 'Log Errors'</i>", Array.Empty<object>());
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001A83C File Offset: 0x00018A3C
		private static string Indent(int size)
		{
			if (size < 0)
			{
				return "";
			}
			return new string('-', size);
		}

		// Token: 0x04000110 RID: 272
		private const string disableInfoMsg = "\n<i>To disable these messages from Easy Save, go to Window > Easy Save 3 > Settings, and uncheck 'Log Info'</i>";

		// Token: 0x04000111 RID: 273
		private const string disableWarningMsg = "\n<i>To disable warnings from Easy Save, go to Window > Easy Save 3 > Settings, and uncheck 'Log Warnings'</i>";

		// Token: 0x04000112 RID: 274
		private const string disableErrorMsg = "\n<i>To disable these error messages from Easy Save, go to Window > Easy Save 3 > Settings, and uncheck 'Log Errors'</i>";

		// Token: 0x04000113 RID: 275
		private const char indentChar = '-';
	}
}
