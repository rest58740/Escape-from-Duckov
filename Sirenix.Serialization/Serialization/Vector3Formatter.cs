using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000AB RID: 171
	public class Vector3Formatter : MinimalBaseFormatter<Vector3>
	{
		// Token: 0x060004CD RID: 1229 RVA: 0x00021195 File Offset: 0x0001F395
		protected override void Read(ref Vector3 value, IDataReader reader)
		{
			value.x = Vector3Formatter.FloatSerializer.ReadValue(reader);
			value.y = Vector3Formatter.FloatSerializer.ReadValue(reader);
			value.z = Vector3Formatter.FloatSerializer.ReadValue(reader);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x000211CA File Offset: 0x0001F3CA
		protected override void Write(ref Vector3 value, IDataWriter writer)
		{
			Vector3Formatter.FloatSerializer.WriteValue(value.x, writer);
			Vector3Formatter.FloatSerializer.WriteValue(value.y, writer);
			Vector3Formatter.FloatSerializer.WriteValue(value.z, writer);
		}

		// Token: 0x040001B4 RID: 436
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();
	}
}
