using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Pathfinding.Util;
using Pathfinding.WindowsStore;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x02000242 RID: 578
	public class TinyJsonDeserializer
	{
		// Token: 0x06000DA4 RID: 3492 RVA: 0x00055AD5 File Offset: 0x00053CD5
		public static object Deserialize(string text, Type type, object populate = null, GameObject contextRoot = null)
		{
			return new TinyJsonDeserializer
			{
				reader = new StringReader(text),
				fullTextDebug = text,
				contextRoot = contextRoot
			}.Deserialize(type, populate);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00055B00 File Offset: 0x00053D00
		private object Deserialize(Type tp, object populate = null)
		{
			Type typeInfo = WindowsStoreCompatibility.GetTypeInfo(tp);
			if (typeInfo.IsEnum)
			{
				return Enum.Parse(tp, this.EatField());
			}
			if (this.TryEat('n'))
			{
				this.Eat("ull");
				this.TryEat(',');
				return null;
			}
			if (object.Equals(tp, typeof(float)))
			{
				return float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
			}
			if (object.Equals(tp, typeof(int)))
			{
				return int.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
			}
			if (object.Equals(tp, typeof(uint)))
			{
				return uint.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
			}
			if (object.Equals(tp, typeof(bool)))
			{
				return bool.Parse(this.EatField());
			}
			if (object.Equals(tp, typeof(string)))
			{
				return this.EatField();
			}
			if (object.Equals(tp, typeof(Version)))
			{
				return new Version(this.EatField());
			}
			if (object.Equals(tp, typeof(Vector2)))
			{
				this.Eat("{");
				Vector2 vector = default(Vector2);
				this.EatField();
				vector.x = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.EatField();
				vector.y = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.Eat("}");
				return vector;
			}
			if (object.Equals(tp, typeof(Vector3)))
			{
				this.Eat("{");
				Vector3 vector2 = default(Vector3);
				this.EatField();
				vector2.x = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.EatField();
				vector2.y = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.EatField();
				vector2.z = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.Eat("}");
				return vector2;
			}
			if (object.Equals(tp, typeof(Pathfinding.Util.Guid)))
			{
				this.Eat("{");
				this.EatField();
				Pathfinding.Util.Guid guid = Pathfinding.Util.Guid.Parse(this.EatField());
				this.Eat("}");
				return guid;
			}
			if (object.Equals(tp, typeof(LayerMask)))
			{
				this.Eat("{");
				this.EatField();
				LayerMask layerMask = int.Parse(this.EatField());
				this.Eat("}");
				return layerMask;
			}
			if (tp.IsGenericType && object.Equals(tp.GetGenericTypeDefinition(), typeof(List<>)))
			{
				IList list = (IList)Activator.CreateInstance(tp);
				Type tp2 = tp.GetGenericArguments()[0];
				this.Eat("[");
				while (!this.TryEat(']'))
				{
					list.Add(this.Deserialize(tp2, null));
					this.TryEat(',');
				}
				return list;
			}
			if (typeInfo.IsArray)
			{
				List<object> list2 = new List<object>();
				this.Eat("[");
				while (!this.TryEat(']'))
				{
					list2.Add(this.Deserialize(tp.GetElementType(), null));
					this.TryEat(',');
				}
				Array array = Array.CreateInstance(tp.GetElementType(), list2.Count);
				list2.ToArray().CopyTo(array, 0);
				return array;
			}
			if (typeof(UnityEngine.Object).IsAssignableFrom(tp))
			{
				return this.DeserializeUnityObject();
			}
			this.Eat("{");
			if (typeInfo.GetCustomAttributes(typeof(JsonDynamicTypeAttribute), true).Length != 0)
			{
				string text = this.EatField();
				if (text != "@type")
				{
					throw new Exception("Expected field '@type' but found '" + text + "'\n\nWhen trying to deserialize: " + this.fullTextDebug);
				}
				string text2 = this.EatField();
				JsonDynamicTypeAliasAttribute[] array2 = typeInfo.GetCustomAttributes(typeof(JsonDynamicTypeAliasAttribute), true) as JsonDynamicTypeAliasAttribute[];
				string b = text2.Split(',', StringSplitOptions.None)[0];
				Type type = null;
				foreach (JsonDynamicTypeAliasAttribute jsonDynamicTypeAliasAttribute in array2)
				{
					if (jsonDynamicTypeAliasAttribute.alias == b)
					{
						type = jsonDynamicTypeAliasAttribute.type;
					}
				}
				if (type == null)
				{
					type = Type.GetType(text2);
				}
				Type type2 = type;
				if (type2 == null)
				{
					throw new Exception("Could not find a type with the name '" + text2 + "'\n\nWhen trying to deserialize: " + this.fullTextDebug);
				}
				tp = type2;
				typeInfo = WindowsStoreCompatibility.GetTypeInfo(tp);
			}
			object obj = populate ?? Activator.CreateInstance(tp);
			while (!this.TryEat('}'))
			{
				string name = this.EatField();
				Type type3 = tp;
				FieldInfo fieldInfo = null;
				while (fieldInfo == null && type3 != null)
				{
					fieldInfo = type3.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					type3 = type3.BaseType;
				}
				if (fieldInfo == null)
				{
					PropertyInfo propertyInfo = null;
					type3 = tp;
					while (propertyInfo == null && type3 != null)
					{
						propertyInfo = type3.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						type3 = type3.BaseType;
					}
					if (propertyInfo == null)
					{
						this.SkipFieldData();
					}
					else
					{
						propertyInfo.SetValue(obj, this.Deserialize(propertyInfo.PropertyType, null));
					}
				}
				else
				{
					fieldInfo.SetValue(obj, this.Deserialize(fieldInfo.FieldType, null));
				}
				this.TryEat(',');
			}
			return obj;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00056067 File Offset: 0x00054267
		private UnityEngine.Object DeserializeUnityObject()
		{
			this.Eat("{");
			UnityEngine.Object result = this.DeserializeUnityObjectInner();
			this.Eat("}");
			return result;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00056088 File Offset: 0x00054288
		private UnityEngine.Object DeserializeUnityObjectInner()
		{
			string a = this.EatField();
			if (a == "InstanceID")
			{
				this.EatField();
				a = this.EatField();
			}
			if (a != "Name")
			{
				throw new Exception("Expected 'Name' field");
			}
			string text = this.EatField();
			if (text == null)
			{
				return null;
			}
			if (this.EatField() != "Type")
			{
				throw new Exception("Expected 'Type' field");
			}
			string text2 = this.EatField();
			if (text2.IndexOf(',') != -1)
			{
				text2 = text2.Substring(0, text2.IndexOf(','));
			}
			Type type = WindowsStoreCompatibility.GetTypeInfo(typeof(AstarPath)).Assembly.GetType(text2);
			type = (type ?? WindowsStoreCompatibility.GetTypeInfo(typeof(Transform)).Assembly.GetType(text2));
			if (object.Equals(type, null))
			{
				Debug.LogError("Could not find type '" + text2 + "'. Cannot deserialize Unity reference");
				return null;
			}
			this.EatWhitespace();
			if ((ushort)this.reader.Peek() == 34)
			{
				if (this.EatField() != "GUID")
				{
					throw new Exception("Expected 'GUID' field");
				}
				string b = this.EatField();
				UnityReferenceHelper[] array;
				int i;
				if (this.contextRoot != null)
				{
					array = this.contextRoot.GetComponentsInChildren<UnityReferenceHelper>(true);
					i = 0;
					while (i < array.Length)
					{
						UnityReferenceHelper unityReferenceHelper = array[i];
						if (unityReferenceHelper.GetGUID() == b)
						{
							if (object.Equals(type, typeof(GameObject)))
							{
								return unityReferenceHelper.gameObject;
							}
							return unityReferenceHelper.GetComponent(type);
						}
						else
						{
							i++;
						}
					}
				}
				array = UnityCompatibility.FindObjectsByTypeUnsortedWithInactive<UnityReferenceHelper>();
				i = 0;
				while (i < array.Length)
				{
					UnityReferenceHelper unityReferenceHelper2 = array[i];
					if (unityReferenceHelper2.GetGUID() == b)
					{
						if (object.Equals(type, typeof(GameObject)))
						{
							return unityReferenceHelper2.gameObject;
						}
						return unityReferenceHelper2.GetComponent(type);
					}
					else
					{
						i++;
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				UnityEngine.Object[] array2 = Resources.LoadAll(text, type);
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j].name == text || array2.Length == 1)
					{
						return array2[j];
					}
				}
			}
			return null;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000562B4 File Offset: 0x000544B4
		private void EatWhitespace()
		{
			while (char.IsWhiteSpace((char)this.reader.Peek()))
			{
				this.reader.Read();
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000562D8 File Offset: 0x000544D8
		private void Eat(string s)
		{
			this.EatWhitespace();
			for (int i = 0; i < s.Length; i++)
			{
				char c = (char)this.reader.Read();
				if (c != s[i])
				{
					throw new Exception(string.Concat(new string[]
					{
						"Expected '",
						s[i].ToString(),
						"' found '",
						c.ToString(),
						"'\n\n...",
						this.reader.ReadLine(),
						"\n\nWhen trying to deserialize: ",
						this.fullTextDebug
					}));
				}
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0005637C File Offset: 0x0005457C
		private string EatUntil(string c, bool inString)
		{
			this.builder.Length = 0;
			bool flag = false;
			for (;;)
			{
				int num = this.reader.Peek();
				if (!flag && (ushort)num == 34)
				{
					inString = !inString;
				}
				char c2 = (char)num;
				if (num == -1)
				{
					break;
				}
				if (!flag && c2 == '\\')
				{
					flag = true;
					this.reader.Read();
				}
				else
				{
					if (!inString && c.IndexOf(c2) != -1)
					{
						goto IL_88;
					}
					this.builder.Append(c2);
					this.reader.Read();
					flag = false;
				}
			}
			throw new Exception("Unexpected EOF\n\nWhen trying to deserialize: " + this.fullTextDebug);
			IL_88:
			return this.builder.ToString();
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0005641C File Offset: 0x0005461C
		private bool TryEat(char c)
		{
			this.EatWhitespace();
			if ((char)this.reader.Peek() == c)
			{
				this.reader.Read();
				return true;
			}
			return false;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00056442 File Offset: 0x00054642
		private string EatField()
		{
			string result = this.EatUntil("\",}]", this.TryEat('"'));
			this.TryEat('"');
			this.TryEat(':');
			this.TryEat(',');
			return result;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00056474 File Offset: 0x00054674
		private void SkipFieldData()
		{
			int num = 0;
			for (;;)
			{
				this.EatUntil(",{}[]", false);
				char c = (char)this.reader.Peek();
				if (c <= '[')
				{
					if (c != ',')
					{
						if (c != '[')
						{
							break;
						}
						goto IL_3E;
					}
					else if (num == 0)
					{
						goto Block_8;
					}
				}
				else
				{
					if (c != ']')
					{
						if (c == '{')
						{
							goto IL_3E;
						}
						if (c != '}')
						{
							break;
						}
					}
					num--;
					if (num < 0)
					{
						return;
					}
				}
				IL_68:
				this.reader.Read();
				continue;
				IL_3E:
				num++;
				goto IL_68;
			}
			goto IL_5D;
			Block_8:
			this.reader.Read();
			return;
			IL_5D:
			throw new Exception("Should not reach this part");
		}

		// Token: 0x04000A84 RID: 2692
		private TextReader reader;

		// Token: 0x04000A85 RID: 2693
		private string fullTextDebug;

		// Token: 0x04000A86 RID: 2694
		private GameObject contextRoot;

		// Token: 0x04000A87 RID: 2695
		private static readonly NumberFormatInfo numberFormat = NumberFormatInfo.InvariantInfo;

		// Token: 0x04000A88 RID: 2696
		private StringBuilder builder = new StringBuilder();
	}
}
