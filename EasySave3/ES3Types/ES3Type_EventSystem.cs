using System;
using UnityEngine.EventSystems;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000061 RID: 97
	[Preserve]
	public class ES3Type_EventSystem : ES3ComponentType
	{
		// Token: 0x060002DD RID: 733 RVA: 0x0000B824 File Offset: 0x00009A24
		public ES3Type_EventSystem() : base(typeof(EventSystem))
		{
			ES3Type_EventSystem.Instance = this;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000B83C File Offset: 0x00009A3C
		protected override void WriteComponent(object obj, ES3Writer writer)
		{
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000B840 File Offset: 0x00009A40
		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			foreach (object obj2 in reader.Properties)
			{
				string text = (string)obj2;
				reader.Skip();
			}
		}

		// Token: 0x040000A0 RID: 160
		public static ES3Type Instance;
	}
}
