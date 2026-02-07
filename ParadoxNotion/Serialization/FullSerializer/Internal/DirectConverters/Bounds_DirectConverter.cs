using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000BC RID: 188
	public class Bounds_DirectConverter : fsDirectConverter<Bounds>
	{
		// Token: 0x060006F2 RID: 1778 RVA: 0x00015965 File Offset: 0x00013B65
		protected override fsResult DoSerialize(Bounds model, Dictionary<string, fsData> serialized)
		{
			return fsResult.Success + base.SerializeMember<Vector3>(serialized, null, "center", model.center) + base.SerializeMember<Vector3>(serialized, null, "size", model.size);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000159A0 File Offset: 0x00013BA0
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Bounds model)
		{
			fsResult success = fsResult.Success;
			Vector3 center = model.center;
			fsResult a = success + base.DeserializeMember<Vector3>(data, null, "center", out center);
			model.center = center;
			Vector3 size = model.size;
			fsResult result = a + base.DeserializeMember<Vector3>(data, null, "size", out size);
			model.size = size;
			return result;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000159F8 File Offset: 0x00013BF8
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Bounds);
		}
	}
}
