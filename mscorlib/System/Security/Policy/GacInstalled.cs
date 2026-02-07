using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x02000410 RID: 1040
	[ComVisible(true)]
	[Serializable]
	public sealed class GacInstalled : EvidenceBase, IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06002A8A RID: 10890 RVA: 0x0009A08B File Offset: 0x0009828B
		public object Copy()
		{
			return new GacInstalled();
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x0009A092 File Offset: 0x00098292
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new GacIdentityPermission();
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x0009A099 File Offset: 0x00098299
		public override bool Equals(object o)
		{
			return o != null && o is GacInstalled;
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x0009A0A9 File Offset: 0x000982A9
		public override string ToString()
		{
			SecurityElement securityElement = new SecurityElement(base.GetType().FullName);
			securityElement.AddAttribute("version", "1");
			return securityElement.ToString();
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000040F7 File Offset: 0x000022F7
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return 1;
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x0008869B File Offset: 0x0008689B
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return position;
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x0009A0D0 File Offset: 0x000982D0
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position] = '\t';
			return position + 1;
		}
	}
}
