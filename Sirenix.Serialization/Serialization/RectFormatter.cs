using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000A7 RID: 167
	public class RectFormatter : MinimalBaseFormatter<Rect>
	{
		// Token: 0x060004C1 RID: 1217 RVA: 0x00021068 File Offset: 0x0001F268
		protected override void Read(ref Rect value, IDataReader reader)
		{
			value.x = RectFormatter.FloatSerializer.ReadValue(reader);
			value.y = RectFormatter.FloatSerializer.ReadValue(reader);
			value.width = RectFormatter.FloatSerializer.ReadValue(reader);
			value.height = RectFormatter.FloatSerializer.ReadValue(reader);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000210BC File Offset: 0x0001F2BC
		protected override void Write(ref Rect value, IDataWriter writer)
		{
			RectFormatter.FloatSerializer.WriteValue(value.x, writer);
			RectFormatter.FloatSerializer.WriteValue(value.y, writer);
			RectFormatter.FloatSerializer.WriteValue(value.width, writer);
			RectFormatter.FloatSerializer.WriteValue(value.height, writer);
		}

		// Token: 0x040001B2 RID: 434
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();
	}
}
