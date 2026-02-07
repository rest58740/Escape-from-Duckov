using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000073 RID: 115
	[Preserve]
	[ES3Properties(new string[]
	{
		"localPosition",
		"localRotation",
		"localScale",
		"parent",
		"siblingIndex"
	})]
	public class ES3Type_Transform : ES3ComponentType
	{
		// Token: 0x06000305 RID: 773 RVA: 0x0000EBD8 File Offset: 0x0000CDD8
		public ES3Type_Transform() : base(typeof(Transform))
		{
			ES3Type_Transform.Instance = this;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000EBF0 File Offset: 0x0000CDF0
		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			Transform transform = (Transform)obj;
			writer.WritePropertyByRef("parent", transform.parent);
			writer.WriteProperty("localPosition", transform.localPosition);
			writer.WriteProperty("localRotation", transform.localRotation);
			writer.WriteProperty("localScale", transform.localScale);
			writer.WriteProperty("siblingIndex", transform.GetSiblingIndex());
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000EC70 File Offset: 0x0000CE70
		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			Transform transform = (Transform)obj;
			CharacterController component = transform.gameObject.GetComponent<CharacterController>();
			if (component != null)
			{
				component.enabled = false;
			}
			foreach (object obj2 in reader.Properties)
			{
				string a = (string)obj2;
				if (!(a == "parent"))
				{
					if (!(a == "localPosition"))
					{
						if (!(a == "localRotation"))
						{
							if (!(a == "localScale"))
							{
								if (!(a == "siblingIndex"))
								{
									reader.Skip();
								}
								else
								{
									transform.SetSiblingIndex(reader.Read<int>());
								}
							}
							else
							{
								transform.localScale = reader.Read<Vector3>();
							}
						}
						else
						{
							transform.localRotation = reader.Read<Quaternion>();
						}
					}
					else
					{
						transform.localPosition = reader.Read<Vector3>();
					}
				}
				else
				{
					transform.SetParent(reader.Read<Transform>());
				}
			}
			if (component != null)
			{
				component.enabled = false;
			}
		}

		// Token: 0x040000B2 RID: 178
		public static int countRead;

		// Token: 0x040000B3 RID: 179
		public static ES3Type Instance;
	}
}
