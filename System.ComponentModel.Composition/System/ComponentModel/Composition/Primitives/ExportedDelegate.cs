using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives
{
	// Token: 0x02000095 RID: 149
	public class ExportedDelegate
	{
		// Token: 0x060003F1 RID: 1009 RVA: 0x00002BAC File Offset: 0x00000DAC
		protected ExportedDelegate()
		{
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000B3F7 File Offset: 0x000095F7
		public ExportedDelegate(object instance, MethodInfo method)
		{
			Requires.NotNull<MethodInfo>(method, "method");
			this._instance = instance;
			this._method = method;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000B418 File Offset: 0x00009618
		public virtual Delegate CreateDelegate(Type delegateType)
		{
			Requires.NotNull<Type>(delegateType, "delegateType");
			if (delegateType == typeof(Delegate) || delegateType == typeof(MulticastDelegate))
			{
				delegateType = this.CreateStandardDelegateType();
			}
			return Delegate.CreateDelegate(delegateType, this._instance, this._method, false);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000B470 File Offset: 0x00009670
		private Type CreateStandardDelegateType()
		{
			ParameterInfo[] parameters = this._method.GetParameters();
			Type[] array = new Type[parameters.Length + 1];
			array[parameters.Length] = this._method.ReturnType;
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			return Expression.GetDelegateType(array);
		}

		// Token: 0x04000187 RID: 391
		private object _instance;

		// Token: 0x04000188 RID: 392
		private MethodInfo _method;
	}
}
