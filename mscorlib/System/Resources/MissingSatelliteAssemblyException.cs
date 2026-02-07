using System;
using System.Runtime.Serialization;

namespace System.Resources
{
	// Token: 0x0200085B RID: 2139
	[Serializable]
	public class MissingSatelliteAssemblyException : SystemException
	{
		// Token: 0x0600473A RID: 18234 RVA: 0x000E8098 File Offset: 0x000E6298
		public MissingSatelliteAssemblyException() : base("Resource lookup fell back to the ultimate fallback resources in a satellite assembly, but that satellite either was not found or could not be loaded. Please consider reinstalling or repairing the application.")
		{
			base.HResult = -2146233034;
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x000E80B0 File Offset: 0x000E62B0
		public MissingSatelliteAssemblyException(string message) : base(message)
		{
			base.HResult = -2146233034;
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x000E80C4 File Offset: 0x000E62C4
		public MissingSatelliteAssemblyException(string message, string cultureName) : base(message)
		{
			base.HResult = -2146233034;
			this._cultureName = cultureName;
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x000E80DF File Offset: 0x000E62DF
		public MissingSatelliteAssemblyException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233034;
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected MissingSatelliteAssemblyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x0600473F RID: 18239 RVA: 0x000E80F4 File Offset: 0x000E62F4
		public string CultureName
		{
			get
			{
				return this._cultureName;
			}
		}

		// Token: 0x04002DA1 RID: 11681
		private string _cultureName;
	}
}
