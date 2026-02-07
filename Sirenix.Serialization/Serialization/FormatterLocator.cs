using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x0200001B RID: 27
	public static class FormatterLocator
	{
		// Token: 0x0600020B RID: 523 RVA: 0x0000DE14 File Offset: 0x0000C014
		static FormatterLocator()
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					string name = assembly.GetName().Name;
					if (!name.StartsWith("System.") && !name.StartsWith("UnityEngine") && !name.StartsWith("UnityEditor") && !(name == "mscorlib"))
					{
						if ((!(assembly.GetName().Name == "Sirenix.Serialization.AOTGenerated") && !assembly.SafeIsDefined(typeof(EmittedAssemblyAttribute), true)) || !EmitUtilities.CanEmit)
						{
							foreach (object obj in assembly.SafeGetCustomAttributes(typeof(RegisterFormatterAttribute), true))
							{
								RegisterFormatterAttribute registerFormatterAttribute = (RegisterFormatterAttribute)obj;
								if (registerFormatterAttribute.FormatterType.IsClass && !registerFormatterAttribute.FormatterType.IsAbstract && !(registerFormatterAttribute.FormatterType.GetConstructor(Type.EmptyTypes) == null) && registerFormatterAttribute.FormatterType.ImplementsOpenGenericInterface(typeof(IFormatter<>)))
								{
									FormatterLocator.FormatterInfos.Add(new FormatterLocator.FormatterInfo
									{
										FormatterType = registerFormatterAttribute.FormatterType,
										WeakFallbackType = registerFormatterAttribute.WeakFallback,
										TargetType = registerFormatterAttribute.FormatterType.GetArgumentsOfInheritedOpenGenericInterface(typeof(IFormatter<>))[0],
										AskIfCanFormatTypes = typeof(IAskIfCanFormatTypes).IsAssignableFrom(registerFormatterAttribute.FormatterType),
										Priority = registerFormatterAttribute.Priority
									});
								}
							}
							foreach (object obj2 in assembly.SafeGetCustomAttributes(typeof(RegisterFormatterLocatorAttribute), true))
							{
								RegisterFormatterLocatorAttribute registerFormatterLocatorAttribute = (RegisterFormatterLocatorAttribute)obj2;
								if (registerFormatterLocatorAttribute.FormatterLocatorType.IsClass && !registerFormatterLocatorAttribute.FormatterLocatorType.IsAbstract && !(registerFormatterLocatorAttribute.FormatterLocatorType.GetConstructor(Type.EmptyTypes) == null) && typeof(IFormatterLocator).IsAssignableFrom(registerFormatterLocatorAttribute.FormatterLocatorType))
								{
									try
									{
										FormatterLocator.FormatterLocators.Add(new FormatterLocator.FormatterLocatorInfo
										{
											LocatorInstance = (IFormatterLocator)Activator.CreateInstance(registerFormatterLocatorAttribute.FormatterLocatorType),
											Priority = registerFormatterLocatorAttribute.Priority
										});
									}
									catch (Exception ex)
									{
										Debug.LogException(new Exception("Exception was thrown while instantiating FormatterLocator of type " + registerFormatterLocatorAttribute.FormatterLocatorType.FullName + ".", ex));
									}
								}
							}
						}
					}
				}
				catch (TypeLoadException)
				{
					if (assembly.GetName().Name == "OdinSerializer")
					{
						Debug.LogError("A TypeLoadException occurred when FormatterLocator tried to load types from assembly '" + assembly.FullName + "'. No serialization formatters in this assembly will be found. Serialization will be utterly broken.");
					}
				}
				catch (ReflectionTypeLoadException)
				{
					if (assembly.GetName().Name == "OdinSerializer")
					{
						Debug.LogError("A ReflectionTypeLoadException occurred when FormatterLocator tried to load types from assembly '" + assembly.FullName + "'. No serialization formatters in this assembly will be found. Serialization will be utterly broken.");
					}
				}
				catch (MissingMemberException)
				{
					if (assembly.GetName().Name == "OdinSerializer")
					{
						Debug.LogError("A ReflectionTypeLoadException occurred when FormatterLocator tried to load types from assembly '" + assembly.FullName + "'. No serialization formatters in this assembly will be found. Serialization will be utterly broken.");
					}
				}
			}
			FormatterLocator.FormatterInfos.Sort(delegate(FormatterLocator.FormatterInfo a, FormatterLocator.FormatterInfo b)
			{
				int num = -a.Priority.CompareTo(b.Priority);
				if (num == 0)
				{
					num = a.FormatterType.Name.CompareTo(b.FormatterType.Name);
				}
				return num;
			});
			FormatterLocator.FormatterLocators.Sort(delegate(FormatterLocator.FormatterLocatorInfo a, FormatterLocator.FormatterLocatorInfo b)
			{
				int num = -a.Priority.CompareTo(b.Priority);
				if (num == 0)
				{
					num = a.LocatorInstance.GetType().Name.CompareTo(b.LocatorInstance.GetType().Name);
				}
				return num;
			});
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600020C RID: 524 RVA: 0x0000E260 File Offset: 0x0000C460
		// (remove) Token: 0x0600020D RID: 525 RVA: 0x0000E260 File Offset: 0x0000C460
		[Obsolete("Use the new IFormatterLocator interface instead, and register your custom locator with the RegisterFormatterLocator assembly attribute.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static event Func<Type, IFormatter> FormatterResolve
		{
			add
			{
				throw new NotSupportedException();
			}
			remove
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000E267 File Offset: 0x0000C467
		public static IFormatter<T> GetFormatter<T>(ISerializationPolicy policy)
		{
			return (IFormatter<T>)FormatterLocator.GetFormatter(typeof(T), policy, false);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000E27F File Offset: 0x0000C47F
		public static IFormatter GetFormatter(Type type, ISerializationPolicy policy)
		{
			return FormatterLocator.GetFormatter(type, policy, true);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000E28C File Offset: 0x0000C48C
		public static IFormatter GetFormatter(Type type, ISerializationPolicy policy, bool allowWeakFallbackFormatters)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (policy == null)
			{
				policy = SerializationPolicies.Strict;
			}
			object obj = allowWeakFallbackFormatters ? FormatterLocator.WeakFormatters_LOCK : FormatterLocator.StrongFormatters_LOCK;
			DoubleLookupDictionary<Type, ISerializationPolicy, IFormatter> doubleLookupDictionary = allowWeakFallbackFormatters ? FormatterLocator.WeakTypeFormatterMap : FormatterLocator.StrongTypeFormatterMap;
			object obj2 = obj;
			IFormatter formatter;
			lock (obj2)
			{
				if (!doubleLookupDictionary.TryGetInnerValue(type, policy, out formatter))
				{
					try
					{
						formatter = FormatterLocator.CreateFormatter(type, policy, allowWeakFallbackFormatters);
					}
					catch (TargetInvocationException ex)
					{
						if (!(ex.GetBaseException() is ExecutionEngineException))
						{
							throw ex;
						}
						FormatterLocator.LogAOTError(type, ex.GetBaseException() as ExecutionEngineException);
					}
					catch (TypeInitializationException ex2)
					{
						if (!(ex2.GetBaseException() is ExecutionEngineException))
						{
							throw ex2;
						}
						FormatterLocator.LogAOTError(type, ex2.GetBaseException() as ExecutionEngineException);
					}
					catch (ExecutionEngineException ex3)
					{
						FormatterLocator.LogAOTError(type, ex3);
					}
					doubleLookupDictionary.AddInner(type, policy, formatter);
				}
			}
			return formatter;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		private static void LogAOTError(Type type, Exception ex)
		{
			string[] array = new List<string>(FormatterLocator.GetAllPossibleMissingAOTTypes(type)).ToArray();
			Debug.LogError(string.Concat(new string[]
			{
				"Creating a serialization formatter for the type '",
				type.GetNiceFullName(),
				"' failed due to missing AOT support. \n\n Please use Odin's AOT generation feature to generate an AOT dll before building, and MAKE SURE that all of the following types were automatically added to the supported types list after a scan (if they were not, please REPORT AN ISSUE with the details of which exact types the scan is missing and ADD THEM MANUALLY): \n\n",
				string.Join("\n", array),
				"\n\nIF ALL THE TYPES ARE IN THE SUPPORT LIST AND YOU STILL GET THIS ERROR, PLEASE REPORT AN ISSUE.The exception contained the following message: \n",
				ex.Message
			}));
			throw new SerializationAbortException("AOT formatter support was missing for type '" + type.GetNiceFullName() + "'.", ex);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000E429 File Offset: 0x0000C629
		private static IEnumerable<string> GetAllPossibleMissingAOTTypes(Type type)
		{
			yield return type.GetNiceFullName() + " (name string: '" + TwoWaySerializationBinder.Default.BindToName(type, null) + "')";
			if (!type.IsGenericType)
			{
				yield break;
			}
			foreach (Type arg in type.GetGenericArguments())
			{
				yield return arg.GetNiceFullName() + " (name string: '" + TwoWaySerializationBinder.Default.BindToName(arg, null) + "')";
				if (arg.IsGenericType)
				{
					foreach (string text in FormatterLocator.GetAllPossibleMissingAOTTypes(arg))
					{
						yield return text;
					}
					IEnumerator<string> enumerator = null;
				}
				arg = null;
			}
			Type[] array = null;
			yield break;
			yield break;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000E43C File Offset: 0x0000C63C
		internal static List<IFormatter> GetAllCompatiblePredefinedFormatters(Type type, ISerializationPolicy policy)
		{
			if (FormatterUtilities.IsPrimitiveType(type))
			{
				throw new ArgumentException("Cannot create formatters for a primitive type like " + type.Name);
			}
			List<IFormatter> list = new List<IFormatter>();
			for (int i = 0; i < FormatterLocator.FormatterLocators.Count; i++)
			{
				try
				{
					IFormatter formatter;
					if (FormatterLocator.FormatterLocators[i].LocatorInstance.TryGetFormatter(type, FormatterLocationStep.BeforeRegisteredFormatters, policy, true, out formatter))
					{
						list.Add(formatter);
					}
				}
				catch (TargetInvocationException ex)
				{
					throw ex;
				}
				catch (TypeInitializationException ex2)
				{
					throw ex2;
				}
				catch (ExecutionEngineException ex3)
				{
					throw ex3;
				}
				catch (Exception ex4)
				{
					Debug.LogException(new Exception("Exception was thrown while calling FormatterLocator " + FormatterLocator.FormatterLocators[i].GetType().FullName + ".", ex4));
				}
			}
			for (int j = 0; j < FormatterLocator.FormatterInfos.Count; j++)
			{
				FormatterLocator.FormatterInfo formatterInfo = FormatterLocator.FormatterInfos[j];
				Type type2 = null;
				if (type == formatterInfo.TargetType)
				{
					type2 = formatterInfo.FormatterType;
				}
				else if (formatterInfo.FormatterType.IsGenericType && formatterInfo.TargetType.IsGenericParameter)
				{
					Type[] array;
					if (formatterInfo.FormatterType.TryInferGenericParameters(out array, new Type[]
					{
						type
					}))
					{
						type2 = formatterInfo.FormatterType.GetGenericTypeDefinition().MakeGenericType(array);
					}
				}
				else if (type.IsGenericType && formatterInfo.FormatterType.IsGenericType && formatterInfo.TargetType.IsGenericType && type.GetGenericTypeDefinition() == formatterInfo.TargetType.GetGenericTypeDefinition())
				{
					Type[] genericArguments = type.GetGenericArguments();
					if (formatterInfo.FormatterType.AreGenericConstraintsSatisfiedBy(genericArguments))
					{
						type2 = formatterInfo.FormatterType.GetGenericTypeDefinition().MakeGenericType(genericArguments);
					}
				}
				if (type2 != null)
				{
					IFormatter formatterInstance = FormatterLocator.GetFormatterInstance(type2);
					if (formatterInstance != null && (!formatterInfo.AskIfCanFormatTypes || ((IAskIfCanFormatTypes)formatterInstance).CanFormatType(type)))
					{
						list.Add(formatterInstance);
					}
				}
			}
			for (int k = 0; k < FormatterLocator.FormatterLocators.Count; k++)
			{
				try
				{
					IFormatter formatter2;
					if (FormatterLocator.FormatterLocators[k].LocatorInstance.TryGetFormatter(type, FormatterLocationStep.AfterRegisteredFormatters, policy, true, out formatter2))
					{
						list.Add(formatter2);
					}
				}
				catch (TargetInvocationException ex5)
				{
					throw ex5;
				}
				catch (TypeInitializationException ex6)
				{
					throw ex6;
				}
				catch (ExecutionEngineException ex7)
				{
					throw ex7;
				}
				catch (Exception ex8)
				{
					Debug.LogException(new Exception("Exception was thrown while calling FormatterLocator " + FormatterLocator.FormatterLocators[k].GetType().FullName + ".", ex8));
				}
			}
			list.Add((IFormatter)Activator.CreateInstance(typeof(ReflectionFormatter<>).MakeGenericType(new Type[]
			{
				type
			})));
			return list;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000E74C File Offset: 0x0000C94C
		private static IFormatter CreateFormatter(Type type, ISerializationPolicy policy, bool allowWeakFormatters)
		{
			if (FormatterUtilities.IsPrimitiveType(type))
			{
				throw new ArgumentException("Cannot create formatters for a primitive type like " + type.Name);
			}
			for (int i = 0; i < FormatterLocator.FormatterLocators.Count; i++)
			{
				try
				{
					IFormatter result;
					if (FormatterLocator.FormatterLocators[i].LocatorInstance.TryGetFormatter(type, FormatterLocationStep.BeforeRegisteredFormatters, policy, allowWeakFormatters, out result))
					{
						return result;
					}
				}
				catch (TargetInvocationException ex)
				{
					throw ex;
				}
				catch (TypeInitializationException ex2)
				{
					throw ex2;
				}
				catch (ExecutionEngineException ex3)
				{
					throw ex3;
				}
				catch (Exception ex4)
				{
					Debug.LogException(new Exception("Exception was thrown while calling FormatterLocator " + FormatterLocator.FormatterLocators[i].GetType().FullName + ".", ex4));
				}
			}
			for (int j = 0; j < FormatterLocator.FormatterInfos.Count; j++)
			{
				FormatterLocator.FormatterInfo formatterInfo = FormatterLocator.FormatterInfos[j];
				Type type2 = null;
				Type type3 = null;
				Type[] array = null;
				if (type == formatterInfo.TargetType)
				{
					type2 = formatterInfo.FormatterType;
				}
				else if (formatterInfo.FormatterType.IsGenericType && formatterInfo.TargetType.IsGenericParameter)
				{
					Type[] array2;
					if (formatterInfo.FormatterType.TryInferGenericParameters(out array2, new Type[]
					{
						type
					}))
					{
						array = array2;
					}
				}
				else if (type.IsGenericType && formatterInfo.FormatterType.IsGenericType && formatterInfo.TargetType.IsGenericType && type.GetGenericTypeDefinition() == formatterInfo.TargetType.GetGenericTypeDefinition())
				{
					Type[] genericArguments = type.GetGenericArguments();
					if (formatterInfo.FormatterType.AreGenericConstraintsSatisfiedBy(genericArguments))
					{
						array = genericArguments;
					}
				}
				if (type2 == null && array != null)
				{
					type2 = formatterInfo.FormatterType.GetGenericTypeDefinition().MakeGenericType(array);
					type3 = formatterInfo.WeakFallbackType;
				}
				if (type2 != null)
				{
					IFormatter formatter = null;
					bool flag = false;
					Exception ex5 = null;
					try
					{
						formatter = FormatterLocator.GetFormatterInstance(type2);
					}
					catch (TargetInvocationException ex6)
					{
						flag = true;
						ex5 = ex6;
					}
					catch (TypeInitializationException ex7)
					{
						flag = true;
						ex5 = ex7;
					}
					catch (ExecutionEngineException ex8)
					{
						flag = true;
						ex5 = ex8;
					}
					if (flag && !EmitUtilities.CanEmit && allowWeakFormatters)
					{
						if (type3 != null)
						{
							formatter = (IFormatter)Activator.CreateInstance(type3, new object[]
							{
								type
							});
						}
						if (formatter == null)
						{
							string text = "";
							for (int k = 0; k < array.Length; k++)
							{
								if (k > 0)
								{
									text += ", ";
								}
								text += array[k].GetNiceFullName();
							}
							Debug.LogError(string.Concat(new string[]
							{
								"No AOT support was generated for serialization formatter type '",
								formatterInfo.FormatterType.GetNiceFullName(),
								"' for the generic arguments <",
								text,
								">, and no weak fallback formatter was specified."
							}));
							throw ex5;
						}
					}
					if (formatter != null && (!formatterInfo.AskIfCanFormatTypes || ((IAskIfCanFormatTypes)formatter).CanFormatType(type)))
					{
						return formatter;
					}
				}
			}
			for (int l = 0; l < FormatterLocator.FormatterLocators.Count; l++)
			{
				try
				{
					IFormatter result2;
					if (FormatterLocator.FormatterLocators[l].LocatorInstance.TryGetFormatter(type, FormatterLocationStep.AfterRegisteredFormatters, policy, allowWeakFormatters, out result2))
					{
						return result2;
					}
				}
				catch (TargetInvocationException ex9)
				{
					throw ex9;
				}
				catch (TypeInitializationException ex10)
				{
					throw ex10;
				}
				catch (ExecutionEngineException ex11)
				{
					throw ex11;
				}
				catch (Exception ex12)
				{
					Debug.LogException(new Exception("Exception was thrown while calling FormatterLocator " + FormatterLocator.FormatterLocators[l].GetType().FullName + ".", ex12));
				}
			}
			if (EmitUtilities.CanEmit)
			{
				IFormatter emittedFormatter = FormatterEmitter.GetEmittedFormatter(type, policy);
				if (emittedFormatter != null)
				{
					return emittedFormatter;
				}
			}
			if (EmitUtilities.CanEmit)
			{
				Debug.LogWarning("Fallback to reflection for type " + type.Name + " when emit is possible on this platform.");
			}
			IFormatter result3;
			try
			{
				result3 = (IFormatter)Activator.CreateInstance(typeof(ReflectionFormatter<>).MakeGenericType(new Type[]
				{
					type
				}));
			}
			catch (TargetInvocationException ex13)
			{
				if (!allowWeakFormatters)
				{
					throw ex13;
				}
				result3 = new WeakReflectionFormatter(type);
			}
			catch (TypeInitializationException ex14)
			{
				if (!allowWeakFormatters)
				{
					throw ex14;
				}
				result3 = new WeakReflectionFormatter(type);
			}
			catch (ExecutionEngineException ex15)
			{
				if (!allowWeakFormatters)
				{
					throw ex15;
				}
				result3 = new WeakReflectionFormatter(type);
			}
			return result3;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000EBF8 File Offset: 0x0000CDF8
		private static IFormatter GetFormatterInstance(Type type)
		{
			IFormatter formatter;
			if (!FormatterLocator.FormatterInstances.TryGetValue(type, ref formatter))
			{
				try
				{
					formatter = (IFormatter)Activator.CreateInstance(type);
					FormatterLocator.FormatterInstances.Add(type, formatter);
				}
				catch (TargetInvocationException ex)
				{
					throw ex;
				}
				catch (TypeInitializationException ex2)
				{
					throw ex2;
				}
				catch (ExecutionEngineException ex3)
				{
					throw ex3;
				}
				catch (Exception ex4)
				{
					Debug.LogException(new Exception("Exception was thrown while instantiating formatter '" + type.GetNiceFullName() + "'.", ex4));
				}
			}
			return formatter;
		}

		// Token: 0x04000085 RID: 133
		private static readonly object StrongFormatters_LOCK = new object();

		// Token: 0x04000086 RID: 134
		private static readonly object WeakFormatters_LOCK = new object();

		// Token: 0x04000087 RID: 135
		private static readonly Dictionary<Type, IFormatter> FormatterInstances = new Dictionary<Type, IFormatter>(FastTypeComparer.Instance);

		// Token: 0x04000088 RID: 136
		private static readonly DoubleLookupDictionary<Type, ISerializationPolicy, IFormatter> StrongTypeFormatterMap = new DoubleLookupDictionary<Type, ISerializationPolicy, IFormatter>(FastTypeComparer.Instance, ReferenceEqualityComparer<ISerializationPolicy>.Default);

		// Token: 0x04000089 RID: 137
		private static readonly DoubleLookupDictionary<Type, ISerializationPolicy, IFormatter> WeakTypeFormatterMap = new DoubleLookupDictionary<Type, ISerializationPolicy, IFormatter>(FastTypeComparer.Instance, ReferenceEqualityComparer<ISerializationPolicy>.Default);

		// Token: 0x0400008A RID: 138
		private static readonly List<FormatterLocator.FormatterLocatorInfo> FormatterLocators = new List<FormatterLocator.FormatterLocatorInfo>();

		// Token: 0x0400008B RID: 139
		private static readonly List<FormatterLocator.FormatterInfo> FormatterInfos = new List<FormatterLocator.FormatterInfo>();

		// Token: 0x020000E0 RID: 224
		private struct FormatterInfo
		{
			// Token: 0x04000252 RID: 594
			public Type FormatterType;

			// Token: 0x04000253 RID: 595
			public Type TargetType;

			// Token: 0x04000254 RID: 596
			public Type WeakFallbackType;

			// Token: 0x04000255 RID: 597
			public bool AskIfCanFormatTypes;

			// Token: 0x04000256 RID: 598
			public int Priority;
		}

		// Token: 0x020000E1 RID: 225
		private struct FormatterLocatorInfo
		{
			// Token: 0x04000257 RID: 599
			public IFormatterLocator LocatorInstance;

			// Token: 0x04000258 RID: 600
			public int Priority;
		}
	}
}
