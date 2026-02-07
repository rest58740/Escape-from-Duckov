using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000064 RID: 100
	[Preserve]
	[ES3Properties(new string[]
	{
		"sharedMesh",
		"convex",
		"inflateMesh",
		"skinWidth",
		"enabled",
		"isTrigger",
		"contactOffset",
		"sharedMaterial"
	})]
	public class ES3Type_MeshCollider : ES3ComponentType
	{
		// Token: 0x060002E4 RID: 740 RVA: 0x0000BF39 File Offset: 0x0000A139
		public ES3Type_MeshCollider() : base(typeof(MeshCollider))
		{
			ES3Type_MeshCollider.Instance = this;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000BF54 File Offset: 0x0000A154
		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			MeshCollider meshCollider = (MeshCollider)obj;
			writer.WritePropertyByRef("sharedMesh", meshCollider.sharedMesh);
			writer.WriteProperty("convex", meshCollider.convex, ES3Type_bool.Instance);
			writer.WriteProperty("enabled", meshCollider.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("isTrigger", meshCollider.isTrigger, ES3Type_bool.Instance);
			writer.WriteProperty("contactOffset", meshCollider.contactOffset, ES3Type_float.Instance);
			writer.WriteProperty("material", meshCollider.sharedMaterial);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000BFF8 File Offset: 0x0000A1F8
		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			MeshCollider meshCollider = (MeshCollider)obj;
			foreach (object obj2 in reader.Properties)
			{
				string a = (string)obj2;
				if (!(a == "sharedMesh"))
				{
					if (!(a == "convex"))
					{
						if (!(a == "enabled"))
						{
							if (!(a == "isTrigger"))
							{
								if (!(a == "contactOffset"))
								{
									if (!(a == "material"))
									{
										reader.Skip();
									}
									else
									{
										meshCollider.sharedMaterial = reader.Read<PhysicMaterial>();
									}
								}
								else
								{
									meshCollider.contactOffset = reader.Read<float>(ES3Type_float.Instance);
								}
							}
							else
							{
								meshCollider.isTrigger = reader.Read<bool>(ES3Type_bool.Instance);
							}
						}
						else
						{
							meshCollider.enabled = reader.Read<bool>(ES3Type_bool.Instance);
						}
					}
					else
					{
						meshCollider.convex = reader.Read<bool>(ES3Type_bool.Instance);
					}
				}
				else
				{
					meshCollider.sharedMesh = reader.Read<Mesh>(ES3Type_Mesh.Instance);
				}
			}
		}

		// Token: 0x040000A3 RID: 163
		public static ES3Type Instance;
	}
}
