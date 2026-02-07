using System;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000025 RID: 37
	public interface ITaskSystem
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001EB RID: 491
		Component agent { get; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001EC RID: 492
		IBlackboard blackboard { get; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001ED RID: 493
		Object contextObject { get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001EE RID: 494
		float elapsedTime { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001EF RID: 495
		float deltaTime { get; }

		// Token: 0x060001F0 RID: 496
		void UpdateTasksOwner();

		// Token: 0x060001F1 RID: 497
		void SendEvent(string name, object value, object sender);

		// Token: 0x060001F2 RID: 498
		void SendEvent<T>(string name, T value, object sender);
	}
}
