using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000035 RID: 53
	[Preserve]
	public class ES3Type_char : ES3Type
	{
		// Token: 0x06000273 RID: 627 RVA: 0x000097B6 File Offset: 0x000079B6
		public ES3Type_char() : base(typeof(char))
		{
			this.isPrimitive = true;
			ES3Type_char.Instance = this;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000097D5 File Offset: 0x000079D5
		public override void Write(object obj, ES3Writer writer)
		{
			writer.WritePrimitive((char)obj);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x000097E3 File Offset: 0x000079E3
		public override object Read<T>(ES3Reader reader)
		{
			return (T)((object)reader.Read_char());
		}

		// Token: 0x04000079 RID: 121
		public static ES3Type Instance;
	}
}
