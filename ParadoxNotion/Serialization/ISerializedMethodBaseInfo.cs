using System;
using System.Reflection;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x02000089 RID: 137
	public interface ISerializedMethodBaseInfo : ISerializedReflectedInfo, ISerializationCallbackReceiver
	{
		// Token: 0x06000598 RID: 1432
		MethodBase GetMethodBase();

		// Token: 0x06000599 RID: 1433
		bool HasChanged();
	}
}
