using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200005E RID: 94
	[Preserve]
	[ES3Properties(new string[]
	{
		"size",
		"density",
		"isTrigger",
		"usedByEffector",
		"offset",
		"sharedMaterial",
		"enabled"
	})]
	public class ES3Type_BoxCollider2D : ES3ComponentType
	{
		// Token: 0x060002D4 RID: 724 RVA: 0x0000A87C File Offset: 0x00008A7C
		public ES3Type_BoxCollider2D() : base(typeof(BoxCollider2D))
		{
			ES3Type_BoxCollider2D.Instance = this;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000A894 File Offset: 0x00008A94
		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			BoxCollider2D boxCollider2D = (BoxCollider2D)obj;
			writer.WriteProperty("size", boxCollider2D.size);
			if (boxCollider2D.attachedRigidbody != null && boxCollider2D.attachedRigidbody.useAutoMass)
			{
				writer.WriteProperty("density", boxCollider2D.density);
			}
			writer.WriteProperty("isTrigger", boxCollider2D.isTrigger);
			writer.WriteProperty("usedByEffector", boxCollider2D.usedByEffector);
			writer.WriteProperty("offset", boxCollider2D.offset);
			writer.WritePropertyByRef("sharedMaterial", boxCollider2D.sharedMaterial);
			writer.WriteProperty("enabled", boxCollider2D.enabled);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000A958 File Offset: 0x00008B58
		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			BoxCollider2D boxCollider2D = (BoxCollider2D)obj;
			foreach (object obj2 in reader.Properties)
			{
				string text = (string)obj2;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 348705738U)
				{
					if (num != 44485181U)
					{
						if (num != 49525662U)
						{
							if (num == 348705738U)
							{
								if (text == "offset")
								{
									boxCollider2D.offset = reader.Read<Vector2>();
									continue;
								}
							}
						}
						else if (text == "enabled")
						{
							boxCollider2D.enabled = reader.Read<bool>();
							continue;
						}
					}
					else if (text == "sharedMaterial")
					{
						boxCollider2D.sharedMaterial = reader.Read<PhysicsMaterial2D>();
						continue;
					}
				}
				else if (num <= 1924728219U)
				{
					if (num != 597743964U)
					{
						if (num == 1924728219U)
						{
							if (text == "density")
							{
								boxCollider2D.density = reader.Read<float>();
								continue;
							}
						}
					}
					else if (text == "size")
					{
						boxCollider2D.size = reader.Read<Vector2>();
						continue;
					}
				}
				else if (num != 2267167091U)
				{
					if (num == 3063381777U)
					{
						if (text == "usedByEffector")
						{
							boxCollider2D.usedByEffector = reader.Read<bool>();
							continue;
						}
					}
				}
				else if (text == "isTrigger")
				{
					boxCollider2D.isTrigger = reader.Read<bool>();
					continue;
				}
				reader.Skip();
			}
		}

		// Token: 0x0400009D RID: 157
		public static ES3Type Instance;
	}
}
