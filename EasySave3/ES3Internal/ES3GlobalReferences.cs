using System;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000DB RID: 219
	public class ES3GlobalReferences : ScriptableObject
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x0001DA66 File Offset: 0x0001BC66
		public static ES3GlobalReferences Instance
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001DA69 File Offset: 0x0001BC69
		public UnityEngine.Object Get(long id)
		{
			return null;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001DA6C File Offset: 0x0001BC6C
		public long GetOrAdd(UnityEngine.Object obj)
		{
			return -1L;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001DA70 File Offset: 0x0001BC70
		public void RemoveInvalidKeys()
		{
		}
	}
}
