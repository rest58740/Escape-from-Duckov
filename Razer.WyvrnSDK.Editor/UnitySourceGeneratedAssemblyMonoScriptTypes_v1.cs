using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// Token: 0x02000002 RID: 2
[CompilerGenerated]
[EditorBrowsable(EditorBrowsableState.Never)]
[GeneratedCode("Unity.MonoScriptGenerator.MonoScriptInfoGenerator", null)]
internal class UnitySourceGeneratedAssemblyMonoScriptTypes_v1
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	[MethodImpl(256)]
	private static UnitySourceGeneratedAssemblyMonoScriptTypes_v1.MonoScriptData Get()
	{
		UnitySourceGeneratedAssemblyMonoScriptTypes_v1.MonoScriptData result = default(UnitySourceGeneratedAssemblyMonoScriptTypes_v1.MonoScriptData);
		result.FilePathsData = new byte[0];
		result.TypesData = new byte[0];
		result.TotalFiles = 0;
		result.TotalTypes = 0;
		result.IsEditorOnly = false;
		return result;
	}

	// Token: 0x02000003 RID: 3
	private struct MonoScriptData
	{
		// Token: 0x04000001 RID: 1
		public byte[] FilePathsData;

		// Token: 0x04000002 RID: 2
		public byte[] TypesData;

		// Token: 0x04000003 RID: 3
		public int TotalTypes;

		// Token: 0x04000004 RID: 4
		public int TotalFiles;

		// Token: 0x04000005 RID: 5
		public bool IsEditorOnly;
	}
}
