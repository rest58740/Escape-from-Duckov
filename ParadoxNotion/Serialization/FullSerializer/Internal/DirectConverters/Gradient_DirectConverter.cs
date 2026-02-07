using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000BD RID: 189
	public class Gradient_DirectConverter : fsDirectConverter<Gradient>
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x00015A1B File Offset: 0x00013C1B
		protected override fsResult DoSerialize(Gradient model, Dictionary<string, fsData> serialized)
		{
			return fsResult.Success + base.SerializeMember<GradientAlphaKey[]>(serialized, null, "alphaKeys", model.alphaKeys) + base.SerializeMember<GradientColorKey[]>(serialized, null, "colorKeys", model.colorKeys);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00015A54 File Offset: 0x00013C54
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Gradient model)
		{
			fsResult success = fsResult.Success;
			GradientAlphaKey[] alphaKeys = model.alphaKeys;
			fsResult a = success + base.DeserializeMember<GradientAlphaKey[]>(data, null, "alphaKeys", out alphaKeys);
			model.alphaKeys = alphaKeys;
			GradientColorKey[] colorKeys = model.colorKeys;
			fsResult result = a + base.DeserializeMember<GradientColorKey[]>(data, null, "colorKeys", out colorKeys);
			model.colorKeys = colorKeys;
			return result;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00015AAE File Offset: 0x00013CAE
		public override object CreateInstance(fsData data, Type storageType)
		{
			return new Gradient();
		}
	}
}
