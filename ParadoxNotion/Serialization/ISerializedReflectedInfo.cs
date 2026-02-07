using System;
using System.Reflection;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x0200008A RID: 138
	public interface ISerializedReflectedInfo : ISerializationCallbackReceiver
	{
		// Token: 0x0600059A RID: 1434
		MemberInfo AsMemberInfo();

		// Token: 0x0600059B RID: 1435
		string AsString();
	}
}
