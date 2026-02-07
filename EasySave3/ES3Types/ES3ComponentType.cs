using System;
using ES3Internal;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000028 RID: 40
	[Preserve]
	public abstract class ES3ComponentType : ES3UnityObjectType
	{
		// Token: 0x06000236 RID: 566 RVA: 0x00008945 File Offset: 0x00006B45
		public ES3ComponentType(Type type) : base(type)
		{
		}

		// Token: 0x06000237 RID: 567
		protected abstract void WriteComponent(object obj, ES3Writer writer);

		// Token: 0x06000238 RID: 568
		protected abstract void ReadComponent<T>(ES3Reader reader, object obj);

		// Token: 0x06000239 RID: 569 RVA: 0x00008950 File Offset: 0x00006B50
		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
			Component component = obj as Component;
			if (obj != null && component == null)
			{
				string str = "Only types of UnityEngine.Component can be written with this method, but argument given is type of ";
				Type type = obj.GetType();
				throw new ArgumentException(str + ((type != null) ? type.ToString() : null));
			}
			ES3ReferenceMgrBase es3ReferenceMgrBase = ES3ReferenceMgrBase.Current;
			if (es3ReferenceMgrBase != null)
			{
				writer.WriteProperty("goID", es3ReferenceMgrBase.Add(component.gameObject).ToString(), ES3Type_string.Instance);
			}
			this.WriteComponent(component, writer);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x000089CD File Offset: 0x00006BCD
		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
			this.ReadComponent<T>(reader, obj);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000089D7 File Offset: 0x00006BD7
		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000089E0 File Offset: 0x00006BE0
		protected override object ReadObject<T>(ES3Reader reader)
		{
			ES3ReferenceMgrBase es3ReferenceMgrBase = ES3ReferenceMgrBase.Current;
			long id = -1L;
			UnityEngine.Object @object = null;
			foreach (object obj in reader.Properties)
			{
				string text = (string)obj;
				if (text == "_ES3Ref")
				{
					id = reader.Read_ref();
					@object = es3ReferenceMgrBase.Get(id, true);
				}
				else if (text == "goID")
				{
					long id2 = reader.Read_ref();
					if (@object != null)
					{
						break;
					}
					GameObject gameObject = (GameObject)es3ReferenceMgrBase.Get(id2, this.type, false);
					if (gameObject == null)
					{
						gameObject = new GameObject("Easy Save 3 Loaded GameObject");
						es3ReferenceMgrBase.Add(gameObject, id2);
					}
					@object = ES3ComponentType.GetOrAddComponent(gameObject, this.type);
					es3ReferenceMgrBase.Add(@object, id);
					break;
				}
				else
				{
					reader.overridePropertiesName = text;
					if (@object == null)
					{
						GameObject gameObject2 = new GameObject("Easy Save 3 Loaded GameObject");
						@object = ES3ComponentType.GetOrAddComponent(gameObject2, this.type);
						es3ReferenceMgrBase.Add(@object, id);
						es3ReferenceMgrBase.Add(gameObject2);
						break;
					}
					break;
				}
			}
			if (@object != null)
			{
				this.ReadComponent<T>(reader, @object);
			}
			return @object;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00008B30 File Offset: 0x00006D30
		private static Component GetOrAddComponent(GameObject go, Type type)
		{
			Component component = go.GetComponent(type);
			if (component != null)
			{
				return component;
			}
			return go.AddComponent(type);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008B58 File Offset: 0x00006D58
		public static Component CreateComponent(Type type)
		{
			GameObject gameObject = new GameObject("Easy Save 3 Loaded Component");
			if (type == typeof(Transform))
			{
				return gameObject.GetComponent(type);
			}
			return ES3ComponentType.GetOrAddComponent(gameObject, type);
		}

		// Token: 0x04000063 RID: 99
		protected const string gameObjectPropertyName = "goID";
	}
}
