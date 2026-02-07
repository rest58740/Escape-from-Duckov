using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000A6 RID: 166
	[Preserve]
	[ES3Properties(new string[]
	{
		"bounciness",
		"friction"
	})]
	public class ES3Type_PhysicsMaterial2D : ES3ObjectType
	{
		// Token: 0x0600039A RID: 922 RVA: 0x00015461 File Offset: 0x00013661
		public ES3Type_PhysicsMaterial2D() : base(typeof(PhysicsMaterial2D))
		{
			ES3Type_PhysicsMaterial2D.Instance = this;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001547C File Offset: 0x0001367C
		protected override void WriteObject(object obj, ES3Writer writer)
		{
			PhysicsMaterial2D physicsMaterial2D = (PhysicsMaterial2D)obj;
			writer.WriteProperty("bounciness", physicsMaterial2D.bounciness, ES3Type_float.Instance);
			writer.WriteProperty("friction", physicsMaterial2D.friction, ES3Type_float.Instance);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000154C8 File Offset: 0x000136C8
		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			PhysicsMaterial2D physicsMaterial2D = (PhysicsMaterial2D)obj;
			foreach (object obj2 in reader.Properties)
			{
				string a = (string)obj2;
				if (!(a == "bounciness"))
				{
					if (!(a == "friction"))
					{
						reader.Skip();
					}
					else
					{
						physicsMaterial2D.friction = reader.Read<float>(ES3Type_float.Instance);
					}
				}
				else
				{
					physicsMaterial2D.bounciness = reader.Read<float>(ES3Type_float.Instance);
				}
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001556C File Offset: 0x0001376C
		protected override object ReadObject<T>(ES3Reader reader)
		{
			PhysicsMaterial2D physicsMaterial2D = new PhysicsMaterial2D();
			this.ReadObject<T>(reader, physicsMaterial2D);
			return physicsMaterial2D;
		}

		// Token: 0x040000E9 RID: 233
		public static ES3Type Instance;
	}
}
