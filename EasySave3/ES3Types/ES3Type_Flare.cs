using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000086 RID: 134
	[Preserve]
	[ES3Properties(new string[]
	{
		"hideFlags"
	})]
	public class ES3Type_Flare : ES3Type
	{
		// Token: 0x0600033A RID: 826 RVA: 0x00010356 File Offset: 0x0000E556
		public ES3Type_Flare() : base(typeof(Flare))
		{
			ES3Type_Flare.Instance = this;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00010370 File Offset: 0x0000E570
		public override void Write(object obj, ES3Writer writer)
		{
			Flare flare = (Flare)obj;
			writer.WriteProperty("hideFlags", flare.hideFlags);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0001039C File Offset: 0x0000E59C
		public override object Read<T>(ES3Reader reader)
		{
			Flare flare = new Flare();
			this.ReadInto<T>(reader, flare);
			return flare;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000103B8 File Offset: 0x0000E5B8
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			Flare flare = (Flare)obj;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (a == "hideFlags")
				{
					flare.hideFlags = reader.Read<HideFlags>();
				}
				else
				{
					reader.Skip();
				}
			}
		}

		// Token: 0x040000C6 RID: 198
		public static ES3Type Instance;
	}
}
