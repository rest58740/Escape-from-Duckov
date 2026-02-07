using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x020000A4 RID: 164
	[Preserve]
	[ES3Properties(new string[]
	{
		"dynamicFriction",
		"staticFriction",
		"bounciness",
		"frictionCombine",
		"bounceCombine"
	})]
	public class ES3Type_PhysicMaterial : ES3ObjectType
	{
		// Token: 0x06000395 RID: 917 RVA: 0x0001527C File Offset: 0x0001347C
		public ES3Type_PhysicMaterial() : base(typeof(PhysicMaterial))
		{
			ES3Type_PhysicMaterial.Instance = this;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00015294 File Offset: 0x00013494
		protected override void WriteObject(object obj, ES3Writer writer)
		{
			PhysicMaterial physicMaterial = (PhysicMaterial)obj;
			writer.WriteProperty("dynamicFriction", physicMaterial.dynamicFriction, ES3Type_float.Instance);
			writer.WriteProperty("staticFriction", physicMaterial.staticFriction, ES3Type_float.Instance);
			writer.WriteProperty("bounciness", physicMaterial.bounciness, ES3Type_float.Instance);
			writer.WriteProperty("frictionCombine", physicMaterial.frictionCombine);
			writer.WriteProperty("bounceCombine", physicMaterial.bounceCombine);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00015328 File Offset: 0x00013528
		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			PhysicMaterial physicMaterial = (PhysicMaterial)obj;
			foreach (object obj2 in reader.Properties)
			{
				string a = (string)obj2;
				if (!(a == "dynamicFriction"))
				{
					if (!(a == "staticFriction"))
					{
						if (!(a == "bounciness"))
						{
							if (!(a == "frictionCombine"))
							{
								if (!(a == "bounceCombine"))
								{
									reader.Skip();
								}
								else
								{
									physicMaterial.bounceCombine = reader.Read<PhysicMaterialCombine>();
								}
							}
							else
							{
								physicMaterial.frictionCombine = reader.Read<PhysicMaterialCombine>();
							}
						}
						else
						{
							physicMaterial.bounciness = reader.Read<float>(ES3Type_float.Instance);
						}
					}
					else
					{
						physicMaterial.staticFriction = reader.Read<float>(ES3Type_float.Instance);
					}
				}
				else
				{
					physicMaterial.dynamicFriction = reader.Read<float>(ES3Type_float.Instance);
				}
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00015428 File Offset: 0x00013628
		protected override object ReadObject<T>(ES3Reader reader)
		{
			PhysicMaterial physicMaterial = new PhysicMaterial();
			this.ReadObject<T>(reader, physicMaterial);
			return physicMaterial;
		}

		// Token: 0x040000E7 RID: 231
		public static ES3Type Instance;
	}
}
