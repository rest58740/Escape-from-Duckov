using System;

namespace System.Reflection
{
	// Token: 0x020008B7 RID: 2231
	public readonly struct ParameterModifier
	{
		// Token: 0x060049DB RID: 18907 RVA: 0x000EF3DF File Offset: 0x000ED5DF
		public ParameterModifier(int parameterCount)
		{
			if (parameterCount <= 0)
			{
				throw new ArgumentException("Must specify one or more parameters.");
			}
			this._byRef = new bool[parameterCount];
		}

		// Token: 0x17000B9A RID: 2970
		public bool this[int index]
		{
			get
			{
				return this._byRef[index];
			}
			set
			{
				this._byRef[index] = value;
			}
		}

		// Token: 0x04002F0C RID: 12044
		private readonly bool[] _byRef;
	}
}
