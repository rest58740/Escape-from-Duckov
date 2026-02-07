using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000097 RID: 151
	[Preserve]
	[ES3Properties(new string[]
	{
		"colorKeys",
		"alphaKeys",
		"mode"
	})]
	public class ES3Type_LayerMask : ES3Type
	{
		// Token: 0x0600036B RID: 875 RVA: 0x000115B1 File Offset: 0x0000F7B1
		public ES3Type_LayerMask() : base(typeof(LayerMask))
		{
			ES3Type_LayerMask.Instance = this;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x000115CC File Offset: 0x0000F7CC
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WriteProperty("value", ((LayerMask)obj).value, ES3Type_int.Instance);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x000115FC File Offset: 0x0000F7FC
		public override object Read<T>(ES3Reader reader)
		{
			LayerMask layerMask = default(LayerMask);
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (a == "value")
				{
					layerMask = reader.Read<int>(ES3Type_int.Instance);
				}
				else
				{
					reader.Skip();
				}
			}
			return layerMask;
		}

		// Token: 0x040000DA RID: 218
		public static ES3Type Instance;
	}
}
