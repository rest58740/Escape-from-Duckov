using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000A1 RID: 161
	public class GradientAlphaKeyFormatter : MinimalBaseFormatter<GradientAlphaKey>
	{
		// Token: 0x060004A8 RID: 1192 RVA: 0x00020B59 File Offset: 0x0001ED59
		protected override void Read(ref GradientAlphaKey value, IDataReader reader)
		{
			value.alpha = GradientAlphaKeyFormatter.FloatSerializer.ReadValue(reader);
			value.time = GradientAlphaKeyFormatter.FloatSerializer.ReadValue(reader);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00020B7D File Offset: 0x0001ED7D
		protected override void Write(ref GradientAlphaKey value, IDataWriter writer)
		{
			GradientAlphaKeyFormatter.FloatSerializer.WriteValue(value.alpha, writer);
			GradientAlphaKeyFormatter.FloatSerializer.WriteValue(value.time, writer);
		}

		// Token: 0x040001A5 RID: 421
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();
	}
}
