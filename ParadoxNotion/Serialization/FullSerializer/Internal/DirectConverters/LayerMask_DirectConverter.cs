using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000C1 RID: 193
	public class LayerMask_DirectConverter : fsDirectConverter<LayerMask>
	{
		// Token: 0x06000706 RID: 1798 RVA: 0x0001634F File Offset: 0x0001454F
		protected override fsResult DoSerialize(LayerMask model, Dictionary<string, fsData> serialized)
		{
			return fsResult.Success + base.SerializeMember<int>(serialized, null, "value", model.value);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00016370 File Offset: 0x00014570
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref LayerMask model)
		{
			fsResult success = fsResult.Success;
			int value = model.value;
			fsResult result = success + base.DeserializeMember<int>(data, null, "value", out value);
			model.value = value;
			return result;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000163A4 File Offset: 0x000145A4
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(LayerMask);
		}
	}
}
