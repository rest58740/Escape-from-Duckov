using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000C2 RID: 194
	public class RectOffset_DirectConverter : fsDirectConverter<RectOffset>
	{
		// Token: 0x0600070A RID: 1802 RVA: 0x000163C8 File Offset: 0x000145C8
		protected override fsResult DoSerialize(RectOffset model, Dictionary<string, fsData> serialized)
		{
			return fsResult.Success + base.SerializeMember<int>(serialized, null, "bottom", model.bottom) + base.SerializeMember<int>(serialized, null, "left", model.left) + base.SerializeMember<int>(serialized, null, "right", model.right) + base.SerializeMember<int>(serialized, null, "top", model.top);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001643C File Offset: 0x0001463C
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref RectOffset model)
		{
			fsResult success = fsResult.Success;
			int bottom = model.bottom;
			fsResult a = success + base.DeserializeMember<int>(data, null, "bottom", out bottom);
			model.bottom = bottom;
			int left = model.left;
			fsResult a2 = a + base.DeserializeMember<int>(data, null, "left", out left);
			model.left = left;
			int right = model.right;
			fsResult a3 = a2 + base.DeserializeMember<int>(data, null, "right", out right);
			model.right = right;
			int top = model.top;
			fsResult result = a3 + base.DeserializeMember<int>(data, null, "top", out top);
			model.top = top;
			return result;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x000164DE File Offset: 0x000146DE
		public override object CreateInstance(fsData data, Type storageType)
		{
			return new RectOffset();
		}
	}
}
