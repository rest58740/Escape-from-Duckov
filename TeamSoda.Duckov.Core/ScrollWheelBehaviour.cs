using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D0 RID: 464
public static class ScrollWheelBehaviour
{
	// Token: 0x06000E00 RID: 3584 RVA: 0x0003A346 File Offset: 0x00038546
	public static string GetDisplayName(ScrollWheelBehaviour.Behaviour behaviour)
	{
		return LocalizationManager.ToPlainText(string.Format("ScrollWheelBehaviour_{0}", behaviour));
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000E01 RID: 3585 RVA: 0x0003A35D File Offset: 0x0003855D
	// (set) Token: 0x06000E02 RID: 3586 RVA: 0x0003A36A File Offset: 0x0003856A
	public static ScrollWheelBehaviour.Behaviour CurrentBehaviour
	{
		get
		{
			return OptionsManager.Load<ScrollWheelBehaviour.Behaviour>("ScrollWheelBehaviour", ScrollWheelBehaviour.Behaviour.AmmoAndInteract);
		}
		set
		{
			OptionsManager.Save<ScrollWheelBehaviour.Behaviour>("ScrollWheelBehaviour", value);
		}
	}

	// Token: 0x020004F7 RID: 1271
	public enum Behaviour
	{
		// Token: 0x04001DE4 RID: 7652
		AmmoAndInteract,
		// Token: 0x04001DE5 RID: 7653
		Weapon
	}
}
