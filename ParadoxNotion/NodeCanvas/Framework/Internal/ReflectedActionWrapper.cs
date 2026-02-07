using System;
using System.Reflection;
using ParadoxNotion;
using ParadoxNotion.Serialization;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000045 RID: 69
	public abstract class ReflectedActionWrapper : ReflectedWrapper
	{
		// Token: 0x0600036F RID: 879 RVA: 0x00009C38 File Offset: 0x00007E38
		public new static ReflectedActionWrapper Create(MethodInfo method, IBlackboard bb)
		{
			if (method == null)
			{
				return null;
			}
			Type type = null;
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length == 0)
			{
				type = typeof(ReflectedAction);
			}
			if (parameters.Length == 1)
			{
				type = typeof(ReflectedAction<>);
			}
			if (parameters.Length == 2)
			{
				type = typeof(ReflectedAction<, >);
			}
			if (parameters.Length == 3)
			{
				type = typeof(ReflectedAction<, , >);
			}
			if (parameters.Length == 4)
			{
				type = typeof(ReflectedAction<, , , >);
			}
			if (parameters.Length == 5)
			{
				type = typeof(ReflectedAction<, , , , >);
			}
			if (parameters.Length == 6)
			{
				type = typeof(ReflectedAction<, , , , , >);
			}
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				Type type2 = parameterInfo.ParameterType.IsByRef ? parameterInfo.ParameterType.GetElementType() : parameterInfo.ParameterType;
				array[i] = type2;
			}
			ReflectedActionWrapper reflectedActionWrapper = (ReflectedActionWrapper)Activator.CreateInstance((array.Length != 0) ? type.RTMakeGenericType(array) : type);
			reflectedActionWrapper._targetMethod = new SerializedMethodInfo(method);
			BBParameter.SetBBFields(reflectedActionWrapper, bb);
			BBParameter[] variables = reflectedActionWrapper.GetVariables();
			for (int j = 0; j < parameters.Length; j++)
			{
				ParameterInfo parameterInfo2 = parameters[j];
				if (parameterInfo2.IsOptional)
				{
					variables[j].value = parameterInfo2.DefaultValue;
				}
			}
			return reflectedActionWrapper;
		}

		// Token: 0x06000370 RID: 880
		public abstract void Call();
	}
}
