using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x0200009A RID: 154
	public class BoundsFormatter : MinimalBaseFormatter<Bounds>
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x00020453 File Offset: 0x0001E653
		protected override void Read(ref Bounds value, IDataReader reader)
		{
			value.center = BoundsFormatter.Vector3Serializer.ReadValue(reader);
			value.size = BoundsFormatter.Vector3Serializer.ReadValue(reader);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00020477 File Offset: 0x0001E677
		protected override void Write(ref Bounds value, IDataWriter writer)
		{
			BoundsFormatter.Vector3Serializer.WriteValue(value.center, writer);
			BoundsFormatter.Vector3Serializer.WriteValue(value.size, writer);
		}

		// Token: 0x04000192 RID: 402
		private static readonly Serializer<Vector3> Vector3Serializer = Serializer.Get<Vector3>();
	}
}
