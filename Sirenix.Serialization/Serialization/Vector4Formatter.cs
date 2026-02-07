using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000AC RID: 172
	public class Vector4Formatter : MinimalBaseFormatter<Vector4>
	{
		// Token: 0x060004D1 RID: 1233 RVA: 0x00021214 File Offset: 0x0001F414
		protected override void Read(ref Vector4 value, IDataReader reader)
		{
			value.x = Vector4Formatter.FloatSerializer.ReadValue(reader);
			value.y = Vector4Formatter.FloatSerializer.ReadValue(reader);
			value.z = Vector4Formatter.FloatSerializer.ReadValue(reader);
			value.w = Vector4Formatter.FloatSerializer.ReadValue(reader);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00021268 File Offset: 0x0001F468
		protected override void Write(ref Vector4 value, IDataWriter writer)
		{
			Vector4Formatter.FloatSerializer.WriteValue(value.x, writer);
			Vector4Formatter.FloatSerializer.WriteValue(value.y, writer);
			Vector4Formatter.FloatSerializer.WriteValue(value.z, writer);
			Vector4Formatter.FloatSerializer.WriteValue(value.w, writer);
		}

		// Token: 0x040001B5 RID: 437
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();
	}
}
