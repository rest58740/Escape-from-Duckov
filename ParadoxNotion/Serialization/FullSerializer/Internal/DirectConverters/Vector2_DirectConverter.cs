using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000C5 RID: 197
	public class Vector2_DirectConverter : fsDirectConverter<Vector2>
	{
		// Token: 0x06000716 RID: 1814 RVA: 0x000166CB File Offset: 0x000148CB
		protected override fsResult DoSerialize(Vector2 model, Dictionary<string, fsData> serialized)
		{
			base.SerializeMember<float>(serialized, null, "x", model.x);
			base.SerializeMember<float>(serialized, null, "y", model.y);
			return fsResult.Success;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000166FC File Offset: 0x000148FC
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Vector2 model)
		{
			float x = model.x;
			base.DeserializeMember<float>(data, null, "x", out x);
			model.x = x;
			float y = model.y;
			base.DeserializeMember<float>(data, null, "y", out y);
			model.y = y;
			return fsResult.Success;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001674C File Offset: 0x0001494C
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Vector2);
		}
	}
}
