using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x020005D9 RID: 1497
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class SoapFieldAttribute : SoapAttribute
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x060038FD RID: 14589 RVA: 0x000CAFDE File Offset: 0x000C91DE
		// (set) Token: 0x060038FE RID: 14590 RVA: 0x000CAFE6 File Offset: 0x000C91E6
		public int Order
		{
			get
			{
				return this._order;
			}
			set
			{
				this._order = value;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x060038FF RID: 14591 RVA: 0x000CAFEF File Offset: 0x000C91EF
		// (set) Token: 0x06003900 RID: 14592 RVA: 0x000CAFF7 File Offset: 0x000C91F7
		public string XmlElementName
		{
			get
			{
				return this._elementName;
			}
			set
			{
				this._isElement = (value != null);
				this._elementName = value;
			}
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x000CB00A File Offset: 0x000C920A
		public bool IsInteropXmlElement()
		{
			return this._isElement;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x000CB014 File Offset: 0x000C9214
		internal override void SetReflectionObject(object reflectionObject)
		{
			FieldInfo fieldInfo = (FieldInfo)reflectionObject;
			if (this._elementName == null)
			{
				this._elementName = fieldInfo.Name;
			}
		}

		// Token: 0x04002602 RID: 9730
		private int _order;

		// Token: 0x04002603 RID: 9731
		private string _elementName;

		// Token: 0x04002604 RID: 9732
		private bool _isElement;
	}
}
