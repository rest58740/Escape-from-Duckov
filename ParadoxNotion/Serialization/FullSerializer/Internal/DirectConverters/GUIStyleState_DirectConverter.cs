using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000BE RID: 190
	public class GUIStyleState_DirectConverter : fsDirectConverter<GUIStyleState>
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x00015ABD File Offset: 0x00013CBD
		protected override fsResult DoSerialize(GUIStyleState model, Dictionary<string, fsData> serialized)
		{
			return fsResult.Success + base.SerializeMember<Texture2D>(serialized, null, "background", model.background) + base.SerializeMember<Color>(serialized, null, "textColor", model.textColor);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00015AF4 File Offset: 0x00013CF4
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref GUIStyleState model)
		{
			fsResult success = fsResult.Success;
			Texture2D background = model.background;
			fsResult a = success + base.DeserializeMember<Texture2D>(data, null, "background", out background);
			model.background = background;
			Color textColor = model.textColor;
			fsResult result = a + base.DeserializeMember<Color>(data, null, "textColor", out textColor);
			model.textColor = textColor;
			return result;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00015B4E File Offset: 0x00013D4E
		public override object CreateInstance(fsData data, Type storageType)
		{
			return new GUIStyleState();
		}
	}
}
