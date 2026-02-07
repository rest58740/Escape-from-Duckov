using System;
using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000808 RID: 2056
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	[CLSCompliant(false)]
	public sealed class TupleElementNamesAttribute : Attribute
	{
		// Token: 0x06004624 RID: 17956 RVA: 0x000E5942 File Offset: 0x000E3B42
		public TupleElementNamesAttribute(string[] transformNames)
		{
			if (transformNames == null)
			{
				throw new ArgumentNullException("transformNames");
			}
			this._transformNames = transformNames;
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x000E595F File Offset: 0x000E3B5F
		public IList<string> TransformNames
		{
			get
			{
				return this._transformNames;
			}
		}

		// Token: 0x04002D42 RID: 11586
		private readonly string[] _transformNames;
	}
}
