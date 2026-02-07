using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000A6 RID: 166
	public class QuaternionFormatter : MinimalBaseFormatter<Quaternion>
	{
		// Token: 0x060004BD RID: 1213 RVA: 0x00020FAC File Offset: 0x0001F1AC
		protected override void Read(ref Quaternion value, IDataReader reader)
		{
			value.x = QuaternionFormatter.FloatSerializer.ReadValue(reader);
			value.y = QuaternionFormatter.FloatSerializer.ReadValue(reader);
			value.z = QuaternionFormatter.FloatSerializer.ReadValue(reader);
			value.w = QuaternionFormatter.FloatSerializer.ReadValue(reader);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00021000 File Offset: 0x0001F200
		protected override void Write(ref Quaternion value, IDataWriter writer)
		{
			QuaternionFormatter.FloatSerializer.WriteValue(value.x, writer);
			QuaternionFormatter.FloatSerializer.WriteValue(value.y, writer);
			QuaternionFormatter.FloatSerializer.WriteValue(value.z, writer);
			QuaternionFormatter.FloatSerializer.WriteValue(value.w, writer);
		}

		// Token: 0x040001B1 RID: 433
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();
	}
}
