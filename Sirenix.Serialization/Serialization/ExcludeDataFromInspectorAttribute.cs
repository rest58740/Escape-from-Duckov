using System;
using System.ComponentModel;

namespace Sirenix.Serialization
{
	// Token: 0x02000063 RID: 99
	[AttributeUsage(384, AllowMultiple = false, Inherited = true)]
	[Obsolete("Use [HideInInspector] instead - it now also excludes the member completely from becoming a property in the property tree.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class ExcludeDataFromInspectorAttribute : Attribute
	{
	}
}
