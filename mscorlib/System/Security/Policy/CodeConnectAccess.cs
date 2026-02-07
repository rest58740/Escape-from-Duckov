using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000406 RID: 1030
	[ComVisible(true)]
	[Serializable]
	public class CodeConnectAccess
	{
		// Token: 0x06002A1B RID: 10779 RVA: 0x000989A0 File Offset: 0x00096BA0
		[MonoTODO("(2.0) validations incomplete")]
		public CodeConnectAccess(string allowScheme, int allowPort)
		{
			if (allowScheme == null || allowScheme.Length == 0)
			{
				throw new ArgumentOutOfRangeException("allowScheme");
			}
			if (allowPort < 0 || allowPort > 65535)
			{
				throw new ArgumentOutOfRangeException("allowPort");
			}
			this._scheme = allowScheme;
			this._port = allowPort;
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06002A1C RID: 10780 RVA: 0x000989EE File Offset: 0x00096BEE
		public int Port
		{
			get
			{
				return this._port;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x000989F6 File Offset: 0x00096BF6
		public string Scheme
		{
			get
			{
				return this._scheme;
			}
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x00098A00 File Offset: 0x00096C00
		public override bool Equals(object o)
		{
			CodeConnectAccess codeConnectAccess = o as CodeConnectAccess;
			return codeConnectAccess != null && this._scheme == codeConnectAccess._scheme && this._port == codeConnectAccess._port;
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x00098A3C File Offset: 0x00096C3C
		public override int GetHashCode()
		{
			return this._scheme.GetHashCode() ^ this._port;
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x00098A50 File Offset: 0x00096C50
		public static CodeConnectAccess CreateAnySchemeAccess(int allowPort)
		{
			return new CodeConnectAccess(CodeConnectAccess.AnyScheme, allowPort);
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x00098A5D File Offset: 0x00096C5D
		public static CodeConnectAccess CreateOriginSchemeAccess(int allowPort)
		{
			return new CodeConnectAccess(CodeConnectAccess.OriginScheme, allowPort);
		}

		// Token: 0x04001F64 RID: 8036
		public static readonly string AnyScheme = "*";

		// Token: 0x04001F65 RID: 8037
		public static readonly int DefaultPort = -3;

		// Token: 0x04001F66 RID: 8038
		public static readonly int OriginPort = -4;

		// Token: 0x04001F67 RID: 8039
		public static readonly string OriginScheme = "$origin";

		// Token: 0x04001F68 RID: 8040
		private string _scheme;

		// Token: 0x04001F69 RID: 8041
		private int _port;
	}
}
