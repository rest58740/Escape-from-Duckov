using System;

namespace System.Runtime.Versioning
{
	// Token: 0x0200063D RID: 1597
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public sealed class ComponentGuaranteesAttribute : Attribute
	{
		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003C29 RID: 15401 RVA: 0x000D1173 File Offset: 0x000CF373
		public ComponentGuaranteesOptions Guarantees { get; }

		// Token: 0x06003C2A RID: 15402 RVA: 0x000D117B File Offset: 0x000CF37B
		public ComponentGuaranteesAttribute(ComponentGuaranteesOptions guarantees)
		{
			this.Guarantees = guarantees;
		}
	}
}
