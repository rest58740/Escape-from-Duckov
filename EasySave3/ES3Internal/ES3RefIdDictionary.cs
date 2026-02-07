using System;
using System.ComponentModel;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000D7 RID: 215
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Serializable]
	public class ES3RefIdDictionary : ES3SerializableDictionary<UnityEngine.Object, long>
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x0001BD71 File Offset: 0x00019F71
		protected override bool KeysAreEqual(UnityEngine.Object a, UnityEngine.Object b)
		{
			return a == b;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001BD7A File Offset: 0x00019F7A
		protected override bool ValuesAreEqual(long a, long b)
		{
			return a == b;
		}
	}
}
