using System;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000D6 RID: 214
	[Serializable]
	public class ES3IdRefDictionary : ES3SerializableDictionary<long, UnityEngine.Object>
	{
		// Token: 0x06000454 RID: 1108 RVA: 0x0001BD5A File Offset: 0x00019F5A
		protected override bool KeysAreEqual(long a, long b)
		{
			return a == b;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001BD60 File Offset: 0x00019F60
		protected override bool ValuesAreEqual(UnityEngine.Object a, UnityEngine.Object b)
		{
			return a == b;
		}
	}
}
