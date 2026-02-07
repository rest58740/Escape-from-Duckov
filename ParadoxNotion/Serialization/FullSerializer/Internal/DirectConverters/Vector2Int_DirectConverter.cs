using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000C4 RID: 196
	public class Vector2Int_DirectConverter : fsDirectConverter<Vector2Int>
	{
		// Token: 0x06000712 RID: 1810 RVA: 0x00016627 File Offset: 0x00014827
		protected override fsResult DoSerialize(Vector2Int model, Dictionary<string, fsData> serialized)
		{
			base.SerializeMember<int>(serialized, null, "x", model.x);
			base.SerializeMember<int>(serialized, null, "y", model.y);
			return fsResult.Success;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00016658 File Offset: 0x00014858
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Vector2Int model)
		{
			int x = model.x;
			base.DeserializeMember<int>(data, null, "x", out x);
			model.x = x;
			int y = model.y;
			base.DeserializeMember<int>(data, null, "y", out y);
			model.y = y;
			return fsResult.Success;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000166A8 File Offset: 0x000148A8
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Vector2Int);
		}
	}
}
