using System;
using Duckov.Options;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class ResolutionOptions : OptionsProviderBase
{
	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0003A43A File Offset: 0x0003863A
	public override string Key
	{
		get
		{
			return ResolutionSetter.Key_Resolution;
		}
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x0003A444 File Offset: 0x00038644
	public override string GetCurrentOption()
	{
		return OptionsManager.Load<DuckovResolution>(this.Key, new DuckovResolution(Screen.resolutions[Screen.resolutions.Length - 1])).ToString();
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x0003A484 File Offset: 0x00038684
	public override string[] GetOptions()
	{
		this.avaliableResolutions = ResolutionSetter.GetResolutions();
		string[] array = new string[this.avaliableResolutions.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.avaliableResolutions[i].ToString();
		}
		return array;
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x0003A4D4 File Offset: 0x000386D4
	public override void Set(int index)
	{
		if (this.avaliableResolutions == null || index >= this.avaliableResolutions.Length)
		{
			Debug.Log("设置分辨率流程错误");
			index = 0;
		}
		DuckovResolution obj = this.avaliableResolutions[index];
		OptionsManager.Save<DuckovResolution>(this.Key, obj);
	}

	// Token: 0x04000C00 RID: 3072
	private DuckovResolution[] avaliableResolutions;
}
