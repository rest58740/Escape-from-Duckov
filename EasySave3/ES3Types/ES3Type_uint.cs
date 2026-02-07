using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200004F RID: 79
	[Preserve]
	public class ES3Type_uint : ES3Type
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000A1A1 File Offset: 0x000083A1
		public ES3Type_uint() : base(typeof(uint))
		{
			this.isPrimitive = true;
			ES3Type_uint.Instance = this;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000A1C0 File Offset: 0x000083C0
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((uint)obj);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000A1CE File Offset: 0x000083CE
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_uint());
		}

		// Token: 0x04000094 RID: 148
		public static ES3Type Instance;
	}
}
