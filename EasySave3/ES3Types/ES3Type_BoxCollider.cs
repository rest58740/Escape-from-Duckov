using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200005D RID: 93
	[Preserve]
	[ES3Properties(new string[]
	{
		"center",
		"size",
		"enabled",
		"isTrigger",
		"contactOffset",
		"sharedMaterial"
	})]
	public class ES3Type_BoxCollider : ES3ComponentType
	{
		// Token: 0x060002D1 RID: 721 RVA: 0x0000A6C3 File Offset: 0x000088C3
		public ES3Type_BoxCollider() : base(typeof(BoxCollider))
		{
			ES3Type_BoxCollider.Instance = this;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000A6DC File Offset: 0x000088DC
		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			BoxCollider boxCollider = (BoxCollider)obj;
			writer.WriteProperty("center", boxCollider.center);
			writer.WriteProperty("size", boxCollider.size);
			writer.WriteProperty("enabled", boxCollider.enabled);
			writer.WriteProperty("isTrigger", boxCollider.isTrigger);
			writer.WriteProperty("contactOffset", boxCollider.contactOffset);
			writer.WritePropertyByRef("material", boxCollider.sharedMaterial);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000A770 File Offset: 0x00008970
		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			BoxCollider boxCollider = (BoxCollider)obj;
			foreach (object obj2 in reader.Properties)
			{
				string a = (string)obj2;
				if (!(a == "center"))
				{
					if (!(a == "size"))
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
										boxCollider.sharedMaterial = reader.Read<PhysicMaterial>();
									}
								}
								else
								{
									boxCollider.contactOffset = reader.Read<float>();
								}
							}
							else
							{
								boxCollider.isTrigger = reader.Read<bool>();
							}
						}
						else
						{
							boxCollider.enabled = reader.Read<bool>();
						}
					}
					else
					{
						boxCollider.size = reader.Read<Vector3>();
					}
				}
				else
				{
					boxCollider.center = reader.Read<Vector3>();
				}
			}
		}

		// Token: 0x0400009C RID: 156
		public static ES3Type Instance;
	}
}
