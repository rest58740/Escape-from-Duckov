using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000AD RID: 173
	public class Vector2IntFormatter : MinimalBaseFormatter<Vector2Int>
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x000212CD File Offset: 0x0001F4CD
		protected override void Read(ref Vector2Int value, IDataReader reader)
		{
			value.x = Vector2IntFormatter.Serializer.ReadValue(reader);
			value.y = Vector2IntFormatter.Serializer.ReadValue(reader);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x000212F1 File Offset: 0x0001F4F1
		protected override void Write(ref Vector2Int value, IDataWriter writer)
		{
			Vector2IntFormatter.Serializer.WriteValue(value.x, writer);
			Vector2IntFormatter.Serializer.WriteValue(value.y, writer);
		}

		// Token: 0x040001B6 RID: 438
		private static readonly Serializer<int> Serializer = Sirenix.Serialization.Serializer.Get<int>();
	}
}
