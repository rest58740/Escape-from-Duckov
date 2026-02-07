using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x0200009F RID: 159
	public class ColorFormatter : MinimalBaseFormatter<Color>
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x00020A94 File Offset: 0x0001EC94
		protected override void Read(ref Color value, IDataReader reader)
		{
			value.r = ColorFormatter.FloatSerializer.ReadValue(reader);
			value.g = ColorFormatter.FloatSerializer.ReadValue(reader);
			value.b = ColorFormatter.FloatSerializer.ReadValue(reader);
			value.a = ColorFormatter.FloatSerializer.ReadValue(reader);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00020AE8 File Offset: 0x0001ECE8
		protected override void Write(ref Color value, IDataWriter writer)
		{
			ColorFormatter.FloatSerializer.WriteValue(value.r, writer);
			ColorFormatter.FloatSerializer.WriteValue(value.g, writer);
			ColorFormatter.FloatSerializer.WriteValue(value.b, writer);
			ColorFormatter.FloatSerializer.WriteValue(value.a, writer);
		}

		// Token: 0x040001A4 RID: 420
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();
	}
}
