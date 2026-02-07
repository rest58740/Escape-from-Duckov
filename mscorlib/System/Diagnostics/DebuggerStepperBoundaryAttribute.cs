using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020009B5 RID: 2485
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DebuggerStepperBoundaryAttribute : Attribute
	{
	}
}
