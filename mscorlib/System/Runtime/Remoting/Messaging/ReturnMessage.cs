using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000635 RID: 1589
	[ComVisible(true)]
	public class ReturnMessage : IMethodReturnMessage, IMethodMessage, IMessage, IInternalMessage
	{
		// Token: 0x06003BF3 RID: 15347 RVA: 0x000D0A9C File Offset: 0x000CEC9C
		public ReturnMessage(object ret, object[] outArgs, int outArgsCount, LogicalCallContext callCtx, IMethodCallMessage mcm)
		{
			this._returnValue = ret;
			this._args = outArgs;
			this._callCtx = callCtx;
			if (mcm != null)
			{
				this._uri = mcm.Uri;
				this._methodBase = mcm.MethodBase;
			}
			if (this._args == null)
			{
				this._args = new object[outArgsCount];
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x000D0AF7 File Offset: 0x000CECF7
		public ReturnMessage(Exception e, IMethodCallMessage mcm)
		{
			this._exception = e;
			if (mcm != null)
			{
				this._methodBase = mcm.MethodBase;
				this._callCtx = mcm.LogicalCallContext;
			}
			this._args = new object[0];
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003BF5 RID: 15349 RVA: 0x000D0B2D File Offset: 0x000CED2D
		public int ArgCount
		{
			[SecurityCritical]
			get
			{
				return this._args.Length;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003BF6 RID: 15350 RVA: 0x000D0B37 File Offset: 0x000CED37
		public object[] Args
		{
			[SecurityCritical]
			get
			{
				return this._args;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003BF7 RID: 15351 RVA: 0x000D0B3F File Offset: 0x000CED3F
		public bool HasVarArgs
		{
			[SecurityCritical]
			get
			{
				return !(this._methodBase == null) && (this._methodBase.CallingConvention | CallingConventions.VarArgs) > (CallingConventions)0;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003BF8 RID: 15352 RVA: 0x000D0B61 File Offset: 0x000CED61
		public LogicalCallContext LogicalCallContext
		{
			[SecurityCritical]
			get
			{
				if (this._callCtx == null)
				{
					this._callCtx = new LogicalCallContext();
				}
				return this._callCtx;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06003BF9 RID: 15353 RVA: 0x000D0B7C File Offset: 0x000CED7C
		public MethodBase MethodBase
		{
			[SecurityCritical]
			get
			{
				return this._methodBase;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003BFA RID: 15354 RVA: 0x000D0B84 File Offset: 0x000CED84
		public string MethodName
		{
			[SecurityCritical]
			get
			{
				if (this._methodBase != null && this._methodName == null)
				{
					this._methodName = this._methodBase.Name;
				}
				return this._methodName;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003BFB RID: 15355 RVA: 0x000D0BB4 File Offset: 0x000CEDB4
		public object MethodSignature
		{
			[SecurityCritical]
			get
			{
				if (this._methodBase != null && this._methodSignature == null)
				{
					ParameterInfo[] parameters = this._methodBase.GetParameters();
					this._methodSignature = new Type[parameters.Length];
					for (int i = 0; i < parameters.Length; i++)
					{
						this._methodSignature[i] = parameters[i].ParameterType;
					}
				}
				return this._methodSignature;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003BFC RID: 15356 RVA: 0x000D0C15 File Offset: 0x000CEE15
		public virtual IDictionary Properties
		{
			[SecurityCritical]
			get
			{
				if (this._properties == null)
				{
					this._properties = new MethodReturnDictionary(this);
				}
				return this._properties;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06003BFD RID: 15357 RVA: 0x000D0C31 File Offset: 0x000CEE31
		public string TypeName
		{
			[SecurityCritical]
			get
			{
				if (this._methodBase != null && this._typeName == null)
				{
					this._typeName = this._methodBase.DeclaringType.AssemblyQualifiedName;
				}
				return this._typeName;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06003BFE RID: 15358 RVA: 0x000D0C65 File Offset: 0x000CEE65
		// (set) Token: 0x06003BFF RID: 15359 RVA: 0x000D0C6D File Offset: 0x000CEE6D
		public string Uri
		{
			[SecurityCritical]
			get
			{
				return this._uri;
			}
			set
			{
				this._uri = value;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003C00 RID: 15360 RVA: 0x000D0C76 File Offset: 0x000CEE76
		// (set) Token: 0x06003C01 RID: 15361 RVA: 0x000D0C7E File Offset: 0x000CEE7E
		string IInternalMessage.Uri
		{
			get
			{
				return this.Uri;
			}
			set
			{
				this.Uri = value;
			}
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x000D0C87 File Offset: 0x000CEE87
		[SecurityCritical]
		public object GetArg(int argNum)
		{
			return this._args[argNum];
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x000D0C91 File Offset: 0x000CEE91
		[SecurityCritical]
		public string GetArgName(int index)
		{
			return this._methodBase.GetParameters()[index].Name;
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003C04 RID: 15364 RVA: 0x000D0CA5 File Offset: 0x000CEEA5
		public Exception Exception
		{
			[SecurityCritical]
			get
			{
				return this._exception;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003C05 RID: 15365 RVA: 0x000D0CAD File Offset: 0x000CEEAD
		public int OutArgCount
		{
			[SecurityCritical]
			get
			{
				if (this._args == null || this._args.Length == 0)
				{
					return 0;
				}
				if (this._inArgInfo == null)
				{
					this._inArgInfo = new ArgInfo(this.MethodBase, ArgInfoType.Out);
				}
				return this._inArgInfo.GetInOutArgCount();
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003C06 RID: 15366 RVA: 0x000D0CE8 File Offset: 0x000CEEE8
		public object[] OutArgs
		{
			[SecurityCritical]
			get
			{
				if (this._outArgs == null && this._args != null)
				{
					if (this._inArgInfo == null)
					{
						this._inArgInfo = new ArgInfo(this.MethodBase, ArgInfoType.Out);
					}
					this._outArgs = this._inArgInfo.GetInOutArgs(this._args);
				}
				return this._outArgs;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06003C07 RID: 15367 RVA: 0x000D0D3C File Offset: 0x000CEF3C
		public virtual object ReturnValue
		{
			[SecurityCritical]
			get
			{
				return this._returnValue;
			}
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x000D0D44 File Offset: 0x000CEF44
		[SecurityCritical]
		public object GetOutArg(int argNum)
		{
			if (this._inArgInfo == null)
			{
				this._inArgInfo = new ArgInfo(this.MethodBase, ArgInfoType.Out);
			}
			return this._args[this._inArgInfo.GetInOutArgIndex(argNum)];
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x000D0D73 File Offset: 0x000CEF73
		[SecurityCritical]
		public string GetOutArgName(int index)
		{
			if (this._inArgInfo == null)
			{
				this._inArgInfo = new ArgInfo(this.MethodBase, ArgInfoType.Out);
			}
			return this._inArgInfo.GetInOutArgName(index);
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003C0A RID: 15370 RVA: 0x000D0D9B File Offset: 0x000CEF9B
		// (set) Token: 0x06003C0B RID: 15371 RVA: 0x000D0DA3 File Offset: 0x000CEFA3
		Identity IInternalMessage.TargetIdentity
		{
			get
			{
				return this._targetIdentity;
			}
			set
			{
				this._targetIdentity = value;
			}
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x000D0DAC File Offset: 0x000CEFAC
		bool IInternalMessage.HasProperties()
		{
			return this._properties != null;
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x000D0DAC File Offset: 0x000CEFAC
		internal bool HasProperties()
		{
			return this._properties != null;
		}

		// Token: 0x040026DC RID: 9948
		private object[] _outArgs;

		// Token: 0x040026DD RID: 9949
		private object[] _args;

		// Token: 0x040026DE RID: 9950
		private LogicalCallContext _callCtx;

		// Token: 0x040026DF RID: 9951
		private object _returnValue;

		// Token: 0x040026E0 RID: 9952
		private string _uri;

		// Token: 0x040026E1 RID: 9953
		private Exception _exception;

		// Token: 0x040026E2 RID: 9954
		private MethodBase _methodBase;

		// Token: 0x040026E3 RID: 9955
		private string _methodName;

		// Token: 0x040026E4 RID: 9956
		private Type[] _methodSignature;

		// Token: 0x040026E5 RID: 9957
		private string _typeName;

		// Token: 0x040026E6 RID: 9958
		private MethodReturnDictionary _properties;

		// Token: 0x040026E7 RID: 9959
		private Identity _targetIdentity;

		// Token: 0x040026E8 RID: 9960
		private ArgInfo _inArgInfo;
	}
}
