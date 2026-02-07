using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000077 RID: 119
	[Preserve]
	[ES3Properties(new string[]
	{
		"boneIndex0",
		"boneIndex1",
		"boneIndex2",
		"boneIndex3",
		"weight0",
		"weight1",
		"weight2",
		"weight3"
	})]
	public class ES3Type_BoneWeight : ES3Type
	{
		// Token: 0x06000311 RID: 785 RVA: 0x0000F0F9 File Offset: 0x0000D2F9
		public ES3Type_BoneWeight() : base(typeof(BoneWeight))
		{
			ES3Type_BoneWeight.Instance = this;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000F114 File Offset: 0x0000D314
		public override void Write(object obj, ES3Writer writer)
		{
			BoneWeight boneWeight = (BoneWeight)obj;
			writer.WriteProperty("boneIndex0", boneWeight.boneIndex0, ES3Type_int.Instance);
			writer.WriteProperty("boneIndex1", boneWeight.boneIndex1, ES3Type_int.Instance);
			writer.WriteProperty("boneIndex2", boneWeight.boneIndex2, ES3Type_int.Instance);
			writer.WriteProperty("boneIndex3", boneWeight.boneIndex3, ES3Type_int.Instance);
			writer.WriteProperty("weight0", boneWeight.weight0, ES3Type_float.Instance);
			writer.WriteProperty("weight1", boneWeight.weight1, ES3Type_float.Instance);
			writer.WriteProperty("weight2", boneWeight.weight2, ES3Type_float.Instance);
			writer.WriteProperty("weight3", boneWeight.weight3, ES3Type_float.Instance);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000F208 File Offset: 0x0000D408
		public override object Read<T>(ES3Reader reader)
		{
			return new BoneWeight
			{
				boneIndex0 = reader.ReadProperty<int>(ES3Type_int.Instance),
				boneIndex1 = reader.ReadProperty<int>(ES3Type_int.Instance),
				boneIndex2 = reader.ReadProperty<int>(ES3Type_int.Instance),
				boneIndex3 = reader.ReadProperty<int>(ES3Type_int.Instance),
				weight0 = reader.ReadProperty<float>(ES3Type_float.Instance),
				weight1 = reader.ReadProperty<float>(ES3Type_float.Instance),
				weight2 = reader.ReadProperty<float>(ES3Type_float.Instance),
				weight3 = reader.ReadProperty<float>(ES3Type_float.Instance)
			};
		}

		// Token: 0x040000B7 RID: 183
		public static ES3Type Instance;
	}
}
