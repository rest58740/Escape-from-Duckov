using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000C6 RID: 198
	public class Vector3Int_DirectConverter : fsDirectConverter<Vector3Int>
	{
		// Token: 0x0600071A RID: 1818 RVA: 0x00016770 File Offset: 0x00014970
		protected override fsResult DoSerialize(Vector3Int model, Dictionary<string, fsData> serialized)
		{
			base.SerializeMember<int>(serialized, null, "x", model.x);
			base.SerializeMember<int>(serialized, null, "y", model.y);
			base.SerializeMember<int>(serialized, null, "z", model.z);
			return fsResult.Success;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000167C4 File Offset: 0x000149C4
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Vector3Int model)
		{
			int x = model.x;
			base.DeserializeMember<int>(data, null, "x", out x);
			model.x = x;
			int y = model.y;
			base.DeserializeMember<int>(data, null, "y", out y);
			model.y = y;
			int z = model.z;
			base.DeserializeMember<int>(data, null, "z", out z);
			model.z = z;
			return fsResult.Success;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00016830 File Offset: 0x00014A30
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Vector3Int);
		}
	}
}
