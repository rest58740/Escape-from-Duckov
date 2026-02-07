using System;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200005B RID: 91
	[Preserve]
	internal class ES3ReflectedUnityObjectType : ES3UnityObjectType
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x0000A5FA File Offset: 0x000087FA
		public ES3ReflectedUnityObjectType(Type type) : base(type)
		{
			this.isReflectedType = true;
			base.GetMembers(true);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000A611 File Offset: 0x00008811
		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
			base.WriteProperties(obj, writer);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000A61C File Offset: 0x0000881C
		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			object obj = ES3Reflection.CreateInstance(this.type);
			base.ReadProperties(reader, obj);
			return obj;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000A63F File Offset: 0x0000883F
		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
			base.ReadProperties(reader, obj);
		}
	}
}
