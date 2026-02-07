using System;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class TaskEventEmitter : MonoBehaviour
{
	// Token: 0x060009DC RID: 2524 RVA: 0x0002B6E1 File Offset: 0x000298E1
	public void SetKey(string key)
	{
		this.eventKey = key;
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x0002B6EA File Offset: 0x000298EA
	private void Awake()
	{
		if (this.emitOnAwake)
		{
			this.EmitEvent();
		}
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x0002B6FA File Offset: 0x000298FA
	public void EmitEvent()
	{
		Debug.Log("TaskEvent:" + this.eventKey);
		TaskEvent.EmitTaskEvent(this.eventKey);
	}

	// Token: 0x040008C9 RID: 2249
	[SerializeField]
	private string eventKey;

	// Token: 0x040008CA RID: 2250
	[SerializeField]
	private bool emitOnAwake;
}
