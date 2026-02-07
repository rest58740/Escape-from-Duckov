using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000AE RID: 174
	public class Vector3IntFormatter : MinimalBaseFormatter<Vector3Int>
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x00021329 File Offset: 0x0001F529
		protected override void Read(ref Vector3Int value, IDataReader reader)
		{
			value.x = Vector3IntFormatter.Serializer.ReadValue(reader);
			value.y = Vector3IntFormatter.Serializer.ReadValue(reader);
			value.z = Vector3IntFormatter.Serializer.ReadValue(reader);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0002135E File Offset: 0x0001F55E
		protected override void Write(ref Vector3Int value, IDataWriter writer)
		{
			Vector3IntFormatter.Serializer.WriteValue(value.x, writer);
			Vector3IntFormatter.Serializer.WriteValue(value.y, writer);
			Vector3IntFormatter.Serializer.WriteValue(value.z, writer);
		}

		// Token: 0x040001B7 RID: 439
		private static readonly Serializer<int> Serializer = Sirenix.Serialization.Serializer.Get<int>();
	}
}
