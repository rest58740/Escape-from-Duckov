using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000A2 RID: 162
	public class GradientColorKeyFormatter : MinimalBaseFormatter<GradientColorKey>
	{
		// Token: 0x060004AC RID: 1196 RVA: 0x00020BB5 File Offset: 0x0001EDB5
		protected override void Read(ref GradientColorKey value, IDataReader reader)
		{
			value.color = GradientColorKeyFormatter.ColorSerializer.ReadValue(reader);
			value.time = GradientColorKeyFormatter.FloatSerializer.ReadValue(reader);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00020BD9 File Offset: 0x0001EDD9
		protected override void Write(ref GradientColorKey value, IDataWriter writer)
		{
			GradientColorKeyFormatter.ColorSerializer.WriteValue(value.color, writer);
			GradientColorKeyFormatter.FloatSerializer.WriteValue(value.time, writer);
		}

		// Token: 0x040001A6 RID: 422
		private static readonly Serializer<Color> ColorSerializer = Serializer.Get<Color>();

		// Token: 0x040001A7 RID: 423
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();
	}
}
