using System;

namespace System.Resources
{
	// Token: 0x02000863 RID: 2147
	public interface IResourceWriter : IDisposable
	{
		// Token: 0x0600475B RID: 18267
		void AddResource(string name, string value);

		// Token: 0x0600475C RID: 18268
		void AddResource(string name, object value);

		// Token: 0x0600475D RID: 18269
		void AddResource(string name, byte[] value);

		// Token: 0x0600475E RID: 18270
		void Close();

		// Token: 0x0600475F RID: 18271
		void Generate();
	}
}
