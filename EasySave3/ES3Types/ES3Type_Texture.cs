using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000BB RID: 187
	[Preserve]
	[ES3Properties(new string[]
	{
		"filterMode",
		"anisoLevel",
		"wrapMode",
		"mipMapBias",
		"rawTextureData"
	})]
	public class ES3Type_Texture : ES3Type
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x00018FDD File Offset: 0x000171DD
		public ES3Type_Texture() : base(typeof(Texture))
		{
			ES3Type_Texture.Instance = this;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00018FF8 File Offset: 0x000171F8
		public override void Write(object obj, ES3Writer writer)
		{
			if (obj.GetType() == typeof(Texture2D))
			{
				ES3Type_Texture2D.Instance.Write(obj, writer);
				return;
			}
			string str = "Textures of type ";
			Type type = obj.GetType();
			throw new NotSupportedException(str + ((type != null) ? type.ToString() : null) + " are not currently supported.");
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00019050 File Offset: 0x00017250
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			if (obj.GetType() == typeof(Texture2D))
			{
				ES3Type_Texture2D.Instance.ReadInto<T>(reader, obj);
				return;
			}
			string str = "Textures of type ";
			Type type = obj.GetType();
			throw new NotSupportedException(str + ((type != null) ? type.ToString() : null) + " are not currently supported.");
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000190A7 File Offset: 0x000172A7
		public override object Read<T>(ES3Reader reader)
		{
			return ES3Type_Texture2D.Instance.Read<T>(reader);
		}

		// Token: 0x040000FE RID: 254
		public static ES3Type Instance;
	}
}
