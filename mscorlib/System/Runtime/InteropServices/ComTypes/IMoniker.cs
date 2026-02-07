using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007AA RID: 1962
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000f-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IMoniker
	{
		// Token: 0x06004521 RID: 17697
		void GetClassID(out Guid pClassID);

		// Token: 0x06004522 RID: 17698
		[PreserveSig]
		int IsDirty();

		// Token: 0x06004523 RID: 17699
		void Load(IStream pStm);

		// Token: 0x06004524 RID: 17700
		void Save(IStream pStm, [MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

		// Token: 0x06004525 RID: 17701
		void GetSizeMax(out long pcbSize);

		// Token: 0x06004526 RID: 17702
		void BindToObject(IBindCtx pbc, IMoniker pmkToLeft, [In] ref Guid riidResult, [MarshalAs(UnmanagedType.Interface)] out object ppvResult);

		// Token: 0x06004527 RID: 17703
		void BindToStorage(IBindCtx pbc, IMoniker pmkToLeft, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);

		// Token: 0x06004528 RID: 17704
		void Reduce(IBindCtx pbc, int dwReduceHowFar, ref IMoniker ppmkToLeft, out IMoniker ppmkReduced);

		// Token: 0x06004529 RID: 17705
		void ComposeWith(IMoniker pmkRight, [MarshalAs(UnmanagedType.Bool)] bool fOnlyIfNotGeneric, out IMoniker ppmkComposite);

		// Token: 0x0600452A RID: 17706
		void Enum([MarshalAs(UnmanagedType.Bool)] bool fForward, out IEnumMoniker ppenumMoniker);

		// Token: 0x0600452B RID: 17707
		[PreserveSig]
		int IsEqual(IMoniker pmkOtherMoniker);

		// Token: 0x0600452C RID: 17708
		void Hash(out int pdwHash);

		// Token: 0x0600452D RID: 17709
		[PreserveSig]
		int IsRunning(IBindCtx pbc, IMoniker pmkToLeft, IMoniker pmkNewlyRunning);

		// Token: 0x0600452E RID: 17710
		void GetTimeOfLastChange(IBindCtx pbc, IMoniker pmkToLeft, out FILETIME pFileTime);

		// Token: 0x0600452F RID: 17711
		void Inverse(out IMoniker ppmk);

		// Token: 0x06004530 RID: 17712
		void CommonPrefixWith(IMoniker pmkOther, out IMoniker ppmkPrefix);

		// Token: 0x06004531 RID: 17713
		void RelativePathTo(IMoniker pmkOther, out IMoniker ppmkRelPath);

		// Token: 0x06004532 RID: 17714
		void GetDisplayName(IBindCtx pbc, IMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplayName);

		// Token: 0x06004533 RID: 17715
		void ParseDisplayName(IBindCtx pbc, IMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out int pchEaten, out IMoniker ppmkOut);

		// Token: 0x06004534 RID: 17716
		[PreserveSig]
		int IsSystemMoniker(out int pdwMksys);
	}
}
