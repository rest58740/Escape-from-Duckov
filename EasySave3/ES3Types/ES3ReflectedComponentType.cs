using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000057 RID: 87
	[Preserve]
	internal class ES3ReflectedComponentType : ES3ComponentType
	{
		// Token: 0x060002BA RID: 698 RVA: 0x0000A31B File Offset: 0x0000851B
		public ES3ReflectedComponentType(Type type) : base(type)
		{
			this.isReflectedType = true;
			base.GetMembers(true);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000A332 File Offset: 0x00008532
		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			base.WriteProperties(obj, writer);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000A33C File Offset: 0x0000853C
		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			base.ReadProperties(reader, obj);
		}
	}
}
