using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D9 RID: 217
	[Obsolete("Execute Function now supports static functions as well")]
	public class ExecuteStaticFunction_Multiplatform : ActionTask
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000E86A File Offset: 0x0000CA6A
		private MethodInfo targetMethod
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000E878 File Offset: 0x0000CA78
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
				string text = (this.targetMethod.ReturnType == typeof(void)) ? "" : (this.returnValue.ToString() + " = ");
				string text2 = "";
				for (int i = 0; i < this.parameters.Count; i++)
				{
					text2 = text2 + ((i != 0) ? ", " : "") + this.parameters[i].ToString();
				}
				return string.Format("{0}{1}.{2} ({3})", new object[]
				{
					text,
					this.targetMethod.DeclaringType.FriendlyName(false),
					this.targetMethod.Name,
					text2
				});
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000E968 File Offset: 0x0000CB68
		public override void OnValidate(ITaskSystem ownerSystem)
		{
			if (this.method != null && this.method.HasChanged())
			{
				this.SetMethod(this.method);
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000E990 File Offset: 0x0000CB90
		protected override string OnInit()
		{
			if (this.method == null)
			{
				return "No methMethodd selected";
			}
			if (this.targetMethod == null)
			{
				return string.Format("Missing Method '{0}'", this.method.AsString());
			}
			return null;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000E9C8 File Offset: 0x0000CBC8
		protected override void OnExecute()
		{
			object[] array = (from p in this.parameters
			select p.value).ToArray<object>();
			this.returnValue.value = this.targetMethod.Invoke(base.agent, array);
			base.EndAction();
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000EA28 File Offset: 0x0000CC28
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
				BBObjectParameter bbobjectParameter = new BBObjectParameter(parameterInfo.ParameterType)
				{
					bb = base.blackboard
				};
				if (parameterInfo.IsOptional)
				{
					bbobjectParameter.value = parameterInfo.DefaultValue;
				}
				this.parameters.Add(bbobjectParameter);
			}
			if (method.ReturnType != typeof(void))
			{
				this.returnValue = new BBObjectParameter(method.ReturnType)
				{
					bb = base.blackboard
				};
				return;
			}
			this.returnValue = null;
		}

		// Token: 0x04000285 RID: 645
		[SerializeField]
		protected SerializedMethodInfo method;

		// Token: 0x04000286 RID: 646
		[SerializeField]
		protected List<BBObjectParameter> parameters = new List<BBObjectParameter>();

		// Token: 0x04000287 RID: 647
		[SerializeField]
		[BlackboardOnly]
		protected BBObjectParameter returnValue;
	}
}
