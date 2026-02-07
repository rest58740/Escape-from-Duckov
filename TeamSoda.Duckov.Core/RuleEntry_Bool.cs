using System;
using System.Reflection;
using Duckov.Rules;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class RuleEntry_Bool : OptionsProviderBase
{
	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x0003C0ED File Offset: 0x0003A2ED
	public override string Key
	{
		get
		{
			return this.fieldName;
		}
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x0003C0F5 File Offset: 0x0003A2F5
	private void Awake()
	{
		this.field = typeof(Ruleset).GetField(this.fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x0003C114 File Offset: 0x0003A314
	public override string GetCurrentOption()
	{
		Ruleset obj = GameRulesManager.Current;
		if ((bool)this.field.GetValue(obj))
		{
			return LocalizationManager.ToPlainText("Options_On");
		}
		return LocalizationManager.ToPlainText("Options_Off");
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x0003C14F File Offset: 0x0003A34F
	public override string[] GetOptions()
	{
		return new string[]
		{
			LocalizationManager.ToPlainText("Options_Off"),
			LocalizationManager.ToPlainText("Options_On")
		};
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x0003C174 File Offset: 0x0003A374
	public override void Set(int index)
	{
		bool flag = index > 0;
		Ruleset obj = GameRulesManager.Current;
		this.field.SetValue(obj, flag);
	}

	// Token: 0x04000C45 RID: 3141
	[SerializeField]
	private string fieldName;

	// Token: 0x04000C46 RID: 3142
	private FieldInfo field;
}
