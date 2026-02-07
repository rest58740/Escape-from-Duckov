using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000059 RID: 89
	[Preserve]
	internal class ES3ReflectedScriptableObjectType : ES3ScriptableObjectType
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x0000A396 File Offset: 0x00008596
		public ES3ReflectedScriptableObjectType(Type type) : base(type)
		{
			this.isReflectedType = true;
			base.GetMembers(true);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000A3AD File Offset: 0x000085AD
		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			base.WriteProperties(obj, writer);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000A3B7 File Offset: 0x000085B7
		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			base.ReadProperties(reader, obj);
		}
	}
}
