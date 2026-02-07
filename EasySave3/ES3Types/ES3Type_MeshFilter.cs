using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000066 RID: 102
	[Preserve]
	[ES3Properties(new string[]
	{
		"sharedMesh"
	})]
	public class ES3Type_MeshFilter : ES3ComponentType
	{
		// Token: 0x060002E8 RID: 744 RVA: 0x0000C139 File Offset: 0x0000A339
		public ES3Type_MeshFilter() : base(typeof(MeshFilter))
		{
			ES3Type_MeshFilter.Instance = this;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000C154 File Offset: 0x0000A354
		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			MeshFilter meshFilter = (MeshFilter)obj;
			writer.WritePropertyByRef("sharedMesh", meshFilter.sharedMesh);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000C17C File Offset: 0x0000A37C
		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			MeshFilter meshFilter = (MeshFilter)obj;
			using (IEnumerator enumerator = reader.Properties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((string)enumerator.Current == "sharedMesh")
					{
						meshFilter.sharedMesh = reader.Read<Mesh>(ES3Type_Mesh.Instance);
					}
					else
					{
						reader.Skip();
					}
				}
			}
		}

		// Token: 0x040000A5 RID: 165
		public static ES3Type Instance;
	}
}
