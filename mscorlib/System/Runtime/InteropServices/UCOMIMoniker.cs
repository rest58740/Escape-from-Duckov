using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200075C RID: 1884
	[Obsolete]
	[Guid("0000000f-0000-0000-c000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMIMoniker
	{
		// Token: 0x0600426A RID: 17002
		void GetClassID(out Guid pClassID);

		// Token: 0x0600426B RID: 17003
		[PreserveSig]
		int IsDirty();

		// Token: 0x0600426C RID: 17004
		void Load(UCOMIStream pStm);

		// Token: 0x0600426D RID: 17005
		void Save(UCOMIStream pStm, [MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

		// Token: 0x0600426E RID: 17006
		void GetSizeMax(out long pcbSize);

		// Token: 0x0600426F RID: 17007
		void BindToObject(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [In] ref Guid riidResult, [MarshalAs(UnmanagedType.Interface)] out object ppvResult);

		// Token: 0x06004270 RID: 17008
		void BindToStorage(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);

		// Token: 0x06004271 RID: 17009
		void Reduce(UCOMIBindCtx pbc, int dwReduceHowFar, ref UCOMIMoniker ppmkToLeft, out UCOMIMoniker ppmkReduced);

		// Token: 0x06004272 RID: 17010
		void ComposeWith(UCOMIMoniker pmkRight, [MarshalAs(UnmanagedType.Bool)] bool fOnlyIfNotGeneric, out UCOMIMoniker ppmkComposite);

		// Token: 0x06004273 RID: 17011
		void Enum([MarshalAs(UnmanagedType.Bool)] bool fForward, out UCOMIEnumMoniker ppenumMoniker);

		// Token: 0x06004274 RID: 17012
		void IsEqual(UCOMIMoniker pmkOtherMoniker);

		// Token: 0x06004275 RID: 17013
		void Hash(out int pdwHash);

		// Token: 0x06004276 RID: 17014
		void IsRunning(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, UCOMIMoniker pmkNewlyRunning);

		// Token: 0x06004277 RID: 17015
		void GetTimeOfLastChange(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, out FILETIME pFileTime);

		// Token: 0x06004278 RID: 17016
		void Inverse(out UCOMIMoniker ppmk);

		// Token: 0x06004279 RID: 17017
		void CommonPrefixWith(UCOMIMoniker pmkOther, out UCOMIMoniker ppmkPrefix);

		// Token: 0x0600427A RID: 17018
		void RelativePathTo(UCOMIMoniker pmkOther, out UCOMIMoniker ppmkRelPath);

		// Token: 0x0600427B RID: 17019
		void GetDisplayName(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplayName);

		// Token: 0x0600427C RID: 17020
		void ParseDisplayName(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out int pchEaten, out UCOMIMoniker ppmkOut);

		// Token: 0x0600427D RID: 17021
		void IsSystemMoniker(out int pdwMksys);
	}
}
