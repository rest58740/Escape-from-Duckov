using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007F9 RID: 2041
	[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	[Serializable]
	public sealed class IndexerNameAttribute : Attribute
	{
		// Token: 0x0600460C RID: 17932 RVA: 0x00002050 File Offset: 0x00000250
		public IndexerNameAttribute(string indexerName)
		{
		}
	}
}
