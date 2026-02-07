using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata;

namespace System.Runtime.Remoting
{
	// Token: 0x02000562 RID: 1378
	[ComVisible(true)]
	public class InternalRemotingServices
	{
		// Token: 0x060035FF RID: 13823 RVA: 0x000472CC File Offset: 0x000454CC
		[Conditional("_LOGGING")]
		public static void DebugOutChnl(string s)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000C2558 File Offset: 0x000C0758
		public static SoapAttribute GetCachedSoapAttribute(object reflectionObject)
		{
			object syncRoot = InternalRemotingServices._soapAttributes.SyncRoot;
			SoapAttribute result;
			lock (syncRoot)
			{
				SoapAttribute soapAttribute = InternalRemotingServices._soapAttributes[reflectionObject] as SoapAttribute;
				if (soapAttribute != null)
				{
					result = soapAttribute;
				}
				else
				{
					object[] customAttributes = ((ICustomAttributeProvider)reflectionObject).GetCustomAttributes(typeof(SoapAttribute), true);
					if (customAttributes.Length != 0)
					{
						soapAttribute = (SoapAttribute)customAttributes[0];
					}
					else if (reflectionObject is Type)
					{
						soapAttribute = new SoapTypeAttribute();
					}
					else if (reflectionObject is FieldInfo)
					{
						soapAttribute = new SoapFieldAttribute();
					}
					else if (reflectionObject is MethodBase)
					{
						soapAttribute = new SoapMethodAttribute();
					}
					else if (reflectionObject is ParameterInfo)
					{
						soapAttribute = new SoapParameterAttribute();
					}
					soapAttribute.SetReflectionObject(reflectionObject);
					InternalRemotingServices._soapAttributes[reflectionObject] = soapAttribute;
					result = soapAttribute;
				}
			}
			return result;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x000472CC File Offset: 0x000454CC
		[Conditional("_DEBUG")]
		public static void RemotingAssert(bool condition, string message)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x000472CC File Offset: 0x000454CC
		[Conditional("_LOGGING")]
		public static void RemotingTrace(params object[] messages)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x000C2630 File Offset: 0x000C0830
		[CLSCompliant(false)]
		public static void SetServerIdentity(MethodCall m, object srvID)
		{
			Identity identity = srvID as Identity;
			if (identity == null)
			{
				throw new ArgumentException("srvID");
			}
			RemotingServices.SetMessageTargetIdentity(m, identity);
		}

		// Token: 0x04002523 RID: 9507
		private static Hashtable _soapAttributes = new Hashtable();
	}
}
