using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B9 RID: 185
	[Name("Execute Function", 10)]
	[Category("✫ Reflected")]
	[Description("Execute a function on a script and save the return if any.\nIf function is an IEnumerator it will execute as a coroutine.")]
	public class ExecuteFunction_Multiplatform : ActionTask, IReflectedWrapper
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000C2E4 File Offset: 0x0000A4E4
		private MethodInfo targetMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000C2F1 File Offset: 0x0000A4F1
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

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000C328 File Offset: 0x0000A528
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
				string text = (this.targetMethod.ReturnType == typeof(void) || this.targetMethod.ReturnType == typeof(IEnumerator)) ? "" : (this.returnValue.ToString() + " = ");
				string text2 = "";
				for (int i = 0; i < this.parameters.Count; i++)
				{
					text2 = text2 + ((i != 0) ? ", " : "") + this.parameters[i].ToString();
				}
				string text3 = this.targetMethod.IsStatic ? this.targetMethod.RTReflectedOrDeclaredType().FriendlyName(false) : base.agentInfo;
				return string.Format("{0}{1}.{2}({3})", new object[]
				{
					text,
					text3,
					this.targetMethod.Name,
					text2
				});
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000C44B File Offset: 0x0000A64B
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this.method;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000C453 File Offset: 0x0000A653
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.method != null && this.method.HasChanged())
			{
				this.SetMethod(this.method);
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000C47C File Offset: 0x0000A67C
		protected override string OnInit()
		{
			if (this.method == null)
			{
				return "No Method selected";
			}
			if (this.targetMethod == null)
			{
				return string.Format("Missing Method '{0}'", this.method.AsString());
			}
			if (this.args == null)
			{
				ParameterInfo[] array = this.targetMethod.GetParameters();
				this.args = new object[array.Length];
				this.parameterIsByRef = new bool[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					this.parameterIsByRef[i] = array[i].ParameterType.IsByRef;
				}
			}
			return null;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000C510 File Offset: 0x0000A710
		protected override void OnExecute()
		{
			for (int i = 0; i < this.parameters.Count; i++)
			{
				this.args[i] = this.parameters[i].value;
			}
			Component component = this.targetMethod.IsStatic ? null : base.agent;
			if (this.targetMethod.ReturnType == typeof(IEnumerator))
			{
				base.StartCoroutine(this.InternalCoroutine((IEnumerator)this.targetMethod.Invoke(component, this.args)));
				return;
			}
			this.returnValue.value = this.targetMethod.Invoke(component, this.args);
			for (int j = 0; j < this.parameters.Count; j++)
			{
				if (this.parameterIsByRef[j])
				{
					this.parameters[j].value = this.args[j];
				}
			}
			base.EndAction();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000C600 File Offset: 0x0000A800
		protected override void OnStop()
		{
			this.routineRunning = false;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000C609 File Offset: 0x0000A809
		private IEnumerator InternalCoroutine(IEnumerator routine)
		{
			this.routineRunning = true;
			while (this.routineRunning && routine.MoveNext())
			{
				if (!this.routineRunning)
				{
					yield break;
				}
				yield return routine.Current;
			}
			if (this.routineRunning)
			{
				base.EndAction();
			}
			yield break;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000C620 File Offset: 0x0000A820
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
			if (method.ReturnType != typeof(void) && this.targetMethod.ReturnType != typeof(IEnumerator))
			{
				this.returnValue = new BBObjectParameter(method.ReturnType)
				{
					bb = base.blackboard
				};
				return;
			}
			this.returnValue = null;
		}

		// Token: 0x04000236 RID: 566
		[SerializeField]
		protected SerializedMethodInfo method;

		// Token: 0x04000237 RID: 567
		[SerializeField]
		protected List<BBObjectParameter> parameters = new List<BBObjectParameter>();

		// Token: 0x04000238 RID: 568
		[SerializeField]
		[BlackboardOnly]
		protected BBObjectParameter returnValue;

		// Token: 0x04000239 RID: 569
		private object[] args;

		// Token: 0x0400023A RID: 570
		private bool routineRunning;

		// Token: 0x0400023B RID: 571
		private bool[] parameterIsByRef;
	}
}
