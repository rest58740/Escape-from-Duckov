using System;
using ES3Internal;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200002A RID: 42
	[Preserve]
	public abstract class ES3ScriptableObjectType : ES3UnityObjectType
	{
		// Token: 0x06000246 RID: 582 RVA: 0x00008CB6 File Offset: 0x00006EB6
		public ES3ScriptableObjectType(Type type) : base(type)
		{
		}

		// Token: 0x06000247 RID: 583
		protected abstract void WriteScriptableObject(object obj, ES3Writer writer);

		// Token: 0x06000248 RID: 584
		protected abstract void ReadScriptableObject<T>(ES3Reader reader, object obj);

		// Token: 0x06000249 RID: 585 RVA: 0x00008CC0 File Offset: 0x00006EC0
		protected override void WriteUnityObject(object obj, ES3Writer writer)
		{
			ScriptableObject scriptableObject = obj as ScriptableObject;
			if (obj != null && scriptableObject == null)
			{
				string str = "Only types of UnityEngine.ScriptableObject can be written with this method, but argument given is type of ";
				Type type = obj.GetType();
				throw new ArgumentException(str + ((type != null) ? type.ToString() : null));
			}
			this.WriteScriptableObject(scriptableObject, writer);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00008D0A File Offset: 0x00006F0A
		protected override void ReadUnityObject<T>(ES3Reader reader, object obj)
		{
			this.ReadScriptableObject<T>(reader, obj);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00008D14 File Offset: 0x00006F14
		protected override object ReadUnityObject<T>(ES3Reader reader)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00008D1C File Offset: 0x00006F1C
		protected override object ReadObject<T>(ES3Reader reader)
		{
			ES3ReferenceMgrBase es3ReferenceMgrBase = ES3ReferenceMgrBase.Current;
			long id = -1L;
			UnityEngine.Object @object = null;
			foreach (object obj in reader.Properties)
			{
				string text = (string)obj;
				if (text == "_ES3Ref" && es3ReferenceMgrBase != null)
				{
					id = reader.Read_ref();
					@object = es3ReferenceMgrBase.Get(id, this.type, false);
					if (@object != null)
					{
						break;
					}
				}
				else
				{
					reader.overridePropertiesName = text;
					if (!(@object == null))
					{
						break;
					}
					@object = ScriptableObject.CreateInstance(this.type);
					if (es3ReferenceMgrBase != null)
					{
						es3ReferenceMgrBase.Add(@object, id);
						break;
					}
					break;
				}
			}
			this.ReadScriptableObject<T>(reader, @object);
			return @object;
		}
	}
}
