using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ES3Types;
using UnityEngine;

namespace ES3Internal
{
	// Token: 0x020000D8 RID: 216
	public static class ES3Reflection
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0001BD88 File Offset: 0x00019F88
		private static Assembly[] Assemblies
		{
			get
			{
				if (ES3Reflection._assemblies == null)
				{
					string[] assemblyNames = new ES3Settings(null, null).assemblyNames;
					List<Assembly> list = new List<Assembly>();
					foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
					{
						try
						{
							if (assemblyNames.Contains(assembly.GetName().Name))
							{
								list.Add(assembly);
							}
						}
						catch
						{
						}
					}
					ES3Reflection._assemblies = list.ToArray();
				}
				return ES3Reflection._assemblies;
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001BE10 File Offset: 0x0001A010
		public static Type[] GetElementTypes(Type type)
		{
			if (ES3Reflection.IsGenericType(type))
			{
				return ES3Reflection.GetGenericArguments(type);
			}
			if (type.IsArray)
			{
				return new Type[]
				{
					ES3Reflection.GetElementType(type)
				};
			}
			return null;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0001BE3C File Offset: 0x0001A03C
		public static List<FieldInfo> GetSerializableFields(Type type, List<FieldInfo> serializableFields = null, bool safe = true, string[] memberNames = null, BindingFlags bindings = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
		{
			if (type == null)
			{
				return new List<FieldInfo>();
			}
			FieldInfo[] fields = type.GetFields(bindings);
			if (serializableFields == null)
			{
				serializableFields = new List<FieldInfo>();
			}
			foreach (FieldInfo fieldInfo in fields)
			{
				string name = fieldInfo.Name;
				if (memberNames == null || memberNames.Contains(name))
				{
					Type fieldType = fieldInfo.FieldType;
					if (ES3Reflection.AttributeIsDefined(fieldInfo, ES3Reflection.es3SerializableAttributeType))
					{
						serializableFields.Add(fieldInfo);
					}
					else if (!ES3Reflection.AttributeIsDefined(fieldInfo, ES3Reflection.es3NonSerializableAttributeType) && (!safe || fieldInfo.IsPublic || ES3Reflection.AttributeIsDefined(fieldInfo, ES3Reflection.serializeFieldAttributeType)) && !fieldInfo.IsLiteral && !fieldInfo.IsInitOnly && (!(fieldType == type) || ES3Reflection.IsAssignableFrom(typeof(UnityEngine.Object), fieldType)) && !ES3Reflection.AttributeIsDefined(fieldInfo, ES3Reflection.nonSerializedAttributeType) && !ES3Reflection.AttributeIsDefined(fieldInfo, ES3Reflection.obsoleteAttributeType) && ES3Reflection.TypeIsSerializable(fieldInfo.FieldType) && (!safe || !name.StartsWith("m_") || fieldInfo.DeclaringType.Namespace == null || !fieldInfo.DeclaringType.Namespace.Contains("UnityEngine")))
					{
						serializableFields.Add(fieldInfo);
					}
				}
			}
			Type left = ES3Reflection.BaseType(type);
			if (left != null && left != typeof(object) && left != typeof(UnityEngine.Object))
			{
				ES3Reflection.GetSerializableFields(ES3Reflection.BaseType(type), serializableFields, safe, memberNames, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return serializableFields;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001BFC4 File Offset: 0x0001A1C4
		public static List<PropertyInfo> GetSerializableProperties(Type type, List<PropertyInfo> serializableProperties = null, bool safe = true, string[] memberNames = null, BindingFlags bindings = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
		{
			bool flag = ES3Reflection.IsAssignableFrom(typeof(Component), type);
			if (!safe)
			{
				bindings |= BindingFlags.NonPublic;
			}
			PropertyInfo[] properties = type.GetProperties(bindings);
			if (serializableProperties == null)
			{
				serializableProperties = new List<PropertyInfo>();
			}
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (ES3Reflection.AttributeIsDefined(propertyInfo, ES3Reflection.es3SerializableAttributeType))
				{
					serializableProperties.Add(propertyInfo);
				}
				else if (!ES3Reflection.AttributeIsDefined(propertyInfo, ES3Reflection.es3NonSerializableAttributeType))
				{
					string name = propertyInfo.Name;
					if (!ES3Reflection.excludedPropertyNames.Contains(name) && (memberNames == null || memberNames.Contains(name)) && (!safe || ES3Reflection.AttributeIsDefined(propertyInfo, ES3Reflection.serializeFieldAttributeType) || ES3Reflection.AttributeIsDefined(propertyInfo, ES3Reflection.es3SerializableAttributeType)))
					{
						Type propertyType = propertyInfo.PropertyType;
						if ((!(propertyType == type) || ES3Reflection.IsAssignableFrom(typeof(UnityEngine.Object), propertyType)) && propertyInfo.CanRead && propertyInfo.CanWrite && (propertyInfo.GetIndexParameters().Length == 0 || propertyType.IsArray) && ES3Reflection.TypeIsSerializable(propertyType) && (!flag || (!(name == "tag") && !(name == "name"))) && !ES3Reflection.AttributeIsDefined(propertyInfo, ES3Reflection.obsoleteAttributeType) && !ES3Reflection.AttributeIsDefined(propertyInfo, ES3Reflection.nonSerializedAttributeType))
						{
							serializableProperties.Add(propertyInfo);
						}
					}
				}
			}
			Type type2 = ES3Reflection.BaseType(type);
			if (type2 != null && type2 != typeof(object))
			{
				ES3Reflection.GetSerializableProperties(type2, serializableProperties, safe, memberNames, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return serializableProperties;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001C158 File Offset: 0x0001A358
		public static bool TypeIsSerializable(Type type)
		{
			if (type == null)
			{
				return false;
			}
			if (ES3Reflection.AttributeIsDefined(type, ES3Reflection.es3NonSerializableAttributeType))
			{
				return false;
			}
			if (ES3Reflection.IsPrimitive(type) || ES3Reflection.IsValueType(type) || ES3Reflection.IsAssignableFrom(typeof(Component), type) || ES3Reflection.IsAssignableFrom(typeof(ScriptableObject), type))
			{
				return true;
			}
			ES3Type orCreateES3Type = ES3TypeMgr.GetOrCreateES3Type(type, false);
			if (orCreateES3Type != null && !orCreateES3Type.isUnsupported)
			{
				return true;
			}
			if (ES3Reflection.TypeIsArray(type))
			{
				return ES3Reflection.TypeIsSerializable(type.GetElementType());
			}
			Type[] genericArguments = type.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (!ES3Reflection.TypeIsSerializable(genericArguments[i]))
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001C208 File Offset: 0x0001A408
		public static object CreateInstance(Type type)
		{
			if (ES3Reflection.IsAssignableFrom(typeof(Component), type))
			{
				return ES3ComponentType.CreateComponent(type);
			}
			if (ES3Reflection.IsAssignableFrom(typeof(ScriptableObject), type))
			{
				return ScriptableObject.CreateInstance(type);
			}
			if (ES3Reflection.HasParameterlessConstructor(type))
			{
				return Activator.CreateInstance(type);
			}
			return FormatterServices.GetUninitializedObject(type);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001C25C File Offset: 0x0001A45C
		public static object CreateInstance(Type type, params object[] args)
		{
			if (ES3Reflection.IsAssignableFrom(typeof(Component), type))
			{
				return ES3ComponentType.CreateComponent(type);
			}
			if (ES3Reflection.IsAssignableFrom(typeof(ScriptableObject), type))
			{
				return ScriptableObject.CreateInstance(type);
			}
			return Activator.CreateInstance(type, args);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001C297 File Offset: 0x0001A497
		public static Array ArrayCreateInstance(Type type, int length)
		{
			return Array.CreateInstance(type, new int[]
			{
				length
			});
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001C2A9 File Offset: 0x0001A4A9
		public static Array ArrayCreateInstance(Type type, int[] dimensions)
		{
			return Array.CreateInstance(type, dimensions);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001C2B2 File Offset: 0x0001A4B2
		public static Type MakeGenericType(Type type, Type genericParam)
		{
			return type.MakeGenericType(new Type[]
			{
				genericParam
			});
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001C2C4 File Offset: 0x0001A4C4
		public static ES3Reflection.ES3ReflectedMember[] GetSerializableMembers(Type type, bool safe = true, string[] memberNames = null)
		{
			if (type == null)
			{
				return new ES3Reflection.ES3ReflectedMember[0];
			}
			List<FieldInfo> serializableFields = ES3Reflection.GetSerializableFields(type, new List<FieldInfo>(), safe, memberNames, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			List<PropertyInfo> serializableProperties = ES3Reflection.GetSerializableProperties(type, new List<PropertyInfo>(), safe, memberNames, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			ES3Reflection.ES3ReflectedMember[] array = new ES3Reflection.ES3ReflectedMember[serializableFields.Count + serializableProperties.Count];
			for (int i = 0; i < serializableFields.Count; i++)
			{
				array[i] = new ES3Reflection.ES3ReflectedMember(serializableFields[i]);
			}
			for (int j = 0; j < serializableProperties.Count; j++)
			{
				array[j + serializableFields.Count] = new ES3Reflection.ES3ReflectedMember(serializableProperties[j]);
			}
			return array;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001C36A File Offset: 0x0001A56A
		public static ES3Reflection.ES3ReflectedMember GetES3ReflectedProperty(Type type, string propertyName)
		{
			return new ES3Reflection.ES3ReflectedMember(ES3Reflection.GetProperty(type, propertyName));
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001C378 File Offset: 0x0001A578
		public static ES3Reflection.ES3ReflectedMember GetES3ReflectedMember(Type type, string fieldName)
		{
			return new ES3Reflection.ES3ReflectedMember(ES3Reflection.GetField(type, fieldName));
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001C388 File Offset: 0x0001A588
		public static IList<T> GetInstances<T>()
		{
			List<T> list = new List<T>();
			Assembly[] assemblies = ES3Reflection.Assemblies;
			for (int i = 0; i < assemblies.Length; i++)
			{
				foreach (Type type in assemblies[i].GetTypes())
				{
					if (ES3Reflection.IsAssignableFrom(typeof(T), type) && ES3Reflection.HasParameterlessConstructor(type) && !ES3Reflection.IsAbstract(type))
					{
						list.Add((T)((object)Activator.CreateInstance(type)));
					}
				}
			}
			return list;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001C408 File Offset: 0x0001A608
		public static IList<Type> GetDerivedTypes(Type derivedType)
		{
			return (from assembly in ES3Reflection.Assemblies
			from type in assembly.GetTypes()
			where ES3Reflection.IsAssignableFrom(derivedType, type)
			select type).ToList<Type>();
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001C4A4 File Offset: 0x0001A6A4
		public static bool IsAssignableFrom(Type a, Type b)
		{
			return a.IsAssignableFrom(b);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001C4AD File Offset: 0x0001A6AD
		public static Type GetGenericTypeDefinition(Type type)
		{
			return type.GetGenericTypeDefinition();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001C4B5 File Offset: 0x0001A6B5
		public static Type[] GetGenericArguments(Type type)
		{
			return type.GetGenericArguments();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001C4BD File Offset: 0x0001A6BD
		public static int GetArrayRank(Type type)
		{
			return type.GetArrayRank();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001C4C5 File Offset: 0x0001A6C5
		public static string GetAssemblyQualifiedName(Type type)
		{
			return type.AssemblyQualifiedName;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001C4CD File Offset: 0x0001A6CD
		public static ES3Reflection.ES3ReflectedMethod GetMethod(Type type, string methodName, Type[] genericParameters, Type[] parameterTypes)
		{
			return new ES3Reflection.ES3ReflectedMethod(type, methodName, genericParameters, parameterTypes);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001C4D8 File Offset: 0x0001A6D8
		public static bool TypeIsArray(Type type)
		{
			return type.IsArray;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001C4E0 File Offset: 0x0001A6E0
		public static Type GetElementType(Type type)
		{
			return type.GetElementType();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001C4E8 File Offset: 0x0001A6E8
		public static bool IsAbstract(Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001C4F0 File Offset: 0x0001A6F0
		public static bool IsInterface(Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001C4F8 File Offset: 0x0001A6F8
		public static bool IsGenericType(Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001C500 File Offset: 0x0001A700
		public static bool IsValueType(Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001C508 File Offset: 0x0001A708
		public static bool IsEnum(Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001C510 File Offset: 0x0001A710
		public static bool HasParameterlessConstructor(Type type)
		{
			return ES3Reflection.IsValueType(type) || ES3Reflection.GetParameterlessConstructor(type) != null;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001C52C File Offset: 0x0001A72C
		public static ConstructorInfo GetParameterlessConstructor(Type type)
		{
			foreach (ConstructorInfo constructorInfo in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (constructorInfo.GetParameters().Length == 0)
				{
					return constructorInfo;
				}
			}
			return null;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0001C560 File Offset: 0x0001A760
		public static string GetShortAssemblyQualifiedName(Type type)
		{
			if (ES3Reflection.IsPrimitive(type))
			{
				return type.ToString();
			}
			return type.FullName + "," + type.Assembly.GetName().Name;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001C594 File Offset: 0x0001A794
		public static PropertyInfo GetProperty(Type type, string propertyName)
		{
			PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null && ES3Reflection.BaseType(type) != typeof(object))
			{
				return ES3Reflection.GetProperty(ES3Reflection.BaseType(type), propertyName);
			}
			return property;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001C5DC File Offset: 0x0001A7DC
		public static FieldInfo GetField(Type type, string fieldName)
		{
			FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null && ES3Reflection.BaseType(type) != typeof(object))
			{
				return ES3Reflection.GetField(ES3Reflection.BaseType(type), fieldName);
			}
			return field;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001C624 File Offset: 0x0001A824
		public static MethodInfo[] GetMethods(Type type, string methodName)
		{
			return (from t in type.GetMethods()
			where t.Name == methodName
			select t).ToArray<MethodInfo>();
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001C65A File Offset: 0x0001A85A
		public static bool IsPrimitive(Type type)
		{
			return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001C688 File Offset: 0x0001A888
		public static bool AttributeIsDefined(MemberInfo info, Type attributeType)
		{
			return Attribute.IsDefined(info, attributeType, true);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001C692 File Offset: 0x0001A892
		public static bool AttributeIsDefined(Type type, Type attributeType)
		{
			return type.IsDefined(attributeType, true);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001C69C File Offset: 0x0001A89C
		public static bool ImplementsInterface(Type type, Type interfaceType)
		{
			return type.GetInterface(interfaceType.Name) != null;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001C6B0 File Offset: 0x0001A8B0
		public static Type BaseType(Type type)
		{
			return type.BaseType;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001C6B8 File Offset: 0x0001A8B8
		public static Type GetType(string typeString)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(typeString);
			if (num <= 2667225454U)
			{
				if (num <= 1625787317U)
				{
					if (num <= 520654156U)
					{
						if (num != 356760993U)
						{
							if (num != 398550328U)
							{
								if (num == 520654156U)
								{
									if (typeString == "decimal")
									{
										return typeof(decimal);
									}
								}
							}
							else if (typeString == "string")
							{
								return typeof(string);
							}
						}
						else if (typeString == "UnityEngine.Object")
						{
							return typeof(UnityEngine.Object);
						}
					}
					else if (num != 718440320U)
					{
						if (num != 1416355490U)
						{
							if (num == 1625787317U)
							{
								if (typeString == "System.Object")
								{
									return typeof(object);
								}
							}
						}
						else if (typeString == "Texture2D")
						{
							return typeof(Texture2D);
						}
					}
					else if (typeString == "Component")
					{
						return typeof(Component);
					}
				}
				else if (num <= 2197844016U)
				{
					if (num != 1630192034U)
					{
						if (num != 1683620383U)
						{
							if (num == 2197844016U)
							{
								if (typeString == "Vector2")
								{
									return typeof(Vector2);
								}
							}
						}
						else if (typeString == "byte")
						{
							return typeof(byte);
						}
					}
					else if (typeString == "ushort")
					{
						return typeof(ushort);
					}
				}
				else if (num <= 2298509730U)
				{
					if (num != 2214621635U)
					{
						if (num == 2298509730U)
						{
							if (typeString == "Vector4")
							{
								return typeof(Vector4);
							}
						}
					}
					else if (typeString == "Vector3")
					{
						return typeof(Vector3);
					}
				}
				else if (num != 2515107422U)
				{
					if (num == 2667225454U)
					{
						if (typeString == "ulong")
						{
							return typeof(ulong);
						}
					}
				}
				else if (typeString == "int")
				{
					return typeof(int);
				}
			}
			else if (num <= 3270303571U)
			{
				if (num <= 2823553821U)
				{
					if (num != 2699759368U)
					{
						if (num != 2797886853U)
						{
							if (num == 2823553821U)
							{
								if (typeString == "char")
								{
									return typeof(char);
								}
							}
						}
						else if (typeString == "float")
						{
							return typeof(float);
						}
					}
					else if (typeString == "double")
					{
						return typeof(double);
					}
				}
				else if (num != 2911022011U)
				{
					if (num != 3122818005U)
					{
						if (num == 3270303571U)
						{
							if (typeString == "long")
							{
								return typeof(long);
							}
						}
					}
					else if (typeString == "short")
					{
						return typeof(short);
					}
				}
				else if (typeString == "Transform")
				{
					return typeof(Transform);
				}
			}
			else if (num <= 3415750305U)
			{
				if (num != 3289806692U)
				{
					if (num != 3365180733U)
					{
						if (num == 3415750305U)
						{
							if (typeString == "uint")
							{
								return typeof(uint);
							}
						}
					}
					else if (typeString == "bool")
					{
						return typeof(bool);
					}
				}
				else if (typeString == "GameObject")
				{
					return typeof(GameObject);
				}
			}
			else if (num <= 3847869726U)
			{
				if (num != 3419754368U)
				{
					if (num == 3847869726U)
					{
						if (typeString == "MeshFilter")
						{
							return typeof(MeshFilter);
						}
					}
				}
				else if (typeString == "Material")
				{
					return typeof(Material);
				}
			}
			else if (num != 3853794552U)
			{
				if (num == 4088464520U)
				{
					if (typeString == "sbyte")
					{
						return typeof(sbyte);
					}
				}
			}
			else if (typeString == "Color")
			{
				return typeof(Color);
			}
			return Type.GetType(typeString);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001CBA8 File Offset: 0x0001ADA8
		public static string GetTypeString(Type type)
		{
			if (type == typeof(bool))
			{
				return "bool";
			}
			if (type == typeof(byte))
			{
				return "byte";
			}
			if (type == typeof(sbyte))
			{
				return "sbyte";
			}
			if (type == typeof(char))
			{
				return "char";
			}
			if (type == typeof(decimal))
			{
				return "decimal";
			}
			if (type == typeof(double))
			{
				return "double";
			}
			if (type == typeof(float))
			{
				return "float";
			}
			if (type == typeof(int))
			{
				return "int";
			}
			if (type == typeof(uint))
			{
				return "uint";
			}
			if (type == typeof(long))
			{
				return "long";
			}
			if (type == typeof(ulong))
			{
				return "ulong";
			}
			if (type == typeof(short))
			{
				return "short";
			}
			if (type == typeof(ushort))
			{
				return "ushort";
			}
			if (type == typeof(string))
			{
				return "string";
			}
			if (type == typeof(Vector2))
			{
				return "Vector2";
			}
			if (type == typeof(Vector3))
			{
				return "Vector3";
			}
			if (type == typeof(Vector4))
			{
				return "Vector4";
			}
			if (type == typeof(Color))
			{
				return "Color";
			}
			if (type == typeof(Transform))
			{
				return "Transform";
			}
			if (type == typeof(Component))
			{
				return "Component";
			}
			if (type == typeof(GameObject))
			{
				return "GameObject";
			}
			if (type == typeof(MeshFilter))
			{
				return "MeshFilter";
			}
			if (type == typeof(Material))
			{
				return "Material";
			}
			if (type == typeof(Texture2D))
			{
				return "Texture2D";
			}
			if (type == typeof(UnityEngine.Object))
			{
				return "UnityEngine.Object";
			}
			if (type == typeof(object))
			{
				return "System.Object";
			}
			return ES3Reflection.GetShortAssemblyQualifiedName(type);
		}

		// Token: 0x0400012F RID: 303
		public const string memberFieldPrefix = "m_";

		// Token: 0x04000130 RID: 304
		public const string componentTagFieldName = "tag";

		// Token: 0x04000131 RID: 305
		public const string componentNameFieldName = "name";

		// Token: 0x04000132 RID: 306
		public static readonly string[] excludedPropertyNames = new string[]
		{
			"runInEditMode",
			"useGUILayout",
			"hideFlags"
		};

		// Token: 0x04000133 RID: 307
		public static readonly Type serializableAttributeType = typeof(SerializableAttribute);

		// Token: 0x04000134 RID: 308
		public static readonly Type serializeFieldAttributeType = typeof(SerializeField);

		// Token: 0x04000135 RID: 309
		public static readonly Type obsoleteAttributeType = typeof(ObsoleteAttribute);

		// Token: 0x04000136 RID: 310
		public static readonly Type nonSerializedAttributeType = typeof(NonSerializedAttribute);

		// Token: 0x04000137 RID: 311
		public static readonly Type es3SerializableAttributeType = typeof(ES3Serializable);

		// Token: 0x04000138 RID: 312
		public static readonly Type es3NonSerializableAttributeType = typeof(ES3NonSerializable);

		// Token: 0x04000139 RID: 313
		public static Type[] EmptyTypes = new Type[0];

		// Token: 0x0400013A RID: 314
		private static Assembly[] _assemblies = null;

		// Token: 0x02000109 RID: 265
		public struct ES3ReflectedMember
		{
			// Token: 0x1700002E RID: 46
			// (get) Token: 0x06000596 RID: 1430 RVA: 0x0001FE03 File Offset: 0x0001E003
			public bool IsNull
			{
				get
				{
					return this.fieldInfo == null && this.propertyInfo == null;
				}
			}

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x06000597 RID: 1431 RVA: 0x0001FE21 File Offset: 0x0001E021
			public string Name
			{
				get
				{
					if (!this.isProperty)
					{
						return this.fieldInfo.Name;
					}
					return this.propertyInfo.Name;
				}
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001FE42 File Offset: 0x0001E042
			public Type MemberType
			{
				get
				{
					if (!this.isProperty)
					{
						return this.fieldInfo.FieldType;
					}
					return this.propertyInfo.PropertyType;
				}
			}

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x06000599 RID: 1433 RVA: 0x0001FE63 File Offset: 0x0001E063
			public bool IsPublic
			{
				get
				{
					if (!this.isProperty)
					{
						return this.fieldInfo.IsPublic;
					}
					return this.propertyInfo.GetGetMethod(true).IsPublic && this.propertyInfo.GetSetMethod(true).IsPublic;
				}
			}

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x0600059A RID: 1434 RVA: 0x0001FE9F File Offset: 0x0001E09F
			public bool IsProtected
			{
				get
				{
					if (!this.isProperty)
					{
						return this.fieldInfo.IsFamily;
					}
					return this.propertyInfo.GetGetMethod(true).IsFamily;
				}
			}

			// Token: 0x17000033 RID: 51
			// (get) Token: 0x0600059B RID: 1435 RVA: 0x0001FEC6 File Offset: 0x0001E0C6
			public bool IsStatic
			{
				get
				{
					if (!this.isProperty)
					{
						return this.fieldInfo.IsStatic;
					}
					return this.propertyInfo.GetGetMethod(true).IsStatic;
				}
			}

			// Token: 0x0600059C RID: 1436 RVA: 0x0001FEF0 File Offset: 0x0001E0F0
			public ES3ReflectedMember(object fieldPropertyInfo)
			{
				if (fieldPropertyInfo == null)
				{
					this.propertyInfo = null;
					this.fieldInfo = null;
					this.isProperty = false;
					return;
				}
				this.isProperty = ES3Reflection.IsAssignableFrom(typeof(PropertyInfo), fieldPropertyInfo.GetType());
				if (this.isProperty)
				{
					this.propertyInfo = (PropertyInfo)fieldPropertyInfo;
					this.fieldInfo = null;
					return;
				}
				this.fieldInfo = (FieldInfo)fieldPropertyInfo;
				this.propertyInfo = null;
			}

			// Token: 0x0600059D RID: 1437 RVA: 0x0001FF60 File Offset: 0x0001E160
			public void SetValue(object obj, object value)
			{
				if (this.isProperty)
				{
					this.propertyInfo.SetValue(obj, value, null);
					return;
				}
				this.fieldInfo.SetValue(obj, value);
			}

			// Token: 0x0600059E RID: 1438 RVA: 0x0001FF86 File Offset: 0x0001E186
			public object GetValue(object obj)
			{
				if (this.isProperty)
				{
					return this.propertyInfo.GetValue(obj, null);
				}
				return this.fieldInfo.GetValue(obj);
			}

			// Token: 0x040001F9 RID: 505
			private FieldInfo fieldInfo;

			// Token: 0x040001FA RID: 506
			private PropertyInfo propertyInfo;

			// Token: 0x040001FB RID: 507
			public bool isProperty;
		}

		// Token: 0x0200010A RID: 266
		public class ES3ReflectedMethod
		{
			// Token: 0x0600059F RID: 1439 RVA: 0x0001FFAC File Offset: 0x0001E1AC
			public ES3ReflectedMethod(Type type, string methodName, Type[] genericParameters, Type[] parameterTypes)
			{
				MethodInfo methodInfo = type.GetMethod(methodName, parameterTypes);
				this.method = methodInfo.MakeGenericMethod(genericParameters);
			}

			// Token: 0x060005A0 RID: 1440 RVA: 0x0001FFD8 File Offset: 0x0001E1D8
			public ES3ReflectedMethod(Type type, string methodName, Type[] genericParameters, Type[] parameterTypes, BindingFlags bindingAttr)
			{
				MethodInfo methodInfo = type.GetMethod(methodName, bindingAttr, null, parameterTypes, null);
				this.method = methodInfo.MakeGenericMethod(genericParameters);
			}

			// Token: 0x060005A1 RID: 1441 RVA: 0x00020006 File Offset: 0x0001E206
			public object Invoke(object obj, object[] parameters = null)
			{
				return this.method.Invoke(obj, parameters);
			}

			// Token: 0x040001FC RID: 508
			private MethodInfo method;
		}
	}
}
