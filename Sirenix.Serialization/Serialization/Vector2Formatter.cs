using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000AA RID: 170
	public class Vector2Formatter : MinimalBaseFormatter<Vector2>
	{
		// Token: 0x060004C9 RID: 1225 RVA: 0x00021139 File Offset: 0x0001F339
		protected override void Read(ref Vector2 value, IDataReader reader)
		{
			value.x = Vector2Formatter.FloatSerializer.ReadValue(reader);
			value.y = Vector2Formatter.FloatSerializer.ReadValue(reader);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0002115D File Offset: 0x0001F35D
		protected override void Write(ref Vector2 value, IDataWriter writer)
		{
			Vector2Formatter.FloatSerializer.WriteValue(value.x, writer);
			Vector2Formatter.FloatSerializer.WriteValue(value.y, writer);
		}

		// Token: 0x040001B3 RID: 435
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();
	}
}
