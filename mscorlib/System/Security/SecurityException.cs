using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;

namespace System.Security
{
	// Token: 0x020003E8 RID: 1000
	[ComVisible(true)]
	[Serializable]
	public class SecurityException : SystemException
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06002936 RID: 10550 RVA: 0x00095AB0 File Offset: 0x00093CB0
		// (set) Token: 0x06002937 RID: 10551 RVA: 0x00095AB8 File Offset: 0x00093CB8
		[ComVisible(false)]
		public SecurityAction Action
		{
			get
			{
				return this._action;
			}
			set
			{
				this._action = value;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06002938 RID: 10552 RVA: 0x00095AC1 File Offset: 0x00093CC1
		// (set) Token: 0x06002939 RID: 10553 RVA: 0x00095AC9 File Offset: 0x00093CC9
		[ComVisible(false)]
		public object DenySetInstance
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this._denyset;
			}
			set
			{
				this._denyset = value;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x0600293A RID: 10554 RVA: 0x00095AD2 File Offset: 0x00093CD2
		// (set) Token: 0x0600293B RID: 10555 RVA: 0x00095ADA File Offset: 0x00093CDA
		[ComVisible(false)]
		public AssemblyName FailedAssemblyInfo
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this._assembly;
			}
			set
			{
				this._assembly = value;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600293C RID: 10556 RVA: 0x00095AE3 File Offset: 0x00093CE3
		// (set) Token: 0x0600293D RID: 10557 RVA: 0x00095AEB File Offset: 0x00093CEB
		[ComVisible(false)]
		public MethodInfo Method
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this._method;
			}
			set
			{
				this._method = value;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600293E RID: 10558 RVA: 0x00095AF4 File Offset: 0x00093CF4
		// (set) Token: 0x0600293F RID: 10559 RVA: 0x00095AFC File Offset: 0x00093CFC
		[ComVisible(false)]
		public object PermitOnlySetInstance
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this._permitset;
			}
			set
			{
				this._permitset = value;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06002940 RID: 10560 RVA: 0x00095B05 File Offset: 0x00093D05
		// (set) Token: 0x06002941 RID: 10561 RVA: 0x00095B0D File Offset: 0x00093D0D
		public string Url
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this._url;
			}
			set
			{
				this._url = value;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06002942 RID: 10562 RVA: 0x00095B16 File Offset: 0x00093D16
		// (set) Token: 0x06002943 RID: 10563 RVA: 0x00095B1E File Offset: 0x00093D1E
		public SecurityZone Zone
		{
			get
			{
				return this._zone;
			}
			set
			{
				this._zone = value;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06002944 RID: 10564 RVA: 0x00095B27 File Offset: 0x00093D27
		// (set) Token: 0x06002945 RID: 10565 RVA: 0x00095B2F File Offset: 0x00093D2F
		[ComVisible(false)]
		public object Demanded
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this._demanded;
			}
			set
			{
				this._demanded = value;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06002946 RID: 10566 RVA: 0x00095B38 File Offset: 0x00093D38
		// (set) Token: 0x06002947 RID: 10567 RVA: 0x00095B40 File Offset: 0x00093D40
		public IPermission FirstPermissionThatFailed
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this._firstperm;
			}
			set
			{
				this._firstperm = value;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06002948 RID: 10568 RVA: 0x00095B49 File Offset: 0x00093D49
		// (set) Token: 0x06002949 RID: 10569 RVA: 0x00095B51 File Offset: 0x00093D51
		public string PermissionState
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this.permissionState;
			}
			set
			{
				this.permissionState = value;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x00095B5A File Offset: 0x00093D5A
		// (set) Token: 0x0600294B RID: 10571 RVA: 0x00095B62 File Offset: 0x00093D62
		public Type PermissionType
		{
			get
			{
				return this.permissionType;
			}
			set
			{
				this.permissionType = value;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600294C RID: 10572 RVA: 0x00095B6B File Offset: 0x00093D6B
		// (set) Token: 0x0600294D RID: 10573 RVA: 0x00095B73 File Offset: 0x00093D73
		public string GrantedSet
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this._granted;
			}
			set
			{
				this._granted = value;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x0600294E RID: 10574 RVA: 0x00095B7C File Offset: 0x00093D7C
		// (set) Token: 0x0600294F RID: 10575 RVA: 0x00095B84 File Offset: 0x00093D84
		public string RefusedSet
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
			get
			{
				return this._refused;
			}
			set
			{
				this._refused = value;
			}
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x00095B8D File Offset: 0x00093D8D
		public SecurityException() : this(Locale.GetText("A security error has been detected."))
		{
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x00095B9F File Offset: 0x00093D9F
		public SecurityException(string message) : base(message)
		{
			base.HResult = -2146233078;
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x00095BB4 File Offset: 0x00093DB4
		protected SecurityException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			base.HResult = -2146233078;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name == "PermissionState")
				{
					this.permissionState = (string)enumerator.Value;
					return;
				}
			}
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x00095C09 File Offset: 0x00093E09
		public SecurityException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233078;
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x00095C1E File Offset: 0x00093E1E
		public SecurityException(string message, Type type) : base(message)
		{
			base.HResult = -2146233078;
			this.permissionType = type;
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x00095C39 File Offset: 0x00093E39
		public SecurityException(string message, Type type, string state) : base(message)
		{
			base.HResult = -2146233078;
			this.permissionType = type;
			this.permissionState = state;
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x00095C5B File Offset: 0x00093E5B
		internal SecurityException(string message, PermissionSet granted, PermissionSet refused) : base(message)
		{
			base.HResult = -2146233078;
			this._granted = granted.ToString();
			this._refused = refused.ToString();
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x00095C87 File Offset: 0x00093E87
		public SecurityException(string message, object deny, object permitOnly, MethodInfo method, object demanded, IPermission permThatFailed) : base(message)
		{
			base.HResult = -2146233078;
			this._denyset = deny;
			this._permitset = permitOnly;
			this._method = method;
			this._demanded = demanded;
			this._firstperm = permThatFailed;
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x00095CC4 File Offset: 0x00093EC4
		public SecurityException(string message, AssemblyName assemblyName, PermissionSet grant, PermissionSet refused, MethodInfo method, SecurityAction action, object demanded, IPermission permThatFailed, Evidence evidence) : base(message)
		{
			base.HResult = -2146233078;
			this._assembly = assemblyName;
			this._granted = ((grant == null) ? string.Empty : grant.ToString());
			this._refused = ((refused == null) ? string.Empty : refused.ToString());
			this._method = method;
			this._action = action;
			this._demanded = demanded;
			this._firstperm = permThatFailed;
			if (this._firstperm != null)
			{
				this.permissionType = this._firstperm.GetType();
			}
			this._evidence = evidence;
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x00095D5C File Offset: 0x00093F5C
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			try
			{
				info.AddValue("PermissionState", this.permissionState);
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x00095D98 File Offset: 0x00093F98
		[SecuritySafeCritical]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString());
			try
			{
				if (this.permissionType != null)
				{
					stringBuilder.AppendFormat("{0}Type: {1}", Environment.NewLine, this.PermissionType);
				}
				if (this._method != null)
				{
					string text = this._method.ToString();
					int startIndex = text.IndexOf(" ") + 1;
					stringBuilder.AppendFormat("{0}Method: {1} {2}.{3}", new object[]
					{
						Environment.NewLine,
						this._method.ReturnType.Name,
						this._method.ReflectedType,
						text.Substring(startIndex)
					});
				}
				if (this.permissionState != null)
				{
					stringBuilder.AppendFormat("{0}State: {1}", Environment.NewLine, this.PermissionState);
				}
				if (this._granted != null && this._granted.Length > 0)
				{
					stringBuilder.AppendFormat("{0}Granted: {1}", Environment.NewLine, this.GrantedSet);
				}
				if (this._refused != null && this._refused.Length > 0)
				{
					stringBuilder.AppendFormat("{0}Refused: {1}", Environment.NewLine, this.RefusedSet);
				}
				if (this._demanded != null)
				{
					stringBuilder.AppendFormat("{0}Demanded: {1}", Environment.NewLine, this.Demanded);
				}
				if (this._firstperm != null)
				{
					stringBuilder.AppendFormat("{0}Failed Permission: {1}", Environment.NewLine, this.FirstPermissionThatFailed);
				}
				if (this._evidence != null)
				{
					stringBuilder.AppendFormat("{0}Evidences:", Environment.NewLine);
					foreach (object obj in this._evidence)
					{
						if (!(obj is Hash))
						{
							stringBuilder.AppendFormat("{0}\t{1}", Environment.NewLine, obj);
						}
					}
				}
			}
			catch (SecurityException)
			{
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001EE1 RID: 7905
		private string permissionState;

		// Token: 0x04001EE2 RID: 7906
		private Type permissionType;

		// Token: 0x04001EE3 RID: 7907
		private string _granted;

		// Token: 0x04001EE4 RID: 7908
		private string _refused;

		// Token: 0x04001EE5 RID: 7909
		private object _demanded;

		// Token: 0x04001EE6 RID: 7910
		private IPermission _firstperm;

		// Token: 0x04001EE7 RID: 7911
		private MethodInfo _method;

		// Token: 0x04001EE8 RID: 7912
		private Evidence _evidence;

		// Token: 0x04001EE9 RID: 7913
		private SecurityAction _action;

		// Token: 0x04001EEA RID: 7914
		private object _denyset;

		// Token: 0x04001EEB RID: 7915
		private object _permitset;

		// Token: 0x04001EEC RID: 7916
		private AssemblyName _assembly;

		// Token: 0x04001EED RID: 7917
		private string _url;

		// Token: 0x04001EEE RID: 7918
		private SecurityZone _zone;
	}
}
