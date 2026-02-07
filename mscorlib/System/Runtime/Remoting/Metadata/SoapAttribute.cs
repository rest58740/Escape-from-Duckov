using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x020005D8 RID: 1496
	[ComVisible(true)]
	public class SoapAttribute : Attribute
	{
		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060038F5 RID: 14581 RVA: 0x000CAF9A File Offset: 0x000C919A
		// (set) Token: 0x060038F6 RID: 14582 RVA: 0x000CAFA2 File Offset: 0x000C91A2
		public virtual bool Embedded
		{
			get
			{
				return this._nested;
			}
			set
			{
				this._nested = value;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060038F7 RID: 14583 RVA: 0x000CAFAB File Offset: 0x000C91AB
		// (set) Token: 0x060038F8 RID: 14584 RVA: 0x000CAFB3 File Offset: 0x000C91B3
		public virtual bool UseAttribute
		{
			get
			{
				return this._useAttribute;
			}
			set
			{
				this._useAttribute = value;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000CAFBC File Offset: 0x000C91BC
		// (set) Token: 0x060038FA RID: 14586 RVA: 0x000CAFC4 File Offset: 0x000C91C4
		public virtual string XmlNamespace
		{
			get
			{
				return this.ProtXmlNamespace;
			}
			set
			{
				this.ProtXmlNamespace = value;
			}
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x000CAFCD File Offset: 0x000C91CD
		internal virtual void SetReflectionObject(object reflectionObject)
		{
			this.ReflectInfo = reflectionObject;
		}

		// Token: 0x040025FE RID: 9726
		private bool _nested;

		// Token: 0x040025FF RID: 9727
		private bool _useAttribute;

		// Token: 0x04002600 RID: 9728
		protected string ProtXmlNamespace;

		// Token: 0x04002601 RID: 9729
		protected object ReflectInfo;
	}
}
