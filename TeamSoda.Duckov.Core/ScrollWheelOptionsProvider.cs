using System;

// Token: 0x020001CF RID: 463
public class ScrollWheelOptionsProvider : OptionsProviderBase
{
	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000DFB RID: 3579 RVA: 0x0003A2C6 File Offset: 0x000384C6
	public override string Key
	{
		get
		{
			return "Input_ScrollWheelBehaviour";
		}
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x0003A2CD File Offset: 0x000384CD
	public override string GetCurrentOption()
	{
		return ScrollWheelBehaviour.GetDisplayName(ScrollWheelBehaviour.CurrentBehaviour);
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x0003A2DC File Offset: 0x000384DC
	public override string[] GetOptions()
	{
		ScrollWheelBehaviour.Behaviour[] array = (ScrollWheelBehaviour.Behaviour[])Enum.GetValues(typeof(ScrollWheelBehaviour.Behaviour));
		string[] array2 = new string[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = ScrollWheelBehaviour.GetDisplayName(array[i]);
		}
		return array2;
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x0003A321 File Offset: 0x00038521
	public override void Set(int index)
	{
		ScrollWheelBehaviour.CurrentBehaviour = ((ScrollWheelBehaviour.Behaviour[])Enum.GetValues(typeof(ScrollWheelBehaviour.Behaviour)))[index];
	}
}
