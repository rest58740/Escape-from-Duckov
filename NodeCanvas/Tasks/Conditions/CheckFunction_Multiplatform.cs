using System;
using System.Collections.Generic;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000039 RID: 57
	[Name("Check Function", 10)]
	[Category("✫ Reflected")]
	[Description("Call a function on a component and return whether or not the return value is equal to the check value")]
	public class CheckFunction_Multiplatform : ConditionTask, IReflectedWrapper
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000052EA File Offset: 0x000034EA
		private MethodInfo targetMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000052F7 File Offset: 0x000034F7
		public override Type agentType
		{
			get
			{
				if (this.targetMethod == null)
				{
					return typeof(Transform);
				}
				if (!this.targetMethod.IsStatic)
				{
					return this.targetMethod.RTReflectedOrDeclaredType();
				}
				return null;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000DF RID: 223 RVA: 0x0000532C File Offset: 0x0000352C
		protected override string info
		{
			get
			{
				if (this.method == null)
				{
					return "No Method Selected";
				}
				if (this.targetMethod == null)
				{
					return this.method.AsString().FormatError();
				}
				string text = "";
				for (int i = 0; i < this.parameters.Count; i++)
				{
					text = text + ((i != 0) ? ", " : "") + this.parameters[i].ToString();
				}
				string text2 = this.targetMethod.IsStatic ? this.targetMethod.RTReflectedOrDeclaredType().FriendlyName(false) : base.agentInfo;
				string text3 = "{0}.{1}({2}){3}";
				object[] array = new object[4];
				array[0] = text2;
				array[1] = this.targetMethod.Name;
				array[2] = text;
				int num = 3;
				string compareString = OperationTools.GetCompareString(this.comparison);
				BBObjectParameter bbobjectParameter = this.checkValue;
				array[num] = compareString + ((bbobjectParameter != null) ? bbobjectParameter.ToString() : null);
				return string.Format(text3, array);
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000541B File Offset: 0x0000361B
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.method;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005423 File Offset: 0x00003623
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.method != null && this.method.HasChanged())
			{
				this.SetMethod(this.method);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000544C File Offset: 0x0000364C
		protected override string OnInit()
		{
			if (this.method == null)
			{
				return "No Method Selected";
			}
			if (this.targetMethod == null)
			{
				return this.method.AsString();
			}
			if (this.args == null)
			{
				ParameterInfo[] array = this.targetMethod.GetParameters();
				this.args = new object[array.Length];
				this.parameterIsByRef = new bool[array.Length];
				for (int i = 0; i < this.parameters.Count; i++)
				{
					this.parameterIsByRef[i] = array[i].ParameterType.IsByRef;
				}
			}
			return null;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000054E0 File Offset: 0x000036E0
		protected override bool OnCheck()
		{
			for (int i = 0; i < this.parameters.Count; i++)
			{
				this.args[i] = this.parameters[i].value;
			}
			Component component = this.targetMethod.IsStatic ? null : base.agent;
			bool result;
			if (this.checkValue.varType == typeof(float))
			{
				result = OperationTools.Compare((float)this.targetMethod.Invoke(component, this.args), (float)this.checkValue.value, this.comparison, 0.05f);
			}
			else if (this.checkValue.varType == typeof(int))
			{
				result = OperationTools.Compare((int)this.targetMethod.Invoke(component, this.args), (int)this.checkValue.value, this.comparison);
			}
			else
			{
				result = ObjectUtils.AnyEquals(this.targetMethod.Invoke(component, this.args), this.checkValue.value);
			}
			for (int j = 0; j < this.parameters.Count; j++)
			{
				if (this.parameterIsByRef[j])
				{
					this.parameters[j].value = this.args[j];
				}
			}
			return result;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005638 File Offset: 0x00003838
		private void SetMethod(MethodInfo method)
		{
			if (method == null)
			{
				return;
			}
			this.method = new SerializedMethodInfo(method);
			this.parameters.Clear();
			foreach (ParameterInfo parameterInfo in method.GetParameters())
			{
				Type parameterType = parameterInfo.ParameterType;
				BBObjectParameter bbobjectParameter = new BBObjectParameter(parameterType.IsByRef ? parameterType.GetElementType() : parameterType)
				{
					bb = base.blackboard
				};
				if (parameterInfo.IsOptional)
				{
					bbobjectParameter.value = parameterInfo.DefaultValue;
				}
				this.parameters.Add(bbobjectParameter);
			}
			this.checkValue = new BBObjectParameter(method.ReturnType)
			{
				bb = base.blackboard
			};
			this.comparison = CompareMethod.EqualTo;
		}

		// Token: 0x040000A6 RID: 166
		[SerializeField]
		protected SerializedMethodInfo method;

		// Token: 0x040000A7 RID: 167
		[SerializeField]
		protected List<BBObjectParameter> parameters = new List<BBObjectParameter>();

		// Token: 0x040000A8 RID: 168
		[SerializeField]
		protected CompareMethod comparison;

		// Token: 0x040000A9 RID: 169
		[SerializeField]
		[BlackboardOnly]
		protected BBObjectParameter checkValue;

		// Token: 0x040000AA RID: 170
		private object[] args;

		// Token: 0x040000AB RID: 171
		private bool[] parameterIsByRef;
	}
}
