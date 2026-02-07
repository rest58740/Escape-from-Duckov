using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class ES3Defaults : ScriptableObject
{
	// Token: 0x04000024 RID: 36
	[SerializeField]
	public ES3SerializableSettings settings = new ES3SerializableSettings();

	// Token: 0x04000025 RID: 37
	public bool addMgrToSceneAutomatically;

	// Token: 0x04000026 RID: 38
	public bool autoUpdateReferences = true;

	// Token: 0x04000027 RID: 39
	public bool addAllPrefabsToManager = true;

	// Token: 0x04000028 RID: 40
	public int collectDependenciesDepth = 4;

	// Token: 0x04000029 RID: 41
	public int collectDependenciesTimeout = 10;

	// Token: 0x0400002A RID: 42
	public bool updateReferencesWhenSceneChanges = true;

	// Token: 0x0400002B RID: 43
	public bool updateReferencesWhenSceneIsSaved = true;

	// Token: 0x0400002C RID: 44
	public bool updateReferencesWhenSceneIsOpened = true;

	// Token: 0x0400002D RID: 45
	[Tooltip("Folders listed here will be searched for references every time the reference manager is refreshed. Path should be relative to the project folder.")]
	public string[] referenceFolders = new string[0];

	// Token: 0x0400002E RID: 46
	public bool logDebugInfo;

	// Token: 0x0400002F RID: 47
	public bool logWarnings = true;

	// Token: 0x04000030 RID: 48
	public bool logErrors = true;
}
