using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000088 RID: 136
	[Preserve]
	[ES3Properties(new string[]
	{
		"material",
		"name"
	})]
	public class ES3Type_Font : ES3UnityObjectType
	{
		// Token: 0x0600033F RID: 831 RVA: 0x00010416 File Offset: 0x0000E616
		public ES3Type_Font() : base(typeof(Font))
		{
			ES3Type_Font.Instance = this;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00010430 File Offset: 0x0000E630
		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
			Font font = (Font)obj;
			writer.WriteProperty("name", font.name, ES3Type_string.Instance);
			writer.WriteProperty("material", font.material);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0001046C File Offset: 0x0000E66C
		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
			Font font = (Font)obj;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (a == "material")
				{
					font.material = reader.Read<Material>(ES3Type_Material.Instance);
				}
				else
				{
					reader.Skip();
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x000104B4 File Offset: 0x0000E6B4
		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			Font font = new Font(reader.ReadProperty<string>(ES3Type_string.Instance));
			this.ReadObject<T>(reader, font);
			return font;
		}

		// Token: 0x040000C8 RID: 200
		public static ES3Type Instance;
	}
}
