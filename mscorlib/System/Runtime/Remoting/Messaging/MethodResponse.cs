using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200062B RID: 1579
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Serializable]
	public class MethodResponse : IMethodReturnMessage, IMethodMessage, IMessage, ISerializable, IInternalMessage, ISerializationRootObject
	{
		// Token: 0x06003B7E RID: 15230 RVA: 0x000CF85C File Offset: 0x000CDA5C
		public MethodResponse(Header[] h1, IMethodCallMessage mcm)
		{
			if (mcm != null)
			{
				this._methodName = mcm.MethodName;
				this._uri = mcm.Uri;
				this._typeName = mcm.TypeName;
				this._methodBase = mcm.MethodBase;
				this._methodSignature = (Type[])mcm.MethodSignature;
				this._args = mcm.Args;
			}
			if (h1 != null)
			{
				foreach (Header header in h1)
				{
					this.InitMethodProperty(header.Name, header.Value);
				}
			}
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x000CF8E8 File Offset: 0x000CDAE8
		internal MethodResponse(Exception e, IMethodCallMessage msg)
		{
			this._callMsg = msg;
			if (msg != null)
			{
				this._uri = msg.Uri;
			}
			else
			{
				this._uri = string.Empty;
			}
			this._exception = e;
			this._returnValue = null;
			this._outArgs = new object[0];
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x000CF938 File Offset: 0x000CDB38
		internal MethodResponse(object returnValue, object[] outArgs, LogicalCallContext callCtx, IMethodCallMessage msg)
		{
			this._callMsg = msg;
			this._uri = msg.Uri;
			this._exception = null;
			this._returnValue = returnValue;
			this._args = outArgs;
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x000CF96C File Offset: 0x000CDB6C
		internal MethodResponse(IMethodCallMessage msg, CADMethodReturnMessage retmsg)
		{
			this._callMsg = msg;
			this._methodBase = msg.MethodBase;
			this._uri = msg.Uri;
			this._methodName = msg.MethodName;
			ArrayList arguments = retmsg.GetArguments();
			this._exception = retmsg.GetException(arguments);
			this._returnValue = retmsg.GetReturnValue(arguments);
			this._args = retmsg.GetArgs(arguments);
			this._callContext = retmsg.GetLogicalCallContext(arguments);
			if (this._callContext == null)
			{
				this._callContext = new LogicalCallContext();
			}
			if (retmsg.PropertiesCount > 0)
			{
				CADMessageBase.UnmarshalProperties(this.Properties, retmsg.PropertiesCount, arguments);
			}
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x000CFA14 File Offset: 0x000CDC14
		internal MethodResponse(IMethodCallMessage msg, object handlerObject, BinaryMethodReturnMessage smuggledMrm)
		{
			if (msg != null)
			{
				this._methodBase = msg.MethodBase;
				this._methodName = msg.MethodName;
				this._uri = msg.Uri;
			}
			this._returnValue = smuggledMrm.ReturnValue;
			this._args = smuggledMrm.Args;
			this._exception = smuggledMrm.Exception;
			this._callContext = smuggledMrm.LogicalCallContext;
			if (smuggledMrm.HasProperties)
			{
				smuggledMrm.PopulateMessageProperties(this.Properties);
			}
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x000CFA94 File Offset: 0x000CDC94
		internal MethodResponse(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry serializationEntry in info)
			{
				this.InitMethodProperty(serializationEntry.Name, serializationEntry.Value);
			}
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x000CFAD4 File Offset: 0x000CDCD4
		internal void InitMethodProperty(string key, object value)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
			if (num <= 1960967436U)
			{
				if (num <= 1201911322U)
				{
					if (num != 990701179U)
					{
						if (num == 1201911322U)
						{
							if (key == "__CallContext")
							{
								this._callContext = (LogicalCallContext)value;
								return;
							}
						}
					}
					else if (key == "__Uri")
					{
						this._uri = (string)value;
						return;
					}
				}
				else if (num != 1637783905U)
				{
					if (num == 1960967436U)
					{
						if (key == "__OutArgs")
						{
							this._args = (object[])value;
							return;
						}
					}
				}
				else if (key == "__Return")
				{
					this._returnValue = value;
					return;
				}
			}
			else if (num <= 3166241401U)
			{
				if (num != 2010141056U)
				{
					if (num == 3166241401U)
					{
						if (key == "__MethodName")
						{
							this._methodName = (string)value;
							return;
						}
					}
				}
				else if (key == "__TypeName")
				{
					this._typeName = (string)value;
					return;
				}
			}
			else if (num != 3626951189U)
			{
				if (num == 3679129400U)
				{
					if (key == "__MethodSignature")
					{
						this._methodSignature = (Type[])value;
						return;
					}
				}
			}
			else if (key == "__fault")
			{
				this._exception = (Exception)value;
				return;
			}
			this.Properties[key] = value;
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003B85 RID: 15237 RVA: 0x000CFC5A File Offset: 0x000CDE5A
		public int ArgCount
		{
			[SecurityCritical]
			get
			{
				if (this._args == null)
				{
					return 0;
				}
				return this._args.Length;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003B86 RID: 15238 RVA: 0x000CFC6E File Offset: 0x000CDE6E
		public object[] Args
		{
			[SecurityCritical]
			get
			{
				return this._args;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06003B87 RID: 15239 RVA: 0x000CFC76 File Offset: 0x000CDE76
		public Exception Exception
		{
			[SecurityCritical]
			get
			{
				return this._exception;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x000CFC7E File Offset: 0x000CDE7E
		public bool HasVarArgs
		{
			[SecurityCritical]
			get
			{
				return (this.MethodBase.CallingConvention | CallingConventions.VarArgs) > (CallingConventions)0;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06003B89 RID: 15241 RVA: 0x000CFC90 File Offset: 0x000CDE90
		public LogicalCallContext LogicalCallContext
		{
			[SecurityCritical]
			get
			{
				if (this._callContext == null)
				{
					this._callContext = new LogicalCallContext();
				}
				return this._callContext;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003B8A RID: 15242 RVA: 0x000CFCAC File Offset: 0x000CDEAC
		public MethodBase MethodBase
		{
			[SecurityCritical]
			get
			{
				if (null == this._methodBase)
				{
					if (this._callMsg != null)
					{
						this._methodBase = this._callMsg.MethodBase;
					}
					else if (this.MethodName != null && this.TypeName != null)
					{
						this._methodBase = RemotingServices.GetMethodBaseFromMethodMessage(this);
					}
				}
				return this._methodBase;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06003B8B RID: 15243 RVA: 0x000CFD04 File Offset: 0x000CDF04
		public string MethodName
		{
			[SecurityCritical]
			get
			{
				if (this._methodName == null && this._callMsg != null)
				{
					this._methodName = this._callMsg.MethodName;
				}
				return this._methodName;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003B8C RID: 15244 RVA: 0x000CFD2D File Offset: 0x000CDF2D
		public object MethodSignature
		{
			[SecurityCritical]
			get
			{
				if (this._methodSignature == null && this._callMsg != null)
				{
					this._methodSignature = (Type[])this._callMsg.MethodSignature;
				}
				return this._methodSignature;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06003B8D RID: 15245 RVA: 0x000CFD5B File Offset: 0x000CDF5B
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

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06003B8E RID: 15246 RVA: 0x000CFD98 File Offset: 0x000CDF98
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

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06003B8F RID: 15247 RVA: 0x000CFDEC File Offset: 0x000CDFEC
		public virtual IDictionary Properties
		{
			[SecurityCritical]
			get
			{
				if (this.ExternalProperties == null)
				{
					MethodReturnDictionary methodReturnDictionary = new MethodReturnDictionary(this);
					this.ExternalProperties = methodReturnDictionary;
					this.InternalProperties = methodReturnDictionary.GetInternalProperties();
				}
				return this.ExternalProperties;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06003B90 RID: 15248 RVA: 0x000CFE21 File Offset: 0x000CE021
		public object ReturnValue
		{
			[SecurityCritical]
			get
			{
				return this._returnValue;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06003B91 RID: 15249 RVA: 0x000CFE29 File Offset: 0x000CE029
		public string TypeName
		{
			[SecurityCritical]
			get
			{
				if (this._typeName == null && this._callMsg != null)
				{
					this._typeName = this._callMsg.TypeName;
				}
				return this._typeName;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06003B92 RID: 15250 RVA: 0x000CFE52 File Offset: 0x000CE052
		// (set) Token: 0x06003B93 RID: 15251 RVA: 0x000CFE7B File Offset: 0x000CE07B
		public string Uri
		{
			[SecurityCritical]
			get
			{
				if (this._uri == null && this._callMsg != null)
				{
					this._uri = this._callMsg.Uri;
				}
				return this._uri;
			}
			set
			{
				this._uri = value;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003B94 RID: 15252 RVA: 0x000CFE84 File Offset: 0x000CE084
		// (set) Token: 0x06003B95 RID: 15253 RVA: 0x000CFE8C File Offset: 0x000CE08C
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

		// Token: 0x06003B96 RID: 15254 RVA: 0x000CFE95 File Offset: 0x000CE095
		[SecurityCritical]
		public object GetArg(int argNum)
		{
			if (this._args == null)
			{
				return null;
			}
			return this._args[argNum];
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x000CFEA9 File Offset: 0x000CE0A9
		[SecurityCritical]
		public string GetArgName(int index)
		{
			return this.MethodBase.GetParameters()[index].Name;
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x000CFEC0 File Offset: 0x000CE0C0
		[SecurityCritical]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (this._exception == null)
			{
				info.AddValue("__TypeName", this._typeName);
				info.AddValue("__MethodName", this._methodName);
				info.AddValue("__MethodSignature", this._methodSignature);
				info.AddValue("__Uri", this._uri);
				info.AddValue("__Return", this._returnValue);
				info.AddValue("__OutArgs", this._args);
			}
			else
			{
				info.AddValue("__fault", this._exception);
			}
			info.AddValue("__CallContext", this._callContext);
			if (this.InternalProperties != null)
			{
				foreach (object obj in this.InternalProperties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					info.AddValue((string)dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x000CFFC8 File Offset: 0x000CE1C8
		[SecurityCritical]
		public object GetOutArg(int argNum)
		{
			if (this._args == null)
			{
				return null;
			}
			if (this._inArgInfo == null)
			{
				this._inArgInfo = new ArgInfo(this.MethodBase, ArgInfoType.Out);
			}
			return this._args[this._inArgInfo.GetInOutArgIndex(argNum)];
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x000D0004 File Offset: 0x000CE204
		[SecurityCritical]
		public string GetOutArgName(int index)
		{
			if (null == this._methodBase)
			{
				return "__method_" + index.ToString();
			}
			if (this._inArgInfo == null)
			{
				this._inArgInfo = new ArgInfo(this.MethodBase, ArgInfoType.Out);
			}
			return this._inArgInfo.GetInOutArgName(index);
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public virtual object HeaderHandler(Header[] h)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public void RootSetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			throw new NotImplementedException();
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003B9D RID: 15261 RVA: 0x000D0057 File Offset: 0x000CE257
		// (set) Token: 0x06003B9E RID: 15262 RVA: 0x000D005F File Offset: 0x000CE25F
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

		// Token: 0x06003B9F RID: 15263 RVA: 0x000D0068 File Offset: 0x000CE268
		bool IInternalMessage.HasProperties()
		{
			return this.ExternalProperties != null || this.InternalProperties != null;
		}

		// Token: 0x040026AC RID: 9900
		private string _methodName;

		// Token: 0x040026AD RID: 9901
		private string _uri;

		// Token: 0x040026AE RID: 9902
		private string _typeName;

		// Token: 0x040026AF RID: 9903
		private MethodBase _methodBase;

		// Token: 0x040026B0 RID: 9904
		private object _returnValue;

		// Token: 0x040026B1 RID: 9905
		private Exception _exception;

		// Token: 0x040026B2 RID: 9906
		private Type[] _methodSignature;

		// Token: 0x040026B3 RID: 9907
		private ArgInfo _inArgInfo;

		// Token: 0x040026B4 RID: 9908
		private object[] _args;

		// Token: 0x040026B5 RID: 9909
		private object[] _outArgs;

		// Token: 0x040026B6 RID: 9910
		private IMethodCallMessage _callMsg;

		// Token: 0x040026B7 RID: 9911
		private LogicalCallContext _callContext;

		// Token: 0x040026B8 RID: 9912
		private Identity _targetIdentity;

		// Token: 0x040026B9 RID: 9913
		protected IDictionary ExternalProperties;

		// Token: 0x040026BA RID: 9914
		protected IDictionary InternalProperties;
	}
}
