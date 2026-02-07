using System;
using System.Reflection;
using ParadoxNotion.Serialization;
using UnityEngine;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x02000044 RID: 68
	public abstract class ReflectedWrapper : IReflectedWrapper
	{
		// Token: 0x06000364 RID: 868 RVA: 0x00009B7C File Offset: 0x00007D7C
		public ReflectedWrapper()
		{
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00009B84 File Offset: 0x00007D84
		public static ReflectedWrapper Create(MethodInfo method, IBlackboard bb)
		{
			if (method == null)
			{
				return null;
			}
			if (method.ReturnType == typeof(void))
			{
				return ReflectedActionWrapper.Create(method, bb);
			}
			return ReflectedFunctionWrapper.Create(method, bb);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00009BB7 File Offset: 0x00007DB7
		ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo()
		{
			return this._targetMethod;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00009BC0 File Offset: 0x00007DC0
		public void SetVariablesBB(IBlackboard bb)
		{
			BBParameter[] variables = this.GetVariables();
			for (int i = 0; i < variables.Length; i++)
			{
				variables[i].bb = bb;
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00009BEB File Offset: 0x00007DEB
		public SerializedMethodInfo GetSerializedMethod()
		{
			return this._targetMethod;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00009BF3 File Offset: 0x00007DF3
		public MethodInfo GetMethod()
		{
			return this._targetMethod;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00009C00 File Offset: 0x00007E00
		public bool HasChanged()
		{
			return this._targetMethod != null && this._targetMethod.HasChanged();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00009C17 File Offset: 0x00007E17
		public string AsString()
		{
			if (this._targetMethod == null)
			{
				return null;
			}
			return this._targetMethod.AsString();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00009C2E File Offset: 0x00007E2E
		public override string ToString()
		{
			return this.AsString();
		}

		// Token: 0x0600036D RID: 877
		public abstract BBParameter[] GetVariables();

		// Token: 0x0600036E RID: 878
		public abstract void Init(object instance);

		// Token: 0x040000FB RID: 251
		[SerializeField]
		protected SerializedMethodInfo _targetMethod;
	}
}
