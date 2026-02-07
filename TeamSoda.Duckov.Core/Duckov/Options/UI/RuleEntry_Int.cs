using System;
using System.Reflection;
using Duckov.Rules;
using Duckov.UI;
using UnityEngine;

namespace Duckov.Options.UI
{
	// Token: 0x02000270 RID: 624
	public class RuleEntry_Int : MonoBehaviour
	{
		// Token: 0x060013A8 RID: 5032 RVA: 0x0004A1D0 File Offset: 0x000483D0
		private void Awake()
		{
			SliderWithTextField sliderWithTextField = this.slider;
			sliderWithTextField.onValueChanged = (Action<float>)Delegate.Combine(sliderWithTextField.onValueChanged, new Action<float>(this.OnValueChanged));
			GameRulesManager.OnRuleChanged += this.OnRuleChanged;
			Type typeFromHandle = typeof(Ruleset);
			this.field = typeFromHandle.GetField(this.fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			this.RefreshValue();
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x0004A23A File Offset: 0x0004843A
		private void OnRuleChanged()
		{
			this.RefreshValue();
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0004A244 File Offset: 0x00048444
		private void OnValueChanged(float value)
		{
			if (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom)
			{
				this.RefreshValue();
				return;
			}
			Ruleset ruleset = GameRulesManager.Current;
			this.SetValue(ruleset, (int)value);
			GameRulesManager.NotifyRuleChanged();
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x0004A274 File Offset: 0x00048474
		public void RefreshValue()
		{
			float valueWithoutNotify = (float)this.GetValue(GameRulesManager.Current);
			this.slider.SetValueWithoutNotify(valueWithoutNotify);
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0004A29A File Offset: 0x0004849A
		protected void SetValue(Ruleset ruleset, int value)
		{
			this.field.SetValue(ruleset, value);
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0004A2AE File Offset: 0x000484AE
		protected int GetValue(Ruleset ruleset)
		{
			return (int)this.field.GetValue(ruleset);
		}

		// Token: 0x04000EBF RID: 3775
		[SerializeField]
		private SliderWithTextField slider;

		// Token: 0x04000EC0 RID: 3776
		[SerializeField]
		private string fieldName = "damageFactor_ToPlayer";

		// Token: 0x04000EC1 RID: 3777
		private FieldInfo field;
	}
}
