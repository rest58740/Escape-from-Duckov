using System;
using System.Reflection;
using Duckov.Rules;
using Duckov.UI;
using UnityEngine;

namespace Duckov.Options.UI
{
	// Token: 0x0200026F RID: 623
	public class RuleEntry_Float : MonoBehaviour
	{
		// Token: 0x060013A1 RID: 5025 RVA: 0x0004A0CC File Offset: 0x000482CC
		private void Awake()
		{
			SliderWithTextField sliderWithTextField = this.slider;
			sliderWithTextField.onValueChanged = (Action<float>)Delegate.Combine(sliderWithTextField.onValueChanged, new Action<float>(this.OnValueChanged));
			GameRulesManager.OnRuleChanged += this.OnRuleChanged;
			Type typeFromHandle = typeof(Ruleset);
			this.field = typeFromHandle.GetField(this.fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			this.RefreshValue();
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0004A136 File Offset: 0x00048336
		private void OnRuleChanged()
		{
			this.RefreshValue();
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0004A140 File Offset: 0x00048340
		private void OnValueChanged(float value)
		{
			if (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom)
			{
				this.RefreshValue();
				return;
			}
			Ruleset ruleset = GameRulesManager.Current;
			this.SetValue(ruleset, value);
			GameRulesManager.NotifyRuleChanged();
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0004A170 File Offset: 0x00048370
		public void RefreshValue()
		{
			float value = this.GetValue(GameRulesManager.Current);
			this.slider.SetValueWithoutNotify(value);
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0004A195 File Offset: 0x00048395
		protected void SetValue(Ruleset ruleset, float value)
		{
			this.field.SetValue(ruleset, value);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0004A1A9 File Offset: 0x000483A9
		protected float GetValue(Ruleset ruleset)
		{
			return (float)this.field.GetValue(ruleset);
		}

		// Token: 0x04000EBC RID: 3772
		[SerializeField]
		private SliderWithTextField slider;

		// Token: 0x04000EBD RID: 3773
		[SerializeField]
		private string fieldName = "damageFactor_ToPlayer";

		// Token: 0x04000EBE RID: 3774
		private FieldInfo field;
	}
}
