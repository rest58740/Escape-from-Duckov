using System;
using System.Security.Cryptography;
using System.Text;

namespace ES3Internal
{
	// Token: 0x020000CE RID: 206
	public static class ES3Hash
	{
		// Token: 0x0600040B RID: 1035 RVA: 0x0001A850 File Offset: 0x00018A50
		public static string SHA1Hash(string input)
		{
			string @string;
			using (SHA1Managed sha1Managed = new SHA1Managed())
			{
				@string = Encoding.UTF8.GetString(sha1Managed.ComputeHash(Encoding.UTF8.GetBytes(input)));
			}
			return @string;
		}
	}
}
