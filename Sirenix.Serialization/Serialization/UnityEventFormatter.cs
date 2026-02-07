using System;
using UnityEngine.Events;

namespace Sirenix.Serialization
{
	// Token: 0x020000A8 RID: 168
	public class UnityEventFormatter<T> : ReflectionFormatter<T> where T : UnityEventBase, new()
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x00021121 File Offset: 0x0001F321
		protected override T GetUninitializedObject()
		{
			return Activator.CreateInstance<T>();
		}
	}
}
