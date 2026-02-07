using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000A5 RID: 165
	public class LayerMaskFormatter : MinimalBaseFormatter<LayerMask>
	{
		// Token: 0x060004B9 RID: 1209 RVA: 0x00020F6F File Offset: 0x0001F16F
		protected override void Read(ref LayerMask value, IDataReader reader)
		{
			value.value = LayerMaskFormatter.IntSerializer.ReadValue(reader);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00020F82 File Offset: 0x0001F182
		protected override void Write(ref LayerMask value, IDataWriter writer)
		{
			LayerMaskFormatter.IntSerializer.WriteValue(value.value, writer);
		}

		// Token: 0x040001B0 RID: 432
		private static readonly Serializer<int> IntSerializer = Serializer.Get<int>();
	}
}
