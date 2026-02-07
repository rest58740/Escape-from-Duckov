using System;
using Duckov.Options;

// Token: 0x020001D3 RID: 467
public class FullScreenOptions : OptionsProviderBase
{
	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000E0D RID: 3597 RVA: 0x0003A403 File Offset: 0x00038603
	public override string Key
	{
		get
		{
			return ResolutionSetter.Key_ScreenMode;
		}
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x0003A40A File Offset: 0x0003860A
	public override string GetCurrentOption()
	{
		return ResolutionSetter.ScreenModeToName(OptionsManager.Load<ResolutionSetter.screenModes>(this.Key, ResolutionSetter.screenModes.Borderless));
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x0003A41D File Offset: 0x0003861D
	public override string[] GetOptions()
	{
		return ResolutionSetter.GetScreenModes();
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x0003A424 File Offset: 0x00038624
	public override void Set(int index)
	{
		OptionsManager.Save<ResolutionSetter.screenModes>(this.Key, (ResolutionSetter.screenModes)index);
	}
}
