using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x0200009B RID: 155
	public class Color32Formatter : MinimalBaseFormatter<Color32>
	{
		// Token: 0x06000490 RID: 1168 RVA: 0x000204B0 File Offset: 0x0001E6B0
		protected override void Read(ref Color32 value, IDataReader reader)
		{
			value.r = Color32Formatter.ByteSerializer.ReadValue(reader);
			value.g = Color32Formatter.ByteSerializer.ReadValue(reader);
			value.b = Color32Formatter.ByteSerializer.ReadValue(reader);
			value.a = Color32Formatter.ByteSerializer.ReadValue(reader);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00020504 File Offset: 0x0001E704
		protected override void Write(ref Color32 value, IDataWriter writer)
		{
			Color32Formatter.ByteSerializer.WriteValue(value.r, writer);
			Color32Formatter.ByteSerializer.WriteValue(value.g, writer);
			Color32Formatter.ByteSerializer.WriteValue(value.b, writer);
			Color32Formatter.ByteSerializer.WriteValue(value.a, writer);
		}

		// Token: 0x04000193 RID: 403
		private static readonly Serializer<byte> ByteSerializer = Serializer.Get<byte>();
	}
}
