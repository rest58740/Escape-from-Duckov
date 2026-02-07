using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200003E RID: 62
	[Preserve]
	public class ES3Type_ES3Ref : ES3Type
	{
		// Token: 0x06000286 RID: 646 RVA: 0x00009E44 File Offset: 0x00008044
		public ES3Type_ES3Ref() : base(typeof(long))
		{
			this.isPrimitive = true;
			ES3Type_ES3Ref.Instance = this;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00009E64 File Offset: 0x00008064
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive(((long)obj).ToString());
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00009E85 File Offset: 0x00008085
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)new ES3Ref(reader.Read_ref()));
		}

		// Token: 0x04000083 RID: 131
		public static ES3Type Instance = new ES3Type_ES3Ref();
	}
}
