using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020009B7 RID: 2487
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
	[Serializable]
	public sealed class DebuggerNonUserCodeAttribute : Attribute
	{
	}
}
