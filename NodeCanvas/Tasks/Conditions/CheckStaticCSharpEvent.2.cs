using System;
using ParadoxNotion.Serialization.FullSerializer;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000033 RID: 51
	[fsMigrateTo(typeof(CheckCSharpEvent<>))]
	internal class CheckStaticCSharpEvent<T>
	{
		// Token: 0x04000096 RID: 150
		[SerializeField]
		public Type targetType;

		// Token: 0x04000097 RID: 151
		[SerializeField]
		public string eventName;
	}
}
