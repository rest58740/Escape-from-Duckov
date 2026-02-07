using System;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class TagUtilities
{
	// Token: 0x06000558 RID: 1368 RVA: 0x00018444 File Offset: 0x00016644
	public static Tag TagFromString(string name)
	{
		name = name.Trim();
		Tag tag = GameplayDataSettings.Tags.AllTags.FirstOrDefault((Tag e) => e != null && e.name == name);
		if (tag == null)
		{
			Debug.LogError("未找到Tag: " + name);
		}
		return tag;
	}
}
