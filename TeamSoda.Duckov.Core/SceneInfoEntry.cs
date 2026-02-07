using System;
using Eflatun.SceneReference;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x0200012F RID: 303
[Serializable]
public class SceneInfoEntry
{
	// Token: 0x06000A0C RID: 2572 RVA: 0x0002BCFC File Offset: 0x00029EFC
	public SceneInfoEntry()
	{
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0002BD04 File Offset: 0x00029F04
	public SceneInfoEntry(string id, SceneReference sceneReference)
	{
		this.id = id;
		this.sceneReference = sceneReference;
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0002BD1A File Offset: 0x00029F1A
	public int BuildIndex
	{
		get
		{
			if (this.sceneReference.UnsafeReason != null)
			{
				return -1;
			}
			return this.sceneReference.BuildIndex;
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0002BD36 File Offset: 0x00029F36
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0002BD3E File Offset: 0x00029F3E
	public SceneReference SceneReference
	{
		get
		{
			return this.sceneReference;
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0002BD46 File Offset: 0x00029F46
	public string Description
	{
		get
		{
			return LocalizationManager.ToPlainText(this.description);
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0002BD53 File Offset: 0x00029F53
	public string DisplayName
	{
		get
		{
			if (string.IsNullOrEmpty(this.displayName))
			{
				return this.id;
			}
			return LocalizationManager.ToPlainText(this.displayName);
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0002BD74 File Offset: 0x00029F74
	public string DisplayNameRaw
	{
		get
		{
			if (string.IsNullOrEmpty(this.displayName))
			{
				return this.id;
			}
			return this.displayName;
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0002BD90 File Offset: 0x00029F90
	public bool IsLoaded
	{
		get
		{
			return this.sceneReference != null && this.sceneReference.UnsafeReason == null && this.sceneReference.LoadedScene.isLoaded;
		}
	}

	// Token: 0x040008DB RID: 2267
	[SerializeField]
	private string id;

	// Token: 0x040008DC RID: 2268
	[SerializeField]
	private SceneReference sceneReference;

	// Token: 0x040008DD RID: 2269
	[LocalizationKey("Default")]
	[SerializeField]
	private string displayName;

	// Token: 0x040008DE RID: 2270
	[LocalizationKey("Default")]
	[SerializeField]
	private string description;
}
