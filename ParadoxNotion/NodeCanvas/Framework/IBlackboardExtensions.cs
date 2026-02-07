using System;
using System.Collections.Generic;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200002E RID: 46
	[SpoofAOT]
	public static class IBlackboardExtensions
	{
		// Token: 0x060002AC RID: 684 RVA: 0x0000803B File Offset: 0x0000623B
		public static IBlackboard GetRoot(this IBlackboard blackboard)
		{
			if (blackboard.parent == null)
			{
				return blackboard;
			}
			return blackboard.parent.GetRoot();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00008052 File Offset: 0x00006252
		public static IEnumerable<IBlackboard> GetAllParents(this IBlackboard blackboard, bool includeSelf)
		{
			if (blackboard == null)
			{
				yield break;
			}
			if (includeSelf)
			{
				yield return blackboard;
			}
			for (IBlackboard current = blackboard.parent; current != null; current = current.parent)
			{
				yield return current;
			}
			yield break;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00008069 File Offset: 0x00006269
		public static bool IsPartOf(this IBlackboard blackboard, IBlackboard child)
		{
			return blackboard != null && child != null && (blackboard == child || blackboard.IsPartOf(child.parent));
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00008085 File Offset: 0x00006285
		public static Variable<T> AddVariable<T>(this IBlackboard blackboard, string varName, T value)
		{
			Variable<T> variable = blackboard.AddVariable(varName);
			variable.value = value;
			return variable;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00008095 File Offset: 0x00006295
		public static Variable<T> AddVariable<T>(this IBlackboard blackboard, string varName)
		{
			return (Variable<T>)blackboard.AddVariable(varName, typeof(T));
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000080B0 File Offset: 0x000062B0
		public static Variable AddVariable(this IBlackboard blackboard, string varName, object value)
		{
			if (value == null)
			{
				return null;
			}
			Variable variable = blackboard.AddVariable(varName, value.GetType());
			if (variable != null)
			{
				variable.value = value;
			}
			return variable;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000080DC File Offset: 0x000062DC
		public static Variable AddVariable(this IBlackboard blackboard, string varName, Type type)
		{
			Variable variable;
			if (!blackboard.variables.TryGetValue(varName, ref variable))
			{
				Variable variable2 = (Variable)Activator.CreateInstance(typeof(Variable<>).RTMakeGenericType(new Type[]
				{
					type
				}));
				variable2.name = varName;
				blackboard.variables[varName] = variable2;
				blackboard.TryInvokeOnVariableAdded(variable2);
				return variable2;
			}
			if (variable.CanConvertTo(type))
			{
				return variable;
			}
			return null;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00008148 File Offset: 0x00006348
		public static Variable RemoveVariable(this IBlackboard blackboard, string varName)
		{
			Variable variable = null;
			if (blackboard.variables.TryGetValue(varName, ref variable))
			{
				blackboard.variables.Remove(varName);
				blackboard.TryInvokeOnVariableRemoved(variable);
				variable.OnDestroy();
			}
			return variable;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00008184 File Offset: 0x00006384
		public static T GetVariableValue<T>(this IBlackboard blackboard, string varName)
		{
			Variable<T> variable = blackboard.GetVariable(varName);
			if (variable == null)
			{
				return default(T);
			}
			if (variable != null)
			{
				return variable.value;
			}
			try
			{
				return variable.value;
			}
			catch
			{
			}
			return default(T);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000081D8 File Offset: 0x000063D8
		public static Variable SetVariableValue(this IBlackboard blackboard, string varName, object value)
		{
			Variable variable;
			if (!blackboard.variables.TryGetValue(varName, ref variable))
			{
				variable = blackboard.AddVariable(varName, value);
				return variable;
			}
			try
			{
				variable.value = value;
			}
			catch
			{
				return null;
			}
			return variable;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00008224 File Offset: 0x00006424
		public static void InitializePropertiesBinding(this IBlackboard blackboard, Component target, bool callSetter)
		{
			if (blackboard.variables.Count == 0)
			{
				return;
			}
			foreach (Variable variable in blackboard.variables.Values)
			{
				variable.InitializePropertyBinding((target != null) ? target.gameObject : null, callSetter);
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00008294 File Offset: 0x00006494
		public static Variable<T> GetVariable<T>(this IBlackboard blackboard, string varName)
		{
			return (Variable<T>)blackboard.GetVariable(varName, typeof(T));
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000082AC File Offset: 0x000064AC
		public static Variable GetVariable(this IBlackboard blackboard, string varName, Type ofType = null)
		{
			Variable variable;
			if (blackboard.variables != null && varName != null && blackboard.variables.TryGetValue(varName, ref variable) && (ofType == null || ofType == typeof(object) || variable.CanConvertTo(ofType)))
			{
				return variable;
			}
			if (blackboard.parent != null)
			{
				Variable variable2 = blackboard.parent.GetVariable(varName, ofType);
				if (variable2 != null)
				{
					return variable2;
				}
			}
			return null;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00008318 File Offset: 0x00006518
		public static Variable GetVariableByID(this IBlackboard blackboard, string ID)
		{
			if (blackboard.variables != null && ID != null)
			{
				foreach (KeyValuePair<string, Variable> keyValuePair in blackboard.variables)
				{
					if (keyValuePair.Value.ID == ID)
					{
						return keyValuePair.Value;
					}
				}
			}
			if (blackboard.parent != null)
			{
				Variable variableByID = blackboard.parent.GetVariableByID(ID);
				if (variableByID != null)
				{
					return variableByID;
				}
			}
			return null;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000083AC File Offset: 0x000065AC
		public static IEnumerable<Variable> GetVariables(this IBlackboard blackboard, Type ofType = null)
		{
			if (blackboard.parent != null)
			{
				foreach (Variable variable in blackboard.parent.GetVariables(ofType))
				{
					yield return variable;
				}
				IEnumerator<Variable> enumerator = null;
			}
			foreach (KeyValuePair<string, Variable> keyValuePair in blackboard.variables)
			{
				if (ofType == null || ofType == typeof(object) || keyValuePair.Value.CanConvertTo(ofType))
				{
					yield return keyValuePair.Value;
				}
			}
			Dictionary<string, Variable>.Enumerator enumerator2 = default(Dictionary<string, Variable>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000083C4 File Offset: 0x000065C4
		public static Variable ChangeVariableType(this IBlackboard blackboard, Variable target, Type newType)
		{
			string name = target.name;
			string id = target.ID;
			blackboard.RemoveVariable(target.name);
			Variable variable = (Variable)Activator.CreateInstance(typeof(Variable<>).RTMakeGenericType(new Type[]
			{
				newType
			}), new object[]
			{
				name,
				id
			});
			blackboard.variables[target.name] = variable;
			blackboard.TryInvokeOnVariableAdded(variable);
			return variable;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00008438 File Offset: 0x00006638
		public static void OverwriteFrom(this IBlackboard blackboard, IBlackboard sourceBlackboard, bool removeMissingVariables = true)
		{
			foreach (KeyValuePair<string, Variable> keyValuePair in sourceBlackboard.variables)
			{
				if (blackboard.variables.ContainsKey(keyValuePair.Key))
				{
					blackboard.SetVariableValue(keyValuePair.Key, keyValuePair.Value.value);
				}
				else
				{
					blackboard.variables[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			if (removeMissingVariables)
			{
				foreach (string text in new List<string>(blackboard.variables.Keys))
				{
					if (!sourceBlackboard.variables.ContainsKey(text))
					{
						blackboard.variables.Remove(text);
					}
				}
			}
		}
	}
}
