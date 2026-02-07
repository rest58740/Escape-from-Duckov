using System;
using System.Diagnostics;

namespace Animancer
{
	// Token: 0x0200001D RID: 29
	[AttributeUsage(AttributeTargets.Field)]
	[Conditional("UNITY_EDITOR")]
	public sealed class DrawAfterEventsAttribute : Attribute
	{
	}
}
