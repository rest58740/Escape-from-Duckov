using System;
using ES3Internal;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000058 RID: 88
	[Preserve]
	internal class ES3ReflectedObjectType : ES3ObjectType
	{
		// Token: 0x060002BD RID: 701 RVA: 0x0000A347 File Offset: 0x00008547
		public ES3ReflectedObjectType(Type type) : base(type)
		{
			this.isReflectedType = true;
			base.GetMembers(true);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000A35E File Offset: 0x0000855E
		protected override void WriteObject(object obj, ES3Writer writer)
		{
			base.WriteProperties(obj, writer);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000A368 File Offset: 0x00008568
		protected override object ReadObject<T>(ES3Reader reader)
		{
			object obj = ES3Reflection.CreateInstance(this.type);
			base.ReadProperties(reader, obj);
			return obj;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000A38B File Offset: 0x0000858B
		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			base.ReadProperties(reader, obj);
		}
	}
}
