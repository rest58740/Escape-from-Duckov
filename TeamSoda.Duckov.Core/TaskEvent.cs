using System;

// Token: 0x02000126 RID: 294
public class TaskEvent
{
	// Token: 0x14000049 RID: 73
	// (add) Token: 0x060009D8 RID: 2520 RVA: 0x0002B660 File Offset: 0x00029860
	// (remove) Token: 0x060009D9 RID: 2521 RVA: 0x0002B694 File Offset: 0x00029894
	public static event Action<string> OnTaskEvent;

	// Token: 0x060009DA RID: 2522 RVA: 0x0002B6C7 File Offset: 0x000298C7
	public static void EmitTaskEvent(string taskEventKey)
	{
		Action<string> onTaskEvent = TaskEvent.OnTaskEvent;
		if (onTaskEvent == null)
		{
			return;
		}
		onTaskEvent(taskEventKey);
	}
}
