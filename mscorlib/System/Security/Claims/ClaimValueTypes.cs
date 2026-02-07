using System;
using System.Runtime.InteropServices;

namespace System.Security.Claims
{
	// Token: 0x020004F2 RID: 1266
	[ComVisible(false)]
	public static class ClaimValueTypes
	{
		// Token: 0x04002394 RID: 9108
		private const string XmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema";

		// Token: 0x04002395 RID: 9109
		public const string Base64Binary = "http://www.w3.org/2001/XMLSchema#base64Binary";

		// Token: 0x04002396 RID: 9110
		public const string Base64Octet = "http://www.w3.org/2001/XMLSchema#base64Octet";

		// Token: 0x04002397 RID: 9111
		public const string Boolean = "http://www.w3.org/2001/XMLSchema#boolean";

		// Token: 0x04002398 RID: 9112
		public const string Date = "http://www.w3.org/2001/XMLSchema#date";

		// Token: 0x04002399 RID: 9113
		public const string DateTime = "http://www.w3.org/2001/XMLSchema#dateTime";

		// Token: 0x0400239A RID: 9114
		public const string Double = "http://www.w3.org/2001/XMLSchema#double";

		// Token: 0x0400239B RID: 9115
		public const string Fqbn = "http://www.w3.org/2001/XMLSchema#fqbn";

		// Token: 0x0400239C RID: 9116
		public const string HexBinary = "http://www.w3.org/2001/XMLSchema#hexBinary";

		// Token: 0x0400239D RID: 9117
		public const string Integer = "http://www.w3.org/2001/XMLSchema#integer";

		// Token: 0x0400239E RID: 9118
		public const string Integer32 = "http://www.w3.org/2001/XMLSchema#integer32";

		// Token: 0x0400239F RID: 9119
		public const string Integer64 = "http://www.w3.org/2001/XMLSchema#integer64";

		// Token: 0x040023A0 RID: 9120
		public const string Sid = "http://www.w3.org/2001/XMLSchema#sid";

		// Token: 0x040023A1 RID: 9121
		public const string String = "http://www.w3.org/2001/XMLSchema#string";

		// Token: 0x040023A2 RID: 9122
		public const string Time = "http://www.w3.org/2001/XMLSchema#time";

		// Token: 0x040023A3 RID: 9123
		public const string UInteger32 = "http://www.w3.org/2001/XMLSchema#uinteger32";

		// Token: 0x040023A4 RID: 9124
		public const string UInteger64 = "http://www.w3.org/2001/XMLSchema#uinteger64";

		// Token: 0x040023A5 RID: 9125
		private const string SoapSchemaNamespace = "http://schemas.xmlsoap.org/";

		// Token: 0x040023A6 RID: 9126
		public const string DnsName = "http://schemas.xmlsoap.org/claims/dns";

		// Token: 0x040023A7 RID: 9127
		public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

		// Token: 0x040023A8 RID: 9128
		public const string Rsa = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa";

		// Token: 0x040023A9 RID: 9129
		public const string UpnName = "http://schemas.xmlsoap.org/claims/UPN";

		// Token: 0x040023AA RID: 9130
		private const string XmlSignatureConstantsNamespace = "http://www.w3.org/2000/09/xmldsig#";

		// Token: 0x040023AB RID: 9131
		public const string DsaKeyValue = "http://www.w3.org/2000/09/xmldsig#DSAKeyValue";

		// Token: 0x040023AC RID: 9132
		public const string KeyInfo = "http://www.w3.org/2000/09/xmldsig#KeyInfo";

		// Token: 0x040023AD RID: 9133
		public const string RsaKeyValue = "http://www.w3.org/2000/09/xmldsig#RSAKeyValue";

		// Token: 0x040023AE RID: 9134
		private const string XQueryOperatorsNameSpace = "http://www.w3.org/TR/2002/WD-xquery-operators-20020816";

		// Token: 0x040023AF RID: 9135
		public const string DaytimeDuration = "http://www.w3.org/TR/2002/WD-xquery-operators-20020816#dayTimeDuration";

		// Token: 0x040023B0 RID: 9136
		public const string YearMonthDuration = "http://www.w3.org/TR/2002/WD-xquery-operators-20020816#yearMonthDuration";

		// Token: 0x040023B1 RID: 9137
		private const string Xacml10Namespace = "urn:oasis:names:tc:xacml:1.0";

		// Token: 0x040023B2 RID: 9138
		public const string Rfc822Name = "urn:oasis:names:tc:xacml:1.0:data-type:rfc822Name";

		// Token: 0x040023B3 RID: 9139
		public const string X500Name = "urn:oasis:names:tc:xacml:1.0:data-type:x500Name";
	}
}
