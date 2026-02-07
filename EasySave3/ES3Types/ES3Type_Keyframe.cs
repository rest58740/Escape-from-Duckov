using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000095 RID: 149
	[Preserve]
	[ES3Properties(new string[]
	{
		"time",
		"value",
		"inTangent",
		"outTangent"
	})]
	public class ES3Type_Keyframe : ES3Type
	{
		// Token: 0x06000367 RID: 871 RVA: 0x000114BD File Offset: 0x0000F6BD
		public ES3Type_Keyframe() : base(typeof(Keyframe))
		{
			ES3Type_Keyframe.Instance = this;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000114D8 File Offset: 0x0000F6D8
		public override void Write(object obj, ES3Writer writer)
		{
			Keyframe keyframe = (Keyframe)obj;
			writer.WriteProperty("time", keyframe.time, ES3Type_float.Instance);
			writer.WriteProperty("value", keyframe.value, ES3Type_float.Instance);
			writer.WriteProperty("inTangent", keyframe.inTangent, ES3Type_float.Instance);
			writer.WriteProperty("outTangent", keyframe.outTangent, ES3Type_float.Instance);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0001155C File Offset: 0x0000F75C
		public override object Read<T>(ES3Reader reader)
		{
			return new Keyframe(reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance), reader.ReadProperty<float>(ES3Type_float.Instance));
		}

		// Token: 0x040000D8 RID: 216
		public static ES3Type Instance;
	}
}
