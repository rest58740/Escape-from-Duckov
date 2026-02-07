using System;
using System.Collections.Generic;

namespace Sirenix.OdinInspector
{
	// Token: 0x02000090 RID: 144
	public class SelfMetaData : List<SelfValidationResult.ResultItemMetaData>
	{
		// Token: 0x060001CA RID: 458 RVA: 0x00004568 File Offset: 0x00002768
		public void Add(string key, object value)
		{
			base.Add(new SelfValidationResult.ResultItemMetaData(key, value, Array.Empty<Attribute>()));
		}
	}
}
