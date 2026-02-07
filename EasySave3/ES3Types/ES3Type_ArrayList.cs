using System;
using System.Collections;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000026 RID: 38
	[Preserve]
	[ES3Properties(new string[]
	{
		"_items",
		"_size",
		"_version"
	})]
	public class ES3Type_ArrayList : ES3ObjectType
	{
		// Token: 0x06000231 RID: 561 RVA: 0x000087D4 File Offset: 0x000069D4
		public ES3Type_ArrayList() : base(typeof(ArrayList))
		{
			ES3Type_ArrayList.Instance = this;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000087EC File Offset: 0x000069EC
		protected override void WriteObject(object obj, ES3Writer writer)
		{
			ArrayList objectContainingField = (ArrayList)obj;
			writer.WritePrivateField("_items", objectContainingField);
			writer.WritePrivateField("_size", objectContainingField);
			writer.WritePrivateField("_version", objectContainingField);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00008824 File Offset: 0x00006A24
		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			ArrayList objectContainingField = (ArrayList)obj;
			foreach (object obj2 in reader.Properties)
			{
				string a = (string)obj2;
				if (!(a == "_items"))
				{
					if (!(a == "_size"))
					{
						if (!(a == "_version"))
						{
							reader.Skip();
						}
						else
						{
							objectContainingField = (ArrayList)reader.SetPrivateField("_version", reader.Read<int>(), objectContainingField);
						}
					}
					else
					{
						objectContainingField = (ArrayList)reader.SetPrivateField("_size", reader.Read<int>(), objectContainingField);
					}
				}
				else
				{
					objectContainingField = (ArrayList)reader.SetPrivateField("_items", reader.Read<object[]>(), objectContainingField);
				}
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000890C File Offset: 0x00006B0C
		protected override object ReadObject<T>(ES3Reader reader)
		{
			ArrayList arrayList = new ArrayList();
			this.ReadObject<T>(reader, arrayList);
			return arrayList;
		}

		// Token: 0x04000061 RID: 97
		public static ES3Type Instance;
	}
}
