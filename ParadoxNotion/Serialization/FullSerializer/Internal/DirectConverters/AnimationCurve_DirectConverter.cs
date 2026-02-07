using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000BB RID: 187
	public class AnimationCurve_DirectConverter : fsDirectConverter<AnimationCurve>
	{
		// Token: 0x060006EE RID: 1774 RVA: 0x0001587C File Offset: 0x00013A7C
		protected override fsResult DoSerialize(AnimationCurve model, Dictionary<string, fsData> serialized)
		{
			return fsResult.Success + base.SerializeMember<Keyframe[]>(serialized, null, "keys", model.keys) + base.SerializeMember<WrapMode>(serialized, null, "preWrapMode", model.preWrapMode) + base.SerializeMember<WrapMode>(serialized, null, "postWrapMode", model.postWrapMode);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000158D8 File Offset: 0x00013AD8
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref AnimationCurve model)
		{
			fsResult success = fsResult.Success;
			Keyframe[] keys = model.keys;
			fsResult a = success + base.DeserializeMember<Keyframe[]>(data, null, "keys", out keys);
			model.keys = keys;
			WrapMode preWrapMode = model.preWrapMode;
			fsResult a2 = a + base.DeserializeMember<WrapMode>(data, null, "preWrapMode", out preWrapMode);
			model.preWrapMode = preWrapMode;
			WrapMode postWrapMode = model.postWrapMode;
			fsResult result = a2 + base.DeserializeMember<WrapMode>(data, null, "postWrapMode", out postWrapMode);
			model.postWrapMode = postWrapMode;
			return result;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00015956 File Offset: 0x00013B56
		public override object CreateInstance(fsData data, Type storageType)
		{
			return new AnimationCurve();
		}
	}
}
