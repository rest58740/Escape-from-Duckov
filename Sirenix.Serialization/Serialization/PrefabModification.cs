using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x0200006E RID: 110
	public sealed class PrefabModification
	{
		// Token: 0x06000386 RID: 902 RVA: 0x00018844 File Offset: 0x00016A44
		public void Apply(Object unityObject)
		{
			if (this.ModificationType == PrefabModificationType.Value)
			{
				this.ApplyValue(unityObject);
				return;
			}
			if (this.ModificationType == PrefabModificationType.ListLength)
			{
				this.ApplyListLength(unityObject);
				return;
			}
			if (this.ModificationType == PrefabModificationType.Dictionary)
			{
				this.ApplyDictionaryModifications(unityObject);
				return;
			}
			throw new NotImplementedException(this.ModificationType.ToString());
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001889C File Offset: 0x00016A9C
		private void ApplyValue(Object unityObject)
		{
			Type type = null;
			if (this.ModifiedValue != null)
			{
				type = this.ModifiedValue.GetType();
			}
			if (type != null && this.ReferencePaths != null && this.ReferencePaths.Count > 0)
			{
				for (int i = 0; i < this.ReferencePaths.Count; i++)
				{
					string path = this.ReferencePaths[i];
					try
					{
						object instanceFromPath = PrefabModification.GetInstanceFromPath(path, unityObject);
						if (instanceFromPath != null && instanceFromPath.GetType() == type)
						{
							this.ModifiedValue = instanceFromPath;
							break;
						}
					}
					catch (Exception)
					{
					}
				}
			}
			PrefabModification.SetInstanceToPath(this.Path, unityObject, this.ModifiedValue);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001894C File Offset: 0x00016B4C
		private void ApplyListLength(Object unityObject)
		{
			object instanceFromPath = PrefabModification.GetInstanceFromPath(this.Path, unityObject);
			if (instanceFromPath == null)
			{
				return;
			}
			Type type = instanceFromPath.GetType();
			if (type.IsArray)
			{
				Array array = (Array)instanceFromPath;
				if (this.NewLength == array.Length)
				{
					return;
				}
				Array array2 = Array.CreateInstance(type.GetElementType(), this.NewLength);
				if (this.NewLength > array.Length)
				{
					Array.Copy(array, 0, array2, 0, array.Length);
					PrefabModification.ReplaceAllReferencesInGraph(unityObject, array, array2, null);
					return;
				}
				Array.Copy(array, 0, array2, 0, array2.Length);
				PrefabModification.ReplaceAllReferencesInGraph(unityObject, array, array2, null);
				return;
			}
			else
			{
				if (typeof(IList).IsAssignableFrom(type))
				{
					IList list = (IList)instanceFromPath;
					Type type2 = type.ImplementsOpenGenericInterface(typeof(IList)) ? type.GetArgumentsOfInheritedOpenGenericInterface(typeof(IList))[0] : null;
					bool flag = type2 != null && type2.IsValueType;
					int num = 0;
					while (list.Count < this.NewLength)
					{
						if (flag)
						{
							list.Add(Activator.CreateInstance(type2));
						}
						else
						{
							list.Add(null);
						}
						num++;
					}
					while (list.Count > this.NewLength)
					{
						list.RemoveAt(list.Count - 1);
					}
					return;
				}
				if (type.ImplementsOpenGenericInterface(typeof(IList)))
				{
					Type type3 = type.GetArgumentsOfInheritedOpenGenericInterface(typeof(IList))[0];
					Type type4 = typeof(ICollection).MakeGenericType(new Type[]
					{
						type3
					});
					bool isValueType = type3.IsValueType;
					PropertyInfo property = type4.GetProperty("Count");
					int num2 = (int)property.GetValue(instanceFromPath, null);
					if (num2 < this.NewLength)
					{
						int num3 = this.NewLength - num2;
						MethodInfo method = type4.GetMethod("Add");
						for (int i = 0; i < num3; i++)
						{
							if (isValueType)
							{
								method.Invoke(instanceFromPath, new object[]
								{
									Activator.CreateInstance(type3)
								});
							}
							else
							{
								method.Invoke(instanceFromPath, new object[1]);
							}
							num2++;
						}
						return;
					}
					if (num2 > this.NewLength)
					{
						int num4 = num2 - this.NewLength;
						Type type5 = typeof(IList).MakeGenericType(new Type[]
						{
							type3
						});
						MethodInfo method2 = type5.GetMethod("RemoveAt");
						for (int j = 0; j < num4; j++)
						{
							method2.Invoke(instanceFromPath, new object[]
							{
								num2 - (num4 + 1)
							});
						}
					}
				}
				return;
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00018BDC File Offset: 0x00016DDC
		private void ApplyDictionaryModifications(Object unityObject)
		{
			object instanceFromPath = PrefabModification.GetInstanceFromPath(this.Path, unityObject);
			if (instanceFromPath == null)
			{
				return;
			}
			Type type = instanceFromPath.GetType();
			if (!type.ImplementsOpenGenericInterface(typeof(IDictionary)))
			{
				return;
			}
			Type[] argumentsOfInheritedOpenGenericInterface = type.GetArgumentsOfInheritedOpenGenericInterface(typeof(IDictionary));
			Type type2 = typeof(IDictionary).MakeGenericType(argumentsOfInheritedOpenGenericInterface);
			if (this.DictionaryKeysRemoved != null && this.DictionaryKeysRemoved.Length != 0)
			{
				MethodInfo method = type2.GetMethod("Remove", new Type[]
				{
					argumentsOfInheritedOpenGenericInterface[0]
				});
				object[] array = new object[1];
				for (int i = 0; i < this.DictionaryKeysRemoved.Length; i++)
				{
					array[0] = this.DictionaryKeysRemoved[i];
					if (array[0] != null && argumentsOfInheritedOpenGenericInterface[0].IsAssignableFrom(array[0].GetType()))
					{
						method.Invoke(instanceFromPath, array);
					}
				}
			}
			if (this.DictionaryKeysAdded != null && this.DictionaryKeysAdded.Length != 0)
			{
				MethodInfo method2 = type2.GetMethod("set_Item", argumentsOfInheritedOpenGenericInterface);
				object[] array2 = new object[]
				{
					default(object),
					argumentsOfInheritedOpenGenericInterface[1].IsValueType ? Activator.CreateInstance(argumentsOfInheritedOpenGenericInterface[1]) : null
				};
				for (int j = 0; j < this.DictionaryKeysAdded.Length; j++)
				{
					array2[0] = this.DictionaryKeysAdded[j];
					if (array2[0] != null && argumentsOfInheritedOpenGenericInterface[0].IsAssignableFrom(array2[0].GetType()))
					{
						method2.Invoke(instanceFromPath, array2);
					}
				}
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00018D3C File Offset: 0x00016F3C
		private static void ReplaceAllReferencesInGraph(object graph, object oldReference, object newReference, HashSet<object> processedReferences = null)
		{
			if (processedReferences == null)
			{
				processedReferences = new HashSet<object>(ReferenceEqualityComparer<object>.Default);
			}
			processedReferences.Add(graph);
			if (graph.GetType().IsArray)
			{
				Array array = (Array)graph;
				for (int i = 0; i < array.Length; i++)
				{
					object obj = array.GetValue(i);
					if (obj != null)
					{
						if (obj == oldReference)
						{
							array.SetValue(newReference, i);
							obj = newReference;
						}
						if (!processedReferences.Contains(obj))
						{
							PrefabModification.ReplaceAllReferencesInGraph(obj, oldReference, newReference, processedReferences);
						}
					}
				}
				return;
			}
			foreach (FieldInfo fieldInfo in FormatterUtilities.GetSerializableMembers(graph.GetType(), SerializationPolicies.Everything))
			{
				if (!fieldInfo.FieldType.IsPrimitive && !(fieldInfo.FieldType == typeof(SerializationData)) && !(fieldInfo.FieldType == typeof(string)))
				{
					object obj2 = fieldInfo.GetValue(graph);
					if (obj2 != null)
					{
						Type type = obj2.GetType();
						if (!type.IsPrimitive && !(type == typeof(SerializationData)) && !(type == typeof(string)))
						{
							if (obj2 == oldReference)
							{
								fieldInfo.SetValue(graph, newReference);
								obj2 = newReference;
							}
							if (!processedReferences.Contains(obj2))
							{
								PrefabModification.ReplaceAllReferencesInGraph(obj2, oldReference, newReference, processedReferences);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00018E94 File Offset: 0x00017094
		private static object GetInstanceFromPath(string path, object instance)
		{
			string[] array = path.Split(new char[]
			{
				'.'
			});
			object obj = instance;
			for (int i = 0; i < array.Length; i++)
			{
				obj = PrefabModification.GetInstanceOfStep(array[i], obj);
				if (obj == null)
				{
					return null;
				}
			}
			return obj;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00018ED4 File Offset: 0x000170D4
		private static object GetInstanceOfStep(string step, object instance)
		{
			Type type = instance.GetType();
			if (step.StartsWith("[", 2) && step.EndsWith("]", 2))
			{
				string text = step.Substring(1, step.Length - 2);
				int num;
				if (!int.TryParse(text, ref num))
				{
					throw new ArgumentException("Couldn't parse an index from the path step '" + step + "'.");
				}
				if (type.IsArray)
				{
					Array array = (Array)instance;
					if (num < 0 || num >= array.Length)
					{
						return null;
					}
					return array.GetValue(num);
				}
				else if (typeof(IList).IsAssignableFrom(type))
				{
					IList list = (IList)instance;
					if (num < 0 || num >= list.Count)
					{
						return null;
					}
					return list[num];
				}
				else
				{
					if (!type.ImplementsOpenGenericInterface(typeof(IList)))
					{
						goto IL_27C;
					}
					Type type2 = type.GetArgumentsOfInheritedOpenGenericInterface(typeof(IList))[0];
					Type type3 = typeof(IList).MakeGenericType(new Type[]
					{
						type2
					});
					MethodInfo method = type3.GetMethod("get_Item", 52);
					try
					{
						return method.Invoke(instance, new object[]
						{
							num
						});
					}
					catch (Exception)
					{
						return null;
					}
				}
			}
			if (step.StartsWith("{", 3) && step.EndsWith("}", 3))
			{
				if (!type.ImplementsOpenGenericInterface(typeof(IDictionary)))
				{
					goto IL_27C;
				}
				Type[] argumentsOfInheritedOpenGenericInterface = type.GetArgumentsOfInheritedOpenGenericInterface(typeof(IDictionary));
				object dictionaryKeyValue = DictionaryKeyUtility.GetDictionaryKeyValue(step, argumentsOfInheritedOpenGenericInterface[0]);
				Type type4 = typeof(IDictionary).MakeGenericType(argumentsOfInheritedOpenGenericInterface);
				MethodInfo method2 = type4.GetMethod("get_Item", 52);
				try
				{
					return method2.Invoke(instance, new object[]
					{
						dictionaryKeyValue
					});
				}
				catch (Exception)
				{
					return null;
				}
			}
			string text2 = null;
			int num2 = step.IndexOf('+');
			if (num2 >= 0)
			{
				text2 = step.Substring(0, num2);
				step = step.Substring(num2 + 1);
			}
			IEnumerable<MemberInfo> enumerable = from n in type.GetAllMembers(52)
			where n is FieldInfo || n is PropertyInfo
			select n;
			foreach (MemberInfo memberInfo in enumerable)
			{
				if (memberInfo.Name == step && (text2 == null || !(memberInfo.DeclaringType.Name != text2)))
				{
					return memberInfo.GetMemberValue(instance);
				}
			}
			IL_27C:
			return null;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001918C File Offset: 0x0001738C
		private static void SetInstanceToPath(string path, object instance, object value)
		{
			string[] steps = path.Split(new char[]
			{
				'.'
			});
			bool flag;
			PrefabModification.SetInstanceToPath(path, steps, 0, instance, value, out flag);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000191B8 File Offset: 0x000173B8
		private static void SetInstanceToPath(string path, string[] steps, int index, object instance, object value, out bool setParentInstance)
		{
			setParentInstance = false;
			if (index < steps.Length - 1)
			{
				object instanceOfStep = PrefabModification.GetInstanceOfStep(steps[index], instance);
				if (instanceOfStep == null)
				{
					return;
				}
				PrefabModification.SetInstanceToPath(path, steps, index + 1, instanceOfStep, value, out setParentInstance);
				if (setParentInstance)
				{
					PrefabModification.TrySetInstanceOfStep(steps[index], instance, instanceOfStep, out setParentInstance);
					return;
				}
			}
			else
			{
				PrefabModification.TrySetInstanceOfStep(steps[index], instance, value, out setParentInstance);
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00019210 File Offset: 0x00017410
		private static bool TrySetInstanceOfStep(string step, object instance, object value, out bool setParentInstance)
		{
			setParentInstance = false;
			bool result;
			try
			{
				Type type = instance.GetType();
				if (step.StartsWith("[", 2) && step.EndsWith("]", 2))
				{
					string text = step.Substring(1, step.Length - 2);
					int num;
					if (!int.TryParse(text, ref num))
					{
						throw new ArgumentException("Couldn't parse an index from the path step '" + step + "'.");
					}
					if (type.IsArray)
					{
						Array array = (Array)instance;
						if (num < 0 || num >= array.Length)
						{
							return false;
						}
						array.SetValue(value, num);
						return true;
					}
					else if (typeof(IList).IsAssignableFrom(type))
					{
						IList list = (IList)instance;
						if (num < 0 || num >= list.Count)
						{
							return false;
						}
						list[num] = value;
						return true;
					}
					else if (type.ImplementsOpenGenericInterface(typeof(IList)))
					{
						Type type2 = type.GetArgumentsOfInheritedOpenGenericInterface(typeof(IList))[0];
						Type type3 = typeof(IList).MakeGenericType(new Type[]
						{
							type2
						});
						MethodInfo method = type3.GetMethod("set_Item", 52);
						method.Invoke(instance, new object[]
						{
							num,
							value
						});
						return true;
					}
				}
				else if (step.StartsWith("{", 2) && step.EndsWith("}", 2))
				{
					if (type.ImplementsOpenGenericInterface(typeof(IDictionary)))
					{
						Type[] argumentsOfInheritedOpenGenericInterface = type.GetArgumentsOfInheritedOpenGenericInterface(typeof(IDictionary));
						object dictionaryKeyValue = DictionaryKeyUtility.GetDictionaryKeyValue(step, argumentsOfInheritedOpenGenericInterface[0]);
						Type type4 = typeof(IDictionary).MakeGenericType(argumentsOfInheritedOpenGenericInterface);
						MethodInfo method2 = type4.GetMethod("ContainsKey", 52);
						MethodInfo method3 = type4.GetMethod("set_Item", 52);
						if (!(bool)method2.Invoke(instance, new object[]
						{
							dictionaryKeyValue
						}))
						{
							return false;
						}
						method3.Invoke(instance, new object[]
						{
							dictionaryKeyValue,
							value
						});
					}
				}
				else
				{
					string text2 = null;
					int num2 = step.IndexOf('+');
					if (num2 >= 0)
					{
						text2 = step.Substring(0, num2);
						step = step.Substring(num2 + 1);
					}
					IEnumerable<MemberInfo> enumerable = from n in type.GetAllMembers(52)
					where n is FieldInfo || n is PropertyInfo
					select n;
					foreach (MemberInfo memberInfo in enumerable)
					{
						if (memberInfo.Name == step && (text2 == null || !(memberInfo.DeclaringType.Name != text2)))
						{
							memberInfo.SetMemberValue(instance, value);
							if (instance.GetType().IsValueType)
							{
								setParentInstance = true;
							}
							return true;
						}
					}
				}
				result = false;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0400013C RID: 316
		public PrefabModificationType ModificationType;

		// Token: 0x0400013D RID: 317
		public string Path;

		// Token: 0x0400013E RID: 318
		public List<string> ReferencePaths;

		// Token: 0x0400013F RID: 319
		public object ModifiedValue;

		// Token: 0x04000140 RID: 320
		public int NewLength;

		// Token: 0x04000141 RID: 321
		public object[] DictionaryKeysAdded;

		// Token: 0x04000142 RID: 322
		public object[] DictionaryKeysRemoved;
	}
}
