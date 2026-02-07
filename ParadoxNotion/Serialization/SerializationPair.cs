using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization
{
	// Token: 0x0200008C RID: 140
	[Serializable]
	public sealed class SerializationPair
	{
		// Token: 0x060005AC RID: 1452 RVA: 0x0001072F File Offset: 0x0000E92F
		public SerializationPair()
		{
			this._references = new List<Object>();
		}

		// Token: 0x040001C0 RID: 448
		public string _json;

		// Token: 0x040001C1 RID: 449
		public List<Object> _references;
	}
}
