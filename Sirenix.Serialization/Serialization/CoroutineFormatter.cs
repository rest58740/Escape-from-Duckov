using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000A0 RID: 160
	public sealed class CoroutineFormatter : IFormatter<Coroutine>, IFormatter
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00020B4D File Offset: 0x0001ED4D
		public Type SerializedType
		{
			get
			{
				return typeof(Coroutine);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000EE6B File Offset: 0x0000D06B
		object IFormatter.Deserialize(IDataReader reader)
		{
			return null;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000EE6B File Offset: 0x0000D06B
		public Coroutine Deserialize(IDataReader reader)
		{
			return null;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000021B8 File Offset: 0x000003B8
		public void Serialize(object value, IDataWriter writer)
		{
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000021B8 File Offset: 0x000003B8
		public void Serialize(Coroutine value, IDataWriter writer)
		{
		}
	}
}
