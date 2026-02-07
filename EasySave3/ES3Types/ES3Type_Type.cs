using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000030 RID: 48
	[Preserve]
	[ES3Properties(new string[]
	{

	})]
	public class ES3Type_Type : ES3Type
	{
		// Token: 0x06000266 RID: 614 RVA: 0x00009687 File Offset: 0x00007887
		public ES3Type_Type() : base(typeof(Type))
		{
			ES3Type_Type.Instance = this;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x000096A0 File Offset: 0x000078A0
		public override void Write(object obj, ES3Writer writer)
		{
			Type type = (Type)obj;
			writer.WriteProperty("assemblyQualifiedName", type.AssemblyQualifiedName);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x000096C5 File Offset: 0x000078C5
		public override object Read<T>(ES3Reader reader)
		{
			return Type.GetType(reader.ReadProperty<string>());
		}

		// Token: 0x04000074 RID: 116
		public static ES3Type Instance;
	}
}
