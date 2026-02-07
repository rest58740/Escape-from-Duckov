using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000C3 RID: 195
	public class Rect_DirectConverter : fsDirectConverter<Rect>
	{
		// Token: 0x0600070E RID: 1806 RVA: 0x000164F0 File Offset: 0x000146F0
		protected override fsResult DoSerialize(Rect model, Dictionary<string, fsData> serialized)
		{
			return fsResult.Success + base.SerializeMember<float>(serialized, null, "xMin", model.xMin) + base.SerializeMember<float>(serialized, null, "yMin", model.yMin) + base.SerializeMember<float>(serialized, null, "xMax", model.xMax) + base.SerializeMember<float>(serialized, null, "yMax", model.yMax);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00016568 File Offset: 0x00014768
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Rect model)
		{
			fsResult success = fsResult.Success;
			float xMin = model.xMin;
			fsResult a = success + base.DeserializeMember<float>(data, null, "xMin", out xMin);
			model.xMin = xMin;
			float yMin = model.yMin;
			fsResult a2 = a + base.DeserializeMember<float>(data, null, "yMin", out yMin);
			model.yMin = yMin;
			float xMax = model.xMax;
			fsResult a3 = a2 + base.DeserializeMember<float>(data, null, "xMax", out xMax);
			model.xMax = xMax;
			float yMax = model.yMax;
			fsResult result = a3 + base.DeserializeMember<float>(data, null, "yMax", out yMax);
			model.yMax = yMax;
			return result;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00016604 File Offset: 0x00014804
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Rect);
		}
	}
}
