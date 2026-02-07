using System;
using System.Collections.Generic;

namespace ES3Types
{
	// Token: 0x02000040 RID: 64
	public class ES3Type_ES3RefDictionary : ES3DictionaryType
	{
		// Token: 0x0600028C RID: 652 RVA: 0x00009ED1 File Offset: 0x000080D1
		public ES3Type_ES3RefDictionary() : base(typeof(Dictionary<ES3Ref, ES3Ref>), ES3Type_ES3Ref.Instance, ES3Type_ES3Ref.Instance)
		{
			ES3Type_ES3RefDictionary.Instance = this;
		}

		// Token: 0x04000085 RID: 133
		public static ES3Type Instance = new ES3Type_ES3RefDictionary();
	}
}
