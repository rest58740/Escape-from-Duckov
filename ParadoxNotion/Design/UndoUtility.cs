using System;
using System.Diagnostics;
using UnityEngine;

namespace ParadoxNotion.Design
{
	// Token: 0x020000E5 RID: 229
	public static class UndoUtility
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x00017288 File Offset: 0x00015488
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x0001728F File Offset: 0x0001548F
		public static string lastOperationName { get; private set; }

		// Token: 0x06000771 RID: 1905 RVA: 0x00017297 File Offset: 0x00015497
		[Conditional("UNITY_EDITOR")]
		public static void RecordObject(Object target, string name)
		{
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00017299 File Offset: 0x00015499
		[Conditional("UNITY_EDITOR")]
		public static void RecordObjectComplete(Object target, string name)
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001729B File Offset: 0x0001549B
		[Conditional("UNITY_EDITOR")]
		public static void SetDirty(Object target)
		{
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001729D File Offset: 0x0001549D
		[Conditional("UNITY_EDITOR")]
		public static void RecordObject(Object target, string name, Action operation)
		{
			operation.Invoke();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000172A5 File Offset: 0x000154A5
		[Conditional("UNITY_EDITOR")]
		public static void RecordObjectComplete(Object target, string name, Action operation)
		{
			operation.Invoke();
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x000172AD File Offset: 0x000154AD
		public static string GetLastOperationNameOr(string operation)
		{
			if (!string.IsNullOrEmpty(UndoUtility.lastOperationName))
			{
				return UndoUtility.lastOperationName;
			}
			return operation;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x000172C4 File Offset: 0x000154C4
		public static void CheckUndo(Object target, string name)
		{
			Event current = Event.current;
			if (current.type == EventType.MouseDown || current.type == EventType.KeyDown || current.type == EventType.DragPerform || current.type == EventType.ExecuteCommand)
			{
				UndoUtility.lastOperationName = name;
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00017302 File Offset: 0x00015502
		public static void CheckDirty(Object target)
		{
			bool changed = GUI.changed;
		}
	}
}
