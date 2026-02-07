using System;
using System.Collections.Generic;
using UnityEngine;

namespace EPOOutline
{
	// Token: 0x02000022 RID: 34
	[Serializable]
	public class SerializedPass : ISerializationCallbackReceiver
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000063FB File Offset: 0x000045FB
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00006403 File Offset: 0x00004603
		public Shader Shader
		{
			get
			{
				return this.shader;
			}
			set
			{
				this.propertiesIsDirty = true;
				this.shader = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00006414 File Offset: 0x00004614
		internal Material Material
		{
			get
			{
				if (this.shader == null)
				{
					return null;
				}
				if (this.material == null || this.material.shader != this.shader)
				{
					if (this.material != null)
					{
						UnityEngine.Object.DestroyImmediate(this.material);
					}
					this.material = new Material(this.shader);
				}
				if (!this.propertiesIsDirty)
				{
					return this.material;
				}
				foreach (KeyValuePair<int, SerializedPass.SerializedPassProperty> keyValuePair in this.propertiesById)
				{
					switch (keyValuePair.Value.PropertyType)
					{
					case SerializedPass.PropertyType.Color:
						this.material.SetColor(keyValuePair.Key, keyValuePair.Value.ColorValue);
						break;
					case SerializedPass.PropertyType.Vector:
						this.material.SetVector(keyValuePair.Key, keyValuePair.Value.VectorValue);
						break;
					case SerializedPass.PropertyType.Float:
						this.material.SetFloat(keyValuePair.Key, keyValuePair.Value.FloatValue);
						break;
					case SerializedPass.PropertyType.Range:
						this.material.SetFloat(keyValuePair.Key, keyValuePair.Value.FloatValue);
						break;
					}
				}
				this.propertiesIsDirty = false;
				return this.material;
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000658C File Offset: 0x0000478C
		public bool HasProperty(string name)
		{
			return this.propertiesByName.ContainsKey(name);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000659A File Offset: 0x0000479A
		public bool HasProperty(int hash)
		{
			return this.propertiesById.ContainsKey(hash);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000065A8 File Offset: 0x000047A8
		public Vector4 GetVector(string name)
		{
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesByName.TryGetValue(name, out serializedPassProperty))
			{
				Debug.LogError("The property " + name + " doesn't exist");
				return Vector4.zero;
			}
			if (serializedPassProperty.PropertyType == SerializedPass.PropertyType.Vector)
			{
				return serializedPassProperty.VectorValue;
			}
			Debug.LogError("The property " + name + " is not a vector property");
			return Vector4.zero;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000660C File Offset: 0x0000480C
		public Vector4 GetVector(int hash)
		{
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesById.TryGetValue(hash, out serializedPassProperty))
			{
				Debug.LogError("The property " + hash.ToString() + " doesn't exist");
				return Vector4.zero;
			}
			if (serializedPassProperty.PropertyType == SerializedPass.PropertyType.Vector)
			{
				return serializedPassProperty.VectorValue;
			}
			Debug.LogError("The property " + hash.ToString() + " is not a vector property");
			return Vector4.zero;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000667C File Offset: 0x0000487C
		public void SetVector(string name, Vector4 value)
		{
			this.propertiesIsDirty = true;
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesByName.TryGetValue(name, out serializedPassProperty))
			{
				serializedPassProperty = new SerializedPass.SerializedPassProperty();
				serializedPassProperty.PropertyType = SerializedPass.PropertyType.Vector;
				this.propertiesByName.Add(name, serializedPassProperty);
				this.propertiesById.Add(Shader.PropertyToID(name), serializedPassProperty);
			}
			if (serializedPassProperty.PropertyType != SerializedPass.PropertyType.Vector)
			{
				Debug.LogError("The property " + name + " is not a vector property");
				return;
			}
			serializedPassProperty.VectorValue = value;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000066F4 File Offset: 0x000048F4
		public void SetVector(int hash, Vector4 value)
		{
			this.propertiesIsDirty = true;
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesById.TryGetValue(hash, out serializedPassProperty))
			{
				Debug.LogWarning("The property " + hash.ToString() + " doesn't exist. Use string overload to create one.");
				return;
			}
			if (serializedPassProperty.PropertyType != SerializedPass.PropertyType.Vector)
			{
				Debug.LogError("The property " + hash.ToString() + " is not a vector property");
				return;
			}
			serializedPassProperty.VectorValue = value;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006764 File Offset: 0x00004964
		public float GetFloat(string name)
		{
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesByName.TryGetValue(name, out serializedPassProperty))
			{
				Debug.LogError("The property " + name + " doesn't exist");
				return 0f;
			}
			if (serializedPassProperty.PropertyType == SerializedPass.PropertyType.Float || serializedPassProperty.PropertyType == SerializedPass.PropertyType.Range)
			{
				return serializedPassProperty.FloatValue;
			}
			Debug.LogError("The property " + name + " is not a float property");
			return 0f;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000067D4 File Offset: 0x000049D4
		public float GetFloat(int hash)
		{
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesById.TryGetValue(hash, out serializedPassProperty))
			{
				Debug.LogError("The property " + hash.ToString() + " is doesn't exist");
				return 0f;
			}
			if (serializedPassProperty.PropertyType == SerializedPass.PropertyType.Float || serializedPassProperty.PropertyType == SerializedPass.PropertyType.Range)
			{
				return serializedPassProperty.FloatValue;
			}
			Debug.LogError("The property " + hash.ToString() + " is not a float property");
			return 0f;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006850 File Offset: 0x00004A50
		public void SetFloat(string name, float value)
		{
			this.propertiesIsDirty = true;
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesByName.TryGetValue(name, out serializedPassProperty))
			{
				serializedPassProperty = new SerializedPass.SerializedPassProperty();
				serializedPassProperty.PropertyType = SerializedPass.PropertyType.Float;
				this.propertiesByName.Add(name, serializedPassProperty);
				this.propertiesById.Add(Shader.PropertyToID(name), serializedPassProperty);
			}
			if (serializedPassProperty.PropertyType != SerializedPass.PropertyType.Float && serializedPassProperty.PropertyType != SerializedPass.PropertyType.Range)
			{
				Debug.LogError("The property " + name + " is not a float property");
				return;
			}
			serializedPassProperty.FloatValue = value;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000068D4 File Offset: 0x00004AD4
		public void SetFloat(int hash, float value)
		{
			this.propertiesIsDirty = true;
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesById.TryGetValue(hash, out serializedPassProperty))
			{
				Debug.LogError("The property " + hash.ToString() + " doesn't exist. Use string overload to create one.");
				return;
			}
			if (serializedPassProperty.PropertyType != SerializedPass.PropertyType.Float)
			{
				Debug.LogError("The property " + hash.ToString() + " is not a float property");
				return;
			}
			serializedPassProperty.FloatValue = value;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006944 File Offset: 0x00004B44
		public Color GetColor(string name)
		{
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesByName.TryGetValue(name, out serializedPassProperty))
			{
				Debug.LogError("The property " + name + " doesn't exist");
				return Color.black;
			}
			if (serializedPassProperty.PropertyType == SerializedPass.PropertyType.Color)
			{
				return serializedPassProperty.ColorValue;
			}
			Debug.LogError("The property " + name + " is not a color property");
			return Color.black;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000069A8 File Offset: 0x00004BA8
		public Color GetColor(int hash)
		{
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesById.TryGetValue(hash, out serializedPassProperty))
			{
				Debug.LogError("The property " + hash.ToString() + " doesn't exist");
				return Color.black;
			}
			if (serializedPassProperty.PropertyType == SerializedPass.PropertyType.Color)
			{
				return serializedPassProperty.ColorValue;
			}
			return Color.black;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000069FC File Offset: 0x00004BFC
		public void SetColor(string name, Color value)
		{
			this.propertiesIsDirty = true;
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesByName.TryGetValue(name, out serializedPassProperty))
			{
				serializedPassProperty = new SerializedPass.SerializedPassProperty();
				serializedPassProperty.PropertyType = SerializedPass.PropertyType.Color;
				this.propertiesByName.Add(name, serializedPassProperty);
				this.propertiesById.Add(Shader.PropertyToID(name), serializedPassProperty);
			}
			if (serializedPassProperty.PropertyType != SerializedPass.PropertyType.Color)
			{
				Debug.LogError("The property " + name + " is not a color property.");
				return;
			}
			serializedPassProperty.ColorValue = value;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006A74 File Offset: 0x00004C74
		public void SetColor(int hash, Color value)
		{
			this.propertiesIsDirty = true;
			SerializedPass.SerializedPassProperty serializedPassProperty = null;
			if (!this.propertiesById.TryGetValue(hash, out serializedPassProperty))
			{
				Debug.LogError("The property " + hash.ToString() + " doesn't exist. Use string overload to create one.");
				return;
			}
			if (serializedPassProperty.PropertyType != SerializedPass.PropertyType.Color)
			{
				Debug.LogError("The property " + hash.ToString() + " is not a color property");
				return;
			}
			serializedPassProperty.ColorValue = value;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006AE4 File Offset: 0x00004CE4
		public void OnBeforeSerialize()
		{
			this.serializedProperties.Clear();
			foreach (KeyValuePair<string, SerializedPass.SerializedPassProperty> keyValuePair in this.propertiesByName)
			{
				SerializedPass.SerializedPropertyKeyValuePair serializedPropertyKeyValuePair = new SerializedPass.SerializedPropertyKeyValuePair();
				serializedPropertyKeyValuePair.Property = keyValuePair.Value;
				serializedPropertyKeyValuePair.PropertyName = keyValuePair.Key;
				this.serializedProperties.Add(serializedPropertyKeyValuePair);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006B68 File Offset: 0x00004D68
		public void OnAfterDeserialize()
		{
			this.propertiesIsDirty = true;
			this.propertiesById.Clear();
			this.propertiesByName.Clear();
			foreach (SerializedPass.SerializedPropertyKeyValuePair serializedPropertyKeyValuePair in this.serializedProperties)
			{
				if (!this.propertiesByName.ContainsKey(serializedPropertyKeyValuePair.PropertyName))
				{
					this.propertiesById.Add(Shader.PropertyToID(serializedPropertyKeyValuePair.PropertyName), serializedPropertyKeyValuePair.Property);
					this.propertiesByName.Add(serializedPropertyKeyValuePair.PropertyName, serializedPropertyKeyValuePair.Property);
				}
			}
		}

		// Token: 0x040000C3 RID: 195
		[SerializeField]
		private Shader shader;

		// Token: 0x040000C4 RID: 196
		[SerializeField]
		private List<SerializedPass.SerializedPropertyKeyValuePair> serializedProperties = new List<SerializedPass.SerializedPropertyKeyValuePair>();

		// Token: 0x040000C5 RID: 197
		private Dictionary<int, SerializedPass.SerializedPassProperty> propertiesById = new Dictionary<int, SerializedPass.SerializedPassProperty>();

		// Token: 0x040000C6 RID: 198
		private Dictionary<string, SerializedPass.SerializedPassProperty> propertiesByName = new Dictionary<string, SerializedPass.SerializedPassProperty>();

		// Token: 0x040000C7 RID: 199
		private Material material;

		// Token: 0x040000C8 RID: 200
		private bool propertiesIsDirty;

		// Token: 0x02000033 RID: 51
		public enum PropertyType
		{
			// Token: 0x04000110 RID: 272
			Color,
			// Token: 0x04000111 RID: 273
			Vector,
			// Token: 0x04000112 RID: 274
			Float,
			// Token: 0x04000113 RID: 275
			Range,
			// Token: 0x04000114 RID: 276
			TexEnv
		}

		// Token: 0x02000034 RID: 52
		[Serializable]
		private class SerializedPropertyKeyValuePair
		{
			// Token: 0x04000115 RID: 277
			[SerializeField]
			public string PropertyName;

			// Token: 0x04000116 RID: 278
			[SerializeField]
			public SerializedPass.SerializedPassProperty Property;
		}

		// Token: 0x02000035 RID: 53
		[Serializable]
		private class SerializedPassProperty
		{
			// Token: 0x04000117 RID: 279
			[SerializeField]
			public Color ColorValue;

			// Token: 0x04000118 RID: 280
			[SerializeField]
			public float FloatValue;

			// Token: 0x04000119 RID: 281
			[SerializeField]
			public Vector4 VectorValue;

			// Token: 0x0400011A RID: 282
			[SerializeField]
			public SerializedPass.PropertyType PropertyType;
		}
	}
}
