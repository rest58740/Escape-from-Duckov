using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;
using Microsoft.Internal.Runtime.Serialization;

namespace System.ComponentModel.Composition.Primitives
{
	// Token: 0x0200008D RID: 141
	[DebuggerTypeProxy(typeof(ComposablePartExceptionDebuggerProxy))]
	[DebuggerDisplay("{Message}")]
	[Serializable]
	public class ComposablePartException : Exception
	{
		// Token: 0x060003BB RID: 955 RVA: 0x0000AD6B File Offset: 0x00008F6B
		public ComposablePartException() : this(null, null, null)
		{
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000AD76 File Offset: 0x00008F76
		public ComposablePartException(string message) : this(message, null, null)
		{
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000AD81 File Offset: 0x00008F81
		public ComposablePartException(string message, ICompositionElement element) : this(message, element, null)
		{
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000AD8C File Offset: 0x00008F8C
		public ComposablePartException(string message, Exception innerException) : this(message, null, innerException)
		{
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000AD97 File Offset: 0x00008F97
		public ComposablePartException(string message, ICompositionElement element, Exception innerException) : base(message, innerException)
		{
			this._element = element;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000ADA8 File Offset: 0x00008FA8
		[SecuritySafeCritical]
		protected ComposablePartException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._element = info.GetValue("Element");
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000ADC3 File Offset: 0x00008FC3
		public ICompositionElement Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000ADCB File Offset: 0x00008FCB
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Element", this._element.ToSerializableElement());
		}

		// Token: 0x04000177 RID: 375
		private readonly ICompositionElement _element;
	}
}
