using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000C0 RID: 192
	public class Keyframe_DirectConverter : fsDirectConverter<Keyframe>
	{
		// Token: 0x06000702 RID: 1794 RVA: 0x000161DC File Offset: 0x000143DC
		protected override fsResult DoSerialize(Keyframe model, Dictionary<string, fsData> serialized)
		{
			return fsResult.Success + base.SerializeMember<float>(serialized, null, "time", model.time) + base.SerializeMember<float>(serialized, null, "value", model.value) + base.SerializeMember<int>(serialized, null, "tangentMode", model.tangentMode) + base.SerializeMember<float>(serialized, null, "inTangent", model.inTangent) + base.SerializeMember<float>(serialized, null, "outTangent", model.outTangent);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001626C File Offset: 0x0001446C
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Keyframe model)
		{
			fsResult success = fsResult.Success;
			float time = model.time;
			fsResult a = success + base.DeserializeMember<float>(data, null, "time", out time);
			model.time = time;
			float value = model.value;
			fsResult a2 = a + base.DeserializeMember<float>(data, null, "value", out value);
			model.value = value;
			int tangentMode = model.tangentMode;
			fsResult a3 = a2 + base.DeserializeMember<int>(data, null, "tangentMode", out tangentMode);
			model.tangentMode = tangentMode;
			float inTangent = model.inTangent;
			fsResult a4 = a3 + base.DeserializeMember<float>(data, null, "inTangent", out inTangent);
			model.inTangent = inTangent;
			float outTangent = model.outTangent;
			fsResult result = a4 + base.DeserializeMember<float>(data, null, "outTangent", out outTangent);
			model.outTangent = outTangent;
			return result;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001632C File Offset: 0x0001452C
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Keyframe);
		}
	}
}
