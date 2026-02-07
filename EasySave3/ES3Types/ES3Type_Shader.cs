using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000B0 RID: 176
	[Preserve]
	[ES3Properties(new string[]
	{
		"name",
		"maximumLOD"
	})]
	public class ES3Type_Shader : ES3Type
	{
		// Token: 0x060003B6 RID: 950 RVA: 0x00016C1B File Offset: 0x00014E1B
		public ES3Type_Shader() : base(typeof(Shader))
		{
			ES3Type_Shader.Instance = this;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00016C34 File Offset: 0x00014E34
		public override void Write(object obj, ES3Writer writer)
		{
			Shader shader = (Shader)obj;
			writer.WriteProperty("name", shader.name, ES3Type_string.Instance);
			writer.WriteProperty("maximumLOD", shader.maximumLOD, ES3Type_int.Instance);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00016C7C File Offset: 0x00014E7C
		public override object Read<T>(ES3Reader reader)
		{
			Shader shader = Shader.Find(reader.ReadProperty<string>(ES3Type_string.Instance));
			if (shader == null)
			{
				shader = Shader.Find("Diffuse");
			}
			this.ReadInto<T>(reader, shader);
			return shader;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00016CB8 File Offset: 0x00014EB8
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			Shader shader = (Shader)obj;
			foreach (object obj2 in reader.Properties)
			{
				string a = (string)obj2;
				if (!(a == "name"))
				{
					if (!(a == "maximumLOD"))
					{
						reader.Skip();
					}
					else
					{
						shader.maximumLOD = reader.Read<int>(ES3Type_int.Instance);
					}
				}
				else
				{
					shader.name = reader.Read<string>(ES3Type_string.Instance);
				}
			}
		}

		// Token: 0x040000F3 RID: 243
		public static ES3Type Instance;
	}
}
