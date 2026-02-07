using System;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000032 RID: 50
	[fsMigrateTo(typeof(CheckCSharpEvent))]
	internal class CheckStaticCSharpEvent
	{
		// Token: 0x04000094 RID: 148
		[SerializeField]
		public Type targetType;

		// Token: 0x04000095 RID: 149
		[SerializeField]
		public string eventName;
	}
}
