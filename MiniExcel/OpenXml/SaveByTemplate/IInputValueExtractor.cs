using System;
using System.Collections.Generic;

namespace MiniExcelLibs.OpenXml.SaveByTemplate
{
	// Token: 0x02000051 RID: 81
	public interface IInputValueExtractor
	{
		// Token: 0x06000286 RID: 646
		IDictionary<string, object> ToValueDictionary(object valueObject);
	}
}
